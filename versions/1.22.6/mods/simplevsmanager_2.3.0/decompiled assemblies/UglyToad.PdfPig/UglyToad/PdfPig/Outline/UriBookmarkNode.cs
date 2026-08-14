using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Outline
{
	// Token: 0x020000DF RID: 223
	public class UriBookmarkNode : BookmarkNode
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0001A305 File Offset: 0x00018505
		public string Uri { get; }

		// Token: 0x06000427 RID: 1063 RVA: 0x0001A30D File Offset: 0x0001850D
		public UriBookmarkNode(string title, int level, string uri, IReadOnlyList<BookmarkNode> children) : base(title, level, children)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			this.Uri = uri;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001A330 File Offset: 0x00018530
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(10, 3);
			defaultInterpolatedStringHandler.AppendLiteral("URI '");
			defaultInterpolatedStringHandler.AppendFormatted(this.Uri);
			defaultInterpolatedStringHandler.AppendLiteral("', ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(base.Level);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted(base.Title);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
