using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000009 RID: 9
	public class AiTaskPetSeekEntity : AiTaskSeekEntity
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002BF8 File Offset: 0x00000DF8
		private EntityBehaviorGiveCommand BehaviorGiveCommand
		{
			get
			{
				if (this._behaviorGiveCommand == null && this.lastOwnerLookup + 5000L < this.entity.World.ElapsedMilliseconds)
				{
					this.lastOwnerLookup = this.entity.World.ElapsedMilliseconds;
					EntityBehaviorTameable behavior = this.entity.GetBehavior<EntityBehaviorTameable>();
					EntityBehaviorGiveCommand behaviorGiveCommand;
					if (behavior == null)
					{
						behaviorGiveCommand = null;
					}
					else
					{
						IPlayer cachedOwner = behavior.CachedOwner;
						if (cachedOwner == null)
						{
							behaviorGiveCommand = null;
						}
						else
						{
							EntityPlayer entity = cachedOwner.Entity;
							behaviorGiveCommand = ((entity != null) ? entity.GetBehavior<EntityBehaviorGiveCommand>() : null);
						}
					}
					this._behaviorGiveCommand = behaviorGiveCommand;
				}
				return this._behaviorGiveCommand;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002C80 File Offset: 0x00000E80
		public AiTaskPetSeekEntity(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			this.isCommandable = taskConfig["isCommandable"].AsBool(false);
			this.moveSpeed *= PetConfig.Current.Difficulty.petSpeedMultiplier;
			this.animMeta.AnimationSpeed *= PetConfig.Current.Difficulty.petSpeedMultiplier;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002CEC File Offset: 0x00000EEC
		public override bool ShouldExecute()
		{
			EntityBehaviorReceiveCommand behavior = this.entity.GetBehavior<EntityBehaviorReceiveCommand>();
			EnumAggressionLevel aggressionLevel = (behavior != null) ? behavior.AggressionLevel : EnumAggressionLevel.AGGRESSIVE;
			long elapsedMs = this.entity.World.ElapsedMilliseconds;
			if (this.lastCheck + 500L < elapsedMs)
			{
				base.NowSeekRange = base.getSeekRange();
				this.lastCheck = elapsedMs;
				if (aggressionLevel == EnumAggressionLevel.PASSIVE)
				{
					return false;
				}
				Entity targetEntity = this.targetEntity;
				if (targetEntity == null || !targetEntity.Alive || !this.CanSense(this.targetEntity, (double)base.NowSeekRange))
				{
					this.targetEntity = null;
				}
				if (this.targetEntity == null)
				{
					if (aggressionLevel != EnumAggressionLevel.NEUTRAL && this.isCommandable)
					{
						EntityBehaviorGiveCommand behaviorGiveCommand = this.BehaviorGiveCommand;
						Entity ownerAttackedBy = (behaviorGiveCommand != null) ? behaviorGiveCommand.Attacker : null;
						if (ownerAttackedBy != null && ownerAttackedBy.Alive && this.CanSense(ownerAttackedBy, (double)base.NowSeekRange))
						{
							this.targetEntity = ownerAttackedBy;
						}
						EntityBehaviorGiveCommand behaviorGiveCommand2 = this.BehaviorGiveCommand;
						Entity ownerAttacks = (behaviorGiveCommand2 != null) ? behaviorGiveCommand2.Victim : null;
						if (ownerAttacks != null && ownerAttacks.Alive && this.CanSense(ownerAttacks, (double)base.NowSeekRange))
						{
							this.targetEntity = ownerAttacks;
						}
					}
					Entity attackedByEntity = this.attackedByEntity;
					if (attackedByEntity != null && attackedByEntity.Alive && this.CanSense(this.attackedByEntity, (double)base.NowSeekRange))
					{
						this.targetEntity = this.attackedByEntity;
					}
				}
				if (this.CanSense(this.targetEntity, (double)base.NowSeekRange))
				{
					this.targetPos = this.targetEntity.Pos.XYZ;
					return true;
				}
			}
			if (aggressionLevel == EnumAggressionLevel.AGGRESSIVE && this.lastSearch + 5000L < elapsedMs)
			{
				this.lastSearch = elapsedMs;
				this.targetEntity = this.partitionUtil.GetNearestInteractableEntity(this.entity.Pos.XYZ, (double)base.NowSeekRange, (Entity e) => this.IsTargetableEntity(e, base.NowSeekRange));
				if (this.IsTargetableEntity(this.targetEntity, base.NowSeekRange))
				{
					this.targetPos = this.targetEntity.Pos.XYZ;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002EE0 File Offset: 0x000010E0
		public override bool IsTargetableEntity(Entity e, float range)
		{
			if (e == null)
			{
				return false;
			}
			EntityBehaviorTameable tameable = this.entity.GetBehavior<EntityBehaviorTameable>();
			EntityPlayer player = e as EntityPlayer;
			if (player != null)
			{
				if (player.PlayerUID == ((tameable != null) ? tameable.OwnerId : null) && tameable != null && tameable.Obedience > 0.5f)
				{
					return false;
				}
				if (!PetConfig.Current.PvpOn && (tameable == null || tameable.DomesticationLevel > DomesticationLevel.WILD) && player.PlayerUID != ((tameable != null) ? tameable.OwnerId : null))
				{
					return false;
				}
			}
			return e.Pos.SquareDistanceTo(this.entity.Pos) <= range * range && base.IsTargetableEntity(e, range);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002F90 File Offset: 0x00001190
		public override bool CanSense(Entity e, double range)
		{
			return e != null && base.CanSense(e, range) && (double)e.Pos.SquareDistanceTo(this.entity.Pos) <= range * range;
		}

		// Token: 0x04000013 RID: 19
		private readonly bool isCommandable;

		// Token: 0x04000014 RID: 20
		private long lastCheck;

		// Token: 0x04000015 RID: 21
		private long lastSearch;

		// Token: 0x04000016 RID: 22
		private EntityBehaviorGiveCommand _behaviorGiveCommand;

		// Token: 0x04000017 RID: 23
		private long lastOwnerLookup;
	}
}
