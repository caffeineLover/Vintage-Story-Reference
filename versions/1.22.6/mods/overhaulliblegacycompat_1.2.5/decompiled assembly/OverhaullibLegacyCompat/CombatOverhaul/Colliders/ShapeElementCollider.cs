using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000162 RID: 354
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ShapeElementCollider
	{
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060011CB RID: 4555 RVA: 0x00056D01 File Offset: 0x00054F01
		public Vector4d[] ElementVertices { get; } = new Vector4d[8];

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x00056D09 File Offset: 0x00054F09
		public Vector4d[] InworldVertices { get; } = new Vector4d[8];

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060011CD RID: 4557 RVA: 0x00056D11 File Offset: 0x00054F11
		// (set) Token: 0x060011CE RID: 4558 RVA: 0x00056D19 File Offset: 0x00054F19
		public int JointId { get; set; }

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x00056D22 File Offset: 0x00054F22
		// (set) Token: 0x060011D0 RID: 4560 RVA: 0x00056D2A File Offset: 0x00054F2A
		[Nullable(2)]
		public EntityShapeRenderer Renderer { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x00056D33 File Offset: 0x00054F33
		// (set) Token: 0x060011D2 RID: 4562 RVA: 0x00056D3B File Offset: 0x00054F3B
		public bool HasRenderer { get; set; }

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00056D44 File Offset: 0x00054F44
		// (set) Token: 0x060011D4 RID: 4564 RVA: 0x00056D4C File Offset: 0x00054F4C
		public string ShapeElementName { get; set; } = "";

		// Token: 0x060011D5 RID: 4565 RVA: 0x00056D58 File Offset: 0x00054F58
		public ShapeElementCollider(ShapeElement element)
		{
			this.JointId = element.JointId;
			this.SetElementVertices(element);
			this.ShapeElementName = element.Name;
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00056DEC File Offset: 0x00054FEC
		public void Transform(float[] transformMatrixAll, ICoreClientAPI api)
		{
			if (this.Renderer == null)
			{
				return;
			}
			double[] transformMatrix = this.GetTransformMatrix(this.JointId, transformMatrixAll);
			EntityPos playerPos = api.World.Player.Entity.Pos;
			for (int vertex = 0; vertex < 8; vertex++)
			{
				this.InworldVertices[vertex] = ShapeElementCollider.MultiplyVectorByMatrix(transformMatrix, this.ElementVertices[vertex]);
				this.InworldVertices[vertex].W = 1.0;
				this.InworldVertices[vertex] = ShapeElementCollider.MultiplyVectorByMatrix(this.Renderer.ModelMat, this.InworldVertices[vertex]);
				Vector4d[] inworldVertices = this.InworldVertices;
				int num = vertex;
				inworldVertices[num].X = inworldVertices[num].X + playerPos.X;
				Vector4d[] inworldVertices2 = this.InworldVertices;
				int num2 = vertex;
				inworldVertices2[num2].Y = inworldVertices2[num2].Y + playerPos.Y;
				Vector4d[] inworldVertices3 = this.InworldVertices;
				int num3 = vertex;
				inworldVertices3[num3].Z = inworldVertices3[num3].Z + playerPos.Z;
			}
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00056EEC File Offset: 0x000550EC
		public bool Collide(Vector3d segmentStart, Vector3d segmentDirection, out double parameter, out Vector3d intersection)
		{
			this._faces[0] = new CuboidFace(this.InworldVertices[0], this.InworldVertices[1], this.InworldVertices[2], this.InworldVertices[3]);
			this._faces[1] = new CuboidFace(this.InworldVertices[4], this.InworldVertices[5], this.InworldVertices[6], this.InworldVertices[7]);
			this._faces[2] = new CuboidFace(this.InworldVertices[0], this.InworldVertices[1], this.InworldVertices[5], this.InworldVertices[4]);
			this._faces[3] = new CuboidFace(this.InworldVertices[2], this.InworldVertices[3], this.InworldVertices[7], this.InworldVertices[6]);
			this._faces[4] = new CuboidFace(this.InworldVertices[0], this.InworldVertices[3], this.InworldVertices[7], this.InworldVertices[4]);
			this._faces[5] = new CuboidFace(this.InworldVertices[1], this.InworldVertices[2], this.InworldVertices[6], this.InworldVertices[5]);
			double closestParameter = double.MaxValue;
			bool foundIntersection = false;
			intersection = Vector3d.Zero;
			foreach (CuboidFace face in this._faces)
			{
				double currentParameter;
				Vector3d faceIntersection;
				if (face.Collide(segmentStart, segmentDirection, out currentParameter, out faceIntersection) && currentParameter < closestParameter)
				{
					closestParameter = currentParameter;
					intersection = faceIntersection;
					foundIntersection = true;
				}
			}
			parameter = closestParameter;
			return foundIntersection;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x000570DC File Offset: 0x000552DC
		public bool Collide(Vector3d thisTickOrigin, Vector3d previousTickOrigin, double radius, out double distance, out Vector3d intersection)
		{
			this.FillVertexScratch();
			Vector3d segmentClosestPoint;
			intersection = this.ClosestPoint(thisTickOrigin, previousTickOrigin, this._vertexScratch, out segmentClosestPoint);
			distance = Vector3d.Distance(intersection, segmentClosestPoint);
			return distance <= radius;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0005711F File Offset: 0x0005531F
		public bool Collide(Vector3d thisTickOrigin, Vector3d previousTickOrigin, double radius, out double distance, out Vector3d intersection, out Vector3d segmentClosestPoint)
		{
			this.FillVertexScratch();
			intersection = this.ClosestPoint(thisTickOrigin, previousTickOrigin, this._vertexScratch, out segmentClosestPoint);
			distance = Vector3d.Distance(intersection, segmentClosestPoint);
			return distance <= radius;
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00057160 File Offset: 0x00055360
		private void FillVertexScratch()
		{
			for (int index = 0; index < 8; index++)
			{
				this._vertexScratch[index] = new Vector3d(this.InworldVertices[index].X, this.InworldVertices[index].Y, this.InworldVertices[index].Z);
			}
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x000571C0 File Offset: 0x000553C0
		private void SetElementVertices(ShapeElement element)
		{
			Vector4d from;
			from..ctor(element.From[0], element.From[1], element.From[2], 1.0);
			Vector4d to;
			to..ctor(element.To[0], element.To[1], element.To[2], 1.0);
			Vector4d diagonal = to - from;
			this.ElementVertices[0] = from;
			this.ElementVertices[6] = to;
			this.ElementVertices[1] = new Vector4d(from.X + diagonal.X, from.Y, from.Z, from.W);
			this.ElementVertices[3] = new Vector4d(from.X, from.Y + diagonal.Y, from.Z, from.W);
			this.ElementVertices[4] = new Vector4d(from.X, from.Y, from.Z + diagonal.Z, from.W);
			this.ElementVertices[2] = new Vector4d(from.X + diagonal.X, from.Y + diagonal.Y, from.Z, from.W);
			this.ElementVertices[7] = new Vector4d(from.X, from.Y + diagonal.Y, from.Z + diagonal.Z, from.W);
			this.ElementVertices[5] = new Vector4d(from.X + diagonal.X, from.Y, from.Z + diagonal.Z, from.W);
			double[] array = new double[16];
			Mat4d.Identity(array);
			Matrixd elementMatrix = new Matrixd(array);
			if (element.ParentElement != null)
			{
				ShapeElementCollider.GetElementTransformMatrix(elementMatrix, element.ParentElement);
			}
			double[] rotationOrigin = ShapeElementCollider.GetRotationOrigin(element);
			elementMatrix.Translate(rotationOrigin[0], rotationOrigin[1], rotationOrigin[2]).RotateX((double)((float)element.RotationX * 0.017453292f)).RotateY((double)((float)element.RotationY * 0.017453292f)).RotateZ((double)((float)element.RotationZ * 0.017453292f)).Translate(0.0 - rotationOrigin[0], 0.0 - rotationOrigin[1], 0.0 - rotationOrigin[2]);
			for (int vertex = 0; vertex < 8; vertex++)
			{
				this.ElementVertices[vertex] = this.ElementVertices[vertex] / 16.0;
				this.ElementVertices[vertex] = ShapeElementCollider.MultiplyVectorByMatrix(elementMatrix.Values, this.ElementVertices[vertex]);
				this.ElementVertices[vertex].W = 1.0;
			}
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00057498 File Offset: 0x00055698
		private static void GetElementTransformMatrix(Matrixd matrix, ShapeElement element)
		{
			if (element.ParentElement != null)
			{
				ShapeElementCollider.GetElementTransformMatrix(matrix, element.ParentElement);
			}
			double[] rotationOrigin = ShapeElementCollider.GetRotationOrigin(element);
			matrix.Translate(rotationOrigin[0], rotationOrigin[1], rotationOrigin[2]).RotateX((double)((float)element.RotationX * 0.017453292f)).RotateY((double)((float)element.RotationY * 0.017453292f)).RotateZ((double)((float)element.RotationZ * 0.017453292f)).Translate(0.0 - rotationOrigin[0], 0.0 - rotationOrigin[1], 0.0 - rotationOrigin[2]);
			if (ShapeElementCollider.HasVector3(element.From))
			{
				matrix.Translate(element.From[0], element.From[1], element.From[2]);
			}
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00057561 File Offset: 0x00055761
		private static double[] GetRotationOrigin(ShapeElement element)
		{
			if (!ShapeElementCollider.HasVector3(element.RotationOrigin))
			{
				return new double[3];
			}
			return element.RotationOrigin;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0005757D File Offset: 0x0005577D
		[NullableContext(2)]
		private static bool HasVector3(double[] vector)
		{
			return vector != null && vector.Length >= 3;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00057590 File Offset: 0x00055790
		private static int? GetIndex(int jointId, int matrixElementIndex)
		{
			int index = 16 * jointId;
			if (matrixElementIndex < 0)
			{
				return default(int?);
			}
			return new int?(index + matrixElementIndex);
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x000575BC File Offset: 0x000557BC
		private double[] GetTransformMatrix(int jointId, float[] TransformationMatrices4x4)
		{
			Mat4d.Identity(this._transformScratch);
			for (int elementIndex = 0; elementIndex < 16; elementIndex++)
			{
				int? transformMatricesIndex = ShapeElementCollider.GetIndex(jointId, elementIndex);
				if (transformMatricesIndex != null)
				{
					if (transformMatricesIndex.Value >= TransformationMatrices4x4.Length)
					{
						return this._transformScratch;
					}
					this._transformScratch[elementIndex] = (double)TransformationMatrices4x4[transformMatricesIndex.Value];
				}
			}
			return this._transformScratch;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00057620 File Offset: 0x00055820
		private static void GetElementTransformMatrixA(Matrixd matrix, ShapeElement element, double[] TransformationMatrices4x4)
		{
			if (element.ParentElement != null)
			{
				ShapeElementCollider.GetElementTransformMatrixA(matrix, element.ParentElement, TransformationMatrices4x4);
			}
			matrix.Translate(element.RotationOrigin[0], element.RotationOrigin[1], element.RotationOrigin[2]).RotateX(element.RotationX * 0.01745329238474369).RotateY(element.RotationY * 0.01745329238474369).RotateZ(element.RotationZ * 0.01745329238474369).Translate(0.0 - element.RotationOrigin[0], 0.0 - element.RotationOrigin[1], 0.0 - element.RotationOrigin[2]).Translate(element.From[0], element.From[1], element.From[2]);
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x000576F8 File Offset: 0x000558F8
		private static Vector4d MultiplyVectorByMatrix(double[] matrix, Vector4d vector)
		{
			Vector4d result;
			result..ctor(0.0, 0.0, 0.0, 0.0);
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					ref Vector4d ptr = ref result;
					int num = i;
					ptr[num] += matrix[4 * j + i] * vector[j];
				}
			}
			return result;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00057770 File Offset: 0x00055970
		private static Vector4d MultiplyVectorByMatrix(float[] matrix, Vector4d vector)
		{
			Vector4d result;
			result..ctor(0.0, 0.0, 0.0, 0.0);
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					ref Vector4d ptr = ref result;
					int num = i;
					ptr[num] += (double)matrix[4 * j + i] * vector[j];
				}
			}
			return result;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x000577EC File Offset: 0x000559EC
		private Vector3d ClosestPoint(Vector3d start, Vector3d end, Vector3d[] obbVertices, out Vector3d segmentClosestPoint)
		{
			Vector3d center = (obbVertices[0] + obbVertices[1] + obbVertices[2] + obbVertices[3] + obbVertices[4] + obbVertices[5] + obbVertices[6] + obbVertices[7]) / 8.0;
			this._axesScratch[0] = Vector3d.Normalize(obbVertices[1] - obbVertices[0]);
			this._axesScratch[1] = Vector3d.Normalize(obbVertices[3] - obbVertices[0]);
			this._axesScratch[2] = Vector3d.Normalize(obbVertices[4] - obbVertices[0]);
			this._halfSizeScratch[0] = Vector3d.Distance(obbVertices[0], obbVertices[1]) / 2.0;
			this._halfSizeScratch[1] = Vector3d.Distance(obbVertices[0], obbVertices[3]) / 2.0;
			this._halfSizeScratch[2] = Vector3d.Distance(obbVertices[0], obbVertices[4]) / 2.0;
			Vector3d closestPoint = center;
			segmentClosestPoint = ShapeElementCollider.ClosestPointOnSegment(start, end, center);
			Vector3d direction = segmentClosestPoint - center;
			for (int i = 0; i < 3; i++)
			{
				double distance = Vector3d.Dot(direction, this._axesScratch[i]);
				distance = Math.Clamp(distance, -this._halfSizeScratch[i], this._halfSizeScratch[i]);
				closestPoint += distance * this._axesScratch[i];
			}
			return closestPoint;
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x000579B4 File Offset: 0x00055BB4
		public static Vector3d ClosestPointOnSegment(Vector3d A, Vector3d B, Vector3d P)
		{
			Vector3d AB = B - A;
			Vector3d vector3d = P - A;
			double AB_dot_AB = Vector3d.Dot(AB, AB);
			double t = Vector3d.Dot(vector3d, AB) / AB_dot_AB;
			t = Math.Max(0.0, Math.Min(1.0, t));
			return A + t * AB;
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00057A0C File Offset: 0x00055C0C
		public void Render(ICoreClientAPI api, EntityAgent entityPlayer, int color = -1)
		{
			BlockPos playerPos = api.World.Player.Entity.Pos.AsBlockPos;
			Vec3f deltaPos = 0f - new Vec3f((float)playerPos.X, (float)playerPos.Y, (float)playerPos.Z);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[0], this.InworldVertices[1], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[0], this.InworldVertices[3], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[0], this.InworldVertices[4], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[1], this.InworldVertices[2], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[1], this.InworldVertices[5], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[2], this.InworldVertices[6], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[2], this.InworldVertices[3], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[3], this.InworldVertices[7], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[4], this.InworldVertices[7], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[4], this.InworldVertices[5], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[6], this.InworldVertices[7], playerPos, deltaPos, color);
			ShapeElementCollider.RenderLine(api, this.InworldVertices[6], this.InworldVertices[5], playerPos, deltaPos, color);
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00057BE8 File Offset: 0x00055DE8
		private static void RenderLine(ICoreClientAPI api, Vector4d start, Vector4d end, BlockPos playerPos, Vec3f deltaPos, int color)
		{
			api.Render.RenderLine(playerPos, (float)start.X + deltaPos.X, (float)start.Y + deltaPos.Y, (float)start.Z + deltaPos.Z, (float)end.X + deltaPos.X, (float)end.Y + deltaPos.Y, (float)end.Z + deltaPos.Z, color);
		}

		// Token: 0x04000869 RID: 2153
		public const int VertexCount = 8;

		// Token: 0x0400086C RID: 2156
		private readonly CuboidFace[] _faces = new CuboidFace[6];

		// Token: 0x0400086D RID: 2157
		private readonly Vector3d[] _vertexScratch = new Vector3d[8];

		// Token: 0x0400086E RID: 2158
		private readonly Vector3d[] _axesScratch = new Vector3d[3];

		// Token: 0x0400086F RID: 2159
		private readonly double[] _halfSizeScratch = new double[3];

		// Token: 0x04000870 RID: 2160
		private readonly double[] _transformScratch = new double[16];
	}
}
