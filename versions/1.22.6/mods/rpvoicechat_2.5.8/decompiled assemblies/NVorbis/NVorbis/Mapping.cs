using System;
using System.IO;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x0200000C RID: 12
	internal class Mapping : IMapping
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00003B24 File Offset: 0x00001D24
		public void Init(IPacket packet, int channels, IFloor[] floors, IResidue[] residues, IMdct mdct)
		{
			int num = 1;
			if (packet.ReadBit())
			{
				num += (int)packet.ReadBits(4);
			}
			int num2 = 0;
			if (packet.ReadBit())
			{
				num2 = (int)packet.ReadBits(8) + 1;
			}
			int count = Utils.ilog(channels - 1);
			this._couplingAngle = new int[num2];
			this._couplingMangitude = new int[num2];
			for (int i = 0; i < num2; i++)
			{
				int num3 = (int)packet.ReadBits(count);
				int num4 = (int)packet.ReadBits(count);
				if (num3 == num4 || num3 > channels - 1 || num4 > channels - 1)
				{
					throw new InvalidDataException("Invalid magnitude or angle in mapping header!");
				}
				this._couplingAngle[i] = num4;
				this._couplingMangitude[i] = num3;
			}
			if (packet.ReadBits(2) != 0UL)
			{
				throw new InvalidDataException("Reserved bits not 0 in mapping header.");
			}
			int[] array = new int[channels];
			if (num > 1)
			{
				for (int j = 0; j < channels; j++)
				{
					array[j] = (int)packet.ReadBits(4);
					if (array[j] > num)
					{
						throw new InvalidDataException("Invalid channel mux submap index in mapping header!");
					}
				}
			}
			this._submapFloor = new IFloor[num];
			this._submapResidue = new IResidue[num];
			for (int k = 0; k < num; k++)
			{
				packet.SkipBits(8);
				int num5 = (int)packet.ReadBits(8);
				if (num5 >= floors.Length)
				{
					throw new InvalidDataException("Invalid floor number in mapping header!");
				}
				int num6 = (int)packet.ReadBits(8);
				if (num6 >= residues.Length)
				{
					throw new InvalidDataException("Invalid residue number in mapping header!");
				}
				this._submapFloor[k] = floors[num5];
				this._submapResidue[k] = residues[num6];
			}
			this._channelFloor = new IFloor[channels];
			this._channelResidue = new IResidue[channels];
			for (int l = 0; l < channels; l++)
			{
				this._channelFloor[l] = this._submapFloor[array[l]];
				this._channelResidue[l] = this._submapResidue[array[l]];
			}
			this._mdct = mdct;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003D00 File Offset: 0x00001F00
		public void DecodePacket(IPacket packet, int blockSize, int channels, float[][] buffer)
		{
			int num = blockSize >> 1;
			IFloorData[] array = new IFloorData[this._channelFloor.Length];
			bool[] array2 = new bool[this._channelFloor.Length];
			for (int i = 0; i < this._channelFloor.Length; i++)
			{
				array[i] = this._channelFloor[i].Unpack(packet, blockSize, i);
				array2[i] = !array[i].ExecuteChannel;
				Array.Clear(buffer[i], 0, num);
			}
			for (int j = 0; j < this._couplingAngle.Length; j++)
			{
				if (array[this._couplingAngle[j]].ExecuteChannel || array[this._couplingMangitude[j]].ExecuteChannel)
				{
					array[this._couplingAngle[j]].ForceEnergy = true;
					array[this._couplingMangitude[j]].ForceEnergy = true;
				}
			}
			for (int k = 0; k < this._submapFloor.Length; k++)
			{
				for (int l = 0; l < this._channelFloor.Length; l++)
				{
					if (this._submapFloor[k] != this._channelFloor[l] || this._submapResidue[k] != this._channelResidue[l])
					{
						array[l].ForceNoEnergy = true;
					}
				}
				this._submapResidue[k].Decode(packet, array2, blockSize, buffer);
			}
			for (int m = this._couplingAngle.Length - 1; m >= 0; m--)
			{
				if (array[this._couplingAngle[m]].ExecuteChannel || array[this._couplingMangitude[m]].ExecuteChannel)
				{
					float[] array3 = buffer[this._couplingMangitude[m]];
					float[] array4 = buffer[this._couplingAngle[m]];
					for (int n = 0; n < num; n++)
					{
						float num2 = array3[n];
						float num3 = array4[n];
						float num4;
						float num5;
						if (num2 > 0f)
						{
							if (num3 > 0f)
							{
								num4 = num2;
								num5 = num2 - num3;
							}
							else
							{
								num5 = num2;
								num4 = num2 + num3;
							}
						}
						else if (num3 > 0f)
						{
							num4 = num2;
							num5 = num2 + num3;
						}
						else
						{
							num5 = num2;
							num4 = num2 - num3;
						}
						array3[n] = num4;
						array4[n] = num5;
					}
				}
			}
			for (int num6 = 0; num6 < this._channelFloor.Length; num6++)
			{
				if (array[num6].ExecuteChannel)
				{
					this._channelFloor[num6].Apply(array[num6], blockSize, buffer[num6]);
					this._mdct.Reverse(buffer[num6], blockSize);
				}
				else
				{
					Array.Clear(buffer[num6], num, num);
				}
			}
		}

		// Token: 0x04000030 RID: 48
		private IMdct _mdct;

		// Token: 0x04000031 RID: 49
		private int[] _couplingAngle;

		// Token: 0x04000032 RID: 50
		private int[] _couplingMangitude;

		// Token: 0x04000033 RID: 51
		private IFloor[] _submapFloor;

		// Token: 0x04000034 RID: 52
		private IResidue[] _submapResidue;

		// Token: 0x04000035 RID: 53
		private IFloor[] _channelFloor;

		// Token: 0x04000036 RID: 54
		private IResidue[] _channelResidue;
	}
}
