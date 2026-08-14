using System;
using System.Runtime.CompilerServices;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Gui.CustomElements;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000077 RID: 119
	public class GuiDialogSwitchboard : GuiDialogBlockEntity
	{
		// Token: 0x06000369 RID: 873 RVA: 0x0001356B File Offset: 0x0001176B
		public GuiDialogSwitchboard(ICoreClientAPI capi, BlockEntitySwitchboard switchboard) : base(UIUtils.I18n("Switchboard.Gui.Title"), switchboard.Pos, capi)
		{
			this.switchboard = switchboard;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000135A4 File Offset: 0x000117A4
		public override void OnGuiOpened()
		{
			base.OnGuiOpened();
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds networkNameLabelBounds = ElementBounds.Fixed(0.0, 35.0, 420.0, 18.0);
			ElementBounds networkNameInputBounds = ElementBounds.Fixed(0.0, 55.0, 320.0, 26.0);
			ElementBounds networkNameSaveBounds = ElementBounds.Fixed(332.0, 55.0, 88.0, 26.0);
			ElementBounds powerToggleLabelBounds = ElementBounds.Fixed(0.0, 91.0, 320.0, 18.0);
			ElementBounds powerToggleBounds = ElementBounds.Fixed(332.0, 89.0, 44.0, 24.0);
			ElementBounds powerGaugeLabelBounds = ElementBounds.Fixed(0.0, 121.0, 420.0, 18.0);
			ElementBounds powerGaugeBarBounds = ElementBounds.Fixed(0.0, 143.0, 420.0, 22.0);
			ElementBounds powerPercentBounds = ElementBounds.Fixed(0.0, 168.0, 420.0, 20.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			bgBounds.WithChildren(new ElementBounds[]
			{
				networkNameLabelBounds,
				networkNameInputBounds,
				networkNameSaveBounds,
				powerToggleLabelBounds,
				powerToggleBounds,
				powerGaugeLabelBounds,
				powerGaugeBarBounds,
				powerPercentBounds
			});
			this.powerGauge = new LinearGauge(this.capi, powerGaugeBarBounds, false);
			base.SingleComposer = GuiElementDynamicTextHelper.AddDynamicText(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSwitch(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddTextInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("switchboarddialog", dialogBounds), bgBounds, true, 5.0, 0.75f), UIUtils.I18n("Switchboard.Gui.Title"), new Action(this.OnTitleBarCloseClicked), null, null, null), UIUtils.I18n("Switchboard.Gui.NetworkName"), CairoFont.WhiteSmallText(), networkNameLabelBounds, null), networkNameInputBounds, new Action<string>(this.OnNetworkNameInputChanged), CairoFont.TextInput(), "networkNameInput"), UIUtils.I18n("Switchboard.Gui.Save"), new ActionConsumable(this.OnSaveNetworkNameClicked), networkNameSaveBounds, 2, null), UIUtils.I18n("Switchboard.Gui.UsePower"), CairoFont.WhiteSmallText(), powerToggleLabelBounds, null), new Action<bool>(this.OnUsePowerToggled), powerToggleBounds, "usePowerToggle", 24.0, 4.0), UIUtils.I18n("Switchboard.Gui.PowerGauge"), CairoFont.WhiteSmallText(), powerGaugeLabelBounds, null).AddInteractiveElement(this.powerGauge.Element, "powerGaugeBar"), "", CairoFont.WhiteSmallText().WithOrientation(2), powerPercentBounds, "powerPercentText").Compose(true);
			this.networkNameInput = (base.SingleComposer.GetElement("networkNameInput") as GuiElementTextInput);
			this.powerPercentText = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "powerPercentText");
			this.lastKnownServerCustomName = (this.switchboard.GetNetworkCustomNameForEditor() ?? "");
			this.pendingNetworkName = this.lastKnownServerCustomName;
			GuiElementTextInput guiElementTextInput = this.networkNameInput;
			if (guiElementTextInput != null)
			{
				guiElementTextInput.SetValue(this.pendingNetworkName, true);
			}
			GuiElementSwitch usePowerToggle = GuiComposerHelpers.GetSwitch(base.SingleComposer, "usePowerToggle");
			if (usePowerToggle != null)
			{
				usePowerToggle.On = this.switchboard.UsePowerRequirements;
			}
			this.RefreshData();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00013944 File Offset: 0x00011B44
		public void RefreshData()
		{
			if (base.SingleComposer == null)
			{
				return;
			}
			string customName = this.switchboard.GetNetworkCustomNameForEditor() ?? "";
			if (customName != this.lastKnownServerCustomName)
			{
				this.lastKnownServerCustomName = customName;
				this.pendingNetworkName = customName;
				GuiElementTextInput guiElementTextInput = this.networkNameInput;
				if (guiElementTextInput != null)
				{
					guiElementTextInput.SetValue(customName, true);
				}
			}
			float ratio = Math.Max(0f, Math.Min(1f, this.switchboard.PowerPercent));
			LinearGauge linearGauge = this.powerGauge;
			if (linearGauge != null)
			{
				linearGauge.SetRatio01(ratio);
			}
			int pct = (int)Math.Round((double)(ratio * 100f));
			GuiElementDynamicText guiElementDynamicText = this.powerPercentText;
			if (guiElementDynamicText == null)
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 1);
			defaultInterpolatedStringHandler.AppendFormatted<int>(pct);
			defaultInterpolatedStringHandler.AppendLiteral("%");
			guiElementDynamicText.SetNewText(defaultInterpolatedStringHandler.ToStringAndClear(), false, false, false);
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00013A16 File Offset: 0x00011C16
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00013A19 File Offset: 0x00011C19
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00013A22 File Offset: 0x00011C22
		private void OnNetworkNameInputChanged(string value)
		{
			this.pendingNetworkName = (value ?? "");
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00013A34 File Offset: 0x00011C34
		private bool OnSaveNetworkNameClicked()
		{
			string candidate = (this.pendingNetworkName ?? "").Trim();
			if (candidate.Length > 0 && WireNetworkHandler.IsNetworkNameTaken(this.switchboard.NetworkUID, candidate))
			{
				ICoreClientAPI capi = this.capi;
				if (capi != null)
				{
					capi.TriggerChatMessage(UIUtils.I18n("Switchboard.Settings.NameAlreadyUsed"));
				}
				return true;
			}
			this.switchboard.RequestRenameNetwork(this.pendingNetworkName);
			return true;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00013AA1 File Offset: 0x00011CA1
		private void OnUsePowerToggled(bool enabled)
		{
			this.switchboard.RequestSetUsePowerRequirements(enabled);
		}

		// Token: 0x040001C2 RID: 450
		private readonly BlockEntitySwitchboard switchboard;

		// Token: 0x040001C3 RID: 451
		private GuiElementTextInput networkNameInput;

		// Token: 0x040001C4 RID: 452
		private GuiElementDynamicText powerPercentText;

		// Token: 0x040001C5 RID: 453
		private LinearGauge powerGauge;

		// Token: 0x040001C6 RID: 454
		private string pendingNetworkName = "";

		// Token: 0x040001C7 RID: 455
		private string lastKnownServerCustomName = "";
	}
}
