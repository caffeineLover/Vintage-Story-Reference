using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018A RID: 394
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct SoundFrame
	{
		// Token: 0x0600134E RID: 4942 RVA: 0x0005D9D0 File Offset: 0x0005BBD0
		public SoundFrame(string[] code, float durationFraction, bool randomizePitch = false, float range = 32f, float volume = 1f, bool synchronize = true)
		{
			this = new SoundFrame(code, durationFraction, randomizePitch, range, volume, synchronize, 1f, false);
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0005D9F2 File Offset: 0x0005BBF2
		public SoundFrame(string[] code, float durationFraction, bool randomizePitch, float range, float volume, bool synchronize, float pitch, bool pitchFollowsAnimationSpeed)
		{
			this.Code = code;
			this.DurationFraction = durationFraction;
			this.RandomizePitch = randomizePitch;
			this.Range = range;
			this.Volume = volume;
			this.Synchronize = synchronize;
			this.Pitch = pitch;
			this.PitchFollowsAnimationSpeed = pitchFollowsAnimationSpeed;
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0005DA34 File Offset: 0x0005BC34
		public float GetPitch(float animationSpeed)
		{
			float pitch = this.PitchFollowsAnimationSpeed ? (this.Pitch * animationSpeed) : this.Pitch;
			return Math.Max(0.01f, pitch);
		}

		// Token: 0x04000955 RID: 2389
		public readonly string[] Code;

		// Token: 0x04000956 RID: 2390
		public readonly float DurationFraction;

		// Token: 0x04000957 RID: 2391
		public readonly bool RandomizePitch;

		// Token: 0x04000958 RID: 2392
		public readonly float Range;

		// Token: 0x04000959 RID: 2393
		public readonly float Volume;

		// Token: 0x0400095A RID: 2394
		public readonly bool Synchronize;

		// Token: 0x0400095B RID: 2395
		public readonly float Pitch;

		// Token: 0x0400095C RID: 2396
		public readonly bool PitchFollowsAnimationSpeed;
	}
}
