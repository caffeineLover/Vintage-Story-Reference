using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig
{
	// Token: 0x02000009 RID: 9
	public class Structure
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000028A5 File Offset: 0x00000AA5
		[NotNull]
		public Catalog Catalog { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000028AD File Offset: 0x00000AAD
		[NotNull]
		public CrossReferenceTable CrossReferenceTable { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000028B5 File Offset: 0x00000AB5
		internal IPdfTokenScanner TokenScanner { get; }

		// Token: 0x06000037 RID: 55 RVA: 0x000028C0 File Offset: 0x00000AC0
		internal Structure(Catalog catalog, CrossReferenceTable crossReferenceTable, IPdfTokenScanner scanner)
		{
			if (catalog == null)
			{
				throw new ArgumentNullException("catalog");
			}
			this.Catalog = catalog;
			if (crossReferenceTable == null)
			{
				throw new ArgumentNullException("crossReferenceTable");
			}
			this.CrossReferenceTable = crossReferenceTable;
			if (scanner == null)
			{
				throw new ArgumentNullException("scanner");
			}
			this.TokenScanner = scanner;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002918 File Offset: 0x00000B18
		public ObjectToken GetObject(IndirectReference reference)
		{
			ObjectToken objectToken = this.TokenScanner.Get(reference);
			if (objectToken == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Could not find the object with reference: ");
				defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(reference);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return objectToken;
		}
	}
}
