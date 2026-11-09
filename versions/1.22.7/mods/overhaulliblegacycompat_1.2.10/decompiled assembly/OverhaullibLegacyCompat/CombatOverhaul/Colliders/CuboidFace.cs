using System;
using OpenTK.Mathematics;

namespace CombatOverhaul.Colliders
{
	// Token: 0x0200016B RID: 363
	public readonly struct CuboidFace
	{
		// Token: 0x0600122B RID: 4651 RVA: 0x000576DC File Offset: 0x000558DC
		public CuboidFace(Vector4d vertexA, Vector4d vertexB, Vector4d vertexC, Vector4d vertexD)
		{
			this.VertexA = new Vector3d(vertexA.X, vertexA.Y, vertexA.Z);
			this.VertexB = new Vector3d(vertexB.X, vertexB.Y, vertexB.Z);
			this.VertexC = new Vector3d(vertexC.X, vertexC.Y, vertexC.Z);
			this.VertexD = new Vector3d(vertexD.X, vertexD.Y, vertexD.Z);
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00057760 File Offset: 0x00055960
		private double IntersectPlaneWithLine(Vector3d start, Vector3d direction, Vector3d normal)
		{
			double startProjection = Vector3d.Dot(normal, start);
			double directionProjection = Vector3d.Dot(normal, start + direction);
			return (Vector3d.Dot(normal, this.VertexA) - startProjection) / (directionProjection - startProjection);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00057798 File Offset: 0x00055998
		public bool Collide(Vector3d segmentStart, Vector3d segmentDirection, out double parameter, out Vector3d intersection)
		{
			Vector3d normal = Vector3d.Cross(this.VertexB - this.VertexA, this.VertexC - this.VertexA);
			if (Math.Abs(Vector3d.Dot(normal, segmentDirection)) < 9.999999747378752E-05)
			{
				parameter = -1.0;
				intersection = Vector3d.Zero;
				return false;
			}
			parameter = this.IntersectPlaneWithLine(segmentStart, segmentDirection, normal);
			if (parameter < 0.0 || parameter > 1.0)
			{
				intersection = Vector3d.Zero;
				return false;
			}
			intersection = segmentStart + parameter * segmentDirection;
			Vector3d edge0 = this.VertexB - this.VertexA;
			Vector3d vp0 = intersection - this.VertexA;
			if (Vector3d.Dot(normal, Vector3d.Cross(edge0, vp0)) < 0.0)
			{
				return false;
			}
			Vector3d edge = this.VertexC - this.VertexB;
			Vector3d vp = intersection - this.VertexB;
			if (Vector3d.Dot(normal, Vector3d.Cross(edge, vp)) < 0.0)
			{
				return false;
			}
			Vector3d edge2 = this.VertexD - this.VertexC;
			Vector3d vp2 = intersection - this.VertexC;
			if (Vector3d.Dot(normal, Vector3d.Cross(edge2, vp2)) < 0.0)
			{
				return false;
			}
			Vector3d edge3 = this.VertexA - this.VertexD;
			Vector3d vp3 = intersection - this.VertexD;
			return Vector3d.Dot(normal, Vector3d.Cross(edge3, vp3)) >= 0.0;
		}

		// Token: 0x04000887 RID: 2183
		public readonly Vector3d VertexA;

		// Token: 0x04000888 RID: 2184
		public readonly Vector3d VertexB;

		// Token: 0x04000889 RID: 2185
		public readonly Vector3d VertexC;

		// Token: 0x0400088A RID: 2186
		public readonly Vector3d VertexD;
	}
}
