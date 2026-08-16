using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010B RID: 267
	[NullableContext(1)]
	public interface ILocationalDamage
	{
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000D35 RID: 3381
		// (set) Token: 0x06000D36 RID: 3382
		Vector3d Position { get; set; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000D37 RID: 3383
		// (set) Token: 0x06000D38 RID: 3384
		string Collider { get; set; }
	}
}
