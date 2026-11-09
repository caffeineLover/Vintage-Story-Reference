using System;

namespace YamlDotNet.Core
{
	// Token: 0x020000D6 RID: 214
	internal sealed class RecursionLevel
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0001398D File Offset: 0x00011B8D
		public int Maximum { get; }

		// Token: 0x0600056C RID: 1388 RVA: 0x00013995 File Offset: 0x00011B95
		public RecursionLevel(int maximum)
		{
			this.Maximum = maximum;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x000139A4 File Offset: 0x00011BA4
		public void Increment()
		{
			if (!this.TryIncrement())
			{
				throw new MaximumRecursionLevelReachedException("Maximum level of recursion reached");
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000139B9 File Offset: 0x00011BB9
		public bool TryIncrement()
		{
			if (this.current < this.Maximum)
			{
				this.current++;
				return true;
			}
			return false;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000139DA File Offset: 0x00011BDA
		public void Decrement()
		{
			if (this.current == 0)
			{
				throw new InvalidOperationException("Attempted to decrement RecursionLevel to a negative value");
			}
			this.current--;
		}

		// Token: 0x04000195 RID: 405
		private int current;
	}
}
