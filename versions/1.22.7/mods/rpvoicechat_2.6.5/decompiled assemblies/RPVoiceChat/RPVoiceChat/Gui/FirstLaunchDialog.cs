using System;
using RPVoiceChat.Config;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000091 RID: 145
	public class FirstLaunchDialog : GuiDialog
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060003CC RID: 972 RVA: 0x00014765 File Offset: 0x00012965
		public override double DrawOrder
		{
			get
			{
				return 0.09;
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00014770 File Offset: 0x00012970
		public FirstLaunchDialog(ICoreClientAPI capi, GuiManager guiManager) : base(capi)
		{
			this.guiManager = guiManager;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00014780 File Offset: 0x00012980
		public void ShowIfNecessary()
		{
			if (!ModConfig.ClientConfig.FirstTimeUse)
			{
				return;
			}
			this.Compose();
			this.TryOpen();
			ModConfig.ClientConfig.FirstTimeUse = false;
			ModConfig.SaveClient(this.capi);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000147B4 File Offset: 0x000129B4
		private void Compose()
		{
			bool canCapture = this.guiManager.audioInputManager.CanUseMicrophoneCapture();
			TextDrawUtil drawUtil = new TextDrawUtil();
			CairoFont font = CairoFont.WhiteSmallishText();
			string modMenuHotkey = this.capi.Input.GetHotKeyByCode("voicechatMenu").CurrentMapping.ToString();
			string titleBarText = UIUtils.I18n("Gui.FirstLaunchDialog.TitleBar");
			string firstTextBlock = canCapture ? UIUtils.I18n("Gui.FirstLaunchDialog.FirstParagraph") : UIUtils.I18n("Gui.FirstLaunchDialog.NoMicrophoneFirstParagraph");
			string secondTextBlock = canCapture ? UIUtils.I18n("Gui.FirstLaunchDialog.SecondParagraph", new object[]
			{
				modMenuHotkey
			}) : UIUtils.I18n("Gui.FirstLaunchDialog.NoMicrophoneSecondParagraph", new object[]
			{
				modMenuHotkey
			});
			string notNowButtonText = UIUtils.I18n("Gui.Button.NotNow");
			string sureButtonText = UIUtils.I18n("Gui.Button.Sure");
			string okButtonText = Lang.Get("Ok", Array.Empty<object>());
			double firstTextBlockHeight = drawUtil.GetMultilineTextHeight(font, firstTextBlock, 460.0, 0);
			double secondTextBlockHeight = drawUtil.GetMultilineTextHeight(font, secondTextBlock, 460.0, 0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding).WithSizing(2);
			ElementBounds firstTextBlockBounds = ElementBounds.Fixed(5.0, GuiStyle.TitleBarHeight + 10.0, 460.0, firstTextBlockHeight);
			ElementBounds secondTextBlockBounds = firstTextBlockBounds.BelowCopy(0.0, 25.0, 0.0, 0.0).WithFixedHeight(secondTextBlockHeight);
			ElementBounds buttonBounds = secondTextBlockBounds.BelowCopy(-5.0, 25.0, 0.0, 0.0).WithFixedSize(0.0, 30.0).WithFixedPadding(10.0, 2.0);
			GuiComposer composer = GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("RPVC_FirstLaunchDialog", ElementStdBounds.AutosizedMainDialog), bgBounds, true, 5.0, 0.75f), titleBarText, delegate()
			{
				this.TryClose();
			}, null, null, null).BeginChildElements(bgBounds), firstTextBlock, font, firstTextBlockBounds, null), secondTextBlock, font, secondTextBlockBounds, null);
			if (canCapture)
			{
				GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(composer, notNowButtonText, new ActionConsumable(this.TryClose), buttonBounds, 2, null), sureButtonText, new ActionConsumable(this.OnSureButtonClick), buttonBounds.FlatCopy().WithAlignment(12), 2, null);
			}
			else
			{
				GuiComposerHelpers.AddButton(composer, okButtonText, new ActionConsumable(this.TryClose), buttonBounds.WithAlignment(8), 2, null);
			}
			base.SingleComposer = composer.EndChildElements().Compose(true);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00014A53 File Offset: 0x00012C53
		private bool OnSureButtonClick()
		{
			this.TryClose();
			if (this.guiManager.audioInputManager.CanUseMicrophoneCapture())
			{
				this.guiManager.audioWizardDialog.TryOpen();
			}
			return true;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00014A80 File Offset: 0x00012C80
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040001FA RID: 506
		private const string i18nPrefix = "Gui.FirstLaunchDialog";

		// Token: 0x040001FB RID: 507
		private const string composerName = "RPVC_FirstLaunchDialog";

		// Token: 0x040001FC RID: 508
		private const int textYOffset = 10;

		// Token: 0x040001FD RID: 509
		private const int textLeftPadding = 5;

		// Token: 0x040001FE RID: 510
		private const int textBottomPadding = 25;

		// Token: 0x040001FF RID: 511
		private const int textWidth = 460;

		// Token: 0x04000200 RID: 512
		private const int buttonHeight = 30;

		// Token: 0x04000201 RID: 513
		private const int buttonXPadding = 10;

		// Token: 0x04000202 RID: 514
		private const int buttonYPadding = 2;

		// Token: 0x04000203 RID: 515
		private GuiManager guiManager;
	}
}
