using System;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common.Entities;

public class PModuleInLiquid : PModule
{
	public float Push;

	public float Swimspeed = 1f;

	[ThreadStatic]
	private static BlockPos tmpPos;

	public override void Initialize(JsonObject config, Entity entity)
	{
		if (!(entity is EntityPlayer))
		{
			Swimspeed = (float)entity.World.Config.GetDecimal("creatureSwimSpeed", 1.0);
		}
	}

	public override bool Applicable(Entity entity, EntityPos pos, EntityControls controls)
	{
		return entity.FeetInLiquid;
	}

	public override void DoApply(float dt, Entity entity, EntityPos pos, EntityControls controls)
	{
		if (entity.Swimming && entity.Alive)
		{
			HandleSwimming(dt, entity, pos, controls);
		}
		if ((object)tmpPos == null)
		{
			tmpPos = new BlockPos(pos.Dimension);
		}
		tmpPos.Set(pos);
		if (entity.World.BlockAccessor.GetBlock(tmpPos, 2) is IBlockFlowing { IsStill: false } blockFlowing)
		{
			FastVec3f pushVector = blockFlowing.GetPushVector(tmpPos);
			if (pushVector.Y >= 0f || !entity.World.BlockAccessor.IsSideSolid((int)pos.X, (int)pos.InternalY - 1, (int)pos.Z, BlockFacing.UP))
			{
				pos.Motion.Add(pushVector);
			}
		}
	}

	public virtual void HandleSwimming(float dt, Entity entity, EntityPos pos, EntityControls controls)
	{
		Push = Math.Max(1f, Push - 0.1f * dt * 60f);
		int x = (int)pos.X;
		int y = (int)pos.InternalY;
		int z = (int)pos.Z;
		Block inBlock = entity.World.BlockAccessor.GetBlockRaw(x, y, z, 2);
		Block aboveBlock = entity.World.BlockAccessor.GetBlockRaw(x, y + 1, z, 2);
		Block twoAboveBlock = entity.World.BlockAccessor.GetBlockRaw(x, y + 2, z, 2);
		float swimLineSubmergedness = GameMath.Clamp((float)(int)pos.Y + (float)inBlock.LiquidLevel / 8f + (aboveBlock.IsLiquid() ? 1.125f : 0f) + (twoAboveBlock.IsLiquid() ? 1.125f : 0f) - (float)pos.Y - (float)entity.SwimmingOffsetY, 0f, 1f);
		swimLineSubmergedness = Math.Min(1f, swimLineSubmergedness + 0.075f);
		double yMotion = ((!controls.Jump) ? (controls.FlyVector.Y * (double)(1f + Push) * 0.029999999329447746 * (double)swimLineSubmergedness) : ((double)(0.005f * swimLineSubmergedness * dt * 60f)));
		if (entity.Properties.Habitat == EnumHabitat.Underwater && inBlock.IsLiquid() && !aboveBlock.IsLiquid())
		{
			float maxY = (float)(int)pos.Y + (float)inBlock.LiquidLevel / 8f - entity.CollisionBox.Y2;
			if (pos.Y > (double)maxY)
			{
				yMotion = 0.0 - GameMath.Clamp(pos.Y - (double)maxY, 0.0, 0.05);
			}
		}
		pos.Motion.Add((double)Swimspeed * controls.FlyVector.X * (double)(1f + Push) * 0.029999999329447746, (double)Swimspeed * yMotion, (double)Swimspeed * controls.FlyVector.Z * (double)(1f + Push) * 0.029999999329447746);
	}
}
