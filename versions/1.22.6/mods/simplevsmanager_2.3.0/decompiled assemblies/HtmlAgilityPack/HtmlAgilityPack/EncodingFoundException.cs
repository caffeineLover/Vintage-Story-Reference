using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace HtmlAgilityPack
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	internal class EncodingFoundException : Exception
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000021B3 File Offset: 0x000003B3
		internal EncodingFoundException(Encoding encoding)
		{
			this._encoding = encoding;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000021C2 File Offset: 0x000003C2
		internal Encoding Encoding
		{
			get
			{
				return this._encoding;
			}
		}

		// Token: 0x04000006 RID: 6
		private Encoding _encoding;
	}
}
