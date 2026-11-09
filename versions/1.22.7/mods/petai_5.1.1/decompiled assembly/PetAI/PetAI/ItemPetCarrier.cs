using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace PetAI
{
	// Token: 0x0200001A RID: 26
	public class ItemPetCarrier : Item
	{
		// Token: 0x060000AA RID: 170 RVA: 0x000064A4 File Offset: 0x000046A4
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			Entity pet = (entitySel != null) ? entitySel.Entity : null;
			EntityBehaviorTameable tameable = (pet != null) ? pet.GetBehavior<EntityBehaviorTameable>() : null;
			JsonObject canPickUp = this.Attributes["canPickUp"];
			handling = 1;
			if (pet != null && byEntity.Controls.Sneak && canPickUp[pet.Code.Path].Exists)
			{
				EntityAgent entityAgent;
				if (tameable == null)
				{
					entityAgent = null;
				}
				else
				{
					IPlayer cachedOwner = tameable.CachedOwner;
					entityAgent = ((cachedOwner != null) ? cachedOwner.Entity : null);
				}
				if (entityAgent == byEntity && tameable != null && tameable.DomesticationLevel == DomesticationLevel.DOMESTICATED && this.Variant["type"] == "empty")
				{
					string newVariant = canPickUp[pet.Code.Path]["type"].AsString(null);
					if (canPickUp[pet.Code.Path]["appendTextureIndex"].AsBool(false))
					{
						newVariant += (entitySel.Entity.WatchedAttributes.GetInt("textureIndex", 0) + 1).ToString();
					}
					slot.Itemstack = new ItemStack(byEntity.World.GetItem(slot.Itemstack.Item.CodeWithVariant("type", newVariant)).Id, 1, slot.Itemstack.StackSize, (TreeAttribute)slot.Itemstack.Attributes, byEntity.World);
					slot.Itemstack.Attributes.MergeTree(PetUtil.EntityToTree(pet));
					entitySel.Entity.Die(3, null);
					slot.MarkDirty();
					return;
				}
			}
			if (byEntity.Controls.Sneak && this.Variant["type"] != "empty")
			{
				pet = PetUtil.EntityFromTree(slot.Itemstack.Attributes, byEntity.World);
				pet.Pos.SetPos(byEntity.Pos);
				byEntity.World.SpawnEntity(pet);
				slot.Itemstack = new ItemStack(byEntity.World.GetItem(slot.Itemstack.Item.CodeWithVariant("type", "empty")).Id, 1, slot.Itemstack.StackSize, (TreeAttribute)slot.Itemstack.Attributes, byEntity.World);
				slot.MarkDirty();
				return;
			}
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000671F File Offset: 0x0000491F
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "petai:pickup-pet",
					HotKeyCode = "sneak",
					MouseButton = 2
				}
			};
		}
	}
}
