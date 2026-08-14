using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x0200001F RID: 31
	[Serializable]
	public struct Matrix4x2d : IEquatable<Matrix4x2d>, IFormattable
	{
		// Token: 0x06000794 RID: 1940 RVA: 0x0001DC29 File Offset: 0x0001BE29
		public Matrix4x2d(Vector2d row0, Vector2d row1, Vector2d row2, Vector2d row3)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
			this.Row3 = row3;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001DC48 File Offset: 0x0001BE48
		public Matrix4x2d(double m00, double m01, double m10, double m11, double m20, double m21, double m30, double m31)
		{
			this.Row0 = new Vector2d(m00, m01);
			this.Row1 = new Vector2d(m10, m11);
			this.Row2 = new Vector2d(m20, m21);
			this.Row3 = new Vector2d(m30, m31);
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x0001DC83 File Offset: 0x0001BE83
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x0001DCB8 File Offset: 0x0001BEB8
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

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x0001DD09 File Offset: 0x0001BF09
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x0001DD3C File Offset: 0x0001BF3C
		public Vector4d Column1
		{
			readonly get
			{
				return new Vector4d(this.Row0.Y, this.Row1.Y, this.Row2.Y, this.Row3.X);
			}
			set
			{
				this.Row0.Y = value.X;
				this.Row1.Y = value.Y;
				this.Row2.Y = value.Z;
				this.Row3.Y = value.W;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0001DD8D File Offset: 0x0001BF8D
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x0001DD9A File Offset: 0x0001BF9A
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

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0001DDA8 File Offset: 0x0001BFA8
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x0001DDB5 File Offset: 0x0001BFB5
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

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0001DDC3 File Offset: 0x0001BFC3
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x0001DDD0 File Offset: 0x0001BFD0
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

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0001DDDE File Offset: 0x0001BFDE
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x0001DDEB File Offset: 0x0001BFEB
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

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0001DDF9 File Offset: 0x0001BFF9
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x0001DE06 File Offset: 0x0001C006
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

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0001DE14 File Offset: 0x0001C014
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x0001DE21 File Offset: 0x0001C021
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

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0001DE2F File Offset: 0x0001C02F
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0001DE3C File Offset: 0x0001C03C
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

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0001DE4A File Offset: 0x0001C04A
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x0001DE57 File Offset: 0x0001C057
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

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0001DE65 File Offset: 0x0001C065
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x0001DE82 File Offset: 0x0001C082
		public Vector2d Diagonal
		{
			readonly get
			{
				return new Vector2d(this.Row0.X, this.Row1.Y);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.Y = value.Y;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x0001DEA6 File Offset: 0x0001C0A6
		public readonly double Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y;
			}
		}

		// Token: 0x170001AA RID: 426
		public double this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 4 && columnIndex < 2)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0.0;
			}
			set
			{
				if (rowIndex < 4 && columnIndex < 2)
				{
					this.GetRowUnsafe(this, rowIndex)[columnIndex] = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to set this matrix at: ({0}, {1})", rowIndex, columnIndex));
			}
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0001DF31 File Offset: 0x0001C131
		[MethodImpl(256)]
		private readonly ref Vector2d GetRowUnsafe(in Matrix4x2d m, int index)
		{
			return Unsafe.Add<Vector2d>(Unsafe.AsRef<Vector2d>(ref m.Row0), index);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0001DF44 File Offset: 0x0001C144
		public readonly Matrix2x4d Transposed()
		{
			return Matrix4x2d.Transpose(this);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001DF51 File Offset: 0x0001C151
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			this = Matrix4x2d.Swizzle(this, rowForRow0, rowForRow1, rowForRow2, rowForRow3);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001DF6C File Offset: 0x0001C16C
		public readonly Matrix4x2d Swizzled(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4x2d result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2, rowForRow3);
			return result;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001DF90 File Offset: 0x0001C190
		public static void CreateRotation(double angle, out Matrix4x2d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row1.X = -num2;
			result.Row1.Y = num;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row3.X = 0.0;
			result.Row3.Y = 0.0;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0001E02C File Offset: 0x0001C22C
		public static Matrix4x2d CreateRotation(double angle)
		{
			Matrix4x2d result;
			Matrix4x2d.CreateRotation(angle, out result);
			return result;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001E044 File Offset: 0x0001C244
		public static void CreateScale(double scale, out Matrix4x2d result)
		{
			result.Row0.X = scale;
			result.Row0.Y = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = scale;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row3.X = 0.0;
			result.Row3.Y = 0.0;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001E0E4 File Offset: 0x0001C2E4
		public static Matrix4x2d CreateScale(double scale)
		{
			Matrix4x2d result;
			Matrix4x2d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001E0FC File Offset: 0x0001C2FC
		public static void CreateScale(Vector2d scale, out Matrix4x2d result)
		{
			result.Row0.X = scale.X;
			result.Row0.Y = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = scale.Y;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row3.X = 0.0;
			result.Row3.Y = 0.0;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001E1A4 File Offset: 0x0001C3A4
		public static Matrix4x2d CreateScale(Vector2d scale)
		{
			Matrix4x2d result;
			Matrix4x2d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001E1BC File Offset: 0x0001C3BC
		public static void CreateScale(double x, double y, out Matrix4x2d result)
		{
			result.Row0.X = x;
			result.Row0.Y = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = y;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row3.X = 0.0;
			result.Row3.Y = 0.0;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001E25C File Offset: 0x0001C45C
		public static Matrix4x2d CreateScale(double x, double y)
		{
			Matrix4x2d result;
			Matrix4x2d.CreateScale(x, y, out result);
			return result;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001E274 File Offset: 0x0001C474
		public static void Mult(in Matrix4x2d left, double right, out Matrix4x2d result)
		{
			result.Row0.X = left.Row0.X * right;
			result.Row0.Y = left.Row0.Y * right;
			result.Row1.X = left.Row1.X * right;
			result.Row1.Y = left.Row1.Y * right;
			result.Row2.X = left.Row2.X * right;
			result.Row2.Y = left.Row2.Y * right;
			result.Row3.X = left.Row3.X * right;
			result.Row3.Y = left.Row3.Y * right;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001E344 File Offset: 0x0001C544
		public static Matrix4x2d Mult(Matrix4x2d left, double right)
		{
			Matrix4x2d result;
			Matrix4x2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001E35C File Offset: 0x0001C55C
		public static void Mult(in Matrix4x2d left, in Matrix2d right, out Matrix4x2d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double x4 = left.Row3.X;
			double y4 = left.Row3.Y;
			double x5 = right.Row0.X;
			double y5 = right.Row0.Y;
			double x6 = right.Row1.X;
			double y6 = right.Row1.Y;
			result.Row0.X = x * x5 + y * x6;
			result.Row0.Y = x * y5 + y * y6;
			result.Row1.X = x2 * x5 + y2 * x6;
			result.Row1.Y = x2 * y5 + y2 * y6;
			result.Row2.X = x3 * x5 + y3 * x6;
			result.Row2.Y = x3 * y5 + y3 * y6;
			result.Row3.X = x4 * x5 + y4 * x6;
			result.Row3.Y = x4 * y5 + y4 * y6;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001E4AC File Offset: 0x0001C6AC
		public static Matrix4x2d Mult(Matrix4x2d left, Matrix2d right)
		{
			Matrix4x2d result;
			Matrix4x2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001E4C8 File Offset: 0x0001C6C8
		public static void Mult(in Matrix4x2d left, in Matrix2x3d right, out Matrix4x3d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double x4 = left.Row3.X;
			double y4 = left.Row3.Y;
			double x5 = right.Row0.X;
			double y5 = right.Row0.Y;
			double z = right.Row0.Z;
			double x6 = right.Row1.X;
			double y6 = right.Row1.Y;
			double z2 = right.Row1.Z;
			result.Row0.X = x * x5 + y * x6;
			result.Row0.Y = x * y5 + y * y6;
			result.Row0.Z = x * z + y * z2;
			result.Row1.X = x2 * x5 + y2 * x6;
			result.Row1.Y = x2 * y5 + y2 * y6;
			result.Row1.Z = x2 * z + y2 * z2;
			result.Row2.X = x3 * x5 + y3 * x6;
			result.Row2.Y = x3 * y5 + y3 * y6;
			result.Row2.Z = x3 * z + y3 * z2;
			result.Row3.X = x4 * x5 + y4 * x6;
			result.Row3.Y = x4 * y5 + y4 * y6;
			result.Row3.Z = x4 * z + y4 * z2;
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001E684 File Offset: 0x0001C884
		public static Matrix4x3d Mult(Matrix4x2d left, Matrix2x3d right)
		{
			Matrix4x3d result;
			Matrix4x2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001E6A0 File Offset: 0x0001C8A0
		public static void Mult(in Matrix4x2d left, in Matrix2x4d right, out Matrix4d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double x4 = left.Row3.X;
			double y4 = left.Row3.Y;
			double x5 = right.Row0.X;
			double y5 = right.Row0.Y;
			double z = right.Row0.Z;
			double w = right.Row0.W;
			double x6 = right.Row1.X;
			double y6 = right.Row1.Y;
			double z2 = right.Row1.Z;
			double w2 = right.Row1.W;
			result.Row0.X = x * x5 + y * x6;
			result.Row0.Y = x * y5 + y * y6;
			result.Row0.Z = x * z + y * z2;
			result.Row0.W = x * w + y * w2;
			result.Row1.X = x2 * x5 + y2 * x6;
			result.Row1.Y = x2 * y5 + y2 * y6;
			result.Row1.Z = x2 * z + y2 * z2;
			result.Row1.W = x2 * w + y2 * w2;
			result.Row2.X = x3 * x5 + y3 * x6;
			result.Row2.Y = x3 * y5 + y3 * y6;
			result.Row2.Z = x3 * z + y3 * z2;
			result.Row2.W = x3 * w + y3 * w2;
			result.Row3.X = x4 * x5 + y4 * x6;
			result.Row3.Y = x4 * y5 + y4 * y6;
			result.Row3.Z = x4 * z + y4 * z2;
			result.Row3.W = x4 * w + y4 * w2;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001E8CC File Offset: 0x0001CACC
		public static Matrix4d Mult(Matrix4x2d left, Matrix2x4d right)
		{
			Matrix4d result;
			Matrix4x2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0001E8E8 File Offset: 0x0001CAE8
		public static void Add(in Matrix4x2d left, in Matrix4x2d right, out Matrix4x2d result)
		{
			result.Row0.X = left.Row0.X + right.Row0.X;
			result.Row0.Y = left.Row0.Y + right.Row0.Y;
			result.Row1.X = left.Row1.X + right.Row1.X;
			result.Row1.Y = left.Row1.Y + right.Row1.Y;
			result.Row2.X = left.Row2.X + right.Row2.X;
			result.Row2.Y = left.Row2.Y + right.Row2.Y;
			result.Row3.X = left.Row3.X + right.Row3.X;
			result.Row3.Y = left.Row3.Y + right.Row3.Y;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001EA08 File Offset: 0x0001CC08
		public static Matrix4x2d Add(Matrix4x2d left, Matrix4x2d right)
		{
			Matrix4x2d result;
			Matrix4x2d.Add(left, right, out result);
			return result;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001EA24 File Offset: 0x0001CC24
		public static void Subtract(in Matrix4x2d left, in Matrix4x2d right, out Matrix4x2d result)
		{
			result.Row0.X = left.Row0.X - right.Row0.X;
			result.Row0.Y = left.Row0.Y - right.Row0.Y;
			result.Row1.X = left.Row1.X - right.Row1.X;
			result.Row1.Y = left.Row1.Y - right.Row1.Y;
			result.Row2.X = left.Row2.X - right.Row2.X;
			result.Row2.Y = left.Row2.Y - right.Row2.Y;
			result.Row3.X = left.Row3.X - right.Row3.X;
			result.Row3.Y = left.Row3.Y - right.Row3.Y;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0001EB44 File Offset: 0x0001CD44
		public static Matrix4x2d Subtract(Matrix4x2d left, Matrix4x2d right)
		{
			Matrix4x2d result;
			Matrix4x2d.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001EB60 File Offset: 0x0001CD60
		public static void Transpose(in Matrix4x2d mat, out Matrix2x4d result)
		{
			result.Row0.X = mat.Row0.X;
			result.Row0.Y = mat.Row1.X;
			result.Row0.Z = mat.Row2.X;
			result.Row0.W = mat.Row3.X;
			result.Row1.X = mat.Row0.Y;
			result.Row1.Y = mat.Row1.Y;
			result.Row1.Z = mat.Row2.Y;
			result.Row1.W = mat.Row3.Y;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0001EC20 File Offset: 0x0001CE20
		public static Matrix2x4d Transpose(Matrix4x2d mat)
		{
			Matrix2x4d result;
			Matrix4x2d.Transpose(mat, out result);
			return result;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0001EC38 File Offset: 0x0001CE38
		public static Matrix4x2d Swizzle(Matrix4x2d mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4x2d result;
			Matrix4x2d.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, rowForRow3, out result);
			return result;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001EC54 File Offset: 0x0001CE54
		public static void Swizzle(in Matrix4x2d mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3, out Matrix4x2d result)
		{
			Vector2d vector2d;
			switch (rowForRow0)
			{
			case 0:
				vector2d = mat.Row0;
				break;
			case 1:
				vector2d = mat.Row1;
				break;
			case 2:
				vector2d = mat.Row2;
				break;
			case 3:
				vector2d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow0", rowForRow0));
			}
			result.Row0 = vector2d;
			switch (rowForRow1)
			{
			case 0:
				vector2d = mat.Row0;
				break;
			case 1:
				vector2d = mat.Row1;
				break;
			case 2:
				vector2d = mat.Row2;
				break;
			case 3:
				vector2d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow1", rowForRow1));
			}
			result.Row1 = vector2d;
			switch (rowForRow2)
			{
			case 0:
				vector2d = mat.Row0;
				break;
			case 1:
				vector2d = mat.Row1;
				break;
			case 2:
				vector2d = mat.Row2;
				break;
			case 3:
				vector2d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow2", rowForRow2));
			}
			result.Row2 = vector2d;
			switch (rowForRow3)
			{
			case 0:
				vector2d = mat.Row0;
				break;
			case 1:
				vector2d = mat.Row1;
				break;
			case 2:
				vector2d = mat.Row2;
				break;
			case 3:
				vector2d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow3", rowForRow3));
			}
			result.Row3 = vector2d;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001EDDF File Offset: 0x0001CFDF
		public static Matrix4x2d operator *(double left, Matrix4x2d right)
		{
			return Matrix4x2d.Mult(right, left);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0001EDE8 File Offset: 0x0001CFE8
		public static Matrix4x2d operator *(Matrix4x2d left, double right)
		{
			return Matrix4x2d.Mult(left, right);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0001EDF1 File Offset: 0x0001CFF1
		public static Matrix4x2d operator *(Matrix4x2d left, Matrix2d right)
		{
			return Matrix4x2d.Mult(left, right);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0001EDFA File Offset: 0x0001CFFA
		public static Matrix4x3d operator *(Matrix4x2d left, Matrix2x3d right)
		{
			return Matrix4x2d.Mult(left, right);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0001EE03 File Offset: 0x0001D003
		public static Matrix4d operator *(Matrix4x2d left, Matrix2x4d right)
		{
			return Matrix4x2d.Mult(left, right);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0001EE0C File Offset: 0x0001D00C
		public static Matrix4x2d operator +(Matrix4x2d left, Matrix4x2d right)
		{
			return Matrix4x2d.Add(left, right);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0001EE15 File Offset: 0x0001D015
		public static Matrix4x2d operator -(Matrix4x2d left, Matrix4x2d right)
		{
			return Matrix4x2d.Subtract(left, right);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0001EE1E File Offset: 0x0001D01E
		public static bool operator ==(Matrix4x2d left, Matrix4x2d right)
		{
			return left.Equals(right);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001EE28 File Offset: 0x0001D028
		public static bool operator !=(Matrix4x2d left, Matrix4x2d right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0001EE35 File Offset: 0x0001D035
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0001EE3F File Offset: 0x0001D03F
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0001EE49 File Offset: 0x0001D049
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0001EE54 File Offset: 0x0001D054
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

		// Token: 0x060007D8 RID: 2008 RVA: 0x0001EECC File Offset: 0x0001D0CC
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector2d, Vector2d, Vector2d, Vector2d>(this.Row0, this.Row1, this.Row2, this.Row3);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0001EEEB File Offset: 0x0001D0EB
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix4x2d && this.Equals((Matrix4x2d)obj);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0001EF04 File Offset: 0x0001D104
		public readonly bool Equals(Matrix4x2d other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2 && this.Row3 == other.Row3;
		}

		// Token: 0x04000077 RID: 119
		public Vector2d Row0;

		// Token: 0x04000078 RID: 120
		public Vector2d Row1;

		// Token: 0x04000079 RID: 121
		public Vector2d Row2;

		// Token: 0x0400007A RID: 122
		public Vector2d Row3;

		// Token: 0x0400007B RID: 123
		public static readonly Matrix4x2d Zero = new Matrix4x2d(Vector2d.Zero, Vector2d.Zero, Vector2d.Zero, Vector2d.Zero);
	}
}
