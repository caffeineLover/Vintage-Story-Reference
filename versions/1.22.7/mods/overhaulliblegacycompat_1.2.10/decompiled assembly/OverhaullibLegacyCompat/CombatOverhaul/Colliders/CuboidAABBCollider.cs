using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Colliders
{
	// Token: 0x0200016D RID: 365
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct CuboidAABBCollider
	{
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x0005795C File Offset: 0x00055B5C
		public Vector3d Center
		{
			get
			{
				return (this.VertexA + this.VertexB) / 2.0;
			}
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00057980 File Offset: 0x00055B80
		public CuboidAABBCollider(Vector3d vertexA, Vector3d vertexB)
		{
			this.VertexA = new Vector3d(Math.Min(vertexA.X, vertexB.X), Math.Min(vertexA.Y, vertexB.Y), Math.Min(vertexA.Z, vertexB.Z));
			this.VertexB = new Vector3d(Math.Max(vertexA.X, vertexB.X), Math.Max(vertexA.Y, vertexB.Y), Math.Max(vertexA.Z, vertexB.Z));
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00057A0C File Offset: 0x00055C0C
		public CuboidAABBCollider(Cuboidf cuboid)
		{
			this.VertexA = new Vector3d((double)Math.Min(cuboid.X1, cuboid.X2), (double)Math.Min(cuboid.Y1, cuboid.Y2), (double)Math.Min(cuboid.Z1, cuboid.Z2));
			this.VertexB = new Vector3d((double)Math.Max(cuboid.X1, cuboid.X2), (double)Math.Max(cuboid.Y1, cuboid.Y2), (double)Math.Max(cuboid.Z1, cuboid.Z2));
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00057A9C File Offset: 0x00055C9C
		public CuboidAABBCollider(Entity entity)
		{
			Cuboidf collisionBox = entity.CollisionBox.Clone();
			EntityPos position = entity.Pos;
			collisionBox.X1 += (float)position.X;
			collisionBox.Y1 += (float)position.Y;
			collisionBox.Z1 += (float)position.Z;
			collisionBox.X2 += (float)position.X;
			collisionBox.Y2 += (float)position.Y;
			collisionBox.Z2 += (float)position.Z;
			this.VertexA = new Vector3d((double)Math.Min(collisionBox.X1, collisionBox.X2), (double)Math.Min(collisionBox.Y1, collisionBox.Y2), (double)Math.Min(collisionBox.Z1, collisionBox.Z2));
			this.VertexB = new Vector3d((double)Math.Max(collisionBox.X1, collisionBox.X2), (double)Math.Max(collisionBox.Y1, collisionBox.Y2), (double)Math.Max(collisionBox.Z1, collisionBox.Z2));
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00057BB8 File Offset: 0x00055DB8
		public bool Collide(Vector3d segmentStart, Vector3d segmentDirection, out double parameter)
		{
			Vector3d min = Vector3d.ComponentMin(this.VertexA, this.VertexB);
			Vector3d max = Vector3d.ComponentMax(this.VertexA, this.VertexB);
			parameter = 0.0;
			double tmin = 0.0;
			double tmax = 1.0;
			if (!CuboidAABBCollider.UpdateSlab(segmentStart.X, segmentDirection.X, min.X, max.X, ref tmin, ref tmax))
			{
				return false;
			}
			if (!CuboidAABBCollider.UpdateSlab(segmentStart.Y, segmentDirection.Y, min.Y, max.Y, ref tmin, ref tmax))
			{
				return false;
			}
			if (!CuboidAABBCollider.UpdateSlab(segmentStart.Z, segmentDirection.Z, min.Z, max.Z, ref tmin, ref tmax))
			{
				return false;
			}
			parameter = tmin;
			return true;
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00057C7C File Offset: 0x00055E7C
		private static bool UpdateSlab(double start, double direction, double min, double max, ref double tmin, ref double tmax)
		{
			if (Math.Abs(direction) <= 5E-324)
			{
				return start >= min && start <= max;
			}
			double axisMin = (min - start) / direction;
			double axisMax = (max - start) / direction;
			if (axisMin > axisMax)
			{
				CuboidAABBCollider.Swap(ref axisMin, ref axisMax);
			}
			if (axisMin > tmin)
			{
				tmin = axisMin;
			}
			if (axisMax < tmax)
			{
				tmax = axisMax;
			}
			return tmin <= tmax;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00057CE0 File Offset: 0x00055EE0
		public bool Collide(Vector3d origin, double radius, out Vector3d intersection)
		{
			intersection = new Vector3d(Math.Clamp(origin.X, Math.Min(this.VertexA.X, this.VertexB.X), Math.Max(this.VertexA.X, this.VertexB.X)), Math.Clamp(origin.Y, Math.Min(this.VertexA.Y, this.VertexB.Y), Math.Max(this.VertexA.Y, this.VertexB.Y)), Math.Clamp(origin.Z, Math.Min(this.VertexA.Z, this.VertexB.Z), Math.Max(this.VertexA.Z, this.VertexB.Z)));
			return Vector3d.DistanceSquared(origin, intersection) <= radius * radius;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00057DD0 File Offset: 0x00055FD0
		public bool Collide(Vector3d thisTickOrigin, Vector3d previousTickOrigin, double radius, out Vector3d intersection)
		{
			Vector3d vector3d = thisTickOrigin - previousTickOrigin;
			if (vector3d.LengthSquared < radius / 2.0)
			{
				bool result = this.Collide(thisTickOrigin, radius, out vector3d);
				intersection = thisTickOrigin;
				return result;
			}
			Vector3d intersectionStart;
			Vector3d intersectionEnd;
			Vector3d closestPointOnSegment;
			Vector3d closestPointOnBox;
			if (CuboidAABBCollider.SegmentIntersectsAABB(previousTickOrigin, thisTickOrigin, this.VertexA, this.VertexB, out intersectionStart, out intersectionEnd, out closestPointOnSegment, out closestPointOnBox))
			{
				Vector3d vector3d2 = intersectionStart;
				vector3d = intersectionEnd - intersectionStart;
				intersection = vector3d2 - vector3d.Normalized() * radius;
				return true;
			}
			intersection = closestPointOnSegment;
			return Vector3d.DistanceSquared(intersection, closestPointOnBox) <= radius * radius;
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00057E6C File Offset: 0x0005606C
		[NullableContext(2)]
		public static bool CollideWithTerrain([Nullable(1)] IBlockAccessor blockAccessor, Vector3d thisTickOrigin, Vector3d previousTickOrigin, double radius, out Vector3d intersection, out Vector3d normal, out BlockFacing facing, out Block block, out BlockPos blockPosition)
		{
			int minX = (int)(Math.Min(thisTickOrigin.X, previousTickOrigin.X) - radius);
			int num = (int)(Math.Min(thisTickOrigin.Y, previousTickOrigin.Y) - radius);
			int minZ = (int)(Math.Min(thisTickOrigin.Z, previousTickOrigin.Z) - radius);
			int maxX = (int)(Math.Max(thisTickOrigin.X, previousTickOrigin.X) + radius);
			int maxY = (int)(Math.Max(thisTickOrigin.Y, previousTickOrigin.Y) + radius);
			int maxZ = (int)(Math.Max(thisTickOrigin.Z, previousTickOrigin.Z) + radius);
			blockPosition = null;
			BlockPos blockPos = new BlockPos(0);
			block = null;
			facing = null;
			intersection = default(Vector3d);
			normal = default(Vector3d);
			double shortestDistance = double.MaxValue;
			for (int y = num; y <= maxY; y++)
			{
				blockPos.SetAndCorrectDimension(minX, y, minZ);
				for (int x = minX; x <= maxX; x++)
				{
					blockPos.X = x;
					for (int z = minZ; z <= maxZ; z++)
					{
						blockPos.Z = z;
						Block blockBuffer = blockAccessor.GetBlock(blockPos, 4);
						Cuboidf[] collisionBoxes = blockBuffer.GetCollisionBoxes(blockAccessor, blockPos);
						if (collisionBoxes != null && collisionBoxes.Length != 0)
						{
							foreach (Cuboidf collisionBox in collisionBoxes)
							{
								if (collisionBox != null)
								{
									collisionBox = collisionBox.OffsetCopy(blockPos);
									CuboidAABBCollider collider = new CuboidAABBCollider(collisionBox);
									Vector3d currentIntersection;
									if (collider.Collide(thisTickOrigin, previousTickOrigin, radius, out currentIntersection))
									{
										double currentDistance = (currentIntersection - previousTickOrigin).Length;
										if (currentDistance < shortestDistance)
										{
											intersection = currentIntersection;
											shortestDistance = currentDistance;
											block = blockBuffer;
											blockPosition = blockPos.Copy();
											facing = collider.GetFacing(intersection - collider.Center, out normal);
										}
									}
								}
							}
						}
					}
				}
			}
			return block != null;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0005804C File Offset: 0x0005624C
		public BlockFacing GetFacing(Vector3d direction, out Vector3d normal)
		{
			normal = CuboidAABBCollider.GetIntersectingFaceNormal(this.VertexA, this.VertexB, direction);
			return BlockFacing.FromNormal(new Vec3f((float)normal.X, (float)normal.Y, (float)normal.Z));
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00058088 File Offset: 0x00056288
		private static Vector3d GetIntersectingFaceNormal(Vector3d min, Vector3d max, Vector3d dir)
		{
			dir.Normalize();
			(min + max) * 0.5;
			Vector3d halfExtents = (max - min) * 0.5;
			double tx = (dir.X != 0.0) ? ((double)Math.Sign(dir.X) * halfExtents.X / dir.X) : double.MaxValue;
			double ty = (dir.Y != 0.0) ? ((double)Math.Sign(dir.Y) * halfExtents.Y / dir.Y) : double.MaxValue;
			double tz = (dir.Z != 0.0) ? ((double)Math.Sign(dir.Z) * halfExtents.Z / dir.Z) : double.MaxValue;
			double t = double.MaxValue;
			Vector3d normal = Vector3d.Zero;
			if (tx > 0.0 && tx < t)
			{
				t = tx;
				normal..ctor((double)Math.Sign(dir.X), 0.0, 0.0);
			}
			if (ty > 0.0 && ty < t)
			{
				t = ty;
				normal..ctor(0.0, (double)Math.Sign(dir.Y), 0.0);
			}
			if (tz > 0.0 && tz < t)
			{
				normal..ctor(0.0, 0.0, (double)Math.Sign(dir.Z));
			}
			return normal;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0005822C File Offset: 0x0005642C
		private static bool SegmentIntersectsAABB(Vector3d p1, Vector3d p2, Vector3d boxMin, Vector3d boxMax, out Vector3d intersectionStart, out Vector3d intersectionEnd, out Vector3d closestPointOnSegment, out Vector3d closestPointOnBox)
		{
			CuboidAABBCollider.<>c__DisplayClass14_0 CS$<>8__locals1;
			CS$<>8__locals1.p1 = p1;
			CS$<>8__locals1.p2 = p2;
			CS$<>8__locals1.boxMin = boxMin;
			CS$<>8__locals1.boxMax = boxMax;
			intersectionStart = Vector3d.Zero;
			intersectionEnd = Vector3d.Zero;
			closestPointOnSegment = Vector3d.Zero;
			closestPointOnBox = Vector3d.Zero;
			Vector3d dir = CS$<>8__locals1.p2 - CS$<>8__locals1.p1;
			Vector3d invDir;
			invDir..ctor(1.0 / ((dir.X != 0.0) ? dir.X : double.Epsilon), 1.0 / ((dir.Y != 0.0) ? dir.Y : double.Epsilon), 1.0 / ((dir.Z != 0.0) ? dir.Z : double.Epsilon));
			double tmin = (CS$<>8__locals1.boxMin.X - CS$<>8__locals1.p1.X) * invDir.X;
			double tmax = (CS$<>8__locals1.boxMax.X - CS$<>8__locals1.p1.X) * invDir.X;
			if (tmin > tmax)
			{
				CuboidAABBCollider.Swap(ref tmin, ref tmax);
			}
			double tymin = (CS$<>8__locals1.boxMin.Y - CS$<>8__locals1.p1.Y) * invDir.Y;
			double tymax = (CS$<>8__locals1.boxMax.Y - CS$<>8__locals1.p1.Y) * invDir.Y;
			if (tymin > tymax)
			{
				CuboidAABBCollider.Swap(ref tymin, ref tymax);
			}
			if (tmin > tymax || tymin > tmax)
			{
				return CuboidAABBCollider.<SegmentIntersectsAABB>g__ComputeClosestPoints|14_0(out closestPointOnSegment, out closestPointOnBox, ref CS$<>8__locals1);
			}
			if (tymin > tmin)
			{
				tmin = tymin;
			}
			if (tymax < tmax)
			{
				tmax = tymax;
			}
			double tzmin = (CS$<>8__locals1.boxMin.Z - CS$<>8__locals1.p1.Z) * invDir.Z;
			double tzmax = (CS$<>8__locals1.boxMax.Z - CS$<>8__locals1.p1.Z) * invDir.Z;
			if (tzmin > tzmax)
			{
				CuboidAABBCollider.Swap(ref tzmin, ref tzmax);
			}
			if (tmin > tzmax || tzmin > tmax)
			{
				return CuboidAABBCollider.<SegmentIntersectsAABB>g__ComputeClosestPoints|14_0(out closestPointOnSegment, out closestPointOnBox, ref CS$<>8__locals1);
			}
			if (tzmin > tmin)
			{
				tmin = tzmin;
			}
			if (tzmax < tmax)
			{
				tmax = tzmax;
			}
			if (tmax < 0.0 || tmin > 1.0)
			{
				return CuboidAABBCollider.<SegmentIntersectsAABB>g__ComputeClosestPoints|14_0(out closestPointOnSegment, out closestPointOnBox, ref CS$<>8__locals1);
			}
			double tEnter = Math.Max(0.0, tmin);
			double tExit = Math.Min(1.0, tmax);
			intersectionStart = CS$<>8__locals1.p1 + dir * tEnter;
			intersectionEnd = CS$<>8__locals1.p1 + dir * tExit;
			CuboidAABBCollider.<SegmentIntersectsAABB>g__ComputeClosestPoints|14_0(out closestPointOnSegment, out closestPointOnBox, ref CS$<>8__locals1);
			return true;
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x000584EC File Offset: 0x000566EC
		private static Vector3d ClosestPointOnSegmentToPoint(Vector3d a, Vector3d b, Vector3d point)
		{
			Vector3d ab = b - a;
			double t = Vector3d.Dot(point - a, ab) / ab.LengthSquared;
			t = Math.Clamp(t, 0.0, 1.0);
			return a + t * ab;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00058540 File Offset: 0x00056740
		private static Vector3d ClampPointToAABB(Vector3d point, Vector3d min, Vector3d max)
		{
			return new Vector3d(Math.Clamp(point.X, min.X, max.X), Math.Clamp(point.Y, min.Y, max.Y), Math.Clamp(point.Z, min.Z, max.Z));
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00058598 File Offset: 0x00056798
		private static void Swap(ref double a, ref double b)
		{
			double tmp = a;
			a = b;
			b = tmp;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x000585B0 File Offset: 0x000567B0
		[CompilerGenerated]
		internal static bool <SegmentIntersectsAABB>g__ComputeClosestPoints|14_0(out Vector3d closestPointOnSegment, out Vector3d closestPointOnBox, ref CuboidAABBCollider.<>c__DisplayClass14_0 A_2)
		{
			Vector3d segClosest = CuboidAABBCollider.ClosestPointOnSegmentToPoint(A_2.p1, A_2.p2, CuboidAABBCollider.ClampPointToAABB((A_2.p1 + A_2.p2) * 0.5, A_2.boxMin, A_2.boxMax));
			Vector3d boxClosest = CuboidAABBCollider.ClampPointToAABB(segClosest, A_2.boxMin, A_2.boxMax);
			closestPointOnSegment = segClosest;
			closestPointOnBox = boxClosest;
			return false;
		}

		// Token: 0x0400088D RID: 2189
		public readonly Vector3d VertexA;

		// Token: 0x0400088E RID: 2190
		public readonly Vector3d VertexB;
	}
}
