using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000116 RID: 278
	[NullableContext(1)]
	public interface ILocationalDamage
	{
		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000D91 RID: 3473
		// (set) Token: 0x06000D92 RID: 3474
		Vector3d Position { get; set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000D93 RID: 3475
		// (set) Token: 0x06000D94 RID: 3476
		string Collider { get; set; }
	}
}
