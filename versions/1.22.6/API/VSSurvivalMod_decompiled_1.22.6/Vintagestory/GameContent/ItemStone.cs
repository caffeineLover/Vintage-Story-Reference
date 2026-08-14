using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ItemStone : Item
{
	private float damage;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		damage = Attributes["damage"].AsFloat(1f);
	}

	public override string GetHeldTpUseAnimation(ItemSlot activeHotbarSlot, Entity byEntity)
	{
		return null;
	}

	public override string GetHeldTpHitAnimation(ItemSlot slot, Entity byEntity)
	{
		if (slot.Itemstack?.Collectible == this)
		{
			return "knap";
		}
		return base.GetHeldTpHitAnimation(slot, byEntity);
	}

	public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		Block lookedAtBlock = ((blockSel == null) ? null : byEntity.World.BlockAccessor.GetBlock(blockSel.Position));
		if (lookedAtBlock is BlockDisplayCase || lookedAtBlock is BlockSign || lookedAtBlock is BlockBloomery)
		{
			base.OnHeldInteractStart(itemslot, byEntity, blockSel, entitySel, firstEvent, ref handling);
			handling = EnumHandHandling.NotHandled;
			return;
		}
		EnumHandHandling bhHandHandling = EnumHandHandling.NotHandled;
		CollectibleBehavior[] collectibleBehaviors = CollectibleBehaviors;
		foreach (CollectibleBehavior obj in collectibleBehaviors)
		{
			EnumHandling hd = EnumHandling.PassThrough;
			obj.OnHeldInteractStart(itemslot, byEntity, blockSel, entitySel, firstEvent, ref bhHandHandling, ref hd);
			if (hd == EnumHandling.PreventSubsequent)
			{
				break;
			}
		}
		if (bhHandHandling != EnumHandHandling.NotHandled)
		{
			handling = bhHandHandling;
			return;
		}
		bool knappable = itemslot.Itemstack.Collectible.Attributes != null && itemslot.Itemstack.Collectible.Attributes["knappable"].AsBool();
		bool haveKnappableStone = false;
		IPlayer byPlayer = (byEntity as EntityPlayer)?.Player;
		if (byEntity.Controls.ShiftKey && blockSel != null)
		{
			Block block = byEntity.World.BlockAccessor.GetBlock(blockSel.Position);
			haveKnappableStone = block.Code.PathStartsWith("loosestones") && block.FirstCodePart(1).Equals(itemslot.Itemstack.Collectible.FirstCodePart(1));
		}
		if (haveKnappableStone)
		{
			if (!knappable)
			{
				if (byEntity.World.Side == EnumAppSide.Client)
				{
					(api as ICoreClientAPI).TriggerIngameError(this, "toosoft", Lang.Get("This type of stone is too soft to be used for knapping."));
				}
				return;
			}
			if (!byEntity.World.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use))
			{
				itemslot.MarkDirty();
				return;
			}
			IWorldAccessor world = byEntity.World;
			Block knappingBlock = world.GetBlock(new AssetLocation("knappingsurface"));
			if (knappingBlock == null)
			{
				return;
			}
			string failCode = "";
			BlockPos pos = blockSel.Position;
			knappingBlock.CanPlaceBlock(world, byPlayer, blockSel, ref failCode);
			if (failCode == "entityintersecting")
			{
				bool selfBlocked = false;
				string err = ((world.GetIntersectingEntities(pos, knappingBlock.GetCollisionBoxes(world.BlockAccessor, pos), delegate(Entity e)
				{
					selfBlocked = e == byEntity;
					return !(e is EntityItem);
				}).Length == 0) ? Lang.Get("Cannot place a knapping surface here") : (selfBlocked ? Lang.Get("Cannot place a knapping surface here, too close to you") : Lang.Get("Cannot place a knapping surface here, to close to another player or creature.")));
				(api as ICoreClientAPI).TriggerIngameError(this, "cantplace", err);
				return;
			}
			world.BlockAccessor.SetBlock(knappingBlock.BlockId, pos);
			world.BlockAccessor.TriggerNeighbourBlockUpdate(blockSel.Position);
			(api as ICoreClientAPI)?.World.Player.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
			if (knappingBlock.Sounds != null)
			{
				world.PlaySoundAt(knappingBlock.Sounds.Place, blockSel.Position, -0.5);
			}
			if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityKnappingSurface bec)
			{
				bec.BaseMaterial = itemslot.Itemstack.Clone();
				bec.BaseMaterial.StackSize = 1;
				if (byEntity.World is IClientWorldAccessor)
				{
					bec.OpenDialog(world as IClientWorldAccessor, pos, itemslot.Itemstack);
				}
			}
			handling = EnumHandHandling.PreventDefault;
			byEntity.Attributes.SetInt("aimingCancel", 1);
		}
		else
		{
			if (blockSel == null || byEntity?.World == null || !byEntity.Controls.ShiftKey)
			{
				return;
			}
			IWorldAccessor world2 = byEntity.World;
			Block block2 = world2.GetBlock(CodeWithPath("loosestones-" + LastCodePart() + "-free"));
			if (block2 == null)
			{
				block2 = world2.GetBlock(CodeWithPath("loosestones-" + LastCodePart(1) + "-" + LastCodePart() + "-free"));
			}
			if (block2 == null)
			{
				return;
			}
			BlockPos targetpos = blockSel.Position.AddCopy(blockSel.Face);
			targetpos.Y--;
			if (!world2.BlockAccessor.GetMostSolidBlock(targetpos).CanAttachBlockAt(world2.BlockAccessor, block2, targetpos, BlockFacing.UP))
			{
				return;
			}
			targetpos.Y++;
			BlockSelection placeSel = blockSel.Clone();
			placeSel.Position = targetpos;
			placeSel.DidOffset = true;
			string error = "";
			if (!block2.TryPlaceBlock(world2, byPlayer, itemslot.Itemstack, placeSel, ref error))
			{
				if (api.Side == EnumAppSide.Client)
				{
					(api as ICoreClientAPI).TriggerIngameError(this, "cantplace", Lang.Get("placefailure-" + error));
				}
				return;
			}
			world2.BlockAccessor.TriggerNeighbourBlockUpdate(blockSel.Position);
			if (block2.Sounds != null)
			{
				world2.PlaySoundAt(block2.Sounds.Place, blockSel.Position, -0.5);
			}
			(api as ICoreClientAPI)?.World.Player.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
			itemslot.Itemstack.StackSize--;
			handling = EnumHandHandling.PreventDefault;
			byEntity.Attributes.SetInt("aimingCancel", 1);
		}
	}

	public override void OnHeldAttackStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandHandling handling)
	{
		if (blockSel != null && byEntity.World.BlockAccessor.GetBlock(blockSel.Position) is BlockKnappingSurface && byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityKnappingSurface bea)
		{
			IPlayer byPlayer = null;
			if (byEntity is EntityPlayer)
			{
				byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
			}
			if (byPlayer != null)
			{
				bea.OnBeginUse(byPlayer, blockSel);
				handling = EnumHandHandling.PreventDefaultAction;
			}
		}
	}

	public override bool OnHeldAttackCancel(float secondsPassed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSelection, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
	{
		return false;
	}

	public override bool OnHeldAttackStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSelection, EntitySelection entitySel)
	{
		return false;
	}

	public override void OnHeldAttackStop(float secondsPassed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (blockSel == null || !(byEntity.World.BlockAccessor.GetBlock(blockSel.Position) is BlockKnappingSurface) || !(byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityKnappingSurface bea))
		{
			return;
		}
		IPlayer byPlayer = null;
		if (byEntity is EntityPlayer)
		{
			byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
		}
		if (byPlayer != null)
		{
			GetToolMode(slot, byPlayer, blockSel);
			if (byEntity.World is IClientWorldAccessor)
			{
				bea.OnUseOver(byPlayer, blockSel.SelectionBoxIndex, blockSel.Face, mouseMode: true);
			}
		}
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		return new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = "heldhelp-place",
				HotKeyCode = "shift",
				MouseButton = EnumMouseButton.Right
			}
		}.Append(base.GetHeldInteractionHelp(inSlot));
	}
}
