using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200017F RID: 383
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct CallbackFrame
	{
		// Token: 0x060012DB RID: 4827 RVA: 0x0005C038 File Offset: 0x0005A238
		public CallbackFrame(string code, float durationFraction)
		{
			this.Code = code;
			this.DurationFraction = durationFraction;
		}

		// Token: 0x0400093A RID: 2362
		public readonly string Code;

		// Token: 0x0400093B RID: 2363
		public readonly float DurationFraction;
	}
}
