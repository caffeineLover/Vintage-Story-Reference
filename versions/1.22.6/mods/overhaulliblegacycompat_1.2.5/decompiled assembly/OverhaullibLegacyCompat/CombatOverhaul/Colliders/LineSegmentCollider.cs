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
	// Token: 0x02000167 RID: 359
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct LineSegmentCollider : IWeaponCollider, ICollider
	{
		// Token: 0x060011F7 RID: 4599 RVA: 0x00057C5D File Offset: 0x00055E5D
		public LineSegmentCollider(Vector3d position, Vector3d direction)
		{
			this.Position = position;
			this.Direction = direction;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00057C70 File Offset: 0x00055E70
		public LineSegmentCollider(JsonObject json)
		{
			this.Position = new Vector3d((double)json["X1"].AsFloat(0f), (double)json["Y1"].AsFloat(0f), (double)json["Z1"].AsFloat(0f));
			this.Direction = new Vector3d((double)json["X2"].AsFloat(0f), (double)json["Y2"].AsFloat(0f), (double)json["Z2"].AsFloat(0f));
			this.Direction -= this.Position;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00057D30 File Offset: 0x00055F30
		public LineSegmentCollider(params float[] positions)
		{
			this.Position = new Vector3d((double)positions[0], (double)positions[1], (double)positions[2]);
			this.Direction = new Vector3d((double)positions[3], (double)positions[4], (double)positions[5]);
			this.Direction -= this.Position;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00057D84 File Offset: 0x00055F84
		public void Render(ICoreClientAPI api, EntityAgent entityPlayer, int color = -1)
		{
			BlockPos playerPos = entityPlayer.Pos.AsBlockPos;
			Vector3d playerPosVector;
			playerPosVector..ctor((double)playerPos.X, (double)playerPos.Y, (double)playerPos.Z);
			Vector3d tail = this.Position - playerPosVector;
			Vector3d head = this.Position + this.Direction - playerPosVector;
			api.Render.RenderLine(playerPos, (float)tail.X, (float)tail.Y, (float)tail.Z, (float)head.X, (float)head.Y, (float)head.Z, color);
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00057E18 File Offset: 0x00056018
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

		// Token: 0x060011FC RID: 4604 RVA: 0x00057E48 File Offset: 0x00056048
		public ICollider Transform(Matrixf modelMatrix, EntityPos origin)
		{
			Vector3d tail = ColliderTools.TransformVector(this.Position, modelMatrix, origin);
			Vector3d head = ColliderTools.TransformVector(this.Direction + this.Position, modelMatrix, origin);
			return new LineSegmentCollider(tail, head - tail);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00057E90 File Offset: 0x00056090
		public LineSegmentCollider? TransformLineSegment(EntityPos origin, EntityAgent entity, ItemSlot itemSlot, ICoreClientAPI api, bool right = true)
		{
			Matrixf modelMatrix = ColliderTools.GetHeldItemModelMatrix(entity, itemSlot, api, right);
			if (modelMatrix == null)
			{
				return default(LineSegmentCollider?);
			}
			return new LineSegmentCollider?(LineSegmentCollider.TransformSegment(this, modelMatrix, origin));
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00057EC8 File Offset: 0x000560C8
		public bool RoughIntersect(Cuboidf collisionBox)
		{
			return ((double)collisionBox.MaxX >= this.Position.X || (double)collisionBox.MaxX >= this.Position.X + this.Direction.X) && ((double)collisionBox.MinX <= this.Position.X || (double)collisionBox.MinX <= this.Position.X + this.Direction.X) && ((double)collisionBox.MaxY >= this.Position.Y || (double)collisionBox.MaxY >= this.Position.Y + this.Direction.Y) && ((double)collisionBox.MinY <= this.Position.Y || (double)collisionBox.MinY <= this.Position.Y + this.Direction.Y) && ((double)collisionBox.MaxZ >= this.Position.Z || (double)collisionBox.MaxZ >= this.Position.Z + this.Direction.Z) && ((double)collisionBox.MinZ <= this.Position.Z || (double)collisionBox.MinZ <= this.Position.Z + this.Direction.Z);
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0005801C File Offset: 0x0005621C
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

		// Token: 0x06001200 RID: 4608 RVA: 0x00058090 File Offset: 0x00056290
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

		// Token: 0x06001201 RID: 4609 RVA: 0x00058186 File Offset: 0x00056386
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

		// Token: 0x06001202 RID: 4610 RVA: 0x000581AC File Offset: 0x000563AC
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

		// Token: 0x06001203 RID: 4611 RVA: 0x0005834C File Offset: 0x0005654C
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

		// Token: 0x06001204 RID: 4612 RVA: 0x0005839C File Offset: 0x0005659C
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

		// Token: 0x06001205 RID: 4613 RVA: 0x0005845C File Offset: 0x0005665C
		public static void ResetPreviousColliders(IEnumerable<IHasLineCollider> segments)
		{
			foreach (IHasLineCollider hasLineCollider in segments)
			{
				hasLineCollider.PreviousInWorldCollider = hasLineCollider.RelativeCollider;
			}
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x000584A8 File Offset: 0x000566A8
		private static LineSegmentCollider TransformSegment(LineSegmentCollider value, Matrixf modelMatrix, EntityPos playerPos)
		{
			Vector3d tail = ColliderTools.TransformVector(value.Position, modelMatrix, playerPos);
			Vector3d head = ColliderTools.TransformVector(value.Direction + value.Position, modelMatrix, playerPos);
			return new LineSegmentCollider(tail, head - tail);
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x000584EC File Offset: 0x000566EC
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

		// Token: 0x06001208 RID: 4616 RVA: 0x0005854C File Offset: 0x0005674C
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

		// Token: 0x06001209 RID: 4617 RVA: 0x00058678 File Offset: 0x00056878
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

		// Token: 0x04000875 RID: 2165
		public readonly Vector3d Position;

		// Token: 0x04000876 RID: 2166
		public readonly Vector3d Direction;

		// Token: 0x04000877 RID: 2167
		private const float _epsilon = 1E-06f;
	}
}
