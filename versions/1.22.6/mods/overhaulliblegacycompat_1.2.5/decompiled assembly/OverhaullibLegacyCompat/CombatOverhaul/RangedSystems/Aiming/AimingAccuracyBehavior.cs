using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000D7 RID: 215
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AimingAccuracyBehavior : EntityBehavior
	{
		// Token: 0x06000B1C RID: 2844 RVA: 0x0003A1DC File Offset: 0x000383DC
		public AimingAccuracyBehavior(Entity entity) : base(entity)
		{
			EntityPlayer player = entity as EntityPlayer;
			if (player == null)
			{
				throw new ArgumentException("Entity must be an EntityPlayer");
			}
			this._player = player;
			ICoreClientAPI clientApi = entity.Api as ICoreClientAPI;
			if (clientApi == null)
			{
				throw new ArgumentException("Must be client side");
			}
			EntityPlayer entityPlayer = entity as EntityPlayer;
			this._mainPlayer = (((entityPlayer != null) ? entityPlayer.PlayerUID : null) == clientApi.Settings.String["playeruid"]);
			CombatOverhaulSystem system = entity.Api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			ClientAimingSystem aimingSystem = system.AimingSystem;
			if (aimingSystem == null)
			{
				throw new Exception();
			}
			this._clientAimingSystem = aimingSystem;
			if (!this._mainPlayer)
			{
				return;
			}
			this._clientApi = clientApi;
			clientApi.Input.InWorldAction += new OnEntityAction(this.InWorldAction);
			this._modifiers.Add(new BaseAimingAccuracy(this._player, this._clientAimingSystem));
			this._modifiers.Add(new MovingAimingAccuracy(this._player, this._clientAimingSystem));
			this._modifiers.Add(new MountedAimingAccuracy(this._player, this._clientAimingSystem));
			this._modifiers.Add(new OnHurtAimingAccuracy(this._player, this._clientAimingSystem));
			this._player.Stats.Register("steadyAim", 2);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0003A344 File Offset: 0x00038544
		public override void OnGameTick(float deltaTime)
		{
			if (!this._mainPlayer || !this._isAiming)
			{
				return;
			}
			if (!this.entity.Alive)
			{
				this.StopAim();
				return;
			}
			if (!this._stats.AllowSprint)
			{
				this._player.CurrentControls &= -5;
				this._player.Controls.Sprint = false;
				this._player.ServerControls.Sprint = false;
			}
			for (int i = 0; i < this._modifiers.Count; i++)
			{
				this._modifiers[i].Update(deltaTime, this._stats);
			}
			this._clientAimingSystem.DriftMultiplier *= 1f / Math.Clamp(this.entity.Stats.GetBlended("steadyAim") * this.entity.Stats.GetBlended("steadyAim"), 0.25f, 4f);
			this._clientAimingSystem.TwitchMultiplier *= 1f / Math.Clamp(this.entity.Stats.GetBlended("steadyAim") * this.entity.Stats.GetBlended("steadyAim"), 0.25f, 4f);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0003A48C File Offset: 0x0003868C
		public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
		{
			if (!this._mainPlayer || damageSource.Type == 6)
			{
				return;
			}
			for (int i = 0; i < this._modifiers.Count; i++)
			{
				this._modifiers[i].OnHurt(damage);
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0003A4D4 File Offset: 0x000386D4
		public override string PropertyName()
		{
			return "CombatOverhaul:aimingAccuracy";
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0003A4DB File Offset: 0x000386DB
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			this.Dispose();
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0003A4EC File Offset: 0x000386EC
		public void StartAim(AimingStats stats)
		{
			this._stats = stats;
			this._isAiming = true;
			if (stats.MoveSpeedPenalty != 0f)
			{
				this.entity.Stats.Set("walkspeed", "CombatOverhaul:aiming", -(stats.MoveSpeedPenalty * this.entity.Stats.GetBlended("walkspeed")), false);
			}
			for (int i = 0; i < this._modifiers.Count; i++)
			{
				this._modifiers[i].BeginAim();
			}
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0003A574 File Offset: 0x00038774
		public void StopAim()
		{
			this._isAiming = false;
			this.entity.Stats.Set("walkspeed", "CombatOverhaul:aiming", 0f, false);
			for (int i = 0; i < this._modifiers.Count; i++)
			{
				this._modifiers[i].EndAim();
			}
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0003A5D0 File Offset: 0x000387D0
		private void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			if (this._mainPlayer && this._clientApi != null)
			{
				this._clientApi.Input.InWorldAction -= new OnEntityAction(this.InWorldAction);
			}
			if (this._isAiming)
			{
				this.StopAim();
			}
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0003A627 File Offset: 0x00038827
		private void InWorldAction(EnumEntityAction action, bool on, ref EnumHandling handled)
		{
			if (this._isAiming && !this._stats.AllowSprint && action == 6 && on)
			{
				handled = 2;
			}
		}

		// Token: 0x04000537 RID: 1335
		private readonly List<AccuracyModifier> _modifiers = new List<AccuracyModifier>();

		// Token: 0x04000538 RID: 1336
		private readonly EntityAgent _player;

		// Token: 0x04000539 RID: 1337
		private readonly bool _mainPlayer;

		// Token: 0x0400053A RID: 1338
		private readonly ClientAimingSystem _clientAimingSystem;

		// Token: 0x0400053B RID: 1339
		[Nullable(2)]
		private ICoreClientAPI _clientApi;

		// Token: 0x0400053C RID: 1340
		private AimingStats _stats = new AimingStats();

		// Token: 0x0400053D RID: 1341
		private bool _isAiming;

		// Token: 0x0400053E RID: 1342
		private bool _disposed;
	}
}
