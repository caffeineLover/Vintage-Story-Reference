using System;
using System.Globalization;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Parser.Parts
{
	// Token: 0x020000A3 RID: 163
	internal class CidFontNameParser : ICidFontPartParser<NameToken>
	{
		// Token: 0x060002CD RID: 717 RVA: 0x00012C6C File Offset: 0x00010E6C
		public void Parse(NameToken nameToken, ITokenScanner scanner, CharacterMapBuilder builder)
		{
			string data = nameToken.Data;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(data);
			if (num <= 2317246077U)
			{
				if (num <= 844476846U)
				{
					if (num != 717233687U)
					{
						if (num != 844476846U)
						{
							return;
						}
						if (!(data == "Registry"))
						{
							return;
						}
						StringToken stringToken;
						if (scanner.TryReadToken<StringToken>(ref stringToken))
						{
							builder.SystemInfoBuilder.Registry = stringToken.Data;
							return;
						}
					}
					else
					{
						if (!(data == "CMapName"))
						{
							return;
						}
						NameToken nameToken2;
						if (scanner.TryReadToken<NameToken>(ref nameToken2))
						{
							builder.Name = nameToken2.Data;
							return;
						}
					}
				}
				else if (num != 1106299572U)
				{
					if (num != 2317246077U)
					{
						return;
					}
					if (!(data == "WMode"))
					{
						return;
					}
					NumericToken numericToken;
					if (scanner.TryReadToken<NumericToken>(ref numericToken))
					{
						builder.WMode = numericToken.Int;
						return;
					}
				}
				else
				{
					if (!(data == "CMapType"))
					{
						return;
					}
					NumericToken numericToken2;
					if (scanner.TryReadToken<NumericToken>(ref numericToken2))
					{
						builder.Type = numericToken2.Int;
						return;
					}
				}
			}
			else if (num <= 3596244126U)
			{
				if (num != 2322697473U)
				{
					if (num != 3596244126U)
					{
						return;
					}
					if (!(data == "CIDSystemInfo"))
					{
						return;
					}
					DictionaryToken dictionary;
					if (scanner.TryReadToken<DictionaryToken>(ref dictionary))
					{
						builder.CharacterIdentifierSystemInfo = CidFontNameParser.GetCharacterIdentifier(dictionary);
					}
				}
				else
				{
					if (!(data == "Ordering"))
					{
						return;
					}
					StringToken stringToken2;
					if (scanner.TryReadToken<StringToken>(ref stringToken2))
					{
						builder.SystemInfoBuilder.Ordering = stringToken2.Data;
						return;
					}
				}
			}
			else if (num != 3663036226U)
			{
				if (num != 3937203984U)
				{
					return;
				}
				if (!(data == "CMapVersion"))
				{
					return;
				}
				if (scanner.MoveNext())
				{
					IToken currentToken = scanner.CurrentToken;
					NumericToken numericToken3 = currentToken as NumericToken;
					if (numericToken3 != null)
					{
						builder.Version = numericToken3.Data.ToString(NumberFormatInfo.InvariantInfo);
						return;
					}
					StringToken stringToken3 = currentToken as StringToken;
					if (stringToken3 != null)
					{
						builder.Version = stringToken3.Data;
						return;
					}
				}
			}
			else
			{
				if (!(data == "Supplement"))
				{
					return;
				}
				NumericToken numericToken4;
				if (scanner.TryReadToken<NumericToken>(ref numericToken4))
				{
					builder.SystemInfoBuilder.Supplement = numericToken4.Int;
					return;
				}
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00012E84 File Offset: 0x00011084
		private static CharacterIdentifierSystemInfo GetCharacterIdentifier(DictionaryToken dictionary)
		{
			IToken token;
			StringToken stringToken;
			if (dictionary.TryGet(NameToken.Registry, ref token))
			{
				stringToken = (token as StringToken);
				if (stringToken != null)
				{
					goto IL_25;
				}
			}
			stringToken = new StringToken("Adobe", 0);
			IL_25:
			IToken token2;
			StringToken stringToken2;
			if (dictionary.TryGet(NameToken.Ordering, ref token2))
			{
				stringToken2 = (token2 as StringToken);
				if (stringToken2 != null)
				{
					goto IL_4A;
				}
			}
			stringToken2 = new StringToken(string.Empty, 0);
			IL_4A:
			IToken token3;
			NumericToken numericToken;
			if (dictionary.TryGet(NameToken.Supplement, ref token3))
			{
				numericToken = (token3 as NumericToken);
				if (numericToken != null)
				{
					goto IL_72;
				}
			}
			numericToken = new NumericToken(0m);
			IL_72:
			return new CharacterIdentifierSystemInfo(stringToken.Data, stringToken2.Data, numericToken.Int);
		}
	}
}
