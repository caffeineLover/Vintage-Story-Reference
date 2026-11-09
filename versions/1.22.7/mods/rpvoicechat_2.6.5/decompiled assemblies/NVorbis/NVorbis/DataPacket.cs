using System;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x02000003 RID: 3
	public abstract class DataPacket : IPacket
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000266F File Offset: 0x0000086F
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002677 File Offset: 0x00000877
		public int ContainerOverheadBits { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002680 File Offset: 0x00000880
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002688 File Offset: 0x00000888
		public long? GranulePosition { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002691 File Offset: 0x00000891
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000269A File Offset: 0x0000089A
		public bool IsResync
		{
			get
			{
				return this.GetFlag(DataPacket.PacketFlags.IsResync);
			}
			set
			{
				this.SetFlag(DataPacket.PacketFlags.IsResync, value);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000026A4 File Offset: 0x000008A4
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000026AD File Offset: 0x000008AD
		public bool IsShort
		{
			get
			{
				return this.GetFlag(DataPacket.PacketFlags.IsShort);
			}
			private set
			{
				this.SetFlag(DataPacket.PacketFlags.IsShort, value);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000026B7 File Offset: 0x000008B7
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000026C0 File Offset: 0x000008C0
		public bool IsEndOfStream
		{
			get
			{
				return this.GetFlag(DataPacket.PacketFlags.IsEndOfStream);
			}
			set
			{
				this.SetFlag(DataPacket.PacketFlags.IsEndOfStream, value);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000026CA File Offset: 0x000008CA
		public int BitsRead
		{
			get
			{
				return this._readBits;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000026D2 File Offset: 0x000008D2
		public int BitsRemaining
		{
			get
			{
				return this.TotalBits - this._readBits;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001C RID: 28
		protected abstract int TotalBits { get; }

		// Token: 0x0600001D RID: 29 RVA: 0x000026E1 File Offset: 0x000008E1
		private bool GetFlag(DataPacket.PacketFlags flag)
		{
			return this._packetFlags.HasFlag(flag);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026F9 File Offset: 0x000008F9
		private void SetFlag(DataPacket.PacketFlags flag, bool value)
		{
			if (value)
			{
				this._packetFlags |= flag;
				return;
			}
			this._packetFlags &= ~flag;
		}

		// Token: 0x0600001F RID: 31
		protected abstract int ReadNextByte();

		// Token: 0x06000020 RID: 32 RVA: 0x0000271D File Offset: 0x0000091D
		public virtual void Done()
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000271F File Offset: 0x0000091F
		public virtual void Reset()
		{
			this._bitBucket = 0UL;
			this._bitCount = 0;
			this._overflowBits = 0;
			this._readBits = 0;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002740 File Offset: 0x00000940
		ulong IPacket.ReadBits(int count)
		{
			if (count == 0)
			{
				return 0UL;
			}
			int num;
			ulong result = this.TryPeekBits(count, out num);
			this.SkipBits(count);
			return result;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002764 File Offset: 0x00000964
		public ulong TryPeekBits(int count, out int bitsRead)
		{
			if (count < 0 || count > 64)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count == 0)
			{
				bitsRead = 0;
				return 0UL;
			}
			while (this._bitCount < count)
			{
				int num = this.ReadNextByte();
				if (num == -1)
				{
					bitsRead = this._bitCount;
					return this._bitBucket;
				}
				this._bitBucket = (ulong)((long)(num & 255) << this._bitCount | (long)this._bitBucket);
				this._bitCount += 8;
				if (this._bitCount > 64)
				{
					this._overflowBits = (byte)(num >> 72 - this._bitCount);
				}
			}
			ulong num2 = this._bitBucket;
			if (count < 64)
			{
				num2 &= (1UL << count) - 1UL;
			}
			bitsRead = count;
			return num2;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000281C File Offset: 0x00000A1C
		public void SkipBits(int count)
		{
			if (count > 0)
			{
				if (this._bitCount > count)
				{
					if (count > 63)
					{
						this._bitBucket = 0UL;
					}
					else
					{
						this._bitBucket >>= count;
					}
					if (this._bitCount > 64)
					{
						int num = this._bitCount - 64;
						this._bitBucket |= (ulong)this._overflowBits << this._bitCount - count - num;
						if (num > count)
						{
							this._overflowBits = (byte)(this._overflowBits >> count);
						}
					}
					this._bitCount -= count;
					this._readBits += count;
					return;
				}
				if (this._bitCount == count)
				{
					this._bitBucket = 0UL;
					this._bitCount = 0;
					this._readBits += count;
					return;
				}
				count -= this._bitCount;
				this._readBits += this._bitCount;
				this._bitCount = 0;
				this._bitBucket = 0UL;
				while (count > 8)
				{
					if (this.ReadNextByte() == -1)
					{
						count = 0;
						this.IsShort = true;
						break;
					}
					count -= 8;
					this._readBits += 8;
				}
				if (count > 0)
				{
					int num2 = this.ReadNextByte();
					if (num2 == -1)
					{
						this.IsShort = true;
						return;
					}
					this._bitBucket = (ulong)((long)(num2 >> count));
					this._bitCount = 8 - count;
					this._readBits += count;
				}
			}
		}

		// Token: 0x0400000A RID: 10
		private ulong _bitBucket;

		// Token: 0x0400000B RID: 11
		private int _bitCount;

		// Token: 0x0400000C RID: 12
		private byte _overflowBits;

		// Token: 0x0400000D RID: 13
		private DataPacket.PacketFlags _packetFlags;

		// Token: 0x0400000E RID: 14
		private int _readBits;

		// Token: 0x0200003E RID: 62
		[Flags]
		protected enum PacketFlags : byte
		{
			// Token: 0x040000E5 RID: 229
			IsResync = 1,
			// Token: 0x040000E6 RID: 230
			IsEndOfStream = 2,
			// Token: 0x040000E7 RID: 231
			IsShort = 4,
			// Token: 0x040000E8 RID: 232
			User0 = 8,
			// Token: 0x040000E9 RID: 233
			User1 = 16,
			// Token: 0x040000EA RID: 234
			User2 = 32,
			// Token: 0x040000EB RID: 235
			User3 = 64,
			// Token: 0x040000EC RID: 236
			User4 = 128
		}
	}
}
