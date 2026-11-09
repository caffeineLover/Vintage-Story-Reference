using System;
using System.IO;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x0200000E RID: 14
	internal class Mode : IMode
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00003FC4 File Offset: 0x000021C4
		public void Init(IPacket packet, int channels, int block0Size, int block1Size, IMapping[] mappings)
		{
			this._channels = channels;
			this._blockFlag = packet.ReadBit();
			if (packet.ReadBits(32) != 0UL)
			{
				throw new InvalidDataException("Mode header had invalid window or transform type!");
			}
			int num = (int)packet.ReadBits(8);
			if (num >= mappings.Length)
			{
				throw new InvalidDataException("Mode header had invalid mapping index!");
			}
			this._mapping = mappings[num];
			if (this._blockFlag)
			{
				this._blockSize = block1Size;
				this._windows = new float[][]
				{
					Mode.CalcWindow(block0Size, block1Size, block0Size),
					Mode.CalcWindow(block1Size, block1Size, block0Size),
					Mode.CalcWindow(block0Size, block1Size, block1Size),
					Mode.CalcWindow(block1Size, block1Size, block1Size)
				};
				this._overlapInfo = new Mode.OverlapInfo[]
				{
					Mode.CalcOverlap(block0Size, block1Size, block0Size),
					Mode.CalcOverlap(block1Size, block1Size, block0Size),
					Mode.CalcOverlap(block0Size, block1Size, block1Size),
					Mode.CalcOverlap(block1Size, block1Size, block1Size)
				};
				return;
			}
			this._blockSize = block0Size;
			this._windows = new float[][]
			{
				Mode.CalcWindow(block0Size, block0Size, block0Size)
			};
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000040E0 File Offset: 0x000022E0
		private static float[] CalcWindow(int prevBlockSize, int blockSize, int nextBlockSize)
		{
			float[] array = new float[blockSize];
			int num = prevBlockSize / 2;
			int num2 = nextBlockSize / 2;
			int num3 = blockSize / 4 - num / 2;
			int num4 = blockSize - blockSize / 4 - num2 / 2;
			for (int i = 0; i < num; i++)
			{
				float num5 = (float)Math.Sin(((double)i + 0.5) / (double)num * 1.5707963705062866);
				num5 *= num5;
				array[num3 + i] = (float)Math.Sin((double)(num5 * 1.5707964f));
			}
			for (int j = num3 + num; j < num4; j++)
			{
				array[j] = 1f;
			}
			for (int k = 0; k < num2; k++)
			{
				float num6 = (float)Math.Sin(((double)(num2 - k) - 0.5) / (double)num2 * 1.5707963705062866);
				num6 *= num6;
				array[num4 + k] = (float)Math.Sin((double)(num6 * 1.5707964f));
			}
			return array;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000041CC File Offset: 0x000023CC
		private static Mode.OverlapInfo CalcOverlap(int prevBlockSize, int blockSize, int nextBlockSize)
		{
			int num = prevBlockSize / 4;
			int num2 = nextBlockSize / 4;
			int packetStartIndex = blockSize / 4 - num;
			int num3 = blockSize / 4 * 3 + num2;
			int packetValidLength = num3 - num2 * 2;
			return new Mode.OverlapInfo
			{
				PacketStartIndex = packetStartIndex,
				PacketValidLength = packetValidLength,
				PacketTotalLength = num3
			};
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000421C File Offset: 0x0000241C
		private bool GetPacketInfo(IPacket packet, out int windowIndex, out int packetStartIndex, out int packetValidLength, out int packetTotalLength)
		{
			if (packet.IsShort)
			{
				windowIndex = 0;
				packetStartIndex = 0;
				packetValidLength = 0;
				packetTotalLength = 0;
				return false;
			}
			if (this._blockFlag)
			{
				bool flag = packet.ReadBit();
				bool flag2 = packet.ReadBit();
				windowIndex = (flag ? 1 : 0) + (flag2 ? 2 : 0);
				Mode.OverlapInfo overlapInfo = this._overlapInfo[windowIndex];
				packetStartIndex = overlapInfo.PacketStartIndex;
				packetValidLength = overlapInfo.PacketValidLength;
				packetTotalLength = overlapInfo.PacketTotalLength;
			}
			else
			{
				windowIndex = 0;
				packetStartIndex = 0;
				packetValidLength = this._blockSize / 2;
				packetTotalLength = this._blockSize;
			}
			return true;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000042B0 File Offset: 0x000024B0
		public bool Decode(IPacket packet, float[][] buffer, out int packetStartindex, out int packetValidLength, out int packetTotalLength)
		{
			int num;
			if (this.GetPacketInfo(packet, out num, out packetStartindex, out packetValidLength, out packetTotalLength))
			{
				this._mapping.DecodePacket(packet, this._blockSize, this._channels, buffer);
				float[] array = this._windows[num];
				for (int i = 0; i < this._blockSize; i++)
				{
					for (int j = 0; j < this._channels; j++)
					{
						buffer[j][i] *= array[i];
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004324 File Offset: 0x00002524
		public int GetPacketSampleCount(IPacket packet)
		{
			int num;
			int num2;
			int num3;
			int num4;
			this.GetPacketInfo(packet, out num, out num2, out num3, out num4);
			return num3 - num2;
		}

		// Token: 0x04000039 RID: 57
		private const float M_PI2 = 1.5707964f;

		// Token: 0x0400003A RID: 58
		private int _channels;

		// Token: 0x0400003B RID: 59
		private bool _blockFlag;

		// Token: 0x0400003C RID: 60
		private int _blockSize;

		// Token: 0x0400003D RID: 61
		private IMapping _mapping;

		// Token: 0x0400003E RID: 62
		private float[][] _windows;

		// Token: 0x0400003F RID: 63
		private Mode.OverlapInfo[] _overlapInfo;

		// Token: 0x02000042 RID: 66
		private struct OverlapInfo
		{
			// Token: 0x040000FE RID: 254
			public int PacketStartIndex;

			// Token: 0x040000FF RID: 255
			public int PacketTotalLength;

			// Token: 0x04000100 RID: 256
			public int PacketValidLength;
		}
	}
}
