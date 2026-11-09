using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x0200001D RID: 29
	[Serializable]
	public struct Matrix4d : IEquatable<Matrix4d>, IFormattable
	{
		// Token: 0x060006C0 RID: 1728 RVA: 0x0001980B File Offset: 0x00017A0B
		public Matrix4d(Vector4d row0, Vector4d row1, Vector4d row2, Vector4d row3)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
			this.Row3 = row3;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001982C File Offset: 0x00017A2C
		public Matrix4d(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33)
		{
			this.Row0 = new Vector4d(m00, m01, m02, m03);
			this.Row1 = new Vector4d(m10, m11, m12, m13);
			this.Row2 = new Vector4d(m20, m21, m22, m23);
			this.Row3 = new Vector4d(m30, m31, m32, m33);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00019884 File Offset: 0x00017A84
		public Matrix4d(Matrix3d topLeft)
		{
			this.Row0.X = topLeft.Row0.X;
			this.Row0.Y = topLeft.Row0.Y;
			this.Row0.Z = topLeft.Row0.Z;
			this.Row0.W = 0.0;
			this.Row1.X = topLeft.Row1.X;
			this.Row1.Y = topLeft.Row1.Y;
			this.Row1.Z = topLeft.Row1.Z;
			this.Row1.W = 0.0;
			this.Row2.X = topLeft.Row2.X;
			this.Row2.Y = topLeft.Row2.Y;
			this.Row2.Z = topLeft.Row2.Z;
			this.Row2.W = 0.0;
			this.Row3.X = 0.0;
			this.Row3.Y = 0.0;
			this.Row3.Z = 0.0;
			this.Row3.W = 1.0;
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x000199E4 File Offset: 0x00017BE4
		public readonly double Determinant
		{
			get
			{
				return this.Row0.X * this.Row1.Y * this.Row2.Z * this.Row3.W - this.Row0.X * this.Row1.Y * this.Row2.W * this.Row3.Z + this.Row0.X * this.Row1.Z * this.Row2.W * this.Row3.Y - this.Row0.X * this.Row1.Z * this.Row2.Y * this.Row3.W + this.Row0.X * this.Row1.W * this.Row2.Y * this.Row3.Z - this.Row0.X * this.Row1.W * this.Row2.Z * this.Row3.Y - this.Row0.Y * this.Row1.Z * this.Row2.W * this.Row3.X + this.Row0.Y * this.Row1.Z * this.Row2.X * this.Row3.W - this.Row0.Y * this.Row1.W * this.Row2.X * this.Row3.Z + this.Row0.Y * this.Row1.W * this.Row2.Z * this.Row3.X - this.Row0.Y * this.Row1.X * this.Row2.Z * this.Row3.W + this.Row0.Y * this.Row1.X * this.Row2.W * this.Row3.Z + this.Row0.Z * this.Row1.W * this.Row2.X * this.Row3.Y - this.Row0.Z * this.Row1.W * this.Row2.Y * this.Row3.X + this.Row0.Z * this.Row1.X * this.Row2.Y * this.Row3.W - this.Row0.Z * this.Row1.X * this.Row2.W * this.Row3.Y + this.Row0.Z * this.Row1.Y * this.Row2.W * this.Row3.X - this.Row0.Z * this.Row1.Y * this.Row2.X * this.Row3.W - this.Row0.W * this.Row1.X * this.Row2.Y * this.Row3.Z + this.Row0.W * this.Row1.X * this.Row2.Z * this.Row3.Y - this.Row0.W * this.Row1.Y * this.Row2.Z * this.Row3.X + this.Row0.W * this.Row1.Y * this.Row2.X * this.Row3.Z - this.Row0.W * this.Row1.Z * this.Row2.X * this.Row3.Y + this.Row0.W * this.Row1.Z * this.Row2.Y * this.Row3.X;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00019E70 File Offset: 0x00018070
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x00019EA4 File Offset: 0x000180A4
		public Vector4d Column0
		{
			readonly get
			{
				return new Vector4d(this.Row0.X, this.Row1.X, this.Row2.X, this.Row3.X);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.X = value.Y;
				this.Row2.X = value.Z;
				this.Row3.X = value.W;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00019EF5 File Offset: 0x000180F5
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x00019F28 File Offset: 0x00018128
		public Vector4d Column1
		{
			readonly get
			{
				return new Vector4d(this.Row0.Y, this.Row1.Y, this.Row2.Y, this.Row3.Y);
			}
			set
			{
				this.Row0.Y = value.X;
				this.Row1.Y = value.Y;
				this.Row2.Y = value.Z;
				this.Row3.Y = value.W;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00019F79 File Offset: 0x00018179
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x00019FAC File Offset: 0x000181AC
		public Vector4d Column2
		{
			readonly get
			{
				return new Vector4d(this.Row0.Z, this.Row1.Z, this.Row2.Z, this.Row3.Z);
			}
			set
			{
				this.Row0.Z = value.X;
				this.Row1.Z = value.Y;
				this.Row2.Z = value.Z;
				this.Row3.Z = value.W;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00019FFD File Offset: 0x000181FD
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x0001A030 File Offset: 0x00018230
		public Vector4d Column3
		{
			readonly get
			{
				return new Vector4d(this.Row0.W, this.Row1.W, this.Row2.W, this.Row3.W);
			}
			set
			{
				this.Row0.W = value.X;
				this.Row1.W = value.Y;
				this.Row2.W = value.Z;
				this.Row3.W = value.W;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0001A081 File Offset: 0x00018281
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x0001A08E File Offset: 0x0001828E
		public double M11
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

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x0001A09C File Offset: 0x0001829C
		// (set) Token: 0x060006CF RID: 1743 RVA: 0x0001A0A9 File Offset: 0x000182A9
		public double M12
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

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0001A0B7 File Offset: 0x000182B7
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x0001A0C4 File Offset: 0x000182C4
		public double M13
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

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0001A0D2 File Offset: 0x000182D2
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x0001A0DF File Offset: 0x000182DF
		public double M14
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

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0001A0ED File Offset: 0x000182ED
		// (set) Token: 0x060006D5 RID: 1749 RVA: 0x0001A0FA File Offset: 0x000182FA
		public double M21
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

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0001A108 File Offset: 0x00018308
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x0001A115 File Offset: 0x00018315
		public double M22
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

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0001A123 File Offset: 0x00018323
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x0001A130 File Offset: 0x00018330
		public double M23
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

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0001A13E File Offset: 0x0001833E
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x0001A14B File Offset: 0x0001834B
		public double M24
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

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001A159 File Offset: 0x00018359
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x0001A166 File Offset: 0x00018366
		public double M31
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

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0001A174 File Offset: 0x00018374
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x0001A181 File Offset: 0x00018381
		public double M32
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

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0001A18F File Offset: 0x0001838F
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x0001A19C File Offset: 0x0001839C
		public double M33
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

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0001A1AA File Offset: 0x000183AA
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0001A1B7 File Offset: 0x000183B7
		public double M34
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

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001A1C5 File Offset: 0x000183C5
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0001A1D2 File Offset: 0x000183D2
		public double M41
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

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001A1E0 File Offset: 0x000183E0
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x0001A1ED File Offset: 0x000183ED
		public double M42
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

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0001A1FB File Offset: 0x000183FB
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x0001A208 File Offset: 0x00018408
		public double M43
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

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0001A216 File Offset: 0x00018416
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x0001A223 File Offset: 0x00018423
		public double M44
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

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0001A231 File Offset: 0x00018431
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0001A264 File Offset: 0x00018464
		public Vector4d Diagonal
		{
			readonly get
			{
				return new Vector4d(this.Row0.X, this.Row1.Y, this.Row2.Z, this.Row3.W);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.Y = value.Y;
				this.Row2.Z = value.Z;
				this.Row3.W = value.W;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0001A2B5 File Offset: 0x000184B5
		public readonly double Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y + this.Row2.Z + this.Row3.W;
			}
		}

		// Token: 0x17000190 RID: 400
		public double this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 4 && columnIndex < 4)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0.0;
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

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001A358 File Offset: 0x00018558
		[MethodImpl(256)]
		private readonly ref Vector4d GetRowUnsafe(in Matrix4d m, int index)
		{
			return Unsafe.Add<Vector4d>(Unsafe.AsRef<Vector4d>(ref m.Row0), index);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001A36B File Offset: 0x0001856B
		public void Invert()
		{
			this = Matrix4d.Invert(this);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001A37C File Offset: 0x0001857C
		public readonly Matrix4d Inverted()
		{
			Matrix4d result = this;
			if (result.Determinant != 0.0)
			{
				result.Invert();
			}
			return result;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001A3AA File Offset: 0x000185AA
		public void Transpose()
		{
			this = Matrix4d.Transpose(this);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001A3C0 File Offset: 0x000185C0
		public readonly Matrix4d Transposed()
		{
			Matrix4d result = this;
			result.Transpose();
			return result;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001A3DC File Offset: 0x000185DC
		public void Normalize()
		{
			double determinant = this.Determinant;
			this.Row0 /= determinant;
			this.Row1 /= determinant;
			this.Row2 /= determinant;
			this.Row3 /= determinant;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001A438 File Offset: 0x00018638
		public readonly Matrix4d Normalized()
		{
			Matrix4d result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001A454 File Offset: 0x00018654
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			this = Matrix4d.Swizzle(this, rowForRow0, rowForRow1, rowForRow2, rowForRow3);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001A46C File Offset: 0x0001866C
		public readonly Matrix4d Swizzled(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4d result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2, rowForRow3);
			return result;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001A490 File Offset: 0x00018690
		public readonly Matrix4d ClearTranslation()
		{
			Matrix4d result = this;
			result.Row3.Xyz = Vector3d.Zero;
			return result;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001A4B8 File Offset: 0x000186B8
		public readonly Matrix4d ClearScale()
		{
			Matrix4d result = this;
			result.Row0.Xyz = result.Row0.Xyz.Normalized();
			result.Row1.Xyz = result.Row1.Xyz.Normalized();
			result.Row2.Xyz = result.Row2.Xyz.Normalized();
			return result;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001A530 File Offset: 0x00018730
		public readonly Matrix4d ClearRotation()
		{
			Matrix4d result = this;
			result.Row0.Xyz = new Vector3d(result.Row0.Xyz.Length, 0.0, 0.0);
			result.Row1.Xyz = new Vector3d(0.0, result.Row1.Xyz.Length, 0.0);
			result.Row2.Xyz = new Vector3d(0.0, 0.0, result.Row2.Xyz.Length);
			return result;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001A5EC File Offset: 0x000187EC
		[Obsolete("This function doesn't actually clear the projection of the matrix. This is equivalent of setting Column3 = (0, 0, 0, 0).")]
		public readonly Matrix4d ClearProjection()
		{
			Matrix4d result = this;
			result.Column3 = Vector4d.Zero;
			return result;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001A610 File Offset: 0x00018810
		public readonly Vector3d ExtractTranslation()
		{
			Vector4d row = this.Row3;
			return row.Xyz;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001A62C File Offset: 0x0001882C
		public readonly Vector3d ExtractScale()
		{
			Vector4d vector4d = this.Row0;
			double length = vector4d.Xyz.Length;
			vector4d = this.Row1;
			double length2 = vector4d.Xyz.Length;
			vector4d = this.Row2;
			return new Vector3d(length, length2, vector4d.Xyz.Length);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001A680 File Offset: 0x00018880
		public readonly Quaterniond ExtractRotation(bool rowNormalize = true)
		{
			Vector4d vector4d = this.Row0;
			Vector3d vector3d = vector4d.Xyz;
			vector4d = this.Row1;
			Vector3d vector3d2 = vector4d.Xyz;
			vector4d = this.Row2;
			Vector3d vector3d3 = vector4d.Xyz;
			if (rowNormalize)
			{
				vector3d = vector3d.Normalized();
				vector3d2 = vector3d2.Normalized();
				vector3d3 = vector3d3.Normalized();
			}
			Quaterniond result = default(Quaterniond);
			double num = 0.25 * (vector3d[0] + vector3d2[1] + vector3d3[2] + 1.0);
			if (num > 0.0)
			{
				double num2 = Math.Sqrt(num);
				result.W = num2;
				num2 = 1.0 / (4.0 * num2);
				result.X = (vector3d2[2] - vector3d3[1]) * num2;
				result.Y = (vector3d3[0] - vector3d[2]) * num2;
				result.Z = (vector3d[1] - vector3d2[0]) * num2;
			}
			else if (vector3d[0] > vector3d2[1] && vector3d[0] > vector3d3[2])
			{
				double num3 = 2.0 * Math.Sqrt(1.0 + vector3d[0] - vector3d2[1] - vector3d3[2]);
				result.X = 0.25 * num3;
				num3 = 1.0 / num3;
				result.W = (vector3d3[1] - vector3d2[2]) * num3;
				result.Y = (vector3d2[0] + vector3d[1]) * num3;
				result.Z = (vector3d3[0] + vector3d[2]) * num3;
			}
			else if (vector3d2[1] > vector3d3[2])
			{
				double num4 = 2.0 * Math.Sqrt(1.0 + vector3d2[1] - vector3d[0] - vector3d3[2]);
				result.Y = 0.25 * num4;
				num4 = 1.0 / num4;
				result.W = (vector3d3[0] - vector3d[2]) * num4;
				result.X = (vector3d2[0] + vector3d[1]) * num4;
				result.Z = (vector3d3[1] + vector3d2[2]) * num4;
			}
			else
			{
				double num5 = 2.0 * Math.Sqrt(1.0 + vector3d3[2] - vector3d[0] - vector3d2[1]);
				result.Z = 0.25 * num5;
				num5 = 1.0 / num5;
				result.W = (vector3d2[0] - vector3d[1]) * num5;
				result.X = (vector3d3[0] + vector3d[2]) * num5;
				result.Y = (vector3d3[1] + vector3d2[2]) * num5;
			}
			result.Normalize();
			return result;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001A9E6 File Offset: 0x00018BE6
		[Obsolete("Use Column3 if the old behaviour is needed, or use ExtractPerspective*() or ExtractOrthographic* instead.")]
		public readonly Vector4d ExtractProjection()
		{
			return this.Column3;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001A9F0 File Offset: 0x00018BF0
		public readonly void ExtractPerspectiveOffCenter(out double left, out double right, out double bottom, out double top, out double depthNear, out double depthFar)
		{
			depthNear = this.Row3.Z / (this.Row2.Z - 1.0);
			depthFar = this.Row3.Z / (this.Row2.Z + 1.0);
			left = depthNear * (this.Row2.X - 1.0) / this.Row0.X;
			right = depthNear * (this.Row2.X + 1.0) / this.Row0.X;
			bottom = depthNear * (this.Row2.Y - 1.0) / this.Row1.Y;
			top = depthNear * (this.Row2.Y + 1.0) / this.Row1.Y;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001AAE4 File Offset: 0x00018CE4
		public readonly void ExtractPerspectiveFieldOfView(out double fovy, out double aspect, out double depthNear, out double depthFar)
		{
			fovy = 2.0 * Math.Atan(1.0 / this.Row1.Y);
			aspect = this.Row1.Y / this.Row0.X;
			depthNear = this.Row3.Z / (this.Row2.Z - 1.0);
			depthFar = this.Row3.Z / (this.Row2.Z + 1.0);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001AB78 File Offset: 0x00018D78
		public readonly void ExtractOrthographicOffCenter(out double left, out double right, out double bottom, out double top, out double depthNear, out double depthFar)
		{
			left = -(1.0 + this.Row3.X) / this.Row0.X;
			right = (1.0 - this.Row3.X) / this.Row0.X;
			bottom = -(1.0 + this.Row3.Y) / this.Row1.Y;
			top = (1.0 - this.Row3.Y) / this.Row1.Y;
			depthNear = (1.0 + this.Row3.Z) / this.Row2.Z;
			depthFar = -(1.0 - this.Row3.Z) / this.Row2.Z;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001AC60 File Offset: 0x00018E60
		public readonly void ExtractOrthographic(out double width, out double height, out double depthNear, out double depthFar)
		{
			double num = -(1.0 + this.Row3.X) / this.Row0.X;
			double num2 = (1.0 - this.Row3.X) / this.Row0.X;
			width = num2 - num;
			double num3 = -(1.0 + this.Row3.Y) / this.Row1.Y;
			double num4 = (1.0 - this.Row3.Y) / this.Row1.Y;
			height = num4 - num3;
			depthNear = (1.0 + this.Row3.Z) / this.Row2.Z;
			depthFar = -(1.0 - this.Row3.Z) / this.Row2.Z;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001AD4C File Offset: 0x00018F4C
		public static void CreateFromAxisAngle(Vector3d axis, double angle, out Matrix4d result)
		{
			axis.Normalize();
			double x = axis.X;
			double y = axis.Y;
			double z = axis.Z;
			double num = Math.Cos(-angle);
			double num2 = Math.Sin(-angle);
			double num3 = 1.0 - num;
			double num4 = num3 * x * x;
			double num5 = num3 * x * y;
			double num6 = num3 * x * z;
			double num7 = num3 * y * y;
			double num8 = num3 * y * z;
			double num9 = num3 * z * z;
			double num10 = num2 * x;
			double num11 = num2 * y;
			double num12 = num2 * z;
			result.Row0.X = num4 + num;
			result.Row0.Y = num5 - num12;
			result.Row0.Z = num6 + num11;
			result.Row0.W = 0.0;
			result.Row1.X = num5 + num12;
			result.Row1.Y = num7 + num;
			result.Row1.Z = num8 - num10;
			result.Row1.W = 0.0;
			result.Row2.X = num6 - num11;
			result.Row2.Y = num8 + num10;
			result.Row2.Z = num9 + num;
			result.Row2.W = 0.0;
			result.Row3 = Vector4d.UnitW;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001AE9C File Offset: 0x0001909C
		public static Matrix4d CreateFromAxisAngle(Vector3d axis, double angle)
		{
			Matrix4d result;
			Matrix4d.CreateFromAxisAngle(axis, angle, out result);
			return result;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001AEB4 File Offset: 0x000190B4
		public static void CreateFromQuaternion(in Quaterniond q, out Matrix4d result)
		{
			double num = q.X * q.X;
			double num2 = q.Y * q.Y;
			double num3 = q.Z * q.Z;
			double num4 = q.W * q.W;
			double num5 = q.X * q.Y;
			double num6 = q.X * q.Z;
			double num7 = q.X * q.W;
			double num8 = q.Y * q.Z;
			double num9 = q.Y * q.W;
			double num10 = q.Z * q.W;
			double num11 = 2.0 / (num + num2 + num3 + num4);
			result.Row0.X = 1.0 - num11 * (num2 + num3);
			result.Row1.Y = 1.0 - num11 * (num + num3);
			result.Row2.Z = 1.0 - num11 * (num + num2);
			result.Row0.Y = num11 * (num5 + num10);
			result.Row1.X = num11 * (num5 - num10);
			result.Row2.X = num11 * (num6 + num9);
			result.Row0.Z = num11 * (num6 - num9);
			result.Row2.Y = num11 * (num8 - num7);
			result.Row1.Z = num11 * (num8 + num7);
			result.Row0.W = 0.0;
			result.Row1.W = 0.0;
			result.Row2.W = 0.0;
			result.Row3 = new Vector4d(0.0, 0.0, 0.0, 1.0);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001B094 File Offset: 0x00019294
		public static Matrix4d CreateFromQuaternion(Quaterniond q)
		{
			Matrix4d result;
			Matrix4d.CreateFromQuaternion(q, out result);
			return result;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001B0AC File Offset: 0x000192AC
		public static void CreateRotationX(double angle, out Matrix4d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0 = Vector4d.UnitX;
			result.Row1 = new Vector4d(0.0, num, num2, 0.0);
			result.Row2 = new Vector4d(0.0, -num2, num, 0.0);
			result.Row3 = Vector4d.UnitW;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001B11C File Offset: 0x0001931C
		public static Matrix4d CreateRotationX(double angle)
		{
			Matrix4d result;
			Matrix4d.CreateRotationX(angle, out result);
			return result;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001B134 File Offset: 0x00019334
		public static void CreateRotationY(double angle, out Matrix4d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0 = new Vector4d(num, 0.0, -num2, 0.0);
			result.Row1 = Vector4d.UnitY;
			result.Row2 = new Vector4d(num2, 0.0, num, 0.0);
			result.Row3 = Vector4d.UnitW;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001B1A4 File Offset: 0x000193A4
		public static Matrix4d CreateRotationY(double angle)
		{
			Matrix4d result;
			Matrix4d.CreateRotationY(angle, out result);
			return result;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001B1BC File Offset: 0x000193BC
		public static void CreateRotationZ(double angle, out Matrix4d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0 = new Vector4d(num, num2, 0.0, 0.0);
			result.Row1 = new Vector4d(-num2, num, 0.0, 0.0);
			result.Row2 = Vector4d.UnitZ;
			result.Row3 = Vector4d.UnitW;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001B22C File Offset: 0x0001942C
		public static Matrix4d CreateRotationZ(double angle)
		{
			Matrix4d result;
			Matrix4d.CreateRotationZ(angle, out result);
			return result;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001B242 File Offset: 0x00019442
		public static void CreateTranslation(double x, double y, double z, out Matrix4d result)
		{
			result = Matrix4d.Identity;
			result.Row3 = new Vector4d(x, y, z, 1.0);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001B266 File Offset: 0x00019466
		public static void CreateTranslation(in Vector3d vector, out Matrix4d result)
		{
			result = Matrix4d.Identity;
			result.Row3 = new Vector4d(vector.X, vector.Y, vector.Z, 1.0);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001B29C File Offset: 0x0001949C
		public static Matrix4d CreateTranslation(double x, double y, double z)
		{
			Matrix4d result;
			Matrix4d.CreateTranslation(x, y, z, out result);
			return result;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001B2B4 File Offset: 0x000194B4
		public static Matrix4d CreateTranslation(Vector3d vector)
		{
			Matrix4d result;
			Matrix4d.CreateTranslation(vector.X, vector.Y, vector.Z, out result);
			return result;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001B2DC File Offset: 0x000194DC
		public static Matrix4d CreateScale(double scale)
		{
			Matrix4d result;
			Matrix4d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001B2F4 File Offset: 0x000194F4
		public static Matrix4d CreateScale(Vector3d scale)
		{
			Matrix4d result;
			Matrix4d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001B30C File Offset: 0x0001950C
		public static Matrix4d CreateScale(double x, double y, double z)
		{
			Matrix4d result;
			Matrix4d.CreateScale(x, y, z, out result);
			return result;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001B324 File Offset: 0x00019524
		public static void CreateScale(double scale, out Matrix4d result)
		{
			result = Matrix4d.Identity;
			result.Row0.X = scale;
			result.Row1.Y = scale;
			result.Row2.Z = scale;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001B355 File Offset: 0x00019555
		public static void CreateScale(in Vector3d scale, out Matrix4d result)
		{
			result = Matrix4d.Identity;
			result.Row0.X = scale.X;
			result.Row1.Y = scale.Y;
			result.Row2.Z = scale.Z;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001B395 File Offset: 0x00019595
		public static void CreateScale(double x, double y, double z, out Matrix4d result)
		{
			result = Matrix4d.Identity;
			result.Row0.X = x;
			result.Row1.Y = y;
			result.Row2.Z = z;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001B3C8 File Offset: 0x000195C8
		public static Matrix4d CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4d result;
			Matrix4d.CreateSwizzle(rowForRow0, rowForRow1, rowForRow2, rowForRow3, out result);
			return result;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001B3E4 File Offset: 0x000195E4
		public static void CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3, out Matrix4d result)
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
			result = Matrix4d.Zero;
			result[0, rowForRow0] = 1.0;
			result[1, rowForRow1] = 1.0;
			result[2, rowForRow2] = 1.0;
			result[3, rowForRow3] = 1.0;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001B4D1 File Offset: 0x000196D1
		public static void CreateOrthographic(double width, double height, double depthNear, double depthFar, out Matrix4d result)
		{
			Matrix4d.CreateOrthographicOffCenter(-width / 2.0, width / 2.0, -height / 2.0, height / 2.0, depthNear, depthFar, out result);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001B50C File Offset: 0x0001970C
		public static Matrix4d CreateOrthographic(double width, double height, double depthNear, double depthFar)
		{
			Matrix4d result;
			Matrix4d.CreateOrthographicOffCenter(-width / 2.0, width / 2.0, -height / 2.0, height / 2.0, depthNear, depthFar, out result);
			return result;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001B554 File Offset: 0x00019754
		public static void CreateOrthographicOffCenter(double left, double right, double bottom, double top, double depthNear, double depthFar, out Matrix4d result)
		{
			double num = 1.0 / (right - left);
			double num2 = 1.0 / (top - bottom);
			double num3 = 1.0 / (depthFar - depthNear);
			result = new Matrix4d
			{
				M11 = 2.0 * num,
				M22 = 2.0 * num2,
				M33 = -2.0 * num3,
				M41 = -(right + left) * num,
				M42 = -(top + bottom) * num2,
				M43 = -(depthFar + depthNear) * num3,
				M44 = 1.0
			};
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001B60C File Offset: 0x0001980C
		public static Matrix4d CreateOrthographicOffCenter(double left, double right, double bottom, double top, double depthNear, double depthFar)
		{
			Matrix4d result;
			Matrix4d.CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar, out result);
			return result;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001B62C File Offset: 0x0001982C
		public static void CreatePerspectiveFieldOfView(double fovy, double aspect, double depthNear, double depthFar, out Matrix4d result)
		{
			if (fovy <= 0.0 || fovy > 3.141592653589793)
			{
				throw new ArgumentOutOfRangeException("fovy");
			}
			if (aspect <= 0.0)
			{
				throw new ArgumentOutOfRangeException("aspect");
			}
			if (depthNear <= 0.0)
			{
				throw new ArgumentOutOfRangeException("depthNear");
			}
			if (depthFar <= 0.0)
			{
				throw new ArgumentOutOfRangeException("depthFar");
			}
			double num = depthNear * Math.Tan(0.5 * fovy);
			double num2 = -num;
			double left = num2 * aspect;
			double right = num * aspect;
			Matrix4d.CreatePerspectiveOffCenter(left, right, num2, num, depthNear, depthFar, out result);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001B6CC File Offset: 0x000198CC
		public static Matrix4d CreatePerspectiveFieldOfView(double fovy, double aspect, double depthNear, double depthFar)
		{
			Matrix4d result;
			Matrix4d.CreatePerspectiveFieldOfView(fovy, aspect, depthNear, depthFar, out result);
			return result;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001B6E8 File Offset: 0x000198E8
		public static void CreatePerspectiveOffCenter(double left, double right, double bottom, double top, double depthNear, double depthFar, out Matrix4d result)
		{
			if (depthNear <= 0.0)
			{
				throw new ArgumentOutOfRangeException("depthNear");
			}
			if (depthFar <= 0.0)
			{
				throw new ArgumentOutOfRangeException("depthFar");
			}
			if (depthNear >= depthFar)
			{
				throw new ArgumentOutOfRangeException("depthNear");
			}
			double m = 2.0 * depthNear / (right - left);
			double m2 = 2.0 * depthNear / (top - bottom);
			double m3 = (right + left) / (right - left);
			double m4 = (top + bottom) / (top - bottom);
			double m5 = -(depthFar + depthNear) / (depthFar - depthNear);
			double m6 = -(2.0 * depthFar * depthNear) / (depthFar - depthNear);
			result = new Matrix4d(m, 0.0, 0.0, 0.0, 0.0, m2, 0.0, 0.0, m3, m4, m5, -1.0, 0.0, 0.0, m6, 0.0);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001B7FC File Offset: 0x000199FC
		public static Matrix4d CreatePerspectiveOffCenter(double left, double right, double bottom, double top, double depthNear, double depthFar)
		{
			Matrix4d result;
			Matrix4d.CreatePerspectiveOffCenter(left, right, bottom, top, depthNear, depthFar, out result);
			return result;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001B819 File Offset: 0x00019A19
		[Obsolete("Use CreateScale instead.")]
		public static Matrix4d Scale(double scale)
		{
			return Matrix4d.Scale(scale, scale, scale);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001B823 File Offset: 0x00019A23
		[Obsolete("Use CreateScale instead.")]
		public static Matrix4d Scale(Vector3d scale)
		{
			return Matrix4d.Scale(scale.X, scale.Y, scale.Z);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001B83C File Offset: 0x00019A3C
		[Obsolete("Use CreateScale instead.")]
		public static Matrix4d Scale(double x, double y, double z)
		{
			Matrix4d result;
			result.Row0 = Vector4d.UnitX * x;
			result.Row1 = Vector4d.UnitY * y;
			result.Row2 = Vector4d.UnitZ * z;
			result.Row3 = Vector4d.UnitW;
			return result;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0001B88C File Offset: 0x00019A8C
		[Obsolete("Use CreateRotationX instead.")]
		public static Matrix4d RotateX(double angle)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			Matrix4d result;
			result.Row0 = Vector4d.UnitX;
			result.Row1 = new Vector4d(0.0, num, num2, 0.0);
			result.Row2 = new Vector4d(0.0, -num2, num, 0.0);
			result.Row3 = Vector4d.UnitW;
			return result;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001B904 File Offset: 0x00019B04
		[Obsolete("Use CreateRotationY instead.")]
		public static Matrix4d RotateY(double angle)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			Matrix4d result;
			result.Row0 = new Vector4d(num, 0.0, -num2, 0.0);
			result.Row1 = Vector4d.UnitY;
			result.Row2 = new Vector4d(num2, 0.0, num, 0.0);
			result.Row3 = Vector4d.UnitW;
			return result;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001B97C File Offset: 0x00019B7C
		[Obsolete("Use CreateRotationZ instead.")]
		public static Matrix4d RotateZ(double angle)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			Matrix4d result;
			result.Row0 = new Vector4d(num, num2, 0.0, 0.0);
			result.Row1 = new Vector4d(-num2, num, 0.0, 0.0);
			result.Row2 = Vector4d.UnitZ;
			result.Row3 = Vector4d.UnitW;
			return result;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001B9F4 File Offset: 0x00019BF4
		[Obsolete("Use CreateFromAxisAngle instead.")]
		public static Matrix4d Rotate(Vector3d axis, double angle)
		{
			double num = Math.Cos(-angle);
			double num2 = Math.Sin(-angle);
			double num3 = 1.0 - num;
			axis.Normalize();
			return new Matrix4d(new Vector4d(num3 * axis.X * axis.X + num, num3 * axis.X * axis.Y - num2 * axis.Z, num3 * axis.X * axis.Z + num2 * axis.Y, 0.0), new Vector4d(num3 * axis.X * axis.Y + num2 * axis.Z, num3 * axis.Y * axis.Y + num, num3 * axis.Y * axis.Z - num2 * axis.X, 0.0), new Vector4d(num3 * axis.X * axis.Z - num2 * axis.Y, num3 * axis.Y * axis.Z + num2 * axis.X, num3 * axis.Z * axis.Z + num, 0.0), Vector4d.UnitW);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001BB1C File Offset: 0x00019D1C
		[Obsolete("Use CreateFromQuaternion instead.")]
		public static Matrix4d Rotate(Quaterniond q)
		{
			Vector3d axis;
			double angle;
			q.ToAxisAngle(out axis, out angle);
			return Matrix4d.Rotate(axis, angle);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001BB3C File Offset: 0x00019D3C
		public static Matrix4d LookAt(Vector3d eye, Vector3d target, Vector3d up)
		{
			Vector3d vector3d = Vector3d.Normalize(eye - target);
			Vector3d vector3d2 = Vector3d.Normalize(Vector3d.Cross(up, vector3d));
			Vector3d vector3d3 = Vector3d.Normalize(Vector3d.Cross(vector3d, vector3d2));
			Matrix4d right = new Matrix4d(new Vector4d(vector3d2.X, vector3d3.X, vector3d.X, 0.0), new Vector4d(vector3d2.Y, vector3d3.Y, vector3d.Y, 0.0), new Vector4d(vector3d2.Z, vector3d3.Z, vector3d.Z, 0.0), Vector4d.UnitW);
			return Matrix4d.CreateTranslation(-eye) * right;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001BBED File Offset: 0x00019DED
		public static Matrix4d LookAt(double eyeX, double eyeY, double eyeZ, double targetX, double targetY, double targetZ, double upX, double upY, double upZ)
		{
			return Matrix4d.LookAt(new Vector3d(eyeX, eyeY, eyeZ), new Vector3d(targetX, targetY, targetZ), new Vector3d(upX, upY, upZ));
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001BC14 File Offset: 0x00019E14
		[Obsolete("Use CreatePerspectiveOffCenter instead.")]
		public static Matrix4d Frustum(double left, double right, double bottom, double top, double depthNear, double depthFar)
		{
			double num = 1.0 / (right - left);
			double num2 = 1.0 / (top - bottom);
			double num3 = 1.0 / (depthFar - depthNear);
			return new Matrix4d(new Vector4d(2.0 * depthNear * num, 0.0, 0.0, 0.0), new Vector4d(0.0, 2.0 * depthNear * num2, 0.0, 0.0), new Vector4d((right + left) * num, (top + bottom) * num2, -(depthFar + depthNear) * num3, -1.0), new Vector4d(0.0, 0.0, -2.0 * depthFar * depthNear * num3, 0.0));
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001BD00 File Offset: 0x00019F00
		[Obsolete("Use CreatePerspectiveFieldOfView instead.")]
		public static Matrix4d Perspective(double fovy, double aspect, double depthNear, double depthFar)
		{
			double num = depthNear * Math.Tan(0.5 * fovy);
			double num2 = -num;
			double left = num2 * aspect;
			double right = num * aspect;
			return Matrix4d.Frustum(left, right, num2, num, depthNear, depthFar);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001BD34 File Offset: 0x00019F34
		public static Matrix4d Add(Matrix4d left, Matrix4d right)
		{
			Matrix4d result;
			Matrix4d.Add(left, right, out result);
			return result;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001BD50 File Offset: 0x00019F50
		public static void Add(in Matrix4d left, in Matrix4d right, out Matrix4d result)
		{
			result.Row0 = left.Row0 + right.Row0;
			result.Row1 = left.Row1 + right.Row1;
			result.Row2 = left.Row2 + right.Row2;
			result.Row3 = left.Row3 + right.Row3;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001BDBC File Offset: 0x00019FBC
		public static Matrix4d Subtract(Matrix4d left, Matrix4d right)
		{
			Matrix4d result;
			Matrix4d.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001BDD8 File Offset: 0x00019FD8
		public static void Subtract(in Matrix4d left, in Matrix4d right, out Matrix4d result)
		{
			result.Row0 = left.Row0 - right.Row0;
			result.Row1 = left.Row1 - right.Row1;
			result.Row2 = left.Row2 - right.Row2;
			result.Row3 = left.Row3 - right.Row3;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001BE44 File Offset: 0x0001A044
		public static Matrix4d Mult(Matrix4d left, Matrix4d right)
		{
			Matrix4d result;
			Matrix4d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001BE60 File Offset: 0x0001A060
		public static void Mult(in Matrix4d left, in Matrix4d right, out Matrix4d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double w = left.Row0.W;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
			double w2 = left.Row1.W;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double z3 = left.Row2.Z;
			double w3 = left.Row2.W;
			double x4 = left.Row3.X;
			double y4 = left.Row3.Y;
			double z4 = left.Row3.Z;
			double w4 = left.Row3.W;
			double x5 = right.Row0.X;
			double y5 = right.Row0.Y;
			double z5 = right.Row0.Z;
			double w5 = right.Row0.W;
			double x6 = right.Row1.X;
			double y6 = right.Row1.Y;
			double z6 = right.Row1.Z;
			double w6 = right.Row1.W;
			double x7 = right.Row2.X;
			double y7 = right.Row2.Y;
			double z7 = right.Row2.Z;
			double w7 = right.Row2.W;
			double x8 = right.Row3.X;
			double y8 = right.Row3.Y;
			double z8 = right.Row3.Z;
			double w8 = right.Row3.W;
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

		// Token: 0x06000736 RID: 1846 RVA: 0x0001C21C File Offset: 0x0001A41C
		public static Matrix4d Mult(Matrix4d left, double right)
		{
			Matrix4d result;
			Matrix4d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001C234 File Offset: 0x0001A434
		public static void Mult(in Matrix4d left, double right, out Matrix4d result)
		{
			result.Row0 = left.Row0 * right;
			result.Row1 = left.Row1 * right;
			result.Row2 = left.Row2 * right;
			result.Row3 = left.Row3 * right;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001C28C File Offset: 0x0001A48C
		public static void Invert(in Matrix4d mat, out Matrix4d result)
		{
			double m = mat.M11;
			double m2 = mat.M21;
			double m3 = mat.M31;
			double m4 = mat.M41;
			double m5 = mat.M12;
			double m6 = mat.M22;
			double m7 = mat.M32;
			double m8 = mat.M42;
			double m9 = mat.M13;
			double m10 = mat.M23;
			double m11 = mat.M33;
			double m12 = mat.M43;
			double m13 = mat.M14;
			double m14 = mat.M24;
			double m15 = mat.M34;
			double m16 = mat.M44;
			double num = m11 * m16 - m12 * m15;
			double num2 = m10 * m16 - m12 * m14;
			double num3 = m10 * m15 - m11 * m14;
			double num4 = m9 * m16 - m12 * m13;
			double num5 = m9 * m15 - m11 * m13;
			double num6 = m9 * m14 - m10 * m13;
			double num7 = m6 * num - m7 * num2 + m8 * num3;
			double num8 = -(m5 * num - m7 * num4 + m8 * num5);
			double num9 = m5 * num2 - m6 * num4 + m8 * num6;
			double num10 = -(m5 * num3 - m6 * num5 + m7 * num6);
			double num11 = m * num7 + m2 * num8 + m3 * num9 + m4 * num10;
			if (Math.Abs(num11) < 5E-324)
			{
				throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
			}
			double scale = 1.0 / num11;
			result.Row0 = new Vector4d(num7, num8, num9, num10) * scale;
			result.Row1 = new Vector4d(-(m2 * num - m3 * num2 + m4 * num3), m * num - m3 * num4 + m4 * num5, -(m * num2 - m2 * num4 + m4 * num6), m * num3 - m2 * num5 + m3 * num6) * scale;
			double num12 = m7 * m16 - m8 * m15;
			double num13 = m6 * m16 - m8 * m14;
			double num14 = m6 * m15 - m7 * m14;
			double num15 = m5 * m16 - m8 * m13;
			double num16 = m5 * m15 - m7 * m13;
			double num17 = m5 * m14 - m6 * m13;
			result.Row2 = new Vector4d(m2 * num12 - m3 * num13 + m4 * num14, -(m * num12 - m3 * num15 + m4 * num16), m * num13 - m2 * num15 + m4 * num17, -(m * num14 - m2 * num16 + m3 * num17)) * scale;
			double num18 = m7 * m12 - m8 * m11;
			double num19 = m6 * m12 - m8 * m10;
			double num20 = m6 * m11 - m7 * m10;
			double num21 = m5 * m12 - m8 * m9;
			double num22 = m5 * m11 - m7 * m9;
			double num23 = m5 * m10 - m6 * m9;
			result.Row3 = new Vector4d(-(m2 * num18 - m3 * num19 + m4 * num20), m * num18 - m3 * num21 + m4 * num22, -(m * num19 - m2 * num21 + m4 * num23), m * num20 - m2 * num22 + m3 * num23) * scale;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001C580 File Offset: 0x0001A780
		public static Matrix4d Invert(in Matrix4d mat)
		{
			Matrix4d result;
			Matrix4d.Invert(mat, out result);
			return result;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001C596 File Offset: 0x0001A796
		public static Matrix4d Transpose(Matrix4d mat)
		{
			return new Matrix4d(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001C5B9 File Offset: 0x0001A7B9
		public static void Transpose(in Matrix4d mat, out Matrix4d result)
		{
			result.Row0 = mat.Column0;
			result.Row1 = mat.Column1;
			result.Row2 = mat.Column2;
			result.Row3 = mat.Column3;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001C5EC File Offset: 0x0001A7EC
		public static Matrix4d Swizzle(Matrix4d mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4d result;
			Matrix4d.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, rowForRow3, out result);
			return result;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001C608 File Offset: 0x0001A808
		public static void Swizzle(in Matrix4d mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3, out Matrix4d result)
		{
			Vector4d vector4d;
			switch (rowForRow0)
			{
			case 0:
				vector4d = mat.Row0;
				break;
			case 1:
				vector4d = mat.Row1;
				break;
			case 2:
				vector4d = mat.Row2;
				break;
			case 3:
				vector4d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow0", rowForRow0));
			}
			result.Row0 = vector4d;
			switch (rowForRow1)
			{
			case 0:
				vector4d = mat.Row0;
				break;
			case 1:
				vector4d = mat.Row1;
				break;
			case 2:
				vector4d = mat.Row2;
				break;
			case 3:
				vector4d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow1", rowForRow1));
			}
			result.Row1 = vector4d;
			switch (rowForRow2)
			{
			case 0:
				vector4d = mat.Row0;
				break;
			case 1:
				vector4d = mat.Row1;
				break;
			case 2:
				vector4d = mat.Row2;
				break;
			case 3:
				vector4d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow2", rowForRow2));
			}
			result.Row2 = vector4d;
			switch (rowForRow3)
			{
			case 0:
				vector4d = mat.Row0;
				break;
			case 1:
				vector4d = mat.Row1;
				break;
			case 2:
				vector4d = mat.Row2;
				break;
			case 3:
				vector4d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow3", rowForRow3));
			}
			result.Row3 = vector4d;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001C793 File Offset: 0x0001A993
		public static Matrix4d operator *(Matrix4d left, Matrix4d right)
		{
			return Matrix4d.Mult(left, right);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0001C79C File Offset: 0x0001A99C
		public static Matrix4d operator *(Matrix4d left, double right)
		{
			return Matrix4d.Mult(left, right);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001C7A5 File Offset: 0x0001A9A5
		public static Matrix4d operator +(Matrix4d left, Matrix4d right)
		{
			return Matrix4d.Add(left, right);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0001C7AE File Offset: 0x0001A9AE
		public static Matrix4d operator -(Matrix4d left, Matrix4d right)
		{
			return Matrix4d.Subtract(left, right);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001C7B7 File Offset: 0x0001A9B7
		public static bool operator ==(Matrix4d left, Matrix4d right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001C7C1 File Offset: 0x0001A9C1
		public static bool operator !=(Matrix4d left, Matrix4d right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001C7CE File Offset: 0x0001A9CE
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001C7D8 File Offset: 0x0001A9D8
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001C7E2 File Offset: 0x0001A9E2
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001C7EC File Offset: 0x0001A9EC
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

		// Token: 0x06000748 RID: 1864 RVA: 0x0001C864 File Offset: 0x0001AA64
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector4d, Vector4d, Vector4d, Vector4d>(this.Row0, this.Row1, this.Row2, this.Row3);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001C883 File Offset: 0x0001AA83
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix4d && this.Equals((Matrix4d)obj);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001C89C File Offset: 0x0001AA9C
		public readonly bool Equals(Matrix4d other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2 && this.Row3 == other.Row3;
		}

		// Token: 0x0400006C RID: 108
		public Vector4d Row0;

		// Token: 0x0400006D RID: 109
		public Vector4d Row1;

		// Token: 0x0400006E RID: 110
		public Vector4d Row2;

		// Token: 0x0400006F RID: 111
		public Vector4d Row3;

		// Token: 0x04000070 RID: 112
		public static readonly Matrix4d Identity = new Matrix4d(Vector4d.UnitX, Vector4d.UnitY, Vector4d.UnitZ, Vector4d.UnitW);

		// Token: 0x04000071 RID: 113
		public static readonly Matrix4d Zero = new Matrix4d(Vector4d.Zero, Vector4d.Zero, Vector4d.Zero, Vector4d.Zero);
	}
}
