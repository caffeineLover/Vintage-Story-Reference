using System;
using RPVoiceChat.Config;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200006E RID: 110
	public class FirstLaunchDialog : GuiDialog
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600030A RID: 778 RVA: 0x000106BD File Offset: 0x0000E8BD
		public override double DrawOrder
		{
			get
			{
				return 0.09;
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x000106C8 File Offset: 0x0000E8C8
		public FirstLaunchDialog(ICoreClientAPI capi, GuiManager guiManager) : base(capi)
		{
			this.guiManager = guiManager;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x000106D8 File Offset: 0x0000E8D8
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

		// Token: 0x0600030D RID: 781 RVA: 0x0001070C File Offset: 0x0000E90C
		private void Compose()
		{
			TextDrawUtil drawUtil = new TextDrawUtil();
			CairoFont font = CairoFont.WhiteSmallishText();
			string modMenuHotkey = this.capi.Input.GetHotKeyByCode("voicechatMenu").CurrentMapping.ToString();
			string titleBarText = UIUtils.I18n("Gui.FirstLaunchDialog.TitleBar");
			string firstTextBlock = UIUtils.I18n("Gui.FirstLaunchDialog.FirstParagraph");
			string secondTextBlock = UIUtils.I18n("Gui.FirstLaunchDialog.SecondParagraph", new object[]
			{
				modMenuHotkey
			});
			string notNowButtonText = UIUtils.I18n("Gui.Button.NotNow");
			string sureButtonText = UIUtils.I18n("Gui.Button.Sure");
			double firstTextBlockHeight = drawUtil.GetMultilineTextHeight(font, firstTextBlock, 460.0, 0);
			double secondTextBlockHeight = drawUtil.GetMultilineTextHeight(font, secondTextBlock, 460.0, 0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding).WithSizing(2);
			ElementBounds firstTextBlockBounds = ElementBounds.Fixed(5.0, GuiStyle.TitleBarHeight + 10.0, 460.0, firstTextBlockHeight);
			ElementBounds secondTextBlockBounds = firstTextBlockBounds.BelowCopy(0.0, 25.0, 0.0, 0.0).WithFixedHeight(secondTextBlockHeight);
			ElementBounds buttonBounds = secondTextBlockBounds.BelowCopy(-5.0, 25.0, 0.0, 0.0).WithFixedSize(0.0, 30.0).WithFixedPadding(10.0, 2.0);
			base.SingleComposer = GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("RPVC_FirstLaunchDialog", ElementStdBounds.AutosizedMainDialog), bgBounds, true, 5.0, 0.75f), titleBarText, delegate()
			{
				this.TryClose();
			}, null, null, null).BeginChildElements(bgBounds), firstTextBlock, font, firstTextBlockBounds, null), secondTextBlock, font, secondTextBlockBounds, null), notNowButtonText, new ActionConsumable(this.TryClose), buttonBounds, 2, null), sureButtonText, new ActionConsumable(this.OnSureButtonClick), buttonBounds.FlatCopy().WithAlignment(12), 2, null).EndChildElements().Compose(true);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00010932 File Offset: 0x0000EB32
		private bool OnSureButtonClick()
		{
			this.TryClose();
			this.guiManager.audioWizardDialog.TryOpen();
			return true;
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0001094D File Offset: 0x0000EB4D
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000189 RID: 393
		private const string i18nPrefix = "Gui.FirstLaunchDialog";

		// Token: 0x0400018A RID: 394
		private const string composerName = "RPVC_FirstLaunchDialog";

		// Token: 0x0400018B RID: 395
		private const int textYOffset = 10;

		// Token: 0x0400018C RID: 396
		private const int textLeftPadding = 5;

		// Token: 0x0400018D RID: 397
		private const int textBottomPadding = 25;

		// Token: 0x0400018E RID: 398
		private const int textWidth = 460;

		// Token: 0x0400018F RID: 399
		private const int buttonHeight = 30;

		// Token: 0x04000190 RID: 400
		private const int buttonXPadding = 10;

		// Token: 0x04000191 RID: 401
		private const int buttonYPadding = 2;

		// Token: 0x04000192 RID: 402
		private GuiManager guiManager;
	}
}
