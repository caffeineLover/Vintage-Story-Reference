using System;

namespace NVorbis.Contracts
{
	// Token: 0x0200002E RID: 46
	public interface IPacketProvider
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001D5 RID: 469
		bool CanSeek { get; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001D6 RID: 470
		int StreamSerial { get; }

		// Token: 0x060001D7 RID: 471
		IPacket GetNextPacket();

		// Token: 0x060001D8 RID: 472
		IPacket PeekNextPacket();

		// Token: 0x060001D9 RID: 473
		long SeekTo(long granulePos, int preRoll, GetPacketGranuleCount getPacketGranuleCount);

		// Token: 0x060001DA RID: 474
		long GetGranuleCount();
	}
}
