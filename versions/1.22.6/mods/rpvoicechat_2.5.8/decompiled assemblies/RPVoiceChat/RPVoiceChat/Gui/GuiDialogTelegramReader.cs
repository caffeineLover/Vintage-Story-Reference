using System;
using Vintagestory.API.Client;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000078 RID: 120
	public class GuiDialogTelegramReader : GuiDialog
	{
		// Token: 0x06000371 RID: 881 RVA: 0x00013AAF File Offset: 0x00011CAF
		public GuiDialogTelegramReader(string title, string message, ICoreClientAPI capi) : base(capi)
		{
			this.message = message;
			this.capi = capi;
			this.SetupDialog(title);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00013AD0 File Offset: 0x00011CD0
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

		// Token: 0x06000373 RID: 883 RVA: 0x00013C3D File Offset: 0x00011E3D
		private void OnTextAreaChanged(string value)
		{
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00013C3F File Offset: 0x00011E3F
		private void OnTitleBarClose()
		{
			this.TryClose();
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00013C48 File Offset: 0x00011E48
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00013C4B File Offset: 0x00011E4B
		public override bool TryOpen()
		{
			return !this.IsOpened() && base.TryOpen();
		}

		// Token: 0x040001C8 RID: 456
		private string message;

		// Token: 0x040001C9 RID: 457
		private ICoreClientAPI capi;
	}
}
