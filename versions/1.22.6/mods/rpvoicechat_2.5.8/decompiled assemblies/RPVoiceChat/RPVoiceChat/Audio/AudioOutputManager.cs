using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using RPVoiceChat.Config;
using RPVoiceChat.DB;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000CA RID: 202
	public class AudioOutputManager : IDisposable
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x00028C42 File Offset: 0x00026E42
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x00028C4A File Offset: 0x00026E4A
		public bool IsLoopbackEnabled
		{
			get
			{
				return this.isLoopbackEnabled;
			}
			set
			{
				this.isLoopbackEnabled = value;
				if (this.localPlayerAudioSource == null)
				{
					return;
				}
				if (this.isLoopbackEnabled)
				{
					this.localPlayerAudioSource.StartPlaying();
					return;
				}
				this.localPlayerAudioSource.StopPlaying();
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00028C7B File Offset: 0x00026E7B
		public AudioOutputManager(ICoreClientAPI api, ClientSettingsRepository settingsRepository)
		{
			this.IsLoopbackEnabled = ModConfig.ClientConfig.Loopback;
			this.capi = api;
			this.clientSettingsRepo = settingsRepository;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00028CB8 File Offset: 0x00026EB8
		public void Launch()
		{
			PlayerListener.Init(this.capi);
			this.capi.Event.PlayerEntitySpawn += new PlayerEventDelegate(this.PlayerSpawned);
			this.capi.Event.PlayerEntityDespawn += new PlayerEventDelegate(this.PlayerDespawned);
			this.ClientLoaded();
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00028D10 File Offset: 0x00026F10
		public void HandleAudioPacket(AudioPacket packet)
		{
			if (packet.AudioData == null || packet.AudioData.Length == 0)
			{
				Logger.client.Debug("Received empty audio packet, dropping");
				return;
			}
			if (packet.AudioData.Length != packet.Length)
			{
				Logger.client.Debug("Audio packet payload had invalid length, dropping packet");
				return;
			}
			if (RPVoiceChatClient.VoiceBanManagerInstance != null && RPVoiceChatClient.VoiceBanManagerInstance.IsPlayerBanned(packet.PlayerId))
			{
				return;
			}
			PlayerAudioSource source = this.GetOrCreatePlayerSource(packet.PlayerId);
			if (source == null)
			{
				Logger.client.Debug("Unable to resolve player ID into player source, dropping packet");
				return;
			}
			this.HandleAudioPacket(packet, source);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00028DA0 File Offset: 0x00026FA0
		public void HandleAudioPacket(AudioPacket packet, PlayerAudioSource source)
		{
			string codec = packet.Codec;
			int frequency = packet.Frequency;
			int channels = AudioUtils.ChannelsPerFormat(packet.Format);
			AudioData audioData = AudioData.FromPacket(packet);
			if (source.voiceLevel != packet.VoiceLevel)
			{
				source.UpdateVoiceLevel(packet.VoiceLevel);
			}
			source.UpdatePlayer();
			source.UpdateAudioFormat(codec, frequency, channels);
			source.EnqueueAudio(audioData, packet.SequenceNumber);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00028E04 File Offset: 0x00027004
		public void HandleLoopback(AudioPacket packet)
		{
			if (!this.IsLoopbackEnabled)
			{
				return;
			}
			this.HandleAudioPacket(packet, this.localPlayerAudioSource);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00028E1C File Offset: 0x0002701C
		private void ClientLoaded()
		{
			this.localPlayerAudioSource = new PlayerAudioSource(this.capi.World.Player, this.capi, this.clientSettingsRepo)
			{
				IsLocational = false
			};
			if (!this.isLoopbackEnabled)
			{
				return;
			}
			this.localPlayerAudioSource.StartPlaying();
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00028E6C File Offset: 0x0002706C
		private PlayerAudioSource GetOrCreatePlayerSource(string playerId)
		{
			PlayerAudioSource source;
			if (this.playerSources.TryGetValue(playerId, ref source) && !source.IsDisposed)
			{
				return source;
			}
			IPlayer player = this.capi.World.PlayerByUid(playerId);
			if (player == null)
			{
				return null;
			}
			return this.CreatePlayerSource(player);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00028EB4 File Offset: 0x000270B4
		private PlayerAudioSource CreatePlayerSource(IPlayer player)
		{
			PlayerAudioSource source = new PlayerAudioSource(player, this.capi, this.clientSettingsRepo);
			this.playerSources.AddOrUpdate(player.PlayerUID, source, ([Nullable(1)] string _, PlayerAudioSource __) => source);
			return source;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00028F09 File Offset: 0x00027109
		private void PlayerSpawned(IPlayer player)
		{
			if (player.ClientId == this.capi.World.Player.ClientId)
			{
				return;
			}
			this.CreatePlayerSource(player);
			PlayerNameTagRenderer.UpdatePlayerNameTag(player, false);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00028F38 File Offset: 0x00027138
		private void PlayerDespawned(IPlayer player)
		{
			if (player.ClientId == this.capi.World.Player.ClientId)
			{
				this.localPlayerAudioSource.Dispose();
				this.localPlayerAudioSource = null;
				PlayerNameTagRenderer.CleanupPlayerNametagCache(player.PlayerUID);
				return;
			}
			PlayerAudioSource source;
			this.playerSources.TryGetValue(player.PlayerUID, ref source);
			if (source != null)
			{
				source.Dispose();
			}
			DictExtensions.Remove<string, PlayerAudioSource>(this.playerSources, player.PlayerUID);
			PlayerNameTagRenderer.CleanupPlayerNametagCache(player.PlayerUID);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00028FBC File Offset: 0x000271BC
		public bool IsPlayerTalking(string playerId)
		{
			PlayerAudioSource source;
			if (this.playerSources.TryGetValue(playerId, ref source))
			{
				return source.IsPlaying;
			}
			if (this.capi.World.Player.PlayerUID == playerId)
			{
				return this.localPlayerAudioSource.IsPlaying;
			}
			Logger.client.Warning("Could not find player audio source for " + playerId + ", assuming player isn't talking");
			return false;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00029024 File Offset: 0x00027224
		public bool SetVoiceLevelForPlayer(string playerId, VoiceLevel voiceLevel)
		{
			PlayerAudioSource source = this.GetOrCreatePlayerSource(playerId);
			if (source == null)
			{
				return false;
			}
			source.UpdateVoiceLevel(voiceLevel);
			return true;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00029048 File Offset: 0x00027248
		public void Dispose()
		{
			try
			{
				PlayerListener.Dispose();
				PlayerAudioSource playerAudioSource = this.localPlayerAudioSource;
				if (playerAudioSource != null)
				{
					playerAudioSource.Dispose();
				}
				foreach (PlayerAudioSource playerAudioSource2 in this.playerSources.Values)
				{
					if (playerAudioSource2 != null)
					{
						playerAudioSource2.Dispose();
					}
				}
			}
			catch (Exception e)
			{
				Logger.client.Warning("Error disposing audio output manager: " + e.Message);
			}
			finally
			{
				try
				{
					this.capi.Event.PlayerEntitySpawn -= new PlayerEventDelegate(this.PlayerSpawned);
					this.capi.Event.PlayerEntityDespawn -= new PlayerEventDelegate(this.PlayerDespawned);
				}
				catch (Exception e2)
				{
					Logger.client.Warning("Error unsubscribing audio output events: " + e2.Message);
				}
			}
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00029150 File Offset: 0x00027350
		public bool ApplyEffectToPlayer(string playerId, string effectName)
		{
			PlayerAudioSource source = this.GetOrCreatePlayerSource(playerId);
			if (source == null)
			{
				return false;
			}
			source.SetSoundEffect(effectName);
			this.playerEffects[playerId] = effectName;
			return true;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00029180 File Offset: 0x00027380
		public bool ClearEffectForPlayer(string playerId)
		{
			string text;
			if (this.playerEffects.TryRemove(playerId, ref text))
			{
				PlayerAudioSource orCreatePlayerSource = this.GetOrCreatePlayerSource(playerId);
				if (orCreatePlayerSource != null)
				{
					orCreatePlayerSource.ClearSoundEffect();
				}
				return true;
			}
			return false;
		}

		// Token: 0x0400034F RID: 847
		private ICoreClientAPI capi;

		// Token: 0x04000350 RID: 848
		private bool isLoopbackEnabled;

		// Token: 0x04000351 RID: 849
		private ConcurrentDictionary<string, string> playerEffects = new ConcurrentDictionary<string, string>();

		// Token: 0x04000352 RID: 850
		private ConcurrentDictionary<string, PlayerAudioSource> playerSources = new ConcurrentDictionary<string, PlayerAudioSource>();

		// Token: 0x04000353 RID: 851
		private PlayerAudioSource localPlayerAudioSource;

		// Token: 0x04000354 RID: 852
		private ClientSettingsRepository clientSettingsRepo;
	}
}
