using System;
using System.IO;

namespace UglyToad.PdfPig.IO
{
	// Token: 0x020000E7 RID: 231
	internal class StreamWrapper : Stream
	{
		// Token: 0x06000461 RID: 1121 RVA: 0x0001ADFD File Offset: 0x00018FFD
		public StreamWrapper(Stream stream)
		{
			this.Stream = stream;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001AE0C File Offset: 0x0001900C
		public override void Flush()
		{
			this.Stream.Flush();
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001AE19 File Offset: 0x00019019
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.Stream.Seek(offset, origin);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001AE28 File Offset: 0x00019028
		public override void SetLength(long value)
		{
			this.Stream.SetLength(value);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001AE36 File Offset: 0x00019036
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.Stream.Read(buffer, offset, count);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001AE46 File Offset: 0x00019046
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.Stream.Write(buffer, offset, count);
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x0001AE56 File Offset: 0x00019056
		public override bool CanRead
		{
			get
			{
				return this.Stream.CanRead;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0001AE63 File Offset: 0x00019063
		public override bool CanSeek
		{
			get
			{
				return this.Stream.CanSeek;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0001AE70 File Offset: 0x00019070
		public override bool CanWrite
		{
			get
			{
				return this.Stream.CanWrite;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0001AE7D File Offset: 0x0001907D
		public override long Length
		{
			get
			{
				return this.Stream.Length;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x0001AE8A File Offset: 0x0001908A
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x0001AE97 File Offset: 0x00019097
		public override long Position
		{
			get
			{
				return this.Stream.Position;
			}
			set
			{
				this.Stream.Position = value;
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001AEA5 File Offset: 0x000190A5
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Stream stream = this.Stream;
			if (stream == null)
			{
				return;
			}
			stream.Dispose();
		}

		// Token: 0x04000234 RID: 564
		protected readonly Stream Stream;
	}
}
