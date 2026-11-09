using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000046 RID: 70
	public static class RadioTransmitFrequencyGuard
	{
		// Token: 0x06000198 RID: 408 RVA: 0x0000B740 File Offset: 0x00009940
		public static bool IsFrequencyAvailable(IWorldAccessor world, string frequency, BlockPos excludePos)
		{
			string normalized = RadioFrequencyUtil.Normalize(frequency);
			if (normalized.Length == 0)
			{
				return true;
			}
			using (IEnumerator<string> enumerator = RadioTransmitFrequencyGuard.EnumerateClaimedTransmitFrequencies(world, excludePos).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (RadioFrequencyUtil.Matches(enumerator.Current, normalized))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000B7A8 File Offset: 0x000099A8
		private static IEnumerable<string> EnumerateClaimedTransmitFrequencies(IWorldAccessor world, BlockPos excludePos)
		{
			RadioTransmitFrequencyGuard.<EnumerateClaimedTransmitFrequencies>d__1 <EnumerateClaimedTransmitFrequencies>d__ = new RadioTransmitFrequencyGuard.<EnumerateClaimedTransmitFrequencies>d__1(-2);
			<EnumerateClaimedTransmitFrequencies>d__.<>3__world = world;
			<EnumerateClaimedTransmitFrequencies>d__.<>3__excludePos = excludePos;
			return <EnumerateClaimedTransmitFrequencies>d__;
		}
	}
}
