using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorHeldBag : CollectibleBehavior, IHeldBag, IAttachedInteractions, IAttachedListener
{
	public const int PacketIdBitShift = 11;

	public TagSet StorageTags;

	public const int defaultFlags = 189;

	public CollectibleBehaviorHeldBag(CollectibleObject collObj)
		: base(collObj)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		JToken tags = properties["tags"].Token;
		if (tags != null)
		{
			StorageTags = CollectibleTagSetConverter.ProxyInstance.ReadJson(tags);
		}
	}

	public void Clear(ItemStack backpackStack)
	{
		backpackStack.Attributes.GetTreeAttribute("backpack")["slots"] = new TreeAttribute();
	}

	public ItemStack[] GetContents(ItemStack bagstack, IWorldAccessor world)
	{
		ITreeAttribute backpackTree = bagstack.Attributes.GetTreeAttribute("backpack");
		if (backpackTree == null)
		{
			return null;
		}
		List<ItemStack> contents = new List<ItemStack>();
		foreach (KeyValuePair<string, IAttribute> item in backpackTree.GetTreeAttribute("slots").SortedCopy())
		{
			ItemStack cstack = (ItemStack)(item.Value?.GetValue());
			cstack?.ResolveBlockOrItem(world);
			contents.Add(cstack);
		}
		return contents.ToArray();
	}

	public virtual bool IsEmpty(ItemStack bagstack)
	{
		ITreeAttribute backpackTree = bagstack.Attributes.GetTreeAttribute("backpack");
		if (backpackTree == null)
		{
			return true;
		}
		foreach (KeyValuePair<string, IAttribute> item in backpackTree.GetTreeAttribute("slots"))
		{
			IItemStack stack = (IItemStack)(item.Value?.GetValue());
			if (stack != null && stack.StackSize > 0)
			{
				return false;
			}
		}
		return true;
	}

	public virtual int GetQuantitySlots(ItemStack bagstack)
	{
		if (bagstack == null || bagstack.Collectible.Attributes == null)
		{
			return 0;
		}
		return bagstack.Collectible.Attributes["backpack"]["quantitySlots"].AsInt();
	}

	public void Store(ItemStack bagstack, ItemSlotBagContent slot)
	{
		bagstack.Attributes.GetTreeAttribute("backpack").GetTreeAttribute("slots")["slot-" + slot.SlotIndex] = new ItemstackAttribute(slot.Itemstack);
	}

	public virtual string GetSlotBgColor(ItemStack bagstack)
	{
		return bagstack.ItemAttributes["backpack"]["slotBgColor"].AsString();
	}

	public virtual EnumItemStorageFlags GetStorageFlags(ItemStack bagstack)
	{
		return (EnumItemStorageFlags)bagstack.ItemAttributes["backpack"]["storageFlags"].AsInt(189);
	}

	public virtual TagSet GetStorageTags(ItemStack bagStack)
	{
		return StorageTags;
	}

	public List<ItemSlotBagContent> GetOrCreateSlots(ItemStack bagstack, InventoryBase parentinv, int bagIndex, IWorldAccessor world)
	{
		List<ItemSlotBagContent> bagContents = new List<ItemSlotBagContent>();
		string bgcolhex = GetSlotBgColor(bagstack);
		EnumItemStorageFlags flags = GetStorageFlags(bagstack);
		int quantitySlots = GetQuantitySlots(bagstack);
		TagSet storageTags = GetStorageTags(bagstack);
		ITreeAttribute stackBackpackTree = bagstack.Attributes.GetTreeAttribute("backpack");
		if (stackBackpackTree == null)
		{
			stackBackpackTree = new TreeAttribute();
			ITreeAttribute slotsTree = new TreeAttribute();
			for (int slotIndex = 0; slotIndex < quantitySlots; slotIndex++)
			{
				ItemSlotBagContent slot = new ItemSlotBagContent(parentinv, bagIndex, slotIndex, flags);
				slot.HexBackgroundColor = bgcolhex;
				slot.CanStoreTags = storageTags;
				bagContents.Add(slot);
				slotsTree["slot-" + slotIndex] = new ItemstackAttribute(null);
			}
			stackBackpackTree["slots"] = slotsTree;
			bagstack.Attributes["backpack"] = stackBackpackTree;
		}
		else
		{
			foreach (KeyValuePair<string, IAttribute> val in stackBackpackTree.GetTreeAttribute("slots"))
			{
				int slotIndex2 = val.Key.Split("-")[1].ToInt();
				ItemSlotBagContent slot2 = new ItemSlotBagContent(parentinv, bagIndex, slotIndex2, flags);
				slot2.HexBackgroundColor = bgcolhex;
				slot2.CanStoreTags = storageTags;
				if (val.Value?.GetValue() != null)
				{
					ItemstackAttribute attr = (ItemstackAttribute)val.Value;
					slot2.Itemstack = attr.value;
					slot2.Itemstack.ResolveBlockOrItem(world);
				}
				while (bagContents.Count <= slotIndex2)
				{
					bagContents.Add(null);
				}
				bagContents[slotIndex2] = slot2;
			}
		}
		return bagContents;
	}

	public void OnAttached(ItemSlot itemslot, int slotIndex, Entity toEntity, EntityAgent byEntity)
	{
	}

	public void OnDetached(ItemSlot itemslot, int slotIndex, Entity fromEntity, EntityAgent byEntity)
	{
		getOrCreateContainerWorkspace(slotIndex, fromEntity, null).Close((byEntity as EntityPlayer).Player);
	}

	public AttachedContainerWorkspace getOrCreateContainerWorkspace(int slotIndex, Entity onEntity, Action onRequireSave)
	{
		return ObjectCacheUtil.GetOrCreate(onEntity.Api, "att-cont-workspace-" + slotIndex + "-" + onEntity.EntityId + "-" + collObj.Id, () => new AttachedContainerWorkspace(onEntity, onRequireSave));
	}

	public AttachedContainerWorkspace getContainerWorkspace(int slotIndex, Entity onEntity)
	{
		return ObjectCacheUtil.TryGet<AttachedContainerWorkspace>(onEntity.Api, "att-cont-workspace-" + slotIndex + "-" + onEntity.EntityId + "-" + collObj.Id);
	}

	public virtual void OnInteract(ItemSlot bagSlot, int slotIndex, Entity onEntity, EntityAgent byEntity, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled, Action onRequireSave)
	{
		if (!(byEntity.MountedOn?.Controls ?? byEntity.Controls).CtrlKey)
		{
			handled = EnumHandling.PreventDefault;
			if (onEntity.Api.Side == EnumAppSide.Client)
			{
				getOrCreateContainerWorkspace(slotIndex, onEntity, onRequireSave).OnInteract(bagSlot, slotIndex, onEntity, byEntity, hitPosition);
			}
			onEntity.WatchedAttributes.SetDouble("lastDismountTotalHours", Math.Max(onEntity.WatchedAttributes.GetDouble("lastDismountTotalHours"), onEntity.World.Calendar.TotalHours - (double)onEntity.World.Calendar.HoursPerDay * 0.9));
			onEntity.GetBehavior<EntityBehaviorTaskAI>()?.TaskManager.StopTasks();
		}
	}

	public void OnReceivedClientPacket(ItemSlot bagSlot, int slotIndex, Entity onEntity, IServerPlayer player, int packetid, byte[] data, ref EnumHandling handled, Action onRequireSave)
	{
		int targetSlotIndex = packetid >> 11;
		if (slotIndex == targetSlotIndex && new Entity.CachedAccessPerms(onEntity, player).IsInteractingPlayerAllowedTo(EnumBlockAccessFlags.None, validatePickRange: true, "collectible behavior held bag"))
		{
			int first10Bits = 2047;
			packetid &= first10Bits;
			getOrCreateContainerWorkspace(slotIndex, onEntity, onRequireSave).OnReceivedClientPacket(player, packetid, data, bagSlot, slotIndex, ref handled);
		}
	}

	public bool OnTryAttach(ItemSlot itemslot, int slotIndex, Entity toEntity)
	{
		return true;
	}

	public bool OnTryDetach(ItemSlot itemslot, int slotIndex, Entity fromEntity)
	{
		return IsEmpty(itemslot.Itemstack);
	}

	public void OnEntityDespawn(ItemSlot itemslot, int slotIndex, Entity onEntity, EntityDespawnData despawn)
	{
		if (despawn.Reason == EnumDespawnReason.Death)
		{
			ItemStack[] contents = GetContents(itemslot.Itemstack, onEntity.World);
			if (contents != null)
			{
				ItemStack[] array = contents;
				foreach (ItemStack stack in array)
				{
					if (stack != null)
					{
						onEntity.World.SpawnItemEntity(stack, onEntity.Pos.XYZ);
					}
				}
			}
		}
		getContainerWorkspace(slotIndex, onEntity)?.OnDespawn(despawn);
	}

	public void OnEntityDeath(ItemSlot itemslot, int slotIndex, Entity onEntity, DamageSource damageSourceForDeath)
	{
	}
}
