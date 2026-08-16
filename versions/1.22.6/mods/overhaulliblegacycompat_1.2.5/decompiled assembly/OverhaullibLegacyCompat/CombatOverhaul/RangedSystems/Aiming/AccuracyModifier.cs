using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000D8 RID: 216
	[NullableContext(1)]
	[Nullable(0)]
	internal class AccuracyModifier
	{
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x0003A64C File Offset: 0x0003884C
		public float SecondsSinceAimStart
		{
			get
			{
				return (float)(this.Entity.World.ElapsedMilliseconds - this.AimStartMs) / 1000f;
			}
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0003A66C File Offset: 0x0003886C
		public AccuracyModifier(EntityAgent entity, ClientAimingSystem clientAimingSystem)
		{
			this.Entity = entity;
			this.ClientAimingSystem = clientAimingSystem;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0003A682 File Offset: 0x00038882
		public virtual void BeginAim()
		{
			this.AimStartMs = this.Entity.World.ElapsedMilliseconds;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0003A69A File Offset: 0x0003889A
		public virtual void EndAim()
		{
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0003A69C File Offset: 0x0003889C
		public virtual void OnHurt(float damage)
		{
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0003A69E File Offset: 0x0003889E
		public virtual void Update(float dt, AimingStats weaponStats)
		{
		}

		// Token: 0x0400053F RID: 1343
		protected EntityAgent Entity;

		// Token: 0x04000540 RID: 1344
		protected long AimStartMs;

		// Token: 0x04000541 RID: 1345
		protected ClientAimingSystem ClientAimingSystem;
	}
}
