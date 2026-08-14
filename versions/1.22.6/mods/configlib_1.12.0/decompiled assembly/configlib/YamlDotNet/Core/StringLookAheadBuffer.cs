using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001CE RID: 462
	internal sealed class StringLookAheadBuffer : ILookAheadBuffer
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x00025219 File Offset: 0x00023419
		// (set) Token: 0x06000A7F RID: 2687 RVA: 0x00025221 File Offset: 0x00023421
		public int Position { get; private set; }

		// Token: 0x06000A80 RID: 2688 RVA: 0x0002522A File Offset: 0x0002342A
		[NullableContext(1)]
		public StringLookAheadBuffer(string value)
		{
			this.value = value;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x00025239 File Offset: 0x00023439
		public int Length
		{
			get
			{
				return this.value.Length;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x00025246 File Offset: 0x00023446
		public bool EndOfInput
		{
			get
			{
				return this.IsOutside(this.Position);
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00025254 File Offset: 0x00023454
		public char Peek(int offset)
		{
			int num = this.Position + offset;
			if (!this.IsOutside(num))
			{
				return this.value.get_Chars(num);
			}
			return '\0';
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00025281 File Offset: 0x00023481
		private bool IsOutside(int index)
		{
			return index >= this.value.Length;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00025294 File Offset: 0x00023494
		public void Skip(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "The length must be positive.");
			}
			this.Position += length;
		}

		// Token: 0x04000472 RID: 1138
		[Nullable(1)]
		private readonly string value;
	}
}
