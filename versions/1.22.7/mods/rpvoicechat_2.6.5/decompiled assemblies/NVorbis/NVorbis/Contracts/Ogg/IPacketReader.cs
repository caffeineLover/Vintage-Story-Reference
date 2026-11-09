using System;

namespace NVorbis.Contracts.Ogg
{
	// Token: 0x02000037 RID: 55
	internal interface IPacketReader
	{
		// Token: 0x06000230 RID: 560
		Memory<byte> GetPacketData(int pagePacketIndex);

		// Token: 0x06000231 RID: 561
		void InvalidatePacketCache(IPacket packet);
	}
}
