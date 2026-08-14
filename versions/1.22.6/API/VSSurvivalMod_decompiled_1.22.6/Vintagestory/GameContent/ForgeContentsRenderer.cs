using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class ForgeContentsRenderer : IRenderer, IDisposable, ITexPositionSource
{
	protected ICoreClientAPI capi;

	protected Block block;

	protected BlockPos pos;

	protected MultiTextureMeshRef workItemMeshRef;

	protected MultiTextureMeshRef coalMeshRef;

	protected ItemStack stack;

	protected float fuelLevel;

	protected bool burning;

	protected string tmpMetal;

	protected ITexPositionSource tmpTextureSource;

	protected Matrixf ModelMat = new Matrixf();

	private float extraOxygenRate;

	private float targetExtraOxygenRate;

	protected Vec3f rotationRad;

	public double RenderOrder => 0.5;

	public int RenderRange => 24;

	public Size2i AtlasSize => capi.BlockTextureAtlas.Size;

	public TextureAtlasPosition this[string textureCode] => tmpTextureSource[tmpMetal];

	public ForgeContentsRenderer(Block block, BlockPos pos, ICoreClientAPI capi, Vec3f rotationRad)
	{
		this.pos = pos;
		this.block = block;
		this.capi = capi;
		this.rotationRad = rotationRad;
	}

	public void SetContents(ItemStack stack, float fuelLevel, bool burning, bool regen, float extraOxygenRate)
	{
		this.stack = stack;
		this.fuelLevel = fuelLevel;
		this.extraOxygenRate = extraOxygenRate;
		if (fuelLevel <= 0f)
		{
			coalMeshRef?.Dispose();
			coalMeshRef = null;
		}
		else if (this.burning != burning || coalMeshRef == null)
		{
			RegenCoalMesh(burning);
		}
		this.burning = burning;
		if (regen)
		{
			RegenMesh();
		}
	}

	private void RegenCoalMesh(bool burning)
	{
		coalMeshRef?.Dispose();
		if (block.Attributes["coalshapeloc"] == null)
		{
			capi.Logger.Warning("Block {0} does not have coalshapeloc set? Forge contents will be invisible.", block.Code);
			return;
		}
		AssetLocation shapeloc = AssetLocation.Create(block.Attributes["coalshapeloc"].ToString(), block.Code.Domain).WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/");
		Shape coalshape = capi.Assets.TryGet(shapeloc)?.ToObject<Shape>();
		if (coalshape == null)
		{
			capi.Logger.Warning("Forge contents coal shape {0} not found", shapeloc);
			return;
		}
		coalshape.Textures["coal"] = block.Textures[burning ? "fuel-ember" : "fuel-coal"].Base;
		capi.Tesselator.TesselateShape("forge coal", coalshape, out var meshdata, new ShapeTextureSource(capi, coalshape, shapeloc), null, 0, 0, 0);
		meshdata.Rotate(new Vec3f(0.5f, 0f, 0.5f), rotationRad.X, rotationRad.Y, rotationRad.Z);
		coalMeshRef = capi.Render.UploadMultiTextureMesh(meshdata);
	}

	public void RegenMesh()
	{
		workItemMeshRef?.Dispose();
		workItemMeshRef = null;
		if (stack == null)
		{
			return;
		}
		tmpMetal = stack.Collectible.LastCodePart();
		MeshData mesh = null;
		string firstCodePart = stack.Collectible.FirstCodePart();
		switch (firstCodePart)
		{
		case "metalplate":
		{
			tmpTextureSource = capi.Tesselator.GetTextureSource(capi.World.GetBlock(new AssetLocation("platepile")));
			Shape shape = Shape.TryGet(capi, "shapes/block/stone/forge/platepile.json");
			capi.Tesselator.TesselateShape("block-fcr", shape, out mesh, this, null, 0, 0, 0, stack.StackSize);
			break;
		}
		case "ironbloom":
			if (stack.Attributes.HasAttribute("voxels"))
			{
				goto case "workitem";
			}
			goto default;
		case "workitem":
		{
			MeshData workItemMesh = ItemWorkItem.GenMesh(capi, stack, ItemWorkItem.GetVoxels(stack));
			if (workItemMesh != null)
			{
				workItemMesh.Scale(0.9f, 0.9f, 0.9f);
				workItemMesh.Translate(0f, -0.5625f, 0f);
				workItemMesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), rotationRad.X, rotationRad.Y, rotationRad.Z);
				workItemMeshRef = capi.Render.UploadMultiTextureMesh(workItemMesh);
			}
			break;
		}
		default:
		{
			if (firstCodePart == "ingot")
			{
				tmpTextureSource = capi.Tesselator.GetTextureSource(capi.World.GetBlock(new AssetLocation("ingotpile")));
				Shape shape = Shape.TryGet(capi, "shapes/block/stone/forge/ingotpile.json");
				capi.Tesselator.TesselateShape("block-fcr", shape, out mesh, this, null, 0, 0, 0, stack.StackSize);
				break;
			}
			JsonObject attributes = stack.Collectible.Attributes;
			if (attributes != null && attributes.IsTrue("forgable"))
			{
				if (stack.Class == EnumItemClass.Block)
				{
					mesh = capi.TesselatorManager.GetDefaultBlockMesh(stack.Block).Clone();
				}
				else
				{
					capi.Tesselator.TesselateItem(stack.Item, out mesh);
				}
				ModelTransform tf = stack.Collectible.Attributes["inForgeTransform"].AsObject<ModelTransform>();
				if (tf != null)
				{
					tf.EnsureDefaultValues();
					mesh.ModelTransform(tf);
				}
			}
			break;
		}
		}
		if (mesh != null)
		{
			mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), rotationRad.X, rotationRad.Y, rotationRad.Z);
			workItemMeshRef = capi.Render.UploadMultiTextureMesh(mesh);
		}
	}

	public void OnRenderFrame(float dt, EnumRenderStage stage)
	{
		if (stack == null && fuelLevel == 0f)
		{
			return;
		}
		IRenderAPI rpi = capi.Render;
		Vec3d camPos = capi.World.Player.Entity.CameraPos;
		rpi.GlDisableCullFace();
		if (stack != null && workItemMeshRef != null)
		{
			int num = (int)stack.Collectible.GetTemperature(capi.World, stack);
			Vec4f lightrgbs = capi.World.BlockAccessor.GetLightRGBs(pos.X, pos.Y, pos.Z);
			float[] glowColor = ColorUtil.GetIncandescenceColorAsColor4f(num);
			int extraGlow = GameMath.Clamp((num - 550) / 2, 0, 255);
			Vec4f glowRgb = new Vec4f();
			glowRgb.R = glowColor[0];
			glowRgb.G = glowColor[1];
			glowRgb.B = glowColor[2];
			glowRgb.A = (float)extraGlow / 255f;
			IShaderProgram smithingWorkItemShader = capi.ModLoader.GetModSystem<SurvivalCoreSystem>().smithingWorkItemShader;
			smithingWorkItemShader.Use();
			smithingWorkItemShader.Uniform("rgbaAmbientIn", rpi.AmbientColor);
			smithingWorkItemShader.Uniform("rgbaFogIn", rpi.FogColor);
			smithingWorkItemShader.Uniform("fogMinIn", rpi.FogMin);
			smithingWorkItemShader.Uniform("dontWarpVertices", 0);
			smithingWorkItemShader.Uniform("addRenderFlags", 0);
			smithingWorkItemShader.Uniform("fogDensityIn", rpi.FogDensity);
			smithingWorkItemShader.Uniform("rgbaTint", ColorUtil.WhiteArgbVec);
			smithingWorkItemShader.Uniform("rgbaLightIn", lightrgbs);
			smithingWorkItemShader.Uniform("rgbaGlowIn", glowRgb);
			smithingWorkItemShader.Uniform("extraGlow", extraGlow);
			smithingWorkItemShader.Uniform("tempGlowMode", 0);
			smithingWorkItemShader.UniformMatrix("modelMatrix", ModelMat.Identity().Translate((double)pos.X - camPos.X, (double)pos.Y - camPos.Y + 0.6875 + (double)((fuelLevel - 1f) / 16f / 4f), (double)pos.Z - camPos.Z).Values);
			smithingWorkItemShader.UniformMatrix("viewMatrix", rpi.CameraMatrixOriginf);
			smithingWorkItemShader.UniformMatrix("projectionMatrix", rpi.CurrentProjectionMatrix);
			rpi.RenderMultiTextureMesh(workItemMeshRef, "tex");
			smithingWorkItemShader.Stop();
		}
		if (fuelLevel > 0f && coalMeshRef != null)
		{
			IStandardShaderProgram prog = rpi.StandardShader;
			prog.Use();
			prog.RgbaAmbientIn = rpi.AmbientColor;
			prog.RgbaFogIn = rpi.FogColor;
			prog.FogMinIn = rpi.FogMin;
			prog.FogDensityIn = rpi.FogDensity;
			prog.RgbaTint = ColorUtil.WhiteArgbVec;
			prog.DontWarpVertices = 0;
			prog.AddRenderFlags = 0;
			prog.ExtraGodray = 0f;
			prog.OverlayOpacity = 0f;
			capi.World.BlockAccessor.GetLightRGBs(pos.X, pos.Y, pos.Z);
			long seed = capi.World.ElapsedMilliseconds / 10 + pos.GetHashCode();
			float flicker = (float)(Math.Sin((double)seed / 10.0) * 0.20000000298023224 + Math.Sin((double)seed / 20.0) * 0.20000000298023224 + Math.Cos((double)seed / 110.0) * 0.6000000238418579 + Math.Sin((double)seed / 50.0) + 1.0) / 4f;
			if (burning)
			{
				targetExtraOxygenRate += (extraOxygenRate - targetExtraOxygenRate) * dt * 10f;
				float[] glowColor2 = ColorUtil.GetIncandescenceColorAsColor4f((int)(1100f * (1f + extraOxygenRate)));
				glowColor2[0] *= 1f - flicker * 0.15f;
				glowColor2[1] *= 1f - flicker * 0.15f;
				glowColor2[2] *= 1f - flicker * 0.15f;
				prog.RgbaGlowIn = new Vec4f(glowColor2[0], glowColor2[1], glowColor2[2], 1f);
			}
			else
			{
				prog.RgbaGlowIn = new Vec4f(0f, 0f, 0f, 0f);
			}
			prog.NormalShaded = 0;
			prog.RgbaLightIn = new Vec4f(1f, 1f, 1f, 1f);
			prog.TempGlowMode = 1;
			int glow = (int)(255f * targetExtraOxygenRate * 2f);
			prog.ExtraGlow = (burning ? glow : 0);
			prog.ModelMatrix = ModelMat.Identity().Translate((double)pos.X - camPos.X, (double)pos.Y - camPos.Y + (double)((fuelLevel - 1f) / 16f / 4f), (double)pos.Z - camPos.Z).Values;
			prog.ViewMatrix = rpi.CameraMatrixOriginf;
			prog.ProjectionMatrix = rpi.CurrentProjectionMatrix;
			rpi.RenderMultiTextureMesh(coalMeshRef, "tex");
			prog.Stop();
		}
	}

	public void Dispose()
	{
		capi.Event.UnregisterRenderer(this, EnumRenderStage.Opaque);
		coalMeshRef?.Dispose();
		workItemMeshRef?.Dispose();
	}
}
