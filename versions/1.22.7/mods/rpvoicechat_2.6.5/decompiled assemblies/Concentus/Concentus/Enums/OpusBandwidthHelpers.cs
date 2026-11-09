using System;

namespace Concentus.Enums
{
	// Token: 0x02000065 RID: 101
	internal static class OpusBandwidthHelpers
	{
		// Token: 0x0600024A RID: 586 RVA: 0x0001FE1B File Offset: 0x0001E01B
		public static int GetOrdinal(OpusBandwidth bw)
		{
			return bw - OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0001FE24 File Offset: 0x0001E024
		public static OpusBandwidth MIN(OpusBandwidth a, OpusBandwidth b)
		{
			if (a < b)
			{
				return a;
			}
			return b;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0001FE2D File Offset: 0x0001E02D
		public static OpusBandwidth MAX(OpusBandwidth a, OpusBandwidth b)
		{
			if (a > b)
			{
				return a;
			}
			return b;
		}
	}
}
