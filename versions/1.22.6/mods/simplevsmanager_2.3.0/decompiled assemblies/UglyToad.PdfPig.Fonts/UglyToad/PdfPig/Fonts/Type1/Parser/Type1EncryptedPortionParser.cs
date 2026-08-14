using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Type1.CharStrings;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Fonts.Type1.Parser
{
	// Token: 0x0200000C RID: 12
	internal class Type1EncryptedPortionParser
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00002CE8 File Offset: 0x00000EE8
		public ValueTuple<Type1PrivateDictionary, Type1CharStrings> Parse(IReadOnlyList<byte> bytes, bool isLenientParsing)
		{
			if (!Type1EncryptedPortionParser.IsBinary(bytes))
			{
				bytes = Type1EncryptedPortionParser.ConvertHexToBinary(bytes);
			}
			IReadOnlyList<byte> readOnlyList = Type1EncryptedPortionParser.Decrypt(bytes, 55665, 4);
			if (readOnlyList.Count == 0)
			{
				Type1PrivateDictionary type1PrivateDictionary = new Type1PrivateDictionary(new Type1PrivateDictionary.Builder());
				Type1CharStrings type1CharStrings = new Type1CharStrings(new Dictionary<string, Type1CharStrings.CommandSequence>(), new Dictionary<int, string>(), new Dictionary<int, Type1CharStrings.CommandSequence>());
				return new ValueTuple<Type1PrivateDictionary, Type1CharStrings>(type1PrivateDictionary, type1CharStrings);
			}
			Type1Tokenizer type1Tokenizer = new Type1Tokenizer(new ByteArrayInputBytes(readOnlyList));
			while (!type1Tokenizer.CurrentToken.IsPrivateDictionary)
			{
				type1Tokenizer.GetNext();
				if (type1Tokenizer.CurrentToken == null)
				{
					throw new InvalidOperationException("Did not find the private dictionary start token.");
				}
			}
			Type1Token next = type1Tokenizer.GetNext();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (next == null || next.Type != Type1Token.TokenType.Integer)
			{
				defaultInterpolatedStringHandler..ctor(95, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No length token was present in the stream following the private dictionary start, instead got ");
				defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int num = next.AsInt();
			Type1EncryptedPortionParser.ReadExpected(type1Tokenizer, Type1Token.TokenType.Name, "dict");
			Type1EncryptedPortionParser.ReadExpectedAfterOptional(type1Tokenizer, Type1Token.TokenType.Name, "def", Type1Token.TokenType.Name, "dup");
			Type1EncryptedPortionParser.ReadExpected(type1Tokenizer, Type1Token.TokenType.Name, "begin");
			int lenIv = 4;
			Type1PrivateDictionary.Builder builder = new Type1PrivateDictionary.Builder();
			int i = 0;
			while (i < num)
			{
				Type1Token next2 = type1Tokenizer.GetNext();
				if (next2 != null && next2.Type == Type1Token.TokenType.Literal)
				{
					string text = next2.Text;
					uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
					if (num2 <= 1105455451U)
					{
						if (num2 <= 910909208U)
						{
							if (num2 <= 522510585U)
							{
								if (num2 != 73654547U)
								{
									if (num2 != 520323151U)
									{
										if (num2 != 522510585U)
										{
											goto IL_A02;
										}
										if (!(text == "BlueScale"))
										{
											goto IL_A02;
										}
										builder.BlueScale = new decimal?(Type1EncryptedPortionParser.ReadNumeric(type1Tokenizer));
										Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
										goto IL_A09;
									}
									else
									{
										if (!(text == "ForceBold"))
										{
											goto IL_A02;
										}
										builder.ForceBold = new bool?(Type1EncryptedPortionParser.ReadBoolean(type1Tokenizer));
										Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
										goto IL_A09;
									}
								}
								else
								{
									if (!(text == "BlueValues"))
									{
										goto IL_A02;
									}
									IReadOnlyList<int> blueValues = Type1EncryptedPortionParser.ReadArrayValues<int>(type1Tokenizer, (Type1Token x) => x.AsInt(), false, true);
									builder.BlueValues = blueValues;
									goto IL_A09;
								}
							}
							else if (num2 <= 619077139U)
							{
								if (num2 != 609788066U)
								{
									if (num2 != 619077139U)
									{
										goto IL_A02;
									}
									if (!(text == "NP"))
									{
										goto IL_A02;
									}
									goto IL_5D8;
								}
								else if (!(text == "|-"))
								{
									goto IL_A02;
								}
							}
							else if (num2 != 839130536U)
							{
								if (num2 != 910909208U)
								{
									goto IL_A02;
								}
								if (!(text == "password"))
								{
									goto IL_A02;
								}
								int num3 = (int)Type1EncryptedPortionParser.ReadNumeric(type1Tokenizer);
								if (num3 != 5839 && !isLenientParsing)
								{
									defaultInterpolatedStringHandler..ctor(36, 1);
									defaultInterpolatedStringHandler.AppendLiteral("Type 1 font had the wrong password: ");
									defaultInterpolatedStringHandler.AppendFormatted<int>(num3);
									throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
								}
								builder.Password = new int?(num3);
								Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
								goto IL_A09;
							}
							else
							{
								if (!(text == "BlueFuzz"))
								{
									goto IL_A02;
								}
								builder.BlueFuzz = new int?((int)Type1EncryptedPortionParser.ReadNumeric(type1Tokenizer));
								Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
								goto IL_A09;
							}
						}
						else if (num2 <= 954629519U)
						{
							if (num2 != 913970238U)
							{
								if (num2 != 922378242U)
								{
									if (num2 != 954629519U)
									{
										goto IL_A02;
									}
									if (!(text == "ND"))
									{
										goto IL_A02;
									}
								}
								else
								{
									if (!(text == "Subrs"))
									{
										goto IL_A02;
									}
									builder.Subroutines = Type1EncryptedPortionParser.ReadSubroutines(type1Tokenizer, lenIv, isLenientParsing);
									goto IL_A09;
								}
							}
							else
							{
								if (!(text == "StemSnapV"))
								{
									goto IL_A02;
								}
								IReadOnlyList<decimal> stemSnapVerticalWidths = Type1EncryptedPortionParser.ReadArrayValues<decimal>(type1Tokenizer, (Type1Token x) => x.AsDecimal(), false, true);
								builder.StemSnapVerticalWidths = stemSnapVerticalWidths;
								goto IL_A09;
							}
						}
						else if (num2 <= 1055321074U)
						{
							if (num2 != 1026753310U)
							{
								if (num2 != 1055321074U)
								{
									goto IL_A02;
								}
								if (!(text == "LanguageGroup"))
								{
									goto IL_A02;
								}
								builder.LanguageGroup = new int?((int)Type1EncryptedPortionParser.ReadNumeric(type1Tokenizer));
								Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
								goto IL_A09;
							}
							else
							{
								if (!(text == "FamilyOtherBlues"))
								{
									goto IL_A02;
								}
								builder.FamilyOtherBlues = Type1EncryptedPortionParser.ReadArrayValues<int>(type1Tokenizer, (Type1Token x) => x.AsInt(), false, true);
								goto IL_A09;
							}
						}
						else if (num2 != 1081746428U)
						{
							if (num2 != 1105455451U)
							{
								goto IL_A02;
							}
							if (!(text == "StdHW"))
							{
								goto IL_A02;
							}
							decimal num4 = Type1EncryptedPortionParser.ReadArrayValues<decimal>(type1Tokenizer, (Type1Token x) => x.AsDecimal(), false, true)[0];
							builder.StandardHorizontalWidth = new decimal?(num4);
							goto IL_A09;
						}
						else
						{
							if (!(text == "StemSnapH"))
							{
								goto IL_A02;
							}
							IReadOnlyList<decimal> stemSnapHorizontalWidths = Type1EncryptedPortionParser.ReadArrayValues<decimal>(type1Tokenizer, (Type1Token x) => x.AsDecimal(), false, true);
							builder.StemSnapHorizontalWidths = stemSnapHorizontalWidths;
							goto IL_A09;
						}
						IReadOnlyList<Type1Token> noAccessDef = Type1EncryptedPortionParser.ReadProcedure(type1Tokenizer, false);
						builder.NoAccessDef = noAccessDef;
						Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
					}
					else if (num2 <= 2175677473U)
					{
						if (num2 <= 1557992299U)
						{
							if (num2 != 1338861324U)
							{
								if (num2 != 1453948063U)
								{
									if (num2 != 1557992299U)
									{
										goto IL_A02;
									}
									if (!(text == "lenIV"))
									{
										goto IL_A02;
									}
									lenIv = (int)Type1EncryptedPortionParser.ReadNumeric(type1Tokenizer);
									Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
									goto IL_A09;
								}
								else
								{
									if (!(text == "BlueShift"))
									{
										goto IL_A02;
									}
									builder.BlueShift = new int?((int)Type1EncryptedPortionParser.ReadNumeric(type1Tokenizer));
									Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
									goto IL_A09;
								}
							}
							else if (!(text == "-|"))
							{
								goto IL_A02;
							}
						}
						else if (num2 <= 1935390258U)
						{
							if (num2 != 1665879978U)
							{
								if (num2 != 1935390258U)
								{
									goto IL_A02;
								}
								if (!(text == "OtherSubrs"))
								{
									goto IL_A02;
								}
								Type1EncryptedPortionParser.ReadOtherSubroutines(type1Tokenizer, isLenientParsing);
								Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
								goto IL_A09;
							}
							else
							{
								if (!(text == "OtherBlues"))
								{
									goto IL_A02;
								}
								IReadOnlyList<int> otherBlues = Type1EncryptedPortionParser.ReadArrayValues<int>(type1Tokenizer, (Type1Token x) => x.AsInt(), false, true);
								builder.OtherBlues = otherBlues;
								goto IL_A09;
							}
						}
						else if (num2 != 2030368611U)
						{
							if (num2 != 2175677473U)
							{
								goto IL_A02;
							}
							if (!(text == "StdVW"))
							{
								goto IL_A02;
							}
							decimal num5 = Type1EncryptedPortionParser.ReadArrayValues<decimal>(type1Tokenizer, (Type1Token x) => x.AsDecimal(), false, true)[0];
							builder.StandardVerticalWidth = new decimal?(num5);
							goto IL_A09;
						}
						else if (!(text == "RD"))
						{
							goto IL_A02;
						}
						IReadOnlyList<Type1Token> rd = Type1EncryptedPortionParser.ReadProcedure(type1Tokenizer, false);
						builder.Rd = rd;
						Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
					}
					else if (num2 <= 3194594411U)
					{
						if (num2 != 2384754517U)
						{
							if (num2 != 3162123003U)
							{
								if (num2 != 3194594411U)
								{
									goto IL_A02;
								}
								if (!(text == "MinFeature"))
								{
									goto IL_A02;
								}
								try
								{
									Type1Token next3 = type1Tokenizer.GetNext();
									if (next3.Type == Type1Token.TokenType.StartArray)
									{
										IReadOnlyList<int> readOnlyList2 = Type1EncryptedPortionParser.ReadArrayValues<int>(type1Tokenizer, (Type1Token x) => x.AsInt(), true, true);
										builder.MinFeature = new MinFeature(readOnlyList2[0], readOnlyList2[1]);
									}
									else if (next3.Type == Type1Token.TokenType.StartProc)
									{
										IReadOnlyList<Type1Token> readOnlyList3 = Type1EncryptedPortionParser.ReadProcedure(type1Tokenizer, true);
										builder.MinFeature = new MinFeature(readOnlyList3[0].AsInt(), readOnlyList3[1].AsInt());
									}
								}
								catch
								{
								}
								Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
							}
							else
							{
								if (!(text == "RndStemUp"))
								{
									goto IL_A02;
								}
								builder.RoundStemUp = new bool?(Type1EncryptedPortionParser.ReadBoolean(type1Tokenizer));
								Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
							}
						}
						else
						{
							if (!(text == "ExpansionFactor"))
							{
								goto IL_A02;
							}
							builder.ExpansionFactor = new decimal?(Type1EncryptedPortionParser.ReadNumeric(type1Tokenizer));
							Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
						}
					}
					else if (num2 <= 3550098136U)
					{
						if (num2 != 3357414043U)
						{
							if (num2 != 3550098136U)
							{
								goto IL_A02;
							}
							if (!(text == "Erode"))
							{
								goto IL_A02;
							}
							Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, true);
						}
						else
						{
							if (!(text == "UniqueID"))
							{
								goto IL_A02;
							}
							int num6 = (int)Type1EncryptedPortionParser.ReadNumeric(type1Tokenizer);
							builder.UniqueId = new int?(num6);
							Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
						}
					}
					else if (num2 != 3906286190U)
					{
						if (num2 != 4178332219U)
						{
							goto IL_A02;
						}
						if (!(text == "|"))
						{
							goto IL_A02;
						}
						goto IL_5D8;
					}
					else
					{
						if (!(text == "FamilyBlues"))
						{
							goto IL_A02;
						}
						builder.FamilyBlues = Type1EncryptedPortionParser.ReadArrayValues<int>(type1Tokenizer, (Type1Token x) => x.AsInt(), false, true);
					}
					IL_A09:
					i++;
					continue;
					IL_5D8:
					IReadOnlyList<Type1Token> noAccessPut = Type1EncryptedPortionParser.ReadProcedure(type1Tokenizer, false);
					builder.NoAccessPut = noAccessPut;
					Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, false);
					goto IL_A09;
					IL_A02:
					Type1EncryptedPortionParser.ReadTillDef(type1Tokenizer, true);
					goto IL_A09;
				}
				break;
			}
			Type1Token type1Token = type1Tokenizer.CurrentToken;
			IReadOnlyList<Type1CharstringDecryptedBytes> charStrings;
			if (type1Token != null)
			{
				while (type1Token != null && type1Token.Type != Type1Token.TokenType.Literal && !string.Equals(type1Token.Text, "CharStrings", 5))
				{
					type1Token = type1Tokenizer.GetNext();
				}
				if (type1Token != null)
				{
					charStrings = Type1EncryptedPortionParser.ReadCharStrings(type1Tokenizer, lenIv, isLenientParsing);
				}
				else
				{
					charStrings = new Type1CharstringDecryptedBytes[0];
				}
			}
			else
			{
				charStrings = new Type1CharstringDecryptedBytes[0];
			}
			Type1PrivateDictionary type1PrivateDictionary2 = builder.Build();
			Type1CharStrings type1CharStrings2 = Type1CharStringParser.Parse(charStrings, builder.Subroutines ?? new Type1CharstringDecryptedBytes[0]);
			return new ValueTuple<Type1PrivateDictionary, Type1CharStrings>(type1PrivateDictionary2, type1CharStrings2);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000037A0 File Offset: 0x000019A0
		private static bool IsBinary(IReadOnlyList<byte> bytes)
		{
			if (bytes.Count < 4)
			{
				return true;
			}
			if (ReadHelper.IsWhitespace(bytes[0]))
			{
				return true;
			}
			for (int i = 1; i < 4; i++)
			{
				if (!ReadHelper.IsHex(bytes[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000037E8 File Offset: 0x000019E8
		private static IReadOnlyList<byte> ConvertHexToBinary(IReadOnlyList<byte> bytes)
		{
			List<byte> list = new List<byte>(bytes.Count / 2);
			char c = '\0';
			int num = 0;
			for (int i = 0; i < bytes.Count; i++)
			{
				char c2 = (char)bytes[i];
				if (ReadHelper.IsHex(c2))
				{
					if (num == 1)
					{
						list.Add(HexToken.Convert(c, c2));
						num = 0;
					}
					else
					{
						num++;
					}
					c = c2;
				}
			}
			return list;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003848 File Offset: 0x00001A48
		private static IReadOnlyList<byte> Decrypt(IReadOnlyList<byte> bytes, int key, int randomBytes)
		{
			if (randomBytes == -1)
			{
				return bytes;
			}
			if (randomBytes > bytes.Count || bytes.Count == 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[bytes.Count - randomBytes];
			for (int i = 0; i < bytes.Count; i++)
			{
				byte b = bytes[i] & byte.MaxValue;
				int num = (int)b ^ key >> 8;
				if (i >= randomBytes)
				{
					array[i - randomBytes] = (byte)num;
				}
				key = (((int)b + key) * 52845 + 22719 & 65535);
			}
			return array;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000038C8 File Offset: 0x00001AC8
		private static void ReadExpected(Type1Tokenizer tokenizer, Type1Token.TokenType type, string text = null)
		{
			Type1Token next = tokenizer.GetNext();
			if (next == null)
			{
				throw new InvalidOperationException("Type 1 Encrypted portion ended when a token with text '" + text + "' was expected.");
			}
			if (next.Type != type || (text != null && !string.Equals(next.Text, text, 5)))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(56, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Found invalid token ");
				defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next);
				defaultInterpolatedStringHandler.AppendLiteral(" when type ");
				defaultInterpolatedStringHandler.AppendFormatted<Type1Token.TokenType>(type);
				defaultInterpolatedStringHandler.AppendLiteral(" with text ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(" was expected.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003970 File Offset: 0x00001B70
		private static void ReadExpectedAfterOptional(Type1Tokenizer tokenizer, Type1Token.TokenType optionalType, string optionalText, Type1Token.TokenType type, string text)
		{
			Type1Token next = tokenizer.GetNext();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (next == null)
			{
				defaultInterpolatedStringHandler..ctor(76, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Type 1 Encrypted portion ended when a token with text '");
				defaultInterpolatedStringHandler.AppendFormatted(optionalText);
				defaultInterpolatedStringHandler.AppendLiteral("' or '");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral("' was expected.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (next.Type == type && string.Equals(next.Text, text, 5))
			{
				return;
			}
			if (next.Type == optionalType && string.Equals(next.Text, optionalText, 5))
			{
				Type1EncryptedPortionParser.ReadExpected(tokenizer, type, text);
				return;
			}
			defaultInterpolatedStringHandler..ctor(56, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Found invalid token ");
			defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next);
			defaultInterpolatedStringHandler.AppendLiteral(" when type ");
			defaultInterpolatedStringHandler.AppendFormatted<Type1Token.TokenType>(type);
			defaultInterpolatedStringHandler.AppendLiteral(" with text ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(" was expected.");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003A70 File Offset: 0x00001C70
		private static IReadOnlyList<Type1Token> ReadProcedure(Type1Tokenizer tokenizer, bool hasReadStartProc)
		{
			List<Type1Token> list = new List<Type1Token>();
			int num = hasReadStartProc ? 1 : -1;
			Type1EncryptedPortionParser.ReadProcedure(tokenizer, list, ref num);
			return list;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003A98 File Offset: 0x00001C98
		private static void ReadProcedure(Type1Tokenizer tokenizer, List<Type1Token> tokens, ref int depth)
		{
			if (depth == -1)
			{
				Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.StartProc, null);
				depth = 1;
			}
			if (depth == 0)
			{
				return;
			}
			Type1Token next;
			while ((next = tokenizer.GetNext()) != null)
			{
				if (next.Type == Type1Token.TokenType.StartProc)
				{
					depth++;
					Type1EncryptedPortionParser.ReadProcedure(tokenizer, tokens, ref depth);
				}
				else
				{
					if (next.Type == Type1Token.TokenType.EndProc)
					{
						depth--;
						return;
					}
					tokens.Add(next);
				}
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003AF8 File Offset: 0x00001CF8
		private static void ReadTillDef(Type1Tokenizer tokenizer, bool skip = false)
		{
			if (string.Equals(tokenizer.CurrentToken.Text, "def", 5))
			{
				return;
			}
			Type1Token next;
			while ((next = tokenizer.GetNext()) != null)
			{
				if (next.Type == Type1Token.TokenType.Name)
				{
					if (string.Equals(next.Text, "ND", 5) || string.Equals(next.Text, "|-", 5))
					{
						return;
					}
					if (string.Equals(next.Text, "def", 5))
					{
						break;
					}
					if (string.Equals(next.Text, "systemdict"))
					{
						Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.Literal, "internaldict");
						Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.Name, "known");
						Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.StartProc, null);
						while ((next = tokenizer.GetNext()) != null)
						{
							if (next.Type == Type1Token.TokenType.Name && (next.Text == "ND" || next.Text == "def"))
							{
								return;
							}
						}
					}
				}
				else if (!skip)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(70, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Encountered unexpected non-name token while reading till 'def' token: ");
					defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next);
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003C18 File Offset: 0x00001E18
		private static void ReadTillPut(Type1Tokenizer tokenizer)
		{
			Type1Token next;
			while ((next = tokenizer.GetNext()) != null && !string.Equals(next.Text, "put", 5))
			{
				string text = next.Text;
				if (text == "NP" || text == "|")
				{
					return;
				}
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003C68 File Offset: 0x00001E68
		private static IReadOnlyList<T> ReadArrayValues<T>(Type1Tokenizer tokenizer, Func<Type1Token, T> converter, bool hasReadStart = false, bool includeDef = true)
		{
			if (!hasReadStart)
			{
				Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.StartArray, null);
			}
			List<T> list = new List<T>();
			Type1Token next;
			while ((next = tokenizer.GetNext()) != null && next.Type != Type1Token.TokenType.EndArray)
			{
				if (next.Type == Type1Token.TokenType.StartArray)
				{
					IReadOnlyList<T> readOnlyList = Type1EncryptedPortionParser.ReadArrayValues<T>(tokenizer, converter, true, false);
					list.AddRange(readOnlyList);
				}
				try
				{
					T t = converter.Invoke(next);
					list.Add(t);
				}
				catch (Exception ex)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(48, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Conversion of token '");
					defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next);
					defaultInterpolatedStringHandler.AppendLiteral("' to value of type ");
					defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
					defaultInterpolatedStringHandler.AppendLiteral(" failed.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
				}
			}
			if (includeDef)
			{
				Type1EncryptedPortionParser.ReadTillDef(tokenizer, false);
			}
			return list;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003D48 File Offset: 0x00001F48
		private static decimal ReadNumeric(Type1Tokenizer tokenizer)
		{
			Type1Token next = tokenizer.GetNext();
			if (next == null || (next.Type != Type1Token.TokenType.Integer && next.Type != Type1Token.TokenType.Real))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Expected to read a numeric token, instead got: ");
				defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return next.AsDecimal();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003DB0 File Offset: 0x00001FB0
		private static bool ReadBoolean(Type1Tokenizer tokenizer)
		{
			Type1Token next = tokenizer.GetNext();
			if (next == null || (!string.Equals(next.Text, "true", 5) && !string.Equals(next.Text, "false", 5)))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Expected to read a boolean token, instead got: ");
				defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return next.AsBool();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003E2C File Offset: 0x0000202C
		private static void ReadOtherSubroutines(Type1Tokenizer tokenizer, bool isLenientParsing)
		{
			Type1Token next = tokenizer.GetNext();
			if (next.Type == Type1Token.TokenType.StartArray)
			{
				Type1EncryptedPortionParser.ReadArrayValues<Type1Token>(tokenizer, (Type1Token x) => x, true, false);
				return;
			}
			if (next.Type == Type1Token.TokenType.Integer || next.Type == Type1Token.TokenType.Real)
			{
				int num = next.AsInt();
				Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.Name, "array");
				for (int i = 0; i < num; i++)
				{
					Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.Name, "dup");
					Type1EncryptedPortionParser.ReadNumeric(tokenizer);
					Type1EncryptedPortionParser.ReadTillPut(tokenizer);
				}
				Type1EncryptedPortionParser.ReadTillDef(tokenizer, false);
				return;
			}
			if (!isLenientParsing)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(61, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to read start of /OtherSubrs array. Got start token: ");
				defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003F00 File Offset: 0x00002100
		private static IReadOnlyList<Type1CharstringDecryptedBytes> ReadSubroutines(Type1Tokenizer tokenizer, int lenIv, bool isLenientParsing)
		{
			int num = (int)Type1EncryptedPortionParser.ReadNumeric(tokenizer);
			List<Type1CharstringDecryptedBytes> list = new List<Type1CharstringDecryptedBytes>(num);
			Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.Name, "array");
			for (int i = 0; i < num; i++)
			{
				Type1Token next = tokenizer.GetNext();
				if (next.Type != Type1Token.TokenType.Name || !string.Equals(next.Text, "dup"))
				{
					break;
				}
				int index = (int)Type1EncryptedPortionParser.ReadNumeric(tokenizer);
				int num2 = (int)Type1EncryptedPortionParser.ReadNumeric(tokenizer);
				Type1Token next2 = tokenizer.GetNext();
				Type1DataToken type1DataToken = next2 as Type1DataToken;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (type1DataToken == null)
				{
					defaultInterpolatedStringHandler..ctor(61, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Found an unexpected token instead of subroutine charstring: ");
					defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next2);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (!isLenientParsing && type1DataToken.Data.Count != num2)
				{
					defaultInterpolatedStringHandler..ctor(64, 2);
					defaultInterpolatedStringHandler.AppendLiteral("The subroutine charstring ");
					defaultInterpolatedStringHandler.AppendFormatted<Type1DataToken>(type1DataToken);
					defaultInterpolatedStringHandler.AppendLiteral(" did not have the expected length of ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				IReadOnlyList<byte> bytes = Type1EncryptedPortionParser.Decrypt(type1DataToken.Data, 4330, lenIv);
				list.Add(new Type1CharstringDecryptedBytes(bytes, index));
				Type1EncryptedPortionParser.ReadTillPut(tokenizer);
			}
			Type1EncryptedPortionParser.ReadTillDef(tokenizer, false);
			return list;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004060 File Offset: 0x00002260
		private static IReadOnlyList<Type1CharstringDecryptedBytes> ReadCharStrings(Type1Tokenizer tokenizer, int lenIv, bool isLenientParsing)
		{
			int num = (int)Type1EncryptedPortionParser.ReadNumeric(tokenizer);
			Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.Name, "dict");
			Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.Name, null);
			Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.Name, "begin");
			List<Type1CharstringDecryptedBytes> list = new List<Type1CharstringDecryptedBytes>();
			for (int i = 0; i < num; i++)
			{
				Type1Token next = tokenizer.GetNext();
				if (next.Type == Type1Token.TokenType.Name && string.Equals(next.Text, "end", 5))
				{
					break;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (next.Type != Type1Token.TokenType.Literal)
				{
					defaultInterpolatedStringHandler..ctor(71, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Type 1 font error. Expected literal for charstring name, instead got: ");
					defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string text = next.Text;
				decimal num2 = Type1EncryptedPortionParser.ReadNumeric(tokenizer);
				Type1Token next2 = tokenizer.GetNext();
				Type1DataToken type1DataToken = next2 as Type1DataToken;
				if (type1DataToken == null)
				{
					defaultInterpolatedStringHandler..ctor(60, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Got wrong type of token, expected charstring, instead got: ");
					defaultInterpolatedStringHandler.AppendFormatted<Type1Token>(next2);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (!isLenientParsing && type1DataToken.Data.Count != num2)
				{
					defaultInterpolatedStringHandler..ctor(53, 2);
					defaultInterpolatedStringHandler.AppendLiteral("The charstring ");
					defaultInterpolatedStringHandler.AppendFormatted<Type1DataToken>(type1DataToken);
					defaultInterpolatedStringHandler.AppendLiteral(" did not have the expected length of ");
					defaultInterpolatedStringHandler.AppendFormatted<decimal>(num2);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				IReadOnlyList<byte> bytes = Type1EncryptedPortionParser.Decrypt(type1DataToken.Data, 4330, lenIv);
				list.Add(new Type1CharstringDecryptedBytes(text, bytes, i));
				Type1EncryptedPortionParser.ReadTillDef(tokenizer, false);
			}
			Type1EncryptedPortionParser.ReadExpected(tokenizer, Type1Token.TokenType.Name, "end");
			return list;
		}

		// Token: 0x0400002B RID: 43
		private const ushort EexecEncryptionKey = 55665;

		// Token: 0x0400002C RID: 44
		private const int EexecRandomBytes = 4;

		// Token: 0x0400002D RID: 45
		private const int Len4Bytes = 4;

		// Token: 0x0400002E RID: 46
		private const int Password = 5839;

		// Token: 0x0400002F RID: 47
		private const int CharstringEncryptionKey = 4330;
	}
}
