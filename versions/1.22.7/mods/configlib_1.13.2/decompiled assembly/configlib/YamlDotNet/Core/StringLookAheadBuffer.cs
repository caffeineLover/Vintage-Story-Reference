using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001CA RID: 458
	internal sealed class StringLookAheadBuffer : ILookAheadBuffer
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x000248D1 File Offset: 0x00022AD1
		// (set) Token: 0x06000A43 RID: 2627 RVA: 0x000248D9 File Offset: 0x00022AD9
		public int Position { get; private set; }

		// Token: 0x06000A44 RID: 2628 RVA: 0x000248E2 File Offset: 0x00022AE2
		[NullableContext(1)]
		public StringLookAheadBuffer(string value)
		{
			this.value = value;
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x000248F1 File Offset: 0x00022AF1
		public int Length
		{
			get
			{
				return this.value.Length;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x000248FE File Offset: 0x00022AFE
		public bool EndOfInput
		{
			get
			{
				return this.IsOutside(this.Position);
			}
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0002490C File Offset: 0x00022B0C
		public char Peek(int offset)
		{
			int num = this.Position + offset;
			if (!this.IsOutside(num))
			{
				return this.value.get_Chars(num);
			}
			return '\0';
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00024939 File Offset: 0x00022B39
		private bool IsOutside(int index)
		{
			return index >= this.value.Length;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0002494C File Offset: 0x00022B4C
		public void Skip(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "The length must be positive.");
			}
			this.Position += length;
		}

		// Token: 0x04000444 RID: 1092
		[Nullable(1)]
		private readonly string value;
	}
}
