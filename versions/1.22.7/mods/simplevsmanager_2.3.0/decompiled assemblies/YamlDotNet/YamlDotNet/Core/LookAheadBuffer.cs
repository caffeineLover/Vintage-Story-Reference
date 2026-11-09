using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020000CF RID: 207
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	public sealed class LookAheadBuffer : ILookAheadBuffer
	{
		// Token: 0x06000522 RID: 1314 RVA: 0x00011B00 File Offset: 0x0000FD00
		public LookAheadBuffer(TextReader input, int capacity)
		{
			if (capacity < 1)
			{
				throw new ArgumentOutOfRangeException("capacity", "The capacity must be positive.");
			}
			if (!capacity.IsPowerOfTwo())
			{
				throw new ArgumentException("The capacity must be a power of 2.", "capacity");
			}
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			this.input = input;
			this.blockSize = capacity;
			this.buffer = new char[capacity * 2];
			this.mask = capacity * 2 - 1;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00011B75 File Offset: 0x0000FD75
		public bool EndOfInput
		{
			get
			{
				return this.endOfInput && this.count == 0;
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00011B8A File Offset: 0x0000FD8A
		private int GetIndexForOffset(int offset)
		{
			return this.firstIndex + offset & this.mask;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00011B9B File Offset: 0x0000FD9B
		public char Peek(int offset)
		{
			if (offset >= this.count)
			{
				this.FillBuffer();
			}
			if (offset < this.count)
			{
				return this.buffer[this.firstIndex + offset & this.mask];
			}
			return '\0';
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00011BCD File Offset: 0x0000FDCD
		public void Cache(int length)
		{
			if (length >= this.count)
			{
				this.FillBuffer();
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00011BE0 File Offset: 0x0000FDE0
		private void FillBuffer()
		{
			if (this.endOfInput)
			{
				return;
			}
			int num = this.blockSize;
			for (;;)
			{
				int num2 = this.input.Read(this.buffer, this.writeOffset, num);
				if (num2 == 0)
				{
					break;
				}
				num -= num2;
				this.writeOffset += num2;
				this.count += num2;
				if (num <= 0)
				{
					goto Block_3;
				}
			}
			this.endOfInput = true;
			return;
			Block_3:
			if (this.writeOffset == this.buffer.Length)
			{
				this.writeOffset = 0;
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00011C5C File Offset: 0x0000FE5C
		public void Skip(int length)
		{
			if (length < 1 || length > this.blockSize)
			{
				throw new ArgumentOutOfRangeException("length", "The length must be between 1 and the number of characters in the buffer. Use the Peek() and / or Cache() methods to fill the buffer.");
			}
			this.firstIndex = this.GetIndexForOffset(length);
			this.count -= length;
		}

		// Token: 0x04000164 RID: 356
		private readonly TextReader input;

		// Token: 0x04000165 RID: 357
		private readonly char[] buffer;

		// Token: 0x04000166 RID: 358
		private readonly int blockSize;

		// Token: 0x04000167 RID: 359
		private readonly int mask;

		// Token: 0x04000168 RID: 360
		private int firstIndex;

		// Token: 0x04000169 RID: 361
		private int writeOffset;

		// Token: 0x0400016A RID: 362
		private int count;

		// Token: 0x0400016B RID: 363
		private bool endOfInput;
	}
}
