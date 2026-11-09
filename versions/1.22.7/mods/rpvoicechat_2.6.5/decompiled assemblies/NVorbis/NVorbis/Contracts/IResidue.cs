using System;

namespace NVorbis.Contracts
{
	// Token: 0x0200002F RID: 47
	internal interface IResidue
	{
		// Token: 0x060001DB RID: 475
		void Init(IPacket packet, int channels, ICodebook[] codebooks);

		// Token: 0x060001DC RID: 476
		void Decode(IPacket packet, bool[] doNotDecodeChannel, int blockSize, float[][] buffer);
	}
}
