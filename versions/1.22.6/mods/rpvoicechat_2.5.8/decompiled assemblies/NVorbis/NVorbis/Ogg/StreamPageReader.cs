using System;
using System.Collections.Generic;
using System.IO;
using NVorbis.Contracts;
using NVorbis.Contracts.Ogg;

namespace NVorbis.Ogg
{
	// Token: 0x02000020 RID: 32
	internal class StreamPageReader : IStreamPageReader
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00007F6E File Offset: 0x0000616E
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00007F75 File Offset: 0x00006175
		internal static Func<IStreamPageReader, int, NVorbis.Contracts.IPacketProvider> CreatePacketProvider { get; set; } = (IStreamPageReader pr, int ss) => new PacketProvider(pr, ss);

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00007F7D File Offset: 0x0000617D
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00007F85 File Offset: 0x00006185
		public NVorbis.Contracts.IPacketProvider PacketProvider { get; private set; }

		// Token: 0x06000187 RID: 391 RVA: 0x00007F8E File Offset: 0x0000618E
		public StreamPageReader(IPageData pageReader, int streamSerial)
		{
			this._reader = pageReader;
			this.PacketProvider = StreamPageReader.CreatePacketProvider.Invoke(this, streamSerial);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007FC4 File Offset: 0x000061C4
		public void AddPage()
		{
			if (!this.HasAllPages)
			{
				if (this._reader.GranulePosition != -1L)
				{
					if (this._firstDataPageIndex == null && this._reader.GranulePosition > 0L)
					{
						this._firstDataPageIndex = new int?(this._pageOffsets.Count);
					}
					else if (this._maxGranulePos > this._reader.GranulePosition)
					{
						throw new InvalidDataException("Granule Position regressed?!");
					}
					this._maxGranulePos = this._reader.GranulePosition;
				}
				else if (this._firstDataPageIndex != null && (!this._reader.IsContinued || this._reader.PacketCount != 1))
				{
					throw new InvalidDataException("Granule Position was -1 but page does not have exactly 1 continued packet.");
				}
				if ((this._reader.PageFlags & PageFlags.EndOfStream) != PageFlags.None)
				{
					this.HasAllPages = true;
				}
				if (this._reader.IsResync.Value || (this._lastSeqNbr != 0 && this._lastSeqNbr + 1 != this._reader.SequenceNumber))
				{
					this._pageOffsets.Add(-this._reader.PageOffset);
				}
				else
				{
					this._pageOffsets.Add(this._reader.PageOffset);
				}
				this._lastSeqNbr = this._reader.SequenceNumber;
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000810C File Offset: 0x0000630C
		public Memory<byte>[] GetPagePackets(int pageIndex)
		{
			if (this._cachedPagePackets != null && this._lastPageIndex == pageIndex)
			{
				return this._cachedPagePackets;
			}
			long num = this._pageOffsets[pageIndex];
			if (num < 0L)
			{
				num = -num;
			}
			this._reader.Lock();
			Memory<byte>[] result;
			try
			{
				this._reader.ReadPageAt(num);
				Memory<byte>[] packets = this._reader.GetPackets();
				if (pageIndex == this._lastPageIndex)
				{
					this._cachedPagePackets = packets;
				}
				result = packets;
			}
			finally
			{
				this._reader.Release();
			}
			return result;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000819C File Offset: 0x0000639C
		public int FindPage(long granulePos)
		{
			int num = -1;
			if (granulePos == 0L)
			{
				num = this.FindFirstDataPage();
			}
			else
			{
				int num2 = this._pageOffsets.Count - 1;
				long num3;
				if (this.GetPageRaw(num2, out num3))
				{
					if (granulePos < num3)
					{
						num = this.FindPageBisection(granulePos, this.FindFirstDataPage(), num2, num3);
					}
					else if (granulePos > num3)
					{
						num = this.FindPageForward(num2, num3, granulePos);
					}
					else
					{
						num = num2 + 1;
					}
				}
			}
			if (num == -1)
			{
				throw new ArgumentOutOfRangeException("granulePos");
			}
			return num;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000820C File Offset: 0x0000640C
		private int FindFirstDataPage()
		{
			while (this._firstDataPageIndex == null)
			{
				long num;
				if (!this.GetPageRaw(this._pageOffsets.Count, out num))
				{
					return -1;
				}
			}
			return this._firstDataPageIndex.Value;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000824C File Offset: 0x0000644C
		private int FindPageForward(int pageIndex, long pageGranulePos, long granulePos)
		{
			while (pageGranulePos <= granulePos)
			{
				if (++pageIndex == this._pageOffsets.Count)
				{
					if (!this.GetNextPageGranulePos(out pageGranulePos))
					{
						long? maxGranulePosition = this.MaxGranulePosition;
						if (maxGranulePosition.GetValueOrDefault() < granulePos & maxGranulePosition != null)
						{
							pageIndex = -1;
							break;
						}
						break;
					}
				}
				else if (!this.GetPageRaw(pageIndex, out pageGranulePos))
				{
					pageIndex = -1;
					break;
				}
			}
			return pageIndex;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000082B0 File Offset: 0x000064B0
		private bool GetNextPageGranulePos(out long granulePos)
		{
			int count = this._pageOffsets.Count;
			while (count == this._pageOffsets.Count && !this.HasAllPages)
			{
				this._reader.Lock();
				try
				{
					if (!this._reader.ReadNextPage())
					{
						this.HasAllPages = true;
					}
					else if (count < this._pageOffsets.Count)
					{
						granulePos = this._reader.GranulePosition;
						return true;
					}
				}
				finally
				{
					this._reader.Release();
				}
			}
			granulePos = 0L;
			return false;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00008348 File Offset: 0x00006548
		private int FindPageBisection(long granulePos, int low, int high, long highGranulePos)
		{
			long num = 0L;
			int num2;
			while ((num2 = high - low) > 0)
			{
				int num3 = low + (int)((double)num2 * ((double)(granulePos - num) / (double)(highGranulePos - num)));
				long num4;
				if (!this.GetPageRaw(num3, out num4))
				{
					return -1;
				}
				if (num4 > granulePos)
				{
					high = num3;
					highGranulePos = num4;
				}
				else
				{
					if (num4 >= granulePos)
					{
						return num3 + 1;
					}
					low = num3 + 1;
					num = num4 + 1L;
				}
			}
			return low;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000083A4 File Offset: 0x000065A4
		private bool GetPageRaw(int pageIndex, out long pageGranulePos)
		{
			long num = this._pageOffsets[pageIndex];
			if (num < 0L)
			{
				num = -num;
			}
			this._reader.Lock();
			bool result;
			try
			{
				if (this._reader.ReadPageAt(num))
				{
					pageGranulePos = this._reader.GranulePosition;
					result = true;
				}
				else
				{
					pageGranulePos = 0L;
					result = false;
				}
			}
			finally
			{
				this._reader.Release();
			}
			return result;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008418 File Offset: 0x00006618
		public bool GetPage(int pageIndex, out long granulePos, out bool isResync, out bool isContinuation, out bool isContinued, out int packetCount, out int pageOverhead)
		{
			if (this._lastPageIndex == pageIndex)
			{
				granulePos = this._lastPageGranulePos;
				isResync = this._lastPageIsResync;
				isContinuation = this._lastPageIsContinuation;
				isContinued = this._lastPageIsContinued;
				packetCount = this._lastPagePacketCount;
				pageOverhead = this._lastPageOverhead;
				return true;
			}
			this._reader.Lock();
			try
			{
				while (pageIndex >= this._pageOffsets.Count && !this.HasAllPages && this._reader.ReadNextPage())
				{
					if (pageIndex < this._pageOffsets.Count)
					{
						isResync = this._reader.IsResync.Value;
						this.ReadPageData(pageIndex, out granulePos, out isContinuation, out isContinued, out packetCount, out pageOverhead);
						return true;
					}
				}
			}
			finally
			{
				this._reader.Release();
			}
			if (pageIndex < this._pageOffsets.Count)
			{
				long num = this._pageOffsets[pageIndex];
				if (num < 0L)
				{
					isResync = true;
					num = -num;
				}
				else
				{
					isResync = false;
				}
				this._reader.Lock();
				try
				{
					if (this._reader.ReadPageAt(num))
					{
						this._lastPageIsResync = isResync;
						this.ReadPageData(pageIndex, out granulePos, out isContinuation, out isContinued, out packetCount, out pageOverhead);
						return true;
					}
				}
				finally
				{
					this._reader.Release();
				}
			}
			granulePos = 0L;
			isResync = false;
			isContinuation = false;
			isContinued = false;
			packetCount = 0;
			pageOverhead = 0;
			return false;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008584 File Offset: 0x00006784
		private void ReadPageData(int pageIndex, out long granulePos, out bool isContinuation, out bool isContinued, out int packetCount, out int pageOverhead)
		{
			this._cachedPagePackets = null;
			this._lastPageGranulePos = (granulePos = this._reader.GranulePosition);
			this._lastPageIsContinuation = (isContinuation = ((this._reader.PageFlags & PageFlags.ContinuesPacket) > PageFlags.None));
			this._lastPageIsContinued = (isContinued = this._reader.IsContinued);
			this._lastPagePacketCount = (packetCount = (int)this._reader.PacketCount);
			this._lastPageOverhead = (pageOverhead = this._reader.PageOverhead);
			this._lastPageIndex = pageIndex;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008615 File Offset: 0x00006815
		public void SetEndOfStream()
		{
			this.HasAllPages = true;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000861E File Offset: 0x0000681E
		public int PageCount
		{
			get
			{
				return this._pageOffsets.Count;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000862B File Offset: 0x0000682B
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00008633 File Offset: 0x00006833
		public bool HasAllPages { get; private set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000863C File Offset: 0x0000683C
		public long? MaxGranulePosition
		{
			get
			{
				if (!this.HasAllPages)
				{
					return default(long?);
				}
				return new long?(this._maxGranulePos);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00008666 File Offset: 0x00006866
		public int FirstDataPageIndex
		{
			get
			{
				return this.FindFirstDataPage();
			}
		}

		// Token: 0x040000C1 RID: 193
		private readonly IPageData _reader;

		// Token: 0x040000C2 RID: 194
		private readonly List<long> _pageOffsets = new List<long>();

		// Token: 0x040000C3 RID: 195
		private int _lastSeqNbr;

		// Token: 0x040000C4 RID: 196
		private int? _firstDataPageIndex;

		// Token: 0x040000C5 RID: 197
		private long _maxGranulePos;

		// Token: 0x040000C6 RID: 198
		private int _lastPageIndex = -1;

		// Token: 0x040000C7 RID: 199
		private long _lastPageGranulePos;

		// Token: 0x040000C8 RID: 200
		private bool _lastPageIsResync;

		// Token: 0x040000C9 RID: 201
		private bool _lastPageIsContinuation;

		// Token: 0x040000CA RID: 202
		private bool _lastPageIsContinued;

		// Token: 0x040000CB RID: 203
		private int _lastPagePacketCount;

		// Token: 0x040000CC RID: 204
		private int _lastPageOverhead;

		// Token: 0x040000CD RID: 205
		private Memory<byte>[] _cachedPagePackets;
	}
}
