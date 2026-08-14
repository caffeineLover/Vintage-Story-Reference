using System;
using System.Globalization;

namespace Windows.Foundation
{
	// Token: 0x0200002C RID: 44
	internal static class TokenizerHelper
	{
		// Token: 0x060002C2 RID: 706 RVA: 0x000055F4 File Offset: 0x000037F4
		public static char GetNumericListSeparator(IFormatProvider provider)
		{
			char c = ',';
			NumberFormatInfo instance = NumberFormatInfo.GetInstance(provider);
			if (instance.NumberDecimalSeparator.Length > 0 && c == instance.NumberDecimalSeparator.get_Chars(0))
			{
				c = ';';
			}
			return c;
		}
	}
}
