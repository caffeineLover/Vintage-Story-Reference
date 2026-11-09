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
	// Token: 0x0200014E RID: 334
	[NullableContext(1)]
	[Nullable(0)]
	public class GearEquipableBag : CollectibleBehavior, IHeldBag, IAttachedInteractions, IAttachedListener
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x0004FA2E File Offset: 0x0004DC2E
		// (set) Token: 0x06001048 RID: 4168 RVA: 0x0004FA36 File Offset: 0x0004DC36
		public SlotConfig DefaultSlotConfig { get; protected set; } = new SlotConfig(Array.Empty<string[]>(), Array.Empty<string[]>());

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x0004FA3F File Offset: 0x0004DC3F
		// (set) Token: 0x0600104A RID: 4170 RVA: 0x0004FA47 File Offset: 0x0004DC47
		public SlotConfig[] SlotConfigs { get; protected set; } = Array.Empty<SlotConfig>();

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x0004FA50 File Offset: 0x0004DC50
		// (set) Token: 0x0600104C RID: 4172 RVA: 0x0004FA58 File Offset: 0x0004DC58
		public int SlotsNumber { get; protected set; }

		// Token: 0x0600104D RID: 4173 RVA: 0x0004FA61 File Offset: 0x0004DC61
		public GearEquipableBag(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0004FA8C File Offset: 0x0004DC8C
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

		// Token: 0x0600104F RID: 4175 RVA: 0x0004FAE8 File Offset: 0x0004DCE8
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

		// Token: 0x06001050 RID: 4176 RVA: 0x0004FBB0 File Offset: 0x0004DDB0
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

		// Token: 0x06001051 RID: 4177 RVA: 0x0004FC14 File Offset: 0x0004DE14
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

		// Token: 0x06001052 RID: 4178 RVA: 0x0004FCC0 File Offset: 0x0004DEC0
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

		// Token: 0x06001053 RID: 4179 RVA: 0x0004FD50 File Offset: 0x0004DF50
		public virtual int GetQuantitySlots(ItemStack bagstack)
		{
			return this.SlotsNumber;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0004FD58 File Offset: 0x0004DF58
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

		// Token: 0x06001055 RID: 4181 RVA: 0x0004FDF5 File Offset: 0x0004DFF5
		public virtual string GetSlotBgColor(ItemStack bagstack)
		{
			return bagstack.ItemAttributes["backpack"]["slotBgColor"].AsString(null);
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0004FE17 File Offset: 0x0004E017
		public virtual EnumItemStorageFlags GetStorageFlags(ItemStack bagstack)
		{
			return 189;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0004FE1E File Offset: 0x0004E01E
		public virtual TagSet GetStorageTags(ItemStack bagStack)
		{
			return TagSet.Empty;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0004FE28 File Offset: 0x0004E028
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

		// Token: 0x06001059 RID: 4185 RVA: 0x000500A0 File Offset: 0x0004E2A0
		public void OnAttached(ItemSlot itemslot, int slotIndex, Entity toEntity, EntityAgent byEntity)
		{
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x000500A2 File Offset: 0x0004E2A2
		public void OnDetached(ItemSlot itemslot, int slotIndex, Entity fromEntity, EntityAgent byEntity)
		{
			this.getOrCreateContainerWorkspace(slotIndex, fromEntity, null).Close((byEntity as EntityPlayer).Player);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x000500C0 File Offset: 0x0004E2C0
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

		// Token: 0x0600105C RID: 4188 RVA: 0x00050154 File Offset: 0x0004E354
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

		// Token: 0x0600105D RID: 4189 RVA: 0x000501C0 File Offset: 0x0004E3C0
		public virtual void OnInteract(ItemSlot bagSlot, int slotIndex, Entity onEntity, EntityAgent byEntity, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled, Action onRequireSave)
		{
			IMountableSeat mountedOn = byEntity.MountedOn;
			if (!(((mountedOn != null) ? mountedOn.Controls : null) ?? byEntity.Controls).Sprint)
			{
				handled = 2;
				this.getOrCreateContainerWorkspace(slotIndex, onEntity, onRequireSave).OnInteract(bagSlot, slotIndex, onEntity, byEntity, hitPosition);
			}
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0005020C File Offset: 0x0004E40C
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

		// Token: 0x0600105F RID: 4191 RVA: 0x00050249 File Offset: 0x0004E449
		public bool OnTryAttach(ItemSlot itemslot, int slotIndex, Entity toEntity)
		{
			return true;
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0005024C File Offset: 0x0004E44C
		public bool OnTryDetach(ItemSlot itemslot, int slotIndex, Entity fromEntity)
		{
			return this.IsEmpty(itemslot.Itemstack);
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0005025A File Offset: 0x0004E45A
		public void OnEntityDespawn(ItemSlot itemslot, int slotIndex, Entity onEntity, EntityDespawnData despawn)
		{
			AttachedContainerWorkspace containerWorkspace = this.getContainerWorkspace(slotIndex, onEntity);
			if (containerWorkspace == null)
			{
				return;
			}
			containerWorkspace.OnDespawn(despawn);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00050270 File Offset: 0x0004E470
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

		// Token: 0x06001063 RID: 4195 RVA: 0x000502C0 File Offset: 0x0004E4C0
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

		// Token: 0x0400078D RID: 1933
		[Nullable(2)]
		protected ICoreAPI Api;

		// Token: 0x0400078E RID: 1934
		protected const int DefaultFlags = 189;
	}
}
