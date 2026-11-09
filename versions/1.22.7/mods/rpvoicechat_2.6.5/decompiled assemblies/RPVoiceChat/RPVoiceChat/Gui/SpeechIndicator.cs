using System;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000094 RID: 148
	public class SpeechIndicator : HudElement
	{
		// Token: 0x060003E1 RID: 993 RVA: 0x00014E50 File Offset: 0x00013050
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

		// Token: 0x060003E2 RID: 994 RVA: 0x00014F5E File Offset: 0x0001315E
		public override void OnOwnPlayerDataReceived()
		{
			this.UpdateVoiceType();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00014F68 File Offset: 0x00013168
		private void UpdateVoiceType()
		{
			IClientPlayer player = this.capi.World.Player;
			this.voiceType = (((player != null) ? player.Entity.talkUtil.soundName.GetName() : null) ?? this.voiceType);
			this.SetupIcon();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00014FB6 File Offset: 0x000131B6
		private void OnVoiceLevelUpdated(VoiceLevel voiceLevel)
		{
			this.currentVoiceLevel = voiceLevel;
			this.bindToMainThread(new Action(this.SetupIcon)).Invoke();
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00014FD6 File Offset: 0x000131D6
		private Action bindToMainThread(Action function)
		{
			return delegate()
			{
				this.capi.Event.EnqueueMainThreadTask(function, "rpvoicechat:SpeechIndicator");
			};
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00014FF6 File Offset: 0x000131F6
		private void OnHudUpdate(string _, ref EnumHandling __, object ___)
		{
			this.bindToMainThread(new Action(this.SetupIcon)).Invoke();
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001500F File Offset: 0x0001320F
		private void OnVoiceBanUpdate(string _, ref EnumHandling __, object ___)
		{
			this.CheckVoiceBanStatus();
			this.bindToMainThread(new Action(this.SetupIcon)).Invoke();
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001502E File Offset: 0x0001322E
		public override void Dispose()
		{
			GuiComposer singleComposer = base.SingleComposer;
			if (singleComposer != null)
			{
				singleComposer.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00015047 File Offset: 0x00013247
		private void CheckVoiceBanStatus()
		{
			if (RPVoiceChatClient.VoiceBanManagerInstance != null && this.capi.World.Player != null)
			{
				this.isVoiceBanned = RPVoiceChatClient.VoiceBanManagerInstance.IsPlayerBanned(this.capi.World.Player.PlayerUID);
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00015088 File Offset: 0x00013288
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

		// Token: 0x060003EB RID: 1003 RVA: 0x00015144 File Offset: 0x00013344
		public void SetupIcon()
		{
			bool minimalHud = ModConfig.ClientConfig.IsMinimalHud;
			bool isMuted = ModConfig.ClientConfig.IsMuted;
			if (base.SingleComposer != null && this.currentVoiceLevel == this.lastBuiltVoiceLevel && this.voiceType == this.lastBuiltVoiceType && minimalHud == this.lastBuiltMinimalHud && isMuted == this.lastBuiltMuted && this.isVoiceBanned == this.lastBuiltBanned)
			{
				this.UpdateDisplay();
				return;
			}
			this.lastBuiltVoiceLevel = this.currentVoiceLevel;
			this.lastBuiltVoiceType = this.voiceType;
			this.lastBuiltMinimalHud = minimalHud;
			this.lastBuiltMuted = isMuted;
			this.lastBuiltBanned = this.isVoiceBanned;
			if (this.IsOpened())
			{
				this.TryClose();
			}
			if (minimalHud)
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
				GuiComposer singleComposer = base.SingleComposer;
				if (singleComposer != null)
				{
					singleComposer.Dispose();
				}
				base.SingleComposer = GuiElementHelpers.AddImage(GuiElementHelpers.AddImage(GuiElementHelpers.AddImage(this.capi.Gui.CreateCompo("rpvcspeechindicator", this.dialogBounds), ElementBounds.Fixed(16.0, 16.0, 32.0, 32.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/" + colorIcon)).AddIf(this.isVoiceBanned), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/banned.png")).EndIf().AddIf(ModConfig.ClientConfig.IsMuted && !this.isVoiceBanned), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/muted.png")).EndIf().Compose(true);
			}
			else
			{
				string voiceIcon = new AssetLocation(RPVoiceChatMod.modID, "textures/gui/" + this.voiceType + ".png");
				if (this.capi.Assets.TryGet(voiceIcon, false) == null)
				{
					voiceIcon = new AssetLocation(RPVoiceChatMod.modID, "textures/gui/megaphone.png");
				}
				GuiComposer singleComposer2 = base.SingleComposer;
				if (singleComposer2 != null)
				{
					singleComposer2.Dispose();
				}
				base.SingleComposer = GuiElementHelpers.AddImage(GuiElementHelpers.AddImage(GuiElementHelpers.AddImage(this.capi.Gui.CreateCompo("rpvcspeechindicator", this.dialogBounds), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), voiceIcon).AddIf(this.isVoiceBanned), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/banned.png")).EndIf().AddIf(ModConfig.ClientConfig.IsMuted && !this.isVoiceBanned), ElementBounds.Fixed(0.0, 0.0, 64.0, 64.0), new AssetLocation(RPVoiceChatMod.modID, "textures/gui/muted.png")).EndIf().Compose(true);
			}
			this.UpdateDisplay();
		}

		// Token: 0x0400020F RID: 527
		private const float size = 64f;

		// Token: 0x04000210 RID: 528
		private MicrophoneManager audioInputManager;

		// Token: 0x04000211 RID: 529
		private ElementBounds dialogBounds = new ElementBounds
		{
			Alignment = 11,
			BothSizing = 0,
			fixedWidth = 64.0,
			fixedHeight = 64.0,
			fixedPaddingX = 10.0,
			fixedPaddingY = 10.0
		};

		// Token: 0x04000212 RID: 530
		private string voiceType;

		// Token: 0x04000213 RID: 531
		private VoiceLevel currentVoiceLevel;

		// Token: 0x04000214 RID: 532
		private bool isVoiceBanned;

		// Token: 0x04000215 RID: 533
		private VoiceLevel lastBuiltVoiceLevel;

		// Token: 0x04000216 RID: 534
		private string lastBuiltVoiceType;

		// Token: 0x04000217 RID: 535
		private bool lastBuiltMinimalHud;

		// Token: 0x04000218 RID: 536
		private bool lastBuiltMuted;

		// Token: 0x04000219 RID: 537
		private bool lastBuiltBanned;

		// Token: 0x0400021A RID: 538
		private bool lastShouldDisplay;
	}
}
