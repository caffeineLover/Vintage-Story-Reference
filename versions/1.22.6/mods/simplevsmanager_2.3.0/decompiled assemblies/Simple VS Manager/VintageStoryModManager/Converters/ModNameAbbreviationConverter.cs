using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000DC RID: 220
	public class ModNameAbbreviationConverter : IValueConverter
	{
		// Token: 0x0600107C RID: 4220 RVA: 0x00057994 File Offset: 0x00055B94
		[NullableContext(1)]
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (text == null || string.IsNullOrWhiteSpace(text))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = text.Split(new char[]
			{
				' ',
				'\t',
				'\r',
				'\n',
				'-',
				'_'
			}, 1);
			int num = 0;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i].Trim();
				if (!string.IsNullOrEmpty(text2) && char.IsLetter(text2.get_Chars(0)))
				{
					stringBuilder.Append(char.ToUpperInvariant(text2.get_Chars(0)));
					num++;
					if (num >= 3)
					{
						break;
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				return stringBuilder.ToString();
			}
			IEnumerable<char> enumerable = text;
			Func<char, bool> func;
			if ((func = ModNameAbbreviationConverter.<>O.<0>__IsLetter) == null)
			{
				func = (ModNameAbbreviationConverter.<>O.<0>__IsLetter = new Func<char, bool>(char.IsLetter));
			}
			char c = Enumerable.FirstOrDefault<char>(enumerable, func);
			if (c != '\0')
			{
				return char.ToUpperInvariant(c).ToString();
			}
			return string.Empty;
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00057A78 File Offset: 0x00055C78
		[NullableContext(1)]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0200031F RID: 799
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040013E2 RID: 5090
			public static Func<char, bool> <0>__IsLetter;
		}
	}
}
