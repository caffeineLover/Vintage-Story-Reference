using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization
{
	// Token: 0x02000004 RID: 4
	internal class DictionaryTokenizer : ITokenizer
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002142 File Offset: 0x00000342
		public bool ReadsNextByte { get; }

		// Token: 0x06000009 RID: 9 RVA: 0x0000214A File Offset: 0x0000034A
		public DictionaryTokenizer(IReadOnlyList<NameToken> requiredKeys = null)
		{
			this.requiredKeys = requiredKeys;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000215C File Offset: 0x0000035C
		public bool TryTokenize(byte currentByte, IInputBytes inputBytes, out IToken token)
		{
			long currentOffset = inputBytes.CurrentOffset;
			try
			{
				return this.TryTokenizeInternal(currentByte, inputBytes, false, out token);
			}
			catch (PdfDocumentFormatException)
			{
				if (this.requiredKeys == null)
				{
					throw;
				}
			}
			inputBytes.Seek(currentOffset);
			return this.TryTokenizeInternal(currentByte, inputBytes, true, out token);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021B0 File Offset: 0x000003B0
		private bool TryTokenizeInternal(byte currentByte, IInputBytes inputBytes, bool useRequiredKeys, out IToken token)
		{
			token = null;
			if (currentByte != 60)
			{
				return false;
			}
			bool flag = false;
			while (inputBytes.MoveNext())
			{
				if (inputBytes.CurrentByte == 60)
				{
					flag = true;
					break;
				}
				if (!ReadHelper.IsWhitespace(inputBytes.CurrentByte))
				{
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			CoreTokenScanner coreTokenScanner = new CoreTokenScanner(inputBytes, ScannerScope.Dictionary, null);
			List<IToken> list = new List<IToken>();
			while (coreTokenScanner.MoveNext())
			{
				if (!(coreTokenScanner.CurrentToken is CommentToken))
				{
					list.Add(coreTokenScanner.CurrentToken);
					if (useRequiredKeys && list.Count >= this.requiredKeys.Count * 2)
					{
						Dictionary<NameToken, IToken> dictionary = DictionaryTokenizer.ConvertToDictionary(list);
						bool flag2 = true;
						foreach (NameToken nameToken in this.requiredKeys)
						{
							IToken token2;
							if (!dictionary.TryGetValue(nameToken, ref token2) || token2 == null)
							{
								flag2 = false;
								break;
							}
						}
						if (flag2)
						{
							token = new DictionaryToken(dictionary);
							return true;
						}
					}
				}
			}
			Dictionary<NameToken, IToken> dictionary2 = DictionaryTokenizer.ConvertToDictionary(list);
			token = new DictionaryToken(dictionary2);
			return true;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022C8 File Offset: 0x000004C8
		private static Dictionary<NameToken, IToken> ConvertToDictionary(List<IToken> tokens)
		{
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			NameToken nameToken = null;
			for (int i = 0; i < tokens.Count; i++)
			{
				IToken token = tokens[i];
				if (!(nameToken == null))
				{
					NumericToken numericToken = token as NumericToken;
					if (numericToken == null)
					{
						goto IL_A4;
					}
					NumericToken numericToken2 = DictionaryTokenizer.PeekNext(tokens, i) as NumericToken;
					if (numericToken2 == null)
					{
						goto IL_A4;
					}
					if (DictionaryTokenizer.PeekNext(tokens, i + 1) == OperatorToken.R)
					{
						dictionary[nameToken] = new IndirectReferenceToken(new IndirectReference(numericToken.Long, numericToken2.Int));
						i += 2;
					}
					IL_AC:
					if (DictionaryTokenizer.PeekNext(tokens, i) == OperatorToken.Def)
					{
						i++;
					}
					nameToken = null;
					goto IL_C0;
					IL_A4:
					dictionary[nameToken] = token;
					goto IL_AC;
				}
				NameToken nameToken2 = token as NameToken;
				if (nameToken2 == null)
				{
					string text = "Expected name as dictionary key, instead got: ";
					IToken token2 = token;
					throw new PdfDocumentFormatException(text + ((token2 != null) ? token2.ToString() : null));
				}
				nameToken = nameToken2;
				IL_C0:;
			}
			return dictionary;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000023A6 File Offset: 0x000005A6
		private static IToken PeekNext(IReadOnlyList<IToken> tokens, int currentIndex)
		{
			if (tokens.Count - 1 < currentIndex + 1)
			{
				return null;
			}
			return tokens[currentIndex + 1];
		}

		// Token: 0x04000003 RID: 3
		private readonly IReadOnlyList<NameToken> requiredKeys;
	}
}
