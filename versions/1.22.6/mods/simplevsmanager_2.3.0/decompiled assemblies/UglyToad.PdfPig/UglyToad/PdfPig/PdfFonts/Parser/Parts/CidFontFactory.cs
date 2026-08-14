using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.CompactFontFormat;
using UglyToad.PdfPig.Fonts.TrueType;
using UglyToad.PdfPig.Fonts.TrueType.Parser;
using UglyToad.PdfPig.Geometry;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.PdfFonts.CidFonts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.PdfFonts.Parser.Parts
{
	// Token: 0x020000A2 RID: 162
	internal class CidFontFactory
	{
		// Token: 0x060002C4 RID: 708 RVA: 0x000123C0 File Offset: 0x000105C0
		public CidFontFactory(IPdfTokenScanner pdfScanner, ILookupFilterProvider filterProvider)
		{
			this.pdfScanner = pdfScanner;
			this.filterProvider = filterProvider;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000123D8 File Offset: 0x000105D8
		public ICidFont Generate(DictionaryToken dictionary)
		{
			NameToken nameOrDefault = dictionary.GetNameOrDefault(NameToken.Type);
			if (!NameToken.Font.Equals(nameOrDefault))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Expected 'Font' dictionary but found '");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameOrDefault);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			IReadOnlyDictionary<int, double> widths = this.ReadWidths(dictionary);
			double? defaultWidth = default(double?);
			NumericToken numericToken;
			if (dictionary.TryGet(NameToken.Dw, this.pdfScanner, out numericToken))
			{
				defaultWidth = new double?(numericToken.Double);
			}
			VerticalWritingMetrics verticalWritingMetrics = CidFontFactory.ReadVerticalDisplacements(dictionary);
			FontDescriptor descriptor = null;
			DictionaryToken dictionary2;
			if (this.TryGetFontDescriptor(dictionary, out dictionary2))
			{
				descriptor = FontDescriptorFactory.Generate(dictionary2, this.pdfScanner);
			}
			ICidFontProgram fontProgram = this.ReadDescriptorFile(descriptor);
			NameToken nameOrDefault2 = dictionary.GetNameOrDefault(NameToken.BaseFont);
			CharacterIdentifierSystemInfo systemInfo = this.GetSystemInfo(dictionary);
			NameToken nameOrDefault3 = dictionary.GetNameOrDefault(NameToken.Subtype);
			if (NameToken.CidFontType0.Equals(nameOrDefault3))
			{
				return new Type0CidFont(fontProgram, nameOrDefault, nameOrDefault3, nameOrDefault2, systemInfo, descriptor, verticalWritingMetrics, widths, defaultWidth);
			}
			if (NameToken.CidFontType2.Equals(nameOrDefault3))
			{
				CharacterIdentifierToGlyphIndexMap characterIdentifierToGlyphIndexMap = this.GetCharacterIdentifierToGlyphIndexMap(dictionary);
				return new Type2CidFont(nameOrDefault, nameOrDefault3, nameOrDefault2, systemInfo, descriptor, fontProgram, verticalWritingMetrics, widths, defaultWidth, characterIdentifierToGlyphIndexMap);
			}
			return null;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0001250C File Offset: 0x0001070C
		private bool TryGetFontDescriptor(DictionaryToken dictionary, out DictionaryToken descriptorDictionary)
		{
			descriptorDictionary = null;
			IToken token;
			if (!dictionary.TryGet(NameToken.FontDescriptor, ref token))
			{
				return false;
			}
			try
			{
				DictionaryToken dictionaryToken = DirectObjectFinder.Get<DictionaryToken>(token, this.pdfScanner);
				descriptorDictionary = dictionaryToken;
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00012558 File Offset: 0x00010758
		private ICidFontProgram ReadDescriptorFile(FontDescriptor descriptor)
		{
			if (((descriptor != null) ? descriptor.FontFile : null) == null)
			{
				return null;
			}
			StreamToken streamToken = DirectObjectFinder.Get<StreamToken>(descriptor.FontFile.ObjectKey, this.pdfScanner);
			if (streamToken == null)
			{
				return null;
			}
			IReadOnlyList<byte> readOnlyList = streamToken.Decode(this.filterProvider, this.pdfScanner);
			DescriptorFontFile.FontFileType fileType = descriptor.FontFile.FileType;
			if (fileType == DescriptorFontFile.FontFileType.TrueType)
			{
				return new PdfCidTrueTypeFont(TrueTypeFontParser.Parse(new TrueTypeDataBytes(new ByteArrayInputBytes(readOnlyList))));
			}
			if (fileType != DescriptorFontFile.FontFileType.FromSubtype)
			{
				throw new NotSupportedException("Currently only TrueType fonts are supported.");
			}
			StreamToken streamToken2;
			if (!DirectObjectFinder.TryGet<StreamToken>(descriptor.FontFile.ObjectKey, this.pdfScanner, out streamToken2))
			{
				throw new NotSupportedException("Cannot read CID font from subtype.");
			}
			NameToken nameToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!streamToken2.StreamDictionary.TryGet<NameToken>(NameToken.Subtype, ref nameToken))
			{
				defaultInterpolatedStringHandler..ctor(55, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The font file stream did not contain a subtype entry: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(streamToken2.StreamDictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (nameToken == NameToken.CidFontType0C || nameToken == NameToken.Type1C)
			{
				return new PdfCidCompactFontFormatFont(CompactFontFormatParser.Parse(new CompactFontFormatData(streamToken2.Decode(this.filterProvider, this.pdfScanner))));
			}
			if (nameToken == NameToken.OpenType)
			{
				return new PdfCidTrueTypeFont(TrueTypeFontParser.Parse(new TrueTypeDataBytes(new ByteArrayInputBytes(streamToken2.Decode(this.filterProvider, this.pdfScanner)))));
			}
			defaultInterpolatedStringHandler..ctor(34, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unexpected subtype for CID font: ");
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameToken);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00012700 File Offset: 0x00010900
		private IReadOnlyDictionary<int, double> ReadWidths(DictionaryToken dict)
		{
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			ArrayToken arrayToken;
			if (!dict.TryGet(NameToken.W, this.pdfScanner, out arrayToken))
			{
				return dictionary;
			}
			int count = arrayToken.Data.Count;
			int i = 0;
			while (i < count)
			{
				NumericToken numericToken = DirectObjectFinder.Get<NumericToken>(arrayToken.Data[i++], this.pdfScanner);
				IToken token = arrayToken.Data[i++];
				ArrayToken arrayToken2;
				if (DirectObjectFinder.TryGet<ArrayToken>(token, this.pdfScanner, out arrayToken2))
				{
					int @int = numericToken.Int;
					int count2 = arrayToken2.Data.Count;
					for (int j = 0; j < count2; j++)
					{
						NumericToken numericToken2 = DirectObjectFinder.Get<NumericToken>(arrayToken2.Data[j], this.pdfScanner);
						dictionary[@int + j] = numericToken2.Double;
					}
				}
				else
				{
					NumericToken numericToken3 = DirectObjectFinder.Get<NumericToken>(token, this.pdfScanner);
					NumericToken numericToken4 = DirectObjectFinder.Get<NumericToken>(arrayToken.Data[i++], this.pdfScanner);
					int int2 = numericToken.Int;
					int int3 = numericToken3.Int;
					double @double = numericToken4.Double;
					for (int k = int2; k <= int3; k++)
					{
						dictionary[k] = @double;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0001283C File Offset: 0x00010A3C
		private static VerticalWritingMetrics ReadVerticalDisplacements(DictionaryToken dict)
		{
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			Dictionary<int, PdfVector> dictionary2 = new Dictionary<int, PdfVector>();
			IToken token;
			VerticalVectorComponents @default;
			if (dict.TryGet(NameToken.Dw2, ref token))
			{
				ArrayToken arrayToken = token as ArrayToken;
				if (arrayToken != null)
				{
					double @double = ((NumericToken)arrayToken.Data[0]).Double;
					double double2 = ((NumericToken)arrayToken.Data[1]).Double;
					@default = new VerticalVectorComponents(@double, double2);
					goto IL_6C;
				}
			}
			@default = VerticalVectorComponents.Default;
			IL_6C:
			IToken token2;
			if (dict.TryGet(NameToken.W2, ref token2))
			{
				ArrayToken arrayToken2 = token2 as ArrayToken;
				if (arrayToken2 != null)
				{
					for (int i = 0; i < arrayToken2.Data.Count; i++)
					{
						NumericToken numericToken = (NumericToken)arrayToken2.Data[i];
						IToken token3 = arrayToken2.Data[++i];
						ArrayToken arrayToken3 = token3 as ArrayToken;
						if (arrayToken3 != null)
						{
							for (int j = 0; j < arrayToken3.Data.Count; j++)
							{
								int num = numericToken.Int + j;
								NumericToken numericToken2 = (NumericToken)arrayToken3.Data[j];
								NumericToken numericToken3 = (NumericToken)arrayToken3.Data[++j];
								NumericToken numericToken4 = (NumericToken)arrayToken3.Data[++j];
								dictionary[num] = numericToken2.Double;
								dictionary2[num] = new PdfVector(numericToken3.Double, numericToken4.Double);
							}
						}
						else
						{
							int @int = numericToken.Int;
							int int2 = ((NumericToken)token3).Int;
							NumericToken numericToken5 = (NumericToken)arrayToken2.Data[++i];
							NumericToken numericToken6 = (NumericToken)arrayToken2.Data[++i];
							NumericToken numericToken7 = (NumericToken)arrayToken2.Data[++i];
							for (int k = @int; k <= int2; k++)
							{
								dictionary[k] = numericToken5.Double;
								dictionary2[k] = new PdfVector(numericToken6.Double, numericToken7.Double);
							}
						}
					}
				}
			}
			return new VerticalWritingMetrics(@default, dictionary, dictionary2);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00012A7C File Offset: 0x00010C7C
		private CharacterIdentifierSystemInfo GetSystemInfo(DictionaryToken dictionary)
		{
			IToken token;
			if (!dictionary.TryGet(NameToken.CidSystemInfo, ref token))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(57, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No CID System Info was found in the CID Font dictionary: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			DictionaryToken dictionaryToken = token as DictionaryToken;
			if (dictionaryToken == null)
			{
				dictionaryToken = DirectObjectFinder.Get<DictionaryToken>(token, this.pdfScanner);
			}
			string registry = this.SafeKeyAccess(dictionaryToken, NameToken.Registry);
			string ordering = this.SafeKeyAccess(dictionaryToken, NameToken.Ordering);
			int intOrDefault = dictionaryToken.GetIntOrDefault(NameToken.Supplement, 0);
			return new CharacterIdentifierSystemInfo(registry, ordering, intOrDefault);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00012B08 File Offset: 0x00010D08
		private CharacterIdentifierToGlyphIndexMap GetCharacterIdentifierToGlyphIndexMap(DictionaryToken dictionary)
		{
			IToken token;
			if (!dictionary.TryGet(NameToken.CidToGidMap, ref token))
			{
				return new CharacterIdentifierToGlyphIndexMap();
			}
			NameToken nameToken;
			if (DirectObjectFinder.TryGet<NameToken>(token, this.pdfScanner, out nameToken))
			{
				return new CharacterIdentifierToGlyphIndexMap();
			}
			StreamToken stream;
			if (!DirectObjectFinder.TryGet<StreamToken>(token, this.pdfScanner, out stream))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(63, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No stream or name token found for /CIDToGIDMap in dictionary: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new CharacterIdentifierToGlyphIndexMap(stream.Decode(this.filterProvider, this.pdfScanner));
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00012BA0 File Offset: 0x00010DA0
		private string SafeKeyAccess(DictionaryToken dictionary, NameToken keyName)
		{
			IToken token;
			if (!dictionary.TryGet(keyName, ref token))
			{
				return string.Empty;
			}
			StringToken stringToken = token as StringToken;
			if (stringToken != null)
			{
				return stringToken.Data;
			}
			HexToken hexToken = token as HexToken;
			if (hexToken != null)
			{
				return hexToken.Data;
			}
			IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
			if (indirectReferenceToken == null)
			{
				return string.Empty;
			}
			StringToken stringToken2;
			if (DirectObjectFinder.TryGet<StringToken>(indirectReferenceToken, this.pdfScanner, out stringToken2))
			{
				return stringToken2.Data;
			}
			HexToken hexToken2;
			if (DirectObjectFinder.TryGet<HexToken>(indirectReferenceToken, this.pdfScanner, out hexToken2))
			{
				return hexToken2.Data;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(33, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Could not get key for name: ");
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(keyName);
			defaultInterpolatedStringHandler.AppendLiteral(" in ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x04000181 RID: 385
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x04000182 RID: 386
		private readonly IPdfTokenScanner pdfScanner;
	}
}
