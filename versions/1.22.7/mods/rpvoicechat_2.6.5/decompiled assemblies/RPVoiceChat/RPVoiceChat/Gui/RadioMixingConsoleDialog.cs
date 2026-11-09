using System;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200009D RID: 157
	public class RadioMixingConsoleDialog : GuiDialogBlockEntity
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x00018494 File Offset: 0x00016694
		public RadioMixingConsoleDialog(ICoreClientAPI capi, BlockEntityRadioMixingConsole mixingConsole) : base(UIUtils.I18n("Radio.MixingConsole.Gui.Title"), mixingConsole.Pos, capi)
		{
			this.mixingConsole = mixingConsole;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000184CA File Offset: 0x000166CA
		public override void OnGuiOpened()
		{
			base.OnGuiOpened();
			this.BuildComposer();
			this.RefreshData();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000184DE File Offset: 0x000166DE
		public void RefreshData()
		{
			if (base.SingleComposer == null)
			{
				return;
			}
			this.pendingHlsUrl = this.mixingConsole.HlsStreamUrl;
			GuiElementTextInput guiElementTextInput = this.hlsUrlInput;
			if (guiElementTextInput != null)
			{
				guiElementTextInput.SetValue(this.pendingHlsUrl, true);
			}
			this.BuildComposer();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00018518 File Offset: 0x00016718
		private string GetStatusText()
		{
			if (this.mixingConsole.IsOnAir && !this.mixingConsole.HasActiveBroadcastOutput())
			{
				return UIUtils.I18n("Radio.MixingConsole.Gui.Status.WaitingPower");
			}
			if (this.mixingConsole.IsBusyForOtherPlayer(this.capi.World.Player.PlayerUID))
			{
				return UIUtils.I18n("Radio.MixingConsole.Gui.Status.Busy");
			}
			return "";
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0001857C File Offset: 0x0001677C
		private void BuildComposer()
		{
			this.actionButtonLangKey = (this.mixingConsole.IsOnAir ? "Radio.MixingConsole.Gui.TurnOff" : "Radio.MixingConsole.Gui.TurnOn");
			string statusText = this.GetStatusText();
			bool showStatus = statusText.Length > 0;
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds urlLabelBounds = ElementBounds.Fixed(0.0, 35.0, 520.0, 18.0);
			ElementBounds urlInputBounds = ElementBounds.Fixed(0.0, 55.0, 400.0, 26.0);
			ElementBounds urlSaveBounds = ElementBounds.Fixed(412.0, 55.0, 88.0, 26.0);
			ElementBounds statusBounds = ElementBounds.Fixed(0.0, 90.0, 520.0, 36.0);
			ElementBounds toggleBounds = ElementBounds.Fixed(0.0, (double)(showStatus ? 132 : 95), 180.0, 28.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			if (showStatus)
			{
				bgBounds.WithChildren(new ElementBounds[]
				{
					urlLabelBounds,
					urlInputBounds,
					urlSaveBounds,
					statusBounds,
					toggleBounds
				});
			}
			else
			{
				bgBounds.WithChildren(new ElementBounds[]
				{
					urlLabelBounds,
					urlInputBounds,
					urlSaveBounds,
					toggleBounds
				});
			}
			GuiComposer composer = GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddTextInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("radiomixingconsole", dialogBounds), bgBounds, true, 5.0, 0.75f), UIUtils.I18n("Radio.MixingConsole.Gui.Title"), new Action(this.OnTitleBarCloseClicked), null, null, null), UIUtils.I18n("Radio.MixingConsole.Gui.HlsUrl"), CairoFont.WhiteSmallText(), urlLabelBounds, null), urlInputBounds, new Action<string>(this.OnHlsUrlChanged), CairoFont.TextInput(), "radioMixingHlsUrlInput"), UIUtils.I18n("Radio.Gui.Save"), new ActionConsumable(this.OnSaveHlsUrlClicked), urlSaveBounds, 2, null);
			if (showStatus)
			{
				GuiElementDynamicTextHelper.AddDynamicText(composer, statusText, CairoFont.WhiteSmallText(), statusBounds, "radioMixingStatusText");
			}
			base.SingleComposer = GuiComposerHelpers.AddSmallButton(composer, UIUtils.I18n(this.actionButtonLangKey), new ActionConsumable(this.OnToggleOnAirClicked), toggleBounds, 2, null).Compose(true);
			this.hlsUrlInput = GuiComposerHelpers.GetTextInput(base.SingleComposer, "radioMixingHlsUrlInput");
			GuiElementTextInput guiElementTextInput = this.hlsUrlInput;
			if (guiElementTextInput == null)
			{
				return;
			}
			guiElementTextInput.SetValue(this.pendingHlsUrl, true);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00018815 File Offset: 0x00016A15
		private void OnHlsUrlChanged(string value)
		{
			this.pendingHlsUrl = (value ?? "");
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00018828 File Offset: 0x00016A28
		private bool OnSaveHlsUrlClicked()
		{
			if (BlockEntityRadioMixingConsole.NormalizeHlsUrl(this.pendingHlsUrl) == null && !string.IsNullOrWhiteSpace(this.pendingHlsUrl))
			{
				this.capi.TriggerIngameError(this, "radio-hls-url-invalid", UIUtils.I18n("Radio.MixingConsole.Error.InvalidUrl"));
				return true;
			}
			this.mixingConsole.RequestSetHlsUrl(this.pendingHlsUrl);
			return true;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00018880 File Offset: 0x00016A80
		private bool OnToggleOnAirClicked()
		{
			string playerUid = this.capi.World.Player.PlayerUID;
			bool enable = !this.mixingConsole.IsOnAir;
			if (enable)
			{
				if (this.mixingConsole.IsBusyForOtherPlayer(playerUid))
				{
					this.capi.TriggerIngameError(this, "radio-mixing-busy", UIUtils.I18n("Radio.MixingConsole.Error.Busy"));
					return true;
				}
				if (this.mixingConsole.NetworkUID == 0L)
				{
					this.capi.TriggerIngameError(this, "radio-mixing-not-wired", UIUtils.I18n("Radio.MixingConsole.Error.NotWired"));
					return true;
				}
				if (!this.mixingConsole.HasWiredBroadcastPath())
				{
					this.capi.TriggerIngameError(this, "radio-mixing-no-path", UIUtils.I18n("Radio.MixingConsole.Error.NoBroadcastPath"));
					return true;
				}
			}
			else if (!this.mixingConsole.IsOperator(playerUid))
			{
				this.capi.TriggerIngameError(this, "radio-mixing-not-operator", UIUtils.I18n("Radio.MixingConsole.Error.NotOperator"));
				return true;
			}
			this.mixingConsole.RequestSetOnAir(enable);
			return true;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001896C File Offset: 0x00016B6C
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x04000249 RID: 585
		private readonly BlockEntityRadioMixingConsole mixingConsole;

		// Token: 0x0400024A RID: 586
		private GuiElementTextInput hlsUrlInput;

		// Token: 0x0400024B RID: 587
		private string pendingHlsUrl = "";

		// Token: 0x0400024C RID: 588
		private string actionButtonLangKey = "Radio.MixingConsole.Gui.TurnOn";
	}
}
