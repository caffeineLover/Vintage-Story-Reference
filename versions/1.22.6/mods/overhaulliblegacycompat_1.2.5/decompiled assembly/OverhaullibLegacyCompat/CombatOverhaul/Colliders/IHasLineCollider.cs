using System;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000165 RID: 357
	public interface IHasLineCollider
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060011EE RID: 4590
		LineSegmentCollider RelativeCollider { get; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060011EF RID: 4591
		// (set) Token: 0x060011F0 RID: 4592
		LineSegmentCollider InWorldCollider { get; set; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060011F1 RID: 4593
		// (set) Token: 0x060011F2 RID: 4594
		LineSegmentCollider PreviousInWorldCollider { get; set; }
	}
}
