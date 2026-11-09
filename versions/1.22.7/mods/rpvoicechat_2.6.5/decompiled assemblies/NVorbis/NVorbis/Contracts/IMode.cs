using System;

namespace NVorbis.Contracts
{
	// Token: 0x0200002B RID: 43
	internal interface IMode
	{
		// Token: 0x060001C2 RID: 450
		void Init(IPacket packet, int channels, int block0Size, int block1Size, IMapping[] mappings);

		// Token: 0x060001C3 RID: 451
		bool Decode(IPacket packet, float[][] buffer, out int packetStartindex, out int packetValidLength, out int packetTotalLength);

		// Token: 0x060001C4 RID: 452
		int GetPacketSampleCount(IPacket packet);
	}
}
