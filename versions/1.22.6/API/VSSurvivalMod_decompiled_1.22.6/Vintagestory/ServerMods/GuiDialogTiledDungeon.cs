using Vintagestory.API.Client;
using Vintagestory.API.Datastructures;

namespace Vintagestory.ServerMods;

public class GuiDialogTiledDungeon : GuiDialogGeneric
{
	private bool save;

	public override ITreeAttribute Attributes
	{
		get
		{
			TreeAttribute treeAttribute = new TreeAttribute();
			treeAttribute.SetInt("save", save ? 1 : 0);
			treeAttribute.SetString("target", base.SingleComposer.GetTextInput("target").GetText());
			treeAttribute.SetString("name", base.SingleComposer.GetTextInput("name").GetText());
			return treeAttribute;
		}
	}

	public GuiDialogTiledDungeon(string dialogTitle, string name, string target, ICoreClientAPI capi)
		: base(dialogTitle, capi)
	{
		_ = GuiElementItemSlotGridBase.unscaledSlotPadding;
		ElementBounds slotBounds = ElementBounds.Fixed(EnumDialogArea.None, 0.0, 30.0, 350.0, 30.0);
		ElementBounds leftButton = ElementBounds.Fixed(EnumDialogArea.LeftFixed, 0.0, 0.0, 0.0, 0.0).WithFixedPadding(10.0, 1.0);
		ElementBounds rightButton = ElementBounds.Fixed(EnumDialogArea.RightFixed, 0.0, 0.0, 0.0, 0.0).WithFixedPadding(10.0, 1.0);
		ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
		bgBounds.BothSizing = ElementSizing.FitToChildren;
		ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle).WithFixedAlignmentOffset(GuiStyle.DialogToScreenPadding, 0.0);
		CairoFont textInput = CairoFont.TextInput();
		CairoFont text = CairoFont.WhiteSmallishText();
		base.SingleComposer = capi.Gui.CreateCompo("tiledungeon", dialogBounds).AddShadedDialogBG(bgBounds).AddDialogTitleBar(dialogTitle, OnTitleBarClose)
			.BeginChildElements(bgBounds)
			.AddStaticText("Name", text, slotBounds = slotBounds.FlatCopy().WithFixedWidth(100.0))
			.AddTextInput(slotBounds = slotBounds.RightCopy(5.0, -3.0).WithFixedWidth(350.0), null, textInput, "name")
			.AddStaticText("Target", text, slotBounds = slotBounds.BelowCopy(0.0, 15.0).WithFixedX(0).WithFixedWidth(100.0))
			.AddTextInput(slotBounds = slotBounds.RightCopy(5.0, -3.0).WithFixedWidth(350.0), null, textInput, "target")
			.AddStaticText("Empty target means it only accepts connections but does not actively connect to anything. The opposite happens when name field is empty.", CairoFont.WhiteSmallText(), slotBounds = slotBounds.BelowCopy(0.0, 20.0).WithFixedX(0).WithFixedWidth(500.0))
			.AddButton("Save", OnSaveClicked, rightButton.FixedUnder(slotBounds, 40.0))
			.AddButton("Close", OnCloseClicked, leftButton.FixedUnder(slotBounds, 40.0))
			.EndChildElements()
			.Compose();
		GuiElementTextInput textInput2 = base.SingleComposer.GetTextInput("target");
		textInput2.Enabled = target != null;
		textInput2.SetValue(target);
		GuiElementTextInput textInput3 = base.SingleComposer.GetTextInput("name");
		textInput3.Enabled = name != null;
		textInput3.SetValue(name);
	}

	private void OnTitleBarClose()
	{
		TryClose();
	}

	private bool OnSaveClicked()
	{
		save = true;
		TryClose();
		return true;
	}

	private bool OnCloseClicked()
	{
		TryClose();
		return true;
	}
}
