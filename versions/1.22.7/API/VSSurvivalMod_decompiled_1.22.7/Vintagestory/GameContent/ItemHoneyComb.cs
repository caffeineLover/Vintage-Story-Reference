using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ItemHoneyComb : Item
{
	public float ContainedHoneyLitres = 0.2f;

	private WorldInteraction[]? interactions;

	[Obsolete("Use the overload that takes an actor for claim validation.", true)]
	public bool CanSqueezeInto(Block block, BlockSelection? blockSel)
	{
		if (blockSel == null)
		{
			return block is BlockLiquidContainerTopOpened;
		}
		return CanSqueezeInto(null, block, blockSel);
	}

	public bool CanSqueezeInto(EntityAgent? byEntity, Block block, BlockSelection blockSel)
	{
		IPlayer byPlayer = (byEntity as EntityPlayer)?.Player;
		if (block is BlockLiquidContainerTopOpened blockLiquidContainer)
		{
			if (blockLiquidContainer.IsFull(blockSel.Position))
			{
				return false;
			}
			if (byPlayer != null)
			{
				return byEntity.World.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use);
			}
			return true;
		}
		if (block is BlockBarrel barrel && api.World.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityBarrel beb)
		{
			if (beb.Sealed || barrel.IsFull(blockSel.Position))
			{
				return false;
			}
			if (byPlayer != null)
			{
				return byEntity.World.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use);
			}
			return true;
		}
		if (api.World.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityGroundStorage beg)
		{
			ItemSlot squeezeIntoSlot = beg.GetSlotAt(blockSel);
			if (squeezeIntoSlot?.Itemstack?.Block is BlockLiquidContainerTopOpened itemLiquidContainer)
			{
				if (itemLiquidContainer.IsFull(squeezeIntoSlot.Itemstack))
				{
					return false;
				}
				if (byPlayer != null)
				{
					return byEntity.World.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use);
				}
				return true;
			}
		}
		return false;
	}

	public override void OnLoaded(ICoreAPI api)
	{
		if (api.Side != EnumAppSide.Client)
		{
			return;
		}
		interactions = ObjectCacheUtil.GetOrCreate(api, "honeyCombInteractions", delegate
		{
			List<ItemStack> list = new List<ItemStack>();
			foreach (Block current in api.World.Blocks)
			{
				if (!(current.Code == null))
				{
					if (current is BlockBarrel)
					{
						list.Add(new ItemStack(current));
					}
					if (current is BlockLiquidContainerTopOpened)
					{
						list.Add(new ItemStack(current));
					}
				}
			}
			return new WorldInteraction[1]
			{
				new WorldInteraction
				{
					ActionLangCode = "heldhelp-squeeze",
					HotKeyCode = "shift",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = list.ToArray()
				}
			};
		});
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		if (blockSel?.Block != null && CanSqueezeInto(byEntity, blockSel.Block, blockSel) && byEntity.Controls.ShiftKey)
		{
			handling = EnumHandHandling.PreventDefault;
			if (api.World.Side == EnumAppSide.Client)
			{
				byEntity.World.PlaySoundAt(new AssetLocation("sounds/player/squeezehoneycomb"), byEntity, null, randomizePitch: true, 16f, 0.5f);
			}
		}
		else
		{
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (blockSel?.Block != null && CanSqueezeInto(byEntity, blockSel.Block, blockSel))
		{
			if (!byEntity.Controls.ShiftKey)
			{
				return false;
			}
			if (byEntity.World is IClientWorldAccessor)
			{
				byEntity.StartAnimation("squeezehoneycomb");
			}
			return secondsUsed < 2f;
		}
		return base.OnHeldInteractStep(secondsUsed, slot, byEntity, blockSel, entitySel);
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		byEntity.StopAnimation("squeezehoneycomb");
		if (blockSel != null)
		{
			Block block = byEntity.World.BlockAccessor.GetBlock(blockSel.Position);
			if (CanSqueezeInto(byEntity, block, blockSel))
			{
				if (secondsUsed < 1.9f)
				{
					return;
				}
				IWorldAccessor world = byEntity.World;
				ItemStack honeyStack = new ItemStack(world.GetItem(new AssetLocation("honeyportion")), 99999);
				if (block is BlockLiquidContainerTopOpened blockCnt)
				{
					if (blockCnt.TryPutLiquid(blockSel.Position, honeyStack, ContainedHoneyLitres) == 0)
					{
						return;
					}
				}
				else if (block is BlockBarrel blockBarrel && api.World.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityBarrel beb)
				{
					if (beb.Sealed || blockBarrel.TryPutLiquid(blockSel.Position, honeyStack, ContainedHoneyLitres) == 0)
					{
						return;
					}
				}
				else if (api.World.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityGroundStorage beg)
				{
					ItemSlot squeezeIntoSlot = beg.GetSlotAt(blockSel);
					if (squeezeIntoSlot != null && squeezeIntoSlot.Itemstack?.Block != null)
					{
						if (squeezeIntoSlot.Itemstack.Block is BlockLiquidContainerTopOpened blockCnt2)
						{
							blockCnt2.TryPutLiquid(squeezeIntoSlot.Itemstack, honeyStack, ContainedHoneyLitres);
						}
						beg.MarkDirty(redrawOnClient: true);
					}
				}
				slot.TakeOut(1);
				slot.MarkDirty();
				IPlayer obj = (byEntity as EntityPlayer)?.Player;
				ItemStack stack = new ItemStack(world.GetItem(new AssetLocation("beeswax")));
				if (obj != null && !obj.InventoryManager.TryGiveItemstack(stack))
				{
					byEntity.World.SpawnItemEntity(stack, byEntity.Pos.XYZ);
				}
				return;
			}
		}
		base.OnHeldInteractStop(secondsUsed, slot, byEntity, blockSel, entitySel);
	}

	public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
	{
		byEntity.StopAnimation("squeezehoneycomb");
		return base.OnHeldInteractCancel(secondsUsed, slot, byEntity, blockSel, entitySel, cancelReason);
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		return interactions.Append<WorldInteraction>(base.GetHeldInteractionHelp(inSlot));
	}
}
