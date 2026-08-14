using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityArmorStand : EntityAgent
{
	private float fireDamage;

	private string[] poses = new string[4] { "idle", "lefthandup", "righthandup", "twohandscross" };

	public override bool IsCreature => false;

	private int CurPose
	{
		get
		{
			return WatchedAttributes.GetInt("curPose");
		}
		set
		{
			WatchedAttributes.SetInt("curPose", value);
		}
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		TryEarlyUpdateOldArmorStandInventory(api.World);
		base.Initialize(properties, api, InChunkIndex3d);
		AnimManager.StartAnimation(new AnimationMetaData
		{
			Animation = poses[CurPose],
			Code = poses[CurPose]
		}.Init());
	}

	public void TryEarlyUpdateOldArmorStandInventory(IWorldAccessor world)
	{
		if (world.Api.Side != EnumAppSide.Server || !WatchedAttributes.HasAttribute("inventory"))
		{
			return;
		}
		ITreeAttribute slots = WatchedAttributes.GetTreeAttribute("inventory").GetTreeAttribute("slots");
		if (slots.Count > 0)
		{
			string[] oldSlots = new string[5] { "12", "13", "14", "15", "16" };
			TreeAttribute newInventory = new TreeAttribute();
			newInventory.SetInt("qslots", 5);
			TreeAttribute newSlots = new TreeAttribute();
			newInventory.SetAttribute("slots", newSlots);
			for (int index = 0; index < oldSlots.Length; index++)
			{
				string slotKey = oldSlots[index];
				if (slots.HasAttribute(slotKey))
				{
					ItemStack newStack = slots.GetItemstack(slotKey);
					newSlots.SetItemstack(index.ToString(), newStack);
				}
			}
			WatchedAttributes.SetAttribute("wearablesInv", newInventory);
		}
		WatchedAttributes.RemoveAttribute("inventory");
	}

	public override void OnInteract(EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
	{
		if (!Alive || World.Side == EnumAppSide.Client || mode == EnumInteractMode.Attack)
		{
			return;
		}
		IPlayer player = (byEntity as EntityPlayer)?.Player;
		if (player != null && !byEntity.World.Claims.TryAccess(player, base.Pos.AsBlockPos, EnumBlockAccessFlags.Use))
		{
			player.InventoryManager.ActiveHotbarSlot.MarkDirty();
			WatchedAttributes.MarkAllDirty();
			return;
		}
		if (mode == EnumInteractMode.Interact && byEntity.Controls.ShiftKey && byEntity.RightHandItemSlot.Empty)
		{
			EntityBehaviorAttachable? behavior = GetBehavior<EntityBehaviorAttachable>();
			if (behavior != null && behavior.Inventory.Empty)
			{
				ItemStack stack = new ItemStack(byEntity.World.GetItem(Code));
				if (!byEntity.TryGiveItemStack(stack))
				{
					byEntity.World.SpawnItemEntity(stack, base.Pos.XYZ);
				}
				byEntity.World.Logger.Audit("{0} Picked up 1x{1} at {2}.", byEntity.GetName(), stack.Collectible.Code, base.Pos);
				Die();
				return;
			}
		}
		if (mode == EnumInteractMode.Interact && byEntity.RightHandItemSlot?.Itemstack?.Collectible is ItemWrench)
		{
			AnimManager.StopAnimation(poses[CurPose]);
			CurPose = (CurPose + 1) % poses.Length;
			AnimManager.StartAnimation(new AnimationMetaData
			{
				Animation = poses[CurPose],
				Code = poses[CurPose]
			}.Init());
		}
		else
		{
			base.OnInteract(byEntity, slot, hitPosition, mode);
		}
	}

	public override bool ReceiveDamage(DamageSource damageSource, float damage)
	{
		if (damageSource.Source == EnumDamageSource.Internal && damageSource.Type == EnumDamageType.Fire)
		{
			fireDamage += damage;
		}
		if (fireDamage > 4f)
		{
			Die();
		}
		return base.ReceiveDamage(damageSource, damage);
	}
}
