using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000030 RID: 48
	internal static class InternalStringExtensions
	{
		// Token: 0x06000137 RID: 311 RVA: 0x0000BF1C File Offset: 0x0000A11C
		public static bool StartsWithOffset(this string value, string start, int offset)
		{
			if (offset < 0)
			{
				string text = "offset";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Offset cannot be negative: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(offset);
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (value == null)
			{
				return start == null && offset == 0;
			}
			return offset <= value.Length - 1 && value.Substring(offset).StartsWith(start);
		}
	}
}
