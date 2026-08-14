using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Outline.Destinations;

namespace UglyToad.PdfPig.Outline
{
	// Token: 0x020000DD RID: 221
	public class EmbeddedBookmarkNode : DocumentBookmarkNode
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0001A1D5 File Offset: 0x000183D5
		public string FileSpecification { get; }

		// Token: 0x06000421 RID: 1057 RVA: 0x0001A1DD File Offset: 0x000183DD
		public EmbeddedBookmarkNode(string title, int level, ExplicitDestination destination, IReadOnlyList<BookmarkNode> children, string fileSpecification) : base(title, level, destination, children)
		{
			if (fileSpecification == null)
			{
				throw new ArgumentNullException("fileSpecification");
			}
			this.FileSpecification = fileSpecification;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001A204 File Offset: 0x00018404
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(20, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Embedded file '");
			defaultInterpolatedStringHandler.AppendFormatted(this.FileSpecification);
			defaultInterpolatedStringHandler.AppendLiteral("', ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(base.Level);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted(base.Title);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
