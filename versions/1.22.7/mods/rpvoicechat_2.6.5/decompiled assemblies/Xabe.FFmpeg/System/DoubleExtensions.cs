using System;
using System.Globalization;

namespace System
{
	// Token: 0x02000002 RID: 2
	public static class DoubleExtensions
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static string ToFFmpegFormat(this double number, int decimalPlaces = 1)
		{
			return string.Format(CultureInfo.GetCultureInfo("en-US"), string.Format("{{0:N{0}}}", decimalPlaces), number);
		}
	}
}
