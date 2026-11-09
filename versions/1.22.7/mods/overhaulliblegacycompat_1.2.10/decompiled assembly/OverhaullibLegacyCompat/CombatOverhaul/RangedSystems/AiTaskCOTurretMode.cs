using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using VSEssentialsMod.Entity.AI.Task;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000E1 RID: 225
	[NullableContext(1)]
	[Nullable(0)]
	public class AiTaskCOTurretMode : AiTaskTurretModeR
	{
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0003AD14 File Offset: 0x00038F14
		private AiTaskCOTurretModeConfig Config
		{
			get
			{
				return base.GetConfig<AiTaskCOTurretModeConfig>();
			}
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0003AD1C File Offset: 0x00038F1C
		public AiTaskCOTurretMode(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			this.baseConfig = AiTaskBaseR.LoadConfig<AiTaskCOTurretModeConfig>(entity, taskConfig, aiConfig);
			CombatOverhaulSystem system = entity.Api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			ProjectileSystemServer serverProjectileSystem = system.ServerProjectileSystem;
			if (serverProjectileSystem == null)
			{
				throw new Exception();
			}
			this.ProjectileSystem = serverProjectileSystem;
			this.LastEntityPosition = entity.Pos.XYZ.Clone();
			this.BallisticSolver = default(BallisticSolver);
			string @string = entity.World.Config.GetString("creatureHostility", null);
			EnumCreatureHostility creatureHostility;
			if (!(@string == "aggressive"))
			{
				if (!(@string == "passive"))
				{
					if (!(@string == "off"))
					{
						creatureHostility = 0;
					}
					else
					{
						creatureHostility = 2;
					}
				}
				else
				{
					creatureHostility = 1;
				}
			}
			else
			{
				creatureHostility = 0;
			}
			this.CreatureHostility = creatureHostility;
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0003ADE7 File Offset: 0x00038FE7
		protected ICoreAPI Api
		{
			get
			{
				return this.entity.Api;
			}
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0003ADF4 File Offset: 0x00038FF4
		protected override bool CheckEntityHostility(EntityPlayer target)
		{
			if (!this.Config.FriendlyTarget && this.AggressiveTargeting)
			{
				bool result;
				switch (this.CreatureHostility)
				{
				case 0:
					result = true;
					break;
				case 1:
					result = (this.emotionStatesBehavior == null || !this.IsInEmotionState(this.HostileEmotionStates));
					break;
				case 2:
					result = false;
					break;
				default:
					result = false;
					break;
				}
				return result;
			}
			return true;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0003AE5C File Offset: 0x0003905C
		protected override void SetOrAdjustDispersion()
		{
			if (this.targetEntity == null)
			{
				return;
			}
			if (this.targetEntity.EntityId == this.previousTargetId && (this.LastEntityPosition - this.entity.Pos.XYZ).Length() < (double)this.Config.MinDistanceToRezero)
			{
				this.currentYawDispersion = MathF.Max(this.Config.YawDispersionDeg, this.currentYawDispersion * this.Config.DispersionReductionRate);
				this.currentPitchDispersion = MathF.Max(this.Config.PitchDispersionDeg, this.currentPitchDispersion * this.Config.DispersionReductionRate);
			}
			else
			{
				float factor = GameMath.Clamp(((float)(this.LastEntityPosition - this.entity.Pos.XYZ).Length() - this.Config.MinDistanceToRezero) / (this.Config.MaxDistanceToRezero - this.Config.MinDistanceToRezero), 0f, 1f);
				this.currentYawDispersion = MathF.Max((this.Config.MaxYawDispersionDeg - this.Config.YawDispersionDeg) * factor + this.Config.YawDispersionDeg, this.Config.YawDispersionDeg);
				this.currentPitchDispersion = MathF.Max((this.Config.MaxPitchDispersionDeg - this.Config.PitchDispersionDeg) * factor + this.Config.PitchDispersionDeg, this.Config.PitchDispersionDeg);
				this.previousTargetId = this.targetEntity.EntityId;
			}
			this.LastEntityPosition.Set(this.entity.Pos.XYZ);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0003B004 File Offset: 0x00039204
		protected override void ShootProjectile()
		{
			if (this.targetEntity == null || this.Config.ProjectileStack == null)
			{
				return;
			}
			ProjectileBehavior collectibleBehavior = this.Config.ProjectileStack.Item.GetCollectibleBehavior<ProjectileBehavior>(true);
			ProjectileStats stats = (collectibleBehavior != null) ? collectibleBehavior.GetStats(this.Config.ProjectileStack) : null;
			if (stats == null)
			{
				ICoreAPI api = this.Api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(66, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to create projectile '");
				CollectibleObject collectible = this.Config.ProjectileStack.Collectible;
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>((collectible != null) ? collectible.Code : null);
				defaultInterpolatedStringHandler.AppendLiteral("': item is missing ProjectileBehavior");
				LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			IBallisticSolver.TargetData target = IBallisticSolver.GetTargetData(this.entity, this.targetEntity, this.Config.ProjectileGravityFactor, this.Config.ProjectileSpeed);
			IBallisticSolver.BallisticOutput ballisticData = this.BallisticSolver.SolveBallisticArc(target);
			IBallisticSolver.DispersionData dispersion = new IBallisticSolver.DispersionData(Angle.FromDegrees(this.currentYawDispersion), Angle.FromDegrees(this.currentPitchDispersion), this.randomFloat);
			for (int count = 0; count < this.Config.ProjectilesNumber; count++)
			{
				IBallisticSolver.BallisticOutput ballisticDataWithDispersion = this.BallisticSolver.ApplyDispersion(ballisticData, dispersion);
				ProjectileSpawnStats spawnStats = new ProjectileSpawnStats
				{
					ProducerEntityId = this.entity.EntityId,
					DamageMultiplier = this.Config.ProjectileDamageMultiplier,
					DamageTier = this.Config.ProjectileDamageTier,
					Position = target.ShooterPosition.ToOpenTK(),
					Velocity = ballisticDataWithDispersion.Velocity.ToOpenTK()
				};
				this.ProjectileSystem.Spawn(Guid.NewGuid(), stats, spawnStats, this.Config.ProjectileStack, null, this.entity, this.targetEntity);
			}
		}

		// Token: 0x04000555 RID: 1365
		protected Vec3d LastEntityPosition;

		// Token: 0x04000556 RID: 1366
		protected ProjectileSystemServer ProjectileSystem;

		// Token: 0x04000557 RID: 1367
		protected IBallisticSolver BallisticSolver;

		// Token: 0x04000558 RID: 1368
		protected EnumCreatureHostility CreatureHostility;

		// Token: 0x02000260 RID: 608
		[NullableContext(0)]
		protected enum EnumArcCalculationType
		{
			// Token: 0x04000C8D RID: 3213
			Any,
			// Token: 0x04000C8E RID: 3214
			Full,
			// Token: 0x04000C8F RID: 3215
			Horizontal,
			// Token: 0x04000C90 RID: 3216
			Vertical
		}
	}
}
