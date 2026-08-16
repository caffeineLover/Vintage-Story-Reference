using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000117 RID: 279
	// (Invoke) Token: 0x06000DA2 RID: 3490
	public delegate void OnEntityReceiveDamageDelegate(ref float damage, DamageSource damageSource, ColliderTypes damageZone, [Nullable(2)] string collider);
}
