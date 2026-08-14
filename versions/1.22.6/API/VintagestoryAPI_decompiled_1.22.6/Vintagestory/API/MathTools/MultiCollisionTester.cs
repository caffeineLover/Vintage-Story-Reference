using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.API.MathTools;

public class MultiCollisionTester
{
	public CachedCuboidList CollisionBoxList = new CachedCuboidList();

	public Cuboidd[] entityBox = ArrayUtil.CreateFilled(10, (int i) => new Cuboidd());

	protected int count;

	public BlockPos tmpPos = new BlockPos(0);

	public Vec3d tmpPosDelta = new Vec3d();

	public BlockPos minPos = new BlockPos(0);

	public BlockPos maxPos = new BlockPos(0);

	public Vec3d pos = new Vec3d();

	public void ApplyTerrainCollision(Cuboidf[] collisionBoxes, int collBoxCount, Entity entity, EntityPos entityPos, float dtFactor, ref Vec3d newPosition, float stepHeight = 1f, float yExtra = 1f)
	{
		count = collBoxCount;
		IWorldAccessor world = entity.World;
		pos.X = entityPos.X;
		pos.Y = entityPos.Y;
		pos.Z = entityPos.Z;
		EnumPushDirection pushDirection = EnumPushDirection.None;
		for (int i = 0; i < collBoxCount; i++)
		{
			entityBox[i].SetAndTranslate(collisionBoxes[i], pos.X, pos.Y, pos.Z);
		}
		double motionX = entityPos.Motion.X * (double)dtFactor;
		double motionY = entityPos.Motion.Y * (double)dtFactor;
		double motionZ = entityPos.Motion.Z * (double)dtFactor;
		GenerateCollisionBoxList(world.BlockAccessor, motionX, motionY, motionZ, stepHeight, yExtra, entityPos.Dimension);
		bool collided = false;
		int collisionBoxListCount = CollisionBoxList.Count;
		for (int j = 0; j < collisionBoxListCount; j++)
		{
			for (int k = 0; k < collBoxCount; k++)
			{
				motionY = CollisionBoxList.cuboids[j].pushOutY(entityBox[k], motionY, ref pushDirection);
				if (pushDirection != EnumPushDirection.None)
				{
					CollisionBoxList.blocks[j].OnEntityCollide(world, entity, CollisionBoxList.positions[j], (pushDirection == EnumPushDirection.Negative) ? BlockFacing.UP : BlockFacing.DOWN, tmpPosDelta.Set(motionX, motionY, motionZ), !entity.CollidedVertically);
					collided = true;
				}
			}
		}
		for (int l = 0; l < collBoxCount; l++)
		{
			entityBox[l].Translate(0.0, motionY, 0.0);
		}
		entity.CollidedVertically = collided;
		bool horizontallyBlocked = false;
		for (int m = 0; m < collBoxCount; m++)
		{
			entityBox[m].Translate(motionX, 0.0, motionZ);
		}
		foreach (Cuboidd cuboid in CollisionBoxList)
		{
			bool blocked = false;
			for (int n = 0; n < collBoxCount; n++)
			{
				if (cuboid.Intersects(entityBox[n]))
				{
					horizontallyBlocked = true;
					blocked = true;
					break;
				}
			}
			if (blocked)
			{
				break;
			}
		}
		for (int num = 0; num < collBoxCount; num++)
		{
			entityBox[num].Translate(0.0 - motionX, 0.0, 0.0 - motionZ);
		}
		collided = false;
		if (horizontallyBlocked)
		{
			for (int num2 = 0; num2 < collisionBoxListCount; num2++)
			{
				bool pushed = false;
				for (int num3 = 0; num3 < collBoxCount; num3++)
				{
					motionX = CollisionBoxList.cuboids[num2].pushOutX(entityBox[num3], motionX, ref pushDirection);
					if (pushDirection != EnumPushDirection.None)
					{
						CollisionBoxList.blocks[num2].OnEntityCollide(world, entity, CollisionBoxList.positions[num2], (pushDirection == EnumPushDirection.Negative) ? BlockFacing.EAST : BlockFacing.WEST, tmpPosDelta.Set(motionX, motionY, motionZ), !entity.CollidedHorizontally);
					}
					pushed = pushed || pushDirection != EnumPushDirection.None;
				}
				collided = pushed;
			}
			for (int num4 = 0; num4 < collBoxCount; num4++)
			{
				entityBox[num4].Translate(motionX, 0.0, 0.0);
			}
			for (int num5 = 0; num5 < collisionBoxListCount; num5++)
			{
				bool pushed2 = false;
				for (int num6 = 0; num6 < collBoxCount; num6++)
				{
					motionZ = CollisionBoxList.cuboids[num5].pushOutZ(entityBox[num6], motionZ, ref pushDirection);
					if (pushDirection != EnumPushDirection.None)
					{
						CollisionBoxList.blocks[num5].OnEntityCollide(world, entity, CollisionBoxList.positions[num5], (pushDirection == EnumPushDirection.Negative) ? BlockFacing.SOUTH : BlockFacing.NORTH, tmpPosDelta.Set(motionX, motionY, motionZ), !entity.CollidedHorizontally);
					}
					pushed2 = pushed2 || pushDirection != EnumPushDirection.None;
				}
				collided = pushed2;
			}
		}
		entity.CollidedHorizontally = collided;
		newPosition.Set(pos.X + motionX, pos.Y + motionY, pos.Z + motionZ);
	}

	protected virtual void GenerateCollisionBoxList(IBlockAccessor blockAccessor, double motionX, double motionY, double motionZ, float stepHeight, float yExtra, int dimension)
	{
		double minx = double.MaxValue;
		double miny = double.MaxValue;
		double minz = double.MaxValue;
		double maxx = double.MinValue;
		double maxy = double.MinValue;
		double maxz = double.MinValue;
		for (int i = 0; i < count; i++)
		{
			Cuboidd ebox = entityBox[i];
			minx = Math.Min(minx, ebox.X1);
			miny = Math.Min(miny, ebox.Y1);
			minz = Math.Min(minz, ebox.Z1);
			maxx = Math.Max(maxx, ebox.X2);
			maxy = Math.Max(maxy, ebox.Y2);
			maxz = Math.Max(maxz, ebox.Z2);
		}
		minPos.Set((int)(minx + Math.Min(0.0, motionX)), (int)(miny + Math.Min(0.0, motionY) - (double)yExtra), (int)(minz + Math.Min(0.0, motionZ)));
		double y2 = Math.Max(miny + (double)stepHeight, maxy);
		maxPos.Set((int)(maxx + Math.Max(0.0, motionX)), (int)(y2 + Math.Max(0.0, motionY)), (int)(maxz + Math.Max(0.0, motionZ)));
		minPos.SetDimension(dimension);
		tmpPos.SetDimension(dimension);
		CollisionBoxList.Clear();
		blockAccessor.WalkBlocks(minPos, maxPos, delegate(Block block, int x, int y3, int z)
		{
			Cuboidf[] collisionBoxes = block.GetCollisionBoxes(blockAccessor, tmpPos.Set(x, y3, z));
			if (collisionBoxes != null)
			{
				CollisionBoxList.Add(collisionBoxes, x, y3, z, block);
			}
		}, centerOrder: true);
	}

	/// <summary>
	/// If given cuboidf collides with the terrain, returns the collision box it collides with. By default also checks if the cuboid is merely touching the terrain, set alsoCheckTouch to disable that.
	/// </summary>
	/// <param name="blockAccessor"></param>
	/// <param name="ecollisionBoxes"></param>
	/// <param name="collBoxCount"></param>
	/// <param name="pos"></param>
	/// <param name="alsoCheckTouch"></param>
	/// <returns></returns>
	public Cuboidd GetCollidingCollisionBox(IBlockAccessor blockAccessor, Cuboidf[] ecollisionBoxes, int collBoxCount, Vec3d pos, bool alsoCheckTouch = true)
	{
		BlockPos blockPos = new BlockPos(0);
		Vec3d blockPosVec = new Vec3d();
		for (int j = 0; j < collBoxCount; j++)
		{
			Cuboidf obj = ecollisionBoxes[j];
			Cuboidd entityBox = obj.ToDouble().Translate(pos);
			entityBox.Y1 = Math.Round(entityBox.Y1, 5);
			int minX = (int)((double)obj.X1 + pos.X);
			int minY = (int)((double)obj.Y1 + pos.Y - 1.0);
			int minZ = (int)((double)obj.Z1 + pos.Z);
			int maxX = (int)Math.Ceiling((double)obj.X2 + pos.X);
			int maxY = (int)Math.Ceiling((double)obj.Y2 + pos.Y);
			int maxZ = (int)Math.Ceiling((double)obj.Z2 + pos.Z);
			for (int y = minY; y <= maxY; y++)
			{
				for (int x = minX; x <= maxX; x++)
				{
					for (int z = minZ; z <= maxZ; z++)
					{
						Block mostSolidBlock = blockAccessor.GetMostSolidBlock(x, y, z);
						blockPos.Set(x, y, z);
						blockPosVec.Set(x, y, z);
						Cuboidf[] collisionBoxes = mostSolidBlock.GetCollisionBoxes(blockAccessor, blockPos);
						if (collisionBoxes == null)
						{
							continue;
						}
						foreach (Cuboidf collBox in collisionBoxes)
						{
							if (collBox != null && (alsoCheckTouch ? entityBox.IntersectsOrTouches(collBox, blockPosVec) : entityBox.Intersects(collBox, blockPosVec)))
							{
								return collBox.ToDouble().Translate(blockPos);
							}
						}
					}
				}
			}
		}
		return null;
	}
}
