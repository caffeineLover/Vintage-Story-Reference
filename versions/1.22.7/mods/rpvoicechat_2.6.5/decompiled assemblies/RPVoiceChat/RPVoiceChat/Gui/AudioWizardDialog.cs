using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200008C RID: 140
	public class AudioWizardDialog : GuiDialog
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000394 RID: 916 RVA: 0x0001337C File Offset: 0x0001157C
		// (remove) Token: 0x06000395 RID: 917 RVA: 0x000133B4 File Offset: 0x000115B4
		public event Action GainCalibrationDone;

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000396 RID: 918 RVA: 0x000133E9 File Offset: 0x000115E9
		public override double DrawOrder
		{
			get
			{
				return 0.11;
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000133F4 File Offset: 0x000115F4
		public AudioWizardDialog(ICoreClientAPI capi, MicrophoneManager audioInputManager, AudioOutputManager audioOutputManager) : base(capi)
		{
			this.audioInputManager = audioInputManager;
			this.audioOutputManager = audioOutputManager;
			this.doneDialog = new AudioWizardDialog.AudioWizardDoneDialog(capi);
			this.doneDialog.OnClosed += new Action(this.SaveAndExit);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00013430 File Offset: 0x00011630
		public override bool TryOpen()
		{
			this.configurationCTS = new CancellationTokenSource();
			this.adjustedGain = ModConfig.ClientConfig.InputGain;
			this.adjustedThreshold = ModConfig.ClientConfig.InputThreshold;
			this.noMicrophoneMode = !this.audioInputManager.CanUseMicrophoneCapture();
			if (this.noMicrophoneMode)
			{
				this.ComposeNoMicrophone();
				return base.TryOpen();
			}
			this.audioInputManager.AudioWizardActive = true;
			if (ModConfig.ClientConfig.InputGain == 0f)
			{
				this.audioInputManager.SetGain(1f);
			}
			ModConfig.ClientConfig.Loopback = true;
			this.audioOutputManager.IsLoopbackEnabled = true;
			this.Compose();
			return base.TryOpen();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000134E4 File Offset: 0x000116E4
		public override bool TryClose()
		{
			CancellationTokenSource cancellationTokenSource = this.configurationCTS;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			CancellationTokenSource cancellationTokenSource2 = this.configurationCTS;
			if (cancellationTokenSource2 != null)
			{
				cancellationTokenSource2.Dispose();
			}
			this.configurationInProcess = false;
			if (!this.noMicrophoneMode)
			{
				ModConfig.ClientConfig.InputGain = this.adjustedGain;
				ModConfig.ClientConfig.InputThreshold = this.adjustedThreshold;
				this.audioInputManager.SetGain(this.adjustedGain);
				this.audioInputManager.SetThreshold(this.adjustedThreshold);
				if (!this.doneDialog.IsOpened())
				{
					this.SaveAndExit();
				}
			}
			this.noMicrophoneMode = false;
			return base.TryClose();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00013584 File Offset: 0x00011784
		private void ComposeNoMicrophone()
		{
			TextDrawUtil textDrawUtil = new TextDrawUtil();
			CairoFont font = CairoFont.WhiteSmallText();
			string titleBarText = UIUtils.I18n("Gui.AudioWizardDialog.TitleBar");
			string bodyText = UIUtils.I18n("Gui.AudioWizardDialog.NoMicrophone");
			string skipButtonText = UIUtils.I18n("Gui.AudioWizardDialog.Skip");
			double bodyHeight = textDrawUtil.GetMultilineTextHeight(font, bodyText, 460.0, 0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding).WithSizing(2);
			ElementBounds bodyBounds = ElementBounds.Fixed(5.0, GuiStyle.TitleBarHeight + 5.0, 460.0, bodyHeight);
			ElementBounds buttonBounds = bodyBounds.BelowCopy(0.0, 15.0, 0.0, 0.0).WithFixedSize(0.0, 30.0).WithFixedPadding(10.0, 2.0).WithAlignment(8);
			base.SingleComposer = GuiComposerHelpers.AddButton(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("RPVC_AudioWizardDialog", ElementStdBounds.AutosizedMainDialog), bgBounds, true, 5.0, 0.75f), titleBarText, delegate()
			{
				this.TryClose();
			}, null, null, null).BeginChildElements(bgBounds), bodyText, font, bodyBounds, null), skipButtonText, () => this.TryClose(), buttonBounds, 2, null).EndChildElements().Compose(true);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000136F0 File Offset: 0x000118F0
		private void Compose()
		{
			TextDrawUtil textDrawUtil = new TextDrawUtil();
			CairoFont font = CairoFont.WhiteSmallText();
			string[] dropdownValues = this.audioInputManager.GetInputDeviceNames();
			string titleBarText = UIUtils.I18n("Gui.AudioWizardDialog.TitleBar");
			string firstTextBlock = UIUtils.I18n("Gui.AudioWizardDialog.FirstParagraph");
			string secondTextBlock = UIUtils.I18n("Gui.AudioWizardDialog.SecondParagraph");
			string startButtonText = Lang.Get("Start", Array.Empty<object>());
			string skipButtonText = UIUtils.I18n("Gui.AudioWizardDialog.Skip");
			double firstTextBlockHeight = textDrawUtil.GetMultilineTextHeight(font, firstTextBlock, 460.0, 0);
			double secondTextBlockHeight = textDrawUtil.GetMultilineTextHeight(font, secondTextBlock, 460.0, 0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding).WithSizing(2);
			ElementBounds firstTextBlockBounds = ElementBounds.Fixed(5.0, GuiStyle.TitleBarHeight + 5.0, 460.0, firstTextBlockHeight);
			ElementBounds dropdownBounds = firstTextBlockBounds.BelowCopy(0.0, 15.0, 0.0, 0.0).WithFixedHeight(30.0);
			ElementBounds secondTextBlockBounds = dropdownBounds.BelowCopy(0.0, 15.0, 0.0, 0.0).WithFixedHeight(secondTextBlockHeight);
			ElementBounds progressBarBounds = secondTextBlockBounds.BelowCopy(-5.0, 15.0, 0.0, 0.0).WithFixedHeight(30.0);
			ElementBounds statusTextBounds = progressBarBounds.BelowCopy(0.0, 8.0, 0.0, 0.0).WithFixedHeight(30.0);
			ElementBounds buttonBounds = statusTextBounds.BelowCopy(0.0, 15.0, 0.0, 0.0).WithFixedSize(0.0, 30.0).WithFixedPadding(10.0, 2.0);
			GuiElementStatbar progressBar = new GuiElementStatbar(this.capi, progressBarBounds, new double[]
			{
				0.1,
				0.4,
				0.1
			}, false, false);
			progressBar.ShowValueOnHover = false;
			base.SingleComposer = GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(GuiElementDynamicTextHelper.AddDynamicText(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDropDown(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("RPVC_AudioWizardDialog", ElementStdBounds.AutosizedMainDialog), bgBounds, true, 5.0, 0.75f), titleBarText, delegate()
			{
				this.TryClose();
			}, null, null, null).BeginChildElements(bgBounds), firstTextBlock, font, firstTextBlockBounds, null), dropdownValues, dropdownValues, 0, new SelectionChangedDelegate(this.OnDropdownSelect), dropdownBounds, "inputDevice"), secondTextBlock, font, secondTextBlockBounds, null).AddInteractiveElement(progressBar, "progressBar"), "", CairoFont.WhiteSmallText(), statusTextBounds, "wizardStatusText"), startButtonText, new ActionConsumable(this.OnStartButtonClick), buttonBounds, 2, null), skipButtonText, () => this.TryClose(), buttonBounds.FlatCopy().WithAlignment(12), 2, null).EndChildElements().Compose(true);
			progressBar.SetValues(0f, 0f, 160f);
			progressBar.SetLineInterval(16f);
			this.wizardStatusText = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "wizardStatusText");
			GuiComposerHelpers.GetDropDown(base.SingleComposer, "inputDevice").SetSelectedValue(new string[]
			{
				ModConfig.ClientConfig.InputDevice ?? "Default"
			});
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00013A78 File Offset: 0x00011C78
		private bool OnStartButtonClick()
		{
			if (this.configurationInProcess || this.noMicrophoneMode)
			{
				return true;
			}
			if (!this.audioInputManager.CanUseMicrophoneCapture())
			{
				GuiElementDynamicText guiElementDynamicText = this.wizardStatusText;
				if (guiElementDynamicText != null)
				{
					guiElementDynamicText.SetNewText(UIUtils.I18n("Gui.AudioWizardDialog.NoMicrophone"), false, false, false);
				}
				return true;
			}
			this.configurationInProcess = true;
			GuiElementDynamicText guiElementDynamicText2 = this.wizardStatusText;
			if (guiElementDynamicText2 != null)
			{
				guiElementDynamicText2.SetNewText(UIUtils.I18n("Gui.AudioWizardDialog.Status.CalibratingGain"), false, false, false);
			}
			float maxGain = AudioUtils.DBsToFactor(20f);
			this.audioInputManager.SetGain(maxGain);
			this.audioInputManager.ClearCalibrationSamples();
			this.StartCalibration();
			return true;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00013B14 File Offset: 0x00011D14
		private void StartCalibration()
		{
			AudioWizardDialog.<StartCalibration>d__35 <StartCalibration>d__;
			<StartCalibration>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<StartCalibration>d__.<>4__this = this;
			<StartCalibration>d__.<>1__state = -1;
			<StartCalibration>d__.<>t__builder.Start<AudioWizardDialog.<StartCalibration>d__35>(ref <StartCalibration>d__);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00013B4C File Offset: 0x00011D4C
		private void SetProgressValue(int step)
		{
			if (!this.IsOpened() || base.SingleComposer == null)
			{
				return;
			}
			GuiElementStatbar progressBar = GuiComposerHelpers.GetStatbar(base.SingleComposer, "progressBar");
			if (progressBar != null)
			{
				progressBar.SetValue((float)step);
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00013B88 File Offset: 0x00011D88
		private Task RunOnMainThread(Action action)
		{
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			this.capi.Event.EnqueueMainThreadTask(delegate()
			{
				try
				{
					action.Invoke();
					tcs.TrySetResult(true);
				}
				catch (Exception ex)
				{
					tcs.TrySetException(ex);
				}
			}, "rpvoicechat:AudioWizard");
			return tcs.Task;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00013BDC File Offset: 0x00011DDC
		private static float ComputeCalibratedGain(List<float> effectiveGains)
		{
			if (effectiveGains == null || effectiveGains.Count == 0)
			{
				return ModConfig.ClientConfig.InputGain;
			}
			effectiveGains.Sort();
			return AudioUtils.DBsToFactor(GameMath.Clamp(AudioUtils.FactorToDBs(effectiveGains[effectiveGains.Count / 4]), -20f, 20f));
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00013C2C File Offset: 0x00011E2C
		private float ComputeCalibratedThreshold(List<double> amplitudes)
		{
			if (amplitudes == null || amplitudes.Count == 0)
			{
				return ModConfig.ClientConfig.InputThreshold;
			}
			amplitudes.Sort();
			int count = amplitudes.Count;
			double noiseFloor = amplitudes[Math.Max(0, count / 10)];
			double speechLevel = amplitudes[Math.Min(count - 1, count * 6 / 10)];
			double absoluteThreshold = noiseFloor + (speechLevel - noiseFloor) * 0.35;
			absoluteThreshold = Math.Min(absoluteThreshold, speechLevel * 0.55);
			double maxThreshold = this.audioInputManager.GetMaxInputThreshold();
			if (maxThreshold <= 0.0)
			{
				return ModConfig.ClientConfig.InputThreshold;
			}
			return GameMath.Clamp((float)(absoluteThreshold / maxThreshold), 0.08f, 0.75f);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00013CE0 File Offset: 0x00011EE0
		private void SaveAndExit()
		{
			this.audioInputManager.AudioWizardActive = false;
			ModConfig.ClientConfig.Loopback = false;
			this.audioOutputManager.IsLoopbackEnabled = false;
			ModConfig.ClientConfig.InputGain = this.adjustedGain;
			ModConfig.ClientConfig.InputThreshold = this.adjustedThreshold;
			this.audioInputManager.SetGain(this.adjustedGain);
			this.audioInputManager.SetThreshold(this.adjustedThreshold);
			ModConfig.SaveClient(this.capi);
			Action gainCalibrationDone = this.GainCalibrationDone;
			if (gainCalibrationDone == null)
			{
				return;
			}
			gainCalibrationDone.Invoke();
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00013D70 File Offset: 0x00011F70
		private void OnDropdownSelect(string value, bool selected)
		{
			this.audioInputManager.SetInputDevice(value);
			if (!this.IsOpened() || base.SingleComposer == null)
			{
				return;
			}
			GuiElementDropDown dropDown = GuiComposerHelpers.GetDropDown(base.SingleComposer, "inputDevice");
			if (dropDown == null)
			{
				return;
			}
			dropDown.SetSelectedValue(new string[]
			{
				ModConfig.ClientConfig.InputDevice ?? "Default"
			});
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00013DD0 File Offset: 0x00011FD0
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00013DD3 File Offset: 0x00011FD3
		public override void Dispose()
		{
			GuiDialog guiDialog = this.doneDialog;
			if (guiDialog != null)
			{
				guiDialog.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x040001C2 RID: 450
		private const string i18nPrefix = "Gui.AudioWizardDialog";

		// Token: 0x040001C3 RID: 451
		private const string composerName = "RPVC_AudioWizardDialog";

		// Token: 0x040001C4 RID: 452
		private const int textYOffset = 5;

		// Token: 0x040001C5 RID: 453
		private const int textLeftPadding = 5;

		// Token: 0x040001C6 RID: 454
		private const int textBottomPadding = 15;

		// Token: 0x040001C7 RID: 455
		private const int textWidth = 460;

		// Token: 0x040001C8 RID: 456
		private const int defaultElementHeight = 30;

		// Token: 0x040001C9 RID: 457
		private const int buttonXPadding = 10;

		// Token: 0x040001CA RID: 458
		private const int buttonYPadding = 2;

		// Token: 0x040001CB RID: 459
		private const int gainCalibrationDuration = 4000;

		// Token: 0x040001CC RID: 460
		private const int thresholdCalibrationDuration = 4000;

		// Token: 0x040001CD RID: 461
		private const int calibrationUpdateInterval = 50;

		// Token: 0x040001CE RID: 462
		private const int gainCalibrationSteps = 80;

		// Token: 0x040001CF RID: 463
		private const int thresholdCalibrationSteps = 80;

		// Token: 0x040001D0 RID: 464
		private const int totalCalibrationSteps = 160;

		// Token: 0x040001D1 RID: 465
		private MicrophoneManager audioInputManager;

		// Token: 0x040001D2 RID: 466
		private AudioOutputManager audioOutputManager;

		// Token: 0x040001D3 RID: 467
		private CancellationTokenSource configurationCTS;

		// Token: 0x040001D4 RID: 468
		private GuiDialog doneDialog;

		// Token: 0x040001D5 RID: 469
		private GuiElementDynamicText wizardStatusText;

		// Token: 0x040001D6 RID: 470
		private float adjustedGain;

		// Token: 0x040001D7 RID: 471
		private float adjustedThreshold;

		// Token: 0x040001D8 RID: 472
		private bool configurationInProcess;

		// Token: 0x040001D9 RID: 473
		private bool noMicrophoneMode;

		// Token: 0x0200014B RID: 331
		private class AudioWizardDoneDialog : GuiDialog
		{
			// Token: 0x170001BB RID: 443
			// (get) Token: 0x06000AFA RID: 2810 RVA: 0x000397E5 File Offset: 0x000379E5
			public override double DrawOrder
			{
				get
				{
					return 0.11;
				}
			}

			// Token: 0x06000AFB RID: 2811 RVA: 0x000397F0 File Offset: 0x000379F0
			public AudioWizardDoneDialog(ICoreClientAPI capi) : base(capi)
			{
				TextDrawUtil textDrawUtil = new TextDrawUtil();
				CairoFont font = CairoFont.WhiteSmallishText();
				string titleBarText = UIUtils.I18n("Gui.AudioWizardDialog.TitleBar");
				string firstTextBlock = UIUtils.I18n("Gui.AudioWizardDialog.Done");
				string okButtonText = Lang.Get("Ok", Array.Empty<object>());
				double firstTextBlockHeight = textDrawUtil.GetMultilineTextHeight(font, firstTextBlock, 460.0, 0);
				ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding).WithSizing(2);
				ElementBounds firstTextBlockBounds = ElementBounds.Fixed(0.0, GuiStyle.TitleBarHeight, 460.0, firstTextBlockHeight);
				ElementBounds buttonBounds = firstTextBlockBounds.BelowCopy(0.0, 15.0, 0.0, 0.0).WithFixedSize(0.0, 30.0).WithFixedPadding(10.0, 2.0).WithAlignment(8);
				base.SingleComposer = GuiComposerHelpers.AddButton(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(capi.Gui.CreateCompo("RPVC_AudioWizardDoneDialog", ElementStdBounds.AutosizedMainDialog), bgBounds, true, 5.0, 0.75f), titleBarText, delegate()
				{
					this.TryClose();
				}, null, null, null).BeginChildElements(bgBounds), firstTextBlock, font, 2, firstTextBlockBounds, null), okButtonText, new ActionConsumable(this.TryClose), buttonBounds, 2, null).EndChildElements().Compose(true);
			}

			// Token: 0x170001BC RID: 444
			// (get) Token: 0x06000AFC RID: 2812 RVA: 0x0003995A File Offset: 0x00037B5A
			public override string ToggleKeyCombinationCode
			{
				get
				{
					return null;
				}
			}

			// Token: 0x0400055C RID: 1372
			private const string composerName = "RPVC_AudioWizardDoneDialog";
		}
	}
}
