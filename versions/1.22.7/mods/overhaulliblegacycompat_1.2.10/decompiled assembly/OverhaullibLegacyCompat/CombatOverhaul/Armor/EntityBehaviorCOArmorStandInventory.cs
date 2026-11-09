using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200012E RID: 302
	[NullableContext(1)]
	[Nullable(0)]
	public class EntityBehaviorCOArmorStandInventory : EntityBehavior
	{
		// Token: 0x06000E90 RID: 3728 RVA: 0x00046988 File Offset: 0x00044B88
		private static bool IsToolOrToolrackItem(ItemStack stack)
		{
			if (stack.Collectible.Tool == null)
			{
				JsonObject itemAttributes = stack.ItemAttributes;
				return itemAttributes != null && itemAttributes["toolrackTransform"].Exists;
			}
			return true;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x000469BC File Offset: 0x00044BBC
		private static bool IsArmorStandArmor(ItemStack stack)
		{
			IArmor armor;
			return ItemSlotCharacter.IsDressType(stack, 13) || ItemSlotCharacter.IsDressType(stack, 12) || ItemSlotCharacter.IsDressType(stack, 14) || ArmorStandArmorSlot.TryGetArmor(stack, out armor);
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x000469F1 File Offset: 0x00044BF1
		public InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x000469F9 File Offset: 0x00044BF9
		public string InventoryClassName
		{
			get
			{
				return "inventory";
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00046A00 File Offset: 0x00044C00
		public EntityBehaviorCOArmorStandInventory(Entity entity) : base(entity)
		{
			EntityAgent entityAgent = entity as EntityAgent;
			if (entityAgent == null)
			{
				throw new Exception("Armor stand inventory behavior requires EntityAgent");
			}
			this.eagent = entityAgent;
			this.inventory = new ArmorStandArmorInventory(null, null);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00046A31 File Offset: 0x00044C31
		public override string PropertyName()
		{
			return "inventory";
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00046A38 File Offset: 0x00044C38
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			base.Initialize(properties, attributes);
			this.inventory.LateInitialize("armorstand-" + this.entity.EntityId.ToString(), this.entity.World.Api);
			this.inventory.SlotModified += new Action<int>(this.OnInventorySlotModified);
			this.LoadInventory();
			this.eagent.WatchedAttributes.RegisterModifiedListener("coArmorStandInv", new Action(this.WearablesModified));
			this.eagent.WatchedAttributes.RegisterModifiedListener("wearablesInv", new Action(this.LegacyWearablesModified));
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00046AE1 File Offset: 0x00044CE1
		private void WearablesModified()
		{
			this.LoadInventory();
			this.eagent.MarkShapeModified();
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00046AF4 File Offset: 0x00044CF4
		private void LegacyWearablesModified()
		{
			if (this.eagent.WatchedAttributes.GetTreeAttribute("coArmorStandInv") != null)
			{
				return;
			}
			this.LoadInventory();
			this.eagent.MarkShapeModified();
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00046B1F File Offset: 0x00044D1F
		private void OnInventorySlotModified(int slotId)
		{
			if (this.loadingInventory)
			{
				return;
			}
			this.SaveInventory(false);
			this.eagent.MarkShapeModified();
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00046B3C File Offset: 0x00044D3C
		private void LoadInventory()
		{
			ITreeAttribute coTree = this.eagent.WatchedAttributes.GetTreeAttribute("coArmorStandInv");
			ITreeAttribute legacyTree = this.eagent.WatchedAttributes.GetTreeAttribute("wearablesInv");
			ITreeAttribute tree = coTree ?? legacyTree;
			this.hasPersistentTree |= (coTree != null);
			bool loadedFromLegacyTree = coTree == null && legacyTree != null;
			bool shouldPersistLoadedLegacyTree = this.entity.World.Side == 1 && loadedFromLegacyTree && EntityBehaviorCOArmorStandInventory.LooksLikeVanillaArmorStandInventory(legacyTree);
			this.loadingInventory = true;
			try
			{
				this.inventory.FromTreeAttributes(tree ?? new TreeAttribute());
			}
			finally
			{
				this.loadingInventory = false;
			}
			if (shouldPersistLoadedLegacyTree && !this.inventory.Empty)
			{
				this.SaveInventory(true);
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00046C08 File Offset: 0x00044E08
		[NullableContext(2)]
		private bool MigrateVanillaArmorStandContentsIfNeeded(ITreeAttribute sourceTree)
		{
			if (sourceTree == null || this.eagent.WatchedAttributes.GetBool("coArmorStandMigratedVanillaInv", false))
			{
				return false;
			}
			if (!EntityBehaviorCOArmorStandInventory.LooksLikeVanillaArmorStandInventory(sourceTree))
			{
				return false;
			}
			bool changed = false;
			bool hadLegacyContent = false;
			bool remainingLegacyContent = false;
			int maxLegacySlot = Math.Min(5, this.inventory.Count);
			for (int slotId = 0; slotId < maxLegacySlot; slotId++)
			{
				ItemSlot sourceSlot = this.inventory[slotId];
				ItemStack stack = sourceSlot.Itemstack;
				if (((stack != null) ? stack.Collectible : null) == null || stack.StackSize <= 0)
				{
					if (stack != null)
					{
						sourceSlot.Itemstack = null;
						sourceSlot.MarkDirty();
						changed = true;
					}
				}
				else
				{
					hadLegacyContent = true;
					if (this.TryMigrateVanillaArmorStandSlot(slotId, sourceSlot))
					{
						changed = true;
					}
					if (!sourceSlot.Empty)
					{
						remainingLegacyContent = true;
					}
				}
			}
			if (!hadLegacyContent || !remainingLegacyContent)
			{
				this.eagent.WatchedAttributes.SetBool("coArmorStandMigratedVanillaInv", true);
				this.eagent.WatchedAttributes.MarkPathDirty("coArmorStandMigratedVanillaInv");
				changed = true;
			}
			return changed;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00046CFC File Offset: 0x00044EFC
		private static bool LooksLikeVanillaArmorStandInventory(ITreeAttribute sourceTree)
		{
			int qslots = sourceTree.GetInt("qslots", 0);
			if (qslots > 0 && qslots <= 5)
			{
				return true;
			}
			ITreeAttribute slotsTree = sourceTree.GetTreeAttribute("slots");
			if (slotsTree == null)
			{
				return false;
			}
			bool hasLegacySlot = false;
			for (int slotId = 0; slotId < 5; slotId++)
			{
				if (slotsTree.GetItemstack(slotId.ToString(), null) != null)
				{
					hasLegacySlot = true;
					break;
				}
			}
			if (!hasLegacySlot)
			{
				return false;
			}
			int scanCount = ArmorInventory._totalSlotsNumber + 6;
			for (int slotId2 = 5; slotId2 < scanCount; slotId2++)
			{
				if (slotsTree.GetItemstack(slotId2.ToString(), null) != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00046D88 File Offset: 0x00044F88
		private bool TryMigrateVanillaArmorStandSlot(int legacySlotId, ItemSlot sourceSlot)
		{
			ItemStack stack = sourceSlot.Itemstack;
			if (((stack != null) ? stack.Collectible : null) == null || stack.StackSize <= 0)
			{
				return false;
			}
			IArmor armor;
			if (ArmorStandArmorSlot.TryGetArmor(stack, out armor) && armor != null)
			{
				return this.TryMigrateVanillaArmorIntoArmorSlot(legacySlotId, sourceSlot, armor);
			}
			if (EntityBehaviorCOArmorStandInventory.IsToolOrToolrackItem(stack))
			{
				return this.TryMigrateVanillaItemIntoHandSlot(legacySlotId, sourceSlot);
			}
			return this.TryMigrateVanillaItemIntoStorageSlot(sourceSlot);
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00046DE4 File Offset: 0x00044FE4
		private bool TryMigrateVanillaArmorIntoArmorSlot(int legacySlotId, ItemSlot sourceSlot, IArmor armor)
		{
			ItemStack stack = sourceSlot.Itemstack;
			if (((stack != null) ? stack.Collectible : null) == null || stack.StackSize <= 0)
			{
				return false;
			}
			foreach (int index in this.OrderedArmorInsertSlots(legacySlotId, armor.ArmorType))
			{
				if (index >= 0 && index < this.inventory.Count)
				{
					ArmorStandArmorSlot targetSlot = this.inventory[index] as ArmorStandArmorSlot;
					if (targetSlot != null && targetSlot.Empty && targetSlot.ArmorType.Intersect(armor.ArmorType) && this.inventory.CanAcceptArmorStack(stack, targetSlot, sourceSlot))
					{
						EntityBehaviorCOArmorStandInventory.MoveWholeStack(sourceSlot, targetSlot);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00046EB8 File Offset: 0x000450B8
		private bool TryMigrateVanillaItemIntoHandSlot(int legacySlotId, ItemSlot sourceSlot)
		{
			if (legacySlotId == 4)
			{
				return EntityBehaviorCOArmorStandInventory.TryMoveWholeStack(sourceSlot, this.inventory[ArmorInventory._totalSlotsNumber + 1]) || EntityBehaviorCOArmorStandInventory.TryMoveWholeStack(sourceSlot, this.inventory[ArmorInventory._totalSlotsNumber]);
			}
			if (legacySlotId == 3)
			{
				return EntityBehaviorCOArmorStandInventory.TryMoveWholeStack(sourceSlot, this.inventory[ArmorInventory._totalSlotsNumber]) || EntityBehaviorCOArmorStandInventory.TryMoveWholeStack(sourceSlot, this.inventory[ArmorInventory._totalSlotsNumber + 1]);
			}
			return EntityBehaviorCOArmorStandInventory.TryMoveWholeStack(sourceSlot, this.inventory[ArmorInventory._totalSlotsNumber]) || EntityBehaviorCOArmorStandInventory.TryMoveWholeStack(sourceSlot, this.inventory[ArmorInventory._totalSlotsNumber + 1]);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00046F68 File Offset: 0x00045168
		private bool TryMigrateVanillaItemIntoStorageSlot(ItemSlot sourceSlot)
		{
			for (int index = ArmorInventory._totalSlotsNumber + 2; index < this.inventory.Count; index++)
			{
				if (EntityBehaviorCOArmorStandInventory.TryMoveWholeStack(sourceSlot, this.inventory[index]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00046FA8 File Offset: 0x000451A8
		private static bool TryMoveWholeStack(ItemSlot sourceSlot, ItemSlot targetSlot)
		{
			if (!targetSlot.Empty)
			{
				return false;
			}
			ItemStack stack = sourceSlot.Itemstack;
			if (((stack != null) ? stack.Collectible : null) == null || stack.StackSize <= 0)
			{
				return false;
			}
			EntityBehaviorCOArmorStandInventory.MoveWholeStack(sourceSlot, targetSlot);
			return true;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00046FE8 File Offset: 0x000451E8
		private static void MoveWholeStack(ItemSlot sourceSlot, ItemSlot targetSlot)
		{
			ItemStack stack = sourceSlot.Itemstack;
			sourceSlot.Itemstack = null;
			targetSlot.Itemstack = stack;
			sourceSlot.MarkDirty();
			targetSlot.MarkDirty();
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x00047018 File Offset: 0x00045218
		public bool HandleInteract(EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
		{
			if (!this.eagent.Alive || mode == null)
			{
				return true;
			}
			if (this.entity.World.Side == 2)
			{
				return true;
			}
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			IPlayer player = (entityPlayer != null) ? entityPlayer.Player : null;
			if (player != null && !byEntity.World.Claims.TryAccess(player, this.entity.Pos.AsBlockPos, 2))
			{
				player.InventoryManager.ActiveHotbarSlot.MarkDirty();
				this.entity.WatchedAttributes.MarkAllDirty();
				return true;
			}
			if (mode != 1)
			{
				return false;
			}
			ItemStack itemstack = slot.Itemstack;
			if (((itemstack != null) ? itemstack.Collectible : null) is ItemWrench)
			{
				this.CyclePose();
				return true;
			}
			int? num;
			if (player == null)
			{
				num = default(int?);
			}
			else
			{
				EntitySelection currentEntitySelection = player.CurrentEntitySelection;
				num = ((currentEntitySelection != null) ? new int?(currentEntitySelection.SelectionBoxIndex) : default(int?));
			}
			int? num2 = num;
			int selectionBoxIndex = num2.GetValueOrDefault(-1);
			if (slot.Empty)
			{
				if (this.TryTakeSelected(byEntity, slot, selectionBoxIndex, hitPosition))
				{
					this.eagent.MarkShapeModified();
					return true;
				}
				if (byEntity.Controls.ShiftKey && this.inventory.Empty)
				{
					Item standItem = byEntity.World.GetItem(this.entity.Code);
					if (standItem != null)
					{
						ItemStack stack = new ItemStack(standItem, 1);
						if (!byEntity.TryGiveItemStack(stack))
						{
							byEntity.World.SpawnItemEntity(stack, this.entity.ServerPos.XYZ, null);
						}
						byEntity.World.Logger.Audit("{0} Took 1x{1} from Armor Stand at {2}.", new object[]
						{
							byEntity.GetName(),
							stack.Collectible.Code,
							this.entity.ServerPos.AsBlockPos
						});
					}
					this.eagent.Die(0, null);
					return true;
				}
				return true;
			}
			else
			{
				ItemStack heldStack = slot.Itemstack;
				if (((heldStack != null) ? heldStack.Collectible : null) == null || heldStack.StackSize <= 0)
				{
					slot.Itemstack = null;
					slot.MarkDirty();
					return true;
				}
				if (EntityBehaviorCOArmorStandInventory.IsToolOrToolrackItem(heldStack))
				{
					AssetLocation code = heldStack.Collectible.Code;
					if (this.TryPutToolIntoHandSlots(slot, selectionBoxIndex, hitPosition) > 0)
					{
						this.eagent.MarkShapeModified();
						byEntity.World.Logger.Audit("{0} Put 1x{1} onto Armor Stand at {2}.", new object[]
						{
							byEntity.GetName(),
							code,
							this.entity.ServerPos.AsBlockPos
						});
					}
					return true;
				}
				if (!EntityBehaviorCOArmorStandInventory.IsArmorStandArmor(heldStack))
				{
					return true;
				}
				AssetLocation armorCode = heldStack.Collectible.Code;
				if (this.TryPutArmorIntoArmorSlots(slot, selectionBoxIndex, hitPosition) > 0)
				{
					this.eagent.MarkShapeModified();
					byEntity.World.Logger.Audit("{0} Put 1x{1} onto Armor Stand at {2}.", new object[]
					{
						byEntity.GetName(),
						armorCode,
						this.entity.ServerPos.AsBlockPos
					});
				}
				return true;
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x000472FC File Offset: 0x000454FC
		private void CyclePose()
		{
			string[] poses = new string[]
			{
				"idle",
				"lefthandup",
				"righthandup",
				"twohandscross"
			};
			int curPose = this.entity.WatchedAttributes.GetInt("curPose", 0);
			this.eagent.AnimManager.StopAnimation(poses[curPose]);
			curPose = (curPose + 1) % poses.Length;
			this.eagent.WatchedAttributes.SetInt("curPose", curPose);
			this.eagent.AnimManager.StartAnimation(new AnimationMetaData
			{
				Animation = poses[curPose],
				Code = poses[curPose]
			}.Init());
			this.eagent.WatchedAttributes.MarkPathDirty("curPose");
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x000473BC File Offset: 0x000455BC
		private void SaveInventory(bool force = false)
		{
			if (this.entity.World.Side != 1 || (!force && !this.hasPersistentTree && this.inventory.Empty))
			{
				return;
			}
			TreeAttribute tree = new TreeAttribute();
			this.inventory.ToTreeAttributes(tree);
			this.eagent.WatchedAttributes["coArmorStandInv"] = tree;
			this.eagent.WatchedAttributes.MarkPathDirty("coArmorStandInv");
			this.hasPersistentTree = true;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0004743C File Offset: 0x0004563C
		public bool TryTakeSelected(EntityAgent byEntity, ItemSlot handslot, int selectionBoxIndex, Vec3d hitPosition)
		{
			if (!handslot.Empty)
			{
				return false;
			}
			foreach (int legacyIndex in this.OrderedVanillaArmorStandSlots(selectionBoxIndex, hitPosition))
			{
				int moved;
				if (this.TryTakeFromSlotOrRawTree(byEntity, handslot, legacyIndex, out moved))
				{
					this.SaveInventory(false);
					this.eagent.MarkShapeModified();
					ILogger logger = byEntity.World.Logger;
					string text = "{0} Took {1}x{2} from vanilla Armor Stand slot at {3}.";
					object[] array = new object[4];
					array[0] = byEntity.GetName();
					array[1] = moved;
					int num = 2;
					ItemStack itemstack = handslot.Itemstack;
					array[num] = ((itemstack != null) ? itemstack.Collectible.Code : null);
					array[3] = this.entity.ServerPos.AsBlockPos;
					logger.Audit(text, array);
					return true;
				}
			}
			foreach (int index in this.OrderedCoSlots(selectionBoxIndex, hitPosition))
			{
				int moved2;
				if (index >= 0 && index < this.inventory.Count && index >= 5 && this.TryMoveWholeStackToEmptyHand(this.inventory[index], handslot, out moved2))
				{
					this.SaveInventory(false);
					this.eagent.MarkShapeModified();
					ILogger logger2 = byEntity.World.Logger;
					string text2 = "{0} Took {1}x{2} from Armor Stand at {3}.";
					object[] array2 = new object[4];
					array2[0] = byEntity.GetName();
					array2[1] = moved2;
					int num2 = 2;
					ItemStack itemstack2 = handslot.Itemstack;
					array2[num2] = ((itemstack2 != null) ? itemstack2.Collectible.Code : null);
					array2[3] = this.entity.ServerPos.AsBlockPos;
					logger2.Audit(text2, array2);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00047604 File Offset: 0x00045804
		private bool TryTakeFromSlotOrRawTree(EntityAgent byEntity, ItemSlot handslot, int slotIndex, out int moved)
		{
			moved = 0;
			if (slotIndex < 0 || slotIndex >= 5 || !handslot.Empty)
			{
				return false;
			}
			if (slotIndex < this.inventory.Count && this.TryMoveWholeStackToEmptyHand(this.inventory[slotIndex], handslot, out moved))
			{
				return true;
			}
			ITreeAttribute treeAttribute = this.eagent.WatchedAttributes.GetTreeAttribute("coArmorStandInv");
			string rawTreeKey = "coArmorStandInv";
			ITreeAttribute slotsTree = (treeAttribute != null) ? treeAttribute.GetTreeAttribute("slots") : null;
			if (slotsTree == null)
			{
				ITreeAttribute treeAttribute2 = this.eagent.WatchedAttributes.GetTreeAttribute("wearablesInv");
				rawTreeKey = "wearablesInv";
				slotsTree = ((treeAttribute2 != null) ? treeAttribute2.GetTreeAttribute("slots") : null);
			}
			if (slotsTree == null)
			{
				return false;
			}
			string key = slotIndex.ToString();
			ItemStack stack = slotsTree.GetItemstack(key, null);
			if (stack == null)
			{
				return false;
			}
			if (this.entity.World != null)
			{
				stack.ResolveBlockOrItem(this.entity.World);
			}
			if (stack.Collectible == null || stack.StackSize <= 0)
			{
				slotsTree.RemoveAttribute(key);
				this.eagent.WatchedAttributes.MarkPathDirty(rawTreeKey);
				return false;
			}
			moved = stack.StackSize;
			slotsTree.RemoveAttribute(key);
			if (slotIndex < this.inventory.Count)
			{
				this.inventory[slotIndex].Itemstack = null;
			}
			handslot.Itemstack = stack;
			handslot.MarkDirty();
			this.eagent.WatchedAttributes.MarkPathDirty(rawTreeKey);
			if (rawTreeKey == "coArmorStandInv")
			{
				this.hasPersistentTree = true;
			}
			return true;
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00047774 File Offset: 0x00045974
		public int TryPutToolIntoHandSlots(ItemSlot handslot, int selectionBoxIndex, Vec3d hitPosition)
		{
			int targetedSlot = this.VanillaSlotFromInteraction(selectionBoxIndex, hitPosition);
			int effectiveSelection = (targetedSlot >= 0 && targetedSlot <= 4) ? targetedSlot : selectionBoxIndex;
			int moved;
			if (effectiveSelection == 3)
			{
				moved = handslot.TryPutInto(this.entity.World, this.inventory[ArmorInventory._totalSlotsNumber], 1);
				if (moved > 0)
				{
					this.SaveInventory(true);
				}
				return moved;
			}
			if (effectiveSelection == 4)
			{
				moved = handslot.TryPutInto(this.entity.World, this.inventory[ArmorInventory._totalSlotsNumber + 1], 1);
				if (moved > 0)
				{
					this.SaveInventory(true);
				}
				return moved;
			}
			moved = handslot.TryPutInto(this.entity.World, this.inventory[ArmorInventory._totalSlotsNumber], 1);
			if (moved == 0)
			{
				moved = handslot.TryPutInto(this.entity.World, this.inventory[ArmorInventory._totalSlotsNumber + 1], 1);
			}
			if (moved > 0)
			{
				this.SaveInventory(true);
			}
			return moved;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00047858 File Offset: 0x00045A58
		public int TryPutArmorIntoArmorSlots(ItemSlot handslot, int selectionBoxIndex, Vec3d hitPosition)
		{
			ItemStack sourceStack = handslot.Itemstack;
			IArmor armor;
			if (((sourceStack != null) ? sourceStack.Collectible : null) == null || !ArmorStandArmorSlot.TryGetArmor(sourceStack, out armor) || armor == null)
			{
				return 0;
			}
			if (!this.inventory.CanAcceptArmorStack(sourceStack, null))
			{
				return 0;
			}
			foreach (int index in this.OrderedArmorInsertSlots(selectionBoxIndex, hitPosition, armor.ArmorType))
			{
				if (index >= 0 && index < this.inventory.Count)
				{
					ArmorStandArmorSlot targetSlot = this.inventory[index] as ArmorStandArmorSlot;
					if (targetSlot != null && targetSlot.Empty && targetSlot.ArmorType.Intersect(armor.ArmorType) && targetSlot.CanHold(handslot))
					{
						int num = handslot.TryPutInto(this.entity.World, targetSlot, 1);
						if (num > 0)
						{
							this.SaveInventory(true);
						}
						return num;
					}
				}
			}
			return 0;
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0004795C File Offset: 0x00045B5C
		private IEnumerable<int> OrderedArmorInsertSlots(int selectionBoxIndex, ArmorType sourceArmorType)
		{
			return this.OrderedArmorInsertSlots(selectionBoxIndex, new Vec3d(0.0, 0.0, 0.0), sourceArmorType);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00047986 File Offset: 0x00045B86
		private IEnumerable<int> OrderedArmorInsertSlots(int selectionBoxIndex, Vec3d hitPosition, ArmorType sourceArmorType)
		{
			EntityBehaviorCOArmorStandInventory.<OrderedArmorInsertSlots>d__47 <OrderedArmorInsertSlots>d__ = new EntityBehaviorCOArmorStandInventory.<OrderedArmorInsertSlots>d__47(-2);
			<OrderedArmorInsertSlots>d__.<>4__this = this;
			<OrderedArmorInsertSlots>d__.<>3__selectionBoxIndex = selectionBoxIndex;
			<OrderedArmorInsertSlots>d__.<>3__hitPosition = hitPosition;
			<OrderedArmorInsertSlots>d__.<>3__sourceArmorType = sourceArmorType;
			return <OrderedArmorInsertSlots>d__;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x000479AC File Offset: 0x00045BAC
		private bool TryMoveWholeStackToEmptyHand(ItemSlot sourceSlot, ItemSlot handslot, out int moved)
		{
			moved = 0;
			if (!handslot.Empty)
			{
				return false;
			}
			ItemStack stack = sourceSlot.Itemstack;
			if (((stack != null) ? stack.Collectible : null) == null || stack.StackSize <= 0)
			{
				sourceSlot.Itemstack = null;
				sourceSlot.MarkDirty();
				return false;
			}
			moved = sourceSlot.TryPutInto(this.entity.World, handslot, 1);
			if (moved <= 0)
			{
				return false;
			}
			sourceSlot.MarkDirty();
			handslot.MarkDirty();
			return true;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00047A1C File Offset: 0x00045C1C
		private int VanillaSlotFromInteraction(int selectionBoxIndex, Vec3d hitPosition)
		{
			if (selectionBoxIndex >= 0 && selectionBoxIndex <= 4)
			{
				return selectionBoxIndex;
			}
			if (hitPosition == null)
			{
				return -1;
			}
			double localY = this.LocalHitY(hitPosition);
			if (double.IsNaN(localY))
			{
				return -1;
			}
			if (localY >= 1.45)
			{
				return 0;
			}
			if (localY >= 0.75)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00047A70 File Offset: 0x00045C70
		private double LocalHitY(Vec3d hitPosition)
		{
			double y = hitPosition.Y;
			double localFromEntity = y - this.entity.Pos.Y;
			if (localFromEntity >= -0.25 && localFromEntity <= 2.75)
			{
				return localFromEntity;
			}
			double localFromServerPos = y - this.entity.ServerPos.Y;
			if (localFromServerPos >= -0.25 && localFromServerPos <= 2.75)
			{
				return localFromServerPos;
			}
			if (y >= -0.25 && y <= 2.75)
			{
				return y;
			}
			return localFromEntity;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00047AF9 File Offset: 0x00045CF9
		private IEnumerable<int> OrderedVanillaArmorStandSlots(int selectionBoxIndex, Vec3d hitPosition)
		{
			EntityBehaviorCOArmorStandInventory.<OrderedVanillaArmorStandSlots>d__51 <OrderedVanillaArmorStandSlots>d__ = new EntityBehaviorCOArmorStandInventory.<OrderedVanillaArmorStandSlots>d__51(-2);
			<OrderedVanillaArmorStandSlots>d__.<>4__this = this;
			<OrderedVanillaArmorStandSlots>d__.<>3__selectionBoxIndex = selectionBoxIndex;
			<OrderedVanillaArmorStandSlots>d__.<>3__hitPosition = hitPosition;
			return <OrderedVanillaArmorStandSlots>d__;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00047B17 File Offset: 0x00045D17
		private IEnumerable<int> OrderedCoSlots(int selectionBoxIndex, Vec3d hitPosition)
		{
			EntityBehaviorCOArmorStandInventory.<OrderedCoSlots>d__52 <OrderedCoSlots>d__ = new EntityBehaviorCOArmorStandInventory.<OrderedCoSlots>d__52(-2);
			<OrderedCoSlots>d__.<>4__this = this;
			<OrderedCoSlots>d__.<>3__selectionBoxIndex = selectionBoxIndex;
			<OrderedCoSlots>d__.<>3__hitPosition = hitPosition;
			return <OrderedCoSlots>d__;
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x00047B35 File Offset: 0x00045D35
		private static int inventorySlotSearchCount
		{
			get
			{
				return ArmorInventory._totalSlotsNumber + 2;
			}
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00047B40 File Offset: 0x00045D40
		public override void OnTesselation(ref Shape entityShape, string shapePathForLogging, ref bool shapeIsCloned, ref string[] willDeleteElements)
		{
			if (entityShape == null)
			{
				return;
			}
			for (int i = 0; i < this.inventory.Count; i++)
			{
				this.RenderSlot(i, this.inventory[i], shapePathForLogging, ref entityShape, ref shapeIsCloned, ref willDeleteElements);
			}
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00047B80 File Offset: 0x00045D80
		private void RenderSlot(int slotIndex, ItemSlot slot, string shapePathForLogging, ref Shape entityShape, ref bool shapeIsCloned, ref string[] willDeleteElements)
		{
			if (!slot.Empty)
			{
				ItemStack itemstack = slot.Itemstack;
				if (((itemstack != null) ? itemstack.Collectible : null) != null)
				{
					string slotCode;
					Dictionary<string, StepParentElementTo> stepParent;
					if (!EntityBehaviorCOArmorStandInventory.TryGetRenderSlot(slotIndex, slot.Itemstack, out slotCode, out stepParent))
					{
						return;
					}
					IAttachableToEntity attachable = IAttachableToEntity.FromCollectible(slot.Itemstack.Collectible);
					if (attachable == null)
					{
						return;
					}
					if (!shapeIsCloned)
					{
						entityShape = entityShape.Clone();
						shapeIsCloned = true;
					}
					ICoreAPI api = this.entity.Api;
					Entity entity = this.entity;
					ICoreClientAPI coreClientAPI = this.entity.Api as ICoreClientAPI;
					entityShape = EntityBehaviorContainer.addGearToShape(api, entity, (coreClientAPI != null) ? coreClientAPI.EntityTextureAtlas : null, entityShape, slot.Itemstack, attachable, slotCode, shapePathForLogging, ref willDeleteElements, this.entity.Properties.Client.Textures, stepParent);
					return;
				}
			}
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00047C40 File Offset: 0x00045E40
		private static bool TryGetRenderSlot(int slotIndex, ItemStack stack, out string slotCode, [Nullable(new byte[]
		{
			2,
			1,
			1
		})] out Dictionary<string, StepParentElementTo> stepParent)
		{
			slotCode = "";
			stepParent = null;
			if (slotIndex == ArmorInventory._totalSlotsNumber)
			{
				slotCode = "righthand";
				stepParent = EntityBehaviorCOArmorStandInventory.RightHandStepParent;
				return true;
			}
			if (slotIndex == ArmorInventory._totalSlotsNumber + 1)
			{
				slotCode = "lefthand";
				stepParent = EntityBehaviorCOArmorStandInventory.LeftHandStepParent;
				return true;
			}
			return EntityBehaviorCOArmorStandInventory.TryGetArmorStandRenderSlot(stack, out slotCode, out stepParent);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00047C94 File Offset: 0x00045E94
		private static bool TryGetArmorStandRenderSlot(ItemStack stack, out string slotCode, [Nullable(new byte[]
		{
			2,
			1,
			1
		})] out Dictionary<string, StepParentElementTo> stepParent)
		{
			slotCode = "";
			stepParent = null;
			if (ItemSlotCharacter.IsDressType(stack, 12))
			{
				slotCode = "head";
				stepParent = EntityBehaviorCOArmorStandInventory.HeadStepParent;
				return true;
			}
			if (ItemSlotCharacter.IsDressType(stack, 13))
			{
				slotCode = "body";
				stepParent = EntityBehaviorCOArmorStandInventory.BodyStepParent;
				return true;
			}
			if (ItemSlotCharacter.IsDressType(stack, 14))
			{
				slotCode = "legs";
				stepParent = EntityBehaviorCOArmorStandInventory.LegsStepParent;
				return true;
			}
			IArmor armor;
			if (!ArmorStandArmorSlot.TryGetArmor(stack, out armor) || armor == null)
			{
				return false;
			}
			ArmorType type = armor.ArmorType;
			if ((type.Slots & (DamageZone.Head | DamageZone.Face | DamageZone.Neck)) != DamageZone.None)
			{
				slotCode = "head";
				stepParent = EntityBehaviorCOArmorStandInventory.HeadStepParent;
				return true;
			}
			if ((type.Slots & (DamageZone.Torso | DamageZone.Arms | DamageZone.Hands)) != DamageZone.None)
			{
				slotCode = "body";
				stepParent = EntityBehaviorCOArmorStandInventory.BodyStepParent;
				return true;
			}
			if ((type.Slots & (DamageZone.Legs | DamageZone.Feet)) != DamageZone.None)
			{
				slotCode = "legs";
				stepParent = EntityBehaviorCOArmorStandInventory.LegsStepParent;
				return true;
			}
			return false;
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00047D63 File Offset: 0x00045F63
		public override void FromBytes(bool isSync)
		{
			this.LoadInventory();
			this.eagent.MarkShapeModified();
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00047D76 File Offset: 0x00045F76
		public override void ToBytes(bool forClient)
		{
			this.SaveInventory(false);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00047D7F File Offset: 0x00045F7F
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			this.SaveInventory(false);
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00047D8F File Offset: 0x00045F8F
		public override void OnEntityDeath(DamageSource damageSourceForDeath)
		{
			if (this.entity.World.Side == 1)
			{
				this.DropAllContentsOnce();
			}
			base.OnEntityDeath(damageSourceForDeath);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00047DB4 File Offset: 0x00045FB4
		private void DropAllContentsOnce()
		{
			for (int i = 0; i < this.inventory.Count; i++)
			{
				ItemStack stack = this.inventory[i].Itemstack;
				if (((stack != null) ? stack.Collectible : null) == null || stack.StackSize <= 0)
				{
					this.inventory[i].Itemstack = null;
					this.inventory[i].MarkDirty();
				}
				else
				{
					this.inventory[i].Itemstack = null;
					this.inventory[i].MarkDirty();
					this.entity.World.SpawnItemEntity(stack, this.entity.ServerPos.XYZ, null);
				}
			}
			this.SaveInventory(false);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00047E78 File Offset: 0x00046078
		// Note: this type is marked as 'beforefieldinit'.
		static EntityBehaviorCOArmorStandInventory()
		{
			Dictionary<string, StepParentElementTo> dictionary = new Dictionary<string, StepParentElementTo>();
			dictionary[""] = new StepParentElementTo
			{
				ElementName = "Head"
			};
			EntityBehaviorCOArmorStandInventory.HeadStepParent = dictionary;
			Dictionary<string, StepParentElementTo> dictionary2 = new Dictionary<string, StepParentElementTo>();
			dictionary2[""] = new StepParentElementTo
			{
				ElementName = "ASUpperBody"
			};
			EntityBehaviorCOArmorStandInventory.BodyStepParent = dictionary2;
			Dictionary<string, StepParentElementTo> dictionary3 = new Dictionary<string, StepParentElementTo>();
			dictionary3[""] = new StepParentElementTo
			{
				ElementName = "Legs"
			};
			EntityBehaviorCOArmorStandInventory.LegsStepParent = dictionary3;
			Dictionary<string, StepParentElementTo> dictionary4 = new Dictionary<string, StepParentElementTo>();
			dictionary4[""] = new StepParentElementTo
			{
				ElementName = "RightHandAP"
			};
			dictionary4["OarStorage"] = new StepParentElementTo
			{
				ElementName = "LowerArmR_R_Oar"
			};
			dictionary4["LowerArmL"] = new StepParentElementTo
			{
				ElementName = "LowerArmR_R"
			};
			EntityBehaviorCOArmorStandInventory.RightHandStepParent = dictionary4;
			Dictionary<string, StepParentElementTo> dictionary5 = new Dictionary<string, StepParentElementTo>();
			dictionary5[""] = new StepParentElementTo
			{
				ElementName = "LeftHandAP"
			};
			dictionary5["OarStorage"] = new StepParentElementTo
			{
				ElementName = "LowerArmL_R_Oar"
			};
			dictionary5["LowerArmR"] = new StepParentElementTo
			{
				ElementName = "LowerArmL"
			};
			EntityBehaviorCOArmorStandInventory.LeftHandStepParent = dictionary5;
		}

		// Token: 0x0400070F RID: 1807
		private const string LegacyTreeKey = "wearablesInv";

		// Token: 0x04000710 RID: 1808
		private const string TreeKey = "coArmorStandInv";

		// Token: 0x04000711 RID: 1809
		private readonly EntityAgent eagent;

		// Token: 0x04000712 RID: 1810
		private readonly ArmorStandArmorInventory inventory;

		// Token: 0x04000713 RID: 1811
		private bool loadingInventory;

		// Token: 0x04000714 RID: 1812
		private bool hasPersistentTree;

		// Token: 0x04000715 RID: 1813
		private const int VanillaHeadSelection = 0;

		// Token: 0x04000716 RID: 1814
		private const int VanillaBodySelection = 1;

		// Token: 0x04000717 RID: 1815
		private const int VanillaLegsSelection = 2;

		// Token: 0x04000718 RID: 1816
		private const int VanillaRightHandSelection = 3;

		// Token: 0x04000719 RID: 1817
		private const int VanillaLeftHandSelection = 4;

		// Token: 0x0400071A RID: 1818
		private const string MigratedVanillaInventoryFlag = "coArmorStandMigratedVanillaInv";

		// Token: 0x0400071B RID: 1819
		private const int VanillaArmorStandSlotCount = 5;

		// Token: 0x0400071C RID: 1820
		private static readonly Dictionary<string, StepParentElementTo> HeadStepParent;

		// Token: 0x0400071D RID: 1821
		private static readonly Dictionary<string, StepParentElementTo> BodyStepParent;

		// Token: 0x0400071E RID: 1822
		private static readonly Dictionary<string, StepParentElementTo> LegsStepParent;

		// Token: 0x0400071F RID: 1823
		private static readonly Dictionary<string, StepParentElementTo> RightHandStepParent;

		// Token: 0x04000720 RID: 1824
		private static readonly Dictionary<string, StepParentElementTo> LeftHandStepParent;
	}
}
