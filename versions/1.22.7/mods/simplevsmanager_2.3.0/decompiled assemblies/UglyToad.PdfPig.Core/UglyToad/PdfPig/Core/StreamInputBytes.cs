using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000012 RID: 18
	public class StreamInputBytes : IInputBytes, IDisposable
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003F2F File Offset: 0x0000212F
		public long CurrentOffset
		{
			get
			{
				return this.stream.Position;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003F3C File Offset: 0x0000213C
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003F44 File Offset: 0x00002144
		public byte CurrentByte { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003F4D File Offset: 0x0000214D
		public long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003F5C File Offset: 0x0000215C
		public StreamInputBytes(Stream stream, bool shouldDispose = true)
		{
			if (stream == null)
			{
				throw new ArgumentNullException();
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException("The provided stream did not support reading.");
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException("The provided stream did not support seeking.");
			}
			this.stream = stream;
			this.shouldDispose = shouldDispose;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003FAC File Offset: 0x000021AC
		public bool MoveNext()
		{
			int num = this.stream.ReadByte();
			if (num == -1)
			{
				this.isAtEnd = true;
				this.CurrentByte = 0;
				return false;
			}
			this.CurrentByte = (byte)num;
			return true;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003FE4 File Offset: 0x000021E4
		public byte? Peek()
		{
			long currentOffset = this.CurrentOffset;
			int num = this.stream.ReadByte();
			this.stream.Seek(currentOffset, 0);
			if (num == -1)
			{
				return default(byte?);
			}
			return new byte?((byte)num);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004027 File Offset: 0x00002227
		public bool IsAtEnd()
		{
			return this.isAtEnd;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000402F File Offset: 0x0000222F
		public void Seek(long position)
		{
			this.isAtEnd = false;
			if (position == 0L)
			{
				this.stream.Seek(0L, 0);
				this.CurrentByte = 0;
				return;
			}
			this.stream.Position = position - 1L;
			this.MoveNext();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004068 File Offset: 0x00002268
		public int Read(byte[] buffer, int? length = null)
		{
			int num = buffer.Length;
			if (length != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (length.Value < 0)
				{
					defaultInterpolatedStringHandler..ctor(31, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot use a negative length: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(length.Value);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (length.Value > num)
				{
					defaultInterpolatedStringHandler..ctor(59, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot read more bytes ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(length.Value);
					defaultInterpolatedStringHandler.AppendLiteral(" than there is space in the buffer ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(buffer.Length);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				num = length.Value;
			}
			if (num == 0)
			{
				return 0;
			}
			int num2 = this.stream.Read(buffer, 0, num);
			if (num2 > 0)
			{
				this.CurrentByte = buffer[num2 - 1];
			}
			this.isAtEnd = (this.stream.Position == this.stream.Length);
			return num2;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004173 File Offset: 0x00002373
		public void Dispose()
		{
			if (this.shouldDispose)
			{
				Stream stream = this.stream;
				if (stream == null)
				{
					return;
				}
				stream.Dispose();
			}
		}

		// Token: 0x04000022 RID: 34
		private readonly Stream stream;

		// Token: 0x04000023 RID: 35
		private readonly bool shouldDispose;

		// Token: 0x04000024 RID: 36
		private bool isAtEnd;
	}
}
