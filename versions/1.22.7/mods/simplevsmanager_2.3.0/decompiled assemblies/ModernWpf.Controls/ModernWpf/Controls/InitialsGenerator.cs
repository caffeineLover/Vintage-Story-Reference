using System;
using System.Linq;

namespace ModernWpf.Controls
{
	// Token: 0x0200007D RID: 125
	internal class InitialsGenerator
	{
		// Token: 0x06000607 RID: 1543 RVA: 0x00015B28 File Offset: 0x00013D28
		public static string InitialsFromDisplayName(string contactDisplayName)
		{
			if (InitialsGenerator.GetCharacterType(contactDisplayName) != CharacterType.Standard)
			{
				return string.Empty;
			}
			string source = contactDisplayName;
			InitialsGenerator.StripTrailingBrackets(ref source);
			string[] array = InitialsGenerator.Split(source, ' ', 25);
			if (array.Length == 1)
			{
				return InitialsGenerator.GetFirstFullCharacter(Enumerable.First<string>(array)).ToUpper();
			}
			if (array.Length > 1)
			{
				string str = Enumerable.First<string>(array);
				string str2 = Enumerable.Last<string>(array);
				return (InitialsGenerator.GetFirstFullCharacter(str) + InitialsGenerator.GetFirstFullCharacter(str2)).ToUpper();
			}
			return string.Empty;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00015BA0 File Offset: 0x00013DA0
		public static CharacterType GetCharacterType(string str)
		{
			CharacterType characterType = CharacterType.Other;
			int num = 0;
			while (num < 3 && num < str.Length && str.get_Chars(num) != '\0' && str.get_Chars(num) != '﻿')
			{
				switch (InitialsGenerator.GetCharacterType(str.get_Chars(num)))
				{
				case CharacterType.Standard:
					if (characterType != CharacterType.Glyph && characterType != CharacterType.Symbolic)
					{
						characterType = CharacterType.Standard;
					}
					break;
				case CharacterType.Symbolic:
					if (characterType != CharacterType.Glyph)
					{
						characterType = CharacterType.Symbolic;
					}
					break;
				case CharacterType.Glyph:
					characterType = CharacterType.Glyph;
					break;
				}
				num++;
			}
			return characterType;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00015C18 File Offset: 0x00013E18
		public static CharacterType GetCharacterType(char character)
		{
			if (character >= 'ɐ' && character <= 'ʯ')
			{
				return CharacterType.Glyph;
			}
			if (character >= '؀' && character <= 'ۿ')
			{
				return CharacterType.Glyph;
			}
			if (character >= 'ݐ' && character <= 'ݿ')
			{
				return CharacterType.Glyph;
			}
			if (character >= 'ࢠ' && character <= 'ࣿ')
			{
				return CharacterType.Glyph;
			}
			if (character >= 'ﭐ' && character <= '﷿')
			{
				return CharacterType.Glyph;
			}
			if (character >= 'ﹰ' && character <= '﻿')
			{
				return CharacterType.Glyph;
			}
			if (character >= 'ऀ' && character <= 'ॿ')
			{
				return CharacterType.Glyph;
			}
			if (character >= '꣠' && character <= 'ꣿ')
			{
				return CharacterType.Glyph;
			}
			if (character >= 'ঀ' && character <= '৿')
			{
				return CharacterType.Glyph;
			}
			if (character >= '਀' && character <= '੿')
			{
				return CharacterType.Glyph;
			}
			if (character >= '઀' && character <= '૿')
			{
				return CharacterType.Glyph;
			}
			if (character >= '଀' && character <= '୿')
			{
				return CharacterType.Glyph;
			}
			if (character >= '஀' && character <= '௿')
			{
				return CharacterType.Glyph;
			}
			if (character >= 'ఀ' && character <= '౿')
			{
				return CharacterType.Glyph;
			}
			if (character >= 'ಀ' && character <= '೿')
			{
				return CharacterType.Glyph;
			}
			if (character >= 'ഀ' && character <= 'ൿ')
			{
				return CharacterType.Glyph;
			}
			if (character >= '඀' && character <= '෿')
			{
				return CharacterType.Glyph;
			}
			if (character >= '฀' && character <= '๿')
			{
				return CharacterType.Glyph;
			}
			if (character >= '຀' && character <= '໿')
			{
				return CharacterType.Glyph;
			}
			if (character >= '一' && character <= '鿿')
			{
				return CharacterType.Symbolic;
			}
			if (character >= '㐀' && character <= '䶿')
			{
				return CharacterType.Symbolic;
			}
			if ((int)character >= 131072 && (int)character <= 173791)
			{
				return CharacterType.Symbolic;
			}
			if ((int)character >= 173824 && (int)character <= 177983)
			{
				return CharacterType.Symbolic;
			}
			if ((int)character >= 177984 && (int)character <= 178207)
			{
				return CharacterType.Symbolic;
			}
			if (character >= '⺀' && character <= '⻿')
			{
				return CharacterType.Symbolic;
			}
			if (character >= '\u3000' && character <= '〿')
			{
				return CharacterType.Symbolic;
			}
			if (character >= '㇀' && character <= '㇯')
			{
				return CharacterType.Symbolic;
			}
			if (character >= '㈀' && character <= '㋿')
			{
				return CharacterType.Symbolic;
			}
			if (character >= '㌀' && character <= '㏿')
			{
				return CharacterType.Symbolic;
			}
			if (character >= '豈' && character <= '﫿')
			{
				return CharacterType.Symbolic;
			}
			if (character >= '︰' && character <= '﹏')
			{
				return CharacterType.Symbolic;
			}
			if ((int)character >= 194560 && (int)character <= 195103)
			{
				return CharacterType.Symbolic;
			}
			if (character >= 'Ͱ' && character <= 'Ͽ')
			{
				return CharacterType.Symbolic;
			}
			if (character >= '֐' && character <= '׿')
			{
				return CharacterType.Symbolic;
			}
			if (character >= '԰' && character <= '֏')
			{
				return CharacterType.Symbolic;
			}
			if (character > '\0' && character <= '\u007f')
			{
				return CharacterType.Standard;
			}
			if (character >= '\u0080' && character <= 'ÿ')
			{
				return CharacterType.Standard;
			}
			if (character >= 'Ā' && character <= 'ſ')
			{
				return CharacterType.Standard;
			}
			if (character >= 'ƀ' && character <= 'ɏ')
			{
				return CharacterType.Standard;
			}
			if (character >= 'Ⱡ' && character <= 'Ɀ')
			{
				return CharacterType.Standard;
			}
			if (character >= '꜠' && character <= 'ꟿ')
			{
				return CharacterType.Standard;
			}
			if (character >= 'ꬰ' && character <= '꭯')
			{
				return CharacterType.Standard;
			}
			if (character >= 'Ḁ' && character <= 'ỿ')
			{
				return CharacterType.Standard;
			}
			if (character >= 'Ѐ' && character <= 'ӿ')
			{
				return CharacterType.Standard;
			}
			if (character >= 'Ԁ' && character <= 'ԯ')
			{
				return CharacterType.Standard;
			}
			if (character >= '̀' && character <= 'ͯ')
			{
				return CharacterType.Standard;
			}
			return CharacterType.Other;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00015F5B File Offset: 0x0001415B
		private static string[] Split(string source, char delim, int maxIterations = 25)
		{
			return source.Split(new char[]
			{
				delim
			}, maxIterations);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00015F70 File Offset: 0x00014170
		private static void StripTrailingBrackets(ref string source)
		{
			string[] array = new string[]
			{
				"{}",
				"()",
				"[]"
			};
			if (source.Length == 0)
			{
				return;
			}
			foreach (string text in array)
			{
				if (source.get_Chars(source.Length - 1) == text.get_Chars(1))
				{
					int num = source.LastIndexOf(text.get_Chars(0));
					if (num != -1)
					{
						source = source.Remove(num);
						return;
					}
				}
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00015FF4 File Offset: 0x000141F4
		private static string GetFirstFullCharacter(string str)
		{
			int i = 0;
			while (i < str.Length)
			{
				char c = str.get_Chars(i);
				if (c >= '!' && c <= '/')
				{
					i++;
				}
				else if (c >= ':' && c <= '@')
				{
					i++;
				}
				else
				{
					if (c < '{' || c > '~')
					{
						break;
					}
					i++;
				}
			}
			if (i >= str.Length)
			{
				i = 0;
			}
			int j;
			for (j = i + 1; j < str.Length; j++)
			{
				char c2 = str.get_Chars(j);
				if (c2 < '̀' || c2 > 'ͯ')
				{
					break;
				}
			}
			int length = j - i;
			return str.SafeSubstring(i, length);
		}
	}
}
