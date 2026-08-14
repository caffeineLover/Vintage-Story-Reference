using System;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000071 RID: 113
	public class SpeechIndicator : HudElement
	{
		// Token: 0x0600031E RID: 798 RVA: 0x00010D10 File Offset: 0x0000EF10
		public SpeechIndicator(ICoreClientAPI capi, MicrophoneManager microphoneManager) : base(capi)
		{
			this.audioInputManager = microphoneManager;
			this.currentVoiceLevel = microphoneManager.GetVoiceLevel();
			GuiDialogCreateCharacterPatch.OnCharacterSelection += this.bindToMainThread(new Action(this.UpdateVoiceType));
			microphoneManager.TransmissionStateChanged += this.bindToMainThread(new Action(this.UpdateDisplay));
			microphoneManager.VoiceLevelUpdated += new Action<VoiceLevel>(this.OnVoiceLevelUpdated);
			capi.Event.RegisterEventBusListener(new EventBusListenerDelegate(this.OnHudUpdate), 0.5, "rpvoicechat:hudUpdate");
			capi.Event.RegisterEventBusListener(new EventBusListenerDelegate(this.OnVoiceBanUpdate), 0.5, "rpvoicechat:voiceBanUpdate");
			this.CheckVoiceBanStatus();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00010E1E File Offset: 0x0000F01E
		public override void OnOwnPlayerDataReceived()
		{
			this.UpdateVoiceType();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00010E28 File Offset: 0x0000F028
		private void UpdateVoiceType()
		{
			IClientPlayer player = this.capi.World.Player;
			this.voiceType = (((player != null) ? player.Entity.talkUtil.soundName.GetName() : null) ?? this.voiceType);
			this.SetupIcon();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00010E76 File Offset: 0x0000F076
		private void OnVoiceLevelUpdated(VoiceLevel voiceLevel)
		{
			this.currentVoiceLevel = voiceLevel;
			this.bindToMainThread(new Action(this.SetupIcon)).Invoke();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00010E96 File Offset: 0x0000F096
		private Action bindToMainThread(Action function)
		{
			return delegate()
			{
				this.capi.Event.EnqueueMainThreadTask(function, "rpvoicechat:SpeechIndicator");
			};
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00010EB6 File Offset: 0x0000F0B6
		private void OnHudUpdate(string _, ref EnumHandling __, object ___)
		{
			this.bindToMainThread(new Action(this.SetupIcon)).Invoke();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00010ECF File Offset: 0x0000F0CF
		private void OnVoiceBanUpdate(string _, ref EnumHandling __, object ___)
		{
			this.CheckVoiceBanStatus();
			this.bindToMainThread(new Action(this.SetupIcon)).Invoke();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00010EEE File Offset: 0x0000F0EE
		private void CheckVoiceBanStatus()
		{
			if (RPVoiceChatClient.VoiceBanManagerInstance != null && this.capi.World.Player != null)
			{
				this.isVoiceBanned = RPVoiceChatClient.VoiceBanManagerInstance.IsPlayerBanned(this.capi.World.Player.PlayerUID);
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00010F30 File Offset: 0x0000F130
		private void UpdateDisplay()
		{
			bool isTalking = this.audioInputManager.Transmitting;
			bool shouldDisplay;
			if (ModConfig.ClientConfig.IsMinimalHud)
			{
				shouldDisplay = ((ModConfig.ClientConfig.IsMuted || this.isVoiceBanned || isTalking) && ModConfig.ClientConfig.ShowHud);
			}
			else
			{
				shouldDisplay = ((ModConfig.ClientConfig.IsMuted || this.isVoiceBanned || isTalking) && ModConfig.ClientConfig.ShowHud);
			}
			if (!(shouldDisplay ? this.TryOpen() : this.TryClose()))
			{
				this.bindToMainThread(new Action(this.UpdateDisplay)).Invoke();
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00010FD0 File Offset: 0x0000F1D0
		public void SetupIcon()
		{
			if (ModConfig.ClientConfig.IsMinimalHud)
			{
				VoiceLevel voiceLevel = this.currentVoiceLevel;
				string text;
				if (voiceLevel != VoiceLevel.Whispering)
				{
					if (voiceLevel != VoiceLevel.Talking)
					{
						if (voiceLevel != VoiceLevel.Shouting)
						{
							text = "minimal-green.png";
						}
						else
						{
							text = "minimal-red.png";
						}
					}
					else
					{
						text = "minimal-green.png";
					}
				}
				else
				{
					text = "minimal-blue.png";
				}
				string colorIcon = text;
				base.SingleComposer = GuiElementHelpers.AddImage(GuiElementHelpers.AddImage(GuiElementHelpers.AddImage(this.capi.Gui.CreateCompo("rpvcspeechindicator", this.dialogBounds), ElementBounds.Fixed(16.0, 16.0, 32.0, 32.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/" + colorIcon)).AddIf(this.isVoiceBanned), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/banned.png")).EndIf().AddIf(ModConfig.ClientConfig.IsMuted && !this.isVoiceBanned), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/muted.png")).EndIf().Compose(true);
			}
			else
			{
				string voiceIcon = new AssetLocation(RPVoiceChatMod.modID, "textures/gui/" + this.voiceType + ".png");
				if (this.capi.Assets.TryGet(voiceIcon, false) == null)
				{
					voiceIcon = new AssetLocation(RPVoiceChatMod.modID, "textures/gui/megaphone.png");
				}
				base.SingleComposer = GuiElementHelpers.AddImage(GuiElementHelpers.AddImage(GuiElementHelpers.AddImage(this.capi.Gui.CreateCompo("rpvcspeechindicator", this.dialogBounds), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), voiceIcon).AddIf(this.isVoiceBanned), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/banned.png")).EndIf().AddIf(ModConfig.ClientConfig.IsMuted && !this.isVoiceBanned), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/muted.png")).EndIf().Compose(true);
			}
			this.UpdateDisplay();
		}

		// Token: 0x0400019D RID: 413
		private const float size = 64f;

		// Token: 0x0400019E RID: 414
		private MicrophoneManager audioInputManager;

		// Token: 0x0400019F RID: 415
		private ElementBounds dialogBounds = new ElementBounds
		{
			Alignment = 11,
			BothSizing = 0,
			fixedWidth = 64.0,
			fixedHeight = 64.0,
			fixedPaddingX = 10.0,
			fixedPaddingY = 10.0
		};

		// Token: 0x040001A0 RID: 416
		private string voiceType;

		// Token: 0x040001A1 RID: 417
		private VoiceLevel currentVoiceLevel;

		// Token: 0x040001A2 RID: 418
		private bool isVoiceBanned;
	}
}
