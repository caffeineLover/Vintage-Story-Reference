using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000E2 RID: 226
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AimingAccuracyBehavior : EntityBehavior
	{
		// Token: 0x06000B78 RID: 2936 RVA: 0x0003B1CC File Offset: 0x000393CC
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

		// Token: 0x06000B79 RID: 2937 RVA: 0x0003B334 File Offset: 0x00039534
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

		// Token: 0x06000B7A RID: 2938 RVA: 0x0003B47C File Offset: 0x0003967C
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

		// Token: 0x06000B7B RID: 2939 RVA: 0x0003B4C4 File Offset: 0x000396C4
		public override string PropertyName()
		{
			return "CombatOverhaul:aimingAccuracy";
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0003B4CB File Offset: 0x000396CB
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			this.Dispose();
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0003B4DC File Offset: 0x000396DC
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

		// Token: 0x06000B7E RID: 2942 RVA: 0x0003B564 File Offset: 0x00039764
		public void StopAim()
		{
			this._isAiming = false;
			this.entity.Stats.Set("walkspeed", "CombatOverhaul:aiming", 0f, false);
			for (int i = 0; i < this._modifiers.Count; i++)
			{
				this._modifiers[i].EndAim();
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0003B5C0 File Offset: 0x000397C0
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

		// Token: 0x06000B80 RID: 2944 RVA: 0x0003B617 File Offset: 0x00039817
		private void InWorldAction(EnumEntityAction action, bool on, ref EnumHandling handled)
		{
			if (this._isAiming && !this._stats.AllowSprint && action == 6 && on)
			{
				handled = 2;
			}
		}

		// Token: 0x04000559 RID: 1369
		private readonly List<AccuracyModifier> _modifiers = new List<AccuracyModifier>();

		// Token: 0x0400055A RID: 1370
		private readonly EntityAgent _player;

		// Token: 0x0400055B RID: 1371
		private readonly bool _mainPlayer;

		// Token: 0x0400055C RID: 1372
		private readonly ClientAimingSystem _clientAimingSystem;

		// Token: 0x0400055D RID: 1373
		[Nullable(2)]
		private ICoreClientAPI _clientApi;

		// Token: 0x0400055E RID: 1374
		private AimingStats _stats = new AimingStats();

		// Token: 0x0400055F RID: 1375
		private bool _isAiming;

		// Token: 0x04000560 RID: 1376
		private bool _disposed;
	}
}
