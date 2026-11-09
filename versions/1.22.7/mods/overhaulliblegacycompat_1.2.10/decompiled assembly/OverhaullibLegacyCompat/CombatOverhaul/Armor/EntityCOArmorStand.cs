using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200012D RID: 301
	[NullableContext(1)]
	[Nullable(0)]
	public class EntityCOArmorStand : EntityHumanoid
	{
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x000464E3 File Offset: 0x000446E3
		public override bool IsCreature
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x000464E6 File Offset: 0x000446E6
		public override bool IsInteractable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x000464E9 File Offset: 0x000446E9
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x000464FC File Offset: 0x000446FC
		private int CurPose
		{
			get
			{
				return this.WatchedAttributes.GetInt("curPose", 0);
			}
			set
			{
				this.WatchedAttributes.SetInt("curPose", value);
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x00046543 File Offset: 0x00044743
		[Nullable(2)]
		public override ItemSlot RightHandItemSlot
		{
			[NullableContext(2)]
			get
			{
				EntityBehaviorCOArmorStandInventory entityBehaviorCOArmorStandInventory = this.invbh;
				if (entityBehaviorCOArmorStandInventory == null)
				{
					return null;
				}
				return entityBehaviorCOArmorStandInventory.Inventory[ArmorInventory._totalSlotsNumber];
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x00046560 File Offset: 0x00044760
		[Nullable(2)]
		public override ItemSlot LeftHandItemSlot
		{
			[NullableContext(2)]
			get
			{
				EntityBehaviorCOArmorStandInventory entityBehaviorCOArmorStandInventory = this.invbh;
				if (entityBehaviorCOArmorStandInventory == null)
				{
					return null;
				}
				return entityBehaviorCOArmorStandInventory.Inventory[ArmorInventory._totalSlotsNumber + 1];
			}
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0004657F File Offset: 0x0004477F
		public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
		{
			base.Initialize(properties, api, InChunkIndex3d);
			this.invbh = this.GetBehavior<EntityBehaviorCOArmorStandInventory>();
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00046598 File Offset: 0x00044798
		public override void OnInteract(EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
		{
			if (!this.Alive || mode == null)
			{
				return;
			}
			if (this.World.Side == 2)
			{
				return;
			}
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			IPlayer player = (entityPlayer != null) ? entityPlayer.Player : null;
			if (player != null && !byEntity.World.Claims.TryAccess(player, base.Pos.AsBlockPos, 2))
			{
				player.InventoryManager.ActiveHotbarSlot.MarkDirty();
				this.WatchedAttributes.MarkAllDirty();
				return;
			}
			EntityBehaviorCOArmorStandInventory entityBehaviorCOArmorStandInventory = this.invbh;
			if (((entityBehaviorCOArmorStandInventory != null) ? entityBehaviorCOArmorStandInventory.Inventory : null) == null)
			{
				base.OnInteract(byEntity, slot, hitPosition, mode);
				return;
			}
			if (mode != 1)
			{
				base.OnInteract(byEntity, slot, hitPosition, mode);
				return;
			}
			ItemStack itemstack = slot.Itemstack;
			if (((itemstack != null) ? itemstack.Collectible : null) is ItemWrench)
			{
				this.AnimManager.StopAnimation(this.poses[this.CurPose]);
				this.CurPose = (this.CurPose + 1) % this.poses.Length;
				this.AnimManager.StartAnimation(new AnimationMetaData
				{
					Animation = this.poses[this.CurPose],
					Code = this.poses[this.CurPose]
				}.Init());
				this.WatchedAttributes.MarkPathDirty("curPose");
				return;
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
				if (this.invbh.TryTakeSelected(byEntity, slot, selectionBoxIndex, hitPosition))
				{
					this.MarkShapeModified();
					return;
				}
				if (byEntity.Controls.ShiftKey && this.invbh.Inventory.Empty)
				{
					Item standItem = byEntity.World.GetItem(this.Code);
					if (standItem != null)
					{
						ItemStack stack = new ItemStack(standItem, 1);
						if (!byEntity.TryGiveItemStack(stack))
						{
							byEntity.World.SpawnItemEntity(stack, base.ServerPos.XYZ, null);
						}
						byEntity.World.Logger.Audit("{0} Took 1x{1} from Armor Stand at {2}.", new object[]
						{
							byEntity.GetName(),
							stack.Collectible.Code,
							base.ServerPos.AsBlockPos
						});
					}
					this.Die(0, null);
					return;
				}
				return;
			}
			else
			{
				ItemStack heldStack = slot.Itemstack;
				if (((heldStack != null) ? heldStack.Collectible : null) == null || heldStack.StackSize <= 0)
				{
					slot.Itemstack = null;
					slot.MarkDirty();
					return;
				}
				if (EntityCOArmorStand.IsToolOrToolrackItem(heldStack))
				{
					AssetLocation code = heldStack.Collectible.Code;
					if (this.invbh.TryPutToolIntoHandSlots(slot, selectionBoxIndex, hitPosition) > 0)
					{
						this.MarkShapeModified();
						byEntity.World.Logger.Audit("{0} Put 1x{1} onto Armor Stand at {2}.", new object[]
						{
							byEntity.GetName(),
							code,
							base.ServerPos.AsBlockPos
						});
					}
					return;
				}
				if (!EntityCOArmorStand.IsArmorStandArmor(heldStack))
				{
					return;
				}
				AssetLocation armorCode = heldStack.Collectible.Code;
				if (this.invbh.TryPutArmorIntoArmorSlots(slot, selectionBoxIndex, hitPosition) > 0)
				{
					this.MarkShapeModified();
					byEntity.World.Logger.Audit("{0} Put 1x{1} onto Armor Stand at {2}.", new object[]
					{
						byEntity.GetName(),
						armorCode,
						base.ServerPos.AsBlockPos
					});
				}
				return;
			}
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x000468E0 File Offset: 0x00044AE0
		private static bool IsToolOrToolrackItem(ItemStack stack)
		{
			if (stack.Collectible.Tool == null)
			{
				JsonObject itemAttributes = stack.ItemAttributes;
				return itemAttributes != null && itemAttributes["toolrackTransform"].Exists;
			}
			return true;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00046914 File Offset: 0x00044B14
		private static bool IsArmorStandArmor(ItemStack stack)
		{
			IArmor armor;
			return ItemSlotCharacter.IsDressType(stack, 13) || ItemSlotCharacter.IsDressType(stack, 12) || ItemSlotCharacter.IsDressType(stack, 14) || ArmorStandArmorSlot.TryGetArmor(stack, out armor);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00046949 File Offset: 0x00044B49
		public override bool ReceiveDamage(DamageSource damageSource, float damage)
		{
			if (damageSource.Source == 7 && damageSource.Type == 1)
			{
				this.fireDamage += damage;
			}
			if (this.fireDamage > 4f)
			{
				this.Die(0, null);
			}
			return base.ReceiveDamage(damageSource, damage);
		}

		// Token: 0x0400070C RID: 1804
		[Nullable(2)]
		private EntityBehaviorCOArmorStandInventory invbh;

		// Token: 0x0400070D RID: 1805
		private float fireDamage;

		// Token: 0x0400070E RID: 1806
		private readonly string[] poses = new string[]
		{
			"idle",
			"lefthandup",
			"righthandup",
			"twohandscross"
		};
	}
}
