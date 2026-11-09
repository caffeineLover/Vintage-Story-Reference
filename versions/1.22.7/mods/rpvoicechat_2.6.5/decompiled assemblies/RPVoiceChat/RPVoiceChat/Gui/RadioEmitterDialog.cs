using System;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200009B RID: 155
	public class RadioEmitterDialog : GuiDialogBlockEntity
	{
		// Token: 0x06000436 RID: 1078 RVA: 0x00017BA7 File Offset: 0x00015DA7
		public RadioEmitterDialog(ICoreClientAPI capi, BlockEntityRadioEmitter emitter) : base(UIUtils.I18n("Radio.Emitter.Gui.Title"), emitter.Pos, capi)
		{
			this.emitter = emitter;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00017BD2 File Offset: 0x00015DD2
		public override void OnGuiOpened()
		{
			base.OnGuiOpened();
			this.BuildComposer();
			this.RefreshData();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00017BE8 File Offset: 0x00015DE8
		public void RefreshData()
		{
			if (this.showingRepeaterControls != this.emitter.IsRepeaterMode)
			{
				this.BuildComposer();
			}
			GuiElementDynamicText guiElementDynamicText = this.statusText;
			if (guiElementDynamicText != null)
			{
				guiElementDynamicText.SetNewText(this.BuildStatusText(), false, false, false);
			}
			GuiElementDynamicText guiElementDynamicText2 = this.rangeText;
			if (guiElementDynamicText2 != null)
			{
				guiElementDynamicText2.SetNewText(this.BuildRangeText(), false, false, false);
			}
			if (this.emitter.IsRepeaterMode)
			{
				this.pendingRepeaterFrequency = this.emitter.RepeaterFrequency;
				GuiElementTextInput guiElementTextInput = this.repeaterFrequencyInput;
				if (guiElementTextInput == null)
				{
					return;
				}
				guiElementTextInput.SetValue(this.pendingRepeaterFrequency, true);
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00017C78 File Offset: 0x00015E78
		private string BuildStatusText()
		{
			string mode = this.emitter.IsRepeaterMode ? UIUtils.I18n("Radio.Emitter.Mode.Repeater") : UIUtils.I18n("Radio.Emitter.Mode.WiredSource");
			int power = (int)Math.Round((double)(this.emitter.PowerPercent * 100f));
			int minPower = ServerConfigManager.RadioNetworkMinPowerPercent;
			string frequency = this.emitter.IsRepeaterMode ? (string.IsNullOrWhiteSpace(this.emitter.RepeaterFrequency) ? UIUtils.I18n("Radio.Emitter.Gui.NoConsole") : this.emitter.RepeaterFrequency) : (string.IsNullOrWhiteSpace(this.emitter.GetConsoleFrequency()) ? UIUtils.I18n("Radio.Emitter.Gui.NoConsole") : this.emitter.GetConsoleFrequency());
			string name = this.emitter.GetConsoleDisplayName();
			string status = UIUtils.I18n("Radio.Emitter.Gui.Status", new object[]
			{
				mode,
				power,
				frequency,
				name
			});
			if (!this.emitter.HasSufficientTransmitPower())
			{
				status = status + "\n" + UIUtils.I18n("Radio.Emitter.Gui.InsufficientPower", new object[]
				{
					minPower
				});
			}
			return status;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00017D95 File Offset: 0x00015F95
		private string BuildRangeText()
		{
			return UIUtils.I18n("Radio.Emitter.Gui.Range", new object[]
			{
				this.emitter.GetEffectiveTransmitRangeBlocks()
			});
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00017DBA File Offset: 0x00015FBA
		private string BuildModeHintText()
		{
			if (!this.emitter.IsRepeaterMode)
			{
				return UIUtils.I18n("Radio.Emitter.Gui.WiredFrequencyHint");
			}
			return UIUtils.I18n("Radio.Emitter.Gui.RepeaterFrequencyHint");
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00017DE0 File Offset: 0x00015FE0
		private void BuildComposer()
		{
			this.showingRepeaterControls = this.emitter.IsRepeaterMode;
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds rangeBounds = ElementBounds.Fixed(0.0, 35.0, 420.0, 22.0);
			ElementBounds statusBounds = ElementBounds.Fixed(0.0, 76.0, 420.0, 120.0);
			ElementBounds wiredButtonBounds = ElementBounds.Fixed(0.0, 208.0, 204.0, 28.0);
			ElementBounds repeaterButtonBounds = ElementBounds.Fixed(216.0, 208.0, 204.0, 28.0);
			ElementBounds modeHintBounds = ElementBounds.Fixed(0.0, 244.0, 420.0, 36.0);
			ElementBounds repeaterLabelBounds = ElementBounds.Fixed(0.0, 288.0, 420.0, 18.0);
			ElementBounds repeaterInputBounds = ElementBounds.Fixed(0.0, 308.0, 320.0, 26.0);
			ElementBounds repeaterSaveBounds = ElementBounds.Fixed(332.0, 308.0, 88.0, 26.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			if (this.showingRepeaterControls)
			{
				bgBounds.WithChildren(new ElementBounds[]
				{
					rangeBounds,
					statusBounds,
					wiredButtonBounds,
					repeaterButtonBounds,
					modeHintBounds,
					repeaterLabelBounds,
					repeaterInputBounds,
					repeaterSaveBounds
				});
			}
			else
			{
				bgBounds.WithChildren(new ElementBounds[]
				{
					rangeBounds,
					statusBounds,
					wiredButtonBounds,
					repeaterButtonBounds,
					modeHintBounds
				});
			}
			this.pendingRepeaterFrequency = this.emitter.RepeaterFrequency;
			GuiComposer composer = GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiElementDynamicTextHelper.AddDynamicText(GuiElementDynamicTextHelper.AddDynamicText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("radioemitter", dialogBounds), bgBounds, true, 5.0, 0.75f), UIUtils.I18n("Radio.Emitter.Gui.Title"), new Action(this.OnTitleBarCloseClicked), null, null, null), this.BuildRangeText(), CairoFont.WhiteSmallishText(), rangeBounds, "radioEmitterRange"), this.BuildStatusText(), CairoFont.WhiteSmallText(), statusBounds, "radioEmitterStatus"), UIUtils.I18n("Radio.Emitter.Mode.WiredSource"), new ActionConsumable(this.OnWiredSourceClicked), wiredButtonBounds, 2, null), UIUtils.I18n("Radio.Emitter.Mode.Repeater"), new ActionConsumable(this.OnRepeaterClicked), repeaterButtonBounds, 2, null), this.BuildModeHintText(), CairoFont.WhiteSmallText(), modeHintBounds, null);
			if (this.showingRepeaterControls)
			{
				GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddTextInput(GuiComposerHelpers.AddStaticText(composer, UIUtils.I18n("Radio.Emitter.Gui.RepeaterFrequency"), CairoFont.WhiteSmallText(), repeaterLabelBounds, null), repeaterInputBounds, delegate(string value)
				{
					this.pendingRepeaterFrequency = (value ?? "");
				}, CairoFont.TextInput(), "radioEmitterRepeaterFrequency"), UIUtils.I18n("Radio.Gui.Save"), new ActionConsumable(this.OnSaveRepeaterFrequencyClicked), repeaterSaveBounds, 2, null);
			}
			base.SingleComposer = composer.Compose(true);
			this.rangeText = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "radioEmitterRange");
			this.statusText = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "radioEmitterStatus");
			this.repeaterFrequencyInput = (this.showingRepeaterControls ? GuiComposerHelpers.GetTextInput(base.SingleComposer, "radioEmitterRepeaterFrequency") : null);
			GuiElementTextInput guiElementTextInput = this.repeaterFrequencyInput;
			if (guiElementTextInput == null)
			{
				return;
			}
			guiElementTextInput.SetValue(this.pendingRepeaterFrequency, true);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00018182 File Offset: 0x00016382
		private bool OnWiredSourceClicked()
		{
			this.emitter.RequestSetOperatingMode(RadioEmitterOperatingMode.WiredSource);
			return true;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00018191 File Offset: 0x00016391
		private bool OnRepeaterClicked()
		{
			this.emitter.RequestSetOperatingMode(RadioEmitterOperatingMode.Repeater);
			return true;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000181A0 File Offset: 0x000163A0
		private bool OnSaveRepeaterFrequencyClicked()
		{
			this.emitter.RequestSetRepeaterFrequency(this.pendingRepeaterFrequency);
			return true;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000181B4 File Offset: 0x000163B4
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x04000241 RID: 577
		private readonly BlockEntityRadioEmitter emitter;

		// Token: 0x04000242 RID: 578
		private GuiElementDynamicText statusText;

		// Token: 0x04000243 RID: 579
		private GuiElementDynamicText rangeText;

		// Token: 0x04000244 RID: 580
		private GuiElementTextInput repeaterFrequencyInput;

		// Token: 0x04000245 RID: 581
		private string pendingRepeaterFrequency = "";

		// Token: 0x04000246 RID: 582
		private bool showingRepeaterControls;
	}
}
