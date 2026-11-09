using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Jaunt.Behaviors
{
	// Token: 0x02000012 RID: 18
	public class EntityBehaviorJauntRevivable : EntityBehavior
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00004C00 File Offset: 0x00002E00
		public EntityBehaviorJauntRevivable(Entity entity) : base(entity)
		{
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004C14 File Offset: 0x00002E14
		public override string PropertyName()
		{
			return "jaunt:revivable";
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004C1C File Offset: 0x00002E1C
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			JsonItemStack jstack = attributes["reviveStack"].AsObject<JsonItemStack>(null);
			this.reviveHealth = attributes["reviveHealth"].AsFloat(0.1f);
			this.reviveSound = attributes["reviveSound"].AsString("sounds/effect/portal");
			if (jstack != null)
			{
				jstack.Resolve(this.entity.Api.World, string.Format("entity {0} jaunt revive stack", this.entity.Code), true);
				this.reviveStack = jstack.ResolvedItemstack;
			}
			base.Initialize(properties, attributes);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004CB8 File Offset: 0x00002EB8
		public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled)
		{
			if (!byEntity.Controls.ShiftKey || (itemslot.Empty && this.reviveStack != null))
			{
				return;
			}
			EntityBehaviorHealth ebh = this.entity.GetBehavior<EntityBehaviorHealth>();
			Entity entity = this.entity;
			EntityBehaviorMortallyWoundable ebm = (entity != null) ? entity.GetBehavior<EntityBehaviorMortallyWoundable>() : null;
			if (ebh != null && ebh.Health < ebh.MaxHealth && (this.reviveStack == null || (this.reviveStack != null && itemslot.Itemstack.Satisfies(this.reviveStack))))
			{
				if (this.entity.World.Side == 1)
				{
					this.entity.World.PlaySoundAt(new AssetLocation(this.reviveSound), this.entity, null, true, 16f, 1f);
				}
				if (this.reviveStack != null)
				{
					itemslot.TakeOut(1);
					itemslot.MarkDirty();
				}
				if (!this.entity.Alive)
				{
					this.entity.Revive();
				}
				IAnimationManager animManager = this.entity.AnimManager;
				if (animManager != null)
				{
					animManager.StopAnimation("dead");
				}
				if (ebm != null)
				{
					this.recover();
				}
				ebh.Health = ebh.MaxHealth * Math.Clamp(this.reviveHealth, 0f, 1f);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004DF8 File Offset: 0x00002FF8
		private void recover()
		{
			Entity entity = this.entity;
			((entity != null) ? entity.GetBehavior<EntityBehaviorMortallyWoundable>() : null).HealthState = 1;
			this.entity.WatchedAttributes.SetFloat("regenSpeed", 1f);
			IAnimationManager animManager = this.entity.AnimManager;
			if (animManager != null)
			{
				animManager.StopAnimation("wounded-idle");
			}
			IAnimationManager animManager2 = this.entity.AnimManager;
			if (animManager2 != null)
			{
				animManager2.StopAnimation("wounded-resthead");
			}
			IAnimationManager animManager3 = this.entity.AnimManager;
			if (animManager3 != null)
			{
				animManager3.StartAnimation("wounded-stand");
			}
			IAnimationManager animManager4 = this.entity.AnimManager;
			if (animManager4 != null)
			{
				animManager4.StartAnimation("idle");
			}
			EntityBehaviorTaskAI behavior = this.entity.GetBehavior<EntityBehaviorTaskAI>();
			List<IAiTask> list;
			if (behavior == null)
			{
				list = null;
			}
			else
			{
				AiTaskManager taskManager = behavior.TaskManager;
				list = ((taskManager != null) ? taskManager.AllTasks : null);
			}
			List<IAiTask> tasks = list;
			if (tasks != null)
			{
				foreach (IAiTask aiTask in tasks)
				{
					AiTaskBaseTargetable aiTaskBaseTargetable = aiTask as AiTaskBaseTargetable;
					if (aiTaskBaseTargetable != null)
					{
						aiTaskBaseTargetable.ClearAttacker();
					}
				}
			}
			EntityBehaviorEmotionStates behavior2 = this.entity.GetBehavior<EntityBehaviorEmotionStates>();
			if (behavior2 == null)
			{
				return;
			}
			behavior2.ClearStates();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004F2C File Offset: 0x0000312C
		public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player, ref EnumHandling handled)
		{
			if (this.entity.Alive)
			{
				return null;
			}
			if (!this.entity.Alive && this.reviveStack != null)
			{
				return new WorldInteraction[]
				{
					new WorldInteraction
					{
						ActionLangCode = "entityinteraction-revive",
						Itemstacks = new ItemStack[]
						{
							this.reviveStack
						},
						HotKeyCode = "shift",
						MouseButton = 2
					}
				};
			}
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "entityinteraction-revive",
					HotKeyCode = "shift",
					MouseButton = 2
				}
			};
		}

		// Token: 0x04000059 RID: 89
		private ItemStack reviveStack;

		// Token: 0x0400005A RID: 90
		public float reviveHealth = 1f;

		// Token: 0x0400005B RID: 91
		public string reviveSound;
	}
}
