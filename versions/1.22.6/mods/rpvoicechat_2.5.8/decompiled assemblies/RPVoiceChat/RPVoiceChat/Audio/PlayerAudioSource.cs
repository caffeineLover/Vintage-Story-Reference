using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RPVoiceChat.Audio.Effects;
using RPVoiceChat.Config;
using RPVoiceChat.DB;
using RPVoiceChat.Gui;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000CC RID: 204
	public class PlayerAudioSource : IDisposable
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00029590 File Offset: 0x00027790
		public bool IsPlaying
		{
			get
			{
				return this._IsPlaying();
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00029598 File Offset: 0x00027798
		public float MaxGain
		{
			get
			{
				return ServerConfigManager.MaxAudioGain;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0002959F File Offset: 0x0002779F
		public int SourceId
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x000295A7 File Offset: 0x000277A7
		// (set) Token: 0x06000770 RID: 1904 RVA: 0x000295AF File Offset: 0x000277AF
		public bool IsLocational { get; set; } = true;

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x000295B8 File Offset: 0x000277B8
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x000295C0 File Offset: 0x000277C0
		public VoiceLevel voiceLevel { get; private set; } = VoiceLevel.Talking;

		// Token: 0x06000773 RID: 1907 RVA: 0x000295CC File Offset: 0x000277CC
		public PlayerAudioSource(IPlayer player, ICoreClientAPI capi, ClientSettingsRepository clientSettingsRepo)
		{
			Dictionary<VoiceLevel, float> dictionary = new Dictionary<VoiceLevel, float>();
			dictionary.Add(VoiceLevel.Whispering, 1.25f);
			dictionary.Add(VoiceLevel.Talking, 2.25f);
			dictionary.Add(VoiceLevel.Shouting, 6.25f);
			this.referenceDistanceByVoiceLevel = dictionary;
			base..ctor();
			this.player = player;
			this.capi = capi;
			this.clientSettingsRepo = clientSettingsRepo;
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
			this.buffer = new CircularAudioBuffer(this.source, 20);
			this.buffer.OnEmptyingQueue += new Action(this.OnSourceStop);
			float gain = this.GetFinalGain();
			OALW.Source(this.source, 4103, false);
			OALW.Source(this.source, 514, true);
			OALW.Source(this.source, 4106, gain);
			OALW.Source(this.source, 4099, 1f);
			this.UpdateVoiceLevel(this.voiceLevel);
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0002972C File Offset: 0x0002792C
		public void UpdateVoiceLevel(VoiceLevel voiceLevel)
		{
			this.voiceLevel = voiceLevel;
			float baseReferenceDistance = this.referenceDistanceByVoiceLevel[voiceLevel];
			float distanceFactor = this.GetDistanceFactor();
			float rolloffFactor = baseReferenceDistance * distanceFactor;
			OALW.Source(this.source, 4128, baseReferenceDistance);
			OALW.Source(this.source, 4129, rolloffFactor);
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002977C File Offset: 0x0002797C
		private void TryApplyNametagRenderRange()
		{
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

		// Token: 0x06000776 RID: 1910 RVA: 0x000297F4 File Offset: 0x000279F4
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

		// Token: 0x06000777 RID: 1911 RVA: 0x00029884 File Offset: 0x00027A84
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
			float wallThickness = this.cachedWallThickness;
			object obj2 = obj;
			if (obj2 != null)
			{
				if (ModConfig.ClientConfig.Muffling)
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
				}
			}
			else if (this.capi.World.Player.Entity.Swimming && this.cachedWallThickness > 0f)
			{
				wallThickness = this.cachedWallThickness + 1f;
			}
			if (obj2 != null)
			{
				bool muffling = ModConfig.ClientConfig.Muffling;
				float wallThicknessWeighting = WorldConfig.GetFloat("wall-thickness-weighting", 0f);
				LowpassFilter lowpassFilter = this.lowpassFilter;
				if (lowpassFilter != null)
				{
					lowpassFilter.Stop();
				}
				if (muffling && wallThickness != 0f)
				{
					this.lowpassFilter = (this.lowpassFilter ?? new LowpassFilter(this.source));
					this.lowpassFilter.Start();
					this.lowpassFilter.SetHFGain(Math.Max(1f - wallThickness / wallThicknessWeighting, 0.1f));
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
			float gain = this.GetFinalGain();
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

		// Token: 0x06000778 RID: 1912 RVA: 0x00029D16 File Offset: 0x00027F16
		private bool _IsPlaying()
		{
			return this.source > 0 && OALW.GetSourceState(this.source) == 4114;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00029D38 File Offset: 0x00027F38
		private float GetFinalGain()
		{
			float num = Math.Clamp(PlayerListener.VoiceGain, 0f, this.MaxGain);
			float sourceGain = this.clientSettingsRepo.GetPlayerGain(this.player.PlayerUID);
			return GameMath.Clamp(num * sourceGain, 0f, this.MaxGain);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00029D84 File Offset: 0x00027F84
		private float GetDistanceFactor()
		{
			AudioData audioData = this.currentAudio;
			if (audioData != null && audioData.isGlobalBroadcast)
			{
				return 0f;
			}
			AudioData audioData2 = this.currentAudio;
			if (audioData2 != null && audioData2.ignoreDistanceReduction)
			{
				return 0f;
			}
			AudioData audioData3 = this.currentAudio;
			float maxHearingDistance = (float)((audioData3 != null && audioData3.effectiveRange > 0) ? this.currentAudio.effectiveRange : WorldConfig.GetInt(this.voiceLevel, default(int?)));
			double exponent = (10f < maxHearingDistance) ? 2.0 : -0.33;
			return (float)Math.Pow((double)(10f / maxHearingDistance), exponent);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00029E2B File Offset: 0x0002802B
		private Vec3f GetRelativeSourcePosition(Vec3d speakerPos, EntityPos listenerPos)
		{
			return LocationUtils.GetRelativeSpeakerLocation(speakerPos.ToVec3f(), listenerPos);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00029E3C File Offset: 0x0002803C
		private Vec3f GetRelativeVelocity(Vec3d speakerPos, EntityPos listenerPos, Vec3f relativeSpeakerPosition)
		{
			Vec3f speakerVelocity = this.GetVelocity(speakerPos);
			Vec3f relativeFuturePosition = LocationUtils.GetRelativeSpeakerLocation(speakerPos.ToVec3f() + speakerVelocity, listenerPos);
			return relativeSpeakerPosition - relativeFuturePosition;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00029E6C File Offset: 0x0002806C
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

		// Token: 0x0600077E RID: 1918 RVA: 0x00029F1C File Offset: 0x0002811C
		public void EnqueueAudio(AudioData audio, long sequenceNumber)
		{
			object obj = this.ordering_queue_lock;
			lock (obj)
			{
				if (this.orderingQueue.ContainsKey(sequenceNumber))
				{
					return;
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
			}
			if (!this.dequeueTaskRunning)
			{
				this.DequeueAudio();
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00029FC8 File Offset: 0x000281C8
		public void DequeueAudio()
		{
			PlayerAudioSource.<DequeueAudio>d__57 <DequeueAudio>d__;
			<DequeueAudio>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DequeueAudio>d__.<>4__this = this;
			<DequeueAudio>d__.<>1__state = -1;
			<DequeueAudio>d__.<>t__builder.Start<PlayerAudioSource.<DequeueAudio>d__57>(ref <DequeueAudio>d__);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002A000 File Offset: 0x00028200
		private void SchedulePlaybackEndCheck()
		{
			PlayerAudioSource.<SchedulePlaybackEndCheck>d__58 <SchedulePlaybackEndCheck>d__;
			<SchedulePlaybackEndCheck>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<SchedulePlaybackEndCheck>d__.<>4__this = this;
			<SchedulePlaybackEndCheck>d__.<>1__state = -1;
			<SchedulePlaybackEndCheck>d__.<>t__builder.Start<PlayerAudioSource.<SchedulePlaybackEndCheck>d__58>(ref <SchedulePlaybackEndCheck>d__);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0002A037 File Offset: 0x00028237
		public void StartPlaying()
		{
			if (this.source <= 0)
			{
				return;
			}
			OALW.SourcePlay(this.source);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0002A04E File Offset: 0x0002824E
		private void NotifyStartedSpeaking()
		{
			PlayerNameTagRenderer.UpdatePlayerNameTag(this.player, true);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002A05C File Offset: 0x0002825C
		public void StopPlaying()
		{
			if (this.source <= 0)
			{
				return;
			}
			OALW.SourceStop(this.source);
			this.OnSourceStop();
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002A079 File Offset: 0x00028279
		private void OnSourceStop()
		{
			PlayerNameTagRenderer.UpdatePlayerNameTag(this.player, false);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0002A088 File Offset: 0x00028288
		public void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			OALW.SourceStop(this.source);
			OALW.DeleteSource(this.source);
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

		// Token: 0x06000786 RID: 1926 RVA: 0x0002A0FC File Offset: 0x000282FC
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

		// Token: 0x06000787 RID: 1927 RVA: 0x0002A174 File Offset: 0x00028374
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

		// Token: 0x0400035C RID: 860
		public bool IsDisposed;

		// Token: 0x0400035D RID: 861
		private const int BufferCount = 20;

		// Token: 0x0400035E RID: 862
		private int source;

		// Token: 0x0400035F RID: 863
		private CircularAudioBuffer buffer;

		// Token: 0x04000360 RID: 864
		private SortedList<long, AudioData> orderingQueue = new SortedList<long, AudioData>();

		// Token: 0x04000361 RID: 865
		private object ordering_queue_lock = new object();

		// Token: 0x04000362 RID: 866
		private object dequeue_audio_lock = new object();

		// Token: 0x04000363 RID: 867
		private int orderingDelay = 50;

		// Token: 0x04000364 RID: 868
		private long lastAudioSequenceNumber = -1L;

		// Token: 0x04000365 RID: 869
		private bool dequeueTaskRunning;

		// Token: 0x04000366 RID: 870
		private bool playbackEndCheckRunning;

		// Token: 0x04000367 RID: 871
		private string currentEffectName;

		// Token: 0x04000368 RID: 872
		private IAudioCodec codec;

		// Token: 0x04000369 RID: 873
		private LowpassFilter lowpassFilter;

		// Token: 0x0400036A RID: 874
		private ReverbEffect reverbEffect;

		// Token: 0x0400036B RID: 875
		private IntoxicatedEffect intoxicatedEffect;

		// Token: 0x0400036C RID: 876
		private UnstableEffect unstableEffect;

		// Token: 0x0400036D RID: 877
		private ICoreClientAPI capi;

		// Token: 0x0400036E RID: 878
		private IPlayer player;

		// Token: 0x0400036F RID: 879
		private ClientSettingsRepository clientSettingsRepo;

		// Token: 0x04000370 RID: 880
		private SoundEffect currentSoundEffect;

		// Token: 0x04000373 RID: 883
		private Dictionary<VoiceLevel, float> referenceDistanceByVoiceLevel;

		// Token: 0x04000374 RID: 884
		private Vec3f lastSpeakerCoords;

		// Token: 0x04000375 RID: 885
		private DateTime? lastSpeakerUpdate;

		// Token: 0x04000376 RID: 886
		private AudioData currentAudio;

		// Token: 0x04000377 RID: 887
		private int? _lastQueuedNametagRenderRange;

		// Token: 0x04000378 RID: 888
		private DateTime? lastFullUpdate;

		// Token: 0x04000379 RID: 889
		private DateTime? lastWallThicknessUpdate;

		// Token: 0x0400037A RID: 890
		private float cachedWallThickness;

		// Token: 0x0400037B RID: 891
		private const int FullUpdateIntervalMs = 50;

		// Token: 0x0400037C RID: 892
		private const int WallThicknessUpdateIntervalMs = 200;
	}
}
