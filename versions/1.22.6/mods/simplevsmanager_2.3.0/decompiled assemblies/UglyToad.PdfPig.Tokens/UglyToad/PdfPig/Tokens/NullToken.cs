using System;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x0200000D RID: 13
	public class NullToken : IDataToken<object>, IToken, IEquatable<IToken>
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00004B47 File Offset: 0x00002D47
		public static NullToken Instance { get; } = new NullToken();

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00004B4E File Offset: 0x00002D4E
		public object Data { get; }

		// Token: 0x06000043 RID: 67 RVA: 0x00004B56 File Offset: 0x00002D56
		private NullToken()
		{
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004B5E File Offset: 0x00002D5E
		public override bool Equals(object obj)
		{
			return obj is NullToken;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004B69 File Offset: 0x00002D69
		protected bool Equals(NullToken other)
		{
			return object.Equals(this.Data, other.Data);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004B7C File Offset: 0x00002D7C
		public bool Equals(IToken obj)
		{
			return obj is NullToken;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00004B87 File Offset: 0x00002D87
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004B8A File Offset: 0x00002D8A
		public override string ToString()
		{
			return "null";
		}
	}
}
