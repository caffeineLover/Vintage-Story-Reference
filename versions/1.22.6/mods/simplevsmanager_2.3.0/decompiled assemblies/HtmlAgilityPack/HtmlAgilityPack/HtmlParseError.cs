using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200001F RID: 31
	[NullableContext(1)]
	[Nullable(0)]
	public class HtmlParseError
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x0000D620 File Offset: 0x0000B820
		internal HtmlParseError(HtmlParseErrorCode code, int line, int linePosition, int streamPosition, string sourceText, string reason)
		{
			this._code = code;
			this._line = line;
			this._linePosition = linePosition;
			this._streamPosition = streamPosition;
			this._sourceText = sourceText;
			this._reason = reason;
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000D655 File Offset: 0x0000B855
		public HtmlParseErrorCode Code
		{
			get
			{
				return this._code;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000D65D File Offset: 0x0000B85D
		public int Line
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000D665 File Offset: 0x0000B865
		public int LinePosition
		{
			get
			{
				return this._linePosition;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000D66D File Offset: 0x0000B86D
		public string Reason
		{
			get
			{
				return this._reason;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000D675 File Offset: 0x0000B875
		public string SourceText
		{
			get
			{
				return this._sourceText;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000D67D File Offset: 0x0000B87D
		public int StreamPosition
		{
			get
			{
				return this._streamPosition;
			}
		}

		// Token: 0x0400009F RID: 159
		private HtmlParseErrorCode _code;

		// Token: 0x040000A0 RID: 160
		private int _line;

		// Token: 0x040000A1 RID: 161
		private int _linePosition;

		// Token: 0x040000A2 RID: 162
		private string _reason;

		// Token: 0x040000A3 RID: 163
		private string _sourceText;

		// Token: 0x040000A4 RID: 164
		private int _streamPosition;
	}
}
