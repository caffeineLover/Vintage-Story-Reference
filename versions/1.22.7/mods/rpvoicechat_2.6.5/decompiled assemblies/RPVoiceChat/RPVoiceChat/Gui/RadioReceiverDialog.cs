using System;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200009E RID: 158
	public class RadioReceiverDialog : GuiDialogBlockEntity
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x00018975 File Offset: 0x00016B75
		public RadioReceiverDialog(ICoreClientAPI capi, BlockEntityRadioReceiver receiver) : base(UIUtils.I18n("Radio.Receiver.Gui.Title"), receiver.Pos, capi)
		{
			this.receiver = receiver;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000189AB File Offset: 0x00016BAB
		public override void OnGuiOpened()
		{
			base.OnGuiOpened();
			this.BuildComposer();
			this.pendingFrequency = this.receiver.TunedFrequency;
			GuiElementTextInput guiElementTextInput = this.frequencyInput;
			if (guiElementTextInput != null)
			{
				guiElementTextInput.SetValue(this.pendingFrequency, true);
			}
			this.RefreshData();
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000189E8 File Offset: 0x00016BE8
		public void RefreshData()
		{
			this.pendingFrequency = this.receiver.TunedFrequency;
			if (base.SingleComposer == null)
			{
				return;
			}
			GuiElementTextInput guiElementTextInput = this.frequencyInput;
			if (guiElementTextInput != null)
			{
				guiElementTextInput.SetValue(this.pendingFrequency, true);
			}
			NamedSlider namedSlider = this.rangeSlider;
			if (namedSlider != null)
			{
				namedSlider.SetValues(this.receiver.PlaybackRangeBlocks, 0, 15, 1, "");
			}
			NamedSlider namedSlider2 = this.volumeSlider;
			if (namedSlider2 != null)
			{
				namedSlider2.SetValues(this.receiver.PlaybackVolumePercent, 0, 100, 1, "%");
			}
			GuiElementDynamicText dynamicText = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "radioReceiverStationText");
			if (dynamicText != null)
			{
				dynamicText.SetNewText(this.GetStationText(), false, false, false);
			}
			if ((this.receiver.IsEnabled ? "Radio.Receiver.Gui.TurnOff" : "Radio.Receiver.Gui.TurnOn") != this.actionButtonLangKey)
			{
				this.BuildComposer();
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00018AC4 File Offset: 0x00016CC4
		private string GetStationText()
		{
			string heardStationName = this.receiver.HeardStationName;
			string name = ((heardStationName != null) ? heardStationName.Trim() : null) ?? "";
			if (name.Length == 0)
			{
				return UIUtils.I18n("Radio.Receiver.Gui.Station.None");
			}
			return UIUtils.I18n("Radio.Receiver.Gui.Station", new object[]
			{
				name
			});
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00018B1C File Offset: 0x00016D1C
		private void BuildComposer()
		{
			this.actionButtonLangKey = (this.receiver.IsEnabled ? "Radio.Receiver.Gui.TurnOff" : "Radio.Receiver.Gui.TurnOn");
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds frequencyLabelBounds = ElementBounds.Fixed(0.0, 35.0, 420.0, 18.0);
			ElementBounds frequencyInputBounds = ElementBounds.Fixed(0.0, 55.0, 320.0, 26.0);
			ElementBounds frequencySaveBounds = ElementBounds.Fixed(332.0, 55.0, 88.0, 26.0);
			ElementBounds stationBounds = ElementBounds.Fixed(0.0, 90.0, 420.0, 22.0);
			ElementBounds toggleBounds = ElementBounds.Fixed(0.0, 120.0, 160.0, 28.0);
			ElementBounds rangeLabelBounds = ElementBounds.Fixed(0.0, 160.0, 420.0, 18.0);
			ElementBounds rangeSliderBounds = ElementBounds.Fixed(0.0, 180.0, 420.0, 26.0);
			ElementBounds volumeLabelBounds = ElementBounds.Fixed(0.0, 220.0, 420.0, 18.0);
			ElementBounds volumeSliderBounds = ElementBounds.Fixed(0.0, 240.0, 420.0, 26.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			bgBounds.WithChildren(new ElementBounds[]
			{
				frequencyLabelBounds,
				frequencyInputBounds,
				frequencySaveBounds,
				stationBounds,
				toggleBounds,
				rangeLabelBounds,
				rangeSliderBounds,
				volumeLabelBounds,
				volumeSliderBounds
			});
			this.rangeSlider = new NamedSlider(this.capi, "radioReceiverRangeSlider", new ActionConsumable<int, string>(this.OnPlaybackRangeChanged), rangeSliderBounds);
			this.rangeSlider.SetValues(this.receiver.PlaybackRangeBlocks, 0, 15, 1, "");
			this.volumeSlider = new NamedSlider(this.capi, "radioReceiverVolumeSlider", new ActionConsumable<int, string>(this.OnPlaybackVolumeChanged), volumeSliderBounds);
			this.volumeSlider.SetValues(this.receiver.PlaybackVolumePercent, 0, 100, 1, "%");
			base.SingleComposer = GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSmallButton(GuiElementDynamicTextHelper.AddDynamicText(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddTextInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("radioreceiver", dialogBounds), bgBounds, true, 5.0, 0.75f), UIUtils.I18n("Radio.Receiver.Gui.Title"), new Action(this.OnTitleBarCloseClicked), null, null, null), UIUtils.I18n("Radio.Receiver.Gui.Frequency"), CairoFont.WhiteSmallText(), frequencyLabelBounds, null), frequencyInputBounds, new Action<string>(this.OnFrequencyChanged), CairoFont.TextInput(), "radioReceiverFrequencyInput"), UIUtils.I18n("Radio.Gui.Save"), new ActionConsumable(this.OnSaveFrequencyClicked), frequencySaveBounds, 2, null), this.GetStationText(), CairoFont.WhiteSmallText(), stationBounds, "radioReceiverStationText"), UIUtils.I18n(this.actionButtonLangKey), new ActionConsumable(this.OnToggleEnabledClicked), toggleBounds, 2, null), UIUtils.I18n("Radio.Receiver.Gui.PlaybackRange"), CairoFont.WhiteSmallText(), rangeLabelBounds, null).AddInteractiveElement(this.rangeSlider, "radioReceiverRangeSlider"), UIUtils.I18n("Radio.Receiver.Gui.PlaybackVolume"), CairoFont.WhiteSmallText(), volumeLabelBounds, null).AddInteractiveElement(this.volumeSlider, "radioReceiverVolumeSlider").Compose(true);
			this.frequencyInput = GuiComposerHelpers.GetTextInput(base.SingleComposer, "radioReceiverFrequencyInput");
			GuiElementTextInput guiElementTextInput = this.frequencyInput;
			if (guiElementTextInput == null)
			{
				return;
			}
			guiElementTextInput.SetValue(this.pendingFrequency, true);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00018F04 File Offset: 0x00017104
		private void OnFrequencyChanged(string value)
		{
			this.pendingFrequency = (value ?? "");
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00018F16 File Offset: 0x00017116
		private bool OnSaveFrequencyClicked()
		{
			this.receiver.RequestSetFrequency(this.pendingFrequency);
			return true;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00018F2A File Offset: 0x0001712A
		private bool OnToggleEnabledClicked()
		{
			this.receiver.RequestSetEnabled(!this.receiver.IsEnabled);
			return true;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00018F46 File Offset: 0x00017146
		private bool OnPlaybackRangeChanged(int value, string _)
		{
			this.receiver.RequestSetPlaybackRange(value);
			return true;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00018F55 File Offset: 0x00017155
		private bool OnPlaybackVolumeChanged(int value, string _)
		{
			this.receiver.RequestSetPlaybackVolume(value);
			return true;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00018F64 File Offset: 0x00017164
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x0400024D RID: 589
		private const string RangeSliderKey = "radioReceiverRangeSlider";

		// Token: 0x0400024E RID: 590
		private const string VolumeSliderKey = "radioReceiverVolumeSlider";

		// Token: 0x0400024F RID: 591
		private readonly BlockEntityRadioReceiver receiver;

		// Token: 0x04000250 RID: 592
		private GuiElementTextInput frequencyInput;

		// Token: 0x04000251 RID: 593
		private NamedSlider rangeSlider;

		// Token: 0x04000252 RID: 594
		private NamedSlider volumeSlider;

		// Token: 0x04000253 RID: 595
		private string pendingFrequency = "";

		// Token: 0x04000254 RID: 596
		private string actionButtonLangKey = "Radio.Receiver.Gui.TurnOn";
	}
}
