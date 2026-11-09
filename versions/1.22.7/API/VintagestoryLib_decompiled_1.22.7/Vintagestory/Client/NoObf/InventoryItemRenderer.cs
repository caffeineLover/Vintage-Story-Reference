using System;
using System.Collections.Generic;
using Cairo;
using OpenTK.Graphics.OpenGL;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.Client.NoObf;

public class InventoryItemRenderer : IRenderer, IDisposable
{
	private ClientMain game;

	private Dictionary<string, LoadedTexture> StackSizeTextures = new Dictionary<string, LoadedTexture>();

	private MeshRef quadModelRef;

	private CairoFont stackSizeFont;

	private Matrixf modelMat = new Matrixf();

	private Queue<AtlasRenderTask> queue = new Queue<AtlasRenderTask>();

	private Dictionary<int, Queue<AtlasRenderTask>> perAtlasQueues = new Dictionary<int, Queue<AtlasRenderTask>>();

	private Queue<AtlasRenderTask> taskCompletedQueue = new Queue<AtlasRenderTask>();

	private int[] clearPixels;

	public const float ForceNoTransform = -1.0125f;

	public double RenderOrder => 9.0;

	public int RenderRange => 24;

	public InventoryItemRenderer(ClientMain game)
	{
		this.game = game;
		MeshData modeldata = QuadMeshUtil.GetQuad();
		modeldata.Uv = new float[8] { 1f, 1f, 0f, 1f, 0f, 0f, 1f, 0f };
		modeldata.Rgba = new byte[16];
		modeldata.Rgba.Fill(byte.MaxValue);
		quadModelRef = game.Platform.UploadMesh(modeldata);
		stackSizeFont = CairoFont.WhiteSmallText().WithFontSize((float)GuiStyle.SmallishFontSize);
		stackSizeFont.FontWeight = FontWeight.Bold;
		stackSizeFont.Color = new double[4] { 1.0, 1.0, 1.0, 1.0 };
		stackSizeFont.StrokeColor = new double[4] { 0.0, 0.0, 0.0, 1.0 };
		stackSizeFont.StrokeWidth = (double)ClientSettings.GUIScale + 0.25;
		ClientSettings.Inst.AddWatcher<float>("guiScale", delegate
		{
			stackSizeFont.StrokeWidth = (double)ClientSettings.GUIScale + 0.25;
			foreach (KeyValuePair<string, LoadedTexture> stackSizeTexture in StackSizeTextures)
			{
				stackSizeTexture.Value?.Dispose();
			}
			StackSizeTextures.Clear();
		});
		game.eventManager.RegisterRenderer(this, EnumRenderStage.Ortho, "renderstacktoatlas");
	}

	public bool RenderItemStackToAtlas(ItemStack stack, ITextureAtlasAPI atlas, int size, Action<int> onComplete, int color = -1, float sepiaLevel = 0f, float scale = 1f)
	{
		AtlasRenderTask task = new AtlasRenderTask
		{
			Stack = stack,
			Atlas = atlas,
			Size = size,
			Color = color,
			SepiaLevel = sepiaLevel,
			OnComplete = onComplete,
			Scale = scale
		};
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			game.EnqueueMainThreadTask(delegate
			{
				queue.Enqueue(task);
			}, "enqueueRenderTask");
			return false;
		}
		if (game.currentRenderStage != EnumRenderStage.Ortho)
		{
			queue.Enqueue(task);
			return false;
		}
		int atlasTextureId = CreatePositionInAtlas(atlas, task);
		FrameBufferRef fb = CreateFrameBuffer(atlas.Size, atlasTextureId);
		RenderItemStackToFrameBuffer(atlas.Size, task, new DummySlot(stack));
		DisposeFrameBuffer(fb);
		onComplete(task.SubId);
		return true;
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		ITextureAtlasAPI atlas = null;
		int maxCount = 512;
		while (queue.Count > 0 && maxCount-- > 0)
		{
			AtlasRenderTask task = queue.Dequeue();
			atlas = task.Atlas;
			int atlasTextureId = CreatePositionInAtlas(atlas, task);
			if (!perAtlasQueues.TryGetValue(atlasTextureId, out var atlasQueue))
			{
				atlasQueue = new Queue<AtlasRenderTask>();
				perAtlasQueues[atlasTextureId] = atlasQueue;
			}
			atlasQueue.Enqueue(task);
			taskCompletedQueue.Enqueue(task);
		}
		if (atlas == null)
		{
			return;
		}
		DummySlot dummySlot = new DummySlot();
		foreach (KeyValuePair<int, Queue<AtlasRenderTask>> atlasIdAndQueue in perAtlasQueues)
		{
			Queue<AtlasRenderTask> itemQueue = atlasIdAndQueue.Value;
			int atlasTextureId2 = atlasIdAndQueue.Key;
			FrameBufferRef fb = CreateFrameBuffer(atlas.Size, atlasTextureId2);
			while (itemQueue.Count > 0)
			{
				AtlasRenderTask itemToRender = itemQueue.Dequeue();
				dummySlot.Set(itemToRender.Stack);
				game.Platform.BindTexture2d(atlasTextureId2);
				RenderItemStackToFrameBuffer(atlas.Size, itemToRender, dummySlot);
			}
			DisposeFrameBuffer(fb);
		}
		while (taskCompletedQueue.Count > 0)
		{
			AtlasRenderTask task2 = taskCompletedQueue.Dequeue();
			task2.OnComplete(task2.SubId);
		}
	}

	private FrameBufferRef CreateFrameBuffer(Size2i atlasSize, int atlasTextureId)
	{
		FramebufferAttrsAttachment[] attachments = new FramebufferAttrsAttachment[2]
		{
			new FramebufferAttrsAttachment
			{
				AttachmentType = EnumFramebufferAttachment.ColorAttachment0,
				Texture = new RawTexture
				{
					Width = atlasSize.Width,
					Height = atlasSize.Height,
					TextureId = atlasTextureId,
					PixelFormat = EnumTexturePixelFormat.Rgba,
					PixelInternalFormat = EnumTextureInternalFormat.Rgba8
				}
			},
			new FramebufferAttrsAttachment
			{
				AttachmentType = EnumFramebufferAttachment.DepthAttachment,
				Texture = new RawTexture
				{
					Width = atlasSize.Width,
					Height = atlasSize.Height,
					PixelFormat = EnumTexturePixelFormat.DepthComponent,
					PixelInternalFormat = EnumTextureInternalFormat.DepthComponent32
				}
			}
		};
		FrameBufferRef fb = game.Platform.CreateFramebuffer(new FramebufferAttrs("atlasRenderer", atlasSize.Width, atlasSize.Height)
		{
			Attachments = attachments
		});
		game.Platform.CurrentFrameBuffer = fb;
		game.Platform.GlEnableDepthTest();
		game.Platform.GlDisableCullFace();
		game.Platform.GlToggleBlend(on: true);
		game.Platform.ClearFrameBuffer(fb, null, clearDepthBuffer: true, clearColorBuffers: false);
		game.OrthoMode(atlasSize.Width, atlasSize.Height, inverseY: true);
		game.Platform.BindTexture2d(atlasTextureId);
		return fb;
	}

	public void RenderItemStackToFrameBuffer(Size2i atlasSize, AtlasRenderTask task, DummySlot dummySlot)
	{
		float x = task.TexPos.x1 * (float)atlasSize.Width;
		float y = task.TexPos.y1 * (float)atlasSize.Height;
		int size = task.Size;
		if (clearPixels == null || clearPixels.Length < size * size)
		{
			clearPixels = new int[size * size];
		}
		game.guiShaderProg.SepiaLevel = task.SepiaLevel;
		GL.TexSubImage2D<int>((TextureTarget)3553, 0, (int)x, (int)y, size, size, (PixelFormat)32993, (PixelType)5121, clearPixels);
		game.api.renderapi.inventoryItemRenderer.RenderItemstackToGui(dummySlot, x + (float)size / 2f, y + (float)size / 2f, 500.0, (float)(size / 2) * task.Scale, task.Color, shading: true, origRotate: false, showStackSize: false);
	}

	public void DisposeFrameBuffer(FrameBufferRef fb)
	{
		game.PerspectiveMode();
		game.guiShaderProg.SepiaLevel = 0f;
		game.Platform.LoadFrameBuffer(EnumFrameBuffer.Default);
		fb.ColorTextureIds = Array.Empty<int>();
		game.Platform.DisposeFrameBuffer(fb);
	}

	private int CreatePositionInAtlas(ITextureAtlasAPI atlas, AtlasRenderTask task)
	{
		if (!atlas.AllocateTextureSpace(task.Size, task.Size, out var subid, out var texPos))
		{
			throw new Exception(string.Format("Was not able to allocate texture space of size {0}x{0} for item stack '{1}', maybe atlas is full?", task.Size, task.Stack.GetName()));
		}
		task.SubId = subid;
		task.TexPos = texPos;
		return texPos.atlasTextureId;
	}

	public void RenderEntityToGui(float dt, Entity entity, double posX, double posY, double posZ, float yawDelta, float size, int color)
	{
		game.guiShaderProg.RgbaIn = new Vec4f(1f, 1f, 1f, 1f);
		game.guiShaderProg.ExtraGlow = 0;
		game.guiShaderProg.ApplyColor = 1;
		game.guiShaderProg.Tex2d2D = game.EntityAtlasManager.AtlasTextures[0].TextureId;
		game.guiShaderProg.AlphaTest = 0.1f;
		game.guiShaderProg.NoTexture = 0f;
		game.guiShaderProg.OverlayOpacity = 0f;
		game.guiShaderProg.NormalShaded = 1;
		entity.Properties.Client.Renderer.RenderToGui(dt, posX, posY, posZ, yawDelta, size);
		game.guiShaderProg.NormalShaded = 0;
	}

	public void RenderItemstackToGui(ItemSlot inSlot, double posX, double posY, double posZ, float size, int color, bool shading = true, bool origRotate = false, bool showStackSize = true)
	{
		RenderItemstackToGui(inSlot, posX, posY, posZ, size, color, 0f, shading, origRotate, showStackSize);
	}

	public void RenderItemstackToGui(ItemSlot inSlot, double posX, double posY, double posZ, float size, int color, float dt, bool shading = true, bool origRotate = false, bool showStackSize = true)
	{
		try
		{
			ItemStack itemstack = inSlot.Itemstack;
			ItemRenderInfo renderInfo = GetItemStackRenderInfo(game, inSlot, EnumItemRenderTarget.Gui, dt);
			if (renderInfo.ModelRef == null)
			{
				return;
			}
			bool noTransform = dt == -1.0125f;
			itemstack.Collectible.InGuiIdle(game, itemstack);
			ModelTransform transform = renderInfo.Transform;
			if (transform == null)
			{
				return;
			}
			bool upsidedown = itemstack.Class == EnumItemClass.Block;
			bool rotate = origRotate && renderInfo.Transform.Rotate;
			Matrixf modelMat = this.modelMat;
			modelMat.Identity();
			modelMat.Translate((int)posX - ((itemstack.Class == EnumItemClass.Item) ? 3 : 0), (int)posY - ((itemstack.Class == EnumItemClass.Item) ? 1 : 0), (float)posZ);
			if (noTransform)
			{
				modelMat.Scale(size * transform.ScaleXYZ.X, size * transform.ScaleXYZ.Y, size * transform.ScaleXYZ.Z);
				if (upsidedown)
				{
					modelMat.Scale(1f, -1f, 1f);
				}
			}
			else
			{
				modelMat.Translate((double)transform.Origin.X + GuiElement.scaled(transform.Translation.X), (double)transform.Origin.Y + GuiElement.scaled(transform.Translation.Y), (double)(transform.Origin.Z * size) + GuiElement.scaled(transform.Translation.Z));
				modelMat.Scale(size * transform.ScaleXYZ.X, size * transform.ScaleXYZ.Y, size * transform.ScaleXYZ.Z);
				modelMat.RotateXDeg(transform.Rotation.X + (upsidedown ? 180f : 0f));
				modelMat.RotateYDeg(transform.Rotation.Y - (float)((!upsidedown) ? 1 : (-1)) * (rotate ? ((float)game.Platform.EllapsedMs / 50f) : 0f));
				modelMat.RotateZDeg(transform.Rotation.Z);
				modelMat.Translate(0f - transform.Origin.X, 0f - transform.Origin.Y, 0f - transform.Origin.Z);
			}
			int num = (int)itemstack.Collectible.GetTemperature(game, itemstack);
			float[] glowColor = ColorUtil.GetIncandescenceColorAsColor4f(num);
			float[] drawcolor = ColorUtil.ToRGBAFloats(color);
			int extraGlow = GameMath.Clamp((num - 550) / 2, 0, 255);
			bool tempGlowMode = itemstack.Attributes.HasAttribute("temperature");
			ShaderProgramGui guiShaderProg = game.guiShaderProg;
			guiShaderProg.NormalShaded = ((renderInfo.NormalShaded && shading) ? 1 : 0);
			guiShaderProg.RgbaIn = new Vec4f(drawcolor[0], drawcolor[1], drawcolor[2], drawcolor[3]);
			guiShaderProg.ExtraGlow = extraGlow;
			guiShaderProg.TempGlowMode = (tempGlowMode ? 1 : 0);
			guiShaderProg.RgbaGlowIn = (tempGlowMode ? new Vec4f(glowColor[0], glowColor[1], glowColor[2], (float)extraGlow / 255f) : new Vec4f(1f, 1f, 1f, (float)extraGlow / 255f));
			guiShaderProg.ApplyColor = (renderInfo.ApplyColor ? 1 : 0);
			guiShaderProg.AlphaTest = renderInfo.AlphaTest;
			guiShaderProg.OverlayOpacity = renderInfo.OverlayOpacity;
			if (renderInfo.OverlayTexture != null && renderInfo.OverlayOpacity > 0f)
			{
				guiShaderProg.Tex2dOverlay2D = renderInfo.OverlayTexture.TextureId;
				guiShaderProg.Uniform("overlayTextureSize", renderInfo.OverlayTexture.Width, renderInfo.OverlayTexture.Height);
				guiShaderProg.Uniform("baseTextureSize", renderInfo.TextureSize.Width, renderInfo.TextureSize.Height);
				TextureAtlasPosition texPos = GetTextureAtlasPosition(game, itemstack);
				guiShaderProg.Uniform("baseUvOrigin", texPos.x1, texPos.y1);
			}
			guiShaderProg.ModelMatrix = modelMat.Values;
			guiShaderProg.ProjectionMatrix = game.CurrentProjectionMatrix;
			guiShaderProg.ModelViewMatrix = modelMat.ReverseMul(game.CurrentModelViewMatrix).Values;
			guiShaderProg.ApplyModelMat = 1;
			if (game.api.eventapi.itemStackRenderersByTarget[(int)itemstack.Collectible.ItemClass][0].TryGetValue(itemstack.Collectible.Id, out var renderer))
			{
				renderer(inSlot, renderInfo, modelMat, posX, posY, posZ, size, color, origRotate, showStackSize);
				guiShaderProg.ApplyModelMat = 0;
				guiShaderProg.NormalShaded = 0;
				guiShaderProg.RgbaGlowIn = new Vec4f(0f, 0f, 0f, 0f);
				guiShaderProg.AlphaTest = 0f;
				return;
			}
			guiShaderProg.DamageEffect = renderInfo.DamageEffect;
			game.api.renderapi.RenderMultiTextureMesh(renderInfo.ModelRef, "tex2d");
			guiShaderProg.ApplyModelMat = 0;
			guiShaderProg.NormalShaded = 0;
			guiShaderProg.TempGlowMode = 0;
			guiShaderProg.DamageEffect = 0f;
			LoadedTexture stackSizeTexture = null;
			if (itemstack.StackSize != 1 && showStackSize)
			{
				float mul = size / (float)GuiElement.scaled(25.600000381469727);
				string key = itemstack.StackSize + "-" + (int)(mul * 100f);
				if (!StackSizeTextures.TryGetValue(key, out stackSizeTexture))
				{
					stackSizeTexture = (StackSizeTextures[key] = GenStackSizeTexture(itemstack.StackSize, mul));
				}
			}
			if (stackSizeTexture != null)
			{
				float mul2 = size / (float)GuiElement.scaled(25.600000381469727);
				game.Platform.GlToggleBlend(on: true, EnumBlendMode.PremultipliedAlpha);
				game.Render2DLoadedTexture(stackSizeTexture, (int)(posX + (double)size + 1.0 - (double)stackSizeTexture.Width), (int)(posY + (double)mul2 * GuiElement.scaled(3.0) - GuiElement.scaled(4.0)), (int)posZ + 100);
				game.Platform.GlToggleBlend(on: true);
			}
			guiShaderProg.AlphaTest = 0f;
			guiShaderProg.RgbaGlowIn = new Vec4f(0f, 0f, 0f, 0f);
		}
		catch (Exception innerException)
		{
			throw new Exception("Error while rendering item in slot " + inSlot, innerException);
		}
	}

	private LoadedTexture GenStackSizeTexture(int stackSize, float fontSizeMultiplier = 1f)
	{
		CairoFont font = stackSizeFont.Clone();
		font.UnscaledFontsize *= fontSizeMultiplier;
		return game.api.guiapi.TextTexture.GenTextTexture(stackSize.ToString() ?? "", font);
	}

	public static ItemRenderInfo GetItemStackRenderInfo(ClientMain game, ItemSlot inSlot, EnumItemRenderTarget target, float dt)
	{
		ItemStack itemstack = inSlot.Itemstack;
		if (itemstack == null || itemstack.Collectible.Code == null)
		{
			return new ItemRenderInfo();
		}
		ItemRenderInfo renderInfo = new ItemRenderInfo();
		renderInfo.dt = dt;
		switch (target)
		{
		case EnumItemRenderTarget.Ground:
			renderInfo.Transform = itemstack.Collectible.GroundTransform;
			break;
		case EnumItemRenderTarget.Gui:
			renderInfo.Transform = itemstack.Collectible.GuiTransform;
			break;
		case EnumItemRenderTarget.HandTp:
			renderInfo.Transform = itemstack.Collectible.TpHandTransform;
			break;
		case EnumItemRenderTarget.HandTpOff:
			renderInfo.Transform = itemstack.Collectible.TpOffHandTransform ?? itemstack.Collectible.TpHandTransform;
			break;
		}
		if (itemstack.Collectible?.Code == null)
		{
			renderInfo.ModelRef = ((itemstack.Block == null) ? game.TesselatorManager.unknownItemModelRef : game.TesselatorManager.unknownBlockModelRef);
		}
		else if (itemstack.Class == EnumItemClass.Block)
		{
			renderInfo.ModelRef = game.TesselatorManager.blockModelRefsInventory[itemstack.Id];
		}
		else
		{
			int variant = (itemstack.TempAttributes.HasAttribute("renderVariant") ? itemstack.TempAttributes.GetInt("renderVariant") : itemstack.Attributes.GetInt("renderVariant"));
			if (variant != 0 && (variant < 0 || game.TesselatorManager.altItemModelRefsInventory[itemstack.Id] == null || game.TesselatorManager.altItemModelRefsInventory[itemstack.Id].Length < variant - 1))
			{
				game.Logger.Warning("Itemstack {0} has an invalid renderVariant {1}. No such model variant exists. Will reset to 0", itemstack.GetName(), variant);
				itemstack.TempAttributes.SetInt("renderVariant", 0);
				variant = 0;
			}
			if (variant == 0)
			{
				renderInfo.ModelRef = game.TesselatorManager.itemModelRefsInventory[itemstack.Id];
			}
			else
			{
				renderInfo.ModelRef = game.TesselatorManager.altItemModelRefsInventory[itemstack.Id][variant - 1];
			}
		}
		ItemRenderInfo itemRenderInfo = renderInfo;
		int normalShaded;
		if (itemstack.Class != EnumItemClass.Block)
		{
			CompositeShape shape = itemstack.Item.Shape;
			normalShaded = ((shape != null && !shape.VoxelizeTexture) ? 1 : 0);
		}
		else
		{
			CompositeShape shape2 = itemstack.Block.Shape;
			normalShaded = ((shape2 != null && !shape2.VoxelizeTexture) ? 1 : 0);
		}
		itemRenderInfo.NormalShaded = (byte)normalShaded != 0;
		renderInfo.TextureSize.Width = ((itemstack.Class == EnumItemClass.Block) ? game.BlockAtlasManager.Size.Width : game.ItemAtlasManager.Size.Width);
		renderInfo.TextureSize.Height = ((itemstack.Class == EnumItemClass.Block) ? game.BlockAtlasManager.Size.Height : game.ItemAtlasManager.Size.Height);
		renderInfo.HalfTransparent = itemstack.Block != null && (itemstack.Block.RenderPass == EnumChunkRenderPass.Meta || itemstack.Block.RenderPass == EnumChunkRenderPass.Transparent);
		renderInfo.AlphaTest = itemstack.Collectible.RenderAlphaTest;
		renderInfo.CullFaces = itemstack.Block != null && (itemstack.Block.RenderPass == EnumChunkRenderPass.Opaque || itemstack.Block.RenderPass == EnumChunkRenderPass.TopSoil);
		renderInfo.ApplyColor = renderInfo.NormalShaded;
		TransitionState state = itemstack.Collectible.UpdateAndGetTransitionState(game, inSlot, EnumTransitionType.Perish);
		if (state != null && state.TransitionLevel > 0f)
		{
			renderInfo.SetRotOverlay(game.api, state.TransitionLevel);
		}
		renderInfo.InSlot = inSlot;
		inSlot.OnBeforeRender(renderInfo);
		itemstack.Collectible.OnBeforeRender(game.api, itemstack, target, ref renderInfo);
		return renderInfo;
	}

	public static TextureAtlasPosition GetTextureAtlasPosition(ClientMain game, IItemStack itemstack)
	{
		int tileSide = BlockFacing.UP.Index;
		if (itemstack.Collectible.Code == null)
		{
			return game.BlockAtlasManager.UnknownTexturePos;
		}
		if (itemstack.Class == EnumItemClass.Block)
		{
			int textureSubId = game.FastBlockTextureSubidsByBlockAndFace[itemstack.Id][tileSide];
			return game.BlockAtlasManager.TextureAtlasPositionsByTextureSubId[textureSubId];
		}
		if (itemstack.Item.FirstTexture == null)
		{
			return game.BlockAtlasManager.UnknownTexturePos;
		}
		int textureSubId2 = itemstack.Item.FirstTexture.Baked.TextureSubId;
		return game.ItemAtlasManager.TextureAtlasPositionsByTextureSubId[textureSubId2];
	}

	public int GetCurrentBlockOrItemTextureId(int side)
	{
		ItemSlot slot = game.player.inventoryMgr.ActiveHotbarSlot;
		if (slot != null && slot.Itemstack.Class == EnumItemClass.Block)
		{
			return game.FastBlockTextureSubidsByBlockAndFace[slot.Itemstack.Id][side];
		}
		return 0;
	}

	public int GetBlockOrItemTextureId(BlockFacing facing, IItemStack itemstack)
	{
		if (itemstack.Class != EnumItemClass.Block)
		{
			return 0;
		}
		return game.FastBlockTextureSubidsByBlockAndFace[itemstack.Id][facing.Index];
	}

	public void Dispose()
	{
		quadModelRef?.Dispose();
		foreach (KeyValuePair<string, LoadedTexture> stackSizeTexture in StackSizeTextures)
		{
			stackSizeTexture.Value?.Dispose();
		}
	}
}
