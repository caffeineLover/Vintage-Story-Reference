using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Outline.Destinations;

namespace UglyToad.PdfPig.Outline
{
	// Token: 0x020000DE RID: 222
	public class ExternalBookmarkNode : DocumentBookmarkNode
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0001A26D File Offset: 0x0001846D
		public string FileName { get; }

		// Token: 0x06000424 RID: 1060 RVA: 0x0001A275 File Offset: 0x00018475
		public ExternalBookmarkNode(string title, int level, ExplicitDestination destination, IReadOnlyList<BookmarkNode> children, string fileName) : base(title, level, destination, children)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this.FileName = fileName;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001A29C File Offset: 0x0001849C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(11, 3);
			defaultInterpolatedStringHandler.AppendLiteral("file '");
			defaultInterpolatedStringHandler.AppendFormatted(this.FileName);
			defaultInterpolatedStringHandler.AppendLiteral("', ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(base.Level);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted(base.Title);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
