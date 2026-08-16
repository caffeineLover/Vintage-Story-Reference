using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul
{
	// Token: 0x02000006 RID: 6
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SlowLogRateLimiter
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00002E70 File Offset: 0x00001070
		public bool ShouldLog(string key, long timestamp, long cooldownTicks)
		{
			if (cooldownTicks <= 0L)
			{
				this._lastLogTimestampByKey[key] = timestamp;
				return true;
			}
			long lastTimestamp;
			if (this._lastLogTimestampByKey.TryGetValue(key, ref lastTimestamp) && timestamp - lastTimestamp < cooldownTicks)
			{
				return false;
			}
			this._lastLogTimestampByKey[key] = timestamp;
			return true;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002EB7 File Offset: 0x000010B7
		public void Clear()
		{
			this._lastLogTimestampByKey.Clear();
		}

		// Token: 0x04000013 RID: 19
		private readonly Dictionary<string, long> _lastLogTimestampByKey = new Dictionary<string, long>();
	}
}
