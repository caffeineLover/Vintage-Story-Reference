using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class GuiHandbookTextPage : GuiHandbookPage
{
	public string pageCode;

	public string Title;

	public string Text = "";

	public string categoryCode = "guide";

	public LoadedTexture Texture;

	private RichTextComponentBase[] comps;

	private string titleCached;

	public override string PageCode => pageCode;

	public override string CategoryCode => categoryCode;

	public override float SearchWeightOffset => 1f;

	public override bool IsDuplicate => false;

	public override void Dispose()
	{
		Texture?.Dispose();
		Texture = null;
	}

	public void Init(ICoreClientAPI capi)
	{
		if (Text.Length < 255)
		{
			Text = Lang.Get(Text);
		}
		comps = VtmlUtil.Richtextify(capi, Text, CairoFont.WhiteSmallText().WithLineHeightMultiplier(1.2));
		titleCached = Lang.Get(Title).ToSearchFriendly();
	}

	public override void ComposePage(GuiComposer detailViewGui, ElementBounds textBounds, ItemStack[] allstacks, ActionConsumable<string> openDetailPageFor)
	{
		detailViewGui.AddRichtext(comps, textBounds, "richtext");
	}

	public void Recompose(ICoreClientAPI capi)
	{
		Texture?.Dispose();
		Texture = new TextTextureUtil(capi).GenTextTexture(Lang.Get(Title), CairoFont.WhiteSmallText());
	}

	public override PageText GetPageText()
	{
		return new PageText
		{
			Title = titleCached,
			Text = Text
		};
	}

	public override void RenderListEntryTo(ICoreClientAPI capi, float dt, double x, double y, double cellWidth, double cellHeight)
	{
		float size = (float)GuiElement.scaled(25.0);
		float pad = (float)GuiElement.scaled(10.0);
		if (Texture == null)
		{
			Recompose(capi);
		}
		capi.Render.Render2DTexturePremultipliedAlpha(Texture.TextureId, x + (double)pad, y + (double)(size / 4f) - GuiElement.scaled(3.0), Texture.Width, Texture.Height);
	}
}
