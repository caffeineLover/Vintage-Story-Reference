using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NVorbis.Contracts;
using NVorbis.Contracts.Ogg;

namespace NVorbis.Ogg
{
	// Token: 0x0200001A RID: 26
	internal class ForwardOnlyPacketProvider : DataPacket, IForwardOnlyPacketProvider, NVorbis.Contracts.IPacketProvider
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00006675 File Offset: 0x00004875
		public ForwardOnlyPacketProvider(IPageReader reader, int streamSerial)
		{
			this._reader = reader;
			this.StreamSerial = streamSerial;
			this._packetIndex = int.MaxValue;
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000066A1 File Offset: 0x000048A1
		public bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000066A4 File Offset: 0x000048A4
		public int StreamSerial { get; }

		// Token: 0x0600011C RID: 284 RVA: 0x000066AC File Offset: 0x000048AC
		public bool AddPage(byte[] buf, bool isResync)
		{
			if ((buf[5] & 2) != 0)
			{
				if (this._isEndOfStream)
				{
					return false;
				}
				isResync = true;
				this._lastSeqNo = BitConverter.ToInt32(buf, 18);
			}
			else
			{
				int num = BitConverter.ToInt32(buf, 18);
				isResync |= (num != this._lastSeqNo + 1);
				this._lastSeqNo = num;
			}
			int num2 = 0;
			for (int i = 0; i < (int)buf[26]; i++)
			{
				num2 += (int)buf[27 + i];
			}
			if (num2 == 0)
			{
				return false;
			}
			this._pageQueue.Enqueue(new ValueTuple<byte[], bool>(buf, isResync));
			return true;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006731 File Offset: 0x00004931
		public void SetEndOfStream()
		{
			this._isEndOfStream = true;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000673A File Offset: 0x0000493A
		public IPacket GetNextPacket()
		{
			if (this._packetBuf.Length > 0)
			{
				if (!this._lastWasPeek)
				{
					throw new InvalidOperationException("Must call Done() on previous packet first.");
				}
				this._lastWasPeek = false;
				return this;
			}
			else
			{
				this._lastWasPeek = false;
				if (this.GetPacket())
				{
					return this;
				}
				return null;
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00006778 File Offset: 0x00004978
		public IPacket PeekNextPacket()
		{
			if (this._packetBuf.Length > 0)
			{
				if (!this._lastWasPeek)
				{
					throw new InvalidOperationException("Must call Done() on previous packet first.");
				}
				return this;
			}
			else
			{
				this._lastWasPeek = true;
				if (this.GetPacket())
				{
					return this;
				}
				return null;
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000067B0 File Offset: 0x000049B0
		private bool GetPacket()
		{
			byte[] pageBuf;
			bool flag;
			int num;
			int packetIndex;
			bool flag2;
			bool flag3;
			if (this._pageBuf != null && this._packetIndex < (int)(27 + this._pageBuf[26]))
			{
				pageBuf = this._pageBuf;
				flag = false;
				num = this._dataStart;
				packetIndex = this._packetIndex;
				flag2 = false;
				flag3 = (pageBuf[(int)(26 + pageBuf[26])] == byte.MaxValue);
			}
			else if (!this.ReadNextPage(out pageBuf, out flag, out num, out packetIndex, out flag2, out flag3))
			{
				return false;
			}
			int num2 = num;
			bool flag4 = packetIndex == 27;
			if (flag2 && flag4)
			{
				flag = true;
				num2 += this.GetPacketLength(pageBuf, ref packetIndex);
				if (packetIndex == (int)(27 + pageBuf[26]))
				{
					return this.GetPacket();
				}
			}
			if (!flag4)
			{
				num2 = 0;
			}
			int packetLength = this.GetPacketLength(pageBuf, ref packetIndex);
			Memory<byte> memory = new Memory<byte>(pageBuf, num, packetLength);
			num += packetLength;
			bool flag5 = packetIndex == (int)(27 + pageBuf[26]);
			if (flag3)
			{
				if (flag5)
				{
					flag5 = false;
				}
				else
				{
					int num3 = packetIndex;
					this.GetPacketLength(pageBuf, ref num3);
					flag5 = (num3 == (int)(27 + pageBuf[26]));
				}
			}
			bool flag6 = false;
			long? granulePosition = default(long?);
			if (flag5)
			{
				granulePosition = new long?(BitConverter.ToInt64(pageBuf, 6));
				if ((pageBuf[5] & 4) != 0 || (this._isEndOfStream && this._pageQueue.Count == 0))
				{
					flag6 = true;
				}
			}
			else
			{
				while (flag3 && packetIndex == (int)(27 + pageBuf[26]) && (this.ReadNextPage(out pageBuf, out flag, out num, out packetIndex, out flag2, out flag3) && !flag && flag2))
				{
					num2 += (int)(27 + pageBuf[26]);
					Memory<byte> memory2 = memory;
					int packetLength2 = this.GetPacketLength(pageBuf, ref packetIndex);
					memory = new Memory<byte>(new byte[memory2.Length + packetLength2]);
					memory2.CopyTo(memory);
					new Memory<byte>(pageBuf, num, packetLength2).CopyTo(memory.Slice(memory2.Length));
					num += packetLength2;
				}
			}
			base.IsResync = flag;
			base.GranulePosition = granulePosition;
			base.IsEndOfStream = flag6;
			base.ContainerOverheadBits = num2 * 8;
			this._pageBuf = pageBuf;
			this._dataStart = num;
			this._packetIndex = packetIndex;
			this._packetBuf = memory;
			this._isEndOfStream = (this._isEndOfStream || flag6);
			this.Reset();
			return true;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000069D4 File Offset: 0x00004BD4
		private bool ReadNextPage(out byte[] pageBuf, out bool isResync, out int dataStart, out int packetIndex, out bool isContinuation, out bool isContinued)
		{
			while (this._pageQueue.Count == 0)
			{
				if (this._isEndOfStream || !this._reader.ReadNextPage())
				{
					pageBuf = null;
					isResync = false;
					dataStart = 0;
					packetIndex = 0;
					isContinuation = false;
					isContinued = false;
					return false;
				}
			}
			ValueTuple<byte[], bool> valueTuple = this._pageQueue.Dequeue();
			pageBuf = valueTuple.Item1;
			isResync = valueTuple.Item2;
			dataStart = (int)(pageBuf[26] + 27);
			packetIndex = 27;
			isContinuation = ((pageBuf[5] & 1) > 0);
			isContinued = (pageBuf[(int)(26 + pageBuf[26])] == byte.MaxValue);
			return true;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006A6C File Offset: 0x00004C6C
		private int GetPacketLength(byte[] pageBuf, ref int packetIndex)
		{
			int num = 0;
			while (pageBuf[packetIndex] == 255 && packetIndex < (int)(pageBuf[26] + 27))
			{
				num += (int)pageBuf[packetIndex];
				packetIndex++;
			}
			if (packetIndex < (int)(pageBuf[26] + 27))
			{
				num += (int)pageBuf[packetIndex];
				packetIndex++;
			}
			return num;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00006AB9 File Offset: 0x00004CB9
		protected override int TotalBits
		{
			get
			{
				return this._packetBuf.Length * 8;
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006AC8 File Offset: 0x00004CC8
		protected unsafe override int ReadNextByte()
		{
			if (this._dataIndex < this._packetBuf.Length)
			{
				Span<byte> span = this._packetBuf.Span;
				int dataIndex = this._dataIndex;
				this._dataIndex = dataIndex + 1;
				return (int)(*span[dataIndex]);
			}
			return -1;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006B0F File Offset: 0x00004D0F
		public override void Reset()
		{
			this._dataIndex = 0;
			base.Reset();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006B1E File Offset: 0x00004D1E
		public override void Done()
		{
			this._packetBuf = Memory<byte>.Empty;
			base.Done();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006B31 File Offset: 0x00004D31
		long NVorbis.Contracts.IPacketProvider.GetGranuleCount()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006B38 File Offset: 0x00004D38
		long NVorbis.Contracts.IPacketProvider.SeekTo(long granulePos, int preRoll, GetPacketGranuleCount getPacketGranuleCount)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000089 RID: 137
		private int _lastSeqNo;

		// Token: 0x0400008A RID: 138
		[TupleElementNames(new string[]
		{
			"buf",
			"isResync"
		})]
		private readonly Queue<ValueTuple<byte[], bool>> _pageQueue = new Queue<ValueTuple<byte[], bool>>();

		// Token: 0x0400008B RID: 139
		private readonly IPageReader _reader;

		// Token: 0x0400008C RID: 140
		private byte[] _pageBuf;

		// Token: 0x0400008D RID: 141
		private int _packetIndex;

		// Token: 0x0400008E RID: 142
		private bool _isEndOfStream;

		// Token: 0x0400008F RID: 143
		private int _dataStart;

		// Token: 0x04000090 RID: 144
		private bool _lastWasPeek;

		// Token: 0x04000091 RID: 145
		private Memory<byte> _packetBuf;

		// Token: 0x04000092 RID: 146
		private int _dataIndex;
	}
}
