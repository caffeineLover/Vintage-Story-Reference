using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Fonts.Type1.CharStrings;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Fonts.Type1.Parser
{
	// Token: 0x0200000D RID: 13
	public static class Type1FontParser
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00004220 File Offset: 0x00002420
		public static Type1Font Parse(IInputBytes inputBytes, int length1, int length2)
		{
			byte? b = inputBytes.Peek();
			int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : default(int?);
			int num2 = 128;
			bool flag = num.GetValueOrDefault() == num2 & num != null;
			IReadOnlyList<byte> bytes = new byte[0];
			if (flag)
			{
				ValueTuple<byte[], byte[]> valueTuple = Type1FontParser.ReadPfbHeader(inputBytes);
				byte[] item = valueTuple.Item1;
				bytes = valueTuple.Item2;
				inputBytes = new ByteArrayInputBytes(item);
			}
			CoreTokenScanner coreTokenScanner = new CoreTokenScanner(inputBytes, 0, null);
			CommentToken commentToken;
			if (!coreTokenScanner.TryReadToken<CommentToken>(ref commentToken) || !commentToken.Data.StartsWith("!"))
			{
				throw new InvalidFontFormatException("The Type1 program did not start with '%!'.");
			}
			string[] array = commentToken.Data.Split(new string[]
			{
				" "
			}, 1);
			string name;
			if (array.Length == 3)
			{
				name = array[1];
			}
			else
			{
				name = "Unknown";
			}
			List<string> list = new List<string>();
			while (coreTokenScanner.MoveNext())
			{
				CommentToken commentToken2 = coreTokenScanner.CurrentToken as CommentToken;
				if (commentToken2 == null)
				{
					break;
				}
				list.Add(commentToken2.Data);
			}
			List<DictionaryToken> dictionaries = new List<DictionaryToken>();
			Type1ArrayTokenizer type1ArrayTokenizer = new Type1ArrayTokenizer();
			Type1NameTokenizer type1NameTokenizer = new Type1NameTokenizer();
			coreTokenScanner.RegisterCustomTokenizer(123, type1ArrayTokenizer);
			coreTokenScanner.RegisterCustomTokenizer(47, type1NameTokenizer);
			try
			{
				List<byte> list2 = new List<byte>();
				Type1FontParser.PreviousTokenSet previousTokenSet = new Type1FontParser.PreviousTokenSet();
				previousTokenSet.Add(coreTokenScanner.CurrentToken);
				while (coreTokenScanner.MoveNext())
				{
					OperatorToken operatorToken = coreTokenScanner.CurrentToken as OperatorToken;
					if (operatorToken != null)
					{
						if (object.Equals(coreTokenScanner.CurrentToken, OperatorToken.Eexec))
						{
							int num3 = 0;
							while (inputBytes.MoveNext())
							{
								if (inputBytes.CurrentByte == (byte)"cleartomark".get_Chars(num3))
								{
									num3++;
								}
								else
								{
									if (num3 > 0)
									{
										for (int i = 0; i < num3; i++)
										{
											list2.Add((byte)"cleartomark".get_Chars(i));
										}
									}
									num3 = 0;
								}
								if (num3 == "cleartomark".Length)
								{
									break;
								}
								if (num3 <= 0)
								{
									list2.Add(inputBytes.CurrentByte);
								}
							}
						}
						else
						{
							Type1FontParser.HandleOperator(operatorToken, coreTokenScanner, previousTokenSet, dictionaries);
						}
					}
					previousTokenSet.Add(coreTokenScanner.CurrentToken);
				}
				if (!flag)
				{
					bytes = list2;
				}
			}
			finally
			{
				coreTokenScanner.DeregisterCustomTokenizer(type1ArrayTokenizer);
				coreTokenScanner.DeregisterCustomTokenizer(type1NameTokenizer);
			}
			IReadOnlyDictionary<int, string> encoding = Type1FontParser.GetEncoding(dictionaries);
			ArrayToken fontMatrix = Type1FontParser.GetFontMatrix(dictionaries);
			PdfRectangle? boundingBox = Type1FontParser.GetBoundingBox(dictionaries);
			ValueTuple<Type1PrivateDictionary, Type1CharStrings> valueTuple2 = Type1FontParser.EncryptedPortionParser.Parse(bytes, false);
			Type1PrivateDictionary item2 = valueTuple2.Item1;
			Type1CharStrings item3 = valueTuple2.Item2;
			return new Type1Font(name, encoding, fontMatrix, boundingBox.GetValueOrDefault(), item2, item3);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000044B0 File Offset: 0x000026B0
		[return: TupleElementNames(new string[]
		{
			"ascii",
			"binary"
		})]
		private static ValueTuple<byte[], byte[]> ReadPfbHeader(IInputBytes bytes)
		{
			Type1FontParser.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.bytes = bytes;
			int num = Type1FontParser.<ReadPfbHeader>g__ReadSize|4_0(1, ref CS$<>8__locals1);
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				CS$<>8__locals1.bytes.MoveNext();
				array[i] = CS$<>8__locals1.bytes.CurrentByte;
			}
			int num2 = Type1FontParser.<ReadPfbHeader>g__ReadSize|4_0(2, ref CS$<>8__locals1);
			byte[] array2 = new byte[num2];
			for (int i = 0; i < num2; i++)
			{
				CS$<>8__locals1.bytes.MoveNext();
				array2[i] = CS$<>8__locals1.bytes.CurrentByte;
			}
			return new ValueTuple<byte[], byte[]>(array, array2);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004540 File Offset: 0x00002740
		private static void HandleOperator(OperatorToken token, ISeekableTokenScanner scanner, Type1FontParser.PreviousTokenSet set, List<DictionaryToken> dictionaries)
		{
			if (token.Data == "dict")
			{
				DictionaryToken dictionaryToken = Type1FontParser.ReadDictionary(((NumericToken)set[0]).Int, scanner);
				dictionaries.Add(dictionaryToken);
				return;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004580 File Offset: 0x00002780
		private static DictionaryToken ReadDictionary(int keys, ISeekableTokenScanner scanner)
		{
			IToken token = null;
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			while (scanner.MoveNext())
			{
				OperatorToken operatorToken = scanner.CurrentToken as OperatorToken;
				if (operatorToken != null && !(operatorToken.Data != "begin"))
				{
					break;
				}
			}
			for (int i = 0; i < keys; i++)
			{
				NameToken nameToken;
				if (!scanner.TryReadToken<NameToken>(ref nameToken))
				{
					return new DictionaryToken(dictionary);
				}
				if (nameToken.Data.Equals(NameToken.Encoding))
				{
					ValueTuple<ArrayToken, NameToken> valueTuple = Type1FontParser.ReadEncoding(scanner);
					Dictionary<NameToken, IToken> dictionary2 = dictionary;
					NameToken nameToken2 = nameToken;
					IToken item = valueTuple.Item1;
					dictionary2[nameToken2] = (item ?? valueTuple.Item2);
				}
				else
				{
					while (scanner.MoveNext())
					{
						if (scanner.CurrentToken == OperatorToken.Def)
						{
							dictionary[nameToken] = token;
							break;
						}
						if (scanner.CurrentToken == OperatorToken.Dict)
						{
							NumericToken numericToken = token as NumericToken;
							if (numericToken == null)
							{
								return new DictionaryToken(dictionary);
							}
							token = Type1FontParser.ReadDictionary(numericToken.Int, scanner);
						}
						else if (scanner.CurrentToken != OperatorToken.Readonly)
						{
							OperatorToken operatorToken2 = scanner.CurrentToken as OperatorToken;
							if (operatorToken2 == null || !(operatorToken2.Data == "end"))
							{
								token = scanner.CurrentToken;
							}
						}
					}
				}
			}
			return new DictionaryToken(dictionary);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000046B8 File Offset: 0x000028B8
		[return: TupleElementNames(new string[]
		{
			"encoding",
			"name"
		})]
		private static ValueTuple<ArrayToken, NameToken> ReadEncoding(ISeekableTokenScanner scanner)
		{
			Type1FontParser.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.scanner = scanner;
			List<IToken> list = new List<IToken>();
			NumericToken numericToken;
			if (!CS$<>8__locals1.scanner.TryReadToken<NumericToken>(ref numericToken))
			{
				OperatorToken operatorToken = CS$<>8__locals1.scanner.CurrentToken as OperatorToken;
				if (operatorToken != null && operatorToken.Data.Equals(NameToken.StandardEncoding))
				{
					return new ValueTuple<ArrayToken, NameToken>(null, NameToken.StandardEncoding);
				}
				return new ValueTuple<ArrayToken, NameToken>(new ArrayToken(list), null);
			}
			else
			{
				OperatorToken operatorToken2;
				if (!CS$<>8__locals1.scanner.TryReadToken<OperatorToken>(ref operatorToken2) || operatorToken2.Data != "array")
				{
					return new ValueTuple<ArrayToken, NameToken>(new ArrayToken(list), null);
				}
				bool flag = false;
				while (CS$<>8__locals1.scanner.MoveNext())
				{
					OperatorToken operatorToken3 = CS$<>8__locals1.scanner.CurrentToken as OperatorToken;
					if (operatorToken3 != null && !(operatorToken3.Data != "for"))
					{
						break;
					}
					OperatorToken operatorToken4 = CS$<>8__locals1.scanner.CurrentToken as OperatorToken;
					if (operatorToken4 != null)
					{
						if (string.Equals(operatorToken4.Data, "for", 5))
						{
							break;
						}
						if (string.Equals(operatorToken4.Data, OperatorToken.Dup.Data, 5))
						{
							flag = true;
							break;
						}
					}
				}
				if (CS$<>8__locals1.scanner.CurrentToken != OperatorToken.For && !flag)
				{
					return new ValueTuple<ArrayToken, NameToken>(new ArrayToken(list), null);
				}
				while ((flag || CS$<>8__locals1.scanner.MoveNext()) && !Type1FontParser.<ReadEncoding>g__IsDefOrReadonly|7_0(ref CS$<>8__locals1))
				{
					flag = false;
					if (CS$<>8__locals1.scanner.CurrentToken != OperatorToken.Dup)
					{
						throw new InvalidFontFormatException("Expected the array for encoding to begin with 'dup'.");
					}
					CS$<>8__locals1.scanner.MoveNext();
					NumericToken numericToken2 = (NumericToken)CS$<>8__locals1.scanner.CurrentToken;
					CS$<>8__locals1.scanner.MoveNext();
					NameToken nameToken = (NameToken)CS$<>8__locals1.scanner.CurrentToken;
					OperatorToken operatorToken5;
					if (!CS$<>8__locals1.scanner.TryReadToken<OperatorToken>(ref operatorToken5) || operatorToken5 != OperatorToken.Put)
					{
						throw new InvalidFontFormatException("Expected the array entry to end with 'put'.");
					}
					list.Add(numericToken2);
					list.Add(nameToken);
				}
				while (CS$<>8__locals1.scanner.CurrentToken != OperatorToken.Def && CS$<>8__locals1.scanner.MoveNext())
				{
				}
				return new ValueTuple<ArrayToken, NameToken>(new ArrayToken(list), null);
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000048D4 File Offset: 0x00002AD4
		private static IReadOnlyDictionary<int, string> GetEncoding(IReadOnlyList<DictionaryToken> dictionaries)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			using (IEnumerator<DictionaryToken> enumerator = dictionaries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IToken token;
					if (enumerator.Current.TryGet(NameToken.Encoding, ref token))
					{
						ArrayToken arrayToken = token as ArrayToken;
						if (arrayToken != null)
						{
							for (int i = 0; i < arrayToken.Data.Count; i += 2)
							{
								NumericToken numericToken = (NumericToken)arrayToken.Data[i];
								NameToken nameToken = (NameToken)arrayToken.Data[i + 1];
								dictionary[numericToken.Int] = nameToken.Data;
							}
							return dictionary;
						}
						NameToken nameToken2 = token as NameToken;
						if (nameToken2 != null && nameToken2.Equals(NameToken.StandardEncoding))
						{
							return StandardEncoding.Instance.CodeToNameMap;
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000049C4 File Offset: 0x00002BC4
		private static ArrayToken GetFontMatrix(IReadOnlyList<DictionaryToken> dictionaries)
		{
			using (IEnumerator<DictionaryToken> enumerator = dictionaries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IToken token;
					if (enumerator.Current.TryGet(NameToken.FontMatrix, ref token))
					{
						ArrayToken arrayToken = token as ArrayToken;
						if (arrayToken != null)
						{
							return arrayToken;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004A24 File Offset: 0x00002C24
		private static PdfRectangle? GetBoundingBox(IReadOnlyList<DictionaryToken> dictionaries)
		{
			using (IEnumerator<DictionaryToken> enumerator = dictionaries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IToken token;
					if (enumerator.Current.TryGet(NameToken.FontBbox, ref token))
					{
						ArrayToken arrayToken = token as ArrayToken;
						if (arrayToken != null && arrayToken.Data.Count == 4)
						{
							NumericToken numericToken = (NumericToken)arrayToken.Data[0];
							NumericToken numericToken2 = (NumericToken)arrayToken.Data[1];
							NumericToken numericToken3 = (NumericToken)arrayToken.Data[2];
							NumericToken numericToken4 = (NumericToken)arrayToken.Data[3];
							return new PdfRectangle?(new PdfRectangle(numericToken.Double, numericToken2.Double, numericToken3.Double, numericToken4.Double));
						}
					}
				}
			}
			return default(PdfRectangle?);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004B1C File Offset: 0x00002D1C
		[CompilerGenerated]
		internal static int <ReadPfbHeader>g__ReadSize|4_0(byte recordType, ref Type1FontParser.<>c__DisplayClass4_0 A_1)
		{
			A_1.bytes.MoveNext();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (A_1.bytes.CurrentByte != 128)
			{
				defaultInterpolatedStringHandler..ctor(77, 1);
				defaultInterpolatedStringHandler.AppendLiteral("File does not start with 0x80, which indicates a full PFB file. Instead got: ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(A_1.bytes.CurrentByte);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			A_1.bytes.MoveNext();
			if (A_1.bytes.CurrentByte != recordType)
			{
				defaultInterpolatedStringHandler..ctor(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Encountered unexpected header type in the PFB file: ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(A_1.bytes.CurrentByte);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			A_1.bytes.MoveNext();
			int currentByte = (int)A_1.bytes.CurrentByte;
			A_1.bytes.MoveNext();
			int num = currentByte + ((int)A_1.bytes.CurrentByte << 8);
			A_1.bytes.MoveNext();
			int num2 = num + ((int)A_1.bytes.CurrentByte << 16);
			A_1.bytes.MoveNext();
			return num2 + ((int)A_1.bytes.CurrentByte << 24);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004C32 File Offset: 0x00002E32
		[CompilerGenerated]
		internal static bool <ReadEncoding>g__IsDefOrReadonly|7_0(ref Type1FontParser.<>c__DisplayClass7_0 A_0)
		{
			return A_0.scanner.CurrentToken == OperatorToken.Def || A_0.scanner.CurrentToken == OperatorToken.Readonly;
		}

		// Token: 0x04000030 RID: 48
		private const string ClearToMark = "cleartomark";

		// Token: 0x04000031 RID: 49
		private const int PfbFileIndicator = 128;

		// Token: 0x04000032 RID: 50
		private static readonly Type1EncryptedPortionParser EncryptedPortionParser = new Type1EncryptedPortionParser();

		// Token: 0x020000BC RID: 188
		private class PreviousTokenSet
		{
			// Token: 0x1700022B RID: 555
			public IToken this[int index]
			{
				get
				{
					return this.tokens[2 - index];
				}
			}

			// Token: 0x060004F1 RID: 1265 RVA: 0x0001F4AD File Offset: 0x0001D6AD
			public void Add(IToken token)
			{
				this.tokens[0] = this.tokens[1];
				this.tokens[1] = this.tokens[2];
				this.tokens[2] = token;
			}

			// Token: 0x0400043F RID: 1087
			private readonly IToken[] tokens = new IToken[3];
		}
	}
}
