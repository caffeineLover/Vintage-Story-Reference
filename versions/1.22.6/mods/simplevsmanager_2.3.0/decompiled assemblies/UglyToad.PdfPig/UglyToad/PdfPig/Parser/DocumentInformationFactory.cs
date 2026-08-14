using System;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Parser
{
	// Token: 0x020000C3 RID: 195
	internal static class DocumentInformationFactory
	{
		// Token: 0x060003BD RID: 957 RVA: 0x000160DC File Offset: 0x000142DC
		public static DocumentInformation Create(IPdfTokenScanner pdfTokenScanner, TrailerDictionary trailer, bool isLenientParsing)
		{
			if (trailer.Info == null)
			{
				return DocumentInformation.Default;
			}
			IToken token = DirectObjectFinder.Get<IToken>(trailer.Info.Value, pdfTokenScanner);
			DictionaryToken dictionaryToken = token as DictionaryToken;
			if (dictionaryToken != null)
			{
				string entryOrDefault = DocumentInformationFactory.GetEntryOrDefault(dictionaryToken, NameToken.Title);
				string entryOrDefault2 = DocumentInformationFactory.GetEntryOrDefault(dictionaryToken, NameToken.Author);
				string entryOrDefault3 = DocumentInformationFactory.GetEntryOrDefault(dictionaryToken, NameToken.Subject);
				string entryOrDefault4 = DocumentInformationFactory.GetEntryOrDefault(dictionaryToken, NameToken.Keywords);
				string entryOrDefault5 = DocumentInformationFactory.GetEntryOrDefault(dictionaryToken, NameToken.Creator);
				string entryOrDefault6 = DocumentInformationFactory.GetEntryOrDefault(dictionaryToken, NameToken.Producer);
				string entryOrDefault7 = DocumentInformationFactory.GetEntryOrDefault(dictionaryToken, NameToken.CreationDate);
				string entryOrDefault8 = DocumentInformationFactory.GetEntryOrDefault(dictionaryToken, NameToken.ModDate);
				return new DocumentInformation(dictionaryToken, entryOrDefault, entryOrDefault2, entryOrDefault3, entryOrDefault4, entryOrDefault5, entryOrDefault6, entryOrDefault7, entryOrDefault8);
			}
			StreamToken streamToken = token as StreamToken;
			if (streamToken != null)
			{
				DictionaryToken streamDictionary = streamToken.StreamDictionary;
				NameToken nameToken;
				if (!streamDictionary.TryGet<NameToken>(NameToken.Type, ref nameToken) || nameToken != "Metadata")
				{
					throw new PdfDocumentFormatException("Unknown document metadata type was found");
				}
				NameToken nameToken2;
				if (!streamDictionary.TryGet<NameToken>(NameToken.Subtype, ref nameToken2) || nameToken2 != "XML")
				{
					throw new PdfDocumentFormatException("Unknown document metadata subtype was found");
				}
				return DocumentInformation.Default;
			}
			else
			{
				if (isLenientParsing)
				{
					return DocumentInformation.Default;
				}
				throw new PdfDocumentFormatException("Unknown document information token was found " + token.GetType().Name);
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00016230 File Offset: 0x00014430
		private static string GetEntryOrDefault(DictionaryToken infoDictionary, NameToken key)
		{
			if (infoDictionary == null)
			{
				return null;
			}
			IToken token;
			if (!infoDictionary.TryGet(key, ref token))
			{
				return null;
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
			return null;
		}
	}
}
