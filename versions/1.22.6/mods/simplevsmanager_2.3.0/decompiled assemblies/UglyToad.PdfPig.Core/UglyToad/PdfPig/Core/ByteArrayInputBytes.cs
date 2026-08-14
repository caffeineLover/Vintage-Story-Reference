using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000002 RID: 2
	public class ByteArrayInputBytes : IInputBytes, IDisposable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[DebuggerStepThrough]
		public ByteArrayInputBytes(IReadOnlyList<byte> bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			byte[] array = bytes as byte[];
			if (array != null)
			{
				this.bytes = array;
			}
			else
			{
				this.bytes = Enumerable.ToArray<byte>(bytes);
			}
			this.upperBound = this.bytes.Length - 1;
			this.currentOffset = -1;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020A7 File Offset: 0x000002A7
		[DebuggerStepThrough]
		public ByteArrayInputBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.bytes = bytes;
			this.currentOffset = -1;
			this.upperBound = bytes.Length - 1;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020D7 File Offset: 0x000002D7
		public long CurrentOffset
		{
			get
			{
				return (long)(this.currentOffset + 1);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E2 File Offset: 0x000002E2
		public bool MoveNext()
		{
			if (this.currentOffset == this.upperBound)
			{
				return false;
			}
			this.currentOffset++;
			this.CurrentByte = this.bytes[this.currentOffset];
			return true;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002116 File Offset: 0x00000316
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000211E File Offset: 0x0000031E
		public byte CurrentByte { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002127 File Offset: 0x00000327
		public long Length
		{
			get
			{
				return (long)this.bytes.Length;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002134 File Offset: 0x00000334
		public byte? Peek()
		{
			if (this.currentOffset == this.upperBound)
			{
				return default(byte?);
			}
			return new byte?(this.bytes[this.currentOffset + 1]);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000216D File Offset: 0x0000036D
		public bool IsAtEnd()
		{
			return this.currentOffset == this.upperBound;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000217D File Offset: 0x0000037D
		public void Seek(long position)
		{
			this.currentOffset = (int)position - 1;
			this.CurrentByte = ((this.currentOffset < 0) ? 0 : this.bytes[this.currentOffset]);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021A8 File Offset: 0x000003A8
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
			int num2 = this.bytes.Length - this.currentOffset - 1;
			int num3 = (num2 < num) ? num2 : num;
			int num4 = this.currentOffset + 1;
			Array.Copy(this.bytes, num4, buffer, 0, num3);
			if (num3 > 0)
			{
				this.currentOffset += num3;
				this.CurrentByte = buffer[num3 - 1];
			}
			return num3;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022C7 File Offset: 0x000004C7
		public void Dispose()
		{
		}

		// Token: 0x04000001 RID: 1
		private readonly int upperBound;

		// Token: 0x04000002 RID: 2
		private readonly byte[] bytes;

		// Token: 0x04000003 RID: 3
		private int currentOffset;
	}
}
