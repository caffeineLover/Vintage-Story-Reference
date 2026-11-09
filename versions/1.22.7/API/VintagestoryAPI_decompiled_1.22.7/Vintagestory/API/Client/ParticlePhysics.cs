using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Client;

public class ParticlePhysics
{
	public IBlockAccessor BlockAccess;

	public float PhysicsTickTime = 0.033f;

	public const float AsyncSpawnTime = 0.033f;

	private Cuboidd particleCollBox = new Cuboidd();

	private Cuboidd blockCollBox = new Cuboidd();

	private BlockPos tmpPos = new BlockPos(0);

	public CachedCuboidList CollisionBoxList = new CachedCuboidList();

	public float MotionCap = 2f;

	private BlockPos minPos = new BlockPos(0);

	private BlockPos maxPos = new BlockPos(0);

	public ParticlePhysics(IBlockAccessor blockAccess)
	{
		BlockAccess = blockAccess;
	}

	public Vec3f CollisionStrength(Vec3f velocitybefore, Vec3f velocitynow, float gravityStrength, float deltatime)
	{
		velocitybefore.Y -= gravityStrength * deltatime;
		return new Vec3f(velocitybefore.X - velocitynow.X, velocitybefore.Y - velocitynow.Y, velocitybefore.Z - velocitynow.Z);
	}

	public void HandleBoyancy(Vec3d pos, Vec3f velocity, bool boyant, float gravityStrength, float deltatime, float height)
	{
		int xPrev = (int)pos.X;
		int yPrev = (int)pos.Y;
		int zPrev = (int)pos.Z;
		tmpPos.SetAndCorrectDimension(xPrev, yPrev, zPrev);
		Block block = BlockAccess.GetBlock(tmpPos, 2);
		Block prevBlock = block;
		if (boyant)
		{
			if (block.IsLiquid())
			{
				block = BlockAccess.GetBlockAbove(tmpPos, 2);
				float swimlineSubmergedness = GameMath.Clamp((float)(int)pos.Y + (float)prevBlock.LiquidLevel / 8f + (block.IsLiquid() ? 1.125f : 0f) - (float)pos.Y + height, 0f, 1f);
				float boyancyStrength = GameMath.Clamp(9f * swimlineSubmergedness, -1.25f, 1.25f);
				velocity.Y += gravityStrength * deltatime * boyancyStrength;
				float waterDrag = GameMath.Clamp(30f * Math.Abs(velocity.Y) - 0.02f, 1f, 1.25f);
				velocity.Y /= waterDrag;
				velocity.X *= 0.99f;
				velocity.Z *= 0.99f;
				if (prevBlock is IBlockFlowing { IsStill: false } blockFlowing && swimlineSubmergedness >= 0f)
				{
					float factor = deltatime / 0.033f;
					FastVec3f pushVector = blockFlowing.GetPushVector(tmpPos);
					velocity.Add(pushVector * (7.5f * factor));
				}
			}
		}
		else if (prevBlock is IBlockFlowing { IsStill: false } blockFlowing2)
		{
			FastVec3f pushVector2 = blockFlowing2.GetPushVector(tmpPos);
			velocity.Add(pushVector2 * (30f * deltatime));
		}
	}

	/// <summary>
	/// Updates the velocity vector according to the amount of passed time, gravity and terrain collision.
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="motion"></param>
	/// <param name="size"></param>
	/// <returns></returns>
	public EnumCollideFlags UpdateMotion(Vec3d pos, Vec3f motion, float size)
	{
		particleCollBox.Set(pos.X - (double)(size / 2f), pos.Y - 0.0, pos.Z - (double)(size / 2f), pos.X + (double)(size / 2f), pos.Y + (double)(size / 2f), pos.Z + (double)(size / 2f));
		motion.X = GameMath.Clamp(motion.X, 0f - MotionCap, MotionCap);
		motion.Y = GameMath.Clamp(motion.Y, 0f - MotionCap, MotionCap);
		motion.Z = GameMath.Clamp(motion.Z, 0f - MotionCap, MotionCap);
		EnumCollideFlags flags = (EnumCollideFlags)0;
		minPos.SetAndCorrectDimension((int)(particleCollBox.X1 + (double)Math.Min(0f, motion.X)), (int)(particleCollBox.Y1 + (double)Math.Min(0f, motion.Y) - 1.0), (int)(particleCollBox.Z1 + (double)Math.Min(0f, motion.Z)));
		maxPos.SetAndCorrectDimension((int)(particleCollBox.X2 + (double)Math.Max(0f, motion.X)), (int)(particleCollBox.Y2 + (double)Math.Max(0f, motion.Y)), (int)(particleCollBox.Z2 + (double)Math.Max(0f, motion.Z)));
		tmpPos.SetDimension(minPos.dimension);
		particleCollBox.Y1 %= 32768.0;
		particleCollBox.Y2 %= 32768.0;
		CollisionBoxList.Clear();
		BlockAccess.WalkBlocks(minPos, maxPos, delegate(Block cblock, int x, int y, int z)
		{
			Cuboidf[] particleCollisionBoxes = cblock.GetParticleCollisionBoxes(BlockAccess, tmpPos.Set(x, y, z));
			if (particleCollisionBoxes != null)
			{
				CollisionBoxList.Add(particleCollisionBoxes, x, y, z, cblock);
			}
		});
		EnumPushDirection pushDirection = EnumPushDirection.None;
		for (int i = 0; i < CollisionBoxList.Count; i++)
		{
			blockCollBox = CollisionBoxList.cuboids[i];
			motion.Y = (float)blockCollBox.pushOutY(particleCollBox, motion.Y, ref pushDirection);
			if (pushDirection != EnumPushDirection.None)
			{
				flags |= EnumCollideFlags.CollideY;
			}
		}
		particleCollBox.Translate(0.0, motion.Y, 0.0);
		for (int i2 = 0; i2 < CollisionBoxList.Count; i2++)
		{
			blockCollBox = CollisionBoxList.cuboids[i2];
			motion.X = (float)blockCollBox.pushOutX(particleCollBox, motion.X, ref pushDirection);
			if (pushDirection != EnumPushDirection.None)
			{
				flags |= EnumCollideFlags.CollideX;
			}
		}
		particleCollBox.Translate(motion.X, 0.0, 0.0);
		for (int i3 = 0; i3 < CollisionBoxList.Count; i3++)
		{
			blockCollBox = CollisionBoxList.cuboids[i3];
			motion.Z = (float)blockCollBox.pushOutZ(particleCollBox, motion.Z, ref pushDirection);
			if (pushDirection != EnumPushDirection.None)
			{
				flags |= EnumCollideFlags.CollideZ;
			}
		}
		return flags;
	}
}
