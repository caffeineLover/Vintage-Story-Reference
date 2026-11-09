using System;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000006 RID: 6
	public interface IInputBytes : IDisposable
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000010 RID: 16
		long CurrentOffset { get; }

		// Token: 0x06000011 RID: 17
		bool MoveNext();

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000012 RID: 18
		byte CurrentByte { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000013 RID: 19
		long Length { get; }

		// Token: 0x06000014 RID: 20
		byte? Peek();

		// Token: 0x06000015 RID: 21
		bool IsAtEnd();

		// Token: 0x06000016 RID: 22
		void Seek(long position);

		// Token: 0x06000017 RID: 23
		int Read(byte[] buffer, int? length = null);
	}
}
