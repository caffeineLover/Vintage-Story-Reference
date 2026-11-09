using System;
using System.Collections.Generic;
using System.IO;
using NVorbis.Contracts.Ogg;

namespace NVorbis.Ogg
{
	// Token: 0x0200001F RID: 31
	internal abstract class PageReaderBase : IPageReader, IDisposable
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000167 RID: 359 RVA: 0x000079AC File Offset: 0x00005BAC
		// (set) Token: 0x06000168 RID: 360 RVA: 0x000079B3 File Offset: 0x00005BB3
		internal static Func<ICrc> CreateCrc { get; set; } = () => new Crc();

		// Token: 0x06000169 RID: 361 RVA: 0x000079BC File Offset: 0x00005BBC
		protected PageReaderBase(Stream stream, bool closeOnDispose)
		{
			this._stream = stream;
			this._closeOnDispose = closeOnDispose;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00007A08 File Offset: 0x00005C08
		protected long StreamPosition
		{
			get
			{
				Stream stream = this._stream;
				if (stream == null)
				{
					throw new ObjectDisposedException("PageReaderBase");
				}
				return stream.Position;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00007A24 File Offset: 0x00005C24
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00007A2C File Offset: 0x00005C2C
		public long ContainerBits { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00007A35 File Offset: 0x00005C35
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00007A3D File Offset: 0x00005C3D
		public long WasteBits { get; private set; }

		// Token: 0x0600016F RID: 367 RVA: 0x00007A48 File Offset: 0x00005C48
		private bool VerifyPage(byte[] headerBuf, int index, int cnt, out byte[] pageBuf, out int bytesRead)
		{
			byte b = headerBuf[index + 26];
			if (cnt - index < index + 27 + (int)b)
			{
				pageBuf = null;
				bytesRead = 0;
				return false;
			}
			int num = 0;
			int i;
			for (i = 0; i < (int)b; i++)
			{
				num += (int)headerBuf[index + i + 27];
			}
			pageBuf = new byte[num + (int)b + 27];
			Buffer.BlockCopy(headerBuf, index, pageBuf, 0, (int)(b + 27));
			bytesRead = this.EnsureRead(pageBuf, (int)(b + 27), num, 10);
			if (bytesRead != num)
			{
				return false;
			}
			num = pageBuf.Length;
			this._crc.Reset();
			for (i = 0; i < 22; i++)
			{
				this._crc.Update((int)pageBuf[i]);
			}
			this._crc.Update(0);
			this._crc.Update(0);
			this._crc.Update(0);
			this._crc.Update(0);
			for (i += 4; i < num; i++)
			{
				this._crc.Update((int)pageBuf[i]);
			}
			return this._crc.Test(BitConverter.ToUInt32(pageBuf, 22));
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007B54 File Offset: 0x00005D54
		private bool AddPage(byte[] pageBuf, bool isResync)
		{
			int num = BitConverter.ToInt32(pageBuf, 14);
			if (!this._ignoredSerials.Contains(num))
			{
				if (this.AddPage(num, pageBuf, isResync))
				{
					this.ContainerBits += (long)(8 * (27 + pageBuf[26]));
					return true;
				}
				this._ignoredSerials.Add(num);
			}
			return false;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007BAC File Offset: 0x00005DAC
		private void EnqueueData(byte[] buf, int count)
		{
			if (this._overflowBuf != null)
			{
				byte[] array = new byte[this._overflowBuf.Length - this._overflowBufIndex + count];
				Buffer.BlockCopy(this._overflowBuf, this._overflowBufIndex, array, 0, array.Length - count);
				int num = buf.Length - count;
				Buffer.BlockCopy(buf, num, array, array.Length - count, count);
				this._overflowBufIndex = 0;
				return;
			}
			this._overflowBuf = buf;
			this._overflowBufIndex = buf.Length - count;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007C20 File Offset: 0x00005E20
		private void ClearEnqueuedData(int count)
		{
			if (this._overflowBuf != null && (this._overflowBufIndex += count) >= this._overflowBuf.Length)
			{
				this._overflowBuf = null;
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007C58 File Offset: 0x00005E58
		private int FillHeader(byte[] buf, int index, int count, int maxTries = 10)
		{
			int num = 0;
			if (this._overflowBuf != null)
			{
				num = Math.Min(this._overflowBuf.Length - this._overflowBufIndex, count);
				Buffer.BlockCopy(this._overflowBuf, this._overflowBufIndex, buf, index, num);
				index += num;
				count -= num;
				if ((this._overflowBufIndex += num) == this._overflowBuf.Length)
				{
					this._overflowBuf = null;
				}
			}
			if (count > 0)
			{
				num += this.EnsureRead(buf, index, count, maxTries);
			}
			return num;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007CD8 File Offset: 0x00005ED8
		private bool VerifyHeader(byte[] buffer, int index, ref int cnt, bool isFromReadNextPage)
		{
			if (buffer[index] == 79 && buffer[index + 1] == 103 && buffer[index + 2] == 103 && buffer[index + 3] == 83)
			{
				if (cnt < 27)
				{
					if (isFromReadNextPage)
					{
						cnt += this.FillHeader(buffer, 27 - cnt + index, 27 - cnt, 10);
					}
					else
					{
						cnt += this.EnsureRead(buffer, 27 - cnt + index, 27 - cnt, 10);
					}
				}
				if (cnt >= 27)
				{
					byte b = buffer[index + 26];
					if (isFromReadNextPage)
					{
						cnt += this.FillHeader(buffer, index + 27, (int)b, 10);
					}
					else
					{
						cnt += this.EnsureRead(buffer, index + 27, (int)b, 10);
					}
					if (cnt == index + 27 + (int)b)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007D9C File Offset: 0x00005F9C
		protected int EnsureRead(byte[] buf, int index, int count, int maxTries = 10)
		{
			int num = 0;
			int num2 = 0;
			do
			{
				int num3 = this._stream.Read(buf, index + num, count - num);
				if (num3 == 0 && ++num2 == maxTries)
				{
					break;
				}
				num += num3;
			}
			while (num < count);
			return num;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007DD5 File Offset: 0x00005FD5
		protected bool VerifyHeader(byte[] buffer, int index, ref int cnt)
		{
			return this.VerifyHeader(buffer, index, ref cnt, false);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007DE1 File Offset: 0x00005FE1
		protected long SeekStream(long offset)
		{
			if (!this.CheckLock())
			{
				throw new InvalidOperationException("Must be locked prior to reading!");
			}
			return this._stream.Seek(offset, 0);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007E03 File Offset: 0x00006003
		protected virtual void PrepareStreamForNextPage()
		{
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007E05 File Offset: 0x00006005
		protected virtual void SaveNextPageSearch()
		{
		}

		// Token: 0x0600017A RID: 378
		protected abstract bool AddPage(int streamSerial, byte[] pageBuf, bool isResync);

		// Token: 0x0600017B RID: 379
		protected abstract void SetEndOfStreams();

		// Token: 0x0600017C RID: 380 RVA: 0x00007E07 File Offset: 0x00006007
		public virtual void Lock()
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007E09 File Offset: 0x00006009
		protected virtual bool CheckLock()
		{
			return true;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007E0C File Offset: 0x0000600C
		public virtual bool Release()
		{
			return false;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007E10 File Offset: 0x00006010
		public bool ReadNextPage()
		{
			if (!this.CheckLock())
			{
				throw new InvalidOperationException("Must be locked prior to reading!");
			}
			bool isResync = false;
			int num = 0;
			this.PrepareStreamForNextPage();
			int num2;
			while ((num2 = this.FillHeader(this._headerBuf, num, 27 - num, 10)) > 0)
			{
				num2 += num;
				for (int i = 0; i < num2 - 4; i++)
				{
					if (this.VerifyHeader(this._headerBuf, i, ref num2, true))
					{
						byte[] array;
						int count;
						if (this.VerifyPage(this._headerBuf, i, num2, out array, out count))
						{
							this.ClearEnqueuedData(count);
							this.SaveNextPageSearch();
							if (this.AddPage(array, isResync))
							{
								return true;
							}
							this.WasteBits += (long)(array.Length * 8);
							num = 0;
							num2 = 0;
							break;
						}
						else if (array != null)
						{
							this.EnqueueData(array, count);
						}
					}
					this.WasteBits += 8L;
					isResync = true;
				}
				if (num2 >= 3)
				{
					this._headerBuf[0] = this._headerBuf[num2 - 3];
					this._headerBuf[1] = this._headerBuf[num2 - 2];
					this._headerBuf[2] = this._headerBuf[num2 - 1];
					num = 3;
				}
			}
			if (num2 == 0)
			{
				this.SetEndOfStreams();
			}
			return false;
		}

		// Token: 0x06000180 RID: 384
		public abstract bool ReadPageAt(long offset);

		// Token: 0x06000181 RID: 385 RVA: 0x00007F2F File Offset: 0x0000612F
		public void Dispose()
		{
			this.SetEndOfStreams();
			if (this._closeOnDispose)
			{
				Stream stream = this._stream;
				if (stream != null)
				{
					stream.Dispose();
				}
			}
			this._stream = null;
		}

		// Token: 0x040000B7 RID: 183
		private readonly ICrc _crc = PageReaderBase.CreateCrc.Invoke();

		// Token: 0x040000B8 RID: 184
		private readonly HashSet<int> _ignoredSerials = new HashSet<int>();

		// Token: 0x040000B9 RID: 185
		private readonly byte[] _headerBuf = new byte[305];

		// Token: 0x040000BA RID: 186
		private byte[] _overflowBuf;

		// Token: 0x040000BB RID: 187
		private int _overflowBufIndex;

		// Token: 0x040000BC RID: 188
		private Stream _stream;

		// Token: 0x040000BD RID: 189
		private bool _closeOnDispose;
	}
}
