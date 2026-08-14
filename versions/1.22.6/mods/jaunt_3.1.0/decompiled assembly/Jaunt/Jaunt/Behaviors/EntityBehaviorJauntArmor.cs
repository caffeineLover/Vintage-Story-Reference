using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Jaunt.Behaviors
{
	// Token: 0x0200000F RID: 15
	public class EntityBehaviorJauntArmor : EntityBehavior
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00004376 File Offset: 0x00002576
		public EntityBehaviorJauntArmor(Entity entity) : base(entity)
		{
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000437F File Offset: 0x0000257F
		public override string PropertyName()
		{
			return "jaunt:armor";
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004386 File Offset: 0x00002586
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			this.plateSound = new AssetLocation("game", "sounds/wearable/plate1");
			base.Initialize(properties, attributes);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000043A5 File Offset: 0x000025A5
		public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled)
		{
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000043A8 File Offset: 0x000025A8
		private void onAttachmentToggled()
		{
			this.entity.MarkShapeModified();
			if (this.entity.World.Side == 1)
			{
				this.entity.World.PlaySoundAt(this.plateSound, this.entity, null, true, 16f, 1f);
			}
			this.entity.WatchedAttributes.MarkAllDirty();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000440C File Offset: 0x0000260C
		public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
		{
			ITreeAttribute tree = this.entity.WatchedAttributes.GetTreeAttribute("ownedby");
			if (tree != null)
			{
				string OwnerId = tree.GetString("uid", null);
				EntityPlayer player = (damageSource.CauseEntity ?? damageSource.SourceEntity) as EntityPlayer;
				if (player != null && player.PlayerUID == OwnerId)
				{
					damage = 0f;
					damageSource.CauseEntity = null;
					damageSource.SourceEntity = null;
				}
			}
			EntityBehaviorHealth behaviorHealth = this.entity.GetBehavior<EntityBehaviorHealth>();
			if (behaviorHealth != null && behaviorHealth.Health < 0f)
			{
				behaviorHealth.Health = GameMath.Clamp(behaviorHealth.Health, 0f, behaviorHealth.MaxHealth);
			}
			Entity entity = this.entity;
			EntityBehaviorRideableAccessories ebr = (entity != null) ? entity.GetBehavior<EntityBehaviorRideableAccessories>() : null;
			if (damageSource.SourceEntity != null && damageSource.Type != 6 && ebr != null)
			{
				int index = 0;
				foreach (ItemSlot slot in ebr.Inventory)
				{
					if (!slot.Empty && slot.Itemstack.Collectible.Attributes["isMountArmor"].AsBool(false))
					{
						float durabilityLeft = (float)slot.Itemstack.Collectible.GetRemainingDurability(slot.Itemstack);
						int remainingDurability = slot.Itemstack.Collectible.GetRemainingDurability(slot.Itemstack);
						float maxDurability = (float)slot.Itemstack.Collectible.GetMaxDurability(slot.Itemstack);
						float num = slot.Itemstack.Collectible.Attributes["damageReduction"].AsFloat(0f);
						float durabilityPercent = (maxDurability > 0f) ? (durabilityLeft / maxDurability) : 0f;
						float effectiveReduction = num * durabilityPercent;
						int num2 = (int)Math.Ceiling((double)damage);
						damage *= 1f - effectiveReduction;
						int damagetakenInt = (int)Math.Ceiling((double)damage);
						int damageInt = num2 - damagetakenInt;
						int newdurability = remainingDurability - damageInt;
						slot.Itemstack.Collectible.SetDurability(slot.Itemstack, newdurability);
						this.onAttachmentToggled();
						slot.MarkDirty();
						ebr.Inventory.MarkSlotDirty(index);
					}
					index++;
				}
			}
		}

		// Token: 0x04000050 RID: 80
		private AssetLocation plateSound;
	}
}
