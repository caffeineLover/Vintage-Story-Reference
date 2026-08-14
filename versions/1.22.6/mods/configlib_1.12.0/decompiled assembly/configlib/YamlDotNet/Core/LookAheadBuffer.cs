using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020001BC RID: 444
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	public sealed class LookAheadBuffer : ILookAheadBuffer
	{
		// Token: 0x060009C1 RID: 2497 RVA: 0x00020024 File Offset: 0x0001E224
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

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x00020099 File Offset: 0x0001E299
		public bool EndOfInput
		{
			get
			{
				return this.endOfInput && this.count == 0;
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x000200AE File Offset: 0x0001E2AE
		private int GetIndexForOffset(int offset)
		{
			return this.firstIndex + offset & this.mask;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x000200BF File Offset: 0x0001E2BF
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

		// Token: 0x060009C5 RID: 2501 RVA: 0x000200F1 File Offset: 0x0001E2F1
		public void Cache(int length)
		{
			if (length >= this.count)
			{
				this.FillBuffer();
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00020104 File Offset: 0x0001E304
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

		// Token: 0x060009C7 RID: 2503 RVA: 0x00020180 File Offset: 0x0001E380
		public void Skip(int length)
		{
			if (length < 1 || length > this.blockSize)
			{
				throw new ArgumentOutOfRangeException("length", "The length must be between 1 and the number of characters in the buffer. Use the Peek() and / or Cache() methods to fill the buffer.");
			}
			this.firstIndex = this.GetIndexForOffset(length);
			this.count -= length;
		}

		// Token: 0x0400040B RID: 1035
		private readonly TextReader input;

		// Token: 0x0400040C RID: 1036
		private readonly char[] buffer;

		// Token: 0x0400040D RID: 1037
		private readonly int blockSize;

		// Token: 0x0400040E RID: 1038
		private readonly int mask;

		// Token: 0x0400040F RID: 1039
		private int firstIndex;

		// Token: 0x04000410 RID: 1040
		private int writeOffset;

		// Token: 0x04000411 RID: 1041
		private int count;

		// Token: 0x04000412 RID: 1042
		private bool endOfInput;
	}
}
