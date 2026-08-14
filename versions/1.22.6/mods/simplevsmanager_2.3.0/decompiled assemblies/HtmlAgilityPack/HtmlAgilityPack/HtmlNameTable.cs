using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace HtmlAgilityPack
{
	// Token: 0x02000011 RID: 17
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class HtmlNameTable : XmlNameTable
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x00008B54 File Offset: 0x00006D54
		public override string Add(string array)
		{
			return this._nametable.Add(array);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00008B62 File Offset: 0x00006D62
		public override string Add(char[] array, int offset, int length)
		{
			return this._nametable.Add(array, offset, length);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00008B72 File Offset: 0x00006D72
		public override string Get(string array)
		{
			return this._nametable.Get(array);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00008B80 File Offset: 0x00006D80
		public override string Get(char[] array, int offset, int length)
		{
			return this._nametable.Get(array, offset, length);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00008B90 File Offset: 0x00006D90
		internal string GetOrAdd(string array)
		{
			string text = this.Get(array);
			if (text == null)
			{
				return this.Add(array);
			}
			return text;
		}

		// Token: 0x04000068 RID: 104
		private NameTable _nametable = new NameTable();
	}
}
