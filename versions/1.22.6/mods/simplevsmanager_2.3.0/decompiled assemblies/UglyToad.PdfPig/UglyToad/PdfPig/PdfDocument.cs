using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.AcroForms;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Encryption;
using UglyToad.PdfPig.Exceptions;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Parser;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig
{
	// Token: 0x02000007 RID: 7
	public class PdfDocument : IDisposable
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000023D4 File Offset: 0x000005D4
		[NotNull]
		public DocumentInformation Information { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000023DC File Offset: 0x000005DC
		[NotNull]
		public Structure Structure { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000023E4 File Offset: 0x000005E4
		public AdvancedPdfDocumentAccess Advanced { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000023EC File Offset: 0x000005EC
		public decimal Version
		{
			get
			{
				return this.version.Version;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000023F9 File Offset: 0x000005F9
		public int NumberOfPages
		{
			get
			{
				return this.pages.Count;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002406 File Offset: 0x00000606
		public bool IsEncrypted
		{
			get
			{
				return this.encryptionDictionary != null;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002414 File Offset: 0x00000614
		internal PdfDocument(IInputBytes inputBytes, HeaderVersion version, CrossReferenceTable crossReferenceTable, Catalog catalog, DocumentInformation information, EncryptionDictionary encryptionDictionary, IPdfTokenScanner pdfScanner, ILookupFilterProvider filterProvider, AcroFormFactory acroFormFactory, BookmarksProvider bookmarksProvider, InternalParsingOptions parsingOptions)
		{
			this.inputBytes = inputBytes;
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this.version = version;
			this.encryptionDictionary = encryptionDictionary;
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
			if (bookmarksProvider == null)
			{
				throw new ArgumentNullException("bookmarksProvider");
			}
			this.bookmarksProvider = bookmarksProvider;
			this.parsingOptions = parsingOptions;
			if (information == null)
			{
				throw new ArgumentNullException("information");
			}
			this.Information = information;
			this.pages = catalog.Pages;
			this.namedDestinations = catalog.NamedDestinations;
			this.Structure = new Structure(catalog, crossReferenceTable, pdfScanner);
			this.Advanced = new AdvancedPdfDocumentAccess(pdfScanner, filterProvider, catalog);
			this.documentForm = new Lazy<AcroForm>(() => acroFormFactory.GetAcroForm(catalog));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002528 File Offset: 0x00000728
		public static PdfDocument Open(byte[] fileBytes, ParsingOptions options = null)
		{
			return PdfDocumentFactory.Open(fileBytes, options);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002531 File Offset: 0x00000731
		public static PdfDocument Open(string filePath, ParsingOptions options = null)
		{
			return PdfDocumentFactory.Open(filePath, options);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000253A File Offset: 0x0000073A
		public static PdfDocument Open(Stream stream, ParsingOptions options = null)
		{
			return PdfDocumentFactory.Open(stream, options);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002544 File Offset: 0x00000744
		public Page GetPage(int pageNumber)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("Cannot access page after the document is disposed.");
			}
			ILog logger = this.parsingOptions.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Accessing page ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pageNumber);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
			Page page;
			try
			{
				page = this.pages.GetPage(pageNumber, this.namedDestinations, this.parsingOptions);
			}
			catch (Exception inner)
			{
				if (this.IsEncrypted)
				{
					throw new PdfDocumentEncryptedException("Document was encrypted which may have caused error when retrieving page.", this.encryptionDictionary, inner);
				}
				throw;
			}
			return page;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000025F0 File Offset: 0x000007F0
		public IEnumerable<Page> GetPages()
		{
			int num;
			for (int i = 0; i < this.NumberOfPages; i = num + 1)
			{
				yield return this.GetPage(i + 1);
				num = i;
			}
			yield break;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002600 File Offset: 0x00000800
		public bool TryGetXmpMetadata(out XmpMetadata metadata)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("Cannot access the document metadata after the document is disposed.");
			}
			metadata = null;
			StreamToken stream;
			if (!this.Structure.Catalog.CatalogDictionary.TryGet(NameToken.Metadata, this.pdfScanner, out stream))
			{
				return false;
			}
			metadata = new XmpMetadata(stream, this.filterProvider, this.pdfScanner);
			return true;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000265E File Offset: 0x0000085E
		public bool TryGetBookmarks(out Bookmarks bookmarks)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("Cannot access the bookmarks after the document is disposed.");
			}
			bookmarks = this.bookmarksProvider.GetBookmarks(this.Structure.Catalog);
			return bookmarks != null;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002692 File Offset: 0x00000892
		public bool TryGetForm(out AcroForm form)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("Cannot access the form after the document is disposed.");
			}
			form = this.documentForm.Value;
			return form != null;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000026BC File Offset: 0x000008BC
		public void Dispose()
		{
			try
			{
				this.Advanced.Dispose();
				this.pdfScanner.Dispose();
				this.inputBytes.Dispose();
			}
			catch (Exception ex)
			{
				this.parsingOptions.Logger.Error("Failed disposing the PdfDocument due to an error.", ex);
			}
			finally
			{
				this.isDisposed = true;
			}
		}

		// Token: 0x04000013 RID: 19
		private bool isDisposed;

		// Token: 0x04000014 RID: 20
		private readonly Lazy<AcroForm> documentForm;

		// Token: 0x04000015 RID: 21
		[NotNull]
		private readonly HeaderVersion version;

		// Token: 0x04000016 RID: 22
		private readonly IInputBytes inputBytes;

		// Token: 0x04000017 RID: 23
		[CanBeNull]
		private readonly EncryptionDictionary encryptionDictionary;

		// Token: 0x04000018 RID: 24
		[NotNull]
		private readonly IPdfTokenScanner pdfScanner;

		// Token: 0x04000019 RID: 25
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x0400001A RID: 26
		private readonly BookmarksProvider bookmarksProvider;

		// Token: 0x0400001B RID: 27
		private readonly InternalParsingOptions parsingOptions;

		// Token: 0x0400001C RID: 28
		[NotNull]
		private readonly Pages pages;

		// Token: 0x0400001D RID: 29
		private readonly NamedDestinations namedDestinations;
	}
}
