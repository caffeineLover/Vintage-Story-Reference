using System;
using System.Linq;
using System.Text;
using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class PlayerMapLayer : MarkerMapLayer
{
	private readonly ICoreClientAPI? capi;

	private LoadedTexture? ownPlayerTexture;

	private LoadedTexture? otherPlayerTexture;

	private MeshRef? quadModel;

	private readonly SystemRemotePlayerTracking playerTracking;

	public override string Title => "Players";

	public override EnumMapAppSide DataSide => EnumMapAppSide.Client;

	public override string LayerGroupCode => "terrain";

	public PlayerMapLayer(ICoreAPI api, IWorldMapManager mapsink)
		: base(api, mapsink)
	{
		capi = api as ICoreClientAPI;
		playerTracking = api.ModLoader.GetModSystem<SystemRemotePlayerTracking>();
	}

	public override void OnMapOpenedClient()
	{
		if (capi != null)
		{
			int size = (int)GuiElement.scaled(32.0);
			if (ownPlayerTexture == null)
			{
				ImageSurface surface = new ImageSurface(Format.Argb32, size, size);
				Context ctx = new Context(surface);
				ctx.SetSourceRGBA(0.0, 0.0, 0.0, 0.0);
				ctx.Paint();
				capi.Gui.Icons.DrawMapPlayer(ctx, 0, 0, size, size, new double[4] { 0.0, 0.0, 0.0, 1.0 }, new double[4] { 1.0, 1.0, 1.0, 1.0 });
				ownPlayerTexture = new LoadedTexture(capi, capi.Gui.LoadCairoTexture(surface, linearMag: false), size / 2, size / 2);
				ctx.Dispose();
				surface.Dispose();
			}
			if (otherPlayerTexture == null)
			{
				ImageSurface surface2 = new ImageSurface(Format.Argb32, size, size);
				Context ctx2 = new Context(surface2);
				ctx2.SetSourceRGBA(0.0, 0.0, 0.0, 0.0);
				ctx2.Paint();
				capi.Gui.Icons.DrawMapPlayer(ctx2, 0, 0, size, size, new double[4] { 0.3, 0.3, 0.3, 1.0 }, new double[4] { 0.7, 0.7, 0.7, 1.0 });
				otherPlayerTexture = new LoadedTexture(capi, capi.Gui.LoadCairoTexture(surface2, linearMag: false), size / 2, size / 2);
				ctx2.Dispose();
				surface2.Dispose();
			}
			if (quadModel == null)
			{
				quadModel = capi.Render.UploadMesh(QuadMeshUtil.GetQuad());
			}
		}
	}

	public override void Render(GuiElementMap map, float dt)
	{
		if (!base.Active || capi == null || ownPlayerTexture == null || otherPlayerTexture == null)
		{
			return;
		}
		Vec2f viewPos = new Vec2f();
		Vec3d worldPos = new Vec3d();
		Matrixf mvMat = new Matrixf();
		IShaderProgram prog = capi.Render.GetEngineShader(EnumShaderProgram.Gui);
		prog.UniformMatrix("projectionMatrix", capi.Render.CurrentProjectionMatrix);
		prog.Uniform("applyColor", 0);
		prog.Uniform("extraGlow", 0);
		prog.Uniform("noTexture", 0f);
		capi.Render.GlToggleBlend(blend: true);
		foreach (PacketPlayerPosition position in playerTracking.GetAllTrackedPlayerPositions())
		{
			if (position.AssociatedPlayer is IClientPlayer player)
			{
				float yaw;
				if (player.Entity == null)
				{
					worldPos.Set(position.PosX, 0.0, position.PosZ);
					yaw = (float)position.Yaw;
				}
				else
				{
					worldPos.Set(player.Entity.Pos.X, player.Entity.Pos.Y, player.Entity.Pos.Z);
					yaw = player.Entity.Pos.Yaw;
				}
				Vec4f color = ColorUtil.WhiteArgbVec;
				string entCode = player.Entitlements?.FirstOrDefault()?.Code;
				if (entCode != null && GlobalConstants.playerColorByEntitlement.TryGetValue(entCode, out var col))
				{
					color = new Vec4f((float)col[0], (float)col[1], (float)col[2], (float)col[3]);
				}
				prog.Uniform("rgbaIn", color);
				map.TranslateWorldPosToViewPos(worldPos, ref viewPos);
				float x = (float)(map.Bounds.renderX + (double)viewPos.X);
				float y = (float)(map.Bounds.renderY + (double)viewPos.Y);
				LoadedTexture tex = ((player == capi.World.Player) ? ownPlayerTexture : otherPlayerTexture);
				prog.BindTexture2D("tex2d", tex.TextureId, 0);
				mvMat.Set(capi.Render.CurrentModelviewMatrix).Translate(x, y, 60f).Scale(tex.Width, tex.Height, 0f)
					.Scale(0.5f, 0.5f, 0f)
					.RotateZ(0f - yaw + (float)Math.PI);
				prog.UniformMatrix("modelViewMatrix", mvMat.Values);
				capi.Render.RenderMesh(quadModel);
			}
		}
	}

	public override void OnMouseMoveClient(MouseEvent args, GuiElementMap mapElem, StringBuilder hoverText)
	{
		if (!base.Active || capi == null)
		{
			return;
		}
		Vec2f viewPos = new Vec2f();
		Vec3d worldPos = new Vec3d();
		foreach (PacketPlayerPosition position in playerTracking.GetAllTrackedPlayerPositions())
		{
			if (position.AssociatedPlayer is IClientPlayer player)
			{
				if (player.Entity == null)
				{
					worldPos.Set(position.PosX, 0.0, position.PosZ);
				}
				else
				{
					worldPos.Set(player.Entity.Pos.X, player.Entity.Pos.Y, player.Entity.Pos.Z);
				}
				mapElem.TranslateWorldPosToViewPos(worldPos, ref viewPos);
				double mouseX = (double)args.X - mapElem.Bounds.renderX;
				double mouseY = (double)args.Y - mapElem.Bounds.renderY;
				double sc = GuiElement.scaled(5.0);
				if (Math.Abs((double)viewPos.X - mouseX) < sc && Math.Abs((double)viewPos.Y - mouseY) < sc)
				{
					hoverText.AppendLine(Lang.Get("worldmap-label-player", player.PlayerName));
				}
			}
		}
	}

	public override void Dispose()
	{
		ownPlayerTexture?.Dispose();
		ownPlayerTexture = null;
		otherPlayerTexture?.Dispose();
		otherPlayerTexture = null;
		quadModel?.Dispose();
		quadModel = null;
	}
}
