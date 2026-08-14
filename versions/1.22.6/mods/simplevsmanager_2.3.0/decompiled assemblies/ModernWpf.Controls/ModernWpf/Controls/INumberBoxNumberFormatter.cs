using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000076 RID: 118
	public interface INumberBoxNumberFormatter
	{
		// Token: 0x0600059F RID: 1439
		string FormatDouble(double value);

		// Token: 0x060005A0 RID: 1440
		double? ParseDouble(string text);
	}
}
