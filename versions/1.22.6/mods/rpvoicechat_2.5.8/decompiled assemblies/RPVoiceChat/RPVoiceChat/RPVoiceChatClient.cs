using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RPVoiceChat.Audio;
using RPVoiceChat.Client;
using RPVoiceChat.Config;
using RPVoiceChat.DB;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace RPVoiceChat
{
	// Token: 0x0200001F RID: 31
	public class RPVoiceChatClient : RPVoiceChatMod
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000050BD File Offset: 0x000032BD
		// (set) Token: 0x0600009F RID: 159 RVA: 0x000050C4 File Offset: 0x000032C4
		public static AudioOutputManager AudioOutputManagerInstance { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000050CC File Offset: 0x000032CC
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x000050D3 File Offset: 0x000032D3
		public static MicrophoneManager MicrophoneManagerInstance { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000050DB File Offset: 0x000032DB
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x000050E2 File Offset: 0x000032E2
		public static VoiceBanClientManager VoiceBanManagerInstance { get; private set; }

		// Token: 0x060000A4 RID: 164 RVA: 0x000050EA File Offset: 0x000032EA
		public override bool ShouldLoad(EnumAppSide forSide)
		{
			return forSide == 2;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000050F0 File Offset: 0x000032F0
		public override void StartPre(ICoreAPI api)
		{
			base.StartPre(api);
			WorldConfig.Set("additional-content", ModConfig.ServerConfig.AdditionalContent);
			WorldConfig.Set("technology-content", ModConfig.ServerConfig.TechnologyContent);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005124 File Offset: 0x00003324
		public override void StartClientSide(ICoreClientAPI api)
		{
			RPVoiceChatMod.capi = api;
			WireNetworkHandler.RegisterClientside(api);
			RPVoiceChatMod.AnnounceClientChannel.SetMessageHandler<AnnouncePacket>(new NetworkServerMessageHandler<AnnouncePacket>(this.OnAnnounceReceived));
			RPVoiceChatMod.NametagConfigClientChannel.SetMessageHandler<NametagConfigChangedPacket>(new NetworkServerMessageHandler<NametagConfigChangedPacket>(this.OnNametagConfigChanged));
			EmbeddedDllClass.ExtractEmbeddedDlls();
			EmbeddedDllClass.LoadDll("RNNoise.dll");
			this.clientSettingsRepository = new ClientSettingsRepository(RPVoiceChatMod.capi.Logger);
			this.microphoneManager = new MicrophoneManager(RPVoiceChatMod.capi);
			this.audioOutputManager = new AudioOutputManager(RPVoiceChatMod.capi, this.clientSettingsRepository);
			RPVoiceChatClient.MicrophoneManagerInstance = this.microphoneManager;
			RPVoiceChatClient.AudioOutputManagerInstance = this.audioOutputManager;
			bool forwardPorts = !ModConfig.ServerConfig.ManualPortForwarding;
			List<INetworkClient> list = new List<INetworkClient>();
			list.Add(new UDPNetworkClient(forwardPorts));
			list.Add(new TCPNetworkClient());
			list.Add(new NativeNetworkClient(RPVoiceChatMod.capi));
			List<INetworkClient> networkTransports = list;
			this.client = new PlayerNetworkClient(RPVoiceChatMod.capi, networkTransports);
			this.voiceBanManager = new VoiceBanClientManager(RPVoiceChatMod.capi);
			RPVoiceChatClient.VoiceBanManagerInstance = this.voiceBanManager;
			this.guiManager = new GuiManager(RPVoiceChatMod.capi, this.microphoneManager, this.audioOutputManager, this.clientSettingsRepository);
			RPVoiceChatMod.capi.Input.RegisterHotKey("voicechatMenu", UIUtils.I18n("Hotkey.ModMenu"), 124, 2, false, false, false);
			RPVoiceChatMod.capi.Input.RegisterHotKey("voicechatVoiceLevel", UIUtils.I18n("Hotkey.VoiceLevel"), 52, 2, false, false, true);
			RPVoiceChatMod.capi.Input.RegisterHotKey("voicechatPTT", UIUtils.I18n("Hotkey.PTT"), 60, 2, false, false, false);
			RPVoiceChatMod.capi.Input.RegisterHotKey("voicechatMute", UIUtils.I18n("Hotkey.Mute"), 96, 2, false, false, false);
			RPVoiceChatMod.capi.Event.KeyUp += new KeyEventDelegate(this.Event_KeyUp);
			RPVoiceChatMod.capi.Input.SetHotKeyHandler("voicechatMenu", delegate(KeyCombination _)
			{
				if (this.voiceMenuPressed)
				{
					return true;
				}
				this.voiceMenuPressed = true;
				this.guiManager.modMenuDialog.Toggle();
				return true;
			});
			RPVoiceChatMod.capi.Input.SetHotKeyHandler("voicechatVoiceLevel", delegate(KeyCombination _)
			{
				if (this.voiceLevelPressed)
				{
					return true;
				}
				this.voiceLevelPressed = true;
				this.microphoneManager.CycleVoiceLevel();
				return true;
			});
			RPVoiceChatMod.capi.Input.SetHotKeyHandler("voicechatMute", delegate(KeyCombination _)
			{
				if (this.mutePressed)
				{
					return true;
				}
				this.mutePressed = true;
				ModConfig.ClientConfig.IsMuted = !ModConfig.ClientConfig.IsMuted;
				RPVoiceChatMod.capi.Event.PushEvent("rpvoicechat:hudUpdate", null);
				ModConfig.SaveClient(RPVoiceChatMod.capi);
				return true;
			});
			RPVoiceChatMod.capi.Event.LevelFinalize += new Action(this.OnLoad);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000537C File Offset: 0x0000357C
		private void OnLoad()
		{
			this.client.OnAudioReceived += new Action<AudioPacket>(this.OnAudioReceived);
			this.microphoneManager.OnBufferRecorded += new Action<AudioData>(this.OnBufferRecorded);
			this.microphoneManager.Launch();
			this.audioOutputManager.Launch();
			PlayerNameTagRenderer.RefreshAllPlayerNameTags();
			this.guiManager.firstLaunchDialog.ShowIfNecessary();
			this.ShowMacOSMicrophoneWarningOnce();
			this.isReady = true;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000053F0 File Offset: 0x000035F0
		private void ShowMacOSMicrophoneWarningOnce()
		{
			if (RuntimeEnv.OS != 1)
			{
				return;
			}
			if (RPVoiceChatClient.macosMicWarningShown)
			{
				return;
			}
			RPVoiceChatClient.macosMicWarningShown = true;
			string message = "§6[RPVoiceChat] WARNING macOS: if microphone doesn't work, see: §lhttps://github.com/Ridderrasmus/RPVoiceChat/wiki/Troubleshooting#macos-microphone-access-issues§r";
			RPVoiceChatMod.capi.TriggerChatMessage(message);
			Logger.client.Warning("macOS detected. If microphone doesn't work, see: https://github.com/Ridderrasmus/RPVoiceChat/wiki/Troubleshooting#macos-microphone-access-issues");
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005434 File Offset: 0x00003634
		private void Event_KeyUp(KeyEvent e)
		{
			if (e.KeyCode == RPVoiceChatClient.<Event_KeyUp>g__HotkeyCode|28_0("voicechatMenu"))
			{
				this.voiceMenuPressed = false;
			}
			if (e.KeyCode == RPVoiceChatClient.<Event_KeyUp>g__HotkeyCode|28_0("voicechatVoiceLevel"))
			{
				this.voiceLevelPressed = false;
			}
			if (e.KeyCode == RPVoiceChatClient.<Event_KeyUp>g__HotkeyCode|28_0("voicechatMute"))
			{
				this.mutePressed = false;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000548C File Offset: 0x0000368C
		private void OnAudioReceived(AudioPacket packet)
		{
			if (!this.isReady)
			{
				return;
			}
			this.audioOutputManager.HandleAudioPacket(packet);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000054A4 File Offset: 0x000036A4
		private void OnBufferRecorded(AudioData audioData)
		{
			if (audioData.data == null || audioData.data.Length == 0)
			{
				return;
			}
			string playerUID = RPVoiceChatMod.capi.World.Player.PlayerUID;
			long sequenceNumber = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			AudioPacket packet = new AudioPacket(playerUID, audioData, sequenceNumber);
			this.audioOutputManager.HandleLoopback(packet);
			if (this.microphoneManager.AudioWizardActive)
			{
				return;
			}
			this.client.SendAudioToServer(packet);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005514 File Offset: 0x00003714
		private void OnAnnounceReceived(AnnouncePacket packet)
		{
			int callbellIndex = RPVoiceChatMod.capi.World.Rand.Next(1, 4);
			string modID = RPVoiceChatMod.modID;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(35, 1);
			defaultInterpolatedStringHandler.AppendLiteral("sounds/block/callbell/callbell_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(callbellIndex);
			defaultInterpolatedStringHandler.AppendLiteral(".ogg");
			AssetLocation callbellSound = new AssetLocation(modID, defaultInterpolatedStringHandler.ToStringAndClear());
			RPVoiceChatMod.capi.World.PlaySoundFor(callbellSound, RPVoiceChatMod.capi.World.Player, true, float.MaxValue, 0.7f);
			GuiManager guiManager = this.guiManager;
			if (((guiManager != null) ? guiManager.announce : null) != null)
			{
				this.guiManager.announce.ShowAnnouncement(packet.Title, packet.Message, packet.Duration, packet.ShowBackground);
				return;
			}
			Logger.client.Warning("GuiManager or announce is null, cannot display announcement");
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000055EA File Offset: 0x000037EA
		private void OnNametagConfigChanged(NametagConfigChangedPacket packet)
		{
			if (packet == null)
			{
				return;
			}
			WorldConfig.Set("force-speaker-nametag", packet.ForceSpeakerNametag);
			WorldConfig.Set("player-nametag-targeted-only", packet.PlayerNametagTargetedOnly);
			WorldConfig.Set("use-nametag-dynamic-range", packet.UseNametagDynamicRange);
			PlayerNameTagRenderer.RefreshAllPlayerNameTags();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005628 File Offset: 0x00003828
		public override void Dispose()
		{
			try
			{
				ModConfig.SaveClient(RPVoiceChatMod.capi);
				PlayerNameTagRenderer.CleanupAllNametagCache();
				MicrophoneManager microphoneManager = this.microphoneManager;
				if (microphoneManager != null)
				{
					microphoneManager.Dispose();
				}
				AudioOutputManager audioOutputManager = this.audioOutputManager;
				if (audioOutputManager != null)
				{
					audioOutputManager.Dispose();
				}
				PlayerNetworkClient playerNetworkClient = this.client;
				if (playerNetworkClient != null)
				{
					playerNetworkClient.Dispose();
				}
				GuiManager guiManager = this.guiManager;
				if (guiManager != null)
				{
					guiManager.Dispose();
				}
				ClientSettingsRepository clientSettingsRepository = this.clientSettingsRepository;
				if (clientSettingsRepository != null)
				{
					clientSettingsRepository.Dispose();
				}
			}
			catch (Exception e)
			{
				Logger.client.Warning("Error during disposal: " + e.Message);
			}
			finally
			{
				try
				{
					RPVoiceChatMod.capi.Event.KeyUp -= new KeyEventDelegate(this.Event_KeyUp);
					RPVoiceChatMod.capi.Event.LevelFinalize -= new Action(this.OnLoad);
				}
				catch (Exception e2)
				{
					Logger.client.Warning("Error unsubscribing events: " + e2.Message);
				}
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000057D9 File Offset: 0x000039D9
		[CompilerGenerated]
		internal static int <Event_KeyUp>g__HotkeyCode|28_0(string hotkeyName)
		{
			return RPVoiceChatMod.capi.Input.HotKeys[hotkeyName].CurrentMapping.KeyCode;
		}

		// Token: 0x04000036 RID: 54
		private ClientSettingsRepository clientSettingsRepository;

		// Token: 0x04000037 RID: 55
		private MicrophoneManager microphoneManager;

		// Token: 0x04000038 RID: 56
		private AudioOutputManager audioOutputManager;

		// Token: 0x04000039 RID: 57
		private PlayerNetworkClient client;

		// Token: 0x0400003A RID: 58
		private GuiManager guiManager;

		// Token: 0x0400003B RID: 59
		private VoiceBanClientManager voiceBanManager;

		// Token: 0x0400003C RID: 60
		private bool isReady;

		// Token: 0x0400003D RID: 61
		private bool mutePressed;

		// Token: 0x0400003E RID: 62
		private bool voiceMenuPressed;

		// Token: 0x0400003F RID: 63
		private bool voiceLevelPressed;

		// Token: 0x04000043 RID: 67
		private static bool macosMicWarningShown;
	}
}
