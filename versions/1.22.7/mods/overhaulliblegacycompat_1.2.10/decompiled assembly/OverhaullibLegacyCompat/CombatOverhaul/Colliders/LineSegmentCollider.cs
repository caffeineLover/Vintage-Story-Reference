using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000173 RID: 371
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct LineSegmentCollider : IWeaponCollider, ICollider
	{
		// Token: 0x0600126B RID: 4715 RVA: 0x0005957D File Offset: 0x0005777D
		public LineSegmentCollider(Vector3d position, Vector3d direction)
		{
			this.Position = position;
			this.Direction = direction;
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00059590 File Offset: 0x00057790
		public LineSegmentCollider(JsonObject json)
		{
			this.Position = new Vector3d((double)json["X1"].AsFloat(0f), (double)json["Y1"].AsFloat(0f), (double)json["Z1"].AsFloat(0f));
			this.Direction = new Vector3d((double)json["X2"].AsFloat(0f), (double)json["Y2"].AsFloat(0f), (double)json["Z2"].AsFloat(0f));
			this.Direction -= this.Position;
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00059650 File Offset: 0x00057850
		public LineSegmentCollider(params float[] positions)
		{
			this.Position = new Vector3d((double)positions[0], (double)positions[1], (double)positions[2]);
			this.Direction = new Vector3d((double)positions[3], (double)positions[4], (double)positions[5]);
			this.Direction -= this.Position;
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x000596A4 File Offset: 0x000578A4
		public void Render(ICoreClientAPI api, EntityAgent entityPlayer, int color = -1)
		{
			BlockPos playerPos = entityPlayer.Pos.AsBlockPos;
			Vector3d playerPosVector;
			playerPosVector..ctor((double)playerPos.X, (double)playerPos.Y, (double)playerPos.Z);
			Vector3d tail = this.Position - playerPosVector;
			Vector3d head = this.Position + this.Direction - playerPosVector;
			api.Render.RenderLine(playerPos, (float)tail.X, (float)tail.Y, (float)tail.Z, (float)head.X, (float)head.Y, (float)head.Z, color);
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00059738 File Offset: 0x00057938
		[return: Nullable(2)]
		public ICollider Transform(EntityPos origin, EntityAgent entity, ItemSlot itemSlot, ICoreClientAPI api, bool right = true)
		{
			Matrixf modelMatrix = ColliderTools.GetHeldItemModelMatrix(entity, itemSlot, api, right);
			if (modelMatrix == null)
			{
				return null;
			}
			return LineSegmentCollider.TransformSegment(this, modelMatrix, origin);
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00059768 File Offset: 0x00057968
		public ICollider Transform(Matrixf modelMatrix, EntityPos origin)
		{
			Vector3d tail = ColliderTools.TransformVector(this.Position, modelMatrix, origin);
			Vector3d head = ColliderTools.TransformVector(this.Direction + this.Position, modelMatrix, origin);
			return new LineSegmentCollider(tail, head - tail);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x000597B0 File Offset: 0x000579B0
		public LineSegmentCollider? TransformLineSegment(EntityPos origin, EntityAgent entity, ItemSlot itemSlot, ICoreClientAPI api, bool right = true)
		{
			Matrixf modelMatrix = ColliderTools.GetHeldItemModelMatrix(entity, itemSlot, api, right);
			if (modelMatrix == null)
			{
				return default(LineSegmentCollider?);
			}
			return new LineSegmentCollider?(LineSegmentCollider.TransformSegment(this, modelMatrix, origin));
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x000597E8 File Offset: 0x000579E8
		public bool RoughIntersect(Cuboidf collisionBox)
		{
			return ((double)collisionBox.MaxX >= this.Position.X || (double)collisionBox.MaxX >= this.Position.X + this.Direction.X) && ((double)collisionBox.MinX <= this.Position.X || (double)collisionBox.MinX <= this.Position.X + this.Direction.X) && ((double)collisionBox.MaxY >= this.Position.Y || (double)collisionBox.MaxY >= this.Position.Y + this.Direction.Y) && ((double)collisionBox.MinY <= this.Position.Y || (double)collisionBox.MinY <= this.Position.Y + this.Direction.Y) && ((double)collisionBox.MaxZ >= this.Position.Z || (double)collisionBox.MaxZ >= this.Position.Z + this.Direction.Z) && ((double)collisionBox.MinZ <= this.Position.Z || (double)collisionBox.MinZ <= this.Position.Z + this.Direction.Z);
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0005993C File Offset: 0x00057B3C
		public Vector3d? IntersectCuboids(IEnumerable<Cuboidf> collisionBoxes)
		{
			Vector3d? closestIntersection = default(Vector3d?);
			double closestParameter = double.MaxValue;
			foreach (Cuboidf collisionBox in collisionBoxes)
			{
				double parameter;
				Vector3d? intersection = this.IntersectCuboid(collisionBox, out parameter);
				if (intersection != null && parameter < closestParameter)
				{
					closestIntersection = intersection;
					closestParameter = parameter;
				}
			}
			return closestIntersection;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x000599B0 File Offset: 0x00057BB0
		public Vector3d? IntersectCuboid(Cuboidf collisionBox, out double parameter)
		{
			double tMin = 0.0;
			double tMax = 1.0;
			parameter = 1.0;
			if (!LineSegmentCollider.CheckAxisIntersection(this.Direction.X, this.Position.X, (double)collisionBox.MinX, (double)collisionBox.MaxX, ref tMin, ref tMax))
			{
				return default(Vector3d?);
			}
			if (!LineSegmentCollider.CheckAxisIntersection(this.Direction.Y, this.Position.Y, (double)collisionBox.MinY, (double)collisionBox.MaxY, ref tMin, ref tMax))
			{
				return default(Vector3d?);
			}
			if (!LineSegmentCollider.CheckAxisIntersection(this.Direction.Z, this.Position.Z, (double)collisionBox.MinZ, (double)collisionBox.MaxZ, ref tMin, ref tMax))
			{
				return default(Vector3d?);
			}
			parameter = tMin;
			return new Vector3d?(this.Position + tMin * this.Direction);
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00059AA6 File Offset: 0x00057CA6
		[return: TupleElementNames(new string[]
		{
			"block",
			"position",
			"parameter"
		})]
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		public ValueTuple<Block, Vector3d, double>? IntersectBlock(ICoreClientAPI api, BlockPos position)
		{
			return this.IntersectBlock(api.World.BlockAccessor, position.X, position.Y, position.Z);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00059ACC File Offset: 0x00057CCC
		[return: TupleElementNames(new string[]
		{
			"block",
			"position",
			"parameter"
		})]
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		public ValueTuple<Block, Vector3d, double>? IntersectTerrain(ICoreClientAPI api)
		{
			int minX = (int)Math.Min(this.Position.X, this.Position.X + this.Direction.X);
			int num = (int)Math.Min(this.Position.Y, this.Position.Y + this.Direction.Y);
			int minZ = (int)Math.Min(this.Position.Z, this.Position.Z + this.Direction.Z);
			int maxX = (int)Math.Max(this.Position.X, this.Position.X + this.Direction.X);
			int maxY = (int)Math.Max(this.Position.Y, this.Position.Y + this.Direction.Y);
			int maxZ = (int)Math.Max(this.Position.Z, this.Position.Z + this.Direction.Z);
			ValueTuple<Block, Vector3d, double>? closestIntersection = default(ValueTuple<Block, Vector3d, double>?);
			for (int y = num; y <= maxY; y++)
			{
				for (int x = minX; x <= maxX; x++)
				{
					for (int z = minZ; z <= maxZ; z++)
					{
						ValueTuple<Block, Vector3d, double>? intersection = this.IntersectBlock(api.World.BlockAccessor, x, y, z);
						ValueTuple<Block, Vector3d, double>? valueTuple = closestIntersection;
						if (valueTuple == null)
						{
							closestIntersection = intersection;
						}
						if (closestIntersection != null && intersection != null && closestIntersection.Value.Item3 > intersection.Value.Item3)
						{
							closestIntersection = intersection;
						}
					}
				}
			}
			return closestIntersection;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00059C6C File Offset: 0x00057E6C
		public static IEnumerable<LineSegmentCollider> Transform(IEnumerable<LineSegmentCollider> segments, EntityAgent entity, ItemSlot itemSlot, ICoreClientAPI api, bool right = true)
		{
			EntityPos playerPos = entity.Pos;
			Matrixf modelMatrix = ColliderTools.GetHeldItemModelMatrix(entity, itemSlot, api, right);
			if (modelMatrix == null)
			{
				return Array.Empty<LineSegmentCollider>();
			}
			return Enumerable.Select<LineSegmentCollider, LineSegmentCollider>(segments, (LineSegmentCollider segment) => LineSegmentCollider.TransformSegment(segment, modelMatrix, playerPos));
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00059CBC File Offset: 0x00057EBC
		public static bool Transform(IEnumerable<IHasLineCollider> segments, EntityAgent entity, ItemSlot itemSlot, ICoreClientAPI api, bool right = true)
		{
			EntityPos playerPos = entity.Pos;
			Matrixf modelMatrix = ColliderTools.GetHeldItemModelMatrix(entity, itemSlot, api, right);
			if (modelMatrix == null)
			{
				return false;
			}
			foreach (IHasLineCollider damageType in segments)
			{
				if (damageType.PreviousInWorldCollider.Position != damageType.RelativeCollider.Position)
				{
					damageType.PreviousInWorldCollider = damageType.InWorldCollider;
				}
				damageType.InWorldCollider = LineSegmentCollider.TransformSegment(damageType.RelativeCollider, modelMatrix, playerPos);
				if (damageType.PreviousInWorldCollider.Position == damageType.RelativeCollider.Position)
				{
					damageType.PreviousInWorldCollider = damageType.InWorldCollider;
				}
			}
			return true;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00059D7C File Offset: 0x00057F7C
		public static void ResetPreviousColliders(IEnumerable<IHasLineCollider> segments)
		{
			foreach (IHasLineCollider hasLineCollider in segments)
			{
				hasLineCollider.PreviousInWorldCollider = hasLineCollider.RelativeCollider;
			}
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00059DC8 File Offset: 0x00057FC8
		private static LineSegmentCollider TransformSegment(LineSegmentCollider value, Matrixf modelMatrix, EntityPos playerPos)
		{
			Vector3d tail = ColliderTools.TransformVector(value.Position, modelMatrix, playerPos);
			Vector3d head = ColliderTools.TransformVector(value.Direction + value.Position, modelMatrix, playerPos);
			return new LineSegmentCollider(tail, head - tail);
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00059E0C File Offset: 0x0005800C
		private static bool CheckAxisIntersection(double dirComponent, double startComponent, double minComponent, double maxComponent, ref double tMin, ref double tMax)
		{
			if (Math.Abs(dirComponent) < 9.999999974752427E-07)
			{
				if (startComponent < minComponent || startComponent > maxComponent)
				{
					return false;
				}
			}
			else
			{
				double t = (minComponent - startComponent) / dirComponent;
				double t2 = (maxComponent - startComponent) / dirComponent;
				if (t > t2)
				{
					double num = t;
					t = t2;
					t2 = num;
				}
				tMin = Math.Max(tMin, t);
				tMax = Math.Min(tMax, t2);
				if (tMin > tMax)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00059E6C File Offset: 0x0005806C
		[NullableContext(0)]
		private ValueTuple<double, Vector3d?> IntersectBlockCollisionBox([Nullable(1)] Cuboidf collisionBox, int x, int y, int z)
		{
			double tMin = 0.0;
			double tMax = 1.0;
			if (!LineSegmentCollider.CheckAxisIntersection(this.Direction.X, this.Position.X, (double)(collisionBox.MinX + (float)x), (double)(collisionBox.MaxX + (float)x), ref tMin, ref tMax))
			{
				return new ValueTuple<double, Vector3d?>(0.0, default(Vector3d?));
			}
			if (!LineSegmentCollider.CheckAxisIntersection(this.Direction.Y, this.Position.Y, (double)(collisionBox.MinY + (float)y), (double)(collisionBox.MaxY + (float)y), ref tMin, ref tMax))
			{
				return new ValueTuple<double, Vector3d?>(0.0, default(Vector3d?));
			}
			if (!LineSegmentCollider.CheckAxisIntersection(this.Direction.Z, this.Position.Z, (double)(collisionBox.MinZ + (float)z), (double)(collisionBox.MaxZ + (float)z), ref tMin, ref tMax))
			{
				return new ValueTuple<double, Vector3d?>(0.0, default(Vector3d?));
			}
			return new ValueTuple<double, Vector3d?>(tMin, new Vector3d?(this.Position + tMin * this.Direction));
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00059F98 File Offset: 0x00058198
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		private ValueTuple<Block, Vector3d, double>? IntersectBlock(IBlockAccessor blockAccessor, int x, int y, int z)
		{
			BlockPos position = new BlockPos(x, y, z, 0);
			Block block = blockAccessor.GetBlock(position, 4);
			Cuboidf[] collisionBoxes = block.GetCollisionBoxes(blockAccessor, position);
			if (collisionBoxes == null || collisionBoxes.Length == 0)
			{
				return default(ValueTuple<Block, Vector3d, double>?);
			}
			double closestIntersectionParameter = 1.0;
			Vector3d? closestIntersection = default(Vector3d?);
			foreach (Cuboidf collBox in collisionBoxes)
			{
				if (collBox != null)
				{
					ValueTuple<double, Vector3d?> valueTuple = this.IntersectBlockCollisionBox(collBox, x, y, z);
					double t = valueTuple.Item1;
					Vector3d? intersection = valueTuple.Item2;
					if (intersection != null)
					{
						closestIntersectionParameter = Math.Min(closestIntersectionParameter, t);
						closestIntersection = intersection;
					}
				}
			}
			if (closestIntersection != null)
			{
				return new ValueTuple<Block, Vector3d, double>?(new ValueTuple<Block, Vector3d, double>(block, closestIntersection.Value, closestIntersectionParameter));
			}
			return default(ValueTuple<Block, Vector3d, double>?);
		}

		// Token: 0x0400089B RID: 2203
		public readonly Vector3d Position;

		// Token: 0x0400089C RID: 2204
		public readonly Vector3d Direction;

		// Token: 0x0400089D RID: 2205
		private const float _epsilon = 1E-06f;
	}
}
