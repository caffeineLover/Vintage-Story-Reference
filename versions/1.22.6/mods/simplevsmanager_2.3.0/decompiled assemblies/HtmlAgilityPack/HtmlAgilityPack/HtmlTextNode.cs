using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000021 RID: 33
	[NullableContext(1)]
	[Nullable(0)]
	public class HtmlTextNode : HtmlNode
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000D685 File Offset: 0x0000B885
		internal HtmlTextNode(HtmlDocument ownerdocument, int index) : base(HtmlNodeType.Text, ownerdocument, index)
		{
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000D690 File Offset: 0x0000B890
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x0000D698 File Offset: 0x0000B898
		public override string InnerHtml
		{
			get
			{
				return this.OuterHtml;
			}
			set
			{
				this._text = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000D6A1 File Offset: 0x0000B8A1
		public override string OuterHtml
		{
			get
			{
				if (this._text == null)
				{
					return base.OuterHtml;
				}
				return this._text;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000D6B8 File Offset: 0x0000B8B8
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x0000D6CF File Offset: 0x0000B8CF
		public string Text
		{
			get
			{
				if (this._text == null)
				{
					return base.OuterHtml;
				}
				return this._text;
			}
			set
			{
				this._text = value;
				base.SetChanged();
			}
		}

		// Token: 0x040000AB RID: 171
		private string _text;
	}
}
