using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	public struct Matrix2x3 : IEquatable<Matrix2x3>, IFormattable
	{
		// Token: 0x06000328 RID: 808 RVA: 0x0000ACBC File Offset: 0x00008EBC
		public Matrix2x3(Vector3 row0, Vector3 row1)
		{
			this.Row0 = row0;
			this.Row1 = row1;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000ACCC File Offset: 0x00008ECC
		public Matrix2x3(float m00, float m01, float m02, float m10, float m11, float m12)
		{
			this.Row0 = new Vector3(m00, m01, m02);
			this.Row1 = new Vector3(m10, m11, m12);
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000ACED File Offset: 0x00008EED
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0000AD0A File Offset: 0x00008F0A
		public Vector2 Column0
		{
			readonly get
			{
				return new Vector2(this.Row0.X, this.Row1.X);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.X = value.Y;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000AD2E File Offset: 0x00008F2E
		// (set) Token: 0x0600032D RID: 813 RVA: 0x0000AD4B File Offset: 0x00008F4B
		public Vector2 Column1
		{
			readonly get
			{
				return new Vector2(this.Row0.Y, this.Row1.Y);
			}
			set
			{
				this.Row0.Y = value.X;
				this.Row1.Y = value.Y;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000AD6F File Offset: 0x00008F6F
		// (set) Token: 0x0600032F RID: 815 RVA: 0x0000AD8C File Offset: 0x00008F8C
		public Vector2 Column2
		{
			readonly get
			{
				return new Vector2(this.Row0.Z, this.Row1.Z);
			}
			set
			{
				this.Row0.Z = value.X;
				this.Row1.Z = value.Y;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000ADB0 File Offset: 0x00008FB0
		// (set) Token: 0x06000331 RID: 817 RVA: 0x0000ADBD File Offset: 0x00008FBD
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

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000ADCB File Offset: 0x00008FCB
		// (set) Token: 0x06000333 RID: 819 RVA: 0x0000ADD8 File Offset: 0x00008FD8
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

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000ADE6 File Offset: 0x00008FE6
		// (set) Token: 0x06000335 RID: 821 RVA: 0x0000ADF3 File Offset: 0x00008FF3
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

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000AE01 File Offset: 0x00009001
		// (set) Token: 0x06000337 RID: 823 RVA: 0x0000AE0E File Offset: 0x0000900E
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

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000AE1C File Offset: 0x0000901C
		// (set) Token: 0x06000339 RID: 825 RVA: 0x0000AE29 File Offset: 0x00009029
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

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000AE37 File Offset: 0x00009037
		// (set) Token: 0x0600033B RID: 827 RVA: 0x0000AE44 File Offset: 0x00009044
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

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000AE52 File Offset: 0x00009052
		// (set) Token: 0x0600033D RID: 829 RVA: 0x0000AE6F File Offset: 0x0000906F
		public Vector2 Diagonal
		{
			readonly get
			{
				return new Vector2(this.Row0.X, this.Row1.Y);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.Y = value.Y;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000AE93 File Offset: 0x00009093
		public readonly float Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y;
			}
		}

		// Token: 0x170000DA RID: 218
		public float this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 2 && columnIndex < 3)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0f;
			}
			set
			{
				if (rowIndex < 2 && columnIndex < 3)
				{
					this.GetRowUnsafe(this, rowIndex)[columnIndex] = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to set this matrix at: ({0}, {1})", rowIndex, columnIndex));
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000AF1A File Offset: 0x0000911A
		[MethodImpl(256)]
		private readonly ref Vector3 GetRowUnsafe(in Matrix2x3 m, int index)
		{
			return Unsafe.Add<Vector3>(Unsafe.AsRef<Vector3>(ref m.Row0), index);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000AF2D File Offset: 0x0000912D
		public readonly Matrix3x2 Transposed()
		{
			return Matrix2x3.Transpose(this);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000AF3A File Offset: 0x0000913A
		public void Swizzle(int rowForRow0, int rowForRow1)
		{
			this = Matrix2x3.Swizzle(this, rowForRow0, rowForRow1);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000AF50 File Offset: 0x00009150
		public readonly Matrix2x3 Swizzled(int rowForRow0, int rowForRow1)
		{
			Matrix2x3 result = this;
			result.Swizzle(rowForRow0, rowForRow1);
			return result;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000AF70 File Offset: 0x00009170
		public static void CreateRotation(float angle, out Matrix2x3 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row0.Z = 0f;
			result.Row1.X = -num2;
			result.Row1.Y = num;
			result.Row1.Z = 0f;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000AFDC File Offset: 0x000091DC
		public static Matrix2x3 CreateRotation(float angle)
		{
			Matrix2x3 result;
			Matrix2x3.CreateRotation(angle, out result);
			return result;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000AFF4 File Offset: 0x000091F4
		public static void CreateScale(float scale, out Matrix2x3 result)
		{
			result.Row0.X = scale;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = scale;
			result.Row1.Z = 0f;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000B05C File Offset: 0x0000925C
		public static Matrix2x3 CreateScale(float scale)
		{
			Matrix2x3 result;
			Matrix2x3.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000B074 File Offset: 0x00009274
		public static void CreateScale(Vector2 scale, out Matrix2x3 result)
		{
			result.Row0.X = scale.X;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = scale.Y;
			result.Row1.Z = 0f;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000B0E4 File Offset: 0x000092E4
		public static Matrix2x3 CreateScale(Vector2 scale)
		{
			Matrix2x3 result;
			Matrix2x3.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000B0FC File Offset: 0x000092FC
		public static void CreateScale(float x, float y, out Matrix2x3 result)
		{
			result.Row0.X = x;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = y;
			result.Row1.Z = 0f;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000B164 File Offset: 0x00009364
		public static Matrix2x3 CreateScale(float x, float y)
		{
			Matrix2x3 result;
			Matrix2x3.CreateScale(x, y, out result);
			return result;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000B17C File Offset: 0x0000937C
		public static void Mult(in Matrix2x3 left, float right, out Matrix2x3 result)
		{
			result.Row0.X = left.Row0.X * right;
			result.Row0.Y = left.Row0.Y * right;
			result.Row0.Z = left.Row0.Z * right;
			result.Row1.X = left.Row1.X * right;
			result.Row1.Y = left.Row1.Y * right;
			result.Row1.Z = left.Row1.Z * right;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000B21C File Offset: 0x0000941C
		public static Matrix2x3 Mult(Matrix2x3 left, float right)
		{
			Matrix2x3 result;
			Matrix2x3.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000B234 File Offset: 0x00009434
		public static void Mult(in Matrix2x3 left, in Matrix3x2 right, out Matrix2 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float z = left.Row0.Z;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float z2 = left.Row1.Z;
			float x3 = right.Row0.X;
			float y3 = right.Row0.Y;
			float x4 = right.Row1.X;
			float y4 = right.Row1.Y;
			float x5 = right.Row2.X;
			float y5 = right.Row2.Y;
			result.Row0.X = x * x3 + y * x4 + z * x5;
			result.Row0.Y = x * y3 + y * y4 + z * y5;
			result.Row1.X = x2 * x3 + y2 * x4 + z2 * x5;
			result.Row1.Y = x2 * y3 + y2 * y4 + z2 * y5;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000B344 File Offset: 0x00009544
		public static Matrix2 Mult(Matrix2x3 left, Matrix3x2 right)
		{
			Matrix2 result;
			Matrix2x3.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000B360 File Offset: 0x00009560
		public static void Mult(in Matrix2x3 left, in Matrix3 right, out Matrix2x3 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float z = left.Row0.Z;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float z2 = left.Row1.Z;
			float x3 = right.Row0.X;
			float y3 = right.Row0.Y;
			float z3 = right.Row0.Z;
			float x4 = right.Row1.X;
			float y4 = right.Row1.Y;
			float z4 = right.Row1.Z;
			float x5 = right.Row2.X;
			float y5 = right.Row2.Y;
			float z5 = right.Row2.Z;
			result.Row0.X = x * x3 + y * x4 + z * x5;
			result.Row0.Y = x * y3 + y * y4 + z * y5;
			result.Row0.Z = x * z3 + y * z4 + z * z5;
			result.Row1.X = x2 * x3 + y2 * x4 + z2 * x5;
			result.Row1.Y = x2 * y3 + y2 * y4 + z2 * y5;
			result.Row1.Z = x2 * z3 + y2 * z4 + z2 * z5;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000B4C8 File Offset: 0x000096C8
		public static Matrix2x3 Mult(Matrix2x3 left, Matrix3 right)
		{
			Matrix2x3 result;
			Matrix2x3.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000B4E4 File Offset: 0x000096E4
		public static void Mult(in Matrix2x3 left, in Matrix3x4 right, out Matrix2x4 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float z = left.Row0.Z;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float z2 = left.Row1.Z;
			float x3 = right.Row0.X;
			float y3 = right.Row0.Y;
			float z3 = right.Row0.Z;
			float w = right.Row0.W;
			float x4 = right.Row1.X;
			float y4 = right.Row1.Y;
			float z4 = right.Row1.Z;
			float w2 = right.Row1.W;
			float x5 = right.Row2.X;
			float y5 = right.Row2.Y;
			float z5 = right.Row2.Z;
			float w3 = right.Row2.W;
			result.Row0.X = x * x3 + y * x4 + z * x5;
			result.Row0.Y = x * y3 + y * y4 + z * y5;
			result.Row0.Z = x * z3 + y * z4 + z * z5;
			result.Row0.W = x * w + y * w2 + z * w3;
			result.Row1.X = x2 * x3 + y2 * x4 + z2 * x5;
			result.Row1.Y = x2 * y3 + y2 * y4 + z2 * y5;
			result.Row1.Z = x2 * z3 + y2 * z4 + z2 * z5;
			result.Row1.W = x2 * w + y2 * w2 + z2 * w3;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000B6A8 File Offset: 0x000098A8
		public static Matrix2x4 Mult(Matrix2x3 left, Matrix3x4 right)
		{
			Matrix2x4 result;
			Matrix2x3.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000B6C4 File Offset: 0x000098C4
		public static void Add(in Matrix2x3 left, in Matrix2x3 right, out Matrix2x3 result)
		{
			result.Row0.X = left.Row0.X + right.Row0.X;
			result.Row0.Y = left.Row0.Y + right.Row0.Y;
			result.Row0.Z = left.Row0.Z + right.Row0.Z;
			result.Row1.X = left.Row1.X + right.Row1.X;
			result.Row1.Y = left.Row1.Y + right.Row1.Y;
			result.Row1.Z = left.Row1.Z + right.Row1.Z;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public static Matrix2x3 Add(Matrix2x3 left, Matrix2x3 right)
		{
			Matrix2x3 result;
			Matrix2x3.Add(left, right, out result);
			return result;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000B7BC File Offset: 0x000099BC
		public static void Subtract(in Matrix2x3 left, in Matrix2x3 right, out Matrix2x3 result)
		{
			result.Row0.X = left.Row0.X - right.Row0.X;
			result.Row0.Y = left.Row0.Y - right.Row0.Y;
			result.Row0.Z = left.Row0.Z - right.Row0.Z;
			result.Row1.X = left.Row1.X - right.Row1.X;
			result.Row1.Y = left.Row1.Y - right.Row1.Y;
			result.Row1.Z = left.Row1.Z - right.Row1.Z;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000B898 File Offset: 0x00009A98
		public static Matrix2x3 Subtract(Matrix2x3 left, Matrix2x3 right)
		{
			Matrix2x3 result;
			Matrix2x3.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000B8B4 File Offset: 0x00009AB4
		public static void Transpose(in Matrix2x3 mat, out Matrix3x2 result)
		{
			result.Row0.X = mat.Row0.X;
			result.Row0.Y = mat.Row1.X;
			result.Row1.X = mat.Row0.Y;
			result.Row1.Y = mat.Row1.Y;
			result.Row2.X = mat.Row0.Z;
			result.Row2.Y = mat.Row1.Z;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000B948 File Offset: 0x00009B48
		public static Matrix3x2 Transpose(Matrix2x3 mat)
		{
			Matrix3x2 result;
			Matrix2x3.Transpose(mat, out result);
			return result;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000B960 File Offset: 0x00009B60
		public static Matrix2x3 Swizzle(Matrix2x3 mat, int rowForRow0, int rowForRow1)
		{
			Matrix2x3 result;
			Matrix2x3.Swizzle(mat, rowForRow0, rowForRow1, out result);
			return result;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000B97C File Offset: 0x00009B7C
		public static void Swizzle(in Matrix2x3 mat, int rowForRow0, int rowForRow1, out Matrix2x3 result)
		{
			Vector3 vector;
			if (rowForRow0 != 0)
			{
				if (rowForRow0 != 1)
				{
					throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow0", rowForRow0));
				}
				vector = mat.Row1;
			}
			else
			{
				vector = mat.Row0;
			}
			result.Row0 = vector;
			if (rowForRow1 != 0)
			{
				if (rowForRow1 != 1)
				{
					throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow1", rowForRow1));
				}
				vector = mat.Row1;
			}
			else
			{
				vector = mat.Row0;
			}
			result.Row1 = vector;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000BA03 File Offset: 0x00009C03
		public static Matrix2x3 operator *(float left, Matrix2x3 right)
		{
			return Matrix2x3.Mult(right, left);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000BA0C File Offset: 0x00009C0C
		public static Matrix2x3 operator *(Matrix2x3 left, float right)
		{
			return Matrix2x3.Mult(left, right);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000BA15 File Offset: 0x00009C15
		public static Matrix2 operator *(Matrix2x3 left, Matrix3x2 right)
		{
			return Matrix2x3.Mult(left, right);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000BA1E File Offset: 0x00009C1E
		public static Matrix2x3 operator *(Matrix2x3 left, Matrix3 right)
		{
			return Matrix2x3.Mult(left, right);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000BA27 File Offset: 0x00009C27
		public static Matrix2x4 operator *(Matrix2x3 left, Matrix3x4 right)
		{
			return Matrix2x3.Mult(left, right);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000BA30 File Offset: 0x00009C30
		public static Matrix2x3 operator +(Matrix2x3 left, Matrix2x3 right)
		{
			return Matrix2x3.Add(left, right);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000BA39 File Offset: 0x00009C39
		public static Matrix2x3 operator -(Matrix2x3 left, Matrix2x3 right)
		{
			return Matrix2x3.Subtract(left, right);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000BA42 File Offset: 0x00009C42
		public static bool operator ==(Matrix2x3 left, Matrix2x3 right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000BA4C File Offset: 0x00009C4C
		public static bool operator !=(Matrix2x3 left, Matrix2x3 right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000BA59 File Offset: 0x00009C59
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000BA63 File Offset: 0x00009C63
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000BA6D File Offset: 0x00009C6D
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000BA78 File Offset: 0x00009C78
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			string text = this.Row0.ToString(format, formatProvider);
			string text2 = this.Row1.ToString(format, formatProvider);
			return text + "\n" + text2;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000BAAB File Offset: 0x00009CAB
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3, Vector3>(this.Row0, this.Row1);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000BABE File Offset: 0x00009CBE
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix2x3 && this.Equals((Matrix2x3)obj);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000BAD6 File Offset: 0x00009CD6
		public readonly bool Equals(Matrix2x3 other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1;
		}

		// Token: 0x04000040 RID: 64
		public Vector3 Row0;

		// Token: 0x04000041 RID: 65
		public Vector3 Row1;

		// Token: 0x04000042 RID: 66
		public static readonly Matrix2x3 Zero = new Matrix2x3(Vector3.Zero, Vector3.Zero);
	}
}
