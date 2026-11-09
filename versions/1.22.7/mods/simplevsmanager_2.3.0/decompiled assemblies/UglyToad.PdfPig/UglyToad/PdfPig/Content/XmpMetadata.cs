using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001F8 RID: 504
	public class XmpMetadata
	{
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x0003D368 File Offset: 0x0003B568
		[NotNull]
		public StreamToken MetadataStreamToken { get; }

		// Token: 0x06000C38 RID: 3128 RVA: 0x0003D370 File Offset: 0x0003B570
		internal XmpMetadata(StreamToken stream, ILookupFilterProvider filterProvider, IPdfTokenScanner pdfTokenScanner)
		{
			if (filterProvider == null)
			{
				throw new ArgumentNullException("filterProvider");
			}
			this.filterProvider = filterProvider;
			this.pdfTokenScanner = pdfTokenScanner;
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.MetadataStreamToken = stream;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0003D3AB File Offset: 0x0003B5AB
		public IReadOnlyList<byte> GetXmlBytes()
		{
			return this.MetadataStreamToken.Decode(this.filterProvider, this.pdfTokenScanner);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0003D3C4 File Offset: 0x0003B5C4
		public XDocument GetXDocument()
		{
			return XDocument.Parse(OtherEncodings.BytesAsLatin1String(Enumerable.ToArray<byte>(this.GetXmlBytes())));
		}

		// Token: 0x0400067C RID: 1660
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x0400067D RID: 1661
		private readonly IPdfTokenScanner pdfTokenScanner;
	}
}
