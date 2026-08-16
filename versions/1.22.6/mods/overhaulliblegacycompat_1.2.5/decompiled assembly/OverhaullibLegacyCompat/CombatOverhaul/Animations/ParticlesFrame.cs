using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200017E RID: 382
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ParticlesFrame
	{
		// Token: 0x060012DA RID: 4826 RVA: 0x0005C011 File Offset: 0x0005A211
		public ParticlesFrame(string code, float durationFraction, Vector3 position, Vector3 velocity, float intensity)
		{
			this.Code = code;
			this.DurationFraction = durationFraction;
			this.Position = position;
			this.Velocity = velocity;
			this.Intensity = intensity;
		}

		// Token: 0x04000935 RID: 2357
		public readonly string Code;

		// Token: 0x04000936 RID: 2358
		public readonly float DurationFraction;

		// Token: 0x04000937 RID: 2359
		public readonly Vector3 Position;

		// Token: 0x04000938 RID: 2360
		public readonly Vector3 Velocity;

		// Token: 0x04000939 RID: 2361
		public readonly float Intensity;
	}
}
