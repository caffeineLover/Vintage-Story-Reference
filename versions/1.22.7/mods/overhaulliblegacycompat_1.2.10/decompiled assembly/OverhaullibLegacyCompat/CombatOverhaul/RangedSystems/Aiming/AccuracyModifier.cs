using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000E3 RID: 227
	[NullableContext(1)]
	[Nullable(0)]
	internal class AccuracyModifier
	{
		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0003B63C File Offset: 0x0003983C
		public float SecondsSinceAimStart
		{
			get
			{
				return (float)(this.Entity.World.ElapsedMilliseconds - this.AimStartMs) / 1000f;
			}
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0003B65C File Offset: 0x0003985C
		public AccuracyModifier(EntityAgent entity, ClientAimingSystem clientAimingSystem)
		{
			this.Entity = entity;
			this.ClientAimingSystem = clientAimingSystem;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0003B672 File Offset: 0x00039872
		public virtual void BeginAim()
		{
			this.AimStartMs = this.Entity.World.ElapsedMilliseconds;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0003B68A File Offset: 0x0003988A
		public virtual void EndAim()
		{
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0003B68C File Offset: 0x0003988C
		public virtual void OnHurt(float damage)
		{
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0003B68E File Offset: 0x0003988E
		public virtual void Update(float dt, AimingStats weaponStats)
		{
		}

		// Token: 0x04000561 RID: 1377
		protected EntityAgent Entity;

		// Token: 0x04000562 RID: 1378
		protected long AimStartMs;

		// Token: 0x04000563 RID: 1379
		protected ClientAimingSystem ClientAimingSystem;
	}
}
