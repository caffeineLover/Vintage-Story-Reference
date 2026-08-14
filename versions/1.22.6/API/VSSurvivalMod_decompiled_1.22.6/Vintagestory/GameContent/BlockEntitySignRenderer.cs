using System;
using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockEntitySignRenderer : IRenderer, IDisposable
{
	public static double SignRendererOrder = 1.101;

	public static double AfterSignRendererOrder = 1.102;

	private static readonly double[] blackColor = new double[4] { 0.0, 0.0, 0.0, 0.8 };

	protected int TextWidth = 200;

	protected int TextHeight = 100;

	protected float QuadWidth = 0.875f;

	protected float QuadHeight = 13f / 32f;

	protected string text;

	protected CairoFont font;

	protected BlockPos pos;

	protected ICoreClientAPI api;

	protected LoadedTexture loadedTexture;

	protected MeshRef quadModelRef;

	public Matrixf ModelMat = new Matrixf();

	public float rotY;

	protected float translateX;

	protected float translateY;

	protected float translateZ;

	protected float offsetX;

	protected float offsetY;

	protected float offsetZ;

	public float fontSize = 20f;

	private EnumVerticalAlign verticalAlign;

	internal bool translateable;

	private static float lastDt = -1f;

	private static int renderTextMaxCount;

	public static IStandardShaderProgram progCached;

	public double RenderOrder => SignRendererOrder;

	public int RenderRange => 24;

	public BlockEntitySignRenderer(BlockPos pos, ICoreClientAPI api, TextAreaConfig config)
	{
		this.api = api;
		this.pos = pos;
		if (config == null)
		{
			config = new TextAreaConfig();
		}
		fontSize = config.FontSize;
		QuadWidth = config.textVoxelWidth / 16f;
		QuadHeight = config.textVoxelHeight / 16f;
		verticalAlign = config.VerticalAlign;
		TextWidth = config.MaxWidth;
		font = new CairoFont(fontSize, config.FontName, blackColor);
		if (config.BoldFont)
		{
			font.WithWeight(FontWeight.Bold);
		}
		font.LineHeightMultiplier = 0.8999999761581421;
		api.Event.RegisterRenderer(this, EnumRenderStage.Opaque, "sign");
		MeshData modeldata = QuadMeshUtil.GetQuad();
		modeldata.Uv = new float[8] { 1f, 1f, 0f, 1f, 0f, 0f, 1f, 0f };
		modeldata.Rgba = new byte[16];
		modeldata.Rgba.Fill(byte.MaxValue);
		quadModelRef = api.Render.UploadMesh(modeldata);
		Block block = api.World.BlockAccessor.GetBlock(pos);
		BlockFacing facing = BlockFacing.FromCode(block.LastCodePart());
		if (facing != null)
		{
			float wallOffset = ((block.Variant["attachment"] == "wall") ? 0.22f : 0f);
			translateY = 0.5625f;
			switch (facing.Index)
			{
			case 0:
				translateX = 0.5f;
				translateZ = 0.29000002f - wallOffset;
				rotY = 180f;
				break;
			case 1:
				translateX = 0.71f + wallOffset;
				translateZ = 0.5f;
				rotY = 90f;
				break;
			case 2:
				translateX = 0.5f;
				translateZ = 0.71f + wallOffset;
				break;
			case 3:
				translateX = 0.29000002f - wallOffset;
				translateZ = 0.5f;
				rotY = 270f;
				break;
			}
			offsetX += config.textVoxelOffsetX / 16f;
			offsetY += config.textVoxelOffsetY / 16f;
			offsetZ += config.textVoxelOffsetZ / 16f;
		}
	}

	public void SetFreestanding(float angleRad)
	{
		rotY = 180f + angleRad * (180f / (float)Math.PI);
		translateX = 0.5f;
		translateZ = 0.5f;
		offsetZ = -0.094375f;
	}

	public virtual void SetNewText(string text, int color)
	{
		if (translateable)
		{
			string translatedText = Lang.Get(text);
			if (Lang.UsesNonLatinCharacters(Lang.CurrentLocale))
			{
				string englishText = Lang.GetL(Lang.DefaultLocale, text);
				if (translatedText != englishText)
				{
					font.Fontname = GuiStyle.StandardFontName;
				}
			}
			text = translatedText;
		}
		font.Color = ColorUtil.ToRGBADoubles(color);
		loadedTexture?.Dispose();
		loadedTexture = null;
		this.text = text;
	}

	protected LoadedTexture RenderText()
	{
		font.UnscaledFontsize = fontSize / RuntimeEnv.GUIScale;
		double verPadding = ((verticalAlign == EnumVerticalAlign.Middle) ? ((double)TextHeight - api.Gui.Text.GetMultilineTextHeight(font, text, TextWidth)) : 0.0);
		TextBackground bg = new TextBackground
		{
			VerPadding = (int)verPadding / 2
		};
		return api.Gui.TextTexture.GenTextTexture(text, font, TextWidth, TextHeight, bg, EnumTextOrientation.Center);
	}

	public virtual void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		if (loadedTexture == null)
		{
			if (text == null || text.Length <= 0)
			{
				return;
			}
			if (lastDt != deltaTime)
			{
				lastDt = deltaTime;
				renderTextMaxCount = 32;
			}
			if (renderTextMaxCount-- <= 0)
			{
				return;
			}
			loadedTexture = RenderText();
		}
		if (api.Render.DefaultFrustumCuller.SphereInFrustum((double)pos.X + 0.5, (double)pos.InternalY + 0.5, (double)pos.Z + 0.5, 1.0))
		{
			IRenderAPI rpi = api.Render;
			Vec3d camPos = api.World.Player.Entity.CameraPos;
			rpi.GlToggleBlend(blend: true, EnumBlendMode.PremultipliedAlpha);
			IStandardShaderProgram prog = progCached;
			if (prog == null)
			{
				rpi.GlDisableCullFace();
				prog = (progCached = rpi.PreparedStandardShader(pos.X, pos.InternalY, pos.Z));
				prog.ViewMatrix = rpi.CameraMatrixOriginf;
				prog.ProjectionMatrix = rpi.CurrentProjectionMatrix;
				prog.NormalShaded = 0;
				prog.ExtraGodray = 0f;
				prog.SsaoAttn = 0f;
				prog.AlphaTest = 0.05f;
				prog.OverlayOpacity = 0f;
			}
			prog.RgbaLightIn = api.World.BlockAccessor.GetLightRGBs(pos);
			prog.Tex2D = loadedTexture.TextureId;
			prog.ModelMatrix = ModelMat.Identity().Translate((double)pos.X - camPos.X + (double)translateX, (double)pos.InternalY - camPos.Y + (double)translateY, (double)pos.Z - camPos.Z + (double)translateZ).RotateY(rotY * ((float)Math.PI / 180f))
				.Translate(offsetX, offsetY, offsetZ)
				.Scale(0.5f * QuadWidth, 0.5f * QuadHeight, 0.5f * QuadWidth)
				.Values;
			rpi.RenderMesh(quadModelRef);
			rpi.GlToggleBlend(blend: true);
		}
	}

	public static void AfterRenderFrameOnce()
	{
		if (progCached != null)
		{
			IStandardShaderProgram standardShaderProgram = progCached;
			progCached = null;
			standardShaderProgram.Stop();
		}
		lastDt = -1f;
	}

	public void Dispose()
	{
		api.Event.UnregisterRenderer(this, EnumRenderStage.Opaque);
		loadedTexture?.Dispose();
		quadModelRef?.Dispose();
	}

	public static void RegisterAndReserveRenderOrderRange(ICoreClientAPI api)
	{
		api.Event.RegisterRenderer(new AfterSignRenderer(), EnumRenderStage.Opaque, "aftersign", SignRendererOrder, AfterSignRendererOrder, typeof(BlockEntitySignRenderer));
	}
}
