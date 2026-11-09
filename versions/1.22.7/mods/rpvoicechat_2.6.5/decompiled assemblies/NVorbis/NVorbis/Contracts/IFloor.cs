using System;

namespace NVorbis.Contracts
{
	// Token: 0x02000026 RID: 38
	internal interface IFloor
	{
		// Token: 0x060001B3 RID: 435
		void Init(IPacket packet, int channels, int block0Size, int block1Size, ICodebook[] codebooks);

		// Token: 0x060001B4 RID: 436
		IFloorData Unpack(IPacket packet, int blockSize, int channel);

		// Token: 0x060001B5 RID: 437
		void Apply(IFloorData floorData, int blockSize, float[] residue);
	}
}
