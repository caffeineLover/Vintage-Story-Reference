using System.Text.RegularExpressions;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class GuiHandbookCommandPage : GuiHandbookPage
{
	public IChatCommand Command;

	public string TextCacheTitle;

	public string TextCacheAll;

	public float searchWeightOffset;

	public LoadedTexture Texture;

	private string categoryCode;

	private bool isDuplicate;

	public override float SearchWeightOffset => searchWeightOffset;

	public override string PageCode => Command.FullName;

	public override string CategoryCode => categoryCode;

	public override bool IsDuplicate => isDuplicate;

	public GuiHandbookCommandPage(IChatCommand command, string fullname, string categoryCode, bool isRootAlias = false)
	{
		Command = command;
		TextCacheTitle = fullname;
		TextCacheAll = command.GetFullSyntaxHandbook(null, string.Empty, isRootAlias);
		this.categoryCode = categoryCode;
	}

	public void Recompose(ICoreClientAPI capi)
	{
		Texture?.Dispose();
		Texture = new TextTextureUtil(capi).GenTextTexture(TextCacheTitle, CairoFont.WhiteSmallText());
	}

	public override void RenderListEntryTo(ICoreClientAPI capi, float dt, double x, double y, double cellWidth, double cellHeight)
	{
		float size = (float)GuiElement.scaled(5.0);
		if (Texture == null)
		{
			Recompose(capi);
		}
		capi.Render.Render2DTexturePremultipliedAlpha(Texture.TextureId, x + (double)size, y + (double)size - GuiElement.scaled(3.0), Texture.Width, Texture.Height);
	}

	public override void Dispose()
	{
		Texture?.Dispose();
		Texture = null;
	}

	public override void ComposePage(GuiComposer detailViewGui, ElementBounds textBounds, ItemStack[] allstacks, ActionConsumable<string> openDetailPageFor)
	{
		RichTextComponentBase[] cmps = GetPageText(detailViewGui.Api, allstacks, openDetailPageFor);
		detailViewGui.AddRichtext(cmps, textBounds, "richtext");
	}

	protected virtual RichTextComponentBase[] GetPageText(ICoreClientAPI capi, ItemStack[] allStacks, ActionConsumable<string> openDetailPageFor)
	{
		return VtmlUtil.Richtextify(capi, "<font size=\"24\"><strong>" + Command.CallSyntax + "</strong></font>\n\n" + TextCacheAll, CairoFont.WhiteSmallText());
	}

	public override PageText GetPageText()
	{
		return new PageText
		{
			Title = TextCacheTitle,
			Text = TextCacheAll
		};
	}

	public override float GetSearchTextPosRel(string searchText)
	{
		if (string.IsNullOrEmpty(searchText) || string.IsNullOrEmpty(TextCacheAll))
		{
			return 0f;
		}
		Regex regex = GuiDialog.RegexFromSearchText(searchText);
		if (TextCacheTitle != null && regex.Matches(TextCacheTitle).Count > 0)
		{
			return 0f;
		}
		string[] matches = regex.Split(TextCacheAll);
		if (matches.Length < 2)
		{
			return 0f;
		}
		return (float)matches[0].Length / (float)TextCacheAll.Length;
	}
}
