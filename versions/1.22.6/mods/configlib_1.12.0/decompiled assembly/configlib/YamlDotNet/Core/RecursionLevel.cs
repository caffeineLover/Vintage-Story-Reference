using System;

namespace YamlDotNet.Core
{
	// Token: 0x020001C9 RID: 457
	internal sealed class RecursionLevel
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x000222DB File Offset: 0x000204DB
		public int Maximum { get; }

		// Token: 0x06000A30 RID: 2608 RVA: 0x000222E3 File Offset: 0x000204E3
		public RecursionLevel(int maximum)
		{
			this.Maximum = maximum;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x000222F2 File Offset: 0x000204F2
		public void Increment()
		{
			if (!this.TryIncrement())
			{
				throw new MaximumRecursionLevelReachedException("Maximum level of recursion reached");
			}
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00022307 File Offset: 0x00020507
		public bool TryIncrement()
		{
			if (this.current < this.Maximum)
			{
				this.current++;
				return true;
			}
			return false;
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00022328 File Offset: 0x00020528
		public void Decrement()
		{
			if (this.current == 0)
			{
				throw new InvalidOperationException("Attempted to decrement RecursionLevel to a negative value");
			}
			this.current--;
		}

		// Token: 0x0400044C RID: 1100
		private int current;
	}
}
