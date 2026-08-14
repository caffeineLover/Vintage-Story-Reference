using System.Collections.Generic;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Systems;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BlockBehaviorLockable : BlockBehavior
{
	public BlockBehaviorLockable(Block block)
		: base(block)
	{
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		if (world.Api.ModLoader.GetModSystem<ModSystemBlockReinforcement>().IsLockedForInteract(blockSel.Position, byPlayer))
		{
			if (world.Side == EnumAppSide.Client)
			{
				(world.Api as ICoreClientAPI).TriggerIngameError(this, "locked", Lang.Get("ingameerror-locked"));
			}
			handling = EnumHandling.PreventSubsequent;
			return false;
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position);
		BEBehaviorDoor bed = blockEntity?.GetBehavior<BEBehaviorDoor>();
		if (bed != null && bed.StoryLockedCode != null)
		{
			if (!(world.Api is ICoreClientAPI capi))
			{
				handling = EnumHandling.PreventSubsequent;
				return false;
			}
			handling = EnumHandling.Handled;
			if (capi.ModLoader.GetModSystem<StoryLockableDoor>().StoryLockedLocationCodes.TryGetValue(bed.StoryLockedCode, out HashSet<string> list) && list.Contains(byPlayer.PlayerUID))
			{
				return true;
			}
			capi.TriggerIngameError(this, "locked", Lang.Get("ingameerror-locked"));
			handling = EnumHandling.PreventSubsequent;
			return false;
		}
		return (blockEntity?.GetBehavior<BEBehaviorDoorBarLock>())?.OnBlockInteractStart(world, byPlayer, blockSel, ref handling) ?? base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
	}
}
