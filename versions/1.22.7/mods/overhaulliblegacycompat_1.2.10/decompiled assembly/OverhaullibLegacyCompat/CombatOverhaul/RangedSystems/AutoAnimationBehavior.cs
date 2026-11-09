using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C8 RID: 200
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AutoAnimationBehavior : EntityBehavior
	{
		// Token: 0x06000A51 RID: 2641 RVA: 0x00036BC8 File Offset: 0x00034DC8
		public AutoAnimationBehavior(Entity entity) : base(entity)
		{
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x00036BDC File Offset: 0x00034DDC
		// (set) Token: 0x06000A53 RID: 2643 RVA: 0x00036BE4 File Offset: 0x00034DE4
		public AutoAnimationConfig Config { get; private set; } = new AutoAnimationConfig();

		// Token: 0x06000A54 RID: 2644 RVA: 0x00036BED File Offset: 0x00034DED
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			base.Initialize(properties, attributes);
			this.Config = (attributes.AsObject<AutoAnimationConfig>(null) ?? new AutoAnimationConfig());
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00036C0D File Offset: 0x00034E0D
		public override void OnTesselated()
		{
			this.TryUpdateAnimation();
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00036C15 File Offset: 0x00034E15
		public override void OnGameTick(float deltaTime)
		{
			this.TryUpdateAnimation();
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00036C1D File Offset: 0x00034E1D
		public override string PropertyName()
		{
			return "combatOverhaulAutoAnimation";
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00036C24 File Offset: 0x00034E24
		private void TryUpdateAnimation()
		{
			if (this.entity.Api.Side != 2 || string.IsNullOrWhiteSpace(this.Config.Animation))
			{
				return;
			}
			bool flag;
			if (this.Config.StopWhenStuck)
			{
				ProjectileEntity projectileEntity = this.entity as ProjectileEntity;
				flag = (projectileEntity != null && projectileEntity.Stuck);
			}
			else
			{
				flag = false;
			}
			if (flag | (this.Config.MinimumSpeed > 0f && this.entity.SidedPos.Motion.Length() < (double)this.Config.MinimumSpeed))
			{
				if (this._started)
				{
					this.entity.AnimManager.StopAnimation(this.Config.Animation);
				}
				this._started = false;
				return;
			}
			if (!this._started)
			{
				this._started = this.entity.AnimManager.StartAnimation(this.Config.Animation);
			}
		}

		// Token: 0x040004C3 RID: 1219
		private bool _started;
	}
}
