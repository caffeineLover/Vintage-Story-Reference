using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace OpenTK.Mathematics
{
	// Token: 0x0200001C RID: 28
	[Serializable]
	public struct Matrix4 : IEquatable<Matrix4>, IFormattable
	{
		// Token: 0x0600063A RID: 1594 RVA: 0x000168CD File Offset: 0x00014ACD
		public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
			this.Row3 = row3;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x000168EC File Offset: 0x00014AEC
		public Matrix4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
		{
			this.Row0 = new Vector4(m00, m01, m02, m03);
			this.Row1 = new Vector4(m10, m11, m12, m13);
			this.Row2 = new Vector4(m20, m21, m22, m23);
			this.Row3 = new Vector4(m30, m31, m32, m33);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00016944 File Offset: 0x00014B44
		public Matrix4(Matrix3 topLeft)
		{
			this.Row0.X = topLeft.Row0.X;
			this.Row0.Y = topLeft.Row0.Y;
			this.Row0.Z = topLeft.Row0.Z;
			this.Row0.W = 0f;
			this.Row1.X = topLeft.Row1.X;
			this.Row1.Y = topLeft.Row1.Y;
			this.Row1.Z = topLeft.Row1.Z;
			this.Row1.W = 0f;
			this.Row2.X = topLeft.Row2.X;
			this.Row2.Y = topLeft.Row2.Y;
			this.Row2.Z = topLeft.Row2.Z;
			this.Row2.W = 0f;
			this.Row3.X = 0f;
			this.Row3.Y = 0f;
			this.Row3.Z = 0f;
			this.Row3.W = 1f;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00016A88 File Offset: 0x00014C88
		public readonly float Determinant
		{
			get
			{
				float x = this.Row0.X;
				float y = this.Row0.Y;
				float z = this.Row0.Z;
				float w = this.Row0.W;
				float x2 = this.Row1.X;
				float y2 = this.Row1.Y;
				float z2 = this.Row1.Z;
				float w2 = this.Row1.W;
				float x3 = this.Row2.X;
				float y3 = this.Row2.Y;
				float z3 = this.Row2.Z;
				float w3 = this.Row2.W;
				float x4 = this.Row3.X;
				float y4 = this.Row3.Y;
				float z4 = this.Row3.Z;
				float w4 = this.Row3.W;
				return x * y2 * z3 * w4 - x * y2 * w3 * z4 + x * z2 * w3 * y4 - x * z2 * y3 * w4 + x * w2 * y3 * z4 - x * w2 * z3 * y4 - y * z2 * w3 * x4 + y * z2 * x3 * w4 - y * w2 * x3 * z4 + y * w2 * z3 * x4 - y * x2 * z3 * w4 + y * x2 * w3 * z4 + z * w2 * x3 * y4 - z * w2 * y3 * x4 + z * x2 * y3 * w4 - z * x2 * w3 * y4 + z * y2 * w3 * x4 - z * y2 * x3 * w4 - w * x2 * y3 * z4 + w * x2 * z3 * y4 - w * y2 * z3 * x4 + w * y2 * x3 * z4 - w * z2 * x3 * y4 + w * z2 * y3 * x4;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x00016C68 File Offset: 0x00014E68
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x00016C9C File Offset: 0x00014E9C
		public Vector4 Column0
		{
			readonly get
			{
				return new Vector4(this.Row0.X, this.Row1.X, this.Row2.X, this.Row3.X);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.X = value.Y;
				this.Row2.X = value.Z;
				this.Row3.X = value.W;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x00016CED File Offset: 0x00014EED
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x00016D20 File Offset: 0x00014F20
		public Vector4 Column1
		{
			readonly get
			{
				return new Vector4(this.Row0.Y, this.Row1.Y, this.Row2.Y, this.Row3.Y);
			}
			set
			{
				this.Row0.Y = value.X;
				this.Row1.Y = value.Y;
				this.Row2.Y = value.Z;
				this.Row3.Y = value.W;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x00016D71 File Offset: 0x00014F71
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x00016DA4 File Offset: 0x00014FA4
		public Vector4 Column2
		{
			readonly get
			{
				return new Vector4(this.Row0.Z, this.Row1.Z, this.Row2.Z, this.Row3.Z);
			}
			set
			{
				this.Row0.Z = value.X;
				this.Row1.Z = value.Y;
				this.Row2.Z = value.Z;
				this.Row3.Z = value.W;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x00016DF5 File Offset: 0x00014FF5
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x00016E28 File Offset: 0x00015028
		public Vector4 Column3
		{
			readonly get
			{
				return new Vector4(this.Row0.W, this.Row1.W, this.Row2.W, this.Row3.W);
			}
			set
			{
				this.Row0.W = value.X;
				this.Row1.W = value.Y;
				this.Row2.W = value.Z;
				this.Row3.W = value.W;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00016E79 File Offset: 0x00015079
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x00016E86 File Offset: 0x00015086
		public float M11
		{
			readonly get
			{
				return this.Row0.X;
			}
			set
			{
				this.Row0.X = value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00016E94 File Offset: 0x00015094
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x00016EA1 File Offset: 0x000150A1
		public float M12
		{
			readonly get
			{
				return this.Row0.Y;
			}
			set
			{
				this.Row0.Y = value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00016EAF File Offset: 0x000150AF
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x00016EBC File Offset: 0x000150BC
		public float M13
		{
			readonly get
			{
				return this.Row0.Z;
			}
			set
			{
				this.Row0.Z = value;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00016ECA File Offset: 0x000150CA
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x00016ED7 File Offset: 0x000150D7
		public float M14
		{
			readonly get
			{
				return this.Row0.W;
			}
			set
			{
				this.Row0.W = value;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00016EE5 File Offset: 0x000150E5
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x00016EF2 File Offset: 0x000150F2
		public float M21
		{
			readonly get
			{
				return this.Row1.X;
			}
			set
			{
				this.Row1.X = value;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00016F00 File Offset: 0x00015100
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x00016F0D File Offset: 0x0001510D
		public float M22
		{
			readonly get
			{
				return this.Row1.Y;
			}
			set
			{
				this.Row1.Y = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00016F1B File Offset: 0x0001511B
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x00016F28 File Offset: 0x00015128
		public float M23
		{
			readonly get
			{
				return this.Row1.Z;
			}
			set
			{
				this.Row1.Z = value;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00016F36 File Offset: 0x00015136
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x00016F43 File Offset: 0x00015143
		public float M24
		{
			readonly get
			{
				return this.Row1.W;
			}
			set
			{
				this.Row1.W = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x00016F51 File Offset: 0x00015151
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x00016F5E File Offset: 0x0001515E
		public float M31
		{
			readonly get
			{
				return this.Row2.X;
			}
			set
			{
				this.Row2.X = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00016F6C File Offset: 0x0001516C
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x00016F79 File Offset: 0x00015179
		public float M32
		{
			readonly get
			{
				return this.Row2.Y;
			}
			set
			{
				this.Row2.Y = value;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00016F87 File Offset: 0x00015187
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x00016F94 File Offset: 0x00015194
		public float M33
		{
			readonly get
			{
				return this.Row2.Z;
			}
			set
			{
				this.Row2.Z = value;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00016FA2 File Offset: 0x000151A2
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x00016FAF File Offset: 0x000151AF
		public float M34
		{
			readonly get
			{
				return this.Row2.W;
			}
			set
			{
				this.Row2.W = value;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x00016FBD File Offset: 0x000151BD
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x00016FCA File Offset: 0x000151CA
		public float M41
		{
			readonly get
			{
				return this.Row3.X;
			}
			set
			{
				this.Row3.X = value;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00016FD8 File Offset: 0x000151D8
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x00016FE5 File Offset: 0x000151E5
		public float M42
		{
			readonly get
			{
				return this.Row3.Y;
			}
			set
			{
				this.Row3.Y = value;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00016FF3 File Offset: 0x000151F3
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x00017000 File Offset: 0x00015200
		public float M43
		{
			readonly get
			{
				return this.Row3.Z;
			}
			set
			{
				this.Row3.Z = value;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0001700E File Offset: 0x0001520E
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x0001701B File Offset: 0x0001521B
		public float M44
		{
			readonly get
			{
				return this.Row3.W;
			}
			set
			{
				this.Row3.W = value;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x00017029 File Offset: 0x00015229
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x0001705C File Offset: 0x0001525C
		public Vector4 Diagonal
		{
			readonly get
			{
				return new Vector4(this.Row0.X, this.Row1.Y, this.Row2.Z, this.Row3.W);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.Y = value.Y;
				this.Row2.Z = value.Z;
				this.Row3.W = value.W;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x000170AD File Offset: 0x000152AD
		public readonly float Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y + this.Row2.Z + this.Row3.W;
			}
		}

		// Token: 0x17000178 RID: 376
		public float this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 4 && columnIndex < 4)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0f;
			}
			set
			{
				if (rowIndex < 4 && columnIndex < 4)
				{
					this.GetRowUnsafe(this, rowIndex)[columnIndex] = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to set this matrix at: ({0}, {1})", rowIndex, columnIndex));
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001714C File Offset: 0x0001534C
		[MethodImpl(256)]
		private readonly ref Vector4 GetRowUnsafe(in Matrix4 m, int index)
		{
			return Unsafe.Add<Vector4>(Unsafe.AsRef<Vector4>(ref m.Row0), index);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001715F File Offset: 0x0001535F
		public void Invert()
		{
			this = Matrix4.Invert(this);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00017174 File Offset: 0x00015374
		public readonly Matrix4 Inverted()
		{
			Matrix4 result = this;
			if (result.Determinant != 0f)
			{
				result.Invert();
			}
			return result;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0001719E File Offset: 0x0001539E
		public void Transpose()
		{
			this = Matrix4.Transpose(this);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x000171B4 File Offset: 0x000153B4
		public readonly Matrix4 Transposed()
		{
			Matrix4 result = this;
			result.Transpose();
			return result;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000171D0 File Offset: 0x000153D0
		public void Normalize()
		{
			float determinant = this.Determinant;
			this.Row0 /= determinant;
			this.Row1 /= determinant;
			this.Row2 /= determinant;
			this.Row3 /= determinant;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001722C File Offset: 0x0001542C
		public readonly Matrix4 Normalized()
		{
			Matrix4 result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00017248 File Offset: 0x00015448
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			this = Matrix4.Swizzle(this, rowForRow0, rowForRow1, rowForRow2, rowForRow3);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00017260 File Offset: 0x00015460
		public readonly Matrix4 Swizzled(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4 result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2, rowForRow3);
			return result;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00017284 File Offset: 0x00015484
		public readonly Matrix4 ClearTranslation()
		{
			Matrix4 result = this;
			result.Row3.Xyz = Vector3.Zero;
			return result;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000172AC File Offset: 0x000154AC
		public readonly Matrix4 ClearScale()
		{
			Matrix4 result = this;
			result.Row0.Xyz = result.Row0.Xyz.Normalized();
			result.Row1.Xyz = result.Row1.Xyz.Normalized();
			result.Row2.Xyz = result.Row2.Xyz.Normalized();
			return result;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00017324 File Offset: 0x00015524
		public readonly Matrix4 ClearRotation()
		{
			Matrix4 result = this;
			result.Row0.Xyz = new Vector3(result.Row0.Xyz.Length, 0f, 0f);
			result.Row1.Xyz = new Vector3(0f, result.Row1.Xyz.Length, 0f);
			result.Row2.Xyz = new Vector3(0f, 0f, result.Row2.Xyz.Length);
			return result;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x000173C8 File Offset: 0x000155C8
		[Obsolete("This function doesn't actually clear the projection of the matrix. This is equivalent of setting Column3 = (0, 0, 0, 0).")]
		public readonly Matrix4 ClearProjection()
		{
			Matrix4 result = this;
			result.Column3 = Vector4.Zero;
			return result;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x000173EC File Offset: 0x000155EC
		public readonly Vector3 ExtractTranslation()
		{
			Vector4 row = this.Row3;
			return row.Xyz;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00017408 File Offset: 0x00015608
		public readonly Vector3 ExtractScale()
		{
			Vector4 vector = this.Row0;
			float length = vector.Xyz.Length;
			vector = this.Row1;
			float length2 = vector.Xyz.Length;
			vector = this.Row2;
			return new Vector3(length, length2, vector.Xyz.Length);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001745C File Offset: 0x0001565C
		public readonly Quaternion ExtractRotation(bool rowNormalize = true)
		{
			Vector4 vector = this.Row0;
			Vector3 vector2 = vector.Xyz;
			vector = this.Row1;
			Vector3 vector3 = vector.Xyz;
			vector = this.Row2;
			Vector3 vector4 = vector.Xyz;
			if (rowNormalize)
			{
				vector2 = vector2.Normalized();
				vector3 = vector3.Normalized();
				vector4 = vector4.Normalized();
			}
			Quaternion result = default(Quaternion);
			double num = 0.25 * ((double)(vector2[0] + vector3[1] + vector4[2]) + 1.0);
			if (num > 0.0)
			{
				double num2 = Math.Sqrt(num);
				result.W = (float)num2;
				num2 = 1.0 / (4.0 * num2);
				result.X = (float)((double)(vector3[2] - vector4[1]) * num2);
				result.Y = (float)((double)(vector4[0] - vector2[2]) * num2);
				result.Z = (float)((double)(vector2[1] - vector3[0]) * num2);
			}
			else if (vector2[0] > vector3[1] && vector2[0] > vector4[2])
			{
				double num3 = 2.0 * Math.Sqrt(1.0 + (double)vector2[0] - (double)vector3[1] - (double)vector4[2]);
				result.X = (float)(0.25 * num3);
				num3 = 1.0 / num3;
				result.W = (float)((double)(vector4[1] - vector3[2]) * num3);
				result.Y = (float)((double)(vector3[0] + vector2[1]) * num3);
				result.Z = (float)((double)(vector4[0] + vector2[2]) * num3);
			}
			else if (vector3[1] > vector4[2])
			{
				double num4 = 2.0 * Math.Sqrt(1.0 + (double)vector3[1] - (double)vector2[0] - (double)vector4[2]);
				result.Y = (float)(0.25 * num4);
				num4 = 1.0 / num4;
				result.W = (float)((double)(vector4[0] - vector2[2]) * num4);
				result.X = (float)((double)(vector3[0] + vector2[1]) * num4);
				result.Z = (float)((double)(vector4[1] + vector3[2]) * num4);
			}
			else
			{
				double num5 = 2.0 * Math.Sqrt(1.0 + (double)vector4[2] - (double)vector2[0] - (double)vector3[1]);
				result.Z = (float)(0.25 * num5);
				num5 = 1.0 / num5;
				result.W = (float)((double)(vector3[0] - vector2[1]) * num5);
				result.X = (float)((double)(vector4[0] + vector2[2]) * num5);
				result.Y = (float)((double)(vector4[1] + vector3[2]) * num5);
			}
			result.Normalize();
			return result;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000177E8 File Offset: 0x000159E8
		[Obsolete("Use Column3 if the old behaviour is needed, or use ExtractPerspective*() or ExtractOrthographic* instead.")]
		public readonly Vector4 ExtractProjection()
		{
			return this.Column3;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x000177F0 File Offset: 0x000159F0
		public readonly void ExtractPerspectiveOffCenter(out float left, out float right, out float bottom, out float top, out float depthNear, out float depthFar)
		{
			depthNear = this.Row3.Z / (this.Row2.Z - 1f);
			depthFar = this.Row3.Z / (this.Row2.Z + 1f);
			left = depthNear * (this.Row2.X - 1f) / this.Row0.X;
			right = depthNear * (this.Row2.X + 1f) / this.Row0.X;
			bottom = depthNear * (this.Row2.Y - 1f) / this.Row1.Y;
			top = depthNear * (this.Row2.Y + 1f) / this.Row1.Y;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x000178CC File Offset: 0x00015ACC
		public readonly void ExtractPerspectiveFieldOfView(out float fovy, out float aspect, out float depthNear, out float depthFar)
		{
			fovy = 2f * MathF.Atan(1f / this.Row1.Y);
			aspect = this.Row1.Y / this.Row0.X;
			depthNear = this.Row3.Z / (this.Row2.Z - 1f);
			depthFar = this.Row3.Z / (this.Row2.Z + 1f);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00017950 File Offset: 0x00015B50
		public readonly void ExtractOrthographicOffCenter(out float left, out float right, out float bottom, out float top, out float depthNear, out float depthFar)
		{
			left = -(1f + this.Row3.X) / this.Row0.X;
			right = (1f - this.Row3.X) / this.Row0.X;
			bottom = -(1f + this.Row3.Y) / this.Row1.Y;
			top = (1f - this.Row3.Y) / this.Row1.Y;
			depthNear = (1f + this.Row3.Z) / this.Row2.Z;
			depthFar = -(1f - this.Row3.Z) / this.Row2.Z;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00017A20 File Offset: 0x00015C20
		public readonly void ExtractOrthographic(out float width, out float height, out float depthNear, out float depthFar)
		{
			float num = -(1f + this.Row3.X) / this.Row0.X;
			float num2 = (1f - this.Row3.X) / this.Row0.X;
			width = num2 - num;
			float num3 = -(1f + this.Row3.Y) / this.Row1.Y;
			float num4 = (1f - this.Row3.Y) / this.Row1.Y;
			height = num4 - num3;
			depthNear = (1f + this.Row3.Z) / this.Row2.Z;
			depthFar = -(1f - this.Row3.Z) / this.Row2.Z;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00017AF4 File Offset: 0x00015CF4
		public static void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix4 result)
		{
			axis.Normalize();
			float x = axis.X;
			float y = axis.Y;
			float z = axis.Z;
			float num = MathF.Cos(-angle);
			float num2 = MathF.Sin(-angle);
			float num3 = 1f - num;
			float num4 = num3 * x * x;
			float num5 = num3 * x * y;
			float num6 = num3 * x * z;
			float num7 = num3 * y * y;
			float num8 = num3 * y * z;
			float num9 = num3 * z * z;
			float num10 = num2 * x;
			float num11 = num2 * y;
			float num12 = num2 * z;
			result.Row0.X = num4 + num;
			result.Row0.Y = num5 - num12;
			result.Row0.Z = num6 + num11;
			result.Row0.W = 0f;
			result.Row1.X = num5 + num12;
			result.Row1.Y = num7 + num;
			result.Row1.Z = num8 - num10;
			result.Row1.W = 0f;
			result.Row2.X = num6 - num11;
			result.Row2.Y = num8 + num10;
			result.Row2.Z = num9 + num;
			result.Row2.W = 0f;
			result.Row3 = Vector4.UnitW;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00017C34 File Offset: 0x00015E34
		public static Matrix4 CreateFromAxisAngle(Vector3 axis, float angle)
		{
			Matrix4 result;
			Matrix4.CreateFromAxisAngle(axis, angle, out result);
			return result;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00017C4C File Offset: 0x00015E4C
		public static void CreateFromQuaternion(in Quaternion q, out Matrix4 result)
		{
			float num = q.X * q.X;
			float num2 = q.Y * q.Y;
			float num3 = q.Z * q.Z;
			float num4 = q.W * q.W;
			float num5 = q.X * q.Y;
			float num6 = q.X * q.Z;
			float num7 = q.X * q.W;
			float num8 = q.Y * q.Z;
			float num9 = q.Y * q.W;
			float num10 = q.Z * q.W;
			float num11 = 2f / (num + num2 + num3 + num4);
			result.Row0.X = 1f - num11 * (num2 + num3);
			result.Row1.Y = 1f - num11 * (num + num3);
			result.Row2.Z = 1f - num11 * (num + num2);
			result.Row0.Y = num11 * (num5 + num10);
			result.Row1.X = num11 * (num5 - num10);
			result.Row2.X = num11 * (num6 + num9);
			result.Row0.Z = num11 * (num6 - num9);
			result.Row2.Y = num11 * (num8 - num7);
			result.Row1.Z = num11 * (num8 + num7);
			result.Row0.W = 0f;
			result.Row1.W = 0f;
			result.Row2.W = 0f;
			result.Row3 = new Vector4(0f, 0f, 0f, 1f);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00017E00 File Offset: 0x00016000
		public static Matrix4 CreateFromQuaternion(Quaternion q)
		{
			Matrix4 result;
			Matrix4.CreateFromQuaternion(q, out result);
			return result;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00017E18 File Offset: 0x00016018
		public static void CreateRotationX(float angle, out Matrix4 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result = Matrix4.Identity;
			result.Row1.Y = num;
			result.Row1.Z = num2;
			result.Row2.Y = -num2;
			result.Row2.Z = num;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00017E70 File Offset: 0x00016070
		public static Matrix4 CreateRotationX(float angle)
		{
			Matrix4 result;
			Matrix4.CreateRotationX(angle, out result);
			return result;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00017E88 File Offset: 0x00016088
		public static void CreateRotationY(float angle, out Matrix4 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result = Matrix4.Identity;
			result.Row0.X = num;
			result.Row0.Z = -num2;
			result.Row2.X = num2;
			result.Row2.Z = num;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00017EE0 File Offset: 0x000160E0
		public static Matrix4 CreateRotationY(float angle)
		{
			Matrix4 result;
			Matrix4.CreateRotationY(angle, out result);
			return result;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00017EF8 File Offset: 0x000160F8
		public static void CreateRotationZ(float angle, out Matrix4 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result = Matrix4.Identity;
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row1.X = -num2;
			result.Row1.Y = num;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00017F50 File Offset: 0x00016150
		public static Matrix4 CreateRotationZ(float angle)
		{
			Matrix4 result;
			Matrix4.CreateRotationZ(angle, out result);
			return result;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00017F66 File Offset: 0x00016166
		public static void CreateTranslation(float x, float y, float z, out Matrix4 result)
		{
			result = Matrix4.Identity;
			result.Row3.X = x;
			result.Row3.Y = y;
			result.Row3.Z = z;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00017F97 File Offset: 0x00016197
		public static void CreateTranslation(in Vector3 vector, out Matrix4 result)
		{
			result = Matrix4.Identity;
			result.Row3.X = vector.X;
			result.Row3.Y = vector.Y;
			result.Row3.Z = vector.Z;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00017FD8 File Offset: 0x000161D8
		public static Matrix4 CreateTranslation(float x, float y, float z)
		{
			Matrix4 result;
			Matrix4.CreateTranslation(x, y, z, out result);
			return result;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00017FF0 File Offset: 0x000161F0
		public static Matrix4 CreateTranslation(Vector3 vector)
		{
			Matrix4 result;
			Matrix4.CreateTranslation(vector.X, vector.Y, vector.Z, out result);
			return result;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00018018 File Offset: 0x00016218
		public static Matrix4 CreateScale(float scale)
		{
			Matrix4 result;
			Matrix4.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00018030 File Offset: 0x00016230
		public static Matrix4 CreateScale(Vector3 scale)
		{
			Matrix4 result;
			Matrix4.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00018048 File Offset: 0x00016248
		public static Matrix4 CreateScale(float x, float y, float z)
		{
			Matrix4 result;
			Matrix4.CreateScale(x, y, z, out result);
			return result;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00018060 File Offset: 0x00016260
		public static void CreateScale(float scale, out Matrix4 result)
		{
			result = Matrix4.Identity;
			result.Row0.X = scale;
			result.Row1.Y = scale;
			result.Row2.Z = scale;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00018091 File Offset: 0x00016291
		public static void CreateScale(in Vector3 scale, out Matrix4 result)
		{
			result = Matrix4.Identity;
			result.Row0.X = scale.X;
			result.Row1.Y = scale.Y;
			result.Row2.Z = scale.Z;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000180D1 File Offset: 0x000162D1
		public static void CreateScale(float x, float y, float z, out Matrix4 result)
		{
			result = Matrix4.Identity;
			result.Row0.X = x;
			result.Row1.Y = y;
			result.Row2.Z = z;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00018104 File Offset: 0x00016304
		public static Matrix4 CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4 result;
			Matrix4.CreateSwizzle(rowForRow0, rowForRow1, rowForRow2, rowForRow3, out result);
			return result;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00018120 File Offset: 0x00016320
		public static void CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3, out Matrix4 result)
		{
			if (rowForRow0 < 0 || rowForRow0 >= 4)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow0", rowForRow0));
			}
			if (rowForRow1 < 0 || rowForRow1 >= 4)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow1", rowForRow1));
			}
			if (rowForRow2 < 0 || rowForRow2 >= 4)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow2", rowForRow2));
			}
			if (rowForRow3 < 0 || rowForRow3 >= 4)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow3", rowForRow3));
			}
			result = Matrix4.Zero;
			result[0, rowForRow0] = 1f;
			result[1, rowForRow1] = 1f;
			result[2, rowForRow2] = 1f;
			result[3, rowForRow3] = 1f;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000181FD File Offset: 0x000163FD
		public static void CreateOrthographic(float width, float height, float depthNear, float depthFar, out Matrix4 result)
		{
			Matrix4.CreateOrthographicOffCenter(-width / 2f, width / 2f, -height / 2f, height / 2f, depthNear, depthFar, out result);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00018228 File Offset: 0x00016428
		public static Matrix4 CreateOrthographic(float width, float height, float depthNear, float depthFar)
		{
			Matrix4 result;
			Matrix4.CreateOrthographicOffCenter(-width / 2f, width / 2f, -height / 2f, height / 2f, depthNear, depthFar, out result);
			return result;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00018260 File Offset: 0x00016460
		public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float depthNear, float depthFar, out Matrix4 result)
		{
			result = Matrix4.Identity;
			float num = 1f / (right - left);
			float num2 = 1f / (top - bottom);
			float num3 = 1f / (depthFar - depthNear);
			result.Row0.X = 2f * num;
			result.Row1.Y = 2f * num2;
			result.Row2.Z = -2f * num3;
			result.Row3.X = -(right + left) * num;
			result.Row3.Y = -(top + bottom) * num2;
			result.Row3.Z = -(depthFar + depthNear) * num3;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001830C File Offset: 0x0001650C
		public static Matrix4 CreateOrthographicOffCenter(float left, float right, float bottom, float top, float depthNear, float depthFar)
		{
			Matrix4 result;
			Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar, out result);
			return result;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001832C File Offset: 0x0001652C
		public static void CreatePerspectiveFieldOfView(float fovy, float aspect, float depthNear, float depthFar, out Matrix4 result)
		{
			if (fovy <= 0f || fovy > 3.1415927f)
			{
				throw new ArgumentOutOfRangeException("fovy", fovy, "Fovy must be in the range [0, PI].");
			}
			if (aspect <= 0f)
			{
				throw new ArgumentOutOfRangeException("aspect", aspect, "Aspect cannot be negative.");
			}
			if (depthNear <= 0f)
			{
				throw new ArgumentOutOfRangeException("depthNear", depthNear, "depthNear cannot be negative.");
			}
			if (depthFar <= 0f)
			{
				throw new ArgumentOutOfRangeException("depthFar", depthFar, "depthFar cannot be negative.");
			}
			float num = depthNear * MathF.Tan(0.5f * fovy);
			float num2 = -num;
			float left = num2 * aspect;
			float right = num * aspect;
			Matrix4.CreatePerspectiveOffCenter(left, right, num2, num, depthNear, depthFar, out result);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x000183E0 File Offset: 0x000165E0
		public static Matrix4 CreatePerspectiveFieldOfView(float fovy, float aspect, float depthNear, float depthFar)
		{
			Matrix4 result;
			Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, depthNear, depthFar, out result);
			return result;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000183FC File Offset: 0x000165FC
		public static void CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float depthNear, float depthFar, out Matrix4 result)
		{
			if (depthNear <= 0f)
			{
				throw new ArgumentOutOfRangeException("depthNear");
			}
			if (depthFar <= 0f)
			{
				throw new ArgumentOutOfRangeException("depthFar");
			}
			if (depthNear >= depthFar)
			{
				throw new ArgumentOutOfRangeException("depthNear");
			}
			float x = 2f * depthNear / (right - left);
			float y = 2f * depthNear / (top - bottom);
			float x2 = (right + left) / (right - left);
			float y2 = (top + bottom) / (top - bottom);
			float z = -(depthFar + depthNear) / (depthFar - depthNear);
			float z2 = -(2f * depthFar * depthNear) / (depthFar - depthNear);
			result.Row0.X = x;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row0.W = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = y;
			result.Row1.Z = 0f;
			result.Row1.W = 0f;
			result.Row2.X = x2;
			result.Row2.Y = y2;
			result.Row2.Z = z;
			result.Row2.W = -1f;
			result.Row3.X = 0f;
			result.Row3.Y = 0f;
			result.Row3.Z = z2;
			result.Row3.W = 0f;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00018588 File Offset: 0x00016788
		public static Matrix4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float depthNear, float depthFar)
		{
			Matrix4 result;
			Matrix4.CreatePerspectiveOffCenter(left, right, bottom, top, depthNear, depthFar, out result);
			return result;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000185A8 File Offset: 0x000167A8
		public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
		{
			Vector3 vector = Vector3.Normalize(eye - target);
			Vector3 vector2 = Vector3.Normalize(Vector3.Cross(up, vector));
			Vector3 vector3 = Vector3.Normalize(Vector3.Cross(vector, vector2));
			Matrix4 result;
			result.Row0.X = vector2.X;
			result.Row0.Y = vector3.X;
			result.Row0.Z = vector.X;
			result.Row0.W = 0f;
			result.Row1.X = vector2.Y;
			result.Row1.Y = vector3.Y;
			result.Row1.Z = vector.Y;
			result.Row1.W = 0f;
			result.Row2.X = vector2.Z;
			result.Row2.Y = vector3.Z;
			result.Row2.Z = vector.Z;
			result.Row2.W = 0f;
			result.Row3.X = -(vector2.X * eye.X + vector2.Y * eye.Y + vector2.Z * eye.Z);
			result.Row3.Y = -(vector3.X * eye.X + vector3.Y * eye.Y + vector3.Z * eye.Z);
			result.Row3.Z = -(vector.X * eye.X + vector.Y * eye.Y + vector.Z * eye.Z);
			result.Row3.W = 1f;
			return result;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00018765 File Offset: 0x00016965
		public static Matrix4 LookAt(float eyeX, float eyeY, float eyeZ, float targetX, float targetY, float targetZ, float upX, float upY, float upZ)
		{
			return Matrix4.LookAt(new Vector3(eyeX, eyeY, eyeZ), new Vector3(targetX, targetY, targetZ), new Vector3(upX, upY, upZ));
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001878C File Offset: 0x0001698C
		public static Matrix4 Add(Matrix4 left, Matrix4 right)
		{
			Matrix4 result;
			Matrix4.Add(left, right, out result);
			return result;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x000187A8 File Offset: 0x000169A8
		public static void Add(in Matrix4 left, in Matrix4 right, out Matrix4 result)
		{
			result.Row0 = left.Row0 + right.Row0;
			result.Row1 = left.Row1 + right.Row1;
			result.Row2 = left.Row2 + right.Row2;
			result.Row3 = left.Row3 + right.Row3;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00018814 File Offset: 0x00016A14
		public static Matrix4 Subtract(Matrix4 left, Matrix4 right)
		{
			Matrix4 result;
			Matrix4.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00018830 File Offset: 0x00016A30
		public static void Subtract(in Matrix4 left, in Matrix4 right, out Matrix4 result)
		{
			result.Row0 = left.Row0 - right.Row0;
			result.Row1 = left.Row1 - right.Row1;
			result.Row2 = left.Row2 - right.Row2;
			result.Row3 = left.Row3 - right.Row3;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001889C File Offset: 0x00016A9C
		public static Matrix4 Mult(Matrix4 left, Matrix4 right)
		{
			Matrix4 result;
			Matrix4.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x000188B8 File Offset: 0x00016AB8
		public static void Mult(in Matrix4 left, in Matrix4 right, out Matrix4 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float z = left.Row0.Z;
			float w = left.Row0.W;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float z2 = left.Row1.Z;
			float w2 = left.Row1.W;
			float x3 = left.Row2.X;
			float y3 = left.Row2.Y;
			float z3 = left.Row2.Z;
			float w3 = left.Row2.W;
			float x4 = left.Row3.X;
			float y4 = left.Row3.Y;
			float z4 = left.Row3.Z;
			float w4 = left.Row3.W;
			float x5 = right.Row0.X;
			float y5 = right.Row0.Y;
			float z5 = right.Row0.Z;
			float w5 = right.Row0.W;
			float x6 = right.Row1.X;
			float y6 = right.Row1.Y;
			float z6 = right.Row1.Z;
			float w6 = right.Row1.W;
			float x7 = right.Row2.X;
			float y7 = right.Row2.Y;
			float z7 = right.Row2.Z;
			float w7 = right.Row2.W;
			float x8 = right.Row3.X;
			float y8 = right.Row3.Y;
			float z8 = right.Row3.Z;
			float w8 = right.Row3.W;
			result.Row0.X = x * x5 + y * x6 + z * x7 + w * x8;
			result.Row0.Y = x * y5 + y * y6 + z * y7 + w * y8;
			result.Row0.Z = x * z5 + y * z6 + z * z7 + w * z8;
			result.Row0.W = x * w5 + y * w6 + z * w7 + w * w8;
			result.Row1.X = x2 * x5 + y2 * x6 + z2 * x7 + w2 * x8;
			result.Row1.Y = x2 * y5 + y2 * y6 + z2 * y7 + w2 * y8;
			result.Row1.Z = x2 * z5 + y2 * z6 + z2 * z7 + w2 * z8;
			result.Row1.W = x2 * w5 + y2 * w6 + z2 * w7 + w2 * w8;
			result.Row2.X = x3 * x5 + y3 * x6 + z3 * x7 + w3 * x8;
			result.Row2.Y = x3 * y5 + y3 * y6 + z3 * y7 + w3 * y8;
			result.Row2.Z = x3 * z5 + y3 * z6 + z3 * z7 + w3 * z8;
			result.Row2.W = x3 * w5 + y3 * w6 + z3 * w7 + w3 * w8;
			result.Row3.X = x4 * x5 + y4 * x6 + z4 * x7 + w4 * x8;
			result.Row3.Y = x4 * y5 + y4 * y6 + z4 * y7 + w4 * y8;
			result.Row3.Z = x4 * z5 + y4 * z6 + z4 * z7 + w4 * z8;
			result.Row3.W = x4 * w5 + y4 * w6 + z4 * w7 + w4 * w8;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00018C74 File Offset: 0x00016E74
		public static Matrix4 Mult(Matrix4 left, float right)
		{
			Matrix4 result;
			Matrix4.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00018C8C File Offset: 0x00016E8C
		public static void Mult(in Matrix4 left, float right, out Matrix4 result)
		{
			result.Row0 = left.Row0 * right;
			result.Row1 = left.Row1 * right;
			result.Row2 = left.Row2 * right;
			result.Row3 = left.Row3 * right;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00018CE1 File Offset: 0x00016EE1
		public static void Invert(in Matrix4 mat, out Matrix4 result)
		{
			if (Sse3.IsSupported)
			{
				Matrix4.InvertSse3(mat, out result);
				return;
			}
			Matrix4.InvertFallback(mat, out result);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00018CFC File Offset: 0x00016EFC
		[MethodImpl(256)]
		private unsafe static void InvertSse3(in Matrix4 mat, out Matrix4 result)
		{
			Vector128<float> vector;
			Vector128<float> vector2;
			Vector128<float> vector3;
			Vector128<float> vector4;
			fixed (float* ptr = &mat.Row0.X)
			{
				float* ptr2 = ptr;
				vector = Sse.LoadVector128(ptr2);
				vector2 = Sse.LoadVector128(ptr2 + 4);
				vector3 = Sse.LoadVector128(ptr2 + 8);
				vector4 = Sse.LoadVector128(ptr2 + 12);
			}
			Vector128<float> vector5 = Sse.MoveLowToHigh(vector, vector2);
			Vector128<float> vector6 = Sse.MoveHighToLow(vector2, vector);
			Vector128<float> vector7 = Sse.MoveLowToHigh(vector3, vector4);
			Vector128<float> vector8 = Sse.MoveHighToLow(vector4, vector3);
			Vector128<float> vector9 = Sse.Subtract(Sse.Multiply(Sse.Shuffle(vector, vector3, 136), Sse.Shuffle(vector2, vector4, 221)), Sse.Multiply(Sse.Shuffle(vector, vector3, 221), Sse.Shuffle(vector2, vector4, 136)));
			Vector128<float> vector10 = Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector9), 0));
			Vector128<float> vector11 = Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector9), 85));
			Vector128<float> vector12 = Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector9), 170));
			Vector128<float> vector13 = Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector9), byte.MaxValue));
			Vector128<float> vector14 = Sse.Subtract(Sse.Multiply(Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector8), 15)), vector7), Sse.Multiply(Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector8), 165)), Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector7), 78))));
			Vector128<float> vector15 = Sse.Subtract(Sse.Multiply(Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector5), 15)), vector6), Sse.Multiply(Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector5), 165)), Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector6), 78))));
			Vector128<float> vector16 = Sse.Subtract(Sse.Multiply(vector13, vector5), Sse.Add(Sse.Multiply(vector6, Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector14), 204))), Sse.Multiply(Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector6), 177)), Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector14), 102)))));
			Vector128<float> vector17 = Sse.Subtract(Sse.Multiply(vector10, vector8), Sse.Add(Sse.Multiply(vector7, Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector15), 204))), Sse.Multiply(Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector7), 177)), Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector15), 102)))));
			Vector128<float> vector18 = Sse.Multiply(vector10, vector13);
			Vector128<float> vector19 = Sse.Subtract(Sse.Multiply(vector11, vector7), Sse.Subtract(Sse.Multiply(vector8, Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector15), 51))), Sse.Multiply(Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector8), 177)), Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector15), 102)))));
			Vector128<float> vector20 = Sse.Subtract(Sse.Multiply(vector12, vector6), Sse.Subtract(Sse.Multiply(vector5, Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector14), 51))), Sse.Multiply(Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector5), 177)), Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector14), 102)))));
			vector18 = Sse.Add(vector18, Sse.Multiply(vector11, vector12));
			Vector128<float> vector21 = Sse.Multiply(vector15, Vector128.AsSingle<int>(Sse2.Shuffle(Vector128.AsInt32<float>(vector14), 216)));
			vector21 = Sse3.HorizontalAdd(vector21, vector21);
			vector21 = Sse3.HorizontalAdd(vector21, vector21);
			vector18 = Sse.Subtract(vector18, vector21);
			if (MathF.Abs(Vector128.GetElement<float>(vector18, 0)) < 1E-45f)
			{
				throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
			}
			Vector128<float> vector22 = Sse.Divide(Vector128.Create(1f, -1f, -1f, 1f), vector18);
			vector16 = Sse.Multiply(vector16, vector22);
			vector19 = Sse.Multiply(vector19, vector22);
			vector20 = Sse.Multiply(vector20, vector22);
			vector17 = Sse.Multiply(vector17, vector22);
			Unsafe.SkipInit<Matrix4>(ref result);
			fixed (float* ptr = &result.Row0.X)
			{
				float* ptr3 = ptr;
				Sse.Store(ptr3, Sse.Shuffle(vector16, vector19, 119));
				Sse.Store(ptr3 + 4, Sse.Shuffle(vector16, vector19, 34));
				Sse.Store(ptr3 + 8, Sse.Shuffle(vector20, vector17, 119));
				Sse.Store(ptr3 + 12, Sse.Shuffle(vector20, vector17, 34));
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00019150 File Offset: 0x00017350
		[MethodImpl(256)]
		private static void InvertFallback(in Matrix4 mat, out Matrix4 result)
		{
			float m = mat.M11;
			float m2 = mat.M21;
			float m3 = mat.M31;
			float m4 = mat.M41;
			float m5 = mat.M12;
			float m6 = mat.M22;
			float m7 = mat.M32;
			float m8 = mat.M42;
			float m9 = mat.M13;
			float m10 = mat.M23;
			float m11 = mat.M33;
			float m12 = mat.M43;
			float m13 = mat.M14;
			float m14 = mat.M24;
			float m15 = mat.M34;
			float m16 = mat.M44;
			float num = m11 * m16 - m12 * m15;
			float num2 = m10 * m16 - m12 * m14;
			float num3 = m10 * m15 - m11 * m14;
			float num4 = m9 * m16 - m12 * m13;
			float num5 = m9 * m15 - m11 * m13;
			float num6 = m9 * m14 - m10 * m13;
			float num7 = m6 * num - m7 * num2 + m8 * num3;
			float num8 = -(m5 * num - m7 * num4 + m8 * num5);
			float num9 = m5 * num2 - m6 * num4 + m8 * num6;
			float num10 = -(m5 * num3 - m6 * num5 + m7 * num6);
			float num11 = m * num7 + m2 * num8 + m3 * num9 + m4 * num10;
			if (MathF.Abs(num11) < 1E-45f)
			{
				throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
			}
			float scale = 1f / num11;
			result.Row0 = new Vector4(num7, num8, num9, num10) * scale;
			result.Row1 = new Vector4(-(m2 * num - m3 * num2 + m4 * num3), m * num - m3 * num4 + m4 * num5, -(m * num2 - m2 * num4 + m4 * num6), m * num3 - m2 * num5 + m3 * num6) * scale;
			float num12 = m7 * m16 - m8 * m15;
			float num13 = m6 * m16 - m8 * m14;
			float num14 = m6 * m15 - m7 * m14;
			float num15 = m5 * m16 - m8 * m13;
			float num16 = m5 * m15 - m7 * m13;
			float num17 = m5 * m14 - m6 * m13;
			result.Row2 = new Vector4(m2 * num12 - m3 * num13 + m4 * num14, -(m * num12 - m3 * num15 + m4 * num16), m * num13 - m2 * num15 + m4 * num17, -(m * num14 - m2 * num16 + m3 * num17)) * scale;
			float num18 = m7 * m12 - m8 * m11;
			float num19 = m6 * m12 - m8 * m10;
			float num20 = m6 * m11 - m7 * m10;
			float num21 = m5 * m12 - m8 * m9;
			float num22 = m5 * m11 - m7 * m9;
			float num23 = m5 * m10 - m6 * m9;
			result.Row3 = new Vector4(-(m2 * num18 - m3 * num19 + m4 * num20), m * num18 - m3 * num21 + m4 * num22, -(m * num19 - m2 * num21 + m4 * num23), m * num20 - m2 * num22 + m3 * num23) * scale;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001943C File Offset: 0x0001763C
		public static Matrix4 Invert(Matrix4 mat)
		{
			Matrix4 result;
			Matrix4.Invert(mat, out result);
			return result;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00019453 File Offset: 0x00017653
		public static Matrix4 Transpose(Matrix4 mat)
		{
			return new Matrix4(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00019476 File Offset: 0x00017676
		public static void Transpose(in Matrix4 mat, out Matrix4 result)
		{
			result.Row0 = mat.Column0;
			result.Row1 = mat.Column1;
			result.Row2 = mat.Column2;
			result.Row3 = mat.Column3;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000194A8 File Offset: 0x000176A8
		public static Matrix4 Swizzle(Matrix4 mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4 result;
			Matrix4.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, rowForRow3, out result);
			return result;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000194C4 File Offset: 0x000176C4
		public static void Swizzle(in Matrix4 mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3, out Matrix4 result)
		{
			Vector4 vector;
			switch (rowForRow0)
			{
			case 0:
				vector = mat.Row0;
				break;
			case 1:
				vector = mat.Row1;
				break;
			case 2:
				vector = mat.Row2;
				break;
			case 3:
				vector = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow0", rowForRow0));
			}
			result.Row0 = vector;
			switch (rowForRow1)
			{
			case 0:
				vector = mat.Row0;
				break;
			case 1:
				vector = mat.Row1;
				break;
			case 2:
				vector = mat.Row2;
				break;
			case 3:
				vector = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow1", rowForRow1));
			}
			result.Row1 = vector;
			switch (rowForRow2)
			{
			case 0:
				vector = mat.Row0;
				break;
			case 1:
				vector = mat.Row1;
				break;
			case 2:
				vector = mat.Row2;
				break;
			case 3:
				vector = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow2", rowForRow2));
			}
			result.Row2 = vector;
			switch (rowForRow3)
			{
			case 0:
				vector = mat.Row0;
				break;
			case 1:
				vector = mat.Row1;
				break;
			case 2:
				vector = mat.Row2;
				break;
			case 3:
				vector = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow3", rowForRow3));
			}
			result.Row3 = vector;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001964F File Offset: 0x0001784F
		public static Matrix4 operator *(Matrix4 left, Matrix4 right)
		{
			return Matrix4.Mult(left, right);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00019658 File Offset: 0x00017858
		public static Matrix4 operator *(Matrix4 left, float right)
		{
			return Matrix4.Mult(left, right);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00019661 File Offset: 0x00017861
		public static Matrix4 operator +(Matrix4 left, Matrix4 right)
		{
			return Matrix4.Add(left, right);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001966A File Offset: 0x0001786A
		public static Matrix4 operator -(Matrix4 left, Matrix4 right)
		{
			return Matrix4.Subtract(left, right);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00019673 File Offset: 0x00017873
		public static bool operator ==(Matrix4 left, Matrix4 right)
		{
			return left.Equals(right);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001967D File Offset: 0x0001787D
		public static bool operator !=(Matrix4 left, Matrix4 right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001968A File Offset: 0x0001788A
		public unsafe static explicit operator Matrix4(Matrix4x4 mat)
		{
			return *Unsafe.As<Matrix4x4, Matrix4>(ref mat);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00019698 File Offset: 0x00017898
		public unsafe static explicit operator Matrix4x4(Matrix4 mat)
		{
			return *Unsafe.As<Matrix4, Matrix4x4>(ref mat);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x000196A6 File Offset: 0x000178A6
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000196B0 File Offset: 0x000178B0
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000196BA File Offset: 0x000178BA
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000196C4 File Offset: 0x000178C4
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			string text = this.Row0.ToString(format, formatProvider);
			string text2 = this.Row1.ToString(format, formatProvider);
			string text3 = this.Row2.ToString(format, formatProvider);
			string text4 = this.Row3.ToString(format, formatProvider);
			return string.Concat(new string[]
			{
				text,
				"\n",
				text2,
				"\n",
				text3,
				"\n",
				text4
			});
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001973C File Offset: 0x0001793C
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector4, Vector4, Vector4, Vector4>(this.Row0, this.Row1, this.Row2, this.Row3);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001975B File Offset: 0x0001795B
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix4 && this.Equals((Matrix4)obj);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00019774 File Offset: 0x00017974
		public readonly bool Equals(Matrix4 other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2 && this.Row3 == other.Row3;
		}

		// Token: 0x04000066 RID: 102
		public Vector4 Row0;

		// Token: 0x04000067 RID: 103
		public Vector4 Row1;

		// Token: 0x04000068 RID: 104
		public Vector4 Row2;

		// Token: 0x04000069 RID: 105
		public Vector4 Row3;

		// Token: 0x0400006A RID: 106
		public static readonly Matrix4 Identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

		// Token: 0x0400006B RID: 107
		public static readonly Matrix4 Zero = new Matrix4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);
	}
}
