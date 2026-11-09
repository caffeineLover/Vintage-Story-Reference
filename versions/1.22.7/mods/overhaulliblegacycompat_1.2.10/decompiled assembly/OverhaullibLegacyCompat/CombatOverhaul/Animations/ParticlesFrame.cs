using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018B RID: 395
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ParticlesFrame
	{
		// Token: 0x06001351 RID: 4945 RVA: 0x0005DA65 File Offset: 0x0005BC65
		public ParticlesFrame(string code, float durationFraction, Vector3 position, Vector3 velocity, float intensity)
		{
			this.Code = code;
			this.DurationFraction = durationFraction;
			this.Position = position;
			this.Velocity = velocity;
			this.Intensity = intensity;
		}

		// Token: 0x0400095D RID: 2397
		public readonly string Code;

		// Token: 0x0400095E RID: 2398
		public readonly float DurationFraction;

		// Token: 0x0400095F RID: 2399
		public readonly Vector3 Position;

		// Token: 0x04000960 RID: 2400
		public readonly Vector3 Velocity;

		// Token: 0x04000961 RID: 2401
		public readonly float Intensity;
	}
}
