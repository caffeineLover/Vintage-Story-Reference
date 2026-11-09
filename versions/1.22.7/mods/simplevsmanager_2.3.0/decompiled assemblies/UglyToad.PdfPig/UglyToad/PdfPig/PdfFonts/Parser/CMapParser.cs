using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.PdfFonts.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser
{
	// Token: 0x0200009B RID: 155
	internal class CMapParser
	{
		// Token: 0x060002AD RID: 685 RVA: 0x000116A4 File Offset: 0x0000F8A4
		public CMap Parse(IInputBytes inputBytes)
		{
			ScannerScope scannerScope = 0;
			Dictionary<NameToken, IReadOnlyList<NameToken>> dictionary = new Dictionary<NameToken, IReadOnlyList<NameToken>>();
			dictionary.Add(NameToken.CidSystemInfo, new NameToken[]
			{
				NameToken.Registry,
				NameToken.Ordering,
				NameToken.Supplement
			});
			CoreTokenScanner coreTokenScanner = new CoreTokenScanner(inputBytes, scannerScope, dictionary);
			CharacterMapBuilder characterMapBuilder = new CharacterMapBuilder();
			IToken token = null;
			while (coreTokenScanner.MoveNext())
			{
				IToken currentToken = coreTokenScanner.CurrentToken;
				OperatorToken operatorToken = currentToken as OperatorToken;
				if (operatorToken != null)
				{
					string data = operatorToken.Data;
					if (!(data == "usecmap"))
					{
						if (!(data == "begincodespacerange"))
						{
							if (!(data == "beginbfchar"))
							{
								if (!(data == "beginbfrange"))
								{
									if (!(data == "begincidchar"))
									{
										if (data == "begincidrange")
										{
											NumericToken numericToken = token as NumericToken;
											if (numericToken == null)
											{
												string text = "Unexpected token preceding start of Cid ranges: ";
												IToken token2 = token;
												throw new InvalidOperationException(text + ((token2 != null) ? token2.ToString() : null));
											}
											CMapParser.CidRangeParser.Parse(numericToken, coreTokenScanner, characterMapBuilder);
										}
									}
									else
									{
										NumericToken numericToken2 = token as NumericToken;
										if (numericToken2 == null)
										{
											string text2 = "Unexpected token preceding start of Cid character mapping: ";
											IToken token3 = token;
											throw new InvalidOperationException(text2 + ((token3 != null) ? token3.ToString() : null));
										}
										CMapParser.CidCharacterParser.Parse(numericToken2, coreTokenScanner, characterMapBuilder);
									}
								}
								else
								{
									NumericToken numericToken3 = token as NumericToken;
									if (numericToken3 == null)
									{
										string text3 = "Unexpected token preceding start of base font character ranges: ";
										IToken token4 = token;
										throw new InvalidOperationException(text3 + ((token4 != null) ? token4.ToString() : null));
									}
									CMapParser.BaseFontRangeParser.Parse(numericToken3, coreTokenScanner, characterMapBuilder);
								}
							}
							else
							{
								NumericToken numericToken4 = token as NumericToken;
								if (numericToken4 == null)
								{
									string text4 = "Unexpected token preceding start of base font characters: ";
									IToken token5 = token;
									throw new InvalidOperationException(text4 + ((token5 != null) ? token5.ToString() : null));
								}
								CMapParser.BaseFontCharacterParser.Parse(numericToken4, coreTokenScanner, characterMapBuilder);
							}
						}
						else
						{
							NumericToken numericToken5 = token as NumericToken;
							if (numericToken5 == null)
							{
								string text5 = "Unexpected token preceding start of codespace range: ";
								IToken token6 = token;
								throw new InvalidOperationException(text5 + ((token6 != null) ? token6.ToString() : null));
							}
							CMapParser.CodespaceRangeParser.Parse(numericToken5, coreTokenScanner, characterMapBuilder);
						}
					}
					else
					{
						NameToken nameToken = token as NameToken;
						CMap other;
						if (nameToken == null || !this.TryParseExternal(nameToken.Data, out other))
						{
							string text6 = "Unexpected token preceding external cmap call: ";
							IToken token7 = token;
							throw new InvalidOperationException(text6 + ((token7 != null) ? token7.ToString() : null));
						}
						characterMapBuilder.UseCMap(other);
					}
				}
				else
				{
					NameToken nameToken2 = currentToken as NameToken;
					if (nameToken2 != null)
					{
						CMapParser.CidFontNameParser.Parse(nameToken2, coreTokenScanner, characterMapBuilder);
					}
				}
				token = currentToken;
			}
			return characterMapBuilder.Build();
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0001191C File Offset: 0x0000FB1C
		public bool TryParseExternal(string name, out CMap result)
		{
			result = null;
			string text = Enumerable.FirstOrDefault<string>(typeof(CMapParser).Assembly.GetManifestResourceNames(), (string x) => x.EndsWith("CMap." + name, 3));
			if (text == null)
			{
				return false;
			}
			byte[] array;
			using (Stream manifestResourceStream = typeof(CMapParser).Assembly.GetManifestResourceStream(text))
			{
				if (manifestResourceStream == null)
				{
					return false;
				}
				using (MemoryStream memoryStream = new MemoryStream())
				{
					manifestResourceStream.CopyTo(memoryStream);
					array = memoryStream.ToArray();
				}
			}
			result = this.Parse(new ByteArrayInputBytes(array));
			return true;
		}

		// Token: 0x0400017A RID: 378
		private static readonly BaseFontRangeParser BaseFontRangeParser = new BaseFontRangeParser();

		// Token: 0x0400017B RID: 379
		private static readonly BaseFontCharacterParser BaseFontCharacterParser = new BaseFontCharacterParser();

		// Token: 0x0400017C RID: 380
		private static readonly CidRangeParser CidRangeParser = new CidRangeParser();

		// Token: 0x0400017D RID: 381
		private static readonly CidFontNameParser CidFontNameParser = new CidFontNameParser();

		// Token: 0x0400017E RID: 382
		private static readonly CodespaceRangeParser CodespaceRangeParser = new CodespaceRangeParser();

		// Token: 0x0400017F RID: 383
		private static readonly CidCharacterParser CidCharacterParser = new CidCharacterParser();
	}
}
