using System;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BlockBehaviorMilkingContainer : BlockBehavior
{
	private ICoreAPI api;

	private BlockLiquidContainerBase lcblock;

	public BlockBehaviorMilkingContainer(Block block)
		: base(block)
	{
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		this.api = api;
		lcblock = block as BlockLiquidContainerBase;
		if (lcblock == null)
		{
			throw new InvalidOperationException($"Block with code {block.Code} has behavior MilkingContainer, but its block class does not inherit from BlockLiquidContainerBase, which is required");
		}
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
	{
		EntityBehaviorMilkable bh = entitySel?.Entity.GetBehavior<EntityBehaviorMilkable>();
		if (bh == null)
		{
			return;
		}
		if (lcblock.GetContent(slot.Itemstack) != null)
		{
			(api as ICoreClientAPI)?.TriggerIngameError(this, "useemptybucket", Lang.Get("Use an empty bucket for milking"));
			return;
		}
		if (firstEvent)
		{
			bh.TryBeginMilking();
		}
		handling = EnumHandling.PreventDefault;
		handHandling = EnumHandHandling.PreventDefault;
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandling handling)
	{
		EntityBehaviorMilkable bh = entitySel?.Entity.GetBehavior<EntityBehaviorMilkable>();
		if (bh == null)
		{
			return false;
		}
		IPlayer player = (byEntity as EntityPlayer)?.Player;
		if (player == null)
		{
			return false;
		}
		if (!bh.CanContinueMilking(player, secondsUsed))
		{
			return false;
		}
		handling = EnumHandling.PreventDefault;
		if (api.Side != EnumAppSide.Server)
		{
			return secondsUsed < 3f;
		}
		return true;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandling handling)
	{
		if (!(secondsUsed <= 2.95f))
		{
			EntityBehaviorMilkable bh = entitySel?.Entity.GetBehavior<EntityBehaviorMilkable>();
			if (bh != null)
			{
				bh.MilkingComplete(slot, byEntity);
				handling = EnumHandling.PreventDefault;
			}
		}
	}
}
