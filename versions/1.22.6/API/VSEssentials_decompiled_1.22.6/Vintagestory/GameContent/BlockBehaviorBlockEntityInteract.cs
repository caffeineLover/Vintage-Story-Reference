using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockBehaviorBlockEntityInteract : BlockBehavior
{
	public delegate bool BEBehaviorDelegate(IInteractable iint, ref EnumHandling handling);

	public BlockBehaviorBlockEntityInteract(Block block)
		: base(block)
	{
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		EnumHandling iihandling = handling;
		bool result = walkInteractables(world, blockSel.Position, delegate(IInteractable ii, ref EnumHandling iiihandling)
		{
			bool result2 = ii.OnBlockInteractStart(world, byPlayer, blockSel, ref iiihandling);
			if (iiihandling != EnumHandling.PassThrough)
			{
				iihandling = iiihandling;
			}
			return result2;
		}, delegate
		{
			base.OnBlockInteractStart(world, byPlayer, blockSel, ref iihandling);
		});
		handling = iihandling;
		return result;
	}

	public override bool OnBlockInteractCancel(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		EnumHandling iihandling = handling;
		bool result = walkInteractables(world, blockSel.Position, delegate(IInteractable ii, ref EnumHandling iiihandling)
		{
			if (ii is ILongInteractable longInteractable)
			{
				bool result2 = longInteractable.OnBlockInteractCancel(secondsUsed, world, byPlayer, blockSel, ref iiihandling);
				if (iiihandling != EnumHandling.PassThrough)
				{
					iihandling = iiihandling;
				}
				return result2;
			}
			iihandling = EnumHandling.PassThrough;
			return true;
		}, delegate
		{
			base.OnBlockInteractCancel(secondsUsed, world, byPlayer, blockSel, ref iihandling);
		});
		handling = iihandling;
		return result;
	}

	public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		EnumHandling iihandling = handling;
		bool result = walkInteractables(world, blockSel.Position, delegate(IInteractable ii, ref EnumHandling iiihandling)
		{
			if (ii is ILongInteractable longInteractable)
			{
				bool result2 = longInteractable.OnBlockInteractStep(secondsUsed, world, byPlayer, blockSel, ref iiihandling);
				if (iiihandling != EnumHandling.PassThrough)
				{
					iihandling = iiihandling;
				}
				return result2;
			}
			iihandling = EnumHandling.PassThrough;
			return true;
		}, delegate
		{
			base.OnBlockInteractStep(secondsUsed, world, byPlayer, blockSel, ref iihandling);
		});
		handling = iihandling;
		return result;
	}

	public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		EnumHandling iihandling = handling;
		walkInteractables(world, blockSel.Position, delegate(IInteractable ii, ref EnumHandling iiihandling)
		{
			if (ii is ILongInteractable longInteractable)
			{
				longInteractable.OnBlockInteractStop(secondsUsed, world, byPlayer, blockSel, ref iiihandling);
				if (iiihandling != EnumHandling.PassThrough)
				{
					iihandling = iiihandling;
				}
				return true;
			}
			iihandling = EnumHandling.PassThrough;
			return true;
		}, delegate
		{
			base.OnBlockInteractStop(secondsUsed, world, byPlayer, blockSel, ref iihandling);
		});
		handling = iihandling;
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection blockSel, IPlayer forPlayer, ref EnumHandling handling)
	{
		EnumHandling iihandling = handling;
		WorldInteraction[] wis = new WorldInteraction[0];
		walkInteractables(world, blockSel.Position, delegate(IInteractable ii, ref EnumHandling reference)
		{
			if (ii is IInteractableWithHelp interactableWithHelp)
			{
				WorldInteraction[] placedBlockInteractionHelp = interactableWithHelp.GetPlacedBlockInteractionHelp(world, blockSel, forPlayer, ref reference);
				wis = wis.Append(placedBlockInteractionHelp);
				return false;
			}
			reference = EnumHandling.PassThrough;
			return false;
		}, delegate
		{
			base.GetPlacedBlockInteractionHelp(world, blockSel, forPlayer, ref iihandling);
		});
		handling = iihandling;
		return wis;
	}

	protected bool walkInteractables(IWorldAccessor world, BlockPos pos, BEBehaviorDelegate onBehavior, Action defaultAction)
	{
		bool executeDefault = true;
		EnumHandling iiHandling = EnumHandling.PassThrough;
		bool resultValue = false;
		BlockEntity be = world.BlockAccessor.GetBlockEntity(pos);
		if (be == null)
		{
			return false;
		}
		if (be is IInteractable ii)
		{
			resultValue = onBehavior(ii, ref iiHandling);
		}
		if (iiHandling == EnumHandling.PreventSubsequent)
		{
			return resultValue;
		}
		foreach (BlockEntityBehavior behavior in be.Behaviors)
		{
			bool iiresult = false;
			iiHandling = EnumHandling.PassThrough;
			if (behavior is IInteractable iibh)
			{
				iiresult = onBehavior(iibh, ref iiHandling);
			}
			switch (iiHandling)
			{
			case EnumHandling.PreventSubsequent:
				return iiresult;
			case EnumHandling.PreventDefault:
				executeDefault = false;
				resultValue = iiresult;
				break;
			}
		}
		if (executeDefault)
		{
			defaultAction();
		}
		return resultValue;
	}
}
