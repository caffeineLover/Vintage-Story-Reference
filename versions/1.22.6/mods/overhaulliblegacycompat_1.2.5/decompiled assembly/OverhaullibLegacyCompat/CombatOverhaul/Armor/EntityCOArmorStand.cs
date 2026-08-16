using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000121 RID: 289
	[NullableContext(1)]
	[Nullable(0)]
	public class EntityCOArmorStand : EntityHumanoid
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x00044BE3 File Offset: 0x00042DE3
		public override bool IsCreature
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000E11 RID: 3601 RVA: 0x00044BE6 File Offset: 0x00042DE6
		public override bool IsInteractable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00044BE9 File Offset: 0x00042DE9
		// (set) Token: 0x06000E13 RID: 3603 RVA: 0x00044BFC File Offset: 0x00042DFC
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

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x00044C43 File Offset: 0x00042E43
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

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x00044C60 File Offset: 0x00042E60
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

		// Token: 0x06000E17 RID: 3607 RVA: 0x00044C7F File Offset: 0x00042E7F
		public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
		{
			base.Initialize(properties, api, InChunkIndex3d);
			this.invbh = this.GetBehavior<EntityBehaviorCOArmorStandInventory>();
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00044C98 File Offset: 0x00042E98
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

		// Token: 0x06000E19 RID: 3609 RVA: 0x00044FE0 File Offset: 0x000431E0
		private static bool IsToolOrToolrackItem(ItemStack stack)
		{
			if (stack.Collectible.Tool == null)
			{
				JsonObject itemAttributes = stack.ItemAttributes;
				return itemAttributes != null && itemAttributes["toolrackTransform"].Exists;
			}
			return true;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00045014 File Offset: 0x00043214
		private static bool IsArmorStandArmor(ItemStack stack)
		{
			IArmor armor;
			return ItemSlotCharacter.IsDressType(stack, 13) || ItemSlotCharacter.IsDressType(stack, 12) || ItemSlotCharacter.IsDressType(stack, 14) || ArmorStandArmorSlot.TryGetArmor(stack, out armor);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00045049 File Offset: 0x00043249
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

		// Token: 0x040006E6 RID: 1766
		[Nullable(2)]
		private EntityBehaviorCOArmorStandInventory invbh;

		// Token: 0x040006E7 RID: 1767
		private float fireDamage;

		// Token: 0x040006E8 RID: 1768
		private readonly string[] poses = new string[]
		{
			"idle",
			"lefthandup",
			"righthandup",
			"twohandscross"
		};
	}
}
