using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000164 RID: 356
	[NullableContext(1)]
	public interface IHasCollider
	{
		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060011EB RID: 4587
		ICollider RelativeCollider { get; }

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060011EC RID: 4588
		// (set) Token: 0x060011ED RID: 4589
		ICollider InWorldCollider { get; set; }
	}
}
