using System;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x02000012 RID: 18
	internal class Residue2 : Residue0
	{
		// Token: 0x0600007C RID: 124 RVA: 0x000048A9 File Offset: 0x00002AA9
		public override void Init(IPacket packet, int channels, ICodebook[] codebooks)
		{
			this._channels = channels;
			base.Init(packet, 1, codebooks);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000048BB File Offset: 0x00002ABB
		public override void Decode(IPacket packet, bool[] doNotDecodeChannel, int blockSize, float[][] buffer)
		{
			base.Decode(packet, doNotDecodeChannel, blockSize * this._channels, buffer);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000048D0 File Offset: 0x00002AD0
		protected override bool WriteVectors(ICodebook codebook, IPacket packet, float[][] residue, int channel, int offset, int partitionSize)
		{
			int num = 0;
			offset /= this._channels;
			int i = 0;
			while (i < partitionSize)
			{
				int num2 = codebook.DecodeScalar(packet);
				if (num2 == -1)
				{
					return true;
				}
				int j = 0;
				while (j < codebook.Dimensions)
				{
					residue[num][offset] += codebook[num2, j];
					if (++num == this._channels)
					{
						num = 0;
						offset++;
					}
					j++;
					i++;
				}
			}
			return false;
		}

		// Token: 0x0400004C RID: 76
		private int _channels;
	}
}
