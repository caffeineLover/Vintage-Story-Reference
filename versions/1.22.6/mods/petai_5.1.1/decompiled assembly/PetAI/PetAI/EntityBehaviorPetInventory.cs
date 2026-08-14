using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000011 RID: 17
	public class EntityBehaviorPetInventory : EntityBehaviorRideableAccessories
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00003999 File Offset: 0x00001B99
		public EntityBehaviorPetInventory(Entity entity) : base(entity)
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000039A2 File Offset: 0x00001BA2
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			base.Initialize(properties, attributes);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000039AC File Offset: 0x00001BAC
		public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled)
		{
			EntityPlayer player = byEntity as EntityPlayer;
			if (player != null)
			{
				string playerUID = player.PlayerUID;
				EntityBehaviorTameable behavior = this.entity.GetBehavior<EntityBehaviorTameable>();
				if (playerUID == ((behavior != null) ? behavior.OwnerId : null))
				{
					base.OnInteract(byEntity, itemslot, hitPosition, mode, ref handled);
				}
			}
			handled = 0;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000039F8 File Offset: 0x00001BF8
		public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player, ref EnumHandling handled)
		{
			string playerUID = player.PlayerUID;
			EntityBehaviorTameable behavior = this.entity.GetBehavior<EntityBehaviorTameable>();
			if (playerUID == ((behavior != null) ? behavior.OwnerId : null))
			{
				return base.GetInteractionHelp(world, es, player, ref handled);
			}
			return Array.Empty<WorldInteraction>();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003A30 File Offset: 0x00001C30
		public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
		{
			if (damageSource.SourceEntity != null && damageSource.Type != 6)
			{
				foreach (ItemSlot slot in this.Inventory)
				{
					if (!slot.Empty)
					{
						damage *= 1f - slot.Itemstack.Item.Attributes["damageReduction"].AsFloat(0f);
					}
				}
			}
		}
	}
}
