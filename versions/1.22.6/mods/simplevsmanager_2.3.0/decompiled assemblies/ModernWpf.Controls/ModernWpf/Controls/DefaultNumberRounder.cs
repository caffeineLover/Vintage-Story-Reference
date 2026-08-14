using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000073 RID: 115
	internal class DefaultNumberRounder
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x00014563 File Offset: 0x00012763
		public double RoundDouble(double value)
		{
			return double.Parse(value.ToString("G12"));
		}
	}
}
