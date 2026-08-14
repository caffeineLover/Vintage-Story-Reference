using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace SimpleExpressionEngine
{
	// Token: 0x02000064 RID: 100
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StatsContext<[Nullable(2)] TArguments> : IContext<float, TArguments>
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x0000EEA3 File Offset: 0x0000D0A3
		public StatsContext(IPlayer player)
		{
			this.mPlayer = player;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000EEB2 File Offset: 0x0000D0B2
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

		// Token: 0x060002CA RID: 714 RVA: 0x0000EEEE File Offset: 0x0000D0EE
		public float Resolve(string name, params TArguments[] arguments)
		{
			return this.mPlayer.Entity.Stats.GetBlended(name);
		}

		// Token: 0x04000167 RID: 359
		private readonly IPlayer mPlayer;
	}
}
