using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace SimpleExpressionEngine
{
	// Token: 0x02000060 RID: 96
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StatsContext<[Nullable(2)] TArguments> : IContext<float, TArguments>
	{
		// Token: 0x0600028C RID: 652 RVA: 0x0000E55B File Offset: 0x0000C75B
		public StatsContext(IPlayer player)
		{
			this.mPlayer = player;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000E56A File Offset: 0x0000C76A
		public bool Resolvable(string name)
		{
			IEnumerable<KeyValuePair<string, EntityFloatStats>> stats = this.mPlayer.Entity.Stats;
			Func<KeyValuePair<string, EntityFloatStats>, string> func;
			if ((func = StatsContext<TArguments>.<>c.<>9__2_0) == null)
			{
				func = (StatsContext<TArguments>.<>c.<>9__2_0 = new Func<KeyValuePair<string, EntityFloatStats>, string>(StatsContext<TArguments>.<>c.<>9.<Resolvable>b__2_0));
			}
			return Enumerable.Contains<string>(Enumerable.Select<KeyValuePair<string, EntityFloatStats>, string>(stats, func), name);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000E5A6 File Offset: 0x0000C7A6
		public float Resolve(string name, params TArguments[] arguments)
		{
			return this.mPlayer.Entity.Stats.GetBlended(name);
		}

		// Token: 0x04000139 RID: 313
		private readonly IPlayer mPlayer;
	}
}
