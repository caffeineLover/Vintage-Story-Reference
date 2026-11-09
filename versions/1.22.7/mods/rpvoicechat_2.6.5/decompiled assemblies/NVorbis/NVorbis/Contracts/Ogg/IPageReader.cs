using System;

namespace NVorbis.Contracts.Ogg
{
	// Token: 0x02000039 RID: 57
	internal interface IPageReader : IDisposable
	{
		// Token: 0x0600023C RID: 572
		void Lock();

		// Token: 0x0600023D RID: 573
		bool Release();

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600023E RID: 574
		long ContainerBits { get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600023F RID: 575
		long WasteBits { get; }

		// Token: 0x06000240 RID: 576
		bool ReadNextPage();

		// Token: 0x06000241 RID: 577
		bool ReadPageAt(long offset);
	}
}
