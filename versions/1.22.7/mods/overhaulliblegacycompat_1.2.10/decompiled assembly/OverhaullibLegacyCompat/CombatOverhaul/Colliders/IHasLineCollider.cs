using System;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000171 RID: 369
	public interface IHasLineCollider
	{
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001262 RID: 4706
		LineSegmentCollider RelativeCollider { get; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001263 RID: 4707
		// (set) Token: 0x06001264 RID: 4708
		LineSegmentCollider InWorldCollider { get; set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001265 RID: 4709
		// (set) Token: 0x06001266 RID: 4710
		LineSegmentCollider PreviousInWorldCollider { get; set; }
	}
}
