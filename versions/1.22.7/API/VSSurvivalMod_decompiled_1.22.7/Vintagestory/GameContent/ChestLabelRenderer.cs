using System;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class ChestLabelRenderer : BlockEntitySignRenderer
{
	public ChestLabelRenderer(BlockPos pos, ICoreClientAPI api)
		: base(pos, api, null)
	{
		TextWidth = 200;
	}

	public void SetRotation(float radY)
	{
		rotY = radY;
	}

	public override void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		if (loadedTexture == null)
		{
			if (text == null || text.Length <= 0)
			{
				return;
			}
			loadedTexture = RenderText();
		}
		IRenderAPI rpi = api.Render;
		Vec3d camPos = api.World.Player.Entity.CameraPos;
		if (!(camPos.SquareDistanceTo(pos.X, pos.Y, pos.Z) > 400f))
		{
			rpi.GlToggleBlend(blend: true, EnumBlendMode.PremultipliedAlpha);
			IStandardShaderProgram prog = BlockEntitySignRenderer.progCached;
			if (prog == null)
			{
				rpi.GlDisableCullFace();
				prog = (BlockEntitySignRenderer.progCached = rpi.PreparedStandardShader(pos.X, pos.InternalY, pos.Z));
				prog.ViewMatrix = rpi.CameraMatrixOriginf;
				prog.ProjectionMatrix = rpi.CurrentProjectionMatrix;
				prog.NormalShaded = 0;
				prog.ExtraGodray = 0f;
				prog.SsaoAttn = 0f;
				prog.AlphaTest = 0.05f;
				prog.OverlayOpacity = 0f;
			}
			prog.RgbaLightIn = api.World.BlockAccessor.GetLightRGBs(pos);
			prog.AddRenderFlags = 0;
			prog.Tex2D = loadedTexture.TextureId;
			prog.ModelMatrix = ModelMat.Identity().Translate((double)((float)pos.X + 0.5f) - camPos.X, (double)((float)pos.Y + 0.5f) - camPos.Y, (double)((float)pos.Z + 0.5f) - camPos.Z).RotateY(rotY + (float)Math.PI)
				.Translate(-0.5, -0.5, -0.5)
				.Translate(0.5f, 0.35f, 0.0925f)
				.Scale(0.45f * QuadWidth, 0.45f * QuadHeight, 0.45f * QuadWidth)
				.Values;
			rpi.RenderMesh(quadModelRef);
			rpi.GlToggleBlend(blend: true);
		}
	}
}
