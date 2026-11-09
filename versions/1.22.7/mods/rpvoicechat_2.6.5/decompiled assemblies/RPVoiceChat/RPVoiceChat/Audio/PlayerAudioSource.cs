using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RPVoiceChat.Audio.Effects;
using RPVoiceChat.Config;
using RPVoiceChat.DB;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Items;
using RPVoiceChat.Gui;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Audio
{
	// Token: 0x02000105 RID: 261
	public class PlayerAudioSource : IDisposable
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x00034E14 File Offset: 0x00033014
		public bool IsPlaying
		{
			get
			{
				return this._IsPlaying();
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x00034E1C File Offset: 0x0003301C
		public bool IsSyntheticSource { get; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x00034E24 File Offset: 0x00033024
		public float MaxGain
		{
			get
			{
				return ServerConfigManager.MaxAudioGain;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x00034E2B File Offset: 0x0003302B
		public int SourceId
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x00034E33 File Offset: 0x00033033
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x00034E3B File Offset: 0x0003303B
		public bool IsLocational { get; set; } = true;

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x00034E44 File Offset: 0x00033044
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x00034E4C File Offset: 0x0003304C
		public VoiceLevel voiceLevel { get; private set; } = VoiceLevel.Talking;

		// Token: 0x06000998 RID: 2456 RVA: 0x00034E55 File Offset: 0x00033055
		public PlayerAudioSource(IPlayer player, ICoreClientAPI capi, ClientSettingsRepository clientSettingsRepo) : this(player, capi, clientSettingsRepo, null)
		{
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00034E64 File Offset: 0x00033064
		public PlayerAudioSource(IPlayer player, ICoreClientAPI capi, ClientSettingsRepository clientSettingsRepo, string syntheticSourceId)
		{
			this.player = player;
			this.capi = capi;
			this.clientSettingsRepo = clientSettingsRepo;
			this.IsSyntheticSource = !string.IsNullOrWhiteSpace(syntheticSourceId);
			this.orderingDelay = (this.IsSyntheticSource ? 250 : 50);
			EntityPlayer entity = player.Entity;
			Vec3f vec3f;
			if (entity == null)
			{
				vec3f = null;
			}
			else
			{
				EntityPos pos = entity.Pos;
				vec3f = ((pos != null) ? pos.XYZFloat : null);
			}
			this.lastSpeakerCoords = vec3f;
			this.lastSpeakerUpdate = new DateTime?(DateTime.Now);
			this.source = OALW.GenSource();
			this.buffer = new CircularAudioBuffer(this.source, this.IsSyntheticSource ? 40 : 20);
			this.buffer.OnEmptyingQueue += new Action(this.OnSourceStop);
			float gain = this.GetFinalGain();
			OALW.Source(this.source, 4103, false);
			OALW.Source(this.source, 514, true);
			OALW.Source(this.source, 4106, gain);
			OALW.Source(this.source, 4099, 1f);
			OALW.Source(this.source, 4128, 1f);
			OALW.Source(this.source, 4129, 0f);
			this.UpdateVoiceLevel(this.voiceLevel);
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00034FFA File Offset: 0x000331FA
		public void PrepareForPacket(AudioData audio)
		{
			this.currentAudio = audio;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00035003 File Offset: 0x00033203
		public void UpdateVoiceLevel(VoiceLevel voiceLevel)
		{
			this.voiceLevel = voiceLevel;
			this.TryApplyNametagRenderRange();
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00035014 File Offset: 0x00033214
		private void TryApplyNametagRenderRange()
		{
			if (this.IsSyntheticSource)
			{
				return;
			}
			int targetRange = WorldConfig.GetBool("use-nametag-dynamic-range", true) ? WorldConfig.GetInt(this.voiceLevel, default(int?)) : WorldConfig.GetInt("nametag-fallback-range", ServerConfigManager.NametagFallbackRenderRange);
			int? lastQueuedNametagRenderRange = this._lastQueuedNametagRenderRange;
			int num = targetRange;
			if (lastQueuedNametagRenderRange.GetValueOrDefault() == num & lastQueuedNametagRenderRange != null)
			{
				return;
			}
			this._lastQueuedNametagRenderRange = new int?(targetRange);
			PlayerNameTagRenderer.SetNametagRenderRange(this.player, targetRange);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00035094 File Offset: 0x00033294
		public void UpdateAudioFormat(string codecName, int frequency, int channels)
		{
			IAudioCodec audioCodec = this.codec;
			if (((audioCodec != null) ? audioCodec.Name : null) == codecName)
			{
				IAudioCodec audioCodec2 = this.codec;
				if (audioCodec2 != null && audioCodec2.SampleRate == frequency)
				{
					IAudioCodec audioCodec3 = this.codec;
					if (audioCodec3 != null && audioCodec3.Channels == channels)
					{
						return;
					}
				}
			}
			IAudioCodec audioCodec4;
			if (!(codecName == "Opus"))
			{
				if (!(codecName == "Dummy"))
				{
					audioCodec4 = null;
				}
				else
				{
					audioCodec4 = new DummyCodec(frequency, channels);
				}
			}
			else
			{
				audioCodec4 = new OpusCodec(frequency, channels);
			}
			this.codec = audioCodec4;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00035124 File Offset: 0x00033324
		public void UpdatePlayer()
		{
			EntityPlayer entity = this.player.Entity;
			EntityPos speakerPos = (entity != null) ? entity.Pos : null;
			EntityPlayer entity2 = this.capi.World.Player.Entity;
			EntityPos listenerPos = (entity2 != null) ? entity2.Pos : null;
			if (speakerPos == null || listenerPos == null)
			{
				return;
			}
			this.TryApplyNametagRenderRange();
			AudioData audioData = this.currentAudio;
			Vec3d sourceOverride = (audioData != null) ? audioData.sourcePosOverride : null;
			Vec3d effectiveSpeakerPos = sourceOverride ?? new Vec3d(speakerPos.X, speakerPos.Y, speakerPos.Z);
			DateTime now = DateTime.Now;
			bool flag = this.lastFullUpdate == null || (now - this.lastFullUpdate.Value).TotalMilliseconds >= 50.0;
			object obj = this.lastWallThicknessUpdate == null || (now - this.lastWallThicknessUpdate.Value).TotalMilliseconds >= 200.0;
			bool mufflingEnabled = ModConfig.ClientConfig.Muffling;
			bool useSoundPhysics = mufflingEnabled && WorldConfig.GetBool("use-sound-physics-adapted", true) && SoundPhysicsCompatibility.IsAvailable;
			float wallThickness = this.cachedWallThickness;
			object obj2 = obj;
			if (obj2 != null)
			{
				if (mufflingEnabled && !useSoundPhysics)
				{
					if (sourceOverride != null)
					{
						wallThickness = LocationUtils.GetWallThickness(this.capi, sourceOverride, LocationUtils.GetLocationOfPlayer(this.capi.World.Player));
					}
					else
					{
						wallThickness = LocationUtils.GetWallThickness(this.capi, this.player, this.capi.World.Player);
					}
					if (this.capi.World.Player.Entity.Swimming)
					{
						wallThickness += 1f;
					}
					this.cachedWallThickness = wallThickness;
					this.lastWallThicknessUpdate = new DateTime?(now);
				}
				else
				{
					wallThickness = 0f;
					this.cachedWallThickness = 0f;
					if (useSoundPhysics)
					{
						this.lastWallThicknessUpdate = new DateTime?(now);
					}
				}
			}
			else if (this.capi.World.Player.Entity.Swimming && this.cachedWallThickness > 0f)
			{
				wallThickness = this.cachedWallThickness + 1f;
			}
			if (obj2 != null)
			{
				LowpassFilter lowpassFilter = this.lowpassFilter;
				if (lowpassFilter != null)
				{
					lowpassFilter.Stop();
				}
				if (!useSoundPhysics)
				{
					this.cachedSoundPhysicsGainHF = 1f;
				}
				if (mufflingEnabled)
				{
					float gainHF = 1f;
					if (useSoundPhysics)
					{
						Vec3d speakerLocation = sourceOverride ?? LocationUtils.GetLocationOfPlayer(this.player);
						Vec3d listenerLocation = LocationUtils.GetLocationOfPlayer(this.capi.World.Player);
						this.QueueSoundPhysicsQuery(speakerLocation, listenerLocation);
						gainHF = this.cachedSoundPhysicsGainHF;
					}
					else if (wallThickness != 0f)
					{
						float wallThicknessWeighting = WorldConfig.GetFloat("wall-thickness-weighting", 0f);
						gainHF = Math.Max(1f - wallThickness / wallThicknessWeighting, 0.1f);
					}
					if (gainHF < 1f)
					{
						this.lowpassFilter = (this.lowpassFilter ?? new LowpassFilter(this.source));
						this.lowpassFilter.Start();
						this.lowpassFilter.SetHFGain(gainHF);
					}
				}
			}
			if (!flag)
			{
				return;
			}
			this.lastFullUpdate = new DateTime?(now);
			int num = 0;
			ReverbEffect reverbEffect = this.reverbEffect;
			if (reverbEffect != null)
			{
				reverbEffect.Clear();
			}
			if (num != 0 && LocationUtils.IsReverbArea(this.capi, speakerPos))
			{
				this.reverbEffect = (this.reverbEffect ?? new ReverbEffect(this.source));
				this.reverbEffect.Apply();
			}
			UnstableEffect unstableEffect = this.unstableEffect;
			if (unstableEffect != null)
			{
				unstableEffect.Clear();
			}
			if (num != 0 && this.player.Entity.WatchedAttributes.GetDouble("temporalStability", 0.0) < 0.5)
			{
				this.unstableEffect = (this.unstableEffect ?? new UnstableEffect(this.source));
				this.unstableEffect.Apply();
			}
			IntoxicatedEffect intoxicatedEffect = this.intoxicatedEffect;
			if (intoxicatedEffect != null)
			{
				intoxicatedEffect.Clear();
			}
			float drunkness = this.player.Entity.WatchedAttributes.GetFloat("intoxication", 0f);
			if (num != 0 && drunkness > 0f)
			{
				this.intoxicatedEffect = (this.intoxicatedEffect ?? new IntoxicatedEffect(this.source));
				this.intoxicatedEffect.SetToxicRate(drunkness);
				this.intoxicatedEffect.Apply();
			}
			float gain = this.GetFinalGain() * this.GetDistanceAttenuationGain(effectiveSpeakerPos, listenerPos);
			if (sourceOverride != null && PlayerAudioSource.IsTalkieRfAtListener(sourceOverride, listenerPos))
			{
				gain *= ItemRadio.GetLocalTalkieListenVolumeGain(this.capi);
			}
			else if (sourceOverride != null)
			{
				float receiverGain = BlockEntityRadioReceiver.GetPlaybackVolumeGainAtSource(this.capi, sourceOverride, listenerPos.Dimension);
				if (receiverGain >= 0f)
				{
					gain *= receiverGain;
				}
			}
			Vec3f sourcePosition = new Vec3f();
			Vec3f velocity = new Vec3f();
			if (this.IsLocational && !ModConfig.ClientConfig.IsMonoMode)
			{
				sourcePosition = this.GetRelativeSourcePosition(effectiveSpeakerPos, listenerPos);
				velocity = this.GetRelativeVelocity(effectiveSpeakerPos, listenerPos, sourcePosition);
			}
			else if (ModConfig.ClientConfig.IsMonoMode)
			{
				float distance = effectiveSpeakerPos.DistanceTo(listenerPos.XYZ);
				sourcePosition = new Vec3f(0f, 0f, distance);
				velocity = new Vec3f();
			}
			OALW.ClearError();
			OALW.Source(this.source, 4106, gain);
			OALW.Source(this.source, 4100, sourcePosition.X, sourcePosition.Y, sourcePosition.Z);
			OALW.Source(this.source, 4102, velocity.X, velocity.Y, velocity.Z);
			OALW.Source(this.source, 514, true);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x000356AD File Offset: 0x000338AD
		private bool _IsPlaying()
		{
			return this.source > 0 && OALW.GetSourceState(this.source) == 4114;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x000356CC File Offset: 0x000338CC
		private void QueueSoundPhysicsQuery(Vec3d speakerLocation, Vec3d listenerLocation)
		{
			if (this.soundPhysicsQueryPending)
			{
				return;
			}
			this.soundPhysicsQueryPending = true;
			this.capi.Event.EnqueueMainThreadTask(delegate()
			{
				try
				{
					this.cachedSoundPhysicsGainHF = SoundPhysicsCompatibility.GetOcclusionGainHF(speakerLocation, listenerLocation);
				}
				finally
				{
					this.soundPhysicsQueryPending = false;
				}
			}, "rpvoicechat:SoundPhysicsOcclusion");
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0003572C File Offset: 0x0003392C
		private float GetFinalGain()
		{
			float num = Math.Clamp(PlayerListener.VoiceGain, 0f, this.MaxGain);
			float sourceGain = this.IsSyntheticSource ? 1f : this.clientSettingsRepo.GetPlayerGain(this.player.PlayerUID);
			return GameMath.Clamp(num * sourceGain, 0f, this.MaxGain);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00035788 File Offset: 0x00033988
		private float GetDistanceAttenuationGain(Vec3d speakerPos, EntityPos listenerPos)
		{
			AudioData audioData = this.currentAudio;
			if (audioData == null || !audioData.isGlobalBroadcast)
			{
				AudioData audioData2 = this.currentAudio;
				if (audioData2 == null || !audioData2.ignoreDistanceReduction)
				{
					AudioData audioData3 = this.currentAudio;
					float maxHearingDistance = (float)((audioData3 != null && audioData3.effectiveRange > 0) ? this.currentAudio.effectiveRange : WorldConfig.GetInt(this.voiceLevel, default(int?)));
					if (maxHearingDistance <= 0.01f)
					{
						return 0f;
					}
					float t = GameMath.Clamp(speakerPos.DistanceTo(listenerPos.XYZ) / maxHearingDistance, 0f, 1f);
					float shaped = (float)Math.Pow(1.0 - (double)t, 1.35);
					return 0.06f + 0.94f * shaped;
				}
			}
			return 1f;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00035854 File Offset: 0x00033A54
		private static bool IsTalkieRfAtListener(Vec3d sourceOverride, EntityPos listenerPos)
		{
			double num = sourceOverride.X - listenerPos.X;
			double dy = sourceOverride.Y - listenerPos.Y;
			double dz = sourceOverride.Z - listenerPos.Z;
			return num * num + dy * dy + dz * dz <= 4.0;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000358A2 File Offset: 0x00033AA2
		private Vec3f GetRelativeSourcePosition(Vec3d speakerPos, EntityPos listenerPos)
		{
			return LocationUtils.GetRelativeSpeakerLocation(speakerPos.ToVec3f(), listenerPos);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x000358B0 File Offset: 0x00033AB0
		private Vec3f GetRelativeVelocity(Vec3d speakerPos, EntityPos listenerPos, Vec3f relativeSpeakerPosition)
		{
			Vec3f speakerVelocity = this.GetVelocity(speakerPos);
			Vec3f relativeFuturePosition = LocationUtils.GetRelativeSpeakerLocation(speakerPos.ToVec3f() + speakerVelocity, listenerPos);
			return relativeSpeakerPosition - relativeFuturePosition;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x000358E0 File Offset: 0x00033AE0
		private Vec3f GetVelocity(Vec3d speakerPos)
		{
			DateTime currentTime = DateTime.Now;
			if (this.lastSpeakerUpdate == null)
			{
				this.lastSpeakerUpdate = new DateTime?(currentTime);
			}
			double dt = (currentTime - this.lastSpeakerUpdate.Value).TotalSeconds;
			dt = GameMath.Clamp(dt, 0.1, 1.0);
			Vec3f speakerCoords = speakerPos.ToVec3f();
			if (this.lastSpeakerCoords == null || dt == 1.0)
			{
				this.lastSpeakerCoords = speakerCoords;
			}
			Vec3f result = (this.lastSpeakerCoords - speakerCoords) / (float)dt;
			this.lastSpeakerCoords = speakerCoords;
			this.lastSpeakerUpdate = new DateTime?(currentTime);
			return result;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00035990 File Offset: 0x00033B90
		public void EnqueueAudio(AudioData audio, long sequenceNumber)
		{
			object obj = this.ordering_queue_lock;
			lock (obj)
			{
				if (this.orderingQueue.ContainsKey(sequenceNumber))
				{
					return;
				}
				if (this.lastAudioSequenceNumber >= 0L && sequenceNumber + 50L < this.lastAudioSequenceNumber)
				{
					this.orderingQueue.Clear();
					this.lastAudioSequenceNumber = -1L;
					this.syntheticPlaybackPrimed = false;
				}
				if (this.lastAudioSequenceNumber >= sequenceNumber)
				{
					Logger client = Logger.client;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Audio sequence ");
					defaultInterpolatedStringHandler.AppendFormatted<long>(sequenceNumber);
					defaultInterpolatedStringHandler.AppendLiteral(" arrived too late, skipping enqueueing");
					client.VerboseDebug(defaultInterpolatedStringHandler.ToStringAndClear());
					return;
				}
				this.orderingQueue.Add(sequenceNumber, audio);
				if (this.IsSyntheticSource)
				{
					while (this.orderingQueue.Count > 80)
					{
						this.orderingQueue.RemoveAt(0);
					}
				}
			}
			if (!this.dequeueTaskRunning)
			{
				this.DequeueAudio();
			}
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00035A94 File Offset: 0x00033C94
		public void DequeueAudio()
		{
			PlayerAudioSource.<DequeueAudio>d__70 <DequeueAudio>d__;
			<DequeueAudio>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DequeueAudio>d__.<>4__this = this;
			<DequeueAudio>d__.<>1__state = -1;
			<DequeueAudio>d__.<>t__builder.Start<PlayerAudioSource.<DequeueAudio>d__70>(ref <DequeueAudio>d__);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00035ACC File Offset: 0x00033CCC
		private Task DrainSyntheticAudioAsync()
		{
			PlayerAudioSource.<DrainSyntheticAudioAsync>d__71 <DrainSyntheticAudioAsync>d__;
			<DrainSyntheticAudioAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DrainSyntheticAudioAsync>d__.<>4__this = this;
			<DrainSyntheticAudioAsync>d__.<>1__state = -1;
			<DrainSyntheticAudioAsync>d__.<>t__builder.Start<PlayerAudioSource.<DrainSyntheticAudioAsync>d__71>(ref <DrainSyntheticAudioAsync>d__);
			return <DrainSyntheticAudioAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00035B10 File Offset: 0x00033D10
		private Task DrainVoiceAudioAsync()
		{
			PlayerAudioSource.<DrainVoiceAudioAsync>d__72 <DrainVoiceAudioAsync>d__;
			<DrainVoiceAudioAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DrainVoiceAudioAsync>d__.<>4__this = this;
			<DrainVoiceAudioAsync>d__.<>1__state = -1;
			<DrainVoiceAudioAsync>d__.<>t__builder.Start<PlayerAudioSource.<DrainVoiceAudioAsync>d__72>(ref <DrainVoiceAudioAsync>d__);
			return <DrainVoiceAudioAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00035B54 File Offset: 0x00033D54
		private bool TryPreparePcm(ref AudioData audio)
		{
			this.currentAudio = audio;
			this.UpdateVoiceLevel(audio.voiceLevel);
			if (this.codec != null)
			{
				audio.data = this.codec.Decode(audio.data);
			}
			if (audio.data == null || audio.data.Length == 0)
			{
				Logger.client.Warning("Received empty audio data, skipping");
				return false;
			}
			float finalGain = this.GetFinalGain();
			PcmUtils.ApplyGainWithSoftClipping(ref audio.data, audio.format, finalGain);
			if (!audio.isGlobalBroadcast && !this.IsSyntheticSource)
			{
				PcmUtils.ApplyCompressor(ref audio.data, audio.format, 0.6f, 4f);
				int maxFadeDuration = Math.Min(2 * audio.frequency / 1000, audio.data.Length / 4);
				if (audio.data.Length > maxFadeDuration * 2)
				{
					AudioUtils.FadeEdges(audio.data, maxFadeDuration);
				}
			}
			return true;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00035C40 File Offset: 0x00033E40
		private void EnsurePlaying()
		{
			if (this.source <= 0)
			{
				return;
			}
			if (OALW.GetSourceState(this.source) != 4114)
			{
				this.StartPlaying();
				this.NotifyStartedSpeaking();
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00035C6C File Offset: 0x00033E6C
		private void SchedulePlaybackEndCheck()
		{
			PlayerAudioSource.<SchedulePlaybackEndCheck>d__75 <SchedulePlaybackEndCheck>d__;
			<SchedulePlaybackEndCheck>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<SchedulePlaybackEndCheck>d__.<>4__this = this;
			<SchedulePlaybackEndCheck>d__.<>1__state = -1;
			<SchedulePlaybackEndCheck>d__.<>t__builder.Start<PlayerAudioSource.<SchedulePlaybackEndCheck>d__75>(ref <SchedulePlaybackEndCheck>d__);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00035CA3 File Offset: 0x00033EA3
		public void StartPlaying()
		{
			if (this.source <= 0)
			{
				return;
			}
			OALW.SourcePlay(this.source);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00035CBA File Offset: 0x00033EBA
		private void NotifyStartedSpeaking()
		{
			if (this.IsSyntheticSource)
			{
				return;
			}
			PlayerNameTagRenderer.UpdatePlayerNameTag(this.player, true);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00035CD1 File Offset: 0x00033ED1
		public void StopPlaying()
		{
			if (this.source <= 0 || this.IsDisposed)
			{
				return;
			}
			OALW.SourceStop(this.source);
			this.OnSourceStop();
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00035CF6 File Offset: 0x00033EF6
		private void OnSourceStop()
		{
			if (this.IsSyntheticSource)
			{
				return;
			}
			PlayerNameTagRenderer.UpdatePlayerNameTag(this.player, false);
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00035D10 File Offset: 0x00033F10
		public void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			if (this.source > 0)
			{
				OALW.SourceStop(this.source);
				OALW.DeleteSource(this.source);
			}
			this.source = 0;
			this.buffer.OnEmptyingQueue -= new Action(this.OnSourceStop);
			SoundEffect soundEffect = this.currentSoundEffect;
			if (soundEffect != null)
			{
				soundEffect.Clear();
			}
			CircularAudioBuffer circularAudioBuffer = this.buffer;
			if (circularAudioBuffer != null)
			{
				circularAudioBuffer.Dispose();
			}
			this.IsDisposed = true;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00035D8C File Offset: 0x00033F8C
		public void SetSoundEffect(string effectName)
		{
			if (string.IsNullOrWhiteSpace(effectName) || this.currentEffectName == effectName)
			{
				return;
			}
			if (this.source <= 0)
			{
				Logger.client.Warning("Cannot apply sound effect: source is invalid");
				return;
			}
			SoundEffect soundEffect = this.currentSoundEffect;
			if (soundEffect != null)
			{
				soundEffect.Clear();
			}
			this.currentSoundEffect = SoundEffect.Create(effectName, this.source);
			SoundEffect soundEffect2 = this.currentSoundEffect;
			if (soundEffect2 != null)
			{
				soundEffect2.Apply();
			}
			this.currentEffectName = effectName;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00035E04 File Offset: 0x00034004
		public void ClearSoundEffect()
		{
			SoundEffect soundEffect = this.currentSoundEffect;
			if (soundEffect != null)
			{
				soundEffect.Clear();
			}
			this.currentSoundEffect = null;
			this.currentEffectName = null;
		}

		// Token: 0x0400043D RID: 1085
		public bool IsDisposed;

		// Token: 0x0400043F RID: 1087
		private const int BufferCount = 20;

		// Token: 0x04000440 RID: 1088
		private const int SyntheticBufferCount = 40;

		// Token: 0x04000441 RID: 1089
		private const int SyntheticMaxQueuedFrames = 80;

		// Token: 0x04000442 RID: 1090
		private const int SyntheticPrimeFrames = 20;

		// Token: 0x04000443 RID: 1091
		private const int SyntheticPrimeTimeoutMs = 250;

		// Token: 0x04000444 RID: 1092
		private int source;

		// Token: 0x04000445 RID: 1093
		private CircularAudioBuffer buffer;

		// Token: 0x04000446 RID: 1094
		private SortedList<long, AudioData> orderingQueue = new SortedList<long, AudioData>();

		// Token: 0x04000447 RID: 1095
		private object ordering_queue_lock = new object();

		// Token: 0x04000448 RID: 1096
		private object dequeue_audio_lock = new object();

		// Token: 0x04000449 RID: 1097
		private int orderingDelay = 50;

		// Token: 0x0400044A RID: 1098
		private bool syntheticPlaybackPrimed;

		// Token: 0x0400044B RID: 1099
		private long lastAudioSequenceNumber = -1L;

		// Token: 0x0400044C RID: 1100
		private bool dequeueTaskRunning;

		// Token: 0x0400044D RID: 1101
		private bool playbackEndCheckRunning;

		// Token: 0x0400044E RID: 1102
		private string currentEffectName;

		// Token: 0x0400044F RID: 1103
		private IAudioCodec codec;

		// Token: 0x04000450 RID: 1104
		private LowpassFilter lowpassFilter;

		// Token: 0x04000451 RID: 1105
		private ReverbEffect reverbEffect;

		// Token: 0x04000452 RID: 1106
		private IntoxicatedEffect intoxicatedEffect;

		// Token: 0x04000453 RID: 1107
		private UnstableEffect unstableEffect;

		// Token: 0x04000454 RID: 1108
		private ICoreClientAPI capi;

		// Token: 0x04000455 RID: 1109
		private IPlayer player;

		// Token: 0x04000456 RID: 1110
		private ClientSettingsRepository clientSettingsRepo;

		// Token: 0x04000457 RID: 1111
		private SoundEffect currentSoundEffect;

		// Token: 0x0400045A RID: 1114
		private Vec3f lastSpeakerCoords;

		// Token: 0x0400045B RID: 1115
		private DateTime? lastSpeakerUpdate;

		// Token: 0x0400045C RID: 1116
		private AudioData currentAudio;

		// Token: 0x0400045D RID: 1117
		private int? _lastQueuedNametagRenderRange;

		// Token: 0x0400045E RID: 1118
		private DateTime? lastFullUpdate;

		// Token: 0x0400045F RID: 1119
		private DateTime? lastWallThicknessUpdate;

		// Token: 0x04000460 RID: 1120
		private float cachedWallThickness;

		// Token: 0x04000461 RID: 1121
		private const int FullUpdateIntervalMs = 50;

		// Token: 0x04000462 RID: 1122
		private const int WallThicknessUpdateIntervalMs = 200;

		// Token: 0x04000463 RID: 1123
		private volatile float cachedSoundPhysicsGainHF = 1f;

		// Token: 0x04000464 RID: 1124
		private volatile bool soundPhysicsQueryPending;
	}
}
