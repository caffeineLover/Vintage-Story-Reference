using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public abstract class GuiHandbookPage : IFlatListItem
{
	public int PageNumber;

	public abstract string PageCode { get; }

	public abstract string CategoryCode { get; }

	public bool Visible { get; set; } = true;

	public abstract bool IsDuplicate { get; }

	public abstract float SearchWeightOffset { get; }

	public abstract void RenderListEntryTo(ICoreClientAPI capi, float dt, double x, double y, double cellWdith, double cellHeight);

	public abstract void Dispose();

	public abstract PageText GetPageText();

	public abstract void ComposePage(GuiComposer detailViewGui, ElementBounds textBounds, ItemStack[] allstacks, ActionConsumable<string> openDetailPageFor);

	public virtual float GetSearchTextPosRel(string searchText)
	{
		return 0f;
	}
}
