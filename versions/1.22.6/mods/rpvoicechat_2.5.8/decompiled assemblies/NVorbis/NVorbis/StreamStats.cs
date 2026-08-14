using System;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x02000014 RID: 20
	internal class StreamStats : IStreamStats
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000057D4 File Offset: 0x000039D4
		public int EffectiveBitRate
		{
			get
			{
				object @lock = this._lock;
				long totalSamples;
				long num;
				lock (@lock)
				{
					totalSamples = this._totalSamples;
					num = this._audioBits + this._headerBits + this._containerBits + this._wasteBits;
				}
				if (totalSamples > 0L)
				{
					return (int)((double)num / (double)totalSamples * (double)this._sampleRate);
				}
				return 0;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00005848 File Offset: 0x00003A48
		public int InstantBitRate
		{
			get
			{
				object @lock = this._lock;
				int num;
				int num2;
				lock (@lock)
				{
					num = this._packetBits[0] + this._packetBits[1];
					num2 = this._packetSamples[0] + this._packetSamples[1];
				}
				if (num2 > 0)
				{
					return (int)((double)num / (double)num2 * (double)this._sampleRate);
				}
				return 0;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000058BC File Offset: 0x00003ABC
		public long ContainerBits
		{
			get
			{
				return this._containerBits;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000058C4 File Offset: 0x00003AC4
		public long OverheadBits
		{
			get
			{
				return this._headerBits;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000058CC File Offset: 0x00003ACC
		public long AudioBits
		{
			get
			{
				return this._audioBits;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000058D4 File Offset: 0x00003AD4
		public long WasteBits
		{
			get
			{
				return this._wasteBits;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000058DC File Offset: 0x00003ADC
		public int PacketCount
		{
			get
			{
				return this._packetCount;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000058E4 File Offset: 0x00003AE4
		public void ResetStats()
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._packetBits[0] = (this._packetBits[1] = 0);
				this._packetSamples[0] = (this._packetSamples[1] = 0);
				this._packetIndex = 0;
				this._packetCount = 0;
				this._audioBits = 0L;
				this._totalSamples = 0L;
				this._headerBits = 0L;
				this._containerBits = 0L;
				this._wasteBits = 0L;
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000597C File Offset: 0x00003B7C
		internal void SetSampleRate(int sampleRate)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._sampleRate = sampleRate;
				this.ResetStats();
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000059C4 File Offset: 0x00003BC4
		internal void AddPacket(int samples, int bits, int waste, int container)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				if (samples >= 0)
				{
					this._audioBits += (long)bits;
					this._wasteBits += (long)waste;
					this._containerBits += (long)container;
					this._totalSamples += (long)samples;
					this._packetBits[this._packetIndex] = bits + waste;
					this._packetSamples[this._packetIndex] = samples;
					int num = this._packetIndex + 1;
					this._packetIndex = num;
					if (num == 2)
					{
						this._packetIndex = 0;
					}
				}
				else
				{
					this._headerBits += (long)bits;
					this._wasteBits += (long)waste;
					this._containerBits += (long)container;
				}
			}
		}

		// Token: 0x0400006A RID: 106
		private int _sampleRate;

		// Token: 0x0400006B RID: 107
		private readonly int[] _packetBits = new int[2];

		// Token: 0x0400006C RID: 108
		private readonly int[] _packetSamples = new int[2];

		// Token: 0x0400006D RID: 109
		private int _packetIndex;

		// Token: 0x0400006E RID: 110
		private long _totalSamples;

		// Token: 0x0400006F RID: 111
		private long _audioBits;

		// Token: 0x04000070 RID: 112
		private long _headerBits;

		// Token: 0x04000071 RID: 113
		private long _containerBits;

		// Token: 0x04000072 RID: 114
		private long _wasteBits;

		// Token: 0x04000073 RID: 115
		private object _lock = new object();

		// Token: 0x04000074 RID: 116
		private int _packetCount;
	}
}
