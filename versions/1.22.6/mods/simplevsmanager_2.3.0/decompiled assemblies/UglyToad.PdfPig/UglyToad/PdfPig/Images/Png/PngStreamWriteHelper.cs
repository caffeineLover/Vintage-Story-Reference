using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x02000100 RID: 256
	internal class PngStreamWriteHelper : Stream
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001D233 File Offset: 0x0001B433
		public override bool CanRead
		{
			get
			{
				return this.inner.CanRead;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0001D240 File Offset: 0x0001B440
		public override bool CanSeek
		{
			get
			{
				return this.inner.CanSeek;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0001D24D File Offset: 0x0001B44D
		public override bool CanWrite
		{
			get
			{
				return this.inner.CanWrite;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x0001D25A File Offset: 0x0001B45A
		public override long Length
		{
			get
			{
				return this.inner.Length;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0001D267 File Offset: 0x0001B467
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x0001D274 File Offset: 0x0001B474
		public override long Position
		{
			get
			{
				return this.inner.Position;
			}
			set
			{
				this.inner.Position = value;
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001D282 File Offset: 0x0001B482
		public PngStreamWriteHelper(Stream inner)
		{
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			this.inner = inner;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001D2AB File Offset: 0x0001B4AB
		public override void Flush()
		{
			this.inner.Flush();
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001D2B8 File Offset: 0x0001B4B8
		public void WriteChunkHeader(byte[] header)
		{
			this.written.Clear();
			this.Write(header, 0, header.Length);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001D2D0 File Offset: 0x0001B4D0
		public void WriteChunkLength(int length)
		{
			StreamHelper.WriteBigEndianInt32(this.inner, length);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001D2DE File Offset: 0x0001B4DE
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.inner.Read(buffer, offset, count);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001D2EE File Offset: 0x0001B4EE
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.inner.Seek(offset, origin);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001D2FD File Offset: 0x0001B4FD
		public override void SetLength(long value)
		{
			this.inner.SetLength(value);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001D30B File Offset: 0x0001B50B
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.written.AddRange(Enumerable.Take<byte>(Enumerable.Skip<byte>(buffer, offset), count));
			this.inner.Write(buffer, offset, count);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001D334 File Offset: 0x0001B534
		public void WriteCrc()
		{
			int value = (int)Crc32.Calculate(this.written);
			StreamHelper.WriteBigEndianInt32(this.inner, value);
		}

		// Token: 0x0400029C RID: 668
		private readonly Stream inner;

		// Token: 0x0400029D RID: 669
		private readonly List<byte> written = new List<byte>();
	}
}
