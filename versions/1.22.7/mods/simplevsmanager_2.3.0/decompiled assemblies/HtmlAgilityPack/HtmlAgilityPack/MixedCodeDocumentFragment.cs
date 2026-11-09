using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000028 RID: 40
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class MixedCodeDocumentFragment
	{
		// Token: 0x0600025E RID: 606 RVA: 0x00011924 File Offset: 0x0000FB24
		internal MixedCodeDocumentFragment(MixedCodeDocument doc, MixedCodeDocumentFragmentType type)
		{
			this.Doc = doc;
			this._type = type;
			if (type != MixedCodeDocumentFragmentType.Code)
			{
				if (type == MixedCodeDocumentFragmentType.Text)
				{
					this.Doc._textfragments.Append(this);
				}
			}
			else
			{
				this.Doc._codefragments.Append(this);
			}
			this.Doc._fragments.Append(this);
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600025F RID: 607 RVA: 0x00011981 File Offset: 0x0000FB81
		// (set) Token: 0x06000260 RID: 608 RVA: 0x000119B3 File Offset: 0x0000FBB3
		public string FragmentText
		{
			get
			{
				if (this._fragmentText == null)
				{
					this._fragmentText = this.Doc._text.Substring(this.Index, this.Length);
				}
				return this._fragmentText;
			}
			internal set
			{
				this._fragmentText = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000261 RID: 609 RVA: 0x000119BC File Offset: 0x0000FBBC
		public MixedCodeDocumentFragmentType FragmentType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000262 RID: 610 RVA: 0x000119C4 File Offset: 0x0000FBC4
		// (set) Token: 0x06000263 RID: 611 RVA: 0x000119CC File Offset: 0x0000FBCC
		public int Line
		{
			get
			{
				return this._line;
			}
			internal set
			{
				this._line = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000264 RID: 612 RVA: 0x000119D5 File Offset: 0x0000FBD5
		public int LinePosition
		{
			get
			{
				return this._lineposition;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000265 RID: 613 RVA: 0x000119DD File Offset: 0x0000FBDD
		public int StreamPosition
		{
			get
			{
				return this.Index;
			}
		}

		// Token: 0x040000D4 RID: 212
		internal MixedCodeDocument Doc;

		// Token: 0x040000D5 RID: 213
		private string _fragmentText;

		// Token: 0x040000D6 RID: 214
		internal int Index;

		// Token: 0x040000D7 RID: 215
		internal int Length;

		// Token: 0x040000D8 RID: 216
		private int _line;

		// Token: 0x040000D9 RID: 217
		internal int _lineposition;

		// Token: 0x040000DA RID: 218
		internal MixedCodeDocumentFragmentType _type;
	}
}
