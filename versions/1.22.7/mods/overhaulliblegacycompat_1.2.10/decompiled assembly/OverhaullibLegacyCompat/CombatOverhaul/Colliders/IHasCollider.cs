using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000170 RID: 368
	[NullableContext(1)]
	public interface IHasCollider
	{
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x0600125F RID: 4703
		ICollider RelativeCollider { get; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001260 RID: 4704
		// (set) Token: 0x06001261 RID: 4705
		ICollider InWorldCollider { get; set; }
	}
}
