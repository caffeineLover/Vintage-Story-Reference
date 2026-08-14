using System;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x0200000D RID: 13
	internal class StringTokenizer : ITokenizer
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00003098 File Offset: 0x00001298
		public bool ReadsNextByte { get; }

		// Token: 0x06000029 RID: 41 RVA: 0x000030A0 File Offset: 0x000012A0
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			token = null;
			if (inputBytes == null)
			{
				return false;
			}
			if (currentByte != 40)
			{
				return false;
			}
			StringBuilder stringBuilder = this.stringBuilder;
			int num = 1;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			short[] array = new short[3];
			int num2 = 0;
			while (inputBytes.MoveNext())
			{
				char currentByte2 = (char)inputBytes.CurrentByte;
				if (flag3)
				{
					bool flag4 = currentByte2 >= '0' && currentByte2 <= '7';
					if (flag4)
					{
						StringTokenizer.LeftShiftOctal(currentByte2, num2, array);
						num2++;
					}
					if (num2 == 3 || !flag4)
					{
						int num3 = OctalHelpers.FromOctalDigits(array);
						stringBuilder.Append((char)num3);
						array[0] = 0;
						array[1] = 0;
						array[2] = 0;
						num2 = 0;
						flag3 = false;
					}
					if (flag4)
					{
						continue;
					}
				}
				if (currentByte2 != '(')
				{
					if (currentByte2 != ')')
					{
						if (currentByte2 != '\\')
						{
							if (flag2)
							{
								if (ReadHelper.IsEndOfLine(currentByte2))
								{
									continue;
								}
								flag2 = false;
								stringBuilder.Append(currentByte2);
							}
							else if (flag)
							{
								StringTokenizer.ProcessEscapedCharacter(currentByte2, stringBuilder, array, ref flag3, ref num2, ref flag2);
								flag = false;
							}
							else
							{
								stringBuilder.Append(currentByte2);
							}
						}
						else
						{
							flag2 = false;
							if (flag)
							{
								stringBuilder.Append(currentByte2);
								flag = false;
							}
							else
							{
								flag = true;
							}
						}
					}
					else
					{
						flag2 = false;
						if (!flag)
						{
							num--;
						}
						flag = false;
						if (num > 0)
						{
							stringBuilder.Append(currentByte2);
						}
						num = StringTokenizer.CheckForEndOfString(num, inputBytes);
					}
				}
				else
				{
					flag2 = false;
					if (!flag)
					{
						num++;
					}
					flag = false;
					stringBuilder.Append(currentByte2);
				}
				if (num <= 0)
				{
					break;
				}
			}
			string text;
			StringToken.Encoding encoding;
			if (stringBuilder.Length >= 2)
			{
				if (stringBuilder.get_Chars(0) == 'þ' && stringBuilder.get_Chars(1) == 'ÿ')
				{
					byte[] array2 = OtherEncodings.StringAsLatin1Bytes(stringBuilder.ToString());
					text = Encoding.BigEndianUnicode.GetString(array2).Substring(1);
					encoding = 2;
				}
				else if (stringBuilder.get_Chars(0) == 'ÿ' && stringBuilder.get_Chars(1) == 'þ')
				{
					byte[] array3 = OtherEncodings.StringAsLatin1Bytes(stringBuilder.ToString());
					text = Encoding.Unicode.GetString(array3).Substring(1);
					encoding = 1;
				}
				else
				{
					text = stringBuilder.ToString();
					encoding = 0;
				}
			}
			else
			{
				text = stringBuilder.ToString();
				encoding = 0;
			}
			stringBuilder.Clear();
			token = new StringToken(text, encoding);
			return true;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000032B0 File Offset: 0x000014B0
		private static void LeftShiftOctal(char nextOctalChar, int octalsRead, short[] octals)
		{
			for (int i = octalsRead; i > 0; i--)
			{
				octals[i] = octals[i - 1];
			}
			short num = OctalHelpers.CharacterToShort(nextOctalChar);
			octals[0] = num;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000032DC File Offset: 0x000014DC
		private static void ProcessEscapedCharacter(char c, StringBuilder builder, short[] octal, ref bool isOctalActive, ref int octalsRead, ref bool isLineBreaking)
		{
			if (c <= 'f')
			{
				switch (c)
				{
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
					octal[0] = OctalHelpers.CharacterToShort(c);
					isOctalActive = true;
					octalsRead = 1;
					return;
				default:
					if (c == 'b')
					{
						builder.Append('\b');
						return;
					}
					if (c == 'f')
					{
						builder.Append('\f');
						return;
					}
					break;
				}
			}
			else
			{
				if (c == 'n')
				{
					builder.Append('\n');
					return;
				}
				if (c == 'r')
				{
					builder.Append('\r');
					return;
				}
				if (c == 't')
				{
					builder.Append('\t');
					return;
				}
			}
			if (c == '\r' || c == '\n')
			{
				isLineBreaking = true;
				return;
			}
			builder.Append(c);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003390 File Offset: 0x00001590
		private static int CheckForEndOfString(int numberOfBrackets, IInputBytes bytes)
		{
			int result = numberOfBrackets;
			byte[] array = new byte[3];
			long currentOffset = bytes.CurrentOffset;
			int num = bytes.Read(array, default(int?));
			if (num == 3 && array[0] == 13 && ((array[1] == 10 && array[2] == 47) || array[2] == 62 || array[1] == 47 || array[1] == 62))
			{
				result = 0;
			}
			if (num > 0)
			{
				bytes.Seek(currentOffset);
			}
			return result;
		}

		// Token: 0x04000015 RID: 21
		private readonly StringBuilder stringBuilder = new StringBuilder();
	}
}
