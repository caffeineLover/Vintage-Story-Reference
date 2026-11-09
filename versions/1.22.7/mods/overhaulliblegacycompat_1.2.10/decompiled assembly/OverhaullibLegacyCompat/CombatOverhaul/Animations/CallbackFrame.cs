using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018C RID: 396
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct CallbackFrame
	{
		// Token: 0x06001352 RID: 4946 RVA: 0x0005DA8C File Offset: 0x0005BC8C
		public CallbackFrame(string code, float durationFraction)
		{
			this.Code = code;
			this.DurationFraction = durationFraction;
		}

		// Token: 0x04000962 RID: 2402
		public readonly string Code;

		// Token: 0x04000963 RID: 2403
		public readonly float DurationFraction;
	}
}
