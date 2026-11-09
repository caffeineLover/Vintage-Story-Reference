using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	[Nullable(0)]
	public class HtmlCommentNode : HtmlNode
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00002E53 File Offset: 0x00001053
		internal HtmlCommentNode(HtmlDocument ownerdocument, int index) : base(HtmlNodeType.Comment, ownerdocument, index)
		{
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002E5E File Offset: 0x0000105E
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002E75 File Offset: 0x00001075
		public string Comment
		{
			get
			{
				if (this._comment == null)
				{
					return base.InnerHtml;
				}
				return this._comment;
			}
			set
			{
				this._comment = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002E7E File Offset: 0x0000107E
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002E95 File Offset: 0x00001095
		public override string InnerHtml
		{
			get
			{
				if (this._comment == null)
				{
					return base.InnerHtml;
				}
				return this._comment;
			}
			set
			{
				this._comment = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002EA0 File Offset: 0x000010A0
		public override string OuterHtml
		{
			get
			{
				if (this._comment == null)
				{
					return base.OuterHtml;
				}
				if (this._comment.StartsWith("<!--") && this._comment.EndsWith("-->"))
				{
					return this._comment;
				}
				return "<!--" + this._comment + "-->";
			}
		}

		// Token: 0x04000020 RID: 32
		private string _comment;
	}
}
