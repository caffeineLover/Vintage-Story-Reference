using System;
using System.Runtime.CompilerServices;
using RPVoiceChat.Util;
using Vintagestory.API.Client;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000093 RID: 147
	public class Announce : HudElement
	{
		// Token: 0x060003DA RID: 986 RVA: 0x00014B92 File Offset: 0x00012D92
		public Announce(ICoreClientAPI capi) : base(capi)
		{
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00014BA4 File Offset: 0x00012DA4
		public void ShowAnnouncement(string title, string message, double durationSeconds = 5.0, bool showBackground = true)
		{
			this.capi.Event.EnqueueMainThreadTask(delegate()
			{
				try
				{
					this.ShowAnnouncementInternal(title, message, durationSeconds, showBackground);
				}
				catch (Exception e)
				{
					Logger client = Logger.client;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(31, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Error displaying announcement: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
					client.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}, "rpvc-announce");
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00014BFC File Offset: 0x00012DFC
		private void ShowAnnouncementInternal(string title, string message, double durationSeconds, bool showBackground)
		{
			this.title = (title ?? "");
			this.message = (message ?? "");
			this.displayDuration = durationSeconds;
			this.showBackground = showBackground;
			this.startTime = (double)this.capi.World.ElapsedMilliseconds / 1000.0;
			this.isDisplaying = true;
			if (this.IsOpened())
			{
				this.TryClose();
			}
			this.SetupDialog();
			this.TryOpen();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00014C7C File Offset: 0x00012E7C
		private void SetupDialog()
		{
			CairoFont titleFont = CairoFont.WhiteMediumText().WithFontSize(24f);
			CairoFont messageFont = CairoFont.WhiteDetailText();
			double maxWidth = 500.0;
			double padding = 20.0;
			double topPadding = (double)(this.showBackground ? 35 : 20);
			ElementBounds bgBounds = ElementBounds.Fixed(0.0, 0.0, maxWidth, 220.0);
			ElementBounds titleBounds = ElementBounds.Fixed(padding, topPadding, maxWidth - padding * 2.0, 40.0);
			ElementBounds messageBounds = ElementBounds.Fixed(padding, topPadding + 50.0, maxWidth - padding * 2.0, 100.0);
			ElementBounds dialogBounds = ElementBounds.Fixed(((double)this.capi.Render.FrameWidth - maxWidth) / 2.0, (double)(this.capi.Render.FrameHeight / 4), maxWidth, 220.0);
			GuiComposer composer = this.capi.Gui.CreateCompo("rpvcannounce", dialogBounds);
			if (this.showBackground)
			{
				GuiComposerHelpers.AddShadedDialogBG(composer, bgBounds, true, 5.0, 0.75f);
			}
			else
			{
				GuiElementInsetHelper.AddInset(composer, bgBounds, 3, 0.5f);
			}
			base.SingleComposer = GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddStaticText(composer, this.title, titleFont, 2, titleBounds, "titleText"), this.message, messageFont, messageBounds, "messageText").Compose(true);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00014DF4 File Offset: 0x00012FF4
		public override void OnRenderGUI(float deltaTime)
		{
			base.OnRenderGUI(deltaTime);
			if (this.isDisplaying && (double)this.capi.World.ElapsedMilliseconds / 1000.0 - this.startTime >= this.displayDuration)
			{
				this.isDisplaying = false;
				this.TryClose();
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00014E48 File Offset: 0x00013048
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x00014E4B File Offset: 0x0001304B
		public override EnumDialogType DialogType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x04000209 RID: 521
		private string title;

		// Token: 0x0400020A RID: 522
		private string message;

		// Token: 0x0400020B RID: 523
		private double displayDuration;

		// Token: 0x0400020C RID: 524
		private double startTime;

		// Token: 0x0400020D RID: 525
		private bool isDisplaying;

		// Token: 0x0400020E RID: 526
		private bool showBackground = true;
	}
}
