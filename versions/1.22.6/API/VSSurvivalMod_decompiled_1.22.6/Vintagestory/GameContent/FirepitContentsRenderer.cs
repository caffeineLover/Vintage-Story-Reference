using System;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class FirepitContentsRenderer : IRenderer, IDisposable
{
	private MultiTextureMeshRef meshref;

	private ICoreClientAPI api;

	private BlockPos pos;

	public ItemStack ContentStack;

	private int textureId;

	private Matrixf ModelMat = new Matrixf();

	public ModelTransform Transform;

	private ModelTransform defaultTransform;

	public IInFirepitRenderer contentStackRenderer;

	public bool RequireSpit
	{
		get
		{
			if (contentStackRenderer == null)
			{
				return ContentStack?.Item != null;
			}
			return false;
		}
	}

	public double RenderOrder => 0.5;

	public int RenderRange => 48;

	public FirepitContentsRenderer(ICoreClientAPI api, BlockPos pos)
	{
		this.api = api;
		this.pos = pos;
		Transform = new ModelTransform().EnsureDefaultValues();
		Transform.Origin.X = 0.5f;
		Transform.Origin.Y = 0.0625f;
		Transform.Origin.Z = 0.5f;
		Transform.Rotation.X = 90f;
		Transform.Rotation.Y = 90f;
		Transform.Rotation.Z = 0f;
		Transform.Translation.X = 0f;
		Transform.Translation.Y = 0.25f;
		Transform.Translation.Z = 0f;
		Transform.ScaleXYZ.X = 0.25f;
		Transform.ScaleXYZ.Y = 0.25f;
		Transform.ScaleXYZ.Z = 0.25f;
		defaultTransform = Transform;
	}

	internal void SetChildRenderer(ItemStack contentStack, IInFirepitRenderer renderer)
	{
		ContentStack = contentStack;
		meshref?.Dispose();
		meshref = null;
		contentStackRenderer = renderer;
	}

	public void SetContents(ItemStack newContentStack, ModelTransform transform)
	{
		contentStackRenderer?.Dispose();
		contentStackRenderer = null;
		Transform = transform;
		if (transform == null)
		{
			Transform = defaultTransform;
		}
		Transform.EnsureDefaultValues();
		meshref?.Dispose();
		meshref = null;
		if (newContentStack == null || newContentStack.Class == EnumItemClass.Block)
		{
			ContentStack = null;
			return;
		}
		MeshData ingredientMesh;
		if (newContentStack.Class == EnumItemClass.Item)
		{
			api.Tesselator.TesselateItem(newContentStack.Item, out ingredientMesh);
			textureId = api.ItemTextureAtlas.Positions[newContentStack.Item.FirstTexture.Baked.TextureSubId].atlasTextureId;
		}
		else
		{
			api.Tesselator.TesselateBlock(newContentStack.Block, out ingredientMesh);
			textureId = api.ItemTextureAtlas.Positions[newContentStack.Block.Textures.FirstOrDefault().Value.Baked.TextureSubId].atlasTextureId;
		}
		meshref = api.Render.UploadMultiTextureMesh(ingredientMesh);
		ContentStack = newContentStack;
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		if (contentStackRenderer != null)
		{
			contentStackRenderer.OnRenderFrame(deltaTime, stage);
		}
		else if (meshref != null)
		{
			IRenderAPI rpi = api.Render;
			Vec3d camPos = api.World.Player.Entity.CameraPos;
			rpi.GlDisableCullFace();
			rpi.GlToggleBlend(blend: true);
			IStandardShaderProgram standardShader = rpi.StandardShader;
			standardShader.Use();
			standardShader.DontWarpVertices = 0;
			standardShader.AddRenderFlags = 0;
			standardShader.RgbaAmbientIn = rpi.AmbientColor;
			standardShader.RgbaFogIn = rpi.FogColor;
			standardShader.FogMinIn = rpi.FogMin;
			standardShader.FogDensityIn = rpi.FogDensity;
			standardShader.RgbaTint = ColorUtil.WhiteArgbVec;
			standardShader.NormalShaded = 1;
			standardShader.ExtraGodray = 0f;
			standardShader.SsaoAttn = 0f;
			standardShader.AlphaTest = 0.05f;
			standardShader.OverlayOpacity = 0f;
			int temp = (int)ContentStack.Collectible.GetTemperature(api.World, ContentStack);
			Vec4f lightrgbs = api.World.BlockAccessor.GetLightRGBs(pos.X, pos.Y, pos.Z);
			float[] glowColor = ColorUtil.GetIncandescenceColorAsColor4f(temp);
			lightrgbs[0] += glowColor[0];
			lightrgbs[1] += glowColor[1];
			lightrgbs[2] += glowColor[2];
			standardShader.RgbaLightIn = lightrgbs;
			standardShader.ExtraGlow = GameMath.Clamp((temp - 500) / 4, 0, 255);
			standardShader.ModelMatrix = ModelMat.Identity().Translate((double)pos.X - camPos.X + (double)Transform.Translation.X, (double)pos.Y - camPos.Y + (double)Transform.Translation.Y, (double)pos.Z - camPos.Z + (double)Transform.Translation.Z).Translate(Transform.Origin.X, 0.6f + Transform.Origin.Y, Transform.Origin.Z)
				.RotateX(Transform.Rotation.X * ((float)Math.PI / 180f))
				.RotateY(Transform.Rotation.Y * ((float)Math.PI / 180f))
				.RotateZ(Transform.Rotation.Z * ((float)Math.PI / 180f))
				.Scale(Transform.ScaleXYZ.X, Transform.ScaleXYZ.Y, Transform.ScaleXYZ.Z)
				.Translate(0f - Transform.Origin.X, 0f - Transform.Origin.Y, 0f - Transform.Origin.Z)
				.Values;
			standardShader.ViewMatrix = rpi.CameraMatrixOriginf;
			standardShader.ProjectionMatrix = rpi.CurrentProjectionMatrix;
			rpi.RenderMultiTextureMesh(meshref, "tex");
			standardShader.Stop();
		}
	}

	public void Dispose()
	{
		api.Event.UnregisterRenderer(this, EnumRenderStage.Opaque);
		meshref?.Dispose();
		contentStackRenderer?.Dispose();
	}
}
