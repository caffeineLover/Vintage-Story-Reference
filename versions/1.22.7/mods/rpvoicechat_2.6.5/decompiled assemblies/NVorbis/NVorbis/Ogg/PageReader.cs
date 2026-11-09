using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NVorbis.Contracts;
using NVorbis.Contracts.Ogg;

namespace NVorbis.Ogg
{
	// Token: 0x0200001E RID: 30
	internal class PageReader : PageReaderBase, IPageData, IPageReader, IDisposable
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00007499 File Offset: 0x00005699
		// (set) Token: 0x06000147 RID: 327 RVA: 0x000074A0 File Offset: 0x000056A0
		internal static Func<IPageData, int, IStreamPageReader> CreateStreamPageReader { get; set; } = (IPageData pr, int ss) => new StreamPageReader(pr, ss);

		// Token: 0x06000148 RID: 328 RVA: 0x000074A8 File Offset: 0x000056A8
		public PageReader(Stream stream, bool closeOnDispose, Func<NVorbis.Contracts.IPacketProvider, bool> newStreamCallback) : base(stream, closeOnDispose)
		{
			this._newStreamCallback = newStreamCallback;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000074D0 File Offset: 0x000056D0
		private ushort ParsePageHeader(byte[] pageBuf, int? streamSerial, bool? isResync)
		{
			byte b = pageBuf[26];
			int num = 0;
			int num2 = 0;
			bool isContinued = false;
			int num3 = 0;
			int i = 0;
			int num4 = 27;
			while (i < (int)b)
			{
				byte b2 = pageBuf[num4];
				num3 += (int)b2;
				num += (int)b2;
				if (b2 < 255)
				{
					if (num3 > 0)
					{
						num2++;
					}
					num3 = 0;
				}
				i++;
				num4++;
			}
			if (num3 > 0)
			{
				isContinued = (pageBuf[(int)(b + 26)] == byte.MaxValue);
				num2++;
			}
			this.StreamSerial = (streamSerial ?? BitConverter.ToInt32(pageBuf, 14));
			this.SequenceNumber = BitConverter.ToInt32(pageBuf, 18);
			this.PageFlags = (PageFlags)pageBuf[5];
			this.GranulePosition = BitConverter.ToInt64(pageBuf, 6);
			this.PacketCount = (short)num2;
			this.IsResync = isResync;
			this.IsContinued = isContinued;
			this.PageOverhead = (int)(27 + b);
			return (ushort)(this.PageOverhead + num);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000075B4 File Offset: 0x000057B4
		private unsafe static Memory<byte>[] ReadPackets(int packetCount, Span<byte> segments, Memory<byte> dataBuffer)
		{
			Memory<byte>[] array = new Memory<byte>[packetCount];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < segments.Length; i++)
			{
				byte b = *segments[i];
				num3 += (int)b;
				if (b < 255)
				{
					if (num3 > 0)
					{
						array[num++] = dataBuffer.Slice(num2, num3);
						num2 += num3;
					}
					num3 = 0;
				}
			}
			if (num3 > 0)
			{
				array[num] = dataBuffer.Slice(num2, num3);
			}
			return array;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007631 File Offset: 0x00005831
		public override void Lock()
		{
			Monitor.Enter(this._readLock);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000763E File Offset: 0x0000583E
		protected override bool CheckLock()
		{
			return Monitor.IsEntered(this._readLock);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000764B File Offset: 0x0000584B
		public override bool Release()
		{
			if (Monitor.IsEntered(this._readLock))
			{
				Monitor.Exit(this._readLock);
				return true;
			}
			return false;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007668 File Offset: 0x00005868
		protected override void SaveNextPageSearch()
		{
			this._nextPageOffset = base.StreamPosition;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007676 File Offset: 0x00005876
		protected override void PrepareStreamForNextPage()
		{
			base.SeekStream(this._nextPageOffset);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00007688 File Offset: 0x00005888
		protected override bool AddPage(int streamSerial, byte[] pageBuf, bool isResync)
		{
			this.PageOffset = base.StreamPosition - (long)pageBuf.Length;
			this.ParsePageHeader(pageBuf, new int?(streamSerial), new bool?(isResync));
			if (this.PacketCount == 0)
			{
				return false;
			}
			this._packets = PageReader.ReadPackets((int)this.PacketCount, new Span<byte>(pageBuf, 27, (int)pageBuf[26]), new Memory<byte>(pageBuf, (int)(27 + pageBuf[26]), pageBuf.Length - 27 - (int)pageBuf[26]));
			IStreamPageReader streamPageReader;
			if (this._streamReaders.TryGetValue(streamSerial, ref streamPageReader))
			{
				streamPageReader.AddPage();
				if ((this.PageFlags & PageFlags.EndOfStream) == PageFlags.EndOfStream)
				{
					this._streamReaders.Remove(this.StreamSerial);
				}
			}
			else
			{
				IStreamPageReader streamPageReader2 = PageReader.CreateStreamPageReader.Invoke(this, this.StreamSerial);
				streamPageReader2.AddPage();
				this._streamReaders.Add(this.StreamSerial, streamPageReader2);
				if (!this._newStreamCallback.Invoke(streamPageReader2.PacketProvider))
				{
					this._streamReaders.Remove(this.StreamSerial);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00007780 File Offset: 0x00005980
		public override bool ReadPageAt(long offset)
		{
			if (!this.CheckLock())
			{
				throw new InvalidOperationException("Must be locked prior to reading!");
			}
			if (offset == this.PageOffset)
			{
				return true;
			}
			byte[] array = new byte[282];
			base.SeekStream(offset);
			int num = base.EnsureRead(array, 0, 27, 10);
			this.PageOffset = offset;
			if (base.VerifyHeader(array, 0, ref num))
			{
				this._packets = null;
				this._pageSize = this.ParsePageHeader(array, default(int?), default(bool?));
				return true;
			}
			return false;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007808 File Offset: 0x00005A08
		protected override void SetEndOfStreams()
		{
			foreach (KeyValuePair<int, IStreamPageReader> keyValuePair in this._streamReaders)
			{
				keyValuePair.Value.SetEndOfStream();
			}
			this._streamReaders.Clear();
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000153 RID: 339 RVA: 0x0000786C File Offset: 0x00005A6C
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00007874 File Offset: 0x00005A74
		public long PageOffset { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000787D File Offset: 0x00005A7D
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00007885 File Offset: 0x00005A85
		public int StreamSerial { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000788E File Offset: 0x00005A8E
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00007896 File Offset: 0x00005A96
		public int SequenceNumber { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000789F File Offset: 0x00005A9F
		// (set) Token: 0x0600015A RID: 346 RVA: 0x000078A7 File Offset: 0x00005AA7
		public PageFlags PageFlags { get; private set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000078B0 File Offset: 0x00005AB0
		// (set) Token: 0x0600015C RID: 348 RVA: 0x000078B8 File Offset: 0x00005AB8
		public long GranulePosition { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000078C1 File Offset: 0x00005AC1
		// (set) Token: 0x0600015E RID: 350 RVA: 0x000078C9 File Offset: 0x00005AC9
		public short PacketCount { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600015F RID: 351 RVA: 0x000078D2 File Offset: 0x00005AD2
		// (set) Token: 0x06000160 RID: 352 RVA: 0x000078DA File Offset: 0x00005ADA
		public bool? IsResync { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000078E3 File Offset: 0x00005AE3
		// (set) Token: 0x06000162 RID: 354 RVA: 0x000078EB File Offset: 0x00005AEB
		public bool IsContinued { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000078F4 File Offset: 0x00005AF4
		// (set) Token: 0x06000164 RID: 356 RVA: 0x000078FC File Offset: 0x00005AFC
		public int PageOverhead { get; private set; }

		// Token: 0x06000165 RID: 357 RVA: 0x00007908 File Offset: 0x00005B08
		public Memory<byte>[] GetPackets()
		{
			if (!this.CheckLock())
			{
				throw new InvalidOperationException("Must be locked!");
			}
			if (this._packets == null)
			{
				byte[] array = new byte[(int)this._pageSize];
				base.SeekStream(this.PageOffset);
				base.EnsureRead(array, 0, (int)this._pageSize, 10);
				this._packets = PageReader.ReadPackets((int)this.PacketCount, new Span<byte>(array, 27, (int)array[26]), new Memory<byte>(array, (int)(27 + array[26]), array.Length - 27 - (int)array[26]));
			}
			return this._packets;
		}

		// Token: 0x040000A7 RID: 167
		private readonly Dictionary<int, IStreamPageReader> _streamReaders = new Dictionary<int, IStreamPageReader>();

		// Token: 0x040000A8 RID: 168
		private readonly Func<NVorbis.Contracts.IPacketProvider, bool> _newStreamCallback;

		// Token: 0x040000A9 RID: 169
		private readonly object _readLock = new object();

		// Token: 0x040000AA RID: 170
		private long _nextPageOffset;

		// Token: 0x040000AB RID: 171
		private ushort _pageSize;

		// Token: 0x040000AC RID: 172
		private Memory<byte>[] _packets;
	}
}
