using System;

namespace NVorbis.Contracts
{
	// Token: 0x02000031 RID: 49
	public interface IStreamStats
	{
		// Token: 0x060001F1 RID: 497
		void ResetStats();

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001F2 RID: 498
		int EffectiveBitRate { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001F3 RID: 499
		int InstantBitRate { get; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001F4 RID: 500
		long ContainerBits { get; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001F5 RID: 501
		long OverheadBits { get; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001F6 RID: 502
		long AudioBits { get; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001F7 RID: 503
		long WasteBits { get; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001F8 RID: 504
		int PacketCount { get; }
	}
}
