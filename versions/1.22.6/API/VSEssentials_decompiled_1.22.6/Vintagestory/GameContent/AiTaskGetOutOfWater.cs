using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskGetOutOfWater : AiTaskBase
{
	private Vec3d target = new Vec3d();

	private BlockPos pos = new BlockPos(0);

	private bool done;

	private float moveSpeed = 0.03f;

	private int searchattempts;

	public AiTaskGetOutOfWater(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		moveSpeed = taskConfig["movespeed"].AsFloat(0.06f);
	}

	protected override void SetDefaultValues()
	{
		base.SetDefaultValues();
		ExecutionChance = 0.04;
		WhenSwimming = true;
	}

	public override bool ShouldExecute()
	{
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		if (base.rand.NextDouble() > ExecutionChance)
		{
			return false;
		}
		int range = GameMath.Min(50, 30 + searchattempts * 2);
		target.Y = entity.Pos.Y;
		int tries = 10;
		int px = (int)entity.Pos.X;
		int pz = (int)entity.Pos.Z;
		pos.SetDimension(entity.Pos.Dimension);
		IBlockAccessor blockAccessor = entity.World.BlockAccessor;
		Vec3d tmpPos = new Vec3d();
		while (tries-- > 0)
		{
			pos.X = px + base.rand.Next(range + 1) - range / 2;
			pos.Z = pz + base.rand.Next(range + 1) - range / 2;
			pos.Y = blockAccessor.GetTerrainMapheightAt(pos) + 1;
			if (!blockAccessor.GetBlock(pos, 2).IsLiquid())
			{
				blockAccessor.GetBlock(pos);
				if (!entity.World.CollisionTester.IsColliding(blockAccessor, entity.CollisionBox, tmpPos.Set((double)pos.X + 0.5, (float)pos.Y + 0.1f, (double)pos.Z + 0.5)) && entity.World.CollisionTester.IsColliding(blockAccessor, entity.CollisionBox, tmpPos.Set((double)pos.X + 0.5, (float)pos.Y - 0.1f, (double)pos.Z + 0.5)))
				{
					target.Set((double)pos.X + 0.5, pos.Y + 1, (double)pos.Z + 0.5);
					return true;
				}
			}
		}
		searchattempts++;
		return false;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		searchattempts = 0;
		done = false;
		pathTraverser.WalkTowards(target, moveSpeed, 0.5f, OnGoalReached, OnStuck);
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		if (base.rand.NextDouble() < 0.10000000149011612 && !entity.FeetInLiquid)
		{
			return false;
		}
		return !done;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		pathTraverser.Stop();
	}

	private void OnStuck()
	{
		done = true;
	}

	private void OnGoalReached()
	{
		done = true;
	}
}
