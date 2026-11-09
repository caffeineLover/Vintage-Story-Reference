using System;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common.Entities;

public class PModulePlayerInLiquid : PModuleInLiquid
{
	private long lastPush;

	private readonly IPlayer player;

	private readonly BlockPos tmpPos = new BlockPos(0);

	public PModulePlayerInLiquid(EntityPlayer entityPlayer)
	{
		player = entityPlayer.World.PlayerByUid(entityPlayer.PlayerUID);
	}

	public override void HandleSwimming(float dt, Entity entity, EntityPos pos, EntityControls controls)
	{
		if ((controls.TriesToMove || controls.Jump) && entity.World.ElapsedMilliseconds - lastPush > 2000)
		{
			Push = 6f;
			lastPush = entity.World.ElapsedMilliseconds;
			entity.PlayEntitySound("swim", player);
		}
		else
		{
			Push = Math.Max(1f, Push - 0.1f * dt * 60f);
		}
		tmpPos.SetDimension(pos.Dimension);
		tmpPos.Set((int)pos.X, (int)pos.Y, (int)pos.Z);
		Block inBlock = entity.World.BlockAccessor.GetBlock(tmpPos, 2);
		Block aboveBlock = entity.World.BlockAccessor.GetBlockAbove(tmpPos, 1, 2);
		Block twoAboveBlock = entity.World.BlockAccessor.GetBlockAbove(tmpPos, 2, 2);
		float swimLineSubmergedness = GameMath.Clamp((float)(int)pos.Y + (float)inBlock.LiquidLevel / 8f + (aboveBlock.IsLiquid() ? 1.125f : 0f) + (twoAboveBlock.IsLiquid() ? 1.125f : 0f) - (float)pos.Y - (float)entity.SwimmingOffsetY, 0f, 1f);
		swimLineSubmergedness = Math.Min(1f, swimLineSubmergedness + 0.075f);
		double yMotion = 0.0;
		if (controls.Jump)
		{
			if (swimLineSubmergedness > 0.1f || !controls.TriesToMove)
			{
				yMotion = 0.005f * swimLineSubmergedness * dt * 60f;
			}
		}
		else
		{
			yMotion = controls.FlyVector.Y * (double)(1f + Push) * 0.029999999329447746 * (double)swimLineSubmergedness;
		}
		pos.Motion.Add(controls.FlyVector.X * (double)(1f + Push) * 0.029999999329447746, yMotion, controls.FlyVector.Z * (double)(1f + Push) * 0.029999999329447746);
	}
}
