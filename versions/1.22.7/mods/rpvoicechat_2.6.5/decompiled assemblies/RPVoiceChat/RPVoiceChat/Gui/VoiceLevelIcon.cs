using System;
using System.Collections.Generic;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000095 RID: 149
	public class VoiceLevelIcon : HudElement
	{
		// Token: 0x060003EC RID: 1004 RVA: 0x000154DC File Offset: 0x000136DC
		public VoiceLevelIcon(ICoreClientAPI capi, MicrophoneManager microphoneManager) : base(capi)
		{
			this.currentVoiceLevel = microphoneManager.GetVoiceLevel();
			microphoneManager.VoiceLevelUpdated += new Action<VoiceLevel>(this.OnVoiceLevelUpdated);
			capi.Event.RegisterEventBusListener(new EventBusListenerDelegate(this.OnHudUpdate), 0.5, "rpvoicechat:hudUpdate");
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00015589 File Offset: 0x00013789
		public override void OnOwnPlayerDataReceived()
		{
			this.SetupIcon();
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00015591 File Offset: 0x00013791
		private Action bindToMainThread(Action function)
		{
			return delegate()
			{
				this.capi.Event.EnqueueMainThreadTask(function, "rpvoicechat:VoiceLevelIcon");
			};
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x000155B1 File Offset: 0x000137B1
		private void OnVoiceLevelUpdated(VoiceLevel voiceLevel)
		{
			this.currentVoiceLevel = voiceLevel;
			this.bindToMainThread(new Action(this.SetupIcon)).Invoke();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x000155D1 File Offset: 0x000137D1
		private void OnHudUpdate(string _, ref EnumHandling __, object ___)
		{
			this.bindToMainThread(new Action(this.UpdateDisplay)).Invoke();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x000155EA File Offset: 0x000137EA
		public override void Dispose()
		{
			GuiComposer singleComposer = base.SingleComposer;
			if (singleComposer != null)
			{
				singleComposer.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00015604 File Offset: 0x00013804
		private void UpdateDisplay()
		{
			bool shouldDisplay = ModConfig.ClientConfig.ShowHud && !ModConfig.ClientConfig.IsMinimalHud;
			if (shouldDisplay == this.lastShouldDisplay && this.IsOpened() == shouldDisplay)
			{
				return;
			}
			this.lastShouldDisplay = shouldDisplay;
			if (!(shouldDisplay ? this.TryOpen() : this.TryClose()))
			{
				this.bindToMainThread(new Action(this.UpdateDisplay)).Invoke();
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00015674 File Offset: 0x00013874
		public void SetupIcon()
		{
			if (base.SingleComposer != null && this.currentVoiceLevel == this.lastBuiltVoiceLevel)
			{
				this.UpdateDisplay();
				return;
			}
			this.lastBuiltVoiceLevel = this.currentVoiceLevel;
			string voiceLevel = VoiceLevelIcon.textureNameByVoiceLevel[this.currentVoiceLevel];
			if (this.IsOpened())
			{
				this.TryClose();
			}
			GuiComposer singleComposer = base.SingleComposer;
			if (singleComposer != null)
			{
				singleComposer.Dispose();
			}
			base.SingleComposer = GuiElementHelpers.AddImage(this.capi.Gui.CreateCompo("rpvcvoicelevelicon", this.dialogBounds), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/" + voiceLevel + ".png")).Compose(true);
			this.UpdateDisplay();
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00015750 File Offset: 0x00013950
		// Note: this type is marked as 'beforefieldinit'.
		static VoiceLevelIcon()
		{
			Dictionary<VoiceLevel, string> dictionary = new Dictionary<VoiceLevel, string>();
			dictionary.Add(VoiceLevel.Whispering, "whisper");
			dictionary.Add(VoiceLevel.Talking, "talk");
			dictionary.Add(VoiceLevel.Shouting, "shout");
			VoiceLevelIcon.textureNameByVoiceLevel = dictionary;
		}

		// Token: 0x0400021B RID: 539
		private const float size = 64f;

		// Token: 0x0400021C RID: 540
		private static readonly Dictionary<VoiceLevel, string> textureNameByVoiceLevel;

		// Token: 0x0400021D RID: 541
		private ElementBounds dialogBounds = new ElementBounds
		{
			Alignment = 11,
			BothSizing = 0,
			fixedWidth = 64.0,
			fixedHeight = 64.0,
			fixedPaddingX = 84.0,
			fixedPaddingY = 10.0
		};

		// Token: 0x0400021E RID: 542
		private VoiceLevel currentVoiceLevel;

		// Token: 0x0400021F RID: 543
		private VoiceLevel lastBuiltVoiceLevel;

		// Token: 0x04000220 RID: 544
		private bool lastShouldDisplay;
	}
}
