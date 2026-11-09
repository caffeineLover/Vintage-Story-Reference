using System;

namespace YamlDotNet.Core
{
	// Token: 0x020001C5 RID: 453
	internal sealed class RecursionLevel
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x00021993 File Offset: 0x0001FB93
		public int Maximum { get; }

		// Token: 0x060009F4 RID: 2548 RVA: 0x0002199B File Offset: 0x0001FB9B
		public RecursionLevel(int maximum)
		{
			this.Maximum = maximum;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x000219AA File Offset: 0x0001FBAA
		public void Increment()
		{
			if (!this.TryIncrement())
			{
				throw new MaximumRecursionLevelReachedException("Maximum level of recursion reached");
			}
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000219BF File Offset: 0x0001FBBF
		public bool TryIncrement()
		{
			if (this.current < this.Maximum)
			{
				this.current++;
				return true;
			}
			return false;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x000219E0 File Offset: 0x0001FBE0
		public void Decrement()
		{
			if (this.current == 0)
			{
				throw new InvalidOperationException("Attempted to decrement RecursionLevel to a negative value");
			}
			this.current--;
		}

		// Token: 0x0400041E RID: 1054
		private int current;
	}
}
