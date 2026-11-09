using System;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x02000101 RID: 257
	public readonly struct AttackId
	{
		// Token: 0x06000CFA RID: 3322 RVA: 0x0003FD1F File Offset: 0x0003DF1F
		public AttackId(int itemId, int id)
		{
			this.ItemId = itemId;
			this.Id = id;
		}

		// Token: 0x04000645 RID: 1605
		public readonly int ItemId;

		// Token: 0x04000646 RID: 1606
		public readonly int Id;
	}
}
