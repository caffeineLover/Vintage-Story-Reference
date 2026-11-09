using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ItemSling : Item
{
	private WorldInteraction[] interactions;

	public override void OnLoaded(ICoreAPI api)
	{
		if (api.Side != EnumAppSide.Client)
		{
			return;
		}
		_ = api;
		interactions = ObjectCacheUtil.GetOrCreate(api, "slingInteractions", delegate
		{
			List<ItemStack> list = new List<ItemStack>();
			foreach (CollectibleObject current in api.World.Collectibles)
			{
				if (current is ItemStone)
				{
					list.Add(new ItemStack(current));
				}
			}
			return new WorldInteraction[1]
			{
				new WorldInteraction
				{
					ActionLangCode = "heldhelp-chargesling",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = list.ToArray()
				}
			};
		});
	}

	public override string GetHeldTpUseAnimation(ItemSlot activeHotbarSlot, Entity byEntity)
	{
		return null;
	}

	private ItemSlot GetNextMunition(EntityAgent byEntity)
	{
		ItemSlot slot = null;
		byEntity.WalkInventory(delegate(ItemSlot invslot)
		{
			if (invslot is ItemSlotCreative)
			{
				return true;
			}
			if (invslot.Itemstack != null && invslot.Itemstack.Collectible is ItemStone)
			{
				slot = invslot;
				return false;
			}
			return true;
		});
		return slot;
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		if (GetNextMunition(byEntity) != null)
		{
			if (byEntity.World is IClientWorldAccessor)
			{
				slot.Itemstack.TempAttributes.SetInt("renderVariant", 1);
			}
			slot.Itemstack.Attributes.SetInt("renderVariant", 1);
			byEntity.Attributes.SetInt("aiming", 1);
			byEntity.Attributes.SetInt("aimingCancel", 0);
			byEntity.AnimManager.StartAnimation("slingaimbalearic");
			IPlayer byPlayer = null;
			if (byEntity is EntityPlayer)
			{
				byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
			}
			byEntity.World.PlaySoundAt(new AssetLocation("sounds/bow-draw"), byEntity, byPlayer, randomizePitch: false, 8f);
			handling = EnumHandHandling.PreventDefault;
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		int renderVariant = GameMath.Clamp((int)Math.Ceiling(secondsUsed * 4f), 0, 3);
		int num = slot.Itemstack.Attributes.GetInt("renderVariant");
		slot.Itemstack.TempAttributes.SetInt("renderVariant", renderVariant);
		slot.Itemstack.Attributes.SetInt("renderVariant", renderVariant);
		if (num != renderVariant)
		{
			(byEntity as EntityPlayer)?.Player?.InventoryManager.BroadcastHotbarSlot();
		}
		return true;
	}

	public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
	{
		byEntity.Attributes.SetInt("aiming", 0);
		byEntity.AnimManager.StopAnimation("slingaimbalearic");
		if (byEntity.World is IClientWorldAccessor)
		{
			slot.Itemstack.TempAttributes.RemoveAttribute("renderVariant");
		}
		slot.Itemstack.Attributes.SetInt("renderVariant", 0);
		if (cancelReason != EnumItemUseCancelReason.Destroyed)
		{
			(byEntity as EntityPlayer)?.Player?.InventoryManager.BroadcastHotbarSlot();
		}
		if (cancelReason != EnumItemUseCancelReason.ReleasedMouse)
		{
			byEntity.Attributes.SetInt("aimingCancel", 1);
		}
		return true;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (byEntity.Attributes.GetInt("aimingCancel") == 1)
		{
			return;
		}
		byEntity.Attributes.SetInt("aiming", 0);
		byEntity.AnimManager.StopAnimation("slingaimbalearic");
		byEntity.World.RegisterCallback(delegate
		{
			slot.Itemstack?.Attributes.SetInt("renderVariant", 2);
		}, 250);
		byEntity.World.RegisterCallback(delegate
		{
			if (byEntity.World is IClientWorldAccessor)
			{
				slot.Itemstack?.TempAttributes.RemoveAttribute("renderVariant");
			}
			slot.Itemstack?.Attributes.SetInt("renderVariant", 0);
		}, 450);
		(byEntity as EntityPlayer)?.Player?.InventoryManager.BroadcastHotbarSlot();
		if (secondsUsed < 0.75f)
		{
			return;
		}
		ItemSlot arrowSlot = GetNextMunition(byEntity);
		if (arrowSlot != null)
		{
			float damage = 0f;
			if (slot.Itemstack.Collectible.Attributes != null)
			{
				damage += slot.Itemstack.Collectible.Attributes["damage"].AsFloat();
			}
			if (arrowSlot.Itemstack.Collectible.Attributes != null)
			{
				damage += arrowSlot.Itemstack.Collectible.Attributes["damage"].AsFloat();
			}
			if (byEntity != null)
			{
				damage *= byEntity.Stats.GetBlended("rangedWeaponsDamage");
			}
			ItemStack stack = arrowSlot.TakeOut(1);
			arrowSlot.MarkDirty();
			if (byEntity is EntityPlayer)
			{
				byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
			}
			if (api.Side == EnumAppSide.Server)
			{
				byEntity.World.PlaySoundAt(new AssetLocation("sounds/tool/sling1"), byEntity, null, randomizePitch: false, 8f, 0.25f);
			}
			EntityProperties type = byEntity.World.GetEntityType(new AssetLocation("thrownstone-" + stack.Collectible.Variant["rock"]));
			Entity entity = byEntity.World.ClassRegistry.CreateEntity(type);
			IProjectile obj = entity as IProjectile;
			obj.FiredBy = byEntity;
			obj.Damage = damage;
			obj.ProjectileStack = stack;
			obj.WeaponStack = slot.Itemstack;
			EntityProjectileBase.SpawnProjectile(speed: 0.8 * (double)byEntity.Stats.GetBlended("bowDrawingStrength"), entity: entity, byEntity: byEntity, dispersionFactor: 0.75, verticalOffset: -0.2, horizontalOffset: 0.1, forwardOffset: 0.4, parallaxDistance: 20.0);
			slot.Itemstack.Collectible.DamageItem(byEntity.World, byEntity, slot);
			byEntity.AnimManager.StartAnimation("slingthrowbalearic");
			byEntity.World.RegisterCallback(delegate
			{
				byEntity.AnimManager.StopAnimation("slingthrowbalearic");
			}, 400);
		}
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		if (inSlot.Itemstack.Collectible.Attributes != null)
		{
			float dmg = inSlot.Itemstack.Collectible.Attributes["damage"].AsFloat();
			if (dmg != 0f)
			{
				dsc.AppendLine(Lang.Get("sling-piercingdamage", dmg));
			}
		}
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		return interactions.Append(base.GetHeldInteractionHelp(inSlot));
	}
}
