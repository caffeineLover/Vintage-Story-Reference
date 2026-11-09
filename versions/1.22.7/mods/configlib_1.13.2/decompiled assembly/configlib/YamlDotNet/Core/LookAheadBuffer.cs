using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020001B8 RID: 440
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	public sealed class LookAheadBuffer : ILookAheadBuffer
	{
		// Token: 0x06000985 RID: 2437 RVA: 0x0001F6DC File Offset: 0x0001D8DC
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
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0001F751 File Offset: 0x0001D951
		public bool EndOfInput
		{
			get
			{
				return this.endOfInput && this.count == 0;
			}
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0001F766 File Offset: 0x0001D966
		private int GetIndexForOffset(int offset)
		{
			return this.firstIndex + offset & this.mask;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0001F777 File Offset: 0x0001D977
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

		// Token: 0x06000989 RID: 2441 RVA: 0x0001F7A9 File Offset: 0x0001D9A9
		public void Cache(int length)
		{
			if (length >= this.count)
			{
				this.FillBuffer();
			}
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0001F7BC File Offset: 0x0001D9BC
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

		// Token: 0x0600098B RID: 2443 RVA: 0x0001F838 File Offset: 0x0001DA38
		public void Skip(int length)
		{
			if (length < 1 || length > this.blockSize)
			{
				throw new ArgumentOutOfRangeException("length", "The length must be between 1 and the number of characters in the buffer. Use the Peek() and / or Cache() methods to fill the buffer.");
			}
			this.firstIndex = this.GetIndexForOffset(length);
			this.count -= length;
		}

		// Token: 0x040003DD RID: 989
		private readonly TextReader input;

		// Token: 0x040003DE RID: 990
		private readonly char[] buffer;

		// Token: 0x040003DF RID: 991
		private readonly int blockSize;

		// Token: 0x040003E0 RID: 992
		private readonly int mask;

		// Token: 0x040003E1 RID: 993
		private int firstIndex;

		// Token: 0x040003E2 RID: 994
		private int writeOffset;

		// Token: 0x040003E3 RID: 995
		private int count;

		// Token: 0x040003E4 RID: 996
		private bool endOfInput;
	}
}
