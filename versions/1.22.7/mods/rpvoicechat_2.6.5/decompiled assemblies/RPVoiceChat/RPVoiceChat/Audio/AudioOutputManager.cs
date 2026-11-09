using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using RPVoiceChat.Config;
using RPVoiceChat.DB;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace RPVoiceChat.Audio
{
	// Token: 0x02000103 RID: 259
	public class AudioOutputManager : IDisposable
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x0003431A File Offset: 0x0003251A
		// (set) Token: 0x06000976 RID: 2422 RVA: 0x00034322 File Offset: 0x00032522
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

		// Token: 0x06000977 RID: 2423 RVA: 0x00034353 File Offset: 0x00032553
		public AudioOutputManager(ICoreClientAPI api, ClientSettingsRepository settingsRepository)
		{
			this.IsLoopbackEnabled = ModConfig.ClientConfig.Loopback;
			this.capi = api;
			this.clientSettingsRepo = settingsRepository;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00034390 File Offset: 0x00032590
		public void Launch()
		{
			PlayerListener.Init(this.capi);
			this.capi.Event.PlayerEntitySpawn += new PlayerEventDelegate(this.PlayerSpawned);
			this.capi.Event.PlayerEntityDespawn += new PlayerEventDelegate(this.PlayerDespawned);
			this.ClientLoaded();
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x000343E8 File Offset: 0x000325E8
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
			if (!RadioProgramRouteKey.IsProgramSource(packet.PlayerId) && RPVoiceChatClient.VoiceBanManagerInstance != null && RPVoiceChatClient.VoiceBanManagerInstance.IsPlayerBanned(packet.PlayerId))
			{
				return;
			}
			if (this.IsOwnTalkieRfReception(packet))
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

		// Token: 0x0600097A RID: 2426 RVA: 0x00034490 File Offset: 0x00032690
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
			source.PrepareForPacket(audioData);
			source.UpdatePlayer();
			source.UpdateAudioFormat(codec, frequency, channels);
			source.EnqueueAudio(audioData, packet.SequenceNumber);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x000344FB File Offset: 0x000326FB
		public void HandleLoopback(AudioPacket packet)
		{
			if (!this.IsLoopbackEnabled)
			{
				return;
			}
			this.HandleAudioPacket(packet, this.localPlayerAudioSource);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00034514 File Offset: 0x00032714
		private bool IsOwnTalkieRfReception(AudioPacket packet)
		{
			if (!packet.HasSourcePositionOverride || this.capi.World.Player == null)
			{
				return false;
			}
			if (packet.PlayerId != this.capi.World.Player.PlayerUID)
			{
				return false;
			}
			EntityPlayer entity = this.capi.World.Player.Entity;
			EntityPos listenerPos = (entity != null) ? entity.Pos : null;
			if (listenerPos == null)
			{
				return false;
			}
			Vec3d sourcePos = new Vec3d(packet.SourcePosX, packet.SourcePosY, packet.SourcePosZ);
			if (BlockEntityRadioReceiver.GetPlaybackVolumeGainAtSource(this.capi, sourcePos, listenerPos.Dimension) >= 0f)
			{
				return false;
			}
			double num = packet.SourcePosX - listenerPos.X;
			double dy = packet.SourcePosY - listenerPos.Y;
			double dz = packet.SourcePosZ - listenerPos.Z;
			return num * num + dy * dy + dz * dz <= 4.0;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x000345FC File Offset: 0x000327FC
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

		// Token: 0x0600097E RID: 2430 RVA: 0x0003464C File Offset: 0x0003284C
		private PlayerAudioSource GetOrCreatePlayerSource(string playerId)
		{
			PlayerAudioSource source;
			if (this.playerSources.TryGetValue(playerId, ref source) && !source.IsDisposed)
			{
				return source;
			}
			if (RadioProgramRouteKey.IsProgramSource(playerId))
			{
				return this.CreateSyntheticSource(playerId);
			}
			IPlayer player = this.capi.World.PlayerByUid(playerId);
			if (player == null)
			{
				return null;
			}
			return this.CreatePlayerSource(player);
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x000346A4 File Offset: 0x000328A4
		private PlayerAudioSource CreatePlayerSource(IPlayer player)
		{
			PlayerAudioSource source = new PlayerAudioSource(player, this.capi, this.clientSettingsRepo);
			this.playerSources.AddOrUpdate(player.PlayerUID, source, ([Nullable(1)] string _, PlayerAudioSource __) => source);
			return source;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x000346FC File Offset: 0x000328FC
		private PlayerAudioSource CreateSyntheticSource(string sourceId)
		{
			PlayerAudioSource source = new PlayerAudioSource(this.capi.World.Player, this.capi, this.clientSettingsRepo, sourceId);
			this.playerSources.AddOrUpdate(sourceId, source, ([Nullable(1)] string _, PlayerAudioSource __) => source);
			return source;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0003475C File Offset: 0x0003295C
		private void PlayerSpawned(IPlayer player)
		{
			if (player.ClientId == this.capi.World.Player.ClientId)
			{
				return;
			}
			this.CreatePlayerSource(player);
			PlayerNameTagRenderer.UpdatePlayerNameTag(player, false);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0003478C File Offset: 0x0003298C
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

		// Token: 0x06000983 RID: 2435 RVA: 0x00034810 File Offset: 0x00032A10
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

		// Token: 0x06000984 RID: 2436 RVA: 0x00034878 File Offset: 0x00032A78
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

		// Token: 0x06000985 RID: 2437 RVA: 0x0003489C File Offset: 0x00032A9C
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

		// Token: 0x06000986 RID: 2438 RVA: 0x000349A4 File Offset: 0x00032BA4
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

		// Token: 0x06000987 RID: 2439 RVA: 0x000349D4 File Offset: 0x00032BD4
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

		// Token: 0x04000430 RID: 1072
		private ICoreClientAPI capi;

		// Token: 0x04000431 RID: 1073
		private bool isLoopbackEnabled;

		// Token: 0x04000432 RID: 1074
		private ConcurrentDictionary<string, string> playerEffects = new ConcurrentDictionary<string, string>();

		// Token: 0x04000433 RID: 1075
		private ConcurrentDictionary<string, PlayerAudioSource> playerSources = new ConcurrentDictionary<string, PlayerAudioSource>();

		// Token: 0x04000434 RID: 1076
		private PlayerAudioSource localPlayerAudioSource;

		// Token: 0x04000435 RID: 1077
		private ClientSettingsRepository clientSettingsRepo;
	}
}
