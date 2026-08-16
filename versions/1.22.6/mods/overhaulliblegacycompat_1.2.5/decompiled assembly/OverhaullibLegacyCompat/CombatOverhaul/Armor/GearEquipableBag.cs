using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000142 RID: 322
	[NullableContext(1)]
	[Nullable(0)]
	public class GearEquipableBag : CollectibleBehavior, IHeldBag, IAttachedInteractions, IAttachedListener
	{
		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x0004E126 File Offset: 0x0004C326
		// (set) Token: 0x06000FD4 RID: 4052 RVA: 0x0004E12E File Offset: 0x0004C32E
		public SlotConfig DefaultSlotConfig { get; protected set; } = new SlotConfig(Array.Empty<string[]>(), Array.Empty<string[]>());

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x0004E137 File Offset: 0x0004C337
		// (set) Token: 0x06000FD6 RID: 4054 RVA: 0x0004E13F File Offset: 0x0004C33F
		public SlotConfig[] SlotConfigs { get; protected set; } = Array.Empty<SlotConfig>();

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x0004E148 File Offset: 0x0004C348
		// (set) Token: 0x06000FD8 RID: 4056 RVA: 0x0004E150 File Offset: 0x0004C350
		public int SlotsNumber { get; protected set; }

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0004E159 File Offset: 0x0004C359
		public GearEquipableBag(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0004E184 File Offset: 0x0004C384
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.Api = api;
			this.DefaultSlotConfig.Resolve(api);
			EnumerableExtensions.Foreach<SlotConfig>(this.SlotConfigs, delegate(SlotConfig config)
			{
				config.Resolve(api);
			});
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0004E1E0 File Offset: 0x0004C3E0
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			SlotConfigJson defaultSlotConfigJson = properties.AsObject<SlotConfigJson>(null);
			JsonObject jsonObject = properties["slots"];
			SlotConfigJson[] slotConfigsJson = (jsonObject != null) ? jsonObject.AsObject<SlotConfigJson[]>(null) : null;
			if (defaultSlotConfigJson != null)
			{
				this.DefaultSlotConfig = defaultSlotConfigJson.ToConfig();
			}
			if (slotConfigsJson != null)
			{
				this.SlotConfigs = Enumerable.ToArray<SlotConfig>(Enumerable.Select<SlotConfigJson, SlotConfig>(slotConfigsJson, (SlotConfigJson config) => config.ToConfig()));
			}
			SlotConfig defaultSlotConfig = this.DefaultSlotConfig;
			int num = (defaultSlotConfig != null) ? defaultSlotConfig.SlotsNumber : 0;
			SlotConfig[] slotConfigs = this.SlotConfigs;
			int num2;
			if (slotConfigs == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = Enumerable.Sum(Enumerable.Select<SlotConfig, int>(slotConfigs, (SlotConfig config) => config.SlotsNumber));
			}
			this.SlotsNumber = num + num2;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0004E2A8 File Offset: 0x0004C4A8
		public void Clear(ItemStack backPackStack)
		{
			ITreeAttribute stackBackPackTree = backPackStack.Attributes.GetTreeAttribute("backpack");
			if (stackBackPackTree == null)
			{
				return;
			}
			TreeAttribute slots = new TreeAttribute();
			for (int slotIndex = 0; slotIndex < this.SlotsNumber; slotIndex++)
			{
				slots["slot-" + slotIndex.ToString()] = new ItemstackAttribute(null);
			}
			stackBackPackTree["slots"] = slots;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0004E30C File Offset: 0x0004C50C
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public ItemStack[] GetContents(ItemStack bagstack, IWorldAccessor world)
		{
			ITreeAttribute backPackTree = bagstack.Attributes.GetTreeAttribute("backpack");
			if (backPackTree == null)
			{
				return Array.Empty<ItemStack>();
			}
			List<ItemStack> contents = new List<ItemStack>();
			foreach (KeyValuePair<string, IAttribute> keyValuePair in backPackTree.GetTreeAttribute("slots").SortedCopy(false))
			{
				string text;
				IAttribute attribute;
				keyValuePair.Deconstruct(ref text, ref attribute);
				IAttribute attribute2 = attribute;
				ItemStack contentStack = (ItemStack)((attribute2 != null) ? attribute2.GetValue() : null);
				if (contentStack != null)
				{
					contentStack.ResolveBlockOrItem(world);
				}
				contents.Add(contentStack);
			}
			return contents.ToArray();
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0004E3B8 File Offset: 0x0004C5B8
		public virtual bool IsEmpty(ItemStack bagstack)
		{
			ITreeAttribute backPackTree = bagstack.Attributes.GetTreeAttribute("backpack");
			if (backPackTree == null)
			{
				return true;
			}
			foreach (KeyValuePair<string, IAttribute> val in backPackTree.GetTreeAttribute("slots"))
			{
				IAttribute value = val.Value;
				IItemStack stack = (IItemStack)((value != null) ? value.GetValue() : null);
				if (stack != null && stack.StackSize > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0004E448 File Offset: 0x0004C648
		public virtual int GetQuantitySlots(ItemStack bagstack)
		{
			return this.SlotsNumber;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0004E450 File Offset: 0x0004C650
		public void Store(ItemStack bagstack, ItemSlotBagContent slot)
		{
			ITreeAttribute treeAttribute = bagstack.Attributes.GetTreeAttribute("backpack");
			ITreeAttribute slotsTree = (treeAttribute != null) ? treeAttribute.GetTreeAttribute("slots") : null;
			if (slotsTree == null)
			{
				this.GetOrCreateSlots(bagstack, slot.Inventory, slot.BagIndex, this.Api.World);
				ITreeAttribute treeAttribute2 = bagstack.Attributes.GetTreeAttribute("backpack");
				slotsTree = ((treeAttribute2 != null) ? treeAttribute2.GetTreeAttribute("slots") : null);
				if (slotsTree == null)
				{
					return;
				}
			}
			slotsTree["slot-" + slot.SlotIndex.ToString()] = new ItemstackAttribute(slot.Itemstack);
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0004E4ED File Offset: 0x0004C6ED
		public virtual string GetSlotBgColor(ItemStack bagstack)
		{
			return bagstack.ItemAttributes["backpack"]["slotBgColor"].AsString(null);
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0004E50F File Offset: 0x0004C70F
		public virtual EnumItemStorageFlags GetStorageFlags(ItemStack bagstack)
		{
			return 189;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0004E516 File Offset: 0x0004C716
		public virtual TagSet GetStorageTags(ItemStack bagStack)
		{
			return TagSet.Empty;
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0004E520 File Offset: 0x0004C720
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public virtual List<ItemSlotBagContent> GetOrCreateSlots(ItemStack bagstack, InventoryBase parentinv, int bagIndex, IWorldAccessor world)
		{
			List<ItemSlotBagContent> bagContents = new List<ItemSlotBagContent>();
			EnumItemStorageFlags flags = 189;
			ITreeAttribute stackBackPackTree = bagstack.Attributes.GetTreeAttribute("backpack");
			if (stackBackPackTree == null)
			{
				stackBackPackTree = new TreeAttribute();
				ITreeAttribute slotsTree = new TreeAttribute();
				int slotIndex;
				for (slotIndex = 0; slotIndex < this.DefaultSlotConfig.SlotsNumber; slotIndex++)
				{
					ItemSlotBagContentWithWildcardMatch slot = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex, flags, bagstack, this.DefaultSlotConfig.SlotColor)
					{
						Config = this.DefaultSlotConfig
					};
					bagContents.Add(slot);
					slotsTree["slot-" + slotIndex.ToString()] = new ItemstackAttribute(null);
					if (this.DefaultSlotConfig.SlotsIcon != null)
					{
						slot.BackgroundIcon = this.DefaultSlotConfig.SlotsIcon;
					}
				}
				foreach (SlotConfig config in this.SlotConfigs)
				{
					int lastIndex = slotIndex + config.SlotsNumber;
					while (slotIndex < lastIndex)
					{
						ItemSlotBagContentWithWildcardMatch slot2 = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex, flags, bagstack, config.SlotColor)
						{
							Config = config
						};
						bagContents.Add(slot2);
						slotsTree["slot-" + slotIndex.ToString()] = new ItemstackAttribute(null);
						if (config.SlotsIcon != null)
						{
							slot2.BackgroundIcon = config.SlotsIcon;
						}
						slotIndex++;
					}
				}
				stackBackPackTree["slots"] = slotsTree;
				bagstack.Attributes["backpack"] = stackBackPackTree;
			}
			else
			{
				foreach (KeyValuePair<string, IAttribute> val in stackBackPackTree.GetTreeAttribute("slots"))
				{
					int slotIndex2 = int.Parse(val.Key.Split("-", 0)[1]);
					SlotConfig config2 = this.GetSlotConfig(slotIndex2);
					ItemSlotBagContentWithWildcardMatch slot3 = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex2, flags, bagstack, config2.SlotColor)
					{
						Config = config2
					};
					if (config2.SlotsIcon != null)
					{
						slot3.BackgroundIcon = config2.SlotsIcon;
					}
					IAttribute value = val.Value;
					if (((value != null) ? value.GetValue() : null) != null)
					{
						ItemstackAttribute attr = (ItemstackAttribute)val.Value;
						slot3.Itemstack = attr.value;
						slot3.Itemstack.ResolveBlockOrItem(world);
					}
					while (bagContents.Count <= slotIndex2)
					{
						bagContents.Add(null);
					}
					bagContents[slotIndex2] = slot3;
				}
			}
			return bagContents;
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0004E798 File Offset: 0x0004C998
		public void OnAttached(ItemSlot itemslot, int slotIndex, Entity toEntity, EntityAgent byEntity)
		{
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0004E79A File Offset: 0x0004C99A
		public void OnDetached(ItemSlot itemslot, int slotIndex, Entity fromEntity, EntityAgent byEntity)
		{
			this.getOrCreateContainerWorkspace(slotIndex, fromEntity, null).Close((byEntity as EntityPlayer).Player);
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0004E7B8 File Offset: 0x0004C9B8
		public AttachedContainerWorkspace getOrCreateContainerWorkspace(int slotIndex, Entity onEntity, Action onRequireSave)
		{
			return ObjectCacheUtil.GetOrCreate<AttachedContainerWorkspace>(onEntity.Api, string.Concat(new string[]
			{
				"att-cont-workspace-",
				slotIndex.ToString(),
				"-",
				onEntity.EntityId.ToString(),
				"-",
				this.collObj.Id.ToString()
			}), () => new AttachedContainerWorkspace(onEntity, onRequireSave));
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0004E84C File Offset: 0x0004CA4C
		public AttachedContainerWorkspace getContainerWorkspace(int slotIndex, Entity onEntity)
		{
			return ObjectCacheUtil.TryGet<AttachedContainerWorkspace>(onEntity.Api, string.Concat(new string[]
			{
				"att-cont-workspace-",
				slotIndex.ToString(),
				"-",
				onEntity.EntityId.ToString(),
				"-",
				this.collObj.Id.ToString()
			}));
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0004E8B8 File Offset: 0x0004CAB8
		public virtual void OnInteract(ItemSlot bagSlot, int slotIndex, Entity onEntity, EntityAgent byEntity, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled, Action onRequireSave)
		{
			IMountableSeat mountedOn = byEntity.MountedOn;
			if (!(((mountedOn != null) ? mountedOn.Controls : null) ?? byEntity.Controls).Sprint)
			{
				handled = 2;
				this.getOrCreateContainerWorkspace(slotIndex, onEntity, onRequireSave).OnInteract(bagSlot, slotIndex, onEntity, byEntity, hitPosition);
			}
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0004E904 File Offset: 0x0004CB04
		public void OnReceivedClientPacket(ItemSlot bagSlot, int slotIndex, Entity onEntity, IServerPlayer player, int packetid, byte[] data, ref EnumHandling handled, Action onRequireSave)
		{
			int targetSlotIndex = packetid >> 11;
			if (slotIndex != targetSlotIndex)
			{
				return;
			}
			int first10Bits = 2047;
			packetid &= first10Bits;
			this.getOrCreateContainerWorkspace(slotIndex, onEntity, onRequireSave).OnReceivedClientPacket(player, packetid, data, bagSlot, slotIndex, ref handled);
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0004E941 File Offset: 0x0004CB41
		public bool OnTryAttach(ItemSlot itemslot, int slotIndex, Entity toEntity)
		{
			return true;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0004E944 File Offset: 0x0004CB44
		public bool OnTryDetach(ItemSlot itemslot, int slotIndex, Entity fromEntity)
		{
			return this.IsEmpty(itemslot.Itemstack);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0004E952 File Offset: 0x0004CB52
		public void OnEntityDespawn(ItemSlot itemslot, int slotIndex, Entity onEntity, EntityDespawnData despawn)
		{
			AttachedContainerWorkspace containerWorkspace = this.getContainerWorkspace(slotIndex, onEntity);
			if (containerWorkspace == null)
			{
				return;
			}
			containerWorkspace.OnDespawn(despawn);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0004E968 File Offset: 0x0004CB68
		public void OnEntityDeath(ItemSlot itemslot, int slotIndex, Entity onEntity, DamageSource damageSourceForDeath)
		{
			foreach (ItemStack stack in this.GetContents(itemslot.Itemstack, onEntity.World))
			{
				if (stack != null)
				{
					onEntity.World.SpawnItemEntity(stack, onEntity.Pos.XYZ, null);
				}
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0004E9B8 File Offset: 0x0004CBB8
		protected virtual SlotConfig GetSlotConfig(int index)
		{
			if (index < this.DefaultSlotConfig.SlotsNumber)
			{
				return this.DefaultSlotConfig;
			}
			int previousIndex = this.DefaultSlotConfig.SlotsNumber;
			for (int configIndex = 0; configIndex < this.SlotConfigs.Length; configIndex++)
			{
				previousIndex += this.SlotConfigs[configIndex].SlotsNumber;
				if (index < previousIndex)
				{
					return this.SlotConfigs[configIndex];
				}
			}
			return this.DefaultSlotConfig;
		}

		// Token: 0x04000767 RID: 1895
		[Nullable(2)]
		protected ICoreAPI Api;

		// Token: 0x04000768 RID: 1896
		protected const int DefaultFlags = 189;
	}
}
