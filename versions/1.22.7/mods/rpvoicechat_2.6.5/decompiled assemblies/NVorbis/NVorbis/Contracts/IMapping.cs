using System;

namespace NVorbis.Contracts
{
	// Token: 0x02000029 RID: 41
	internal interface IMapping
	{
		// Token: 0x060001BF RID: 447
		void Init(IPacket packet, int channels, IFloor[] floors, IResidue[] residues, IMdct mdct);

		// Token: 0x060001C0 RID: 448
		void DecodePacket(IPacket packet, int blockSize, int channels, float[][] buffer);
	}
}
