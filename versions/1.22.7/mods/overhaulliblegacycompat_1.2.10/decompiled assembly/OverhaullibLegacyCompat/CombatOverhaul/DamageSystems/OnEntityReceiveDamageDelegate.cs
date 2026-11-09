using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000122 RID: 290
	// (Invoke) Token: 0x06000DFE RID: 3582
	public delegate void OnEntityReceiveDamageDelegate(ref float damage, DamageSource damageSource, ColliderTypes damageZone, [Nullable(2)] string collider);
}
