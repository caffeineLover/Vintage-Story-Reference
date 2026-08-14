using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public interface ICustomSelectionBoxRender
{
	void RenderSelectionBoxes(BlockSelection blockSel, RenderBoxDelegate renderBoxHandler);
}
