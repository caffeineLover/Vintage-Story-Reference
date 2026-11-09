using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Parser.Parts.CrossReference
{
	// Token: 0x020000CD RID: 205
	internal class CrossReferenceStreamFieldSize
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003DD RID: 989 RVA: 0x000179E5 File Offset: 0x00015BE5
		public int Field1Size { get; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003DE RID: 990 RVA: 0x000179ED File Offset: 0x00015BED
		public int Field2Size { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003DF RID: 991 RVA: 0x000179F5 File Offset: 0x00015BF5
		public int Field3Size { get; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x000179FD File Offset: 0x00015BFD
		public int LineLength { get; }

		// Token: 0x060003E1 RID: 993 RVA: 0x00017A08 File Offset: 0x00015C08
		public CrossReferenceStreamFieldSize(DictionaryToken dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			IToken token;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (dictionary.TryGet(NameToken.W, ref token))
			{
				ArrayToken arrayToken = token as ArrayToken;
				if (arrayToken != null)
				{
					if (arrayToken.Data.Count < 3)
					{
						defaultInterpolatedStringHandler..ctor(72, 1);
						defaultInterpolatedStringHandler.AppendLiteral("There must be at least 3 entries in a W entry for a stream dictionary: ");
						defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(arrayToken);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					this.Field1Size = arrayToken.GetNumeric(0).Int;
					this.Field2Size = arrayToken.GetNumeric(1).Int;
					this.Field3Size = arrayToken.GetNumeric(2).Int;
					this.LineLength = this.Field1Size + this.Field2Size + this.Field3Size;
					return;
				}
			}
			defaultInterpolatedStringHandler..ctor(57, 1);
			defaultInterpolatedStringHandler.AppendLiteral("The W entry for the stream dictionary was not an array: ");
			defaultInterpolatedStringHandler.AppendFormatted<IToken>(token);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
