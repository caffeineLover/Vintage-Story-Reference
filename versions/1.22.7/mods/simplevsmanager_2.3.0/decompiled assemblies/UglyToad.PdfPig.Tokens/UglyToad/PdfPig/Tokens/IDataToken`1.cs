using System;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000008 RID: 8
	public interface IDataToken<out T> : IToken, IEquatable<IToken>
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002D RID: 45
		T Data { get; }
	}
}
