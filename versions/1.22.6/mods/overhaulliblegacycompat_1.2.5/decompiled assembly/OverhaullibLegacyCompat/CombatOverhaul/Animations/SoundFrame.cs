using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200017D RID: 381
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct SoundFrame
	{
		// Token: 0x060012D7 RID: 4823 RVA: 0x0005BF7C File Offset: 0x0005A17C
		public SoundFrame(string[] code, float durationFraction, bool randomizePitch = false, float range = 32f, float volume = 1f, bool synchronize = true)
		{
			this = new SoundFrame(code, durationFraction, randomizePitch, range, volume, synchronize, 1f, false);
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0005BF9E File Offset: 0x0005A19E
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

		// Token: 0x060012D9 RID: 4825 RVA: 0x0005BFE0 File Offset: 0x0005A1E0
		public float GetPitch(float animationSpeed)
		{
			float pitch = this.PitchFollowsAnimationSpeed ? (this.Pitch * animationSpeed) : this.Pitch;
			return Math.Max(0.01f, pitch);
		}

		// Token: 0x0400092D RID: 2349
		public readonly string[] Code;

		// Token: 0x0400092E RID: 2350
		public readonly float DurationFraction;

		// Token: 0x0400092F RID: 2351
		public readonly bool RandomizePitch;

		// Token: 0x04000930 RID: 2352
		public readonly float Range;

		// Token: 0x04000931 RID: 2353
		public readonly float Volume;

		// Token: 0x04000932 RID: 2354
		public readonly bool Synchronize;

		// Token: 0x04000933 RID: 2355
		public readonly float Pitch;

		// Token: 0x04000934 RID: 2356
		public readonly bool PitchFollowsAnimationSpeed;
	}
}
