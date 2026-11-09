using System;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200009A RID: 154
	public class RadioConsoleDialog : GuiDialogBlockEntity
	{
		// Token: 0x0600042E RID: 1070 RVA: 0x00017823 File Offset: 0x00015A23
		public RadioConsoleDialog(ICoreClientAPI capi, BlockEntityRadioSupervisionConsole console) : base(UIUtils.I18n("Radio.Console.Gui.Title"), console.Pos, capi)
		{
			this.console = console;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001785C File Offset: 0x00015A5C
		public override void OnGuiOpened()
		{
			base.OnGuiOpened();
			this.BuildComposer();
			this.pendingFrequency = this.console.Frequency;
			this.pendingDisplayName = this.console.DisplayName;
			GuiElementTextInput guiElementTextInput = this.frequencyInput;
			if (guiElementTextInput != null)
			{
				guiElementTextInput.SetValue(this.pendingFrequency, true);
			}
			GuiElementTextInput guiElementTextInput2 = this.displayNameInput;
			if (guiElementTextInput2 != null)
			{
				guiElementTextInput2.SetValue(this.pendingDisplayName, true);
			}
			this.RefreshData();
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000178D0 File Offset: 0x00015AD0
		public void RefreshData()
		{
			this.pendingFrequency = this.console.Frequency;
			this.pendingDisplayName = this.console.DisplayName;
			GuiElementTextInput guiElementTextInput = this.frequencyInput;
			if (guiElementTextInput != null)
			{
				guiElementTextInput.SetValue(this.pendingFrequency, true);
			}
			GuiElementTextInput guiElementTextInput2 = this.displayNameInput;
			if (guiElementTextInput2 == null)
			{
				return;
			}
			guiElementTextInput2.SetValue(this.pendingDisplayName, true);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00017930 File Offset: 0x00015B30
		private void BuildComposer()
		{
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds frequencyLabelBounds = ElementBounds.Fixed(0.0, 35.0, 420.0, 18.0);
			ElementBounds frequencyInputBounds = ElementBounds.Fixed(0.0, 55.0, 420.0, 26.0);
			ElementBounds displayNameLabelBounds = ElementBounds.Fixed(0.0, 91.0, 420.0, 18.0);
			ElementBounds displayNameInputBounds = ElementBounds.Fixed(0.0, 111.0, 420.0, 26.0);
			ElementBounds saveBounds = ElementBounds.Fixed(0.0, 147.0, 120.0, 28.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			bgBounds.WithChildren(new ElementBounds[]
			{
				frequencyLabelBounds,
				frequencyInputBounds,
				displayNameLabelBounds,
				displayNameInputBounds,
				saveBounds
			});
			base.SingleComposer = GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddTextInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddTextInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("radioconsole", dialogBounds), bgBounds, true, 5.0, 0.75f), UIUtils.I18n("Radio.Console.Gui.Title"), new Action(this.OnTitleBarCloseClicked), null, null, null), UIUtils.I18n("Radio.Console.Gui.Frequency"), CairoFont.WhiteSmallText(), frequencyLabelBounds, null), frequencyInputBounds, new Action<string>(this.OnFrequencyChanged), CairoFont.TextInput(), "radioFrequencyInput"), UIUtils.I18n("Radio.Console.Gui.DisplayName"), CairoFont.WhiteSmallText(), displayNameLabelBounds, null), displayNameInputBounds, new Action<string>(this.OnDisplayNameChanged), CairoFont.TextInput(), "radioDisplayNameInput"), UIUtils.I18n("Radio.Gui.Save"), new ActionConsumable(this.OnSaveClicked), saveBounds, 2, null).Compose(true);
			this.frequencyInput = GuiComposerHelpers.GetTextInput(base.SingleComposer, "radioFrequencyInput");
			this.displayNameInput = GuiComposerHelpers.GetTextInput(base.SingleComposer, "radioDisplayNameInput");
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00017B60 File Offset: 0x00015D60
		private void OnFrequencyChanged(string value)
		{
			this.pendingFrequency = (value ?? "");
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00017B72 File Offset: 0x00015D72
		private void OnDisplayNameChanged(string value)
		{
			this.pendingDisplayName = (value ?? "");
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00017B84 File Offset: 0x00015D84
		private bool OnSaveClicked()
		{
			this.console.RequestSaveSettings(this.pendingFrequency, this.pendingDisplayName);
			return true;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00017B9E File Offset: 0x00015D9E
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x0400023C RID: 572
		private readonly BlockEntityRadioSupervisionConsole console;

		// Token: 0x0400023D RID: 573
		private GuiElementTextInput frequencyInput;

		// Token: 0x0400023E RID: 574
		private GuiElementTextInput displayNameInput;

		// Token: 0x0400023F RID: 575
		private string pendingFrequency = "";

		// Token: 0x04000240 RID: 576
		private string pendingDisplayName = "";
	}
}
