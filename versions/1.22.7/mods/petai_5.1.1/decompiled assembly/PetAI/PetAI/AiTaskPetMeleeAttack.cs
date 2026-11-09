using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000008 RID: 8
	public class AiTaskPetMeleeAttack : AiTaskMeleeAttack
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000029E4 File Offset: 0x00000BE4
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

		// Token: 0x06000018 RID: 24 RVA: 0x00002A69 File Offset: 0x00000C69
		public AiTaskPetMeleeAttack(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			this.isCommandable = taskConfig["isCommandable"].AsBool(false);
			this.damage *= PetConfig.Current.Difficulty.petDamageMultiplier;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002AA8 File Offset: 0x00000CA8
		public override bool IsTargetableEntity(Entity e, float range)
		{
			if (e == null || !e.Alive)
			{
				return false;
			}
			EntityBehaviorReceiveCommand behavior = this.entity.GetBehavior<EntityBehaviorReceiveCommand>();
			EnumAggressionLevel? aggressionLevel = (behavior != null) ? new EnumAggressionLevel?(behavior.AggressionLevel) : default(EnumAggressionLevel?);
			if (aggressionLevel.GetValueOrDefault() == EnumAggressionLevel.PASSIVE)
			{
				return false;
			}
			EntityBehaviorTameable tameable = this.entity.GetBehavior<EntityBehaviorTameable>();
			EntityPlayer player = e as EntityPlayer;
			if (player != null)
			{
				if (tameable != null && player.PlayerUID == tameable.OwnerId && tameable.Obedience > 0.5f)
				{
					return false;
				}
				if (!PetConfig.Current.PvpOn && (tameable == null || tameable.DomesticationLevel > DomesticationLevel.WILD) && player.PlayerUID != ((tameable != null) ? tameable.OwnerId : null))
				{
					return false;
				}
			}
			if (this.isCommandable && (aggressionLevel.GetValueOrDefault() == EnumAggressionLevel.PROTECTIVE || aggressionLevel.GetValueOrDefault() == EnumAggressionLevel.AGGRESSIVE))
			{
				EntityBehaviorGiveCommand behaviorGiveCommand = this.BehaviorGiveCommand;
				if (((behaviorGiveCommand != null) ? behaviorGiveCommand.Attacker : null) != e)
				{
					EntityBehaviorGiveCommand behaviorGiveCommand2 = this.BehaviorGiveCommand;
					if (((behaviorGiveCommand2 != null) ? behaviorGiveCommand2.Victim : null) != e)
					{
						goto IL_101;
					}
				}
				return this.CanSense(e, (double)range);
			}
			IL_101:
			if (this.attackedByEntity == e)
			{
				return this.CanSense(e, (double)range);
			}
			if (aggressionLevel.GetValueOrDefault() != EnumAggressionLevel.PROTECTIVE)
			{
				EnumAggressionLevel? enumAggressionLevel = aggressionLevel;
				EnumAggressionLevel enumAggressionLevel2 = EnumAggressionLevel.NEUTRAL;
				if (!(enumAggressionLevel.GetValueOrDefault() == enumAggressionLevel2 & enumAggressionLevel != null))
				{
					return base.IsTargetableEntity(e, range);
				}
			}
			return false;
		}

		// Token: 0x04000010 RID: 16
		private readonly bool isCommandable;

		// Token: 0x04000011 RID: 17
		private EntityBehaviorGiveCommand _behaviorGiveCommand;

		// Token: 0x04000012 RID: 18
		private long lastOwnerLookup;
	}
}
