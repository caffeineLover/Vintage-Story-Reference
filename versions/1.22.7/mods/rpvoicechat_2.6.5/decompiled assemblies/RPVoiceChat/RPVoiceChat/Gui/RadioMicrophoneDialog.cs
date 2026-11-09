using System;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200009C RID: 156
	public class RadioMicrophoneDialog : GuiDialogBlockEntity
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x000181CF File Offset: 0x000163CF
		public RadioMicrophoneDialog(ICoreClientAPI capi, BlockEntityRadioMicrophone microphone) : base(UIUtils.I18n("Radio.Microphone.Gui.Title"), microphone.Pos, capi)
		{
			this.microphone = microphone;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x000181FA File Offset: 0x000163FA
		public override void OnGuiOpened()
		{
			base.OnGuiOpened();
			this.BuildComposer();
			this.RefreshData();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0001820E File Offset: 0x0001640E
		public void RefreshData()
		{
			if (base.SingleComposer == null)
			{
				return;
			}
			this.BuildComposer();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00018220 File Offset: 0x00016420
		private string GetStatusText()
		{
			if (this.microphone.IsTransmitting)
			{
				if (RadioWireNetworkHelper.HasOnAirMixingConsole(this.microphone))
				{
					return UIUtils.I18n("Radio.Microphone.Gui.Status.ViaMixingConsole");
				}
				return UIUtils.I18n("Radio.Microphone.Gui.Status.On");
			}
			else
			{
				if (this.microphone.IsBusyForOtherPlayer(this.capi.World.Player.PlayerUID))
				{
					return UIUtils.I18n("Radio.Microphone.Gui.Status.Busy");
				}
				if (RadioWireNetworkHelper.HasOnAirMixingConsole(this.microphone))
				{
					return UIUtils.I18n("Radio.Microphone.Gui.Status.MixingConsoleOnAir");
				}
				return UIUtils.I18n("Radio.Microphone.Gui.Status.Off");
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000182AC File Offset: 0x000164AC
		private void BuildComposer()
		{
			this.actionButtonLangKey = (this.microphone.IsTransmitting ? "Radio.Microphone.Gui.TurnOff" : "Radio.Microphone.Gui.TurnOn");
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds statusBounds = ElementBounds.Fixed(0.0, 35.0, 420.0, 40.0);
			ElementBounds toggleBounds = ElementBounds.Fixed(0.0, 85.0, 160.0, 28.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			bgBounds.WithChildren(new ElementBounds[]
			{
				statusBounds,
				toggleBounds
			});
			base.SingleComposer = GuiComposerHelpers.AddSmallButton(GuiElementDynamicTextHelper.AddDynamicText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("radiomicrophone", dialogBounds), bgBounds, true, 5.0, 0.75f), UIUtils.I18n("Radio.Microphone.Gui.Title"), new Action(this.OnTitleBarCloseClicked), null, null, null), this.GetStatusText(), CairoFont.WhiteSmallText(), statusBounds, "radioMicStatusText"), UIUtils.I18n(this.actionButtonLangKey), new ActionConsumable(this.OnToggleClicked), toggleBounds, 2, null).Compose(true);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000183F0 File Offset: 0x000165F0
		private bool OnToggleClicked()
		{
			string playerUid = this.capi.World.Player.PlayerUID;
			bool enable = !this.microphone.IsTransmitting;
			if (enable && this.microphone.IsBusyForOtherPlayer(playerUid))
			{
				this.capi.TriggerIngameError(this, "radio-mic-busy", UIUtils.I18n("Radio.Microphone.Error.Busy"));
				return true;
			}
			if (!enable && !this.microphone.IsOperator(playerUid))
			{
				this.capi.TriggerIngameError(this, "radio-mic-not-operator", UIUtils.I18n("Radio.Microphone.Error.NotOperator"));
				return true;
			}
			this.microphone.RequestSetTransmitting(enable);
			return true;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001848B File Offset: 0x0001668B
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x04000247 RID: 583
		private readonly BlockEntityRadioMicrophone microphone;

		// Token: 0x04000248 RID: 584
		private string actionButtonLangKey = "Radio.Microphone.Gui.TurnOn";
	}
}
