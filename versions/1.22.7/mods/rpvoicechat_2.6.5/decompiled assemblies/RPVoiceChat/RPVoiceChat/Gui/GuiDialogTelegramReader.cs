using System;
using Vintagestory.API.Client;

namespace RPVoiceChat.Gui
{
	// Token: 0x020000A1 RID: 161
	public class GuiDialogTelegramReader : GuiDialog
	{
		// Token: 0x06000477 RID: 1143 RVA: 0x00019B43 File Offset: 0x00017D43
		public GuiDialogTelegramReader(string title, string message, ICoreClientAPI capi) : base(capi)
		{
			this.message = message;
			this.capi = capi;
			this.SetupDialog(title);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00019B64 File Offset: 0x00017D64
		private void SetupDialog(string title)
		{
			ElementBounds dialogBounds = ElementBounds.Fixed(0.0, 0.0, 600.0, 400.0).WithAlignment(6);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			bgBounds.WithChildren(new ElementBounds[]
			{
				dialogBounds
			});
			base.SingleComposer = GuiComposerHelpers.AddTextArea(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("telegramreader-" + Guid.NewGuid().ToString(), dialogBounds), bgBounds, true, 5.0, 0.75f), title, new Action(this.OnTitleBarClose), null, null, null).BeginChildElements(bgBounds), "Message:", CairoFont.WhiteDetailText(), ElementBounds.Fixed(0.0, 40.0, 560.0, 30.0), null), ElementBounds.Fixed(0.0, 70.0, 560.0, 280.0), new Action<string>(this.OnTextAreaChanged), CairoFont.TextInput(), "message").EndChildElements().Compose(true);
			GuiComposerHelpers.GetTextArea(base.SingleComposer, "message").SetValue(this.message, true);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00019CD1 File Offset: 0x00017ED1
		private void OnTextAreaChanged(string value)
		{
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00019CD3 File Offset: 0x00017ED3
		private void OnTitleBarClose()
		{
			this.TryClose();
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00019CDC File Offset: 0x00017EDC
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00019CDF File Offset: 0x00017EDF
		public override bool TryOpen()
		{
			return !this.IsOpened() && base.TryOpen();
		}

		// Token: 0x04000263 RID: 611
		private string message;

		// Token: 0x04000264 RID: 612
		private ICoreClientAPI capi;
	}
}
