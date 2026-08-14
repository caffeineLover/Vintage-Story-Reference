using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.PdfFonts.CidFonts;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.PdfFonts.Composite;
using UglyToad.PdfPig.PdfFonts.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.PdfFonts.Parser.Handlers
{
	// Token: 0x020000AA RID: 170
	internal class Type0FontHandler : IFontHandler
	{
		// Token: 0x060002E2 RID: 738 RVA: 0x00013904 File Offset: 0x00011B04
		public Type0FontHandler(CidFontFactory cidFontFactory, ILookupFilterProvider filterProvider, IPdfTokenScanner scanner, ILog logger)
		{
			this.cidFontFactory = cidFontFactory;
			this.filterProvider = filterProvider;
			this.scanner = scanner;
			this.logger = logger;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0001392C File Offset: 0x00011B2C
		public IFont Generate(DictionaryToken dictionary)
		{
			NameToken nameOrDefault = dictionary.GetNameOrDefault(NameToken.BaseFont);
			bool isCMapPredefined;
			CMap cmap = this.ReadEncoding(dictionary, out isCMapPredefined);
			IToken token;
			if (Type0FontHandler.TryGetFirstDescendant(dictionary, out token))
			{
				IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
				DictionaryToken dictionary2;
				if (indirectReferenceToken != null)
				{
					dictionary2 = DirectObjectFinder.Get<DictionaryToken>(indirectReferenceToken, this.scanner);
				}
				else
				{
					dictionary2 = (DictionaryToken)token;
				}
				ICidFont cidFont = this.ParseDescendant(dictionary2);
				ValueTuple<CMap, bool> ucs2CMap = Type0FontHandler.GetUcs2CMap(dictionary, isCMapPredefined, cidFont);
				CMap item = ucs2CMap.Item1;
				bool item2 = ucs2CMap.Item2;
				CMap toUnicodeCMap = null;
				if (dictionary.ContainsKey(NameToken.ToUnicode))
				{
					IToken token2 = dictionary.Data[NameToken.ToUnicode];
					StreamToken streamToken;
					NameToken nameToken;
					if (DirectObjectFinder.TryGet<StreamToken>(token2, this.scanner, out streamToken))
					{
						IReadOnlyList<byte> readOnlyList = (streamToken != null) ? streamToken.Decode(this.filterProvider, this.scanner) : null;
						if (readOnlyList != null)
						{
							toUnicodeCMap = CMapCache.Parse(new ByteArrayInputBytes(readOnlyList));
						}
					}
					else if (!DirectObjectFinder.TryGet<NameToken>(token2, this.scanner, out nameToken) || !CMapCache.TryGet(nameToken.Data, out toUnicodeCMap))
					{
						ILog log = this.logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(66, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Invalid type of toUnicode CMap encountered for font named ");
						defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameOrDefault);
						defaultInterpolatedStringHandler.AppendLiteral(". Got: ");
						defaultInterpolatedStringHandler.AppendFormatted<IToken>(token2);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				return new Type0Font(nameOrDefault, cidFont, cmap, toUnicodeCMap, item, item2);
			}
			throw new InvalidFontFormatException("No descendant font dictionary was declared for this Type 0 font. This dictionary should contain the CIDFont for the Type 0 font. " + ((dictionary != null) ? dictionary.ToString() : null));
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00013AB0 File Offset: 0x00011CB0
		private static bool TryGetFirstDescendant(DictionaryToken dictionary, out IToken descendant)
		{
			descendant = null;
			IToken token;
			if (!dictionary.TryGet(NameToken.DescendantFonts, ref token))
			{
				return false;
			}
			IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
			if (indirectReferenceToken != null)
			{
				descendant = indirectReferenceToken;
				return true;
			}
			ArrayToken arrayToken = token as ArrayToken;
			if (arrayToken != null && arrayToken.Data.Count > 0)
			{
				IndirectReferenceToken indirectReferenceToken2 = arrayToken.Data[0] as IndirectReferenceToken;
				if (indirectReferenceToken2 != null)
				{
					descendant = indirectReferenceToken2;
				}
				else
				{
					DictionaryToken dictionaryToken = arrayToken.Data[0] as DictionaryToken;
					if (dictionaryToken == null)
					{
						return false;
					}
					descendant = dictionaryToken;
				}
				return true;
			}
			return false;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00013B34 File Offset: 0x00011D34
		private ICidFont ParseDescendant(DictionaryToken dictionary)
		{
			NameToken nameOrDefault = dictionary.GetNameOrDefault(NameToken.Type);
			if (nameOrDefault == null || !nameOrDefault.Equals(NameToken.Font))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Expected 'Font' dictionary but found '");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameOrDefault);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this.cidFontFactory.Generate(dictionary);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00013BA8 File Offset: 0x00011DA8
		private CMap ReadEncoding(DictionaryToken dictionary, out bool isCMapPredefined)
		{
			isCMapPredefined = false;
			NameToken nameToken;
			CMap result;
			if (dictionary.TryGet(NameToken.Encoding, this.scanner, out nameToken))
			{
				CMap cmap;
				if (!CMapCache.TryGet(nameToken.Data, out cmap))
				{
					throw new InvalidOperationException("Missing CMap named " + nameToken.Data + ".");
				}
				CMap cmap2 = cmap;
				if (cmap2 == null)
				{
					throw new InvalidOperationException("Missing CMap named " + nameToken.Data + ".");
				}
				result = cmap2;
				isCMapPredefined = true;
			}
			else
			{
				StreamToken stream;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (!dictionary.TryGet(NameToken.Encoding, this.scanner, out stream))
				{
					defaultInterpolatedStringHandler..ctor(97, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Could not read the encoding, expected a name or a stream but it was not found in the dictionary: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				CMap cmap3 = CMapCache.Parse(new ByteArrayInputBytes(stream.Decode(this.filterProvider, this.scanner)));
				if (cmap3 == null)
				{
					defaultInterpolatedStringHandler..ctor(51, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Could not read CMap from stream in the dictionary: ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = cmap3;
			}
			return result;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00013CB0 File Offset: 0x00011EB0
		[return: TupleElementNames(new string[]
		{
			null,
			"isChineseJapaneseOrKorean"
		})]
		private static ValueTuple<CMap, bool> GetUcs2CMap(DictionaryToken dictionary, bool isCMapPredefined, ICidFont cidFont)
		{
			if (!isCMapPredefined)
			{
				return new ValueTuple<CMap, bool>(null, false);
			}
			NameToken nameOrDefault = dictionary.GetNameOrDefault(NameToken.Encoding);
			if (nameOrDefault == null)
			{
				return new ValueTuple<CMap, bool>(null, false);
			}
			bool flag = false;
			if (cidFont != null && string.Equals(cidFont.SystemInfo.Registry, "Adobe", 5))
			{
				flag = (string.Equals(cidFont.SystemInfo.Ordering, "GB1", 5) || string.Equals(cidFont.SystemInfo.Ordering, "CNS1", 5) || string.Equals(cidFont.SystemInfo.Ordering, "Japan1", 5) || string.Equals(cidFont.SystemInfo.Ordering, "Korea1", 5));
			}
			if ((nameOrDefault.Equals(NameToken.IdentityH) || nameOrDefault.Equals(NameToken.IdentityV)) && !flag)
			{
				return new ValueTuple<CMap, bool>(null, false);
			}
			if (!flag)
			{
				return new ValueTuple<CMap, bool>(null, false);
			}
			CMap cmap;
			string registry;
			string ordering;
			if (CMapCache.TryGet(cidFont.SystemInfo.ToString(), out cmap))
			{
				registry = cmap.Info.Registry;
				ordering = cmap.Info.Ordering;
			}
			else
			{
				registry = cidFont.SystemInfo.Registry;
				ordering = cidFont.SystemInfo.Ordering;
			}
			CMap cmap2;
			if (!CMapCache.TryGet(registry + "-" + ordering + "-UCS2", out cmap2))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Could not locate CMap by name: ");
				defaultInterpolatedStringHandler.AppendFormatted<CMap>(cmap);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new ValueTuple<CMap, bool>(cmap2, true);
		}

		// Token: 0x04000189 RID: 393
		private readonly CidFontFactory cidFontFactory;

		// Token: 0x0400018A RID: 394
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x0400018B RID: 395
		private readonly IPdfTokenScanner scanner;

		// Token: 0x0400018C RID: 396
		private readonly ILog logger;
	}
}
