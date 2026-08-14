using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.AdobeFontMetrics;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Fonts.SystemFonts;
using UglyToad.PdfPig.Fonts.TrueType;
using UglyToad.PdfPig.Fonts.TrueType.Parser;
using UglyToad.PdfPig.Fonts.TrueType.Tables;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.PdfFonts.Simple;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.PdfFonts.Parser.Handlers
{
	// Token: 0x020000A9 RID: 169
	internal class TrueTypeFontHandler : IFontHandler
	{
		// Token: 0x060002DF RID: 735 RVA: 0x000133F8 File Offset: 0x000115F8
		public TrueTypeFontHandler(ILog log, IPdfTokenScanner pdfScanner, ILookupFilterProvider filterProvider, IEncodingReader encodingReader, ISystemFontFinder systemFontFinder, IFontHandler type1FontHandler)
		{
			this.log = log;
			this.filterProvider = filterProvider;
			this.encodingReader = encodingReader;
			this.systemFontFinder = systemFontFinder;
			this.type1FontHandler = type1FontHandler;
			this.pdfScanner = pdfScanner;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00013430 File Offset: 0x00011630
		public IFont Generate(DictionaryToken dictionary)
		{
			NumericToken numericToken;
			IToken token;
			if (!dictionary.TryGetOptionalTokenDirect(NameToken.FirstChar, this.pdfScanner, out numericToken) || !dictionary.TryGet(NameToken.FontDescriptor, this.pdfScanner, out token) || !dictionary.TryGet(NameToken.Widths, ref token))
			{
				NameToken nameToken;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (!dictionary.TryGetOptionalTokenDirect(NameToken.BaseFont, this.pdfScanner, out nameToken))
				{
					defaultInterpolatedStringHandler..ctor(90, 1);
					defaultInterpolatedStringHandler.AppendLiteral("The provided TrueType font dictionary did not contain a /FirstChar or a /BaseFont entry: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				AdobeFontMetrics adobeFontMetrics = Standard14.GetAdobeFontMetrics(nameToken.Data);
				if (adobeFontMetrics == null)
				{
					defaultInterpolatedStringHandler..ctor(103, 1);
					defaultInterpolatedStringHandler.AppendLiteral("The provided TrueType font dictionary did not have a /FirstChar and did not match a Standard 14 font: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				TrueTypeFont trueTypeFont = this.systemFontFinder.GetTrueTypeFont(nameToken.Data);
				Encoding encoding = this.encodingReader.Read(dictionary, null, null);
				if (encoding == null)
				{
					encoding = new AdobeFontMetricsEncoding(adobeFontMetrics);
				}
				int? firstCharacterCode = default(int?);
				double[] widths = null;
				if (dictionary.TryGet(NameToken.FirstChar, this.pdfScanner, out numericToken))
				{
					firstCharacterCode = new int?(numericToken.Int);
				}
				ArrayToken arrayToken;
				if (dictionary.TryGet(NameToken.Widths, this.pdfScanner, out arrayToken))
				{
					widths = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken.Data), (NumericToken x) => x.Double));
				}
				return new TrueTypeStandard14FallbackSimpleFont(nameToken, adobeFontMetrics, encoding, trueTypeFont, new TrueTypeStandard14FallbackSimpleFont.MetricOverrides(firstCharacterCode, widths));
			}
			else
			{
				int @int = numericToken.Int;
				double[] widths2 = FontDictionaryAccessHelper.GetWidths(this.pdfScanner, dictionary);
				FontDescriptor fontDescriptor = FontDictionaryAccessHelper.GetFontDescriptor(this.pdfScanner, dictionary);
				IFontHandler fontHandler;
				TrueTypeFont trueTypeFont2 = this.ParseTrueTypeFont(fontDescriptor, out fontHandler);
				if (trueTypeFont2 == null && fontHandler != null)
				{
					return fontHandler.Generate(dictionary);
				}
				NameToken name = FontDictionaryAccessHelper.GetName(this.pdfScanner, dictionary, fontDescriptor);
				CMap toUnicodeCMap = null;
				IToken token2;
				if (dictionary.TryGet(NameToken.ToUnicode, ref token2))
				{
					try
					{
						IReadOnlyList<byte> readOnlyList = DirectObjectFinder.Get<StreamToken>(token2, this.pdfScanner).Decode(this.filterProvider, this.pdfScanner);
						if (readOnlyList != null)
						{
							toUnicodeCMap = CMapCache.Parse(new ByteArrayInputBytes(readOnlyList));
						}
					}
					catch (Exception ex)
					{
						this.log.Error("Failed to decode ToUnicode CMap for a TrueType font in file due to an exception.", ex);
					}
				}
				Encoding encoding2 = this.encodingReader.Read(dictionary, fontDescriptor, null);
				if (encoding2 == null)
				{
					bool flag;
					if (trueTypeFont2 == null)
					{
						flag = (null != null);
					}
					else
					{
						TableRegister tableRegister = trueTypeFont2.TableRegister;
						flag = (((tableRegister != null) ? tableRegister.CMapTable : null) != null);
					}
					if (flag)
					{
						PostScriptTable postScriptTable = trueTypeFont2.TableRegister.PostScriptTable;
						if (((postScriptTable != null) ? postScriptTable.GlyphNames : null) != null)
						{
							PostScriptTable postScriptTable2 = trueTypeFont2.TableRegister.PostScriptTable;
							Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
							for (int i = 0; i < 256; i++)
							{
								int num;
								if (trueTypeFont2.TableRegister.CMapTable.TryGetGlyphIndex(i, ref num))
								{
									string text;
									if (num >= 0 && num < postScriptTable2.GlyphNames.Count)
									{
										text = postScriptTable2.GlyphNames[num];
									}
									else
									{
										text = num.ToString(CultureInfo.InvariantCulture);
									}
									dictionary2[i] = text;
								}
							}
							encoding2 = new BuiltInEncoding(dictionary2);
						}
					}
				}
				return new TrueTypeSimpleFont(name, fontDescriptor, toUnicodeCMap, encoding2, trueTypeFont2, @int, widths2);
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00013774 File Offset: 0x00011974
		private TrueTypeFont ParseTrueTypeFont(FontDescriptor descriptor, out IFontHandler actualHandler)
		{
			actualHandler = null;
			if (descriptor.FontFile == null)
			{
				try
				{
					return this.systemFontFinder.GetTrueTypeFont(descriptor.FontName.Data);
				}
				catch (Exception ex)
				{
					ILog log = this.log;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(37, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Failed finding system font by name: ");
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(descriptor.FontName);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
				}
				return null;
			}
			TrueTypeFont result;
			try
			{
				StreamToken streamToken = DirectObjectFinder.Get<StreamToken>(descriptor.FontFile.ObjectKey, this.pdfScanner);
				IReadOnlyList<byte> readOnlyList = streamToken.Decode(this.filterProvider, this.pdfScanner);
				if (descriptor.FontFile.FileType == DescriptorFontFile.FontFileType.FromSubtype)
				{
					bool flag = true;
					NameToken nameToken;
					if (streamToken.StreamDictionary.TryGet(NameToken.Subtype, this.pdfScanner, out nameToken))
					{
						if (nameToken == NameToken.Type1C)
						{
							actualHandler = this.type1FontHandler;
							return null;
						}
						if (nameToken == NameToken.OpenType)
						{
							flag = false;
						}
					}
					if (flag)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Expected a TrueType font in the TrueType font descriptor, instead it was ");
						defaultInterpolatedStringHandler.AppendFormatted<DescriptorFontFile.FontFileType>(descriptor.FontFile.FileType);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				result = TrueTypeFontParser.Parse(new TrueTypeDataBytes(new ByteArrayInputBytes(readOnlyList)));
			}
			catch (Exception ex2)
			{
				this.log.Error("Could not parse the TrueType font.", ex2);
				result = null;
			}
			return result;
		}

		// Token: 0x04000183 RID: 387
		private readonly ILog log;

		// Token: 0x04000184 RID: 388
		private readonly IPdfTokenScanner pdfScanner;

		// Token: 0x04000185 RID: 389
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x04000186 RID: 390
		private readonly IEncodingReader encodingReader;

		// Token: 0x04000187 RID: 391
		private readonly ISystemFontFinder systemFontFinder;

		// Token: 0x04000188 RID: 392
		private readonly IFontHandler type1FontHandler;
	}
}
