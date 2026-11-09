using System;
using System.Collections.Generic;

namespace NVorbis.Contracts
{
	// Token: 0x02000024 RID: 36
	public interface IContainerReader : IDisposable
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001A4 RID: 420
		// (set) Token: 0x060001A5 RID: 421
		NewStreamHandler NewStreamCallback { get; set; }

		// Token: 0x060001A6 RID: 422
		IReadOnlyList<IPacketProvider> GetStreams();

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001A7 RID: 423
		bool CanSeek { get; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001A8 RID: 424
		long ContainerBits { get; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001A9 RID: 425
		long WasteBits { get; }

		// Token: 0x060001AA RID: 426
		bool TryInit();

		// Token: 0x060001AB RID: 427
		bool FindNextStream();
	}
}
