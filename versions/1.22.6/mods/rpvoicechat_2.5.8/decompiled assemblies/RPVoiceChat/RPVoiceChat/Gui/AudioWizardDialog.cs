using System;
using System.Runtime.CompilerServices;
using System.Threading;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000069 RID: 105
	public class AudioWizardDialog : GuiDialog
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x060002DF RID: 735 RVA: 0x0000F7D4 File Offset: 0x0000D9D4
		// (remove) Token: 0x060002E0 RID: 736 RVA: 0x0000F80C File Offset: 0x0000DA0C
		public event Action GainCalibrationDone;

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000F841 File Offset: 0x0000DA41
		public override double DrawOrder
		{
			get
			{
				return 0.11;
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F84C File Offset: 0x0000DA4C
		public AudioWizardDialog(ICoreClientAPI capi, MicrophoneManager audioInputManager, AudioOutputManager audioOutputManager) : base(capi)
		{
			this.audioInputManager = audioInputManager;
			this.audioOutputManager = audioOutputManager;
			this.doneDialog = new AudioWizardDialog.AudioWizardDoneDialog(capi);
			this.doneDialog.OnClosed += new Action(this.SaveAndExit);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F888 File Offset: 0x0000DA88
		public override bool TryOpen()
		{
			this.audioInputManager.AudioWizardActive = true;
			this.configurationCTS = new CancellationTokenSource();
			if (ModConfig.ClientConfig.InputGain == 0f)
			{
				this.audioInputManager.SetGain(1f);
			}
			this.adjustedGain = ModConfig.ClientConfig.InputGain;
			ModConfig.ClientConfig.Loopback = true;
			this.audioOutputManager.IsLoopbackEnabled = true;
			this.Compose();
			return base.TryOpen();
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F900 File Offset: 0x0000DB00
		public override bool TryClose()
		{
			this.configurationCTS.Cancel();
			this.configurationCTS.Dispose();
			this.configurationInProcess = false;
			ModConfig.ClientConfig.InputGain = this.adjustedGain;
			this.audioInputManager.SetGain(this.adjustedGain);
			if (!this.doneDialog.IsOpened())
			{
				this.SaveAndExit();
			}
			return base.TryClose();
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F964 File Offset: 0x0000DB64
		private void Compose()
		{
			TextDrawUtil textDrawUtil = new TextDrawUtil();
			CairoFont font = CairoFont.WhiteSmallText();
			string[] dropdownValues = this.audioInputManager.GetInputDeviceNames();
			string titleBarText = UIUtils.I18n("Gui.AudioWizardDialog.TitleBar");
			string firstTextBlock = UIUtils.I18n("Gui.AudioWizardDialog.FirstParagraph");
			string secondTextBlock = UIUtils.I18n("Gui.AudioWizardDialog.SecondParagraph");
			string startButtonText = Lang.Get("Start", Array.Empty<object>());
			double firstTextBlockHeight = textDrawUtil.GetMultilineTextHeight(font, firstTextBlock, 460.0, 0);
			double secondTextBlockHeight = textDrawUtil.GetMultilineTextHeight(font, secondTextBlock, 460.0, 0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding).WithSizing(2);
			ElementBounds firstTextBlockBounds = ElementBounds.Fixed(5.0, GuiStyle.TitleBarHeight + 5.0, 460.0, firstTextBlockHeight);
			ElementBounds dropdownBounds = firstTextBlockBounds.BelowCopy(0.0, 15.0, 0.0, 0.0).WithFixedHeight(30.0);
			ElementBounds secondTextBlockBounds = dropdownBounds.BelowCopy(0.0, 15.0, 0.0, 0.0).WithFixedHeight(secondTextBlockHeight);
			ElementBounds progressBarBounds = secondTextBlockBounds.BelowCopy(-5.0, 15.0, 0.0, 0.0).WithFixedHeight(30.0);
			ElementBounds statusTextBounds = progressBarBounds.BelowCopy(0.0, 8.0, 0.0, 0.0).WithFixedHeight(30.0);
			ElementBounds buttonBounds = statusTextBounds.BelowCopy(0.0, 15.0, 0.0, 0.0).WithFixedSize(0.0, 30.0).WithFixedPadding(10.0, 2.0).WithAlignment(8);
			GuiElementStatbar progressBar = new GuiElementStatbar(this.capi, progressBarBounds, new double[]
			{
				0.1,
				0.4,
				0.1
			}, false, false);
			progressBar.ShowValueOnHover = false;
			base.SingleComposer = GuiComposerHelpers.AddButton(GuiElementDynamicTextHelper.AddDynamicText(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDropDown(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("RPVC_AudioWizardDialog", ElementStdBounds.AutosizedMainDialog), bgBounds, true, 5.0, 0.75f), titleBarText, delegate()
			{
				this.TryClose();
			}, null, null, null).BeginChildElements(bgBounds), firstTextBlock, font, firstTextBlockBounds, null), dropdownValues, dropdownValues, 0, new SelectionChangedDelegate(this.OnDropdownSelect), dropdownBounds, "inputDevice"), secondTextBlock, font, secondTextBlockBounds, null).AddInteractiveElement(progressBar, "progressBar"), "", CairoFont.WhiteSmallText(), statusTextBounds, "wizardStatusText"), startButtonText, new ActionConsumable(this.OnStartButtonClick), buttonBounds, 2, null).EndChildElements().Compose(true);
			progressBar.SetValues(0f, 0f, 140f);
			progressBar.SetLineInterval(14f);
			this.wizardStatusText = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "wizardStatusText");
			GuiComposerHelpers.GetDropDown(base.SingleComposer, "inputDevice").SetSelectedValue(new string[]
			{
				ModConfig.ClientConfig.InputDevice ?? "Default"
			});
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000FCC4 File Offset: 0x0000DEC4
		private bool OnStartButtonClick()
		{
			if (this.configurationInProcess)
			{
				return true;
			}
			this.configurationInProcess = true;
			GuiElementDynamicText guiElementDynamicText = this.wizardStatusText;
			if (guiElementDynamicText != null)
			{
				guiElementDynamicText.SetNewText("", false, false, false);
			}
			float maxGain = AudioUtils.DBsToFactor(20f);
			this.audioInputManager.SetGain(maxGain);
			this.StartGainConfiguration();
			return true;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000FD1C File Offset: 0x0000DF1C
		private void StartGainConfiguration()
		{
			AudioWizardDialog.<StartGainConfiguration>d__29 <StartGainConfiguration>d__;
			<StartGainConfiguration>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<StartGainConfiguration>d__.<>4__this = this;
			<StartGainConfiguration>d__.<>1__state = -1;
			<StartGainConfiguration>d__.<>t__builder.Start<AudioWizardDialog.<StartGainConfiguration>d__29>(ref <StartGainConfiguration>d__);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000FD53 File Offset: 0x0000DF53
		private void SaveAndExit()
		{
			this.audioInputManager.AudioWizardActive = false;
			ModConfig.ClientConfig.Loopback = false;
			this.audioOutputManager.IsLoopbackEnabled = false;
			ModConfig.SaveClient(this.capi);
			Action gainCalibrationDone = this.GainCalibrationDone;
			if (gainCalibrationDone == null)
			{
				return;
			}
			gainCalibrationDone.Invoke();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000FD93 File Offset: 0x0000DF93
		private void OnDropdownSelect(string value, bool selected)
		{
			this.audioInputManager.SetInputDevice(value);
			GuiComposerHelpers.GetDropDown(base.SingleComposer, "inputDevice").SetSelectedValue(new string[]
			{
				ModConfig.ClientConfig.InputDevice ?? "Default"
			});
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000FDD2 File Offset: 0x0000DFD2
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000FDD5 File Offset: 0x0000DFD5
		public override void Dispose()
		{
			GuiDialog guiDialog = this.doneDialog;
			if (guiDialog != null)
			{
				guiDialog.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x04000156 RID: 342
		private const string i18nPrefix = "Gui.AudioWizardDialog";

		// Token: 0x04000157 RID: 343
		private const string composerName = "RPVC_AudioWizardDialog";

		// Token: 0x04000158 RID: 344
		private const int textYOffset = 5;

		// Token: 0x04000159 RID: 345
		private const int textLeftPadding = 5;

		// Token: 0x0400015A RID: 346
		private const int textBottomPadding = 15;

		// Token: 0x0400015B RID: 347
		private const int textWidth = 460;

		// Token: 0x0400015C RID: 348
		private const int defaultElementHeight = 30;

		// Token: 0x0400015D RID: 349
		private const int buttonXPadding = 10;

		// Token: 0x0400015E RID: 350
		private const int buttonYPadding = 2;

		// Token: 0x0400015F RID: 351
		private const int gainCalibrationDuration = 7000;

		// Token: 0x04000160 RID: 352
		private const int calibrationUpdateInterval = 50;

		// Token: 0x04000161 RID: 353
		private const int calibrationSteps = 140;

		// Token: 0x04000162 RID: 354
		private MicrophoneManager audioInputManager;

		// Token: 0x04000163 RID: 355
		private AudioOutputManager audioOutputManager;

		// Token: 0x04000164 RID: 356
		private CancellationTokenSource configurationCTS;

		// Token: 0x04000165 RID: 357
		private GuiDialog doneDialog;

		// Token: 0x04000166 RID: 358
		private GuiElementDynamicText wizardStatusText;

		// Token: 0x04000167 RID: 359
		private float adjustedGain;

		// Token: 0x04000168 RID: 360
		private bool configurationInProcess;

		// Token: 0x020000FC RID: 252
		private class AudioWizardDoneDialog : GuiDialog
		{
			// Token: 0x17000158 RID: 344
			// (get) Token: 0x06000841 RID: 2113 RVA: 0x0002C001 File Offset: 0x0002A201
			public override double DrawOrder
			{
				get
				{
					return 0.11;
				}
			}

			// Token: 0x06000842 RID: 2114 RVA: 0x0002C00C File Offset: 0x0002A20C
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

			// Token: 0x17000159 RID: 345
			// (get) Token: 0x06000843 RID: 2115 RVA: 0x0002C176 File Offset: 0x0002A376
			public override string ToggleKeyCombinationCode
			{
				get
				{
					return null;
				}
			}

			// Token: 0x04000404 RID: 1028
			private const string composerName = "RPVC_AudioWizardDoneDialog";
		}
	}
}
