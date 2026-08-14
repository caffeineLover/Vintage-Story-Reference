using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityMapComponent : MapComponent
{
	private readonly Entity entity;

	private readonly MeshRef quadModel;

	private readonly LoadedTexture texture;

	private Vec2f viewPos = new Vec2f();

	private readonly Matrixf mvMat = new Matrixf();

	private readonly int color;

	public EntityMapComponent(ICoreClientAPI capi, LoadedTexture texture, Entity entity, string? color = null)
		: base(capi)
	{
		quadModel = capi.Render.UploadMesh(QuadMeshUtil.GetQuad());
		this.texture = texture;
		this.entity = entity;
		this.color = ((color != null) ? (ColorUtil.Hex2Int(color) | -16777216) : 0);
	}

	public override void Render(GuiElementMap map, float dt)
	{
		if (!texture.Disposed && !quadModel.Disposed)
		{
			map.TranslateWorldPosToViewPos(entity.Pos.XYZ, ref viewPos);
			float x = (float)(map.Bounds.renderX + (double)viewPos.X);
			float y = (float)(map.Bounds.renderY + (double)viewPos.Y);
			ICoreClientAPI api = map.Api;
			capi.Render.GlToggleBlend(blend: true);
			IShaderProgram prog = api.Render.GetEngineShader(EnumShaderProgram.Gui);
			if (color == 0)
			{
				prog.Uniform("rgbaIn", ColorUtil.WhiteArgbVec);
			}
			else
			{
				Vec4f vec = new Vec4f();
				ColorUtil.ToRGBAVec4f(color, ref vec);
				prog.Uniform("rgbaIn", vec);
			}
			prog.Uniform("applyColor", 0);
			prog.Uniform("extraGlow", 0);
			prog.Uniform("noTexture", 0f);
			prog.BindTexture2D("tex2d", texture.TextureId, 0);
			mvMat.Set(api.Render.CurrentModelviewMatrix).Translate(x, y, 60f).Scale(texture.Width, texture.Height, 0f)
				.Scale(0.5f, 0.5f, 0f)
				.RotateZ(0f - entity.Pos.Yaw + (float)Math.PI);
			prog.UniformMatrix("projectionMatrix", api.Render.CurrentProjectionMatrix);
			prog.UniformMatrix("modelViewMatrix", mvMat.Values);
			api.Render.RenderMesh(quadModel);
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		quadModel.Dispose();
		GC.SuppressFinalize(this);
	}

	public override void OnMouseMove(MouseEvent args, GuiElementMap mapElem, StringBuilder hoverText)
	{
		Vec2f viewPos = new Vec2f();
		mapElem.TranslateWorldPosToViewPos(entity.Pos.XYZ, ref viewPos);
		double mouseX = (double)args.X - mapElem.Bounds.renderX;
		double mouseY = (double)args.Y - mapElem.Bounds.renderY;
		double sc = GuiElement.scaled(5.0);
		if (Math.Abs((double)viewPos.X - mouseX) < sc && Math.Abs((double)viewPos.Y - mouseY) < sc)
		{
			if (entity is EntityPlayer eplr)
			{
				hoverText.AppendLine("Player " + capi.World.PlayerByUid(eplr.PlayerUID)?.PlayerName);
			}
			else
			{
				hoverText.AppendLine(entity.GetName());
			}
		}
	}
}
