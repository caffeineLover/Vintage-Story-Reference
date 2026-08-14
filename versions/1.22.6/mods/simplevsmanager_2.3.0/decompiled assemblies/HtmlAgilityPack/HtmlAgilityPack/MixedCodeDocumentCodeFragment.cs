using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000027 RID: 39
	[NullableContext(1)]
	[Nullable(0)]
	public class MixedCodeDocumentCodeFragment : MixedCodeDocumentFragment
	{
		// Token: 0x0600025B RID: 603 RVA: 0x0001185A File Offset: 0x0000FA5A
		internal MixedCodeDocumentCodeFragment(MixedCodeDocument doc) : base(doc, MixedCodeDocumentFragmentType.Code)
		{
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00011864 File Offset: 0x0000FA64
		// (set) Token: 0x0600025D RID: 605 RVA: 0x00011918 File Offset: 0x0000FB18
		public string Code
		{
			get
			{
				if (this._code == null)
				{
					this._code = base.FragmentText.Substring(this.Doc.TokenCodeStart.Length, base.FragmentText.Length - this.Doc.TokenCodeEnd.Length - this.Doc.TokenCodeStart.Length - 1).Trim();
					if (this._code.StartsWith("="))
					{
						this._code = this.Doc.TokenResponseWrite + this._code.Substring(1, this._code.Length - 1);
					}
				}
				return this._code;
			}
			set
			{
				this._code = value;
			}
		}

		// Token: 0x040000D3 RID: 211
		private string _code;
	}
}
