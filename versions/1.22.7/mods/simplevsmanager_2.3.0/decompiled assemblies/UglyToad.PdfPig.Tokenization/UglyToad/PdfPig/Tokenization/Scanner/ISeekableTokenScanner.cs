using System;

namespace UglyToad.PdfPig.Tokenization.Scanner
{
	// Token: 0x0200000F RID: 15
	public interface ISeekableTokenScanner : ITokenScanner
	{
		// Token: 0x0600003E RID: 62
		void Seek(long position);

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003F RID: 63
		long CurrentPosition { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000040 RID: 64
		long Length { get; }

		// Token: 0x06000041 RID: 65
		void RegisterCustomTokenizer(byte firstByte, ITokenizer tokenizer);

		// Token: 0x06000042 RID: 66
		void DeregisterCustomTokenizer(ITokenizer tokenizer);
	}
}
