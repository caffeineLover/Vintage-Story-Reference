using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskIdleR : AiTaskBaseTargetableR
{
	private AiTaskIdleConfig Config => GetConfig<AiTaskIdleConfig>();

	public AiTaskIdleR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		baseConfig = AiTaskBaseR.LoadConfig<AiTaskIdleConfig>(entity, taskConfig, aiConfig);
	}

	public override bool ShouldExecute()
	{
		if (!PreconditionsSatisficed())
		{
			return false;
		}
		if (CheckForTargetToStop())
		{
			return false;
		}
		if (!CheckForBlockBelow())
		{
			return false;
		}
		return true;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		entity.IdleSoundChanceModifier = 0f;
	}

	public override bool ContinueExecute(float dt)
	{
		if (!base.ContinueExecute(dt))
		{
			return false;
		}
		if (base.Rand.NextDouble() <= (double)Config.ChanceToCheckTarget && CheckForTargetToStop())
		{
			return false;
		}
		return true;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		entity.IdleSoundChanceModifier = 1f;
	}

	protected virtual bool CheckForTargetToStop()
	{
		if (!Config.StopWhenTargetDetected)
		{
			return false;
		}
		if (!CheckAndResetSearchCooldown())
		{
			return false;
		}
		return SearchForTarget();
	}

	protected virtual bool CheckForBlockBelow()
	{
		Block belowBlock = entity.World.BlockAccessor.GetBlockRaw((int)entity.Pos.X, (int)entity.Pos.InternalY - 1, (int)entity.Pos.Z, 1);
		if (Config.CheckForSolidUpSide && !belowBlock.SideSolid[BlockFacing.UP.Index])
		{
			return false;
		}
		if (Config.IgnoreBlockCodeAndTags)
		{
			return true;
		}
		Block block = entity.World.BlockAccessor.GetBlockRaw((int)entity.Pos.X, (int)entity.Pos.InternalY, (int)entity.Pos.Z);
		if (block.Replaceable >= Config.MinBlockInsideReplaceable)
		{
			return CheckForBlock(belowBlock);
		}
		return CheckForBlock(block);
	}

	protected virtual bool CheckForBlock(Block block)
	{
		if (Config.IgnoreBlockCodeAndTags)
		{
			return true;
		}
		if (!Config.AllowedBlockBelowTags.IsEmpty && !Config.AllowedBlockBelowTags.Overlaps(in block.Tags))
		{
			return false;
		}
		if (!Config.SkipBlockBelowTags.IsEmpty && Config.SkipBlockBelowTags.Overlaps(in block.Tags))
		{
			return false;
		}
		if (Config.AllowedBlockBelowCode != null && !block.WildCardMatch(Config.AllowedBlockBelowCode))
		{
			return false;
		}
		return true;
	}
}
