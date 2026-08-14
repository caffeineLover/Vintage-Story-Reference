using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.AcroForms;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Encryption;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Fonts.SystemFonts;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Parser.FileStructure;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Parser.Parts.CrossReference;
using UglyToad.PdfPig.PdfFonts;
using UglyToad.PdfPig.PdfFonts.Parser;
using UglyToad.PdfPig.PdfFonts.Parser.Handlers;
using UglyToad.PdfPig.PdfFonts.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Parser
{
	// Token: 0x020000C8 RID: 200
	internal static class PdfDocumentFactory
	{
		// Token: 0x060003CB RID: 971 RVA: 0x00016D80 File Offset: 0x00014F80
		public static PdfDocument Open(byte[] fileBytes, ParsingOptions options = null)
		{
			return PdfDocumentFactory.Open(new ByteArrayInputBytes(fileBytes), options);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00016D8E File Offset: 0x00014F8E
		public static PdfDocument Open(string filename, ParsingOptions options = null)
		{
			if (!File.Exists(filename))
			{
				throw new InvalidOperationException("No file exists at: " + filename);
			}
			return PdfDocumentFactory.Open(File.ReadAllBytes(filename), options);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00016DB8 File Offset: 0x00014FB8
		internal static PdfDocument Open(Stream stream, ParsingOptions options)
		{
			long position = stream.Position;
			StreamInputBytes inputBytes = new StreamInputBytes(stream, false);
			PdfDocument result;
			try
			{
				result = PdfDocumentFactory.Open(inputBytes, options);
			}
			catch (Exception ex)
			{
				if (position != 0L)
				{
					throw new InvalidOperationException("Could not parse document due to an error, the input stream was not at position zero when provided to the Open method.", ex);
				}
				throw;
			}
			return result;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00016E04 File Offset: 0x00015004
		private static PdfDocument Open(IInputBytes inputBytes, ParsingOptions options = null)
		{
			bool useLenientParsing = options == null || options.UseLenientParsing;
			CoreTokenScanner scanner = new CoreTokenScanner(inputBytes, 0, null);
			List<string> list = new List<string>();
			bool clipPaths = options != null && options.ClipPaths;
			if (options != null && options.Password != null)
			{
				list.Add(options.Password);
			}
			if (((options != null) ? options.Passwords : null) != null)
			{
				list.AddRange(Enumerable.Where<string>(options.Passwords, (string x) => x != null));
			}
			if (!list.Contains(string.Empty))
			{
				list.Add(string.Empty);
			}
			InternalParsingOptions parsingOptions = new InternalParsingOptions(list, useLenientParsing, clipPaths, options != null && options.SkipMissingFonts, options != null && options.SkipMissingFonts, ((options != null) ? options.Logger : null) ?? new NoOpLog());
			return PdfDocumentFactory.OpenDocument(inputBytes, scanner, parsingOptions);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00016EE8 File Offset: 0x000150E8
		private static PdfDocument OpenDocument(IInputBytes inputBytes, ISeekableTokenScanner scanner, InternalParsingOptions parsingOptions)
		{
			FilterProviderWithLookup filterProvider = new FilterProviderWithLookup(DefaultFilterProvider.Instance);
			CrossReferenceTable crossReferenceTable = null;
			XrefOffsetValidator offsetValidator = new XrefOffsetValidator(parsingOptions.Logger);
			ObjectLocationProvider objectLocationProvider = new ObjectLocationProvider(() => crossReferenceTable, inputBytes);
			PdfTokenScanner pdfTokenScanner = new PdfTokenScanner(inputBytes, objectLocationProvider, filterProvider, NoOpEncryptionHandler.Instance);
			CrossReferenceStreamParser crossReferenceStreamParser = new CrossReferenceStreamParser(filterProvider);
			CrossReferenceParser crossReferenceParser = new CrossReferenceParser(parsingOptions.Logger, offsetValidator, crossReferenceStreamParser);
			HeaderVersion headerVersion = FileHeaderParser.Parse(scanner, inputBytes, parsingOptions.UseLenientParsing, parsingOptions.Logger);
			long num = FileTrailerParser.GetFirstCrossReferenceOffset(inputBytes, scanner, parsingOptions.UseLenientParsing) + headerVersion.OffsetInFile;
			num = new CrossReferenceOffsetValidator(offsetValidator).Validate(num, scanner, inputBytes, parsingOptions.UseLenientParsing);
			crossReferenceTable = crossReferenceParser.Parse(inputBytes, parsingOptions.UseLenientParsing, num, headerVersion.OffsetInFile, pdfTokenScanner, scanner);
			EncryptionDictionary encryptionDictionary;
			ValueTuple<IndirectReference, DictionaryToken> valueTuple = PdfDocumentFactory.ParseTrailer(crossReferenceTable, parsingOptions.UseLenientParsing, pdfTokenScanner, out encryptionDictionary);
			IndirectReference item = valueTuple.Item1;
			DictionaryToken item2 = valueTuple.Item2;
			IEncryptionHandler encryptionHandler2;
			if (encryptionDictionary == null)
			{
				IEncryptionHandler encryptionHandler = NoOpEncryptionHandler.Instance;
				encryptionHandler2 = encryptionHandler;
			}
			else
			{
				IEncryptionHandler encryptionHandler = new EncryptionHandler(encryptionDictionary, crossReferenceTable.Trailer, parsingOptions.Passwords);
				encryptionHandler2 = encryptionHandler;
			}
			IEncryptionHandler newHandler = encryptionHandler2;
			pdfTokenScanner.UpdateEncryptionHandler(newHandler);
			CidFontFactory cidFontFactory = new CidFontFactory(pdfTokenScanner, filterProvider);
			EncodingReader encodingReader = new EncodingReader(pdfTokenScanner);
			Type0FontHandler type0FontHandler = new Type0FontHandler(cidFontFactory, filterProvider, pdfTokenScanner, parsingOptions.Logger);
			Type1FontHandler type1FontHandler = new Type1FontHandler(pdfTokenScanner, filterProvider, encodingReader);
			TrueTypeFontHandler trueTypeFontHandler = new TrueTypeFontHandler(parsingOptions.Logger, pdfTokenScanner, filterProvider, encodingReader, SystemFontFinder.Instance, type1FontHandler);
			FontFactory fontFactory = new FontFactory(parsingOptions.Logger, type0FontHandler, trueTypeFontHandler, type1FontHandler, new Type3FontHandler(pdfTokenScanner, filterProvider, encodingReader));
			ResourceStore resourceStore = new ResourceStore(pdfTokenScanner, fontFactory, filterProvider);
			DocumentInformation information = DocumentInformationFactory.Create(pdfTokenScanner, crossReferenceTable.Trailer, parsingOptions.UseLenientParsing);
			PageFactory pageFactory = new PageFactory(pdfTokenScanner, resourceStore, filterProvider, new PageContentParser(new ReflectionGraphicsStateOperationFactory()), parsingOptions.Logger);
			Catalog catalog = CatalogFactory.Create(item, item2, pdfTokenScanner, pageFactory, parsingOptions.Logger, parsingOptions.UseLenientParsing);
			AcroFormFactory acroFormFactory = new AcroFormFactory(pdfTokenScanner, filterProvider, crossReferenceTable);
			BookmarksProvider bookmarksProvider = new BookmarksProvider(parsingOptions.Logger, pdfTokenScanner);
			return new PdfDocument(inputBytes, headerVersion, crossReferenceTable, catalog, information, encryptionDictionary, pdfTokenScanner, filterProvider, acroFormFactory, bookmarksProvider, parsingOptions);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001710C File Offset: 0x0001530C
		private static ValueTuple<IndirectReference, DictionaryToken> ParseTrailer(CrossReferenceTable crossReferenceTable, bool isLenientParsing, IPdfTokenScanner pdfTokenScanner, out EncryptionDictionary encryptionDictionary)
		{
			encryptionDictionary = PdfDocumentFactory.GetEncryptionDictionary(crossReferenceTable, pdfTokenScanner);
			DictionaryToken dictionaryToken = DirectObjectFinder.Get<DictionaryToken>(crossReferenceTable.Trailer.Root, pdfTokenScanner);
			if (!dictionaryToken.ContainsKey(NameToken.Type) && isLenientParsing)
			{
				dictionaryToken = dictionaryToken.With(NameToken.Type, NameToken.Catalog);
			}
			return new ValueTuple<IndirectReference, DictionaryToken>(crossReferenceTable.Trailer.Root, dictionaryToken);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00017168 File Offset: 0x00015368
		private static EncryptionDictionary GetEncryptionDictionary(CrossReferenceTable crossReferenceTable, IPdfTokenScanner pdfTokenScanner)
		{
			if (crossReferenceTable.Trailer.EncryptionToken == null)
			{
				return null;
			}
			DictionaryToken encryptionDictionary;
			if (DirectObjectFinder.TryGet<DictionaryToken>(crossReferenceTable.Trailer.EncryptionToken, pdfTokenScanner, out encryptionDictionary))
			{
				return EncryptionDictionaryFactory.Read(encryptionDictionary, pdfTokenScanner);
			}
			NullToken nullToken;
			if (DirectObjectFinder.TryGet<NullToken>(crossReferenceTable.Trailer.EncryptionToken, pdfTokenScanner, out nullToken))
			{
				return null;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(43, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unrecognized encryption token in trailer: ");
			defaultInterpolatedStringHandler.AppendFormatted<IToken>(crossReferenceTable.Trailer.EncryptionToken);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
