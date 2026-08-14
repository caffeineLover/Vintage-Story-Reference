using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Vintagestory.Client.NoObf;

public class SystemSelectedBlockOutline : ClientSystem
{
	private WireframeCube cubeWireFrame;

	public override string Name => "sbo";

	public SystemSelectedBlockOutline(ClientMain game)
		: base(game)
	{
		cubeWireFrame = WireframeCube.CreateUnitCube(game.api, -1);
		game.eventManager.RegisterRenderer(OnRenderFrame3DPost, EnumRenderStage.AfterFinalComposition, Name, 0.9);
	}

	public override void Dispose(ClientMain game)
	{
		cubeWireFrame.Dispose();
	}

	public void OnRenderFrame3DPost(float deltaTime)
	{
		if (!ClientSettings.SelectedBlockOutline || !game.ShouldRender2DOverlays || game.BlockSelection == null)
		{
			return;
		}
		BlockPos pos = game.BlockSelection.Position;
		if (game.BlockSelection.DidOffset)
		{
			pos = pos.AddCopy(game.BlockSelection.Face.Opposite);
		}
		double x = (double)pos.X + game.Player.Entity.CameraPosOffset.X;
		double y = (double)pos.InternalY + game.Player.Entity.CameraPosOffset.Y;
		double z = (double)pos.Z + game.Player.Entity.CameraPosOffset.Z;
		Block block = game.WorldMap.RelaxedBlockAccess.GetBlock(pos, 2);
		if (!block.SideSolid.Any)
		{
			block = game.WorldMap.RelaxedBlockAccess.GetBlock(pos);
		}
		ICustomSelectionBoxRender icsbr = block.GetInterface<ICustomSelectionBoxRender>(game.World, null);
		if (icsbr != null)
		{
			icsbr.RenderSelectionBoxes(game.BlockSelection, delegate(Cuboidf cuboidf, float width, Vec4f color2)
			{
				cubeWireFrame.Render(game.api, x + (double)cuboidf.X1, y + (double)cuboidf.Y1, z + (double)cuboidf.Z1, cuboidf.XSize, cuboidf.YSize, cuboidf.ZSize, width, color2);
			});
			return;
		}
		Cuboidf[] boxes = ((!block.SideSolid.Any) ? game.GetBlockIntersectionBoxes(pos) : block.GetSelectionBoxes(game.WorldMap.RelaxedBlockAccess, pos));
		if (boxes == null || boxes.Length == 0)
		{
			return;
		}
		bool partialSelection = block.DoPartialSelection(game, pos);
		Vec4f color = block.GetSelectionColor(game.api, pos);
		float linewidthMul = ClientSettings.Wireframethickness;
		for (int i = 0; i < boxes.Length; i++)
		{
			if (partialSelection)
			{
				i = game.BlockSelection.SelectionBoxIndex;
			}
			if (boxes.Length <= i)
			{
				break;
			}
			Cuboidf box = boxes[i];
			if (box is DecorSelectionBox)
			{
				if (partialSelection)
				{
					break;
				}
				continue;
			}
			double posx = x + (double)box.X1;
			double posy = y + (double)box.Y1;
			double posz = z + (double)box.Z1;
			cubeWireFrame.Render(game.api, posx, posy, posz, box.XSize, box.YSize, box.ZSize, 1.6f * linewidthMul, color);
			if (partialSelection)
			{
				break;
			}
		}
	}

	public override EnumClientSystemType GetSystemType()
	{
		return EnumClientSystemType.Render;
	}
}
