using System;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F6 RID: 246
	public readonly struct AttackId
	{
		// Token: 0x06000C9E RID: 3230 RVA: 0x0003ED2F File Offset: 0x0003CF2F
		public AttackId(int itemId, int id)
		{
			this.ItemId = itemId;
			this.Id = id;
		}

		// Token: 0x04000623 RID: 1571
		public readonly int ItemId;

		// Token: 0x04000624 RID: 1572
		public readonly int Id;
	}
}
