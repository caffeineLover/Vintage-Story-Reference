using System;
using System.Runtime.CompilerServices;
using RPVoiceChat.Util;
using Vintagestory.API.Client;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000070 RID: 112
	public class Announce : HudElement
	{
		// Token: 0x06000317 RID: 791 RVA: 0x00010A53 File Offset: 0x0000EC53
		public Announce(ICoreClientAPI capi) : base(capi)
		{
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00010A64 File Offset: 0x0000EC64
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

		// Token: 0x06000319 RID: 793 RVA: 0x00010ABC File Offset: 0x0000ECBC
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

		// Token: 0x0600031A RID: 794 RVA: 0x00010B3C File Offset: 0x0000ED3C
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

		// Token: 0x0600031B RID: 795 RVA: 0x00010CB4 File Offset: 0x0000EEB4
		public override void OnRenderGUI(float deltaTime)
		{
			base.OnRenderGUI(deltaTime);
			if (this.isDisplaying && (double)this.capi.World.ElapsedMilliseconds / 1000.0 - this.startTime >= this.displayDuration)
			{
				this.isDisplaying = false;
				this.TryClose();
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00010D08 File Offset: 0x0000EF08
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00010D0B File Offset: 0x0000EF0B
		public override EnumDialogType DialogType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x04000197 RID: 407
		private string title;

		// Token: 0x04000198 RID: 408
		private string message;

		// Token: 0x04000199 RID: 409
		private double displayDuration;

		// Token: 0x0400019A RID: 410
		private double startTime;

		// Token: 0x0400019B RID: 411
		private bool isDisplaying;

		// Token: 0x0400019C RID: 412
		private bool showBackground = true;
	}
}
