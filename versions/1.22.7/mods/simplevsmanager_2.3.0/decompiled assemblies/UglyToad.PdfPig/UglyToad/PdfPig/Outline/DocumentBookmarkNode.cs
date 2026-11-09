using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Outline.Destinations;

namespace UglyToad.PdfPig.Outline
{
	// Token: 0x020000DC RID: 220
	public class DocumentBookmarkNode : BookmarkNode
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0001A143 File Offset: 0x00018343
		public int PageNumber
		{
			get
			{
				return this.Destination.PageNumber;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x0001A150 File Offset: 0x00018350
		public ExplicitDestination Destination { get; }

		// Token: 0x0600041E RID: 1054 RVA: 0x0001A158 File Offset: 0x00018358
		public DocumentBookmarkNode(string title, int level, ExplicitDestination destination, IReadOnlyList<BookmarkNode> children) : base(title, level, children)
		{
			this.Destination = destination;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001A16C File Offset: 0x0001836C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(10, 3);
			defaultInterpolatedStringHandler.AppendLiteral("page #");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.PageNumber);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(base.Level);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted(base.Title);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
