using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig
{
	// Token: 0x02000004 RID: 4
	public class AdvancedPdfDocumentAccess : IDisposable
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002080 File Offset: 0x00000280
		internal AdvancedPdfDocumentAccess(IPdfTokenScanner pdfScanner, ILookupFilterProvider filterProvider, Catalog catalog)
		{
			if (pdfScanner == null)
			{
				throw new ArgumentNullException("pdfScanner");
			}
			this.pdfScanner = pdfScanner;
			if (filterProvider == null)
			{
				throw new ArgumentNullException("filterProvider");
			}
			this.filterProvider = filterProvider;
			if (catalog == null)
			{
				throw new ArgumentNullException("catalog");
			}
			this.catalog = catalog;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020D8 File Offset: 0x000002D8
		public bool TryGetEmbeddedFiles(out IReadOnlyList<EmbeddedFile> embeddedFiles)
		{
			this.GuardDisposed();
			embeddedFiles = null;
			DictionaryToken dictionary;
			DictionaryToken nameTreeNodeDictionary;
			if (!this.catalog.CatalogDictionary.TryGet(NameToken.Names, this.pdfScanner, out dictionary) || !dictionary.TryGet(NameToken.EmbeddedFiles, this.pdfScanner, out nameTreeNodeDictionary))
			{
				return false;
			}
			IReadOnlyDictionary<string, IToken> readOnlyDictionary = NameTreeParser.FlattenNameTreeToDictionary<IToken>(nameTreeNodeDictionary, this.pdfScanner, (IToken x) => x);
			if (readOnlyDictionary.Count == 0)
			{
				return false;
			}
			List<EmbeddedFile> list = new List<EmbeddedFile>();
			foreach (KeyValuePair<string, IToken> keyValuePair in readOnlyDictionary)
			{
				DictionaryToken dictionary2;
				DictionaryToken dictionary3;
				StreamToken stream;
				if (DirectObjectFinder.TryGet<DictionaryToken>(keyValuePair.Value, this.pdfScanner, out dictionary2) && dictionary2.TryGet(NameToken.Ef, this.pdfScanner, out dictionary3) && dictionary3.TryGet(NameToken.F, this.pdfScanner, out stream))
				{
					string fileSpecification = string.Empty;
					IDataToken<string> dataToken;
					if (dictionary2.TryGet(NameToken.F, this.pdfScanner, out dataToken))
					{
						fileSpecification = dataToken.Data;
					}
					IReadOnlyList<byte> bytes = stream.Decode(this.filterProvider, this.pdfScanner);
					list.Add(new EmbeddedFile(keyValuePair.Key, fileSpecification, bytes, stream));
				}
			}
			embeddedFiles = list;
			return embeddedFiles.Count > 0;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002244 File Offset: 0x00000444
		public void ReplaceIndirectObject(IndirectReference reference, Func<IToken, IToken> replacer)
		{
			ObjectToken objectToken = this.pdfScanner.Get(reference);
			IToken token = replacer.Invoke(objectToken.Data);
			this.pdfScanner.ReplaceToken(reference, token);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002278 File Offset: 0x00000478
		public void ReplaceIndirectObject(IndirectReference reference, IToken replacement)
		{
			this.pdfScanner.ReplaceToken(reference, replacement);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002287 File Offset: 0x00000487
		private void GuardDisposed()
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("AdvancedPdfDocumentAccess");
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000229C File Offset: 0x0000049C
		public void Dispose()
		{
			IPdfTokenScanner pdfTokenScanner = this.pdfScanner;
			if (pdfTokenScanner != null)
			{
				pdfTokenScanner.Dispose();
			}
			this.isDisposed = true;
		}

		// Token: 0x04000002 RID: 2
		private readonly IPdfTokenScanner pdfScanner;

		// Token: 0x04000003 RID: 3
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x04000004 RID: 4
		private readonly Catalog catalog;

		// Token: 0x04000005 RID: 5
		private bool isDisposed;
	}
}
