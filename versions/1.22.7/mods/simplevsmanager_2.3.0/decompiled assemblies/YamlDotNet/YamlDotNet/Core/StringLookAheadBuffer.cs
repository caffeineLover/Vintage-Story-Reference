using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.ObjectPool;

namespace YamlDotNet.Core
{
	// Token: 0x020000DB RID: 219
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class StringLookAheadBuffer : ILookAheadBuffer, IResettable
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x000168B2 File Offset: 0x00014AB2
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x000168BA File Offset: 0x00014ABA
		public string Value { get; set; } = string.Empty;

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x000168C3 File Offset: 0x00014AC3
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x000168CB File Offset: 0x00014ACB
		public int Position { get; private set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x000168D4 File Offset: 0x00014AD4
		public int Length
		{
			get
			{
				return this.Value.Length;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x000168E1 File Offset: 0x00014AE1
		public bool EndOfInput
		{
			get
			{
				return this.IsOutside(this.Position);
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000168F0 File Offset: 0x00014AF0
		public char Peek(int offset)
		{
			int num = this.Position + offset;
			if (!this.IsOutside(num))
			{
				return this.Value.get_Chars(num);
			}
			return '\0';
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001691D File Offset: 0x00014B1D
		private bool IsOutside(int index)
		{
			return index >= this.Value.Length;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00016930 File Offset: 0x00014B30
		public void Skip(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "The length must be positive.");
			}
			this.Position += length;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00016954 File Offset: 0x00014B54
		public bool TryReset()
		{
			this.Position = 0;
			this.Value = string.Empty;
			return true;
		}
	}
}
