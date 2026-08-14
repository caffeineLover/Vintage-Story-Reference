using System;
using System.Collections.Generic;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000072 RID: 114
	public class VoiceLevelIcon : HudElement
	{
		// Token: 0x06000328 RID: 808 RVA: 0x000112A0 File Offset: 0x0000F4A0
		public VoiceLevelIcon(ICoreClientAPI capi, MicrophoneManager microphoneManager) : base(capi)
		{
			this.currentVoiceLevel = microphoneManager.GetVoiceLevel();
			microphoneManager.VoiceLevelUpdated += new Action<VoiceLevel>(this.OnVoiceLevelUpdated);
			capi.Event.RegisterEventBusListener(new EventBusListenerDelegate(this.OnHudUpdate), 0.5, "rpvoicechat:hudUpdate");
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0001134D File Offset: 0x0000F54D
		public override void OnOwnPlayerDataReceived()
		{
			this.SetupIcon();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00011355 File Offset: 0x0000F555
		private Action bindToMainThread(Action function)
		{
			return delegate()
			{
				this.capi.Event.EnqueueMainThreadTask(function, "rpvoicechat:VoiceLevelIcon");
			};
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00011375 File Offset: 0x0000F575
		private void OnVoiceLevelUpdated(VoiceLevel voiceLevel)
		{
			this.currentVoiceLevel = voiceLevel;
			this.bindToMainThread(new Action(this.SetupIcon)).Invoke();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00011395 File Offset: 0x0000F595
		private void OnHudUpdate(string _, ref EnumHandling __, object ___)
		{
			this.bindToMainThread(new Action(this.SetupIcon)).Invoke();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000113B0 File Offset: 0x0000F5B0
		private void UpdateDisplay()
		{
			if (!((ModConfig.ClientConfig.ShowHud && !ModConfig.ClientConfig.IsMinimalHud) ? this.TryOpen() : this.TryClose()))
			{
				this.bindToMainThread(new Action(this.UpdateDisplay)).Invoke();
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00011404 File Offset: 0x0000F604
		public void SetupIcon()
		{
			string voiceLevel = VoiceLevelIcon.textureNameByVoiceLevel[this.currentVoiceLevel];
			base.SingleComposer = GuiElementHelpers.AddImage(this.capi.Gui.CreateCompo("rpvcvoicelevelicon", this.dialogBounds), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/" + voiceLevel + ".png")).Compose(true);
			this.UpdateDisplay();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00011497 File Offset: 0x0000F697
		// Note: this type is marked as 'beforefieldinit'.
		static VoiceLevelIcon()
		{
			Dictionary<VoiceLevel, string> dictionary = new Dictionary<VoiceLevel, string>();
			dictionary.Add(VoiceLevel.Whispering, "whisper");
			dictionary.Add(VoiceLevel.Talking, "talk");
			dictionary.Add(VoiceLevel.Shouting, "shout");
			VoiceLevelIcon.textureNameByVoiceLevel = dictionary;
		}

		// Token: 0x040001A3 RID: 419
		private const float size = 64f;

		// Token: 0x040001A4 RID: 420
		private static readonly Dictionary<VoiceLevel, string> textureNameByVoiceLevel;

		// Token: 0x040001A5 RID: 421
		private ElementBounds dialogBounds = new ElementBounds
		{
			Alignment = 11,
			BothSizing = 0,
			fixedWidth = 64.0,
			fixedHeight = 64.0,
			fixedPaddingX = 84.0,
			fixedPaddingY = 10.0
		};

		// Token: 0x040001A6 RID: 422
		private VoiceLevel currentVoiceLevel;
	}
}
