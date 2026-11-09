using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000006 RID: 6
	public class AiTaskFollowMaster : AiTaskStayCloseToEntity
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000024B4 File Offset: 0x000006B4
		public AiTaskFollowMaster(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			if (taskConfig["command"] != null)
			{
				this.commandName = taskConfig["command"].AsString("followmaster");
			}
			this.allowTeleport &= PetConfig.Current.AllowTeleport;
			this.moveSpeed *= PetConfig.Current.Difficulty.petSpeedMultiplier;
			this.animMeta.AnimationSpeed *= PetConfig.Current.Difficulty.petSpeedMultiplier;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002554 File Offset: 0x00000754
		public override bool ShouldExecute()
		{
			if (!this.PreconditionsSatisfied())
			{
				return false;
			}
			EntityBehaviorTameable entityBehaviorTameable = this.tameable;
			Entity targetEntity;
			if (entityBehaviorTameable == null)
			{
				targetEntity = null;
			}
			else
			{
				IPlayer cachedOwner = entityBehaviorTameable.CachedOwner;
				targetEntity = ((cachedOwner != null) ? cachedOwner.Entity : null);
			}
			this.targetEntity = targetEntity;
			if (this.tameable == null)
			{
				this.tameable = this.entity.GetBehavior<EntityBehaviorTameable>();
			}
			return this.targetEntity != null && this.targetEntity.Alive && !this.targetEntity.ShouldDespawn && this.targetEntity.IsInteractable && this.entity.GetBehavior<EntityBehaviorReceiveCommand>().ComplexCommand == this.commandName && this.targetEntity.Pos.SquareDistanceTo(this.entity.Pos.X, this.entity.Pos.Y, this.entity.Pos.Z) > this.maxDistance * this.maxDistance;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002648 File Offset: 0x00000848
		public override void OnNoPath(Vec3d target)
		{
			if (this.allowTeleport && this.targetEntity.Pos.SquareDistanceTo(this.entity.Pos) > this.teleportAfterRange * this.teleportAfterRange)
			{
				base.tryTeleport();
				return;
			}
			this.stuck = false;
			this.pathTraverser.WalkTowards(this.targetEntity.Pos.XYZ, this.moveSpeed, this.maxDistance, new Action(this.OnGoalReached), new Action(base.OnStuck), 0);
		}

		// Token: 0x04000009 RID: 9
		private readonly string commandName = "followmaster";

		// Token: 0x0400000A RID: 10
		private EntityBehaviorTameable tameable;
	}
}
