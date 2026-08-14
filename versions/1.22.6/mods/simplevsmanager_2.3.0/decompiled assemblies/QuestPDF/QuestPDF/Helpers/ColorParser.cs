using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Helpers
{
	// Token: 0x02000070 RID: 112
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ColorParser
	{
		// Token: 0x060002C0 RID: 704 RVA: 0x000070FC File Offset: 0x000052FC
		public static Color ParseColorHex(string hexString)
		{
			Color result;
			if (!ColorParser.TryParseColorHex(hexString, out result))
			{
				throw new ArgumentException("The provided value '" + hexString + "' is not a valid hex color. The following formats are supported: #RGB, #ARGB, #RRGGBB, #AARRGGBB. The hash sign is optional so the following formats are also valid: RGB, ARGB, RRGGBB, AARRGGBB. For example #FF8800 is a solid orange color, while #20CF is a barely visible aqua color.", "color");
			}
			return result;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00007130 File Offset: 0x00005330
		public static bool TryParseColorHex(string hexString, out Color color)
		{
			ConcurrentDictionary<string, Color?> cache = ColorParser.Cache;
			Func<string, Color?> func;
			if ((func = ColorParser.<>O.<0>__ParseColor) == null)
			{
				func = (ColorParser.<>O.<0>__ParseColor = new Func<string, Color?>(ColorParser.<TryParseColorHex>g__ParseColor|2_0));
			}
			Color? orAdd = cache.GetOrAdd(hexString, func);
			color = (orAdd ?? Colors.Black);
			return orAdd != null;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00007198 File Offset: 0x00005398
		[CompilerGenerated]
		internal static Color? <TryParseColorHex>g__ParseColor|2_0(string hexString)
		{
			if (string.IsNullOrWhiteSpace(hexString))
			{
				return default(Color?);
			}
			string text = hexString.Trim().TrimStart('#');
			int length = text.Length;
			if (length == 3 || length == 4)
			{
				byte b;
				if (length == 4)
				{
					if (!byte.TryParse(text.Substring(0, 1), 515, CultureInfo.InvariantCulture, ref b))
					{
						return default(Color?);
					}
					b = (byte)((int)b << 4 | (int)b);
				}
				else
				{
					b = byte.MaxValue;
				}
				byte b2;
				byte b3;
				byte b4;
				if (!byte.TryParse(text.Substring(length - 3, 1), 515, CultureInfo.InvariantCulture, ref b2) || !byte.TryParse(text.Substring(length - 2, 1), 515, CultureInfo.InvariantCulture, ref b3) || !byte.TryParse(text.Substring(length - 1, 1), 515, CultureInfo.InvariantCulture, ref b4))
				{
					return default(Color?);
				}
				b2 |= (byte)(b2 << 4);
				b3 |= (byte)(b3 << 4);
				b4 |= (byte)(b4 << 4);
				uint num = (uint)((int)b << 24 | (int)b2 << 16 | (int)b3 << 8 | (int)b4);
				return new Color?(new Color(num));
			}
			else
			{
				if (length != 6 && length != 8)
				{
					return default(Color?);
				}
				uint num2;
				if (!uint.TryParse(text, 515, CultureInfo.InvariantCulture, ref num2))
				{
					return default(Color?);
				}
				uint num = num2;
				if (length == 6)
				{
					num |= 4278190080U;
				}
				return new Color?(new Color(num));
			}
		}

		// Token: 0x0400019A RID: 410
		private static readonly ConcurrentDictionary<string, Color?> Cache = new ConcurrentDictionary<string, Color?>();

		// Token: 0x0200016D RID: 365
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040004B5 RID: 1205
			[Nullable(new byte[]
			{
				0,
				1
			})]
			public static Func<string, Color?> <0>__ParseColor;
		}
	}
}
