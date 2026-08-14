using System;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x02000011 RID: 17
	internal class Residue1 : Residue0
	{
		// Token: 0x0600007A RID: 122 RVA: 0x0000484C File Offset: 0x00002A4C
		protected override bool WriteVectors(ICodebook codebook, IPacket packet, float[][] residue, int channel, int offset, int partitionSize)
		{
			float[] array = residue[channel];
			int i = 0;
			while (i < partitionSize)
			{
				int num = codebook.DecodeScalar(packet);
				if (num == -1)
				{
					return true;
				}
				for (int j = 0; j < codebook.Dimensions; j++)
				{
					array[offset + i] += codebook[num, j];
					i++;
				}
			}
			return false;
		}
	}
}
