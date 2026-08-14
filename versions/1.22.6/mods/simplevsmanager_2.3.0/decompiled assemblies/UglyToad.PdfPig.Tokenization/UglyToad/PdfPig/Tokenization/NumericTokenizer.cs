using System;
using System.Globalization;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x0200000A RID: 10
	internal class NumericTokenizer : ITokenizer
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000026C7 File Offset: 0x000008C7
		public bool ReadsNextByte { get; } = 1;

		// Token: 0x0600001F RID: 31 RVA: 0x000026D0 File Offset: 0x000008D0
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			token = null;
			bool flag = currentByte == 45 || currentByte == 43;
			StringBuilder stringBuilder;
			if ((currentByte >= 48 && currentByte <= 57) || currentByte == 46)
			{
				stringBuilder = this.stringBuilder;
				stringBuilder.Append((char)currentByte);
			}
			else
			{
				if (!flag)
				{
					return false;
				}
				stringBuilder = this.stringBuilder;
				stringBuilder.Append((char)currentByte);
			}
			bool flag2 = flag;
			while (inputBytes.MoveNext())
			{
				byte currentByte2 = inputBytes.CurrentByte;
				if (currentByte2 == 43 || currentByte2 == 45)
				{
					if (!flag2)
					{
						stringBuilder.Append((char)currentByte2);
						flag2 = true;
					}
				}
				else
				{
					if ((currentByte2 < 48 || currentByte2 > 57) && currentByte2 != 46 && currentByte2 != 69 && currentByte2 != 101)
					{
						break;
					}
					flag2 = false;
					stringBuilder.Append((char)currentByte2);
				}
			}
			bool result;
			try
			{
				string text = stringBuilder.ToString();
				stringBuilder.Clear();
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 722245873U)
				{
					if (num <= 451618993U)
					{
						if (num <= 350953279U)
						{
							if (num != 334175660U)
							{
								if (num != 348981803U)
								{
									if (num != 350953279U)
									{
										goto IL_66E;
									}
									if (!(text == "19"))
									{
										goto IL_66E;
									}
									token = NumericToken.Nineteen;
									return true;
								}
								else
								{
									if (!(text == "-1"))
									{
										goto IL_66E;
									}
									token = NumericToken.MinusOne;
									return true;
								}
							}
							else
							{
								if (!(text == "18"))
								{
									goto IL_66E;
								}
								token = NumericToken.Eighteen;
								return true;
							}
						}
						else if (num <= 418063755U)
						{
							if (num != 401286136U)
							{
								if (num != 418063755U)
								{
									goto IL_66E;
								}
								if (!(text == "15"))
								{
									goto IL_66E;
								}
								token = NumericToken.Fifteen;
								return true;
							}
							else
							{
								if (!(text == "14"))
								{
									goto IL_66E;
								}
								token = NumericToken.Fourteen;
								return true;
							}
						}
						else if (num != 434841374U)
						{
							if (num != 451618993U)
							{
								goto IL_66E;
							}
							if (!(text == "17"))
							{
								goto IL_66E;
							}
							token = NumericToken.Seventeen;
							return true;
						}
						else
						{
							if (!(text == "16"))
							{
								goto IL_66E;
							}
							token = NumericToken.Sixteen;
							return true;
						}
					}
					else if (num <= 501951850U)
					{
						if (num != 468396612U)
						{
							if (num != 485174231U)
							{
								if (num != 501951850U)
								{
									goto IL_66E;
								}
								if (!(text == "12"))
								{
									goto IL_66E;
								}
								token = NumericToken.Twelve;
								return true;
							}
							else
							{
								if (!(text == "11"))
								{
									goto IL_66E;
								}
								token = NumericToken.Eleven;
								return true;
							}
						}
						else
						{
							if (!(text == "10"))
							{
								goto IL_66E;
							}
							token = NumericToken.Ten;
							return true;
						}
					}
					else if (num <= 580373188U)
					{
						if (num != 518729469U)
						{
							if (num != 580373188U)
							{
								goto IL_66E;
							}
							if (!(text == "1000"))
							{
								goto IL_66E;
							}
							token = NumericToken.OneThousand;
							return true;
						}
						else
						{
							if (!(text == "13"))
							{
								goto IL_66E;
							}
							token = NumericToken.Thirteen;
							return true;
						}
					}
					else if (num != 671913016U)
					{
						if (num != 722245873U)
						{
							goto IL_66E;
						}
						if (!(text == "."))
						{
							goto IL_66E;
						}
					}
					else if (!(text == "-"))
					{
						goto IL_66E;
					}
				}
				else if (num <= 890022063U)
				{
					if (num <= 822911587U)
					{
						if (num != 731423408U)
						{
							if (num != 806133968U)
							{
								if (num != 822911587U)
								{
									goto IL_66E;
								}
								if (!(text == "4"))
								{
									goto IL_66E;
								}
								token = NumericToken.Four;
								return true;
							}
							else
							{
								if (!(text == "5"))
								{
									goto IL_66E;
								}
								token = NumericToken.Five;
								return true;
							}
						}
						else
						{
							if (!(text == "500"))
							{
								goto IL_66E;
							}
							token = NumericToken.FiveHundred;
							return true;
						}
					}
					else if (num <= 856466825U)
					{
						if (num != 839689206U)
						{
							if (num != 856466825U)
							{
								goto IL_66E;
							}
							if (!(text == "6"))
							{
								goto IL_66E;
							}
							token = NumericToken.Six;
							return true;
						}
						else
						{
							if (!(text == "7"))
							{
								goto IL_66E;
							}
							token = NumericToken.Seven;
							return true;
						}
					}
					else if (num != 873244444U)
					{
						if (num != 890022063U)
						{
							goto IL_66E;
						}
						if (!(text == "0"))
						{
							goto IL_66E;
						}
					}
					else
					{
						if (!(text == "1"))
						{
							goto IL_66E;
						}
						token = NumericToken.One;
						return true;
					}
				}
				else if (num <= 1007465396U)
				{
					if (num != 906799682U)
					{
						if (num != 923577301U)
						{
							if (num != 1007465396U)
							{
								goto IL_66E;
							}
							if (!(text == "9"))
							{
								goto IL_66E;
							}
							token = NumericToken.Nine;
							return true;
						}
						else
						{
							if (!(text == "2"))
							{
								goto IL_66E;
							}
							token = NumericToken.Two;
							return true;
						}
					}
					else
					{
						if (!(text == "3"))
						{
							goto IL_66E;
						}
						token = NumericToken.Three;
						return true;
					}
				}
				else if (num <= 1731450012U)
				{
					if (num != 1024243015U)
					{
						if (num != 1731450012U)
						{
							goto IL_66E;
						}
						if (!(text == "100"))
						{
							goto IL_66E;
						}
						token = NumericToken.OneHundred;
						return true;
					}
					else
					{
						if (!(text == "8"))
						{
							goto IL_66E;
						}
						token = NumericToken.Eight;
						return true;
					}
				}
				else if (num != 2381486463U)
				{
					if (num != 3052262101U)
					{
						goto IL_66E;
					}
					if (!(text == "0000"))
					{
						goto IL_66E;
					}
				}
				else
				{
					if (!(text == "20"))
					{
						goto IL_66E;
					}
					token = NumericToken.Twenty;
					return true;
				}
				token = NumericToken.Zero;
				return true;
				IL_66E:
				decimal num2;
				if (!decimal.TryParse(text, 511, CultureInfo.InvariantCulture, ref num2))
				{
					if (NumericTokenizer.TryParseInvalidNumber(text, out num2))
					{
						token = new NumericToken(num2);
						result = true;
					}
					else
					{
						result = false;
					}
				}
				else
				{
					token = new NumericToken(num2);
					result = true;
				}
			}
			catch (FormatException)
			{
				result = false;
			}
			catch (OverflowException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002DD0 File Offset: 0x00000FD0
		private static bool TryParseInvalidNumber(string numeric, out decimal result)
		{
			result = default(decimal);
			if (!numeric.Contains("-") && !numeric.Contains("+"))
			{
				return false;
			}
			string[] array = numeric.Split(new string[]
			{
				"+",
				"-"
			}, 1);
			if (array.Length == 0)
			{
				return false;
			}
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				decimal num;
				if (!decimal.TryParse(array2[i], 511, CultureInfo.InvariantCulture, ref num))
				{
					return false;
				}
				result += num;
			}
			return true;
		}

		// Token: 0x0400000C RID: 12
		private readonly StringBuilder stringBuilder = new StringBuilder();

		// Token: 0x0400000D RID: 13
		private const byte Zero = 48;

		// Token: 0x0400000E RID: 14
		private const byte Nine = 57;
	}
}
