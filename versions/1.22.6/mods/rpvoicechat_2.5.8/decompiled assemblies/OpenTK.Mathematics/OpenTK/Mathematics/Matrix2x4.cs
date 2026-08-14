using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	public struct Matrix2x4 : IEquatable<Matrix2x4>, IFormattable
	{
		// Token: 0x060003B4 RID: 948 RVA: 0x0000C9C4 File Offset: 0x0000ABC4
		public Matrix2x4(Vector4 row0, Vector4 row1)
		{
			this.Row0 = row0;
			this.Row1 = row1;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000C9D4 File Offset: 0x0000ABD4
		public Matrix2x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13)
		{
			this.Row0 = new Vector4(m00, m01, m02, m03);
			this.Row1 = new Vector4(m10, m11, m12, m13);
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0000C9F9 File Offset: 0x0000ABF9
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x0000CA16 File Offset: 0x0000AC16
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000CA3A File Offset: 0x0000AC3A
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x0000CA57 File Offset: 0x0000AC57
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003BA RID: 954 RVA: 0x0000CA7B File Offset: 0x0000AC7B
		// (set) Token: 0x060003BB RID: 955 RVA: 0x0000CA98 File Offset: 0x0000AC98
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

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0000CABC File Offset: 0x0000ACBC
		// (set) Token: 0x060003BD RID: 957 RVA: 0x0000CAD9 File Offset: 0x0000ACD9
		public Vector2 Column3
		{
			readonly get
			{
				return new Vector2(this.Row0.W, this.Row1.W);
			}
			set
			{
				this.Row0.W = value.X;
				this.Row1.W = value.Y;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0000CAFD File Offset: 0x0000ACFD
		// (set) Token: 0x060003BF RID: 959 RVA: 0x0000CB0A File Offset: 0x0000AD0A
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

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000CB18 File Offset: 0x0000AD18
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x0000CB25 File Offset: 0x0000AD25
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

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000CB33 File Offset: 0x0000AD33
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x0000CB40 File Offset: 0x0000AD40
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

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0000CB4E File Offset: 0x0000AD4E
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x0000CB5B File Offset: 0x0000AD5B
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

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000CB69 File Offset: 0x0000AD69
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x0000CB76 File Offset: 0x0000AD76
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

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x0000CB84 File Offset: 0x0000AD84
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x0000CB91 File Offset: 0x0000AD91
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

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003CA RID: 970 RVA: 0x0000CB9F File Offset: 0x0000AD9F
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0000CBAC File Offset: 0x0000ADAC
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

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0000CBBA File Offset: 0x0000ADBA
		// (set) Token: 0x060003CD RID: 973 RVA: 0x0000CBC7 File Offset: 0x0000ADC7
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

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000CBD5 File Offset: 0x0000ADD5
		// (set) Token: 0x060003CF RID: 975 RVA: 0x0000CBF2 File Offset: 0x0000ADF2
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

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000CC16 File Offset: 0x0000AE16
		public readonly float Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y;
			}
		}

		// Token: 0x170000F5 RID: 245
		public float this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 2 && columnIndex < 4)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0f;
			}
			set
			{
				if (rowIndex < 2 && columnIndex < 4)
				{
					this.GetRowUnsafe(this, rowIndex)[columnIndex] = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to set this matrix at: ({0}, {1})", rowIndex, columnIndex));
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000CC9D File Offset: 0x0000AE9D
		[MethodImpl(256)]
		private readonly ref Vector4 GetRowUnsafe(in Matrix2x4 m, int index)
		{
			return Unsafe.Add<Vector4>(Unsafe.AsRef<Vector4>(ref m.Row0), index);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000CCB0 File Offset: 0x0000AEB0
		public readonly Matrix4x2 Transposed()
		{
			return Matrix2x4.Transpose(this);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000CCBD File Offset: 0x0000AEBD
		public void Swizzle(int rowForRow0, int rowForRow1)
		{
			this = Matrix2x4.Swizzle(this, rowForRow0, rowForRow1);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000CCD4 File Offset: 0x0000AED4
		public readonly Matrix2x4 Swizzled(int rowForRow0, int rowForRow1)
		{
			Matrix2x4 result = this;
			result.Swizzle(rowForRow0, rowForRow1);
			return result;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000CCF4 File Offset: 0x0000AEF4
		public static void CreateRotation(float angle, out Matrix2x4 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row0.Z = 0f;
			result.Row0.W = 0f;
			result.Row1.X = -num2;
			result.Row1.Y = num;
			result.Row1.Z = 0f;
			result.Row1.W = 0f;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000CD80 File Offset: 0x0000AF80
		public static Matrix2x4 CreateRotation(float angle)
		{
			Matrix2x4 result;
			Matrix2x4.CreateRotation(angle, out result);
			return result;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000CD98 File Offset: 0x0000AF98
		public static void CreateScale(float scale, out Matrix2x4 result)
		{
			result.Row0.X = scale;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row0.W = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = scale;
			result.Row1.Z = 0f;
			result.Row1.W = 0f;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000CE20 File Offset: 0x0000B020
		public static Matrix2x4 CreateScale(float scale)
		{
			Matrix2x4 result;
			Matrix2x4.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000CE38 File Offset: 0x0000B038
		public static void CreateScale(Vector2 scale, out Matrix2x4 result)
		{
			result.Row0.X = scale.X;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row0.W = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = scale.Y;
			result.Row1.Z = 0f;
			result.Row1.W = 0f;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000CEC8 File Offset: 0x0000B0C8
		public static Matrix2x4 CreateScale(Vector2 scale)
		{
			Matrix2x4 result;
			Matrix2x4.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000CEE0 File Offset: 0x0000B0E0
		public static void CreateScale(float x, float y, out Matrix2x4 result)
		{
			result.Row0.X = x;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row0.W = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = y;
			result.Row1.Z = 0f;
			result.Row1.W = 0f;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000CF68 File Offset: 0x0000B168
		public static Matrix2x4 CreateScale(float x, float y)
		{
			Matrix2x4 result;
			Matrix2x4.CreateScale(x, y, out result);
			return result;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000CF80 File Offset: 0x0000B180
		public static void Mult(in Matrix2x4 left, float right, out Matrix2x4 result)
		{
			result.Row0.X = left.Row0.X * right;
			result.Row0.Y = left.Row0.Y * right;
			result.Row0.Z = left.Row0.Z * right;
			result.Row0.W = left.Row0.W * right;
			result.Row1.X = left.Row1.X * right;
			result.Row1.Y = left.Row1.Y * right;
			result.Row1.Z = left.Row1.Z * right;
			result.Row1.W = left.Row1.W * right;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000D050 File Offset: 0x0000B250
		public static Matrix2x4 Mult(Matrix2x4 left, float right)
		{
			Matrix2x4 result;
			Matrix2x4.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000D068 File Offset: 0x0000B268
		public static void Mult(in Matrix2x4 left, in Matrix4x2 right, out Matrix2 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float z = left.Row0.Z;
			float w = left.Row0.W;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float z2 = left.Row1.Z;
			float w2 = left.Row1.W;
			float x3 = right.Row0.X;
			float y3 = right.Row0.Y;
			float x4 = right.Row1.X;
			float y4 = right.Row1.Y;
			float x5 = right.Row2.X;
			float y5 = right.Row2.Y;
			float x6 = right.Row3.X;
			float y6 = right.Row3.Y;
			result.Row0.X = x * x3 + y * x4 + z * x5 + w * x6;
			result.Row0.Y = x * y3 + y * y4 + z * y5 + w * y6;
			result.Row1.X = x2 * x3 + y2 * x4 + z2 * x5 + w2 * x6;
			result.Row1.Y = x2 * y3 + y2 * y4 + z2 * y5 + w2 * y6;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000D1C4 File Offset: 0x0000B3C4
		public static Matrix2 Mult(Matrix2x4 left, Matrix4x2 right)
		{
			Matrix2 result;
			Matrix2x4.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		public static void Mult(in Matrix2x4 left, in Matrix4x3 right, out Matrix2x3 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float z = left.Row0.Z;
			float w = left.Row0.W;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float z2 = left.Row1.Z;
			float w2 = left.Row1.W;
			float x3 = right.Row0.X;
			float y3 = right.Row0.Y;
			float z3 = right.Row0.Z;
			float x4 = right.Row1.X;
			float y4 = right.Row1.Y;
			float z4 = right.Row1.Z;
			float x5 = right.Row2.X;
			float y5 = right.Row2.Y;
			float z5 = right.Row2.Z;
			float x6 = right.Row3.X;
			float y6 = right.Row3.Y;
			float z6 = right.Row3.Z;
			result.Row0.X = x * x3 + y * x4 + z * x5 + w * x6;
			result.Row0.Y = x * y3 + y * y4 + z * y5 + w * y6;
			result.Row0.Z = x * z3 + y * z4 + z * z5 + w * z6;
			result.Row1.X = x2 * x3 + y2 * x4 + z2 * x5 + w2 * x6;
			result.Row1.Y = x2 * y3 + y2 * y4 + z2 * y5 + w2 * y6;
			result.Row1.Z = x2 * z3 + y2 * z4 + z2 * z5 + w2 * z6;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000D3B0 File Offset: 0x0000B5B0
		public static Matrix2x3 Mult(Matrix2x4 left, Matrix4x3 right)
		{
			Matrix2x3 result;
			Matrix2x4.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000D3CC File Offset: 0x0000B5CC
		public static void Mult(in Matrix2x4 left, in Matrix4 right, out Matrix2x4 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float z = left.Row0.Z;
			float w = left.Row0.W;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float z2 = left.Row1.Z;
			float w2 = left.Row1.W;
			float x3 = right.Row0.X;
			float y3 = right.Row0.Y;
			float z3 = right.Row0.Z;
			float w3 = right.Row0.W;
			float x4 = right.Row1.X;
			float y4 = right.Row1.Y;
			float z4 = right.Row1.Z;
			float w4 = right.Row1.W;
			float x5 = right.Row2.X;
			float y5 = right.Row2.Y;
			float z5 = right.Row2.Z;
			float w5 = right.Row2.W;
			float x6 = right.Row3.X;
			float y6 = right.Row3.Y;
			float z6 = right.Row3.Z;
			float w6 = right.Row3.W;
			result.Row0.X = x * x3 + y * x4 + z * x5 + w * x6;
			result.Row0.Y = x * y3 + y * y4 + z * y5 + w * y6;
			result.Row0.Z = x * z3 + y * z4 + z * z5 + w * z6;
			result.Row0.W = x * w3 + y * w4 + z * w5 + w * w6;
			result.Row1.X = x2 * x3 + y2 * x4 + z2 * x5 + w2 * x6;
			result.Row1.Y = x2 * y3 + y2 * y4 + z2 * y5 + w2 * y6;
			result.Row1.Z = x2 * z3 + y2 * z4 + z2 * z5 + w2 * z6;
			result.Row1.W = x2 * w3 + y2 * w4 + z2 * w5 + w2 * w6;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000D610 File Offset: 0x0000B810
		public static Matrix2x4 Mult(Matrix2x4 left, Matrix4 right)
		{
			Matrix2x4 result;
			Matrix2x4.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000D62C File Offset: 0x0000B82C
		public static void Add(in Matrix2x4 left, in Matrix2x4 right, out Matrix2x4 result)
		{
			result.Row0.X = left.Row0.X + right.Row0.X;
			result.Row0.Y = left.Row0.Y + right.Row0.Y;
			result.Row0.Z = left.Row0.Z + right.Row0.Z;
			result.Row0.W = left.Row0.W + right.Row0.W;
			result.Row1.X = left.Row1.X + right.Row1.X;
			result.Row1.Y = left.Row1.Y + right.Row1.Y;
			result.Row1.Z = left.Row1.Z + right.Row1.Z;
			result.Row1.W = left.Row1.W + right.Row1.W;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000D74C File Offset: 0x0000B94C
		public static Matrix2x4 Add(Matrix2x4 left, Matrix2x4 right)
		{
			Matrix2x4 result;
			Matrix2x4.Add(left, right, out result);
			return result;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000D768 File Offset: 0x0000B968
		public static void Subtract(in Matrix2x4 left, in Matrix2x4 right, out Matrix2x4 result)
		{
			result.Row0.X = left.Row0.X - right.Row0.X;
			result.Row0.Y = left.Row0.Y - right.Row0.Y;
			result.Row0.Z = left.Row0.Z - right.Row0.Z;
			result.Row0.W = left.Row0.W - right.Row0.W;
			result.Row1.X = left.Row1.X - right.Row1.X;
			result.Row1.Y = left.Row1.Y - right.Row1.Y;
			result.Row1.Z = left.Row1.Z - right.Row1.Z;
			result.Row1.W = left.Row1.W - right.Row1.W;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000D888 File Offset: 0x0000BA88
		public static Matrix2x4 Subtract(Matrix2x4 left, Matrix2x4 right)
		{
			Matrix2x4 result;
			Matrix2x4.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000D8A4 File Offset: 0x0000BAA4
		public static void Transpose(in Matrix2x4 mat, out Matrix4x2 result)
		{
			result.Row0.X = mat.Row0.X;
			result.Row0.Y = mat.Row1.X;
			result.Row1.X = mat.Row0.Y;
			result.Row1.Y = mat.Row1.Y;
			result.Row2.X = mat.Row0.Z;
			result.Row2.Y = mat.Row1.Z;
			result.Row3.X = mat.Row0.W;
			result.Row3.Y = mat.Row1.W;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000D964 File Offset: 0x0000BB64
		public static Matrix4x2 Transpose(Matrix2x4 mat)
		{
			Matrix4x2 result;
			Matrix2x4.Transpose(mat, out result);
			return result;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000D97C File Offset: 0x0000BB7C
		public static Matrix2x4 Swizzle(Matrix2x4 mat, int rowForRow0, int rowForRow1)
		{
			Matrix2x4 result;
			Matrix2x4.Swizzle(mat, rowForRow0, rowForRow1, out result);
			return result;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000D998 File Offset: 0x0000BB98
		public static void Swizzle(in Matrix2x4 mat, int rowForRow0, int rowForRow1, out Matrix2x4 result)
		{
			Vector4 vector;
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

		// Token: 0x060003EF RID: 1007 RVA: 0x0000DA1F File Offset: 0x0000BC1F
		public static Matrix2x4 operator *(float left, Matrix2x4 right)
		{
			return Matrix2x4.Mult(right, left);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000DA28 File Offset: 0x0000BC28
		public static Matrix2x4 operator *(Matrix2x4 left, float right)
		{
			return Matrix2x4.Mult(left, right);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000DA31 File Offset: 0x0000BC31
		public static Matrix2 operator *(Matrix2x4 left, Matrix4x2 right)
		{
			return Matrix2x4.Mult(left, right);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000DA3A File Offset: 0x0000BC3A
		public static Matrix2x3 operator *(Matrix2x4 left, Matrix4x3 right)
		{
			return Matrix2x4.Mult(left, right);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000DA43 File Offset: 0x0000BC43
		public static Matrix2x4 operator *(Matrix2x4 left, Matrix4 right)
		{
			return Matrix2x4.Mult(left, right);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000DA4C File Offset: 0x0000BC4C
		public static Matrix2x4 operator +(Matrix2x4 left, Matrix2x4 right)
		{
			return Matrix2x4.Add(left, right);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000DA55 File Offset: 0x0000BC55
		public static Matrix2x4 operator -(Matrix2x4 left, Matrix2x4 right)
		{
			return Matrix2x4.Subtract(left, right);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000DA5E File Offset: 0x0000BC5E
		public static bool operator ==(Matrix2x4 left, Matrix2x4 right)
		{
			return left.Equals(right);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000DA68 File Offset: 0x0000BC68
		public static bool operator !=(Matrix2x4 left, Matrix2x4 right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000DA75 File Offset: 0x0000BC75
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000DA7F File Offset: 0x0000BC7F
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000DA89 File Offset: 0x0000BC89
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000DA94 File Offset: 0x0000BC94
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			string text = this.Row0.ToString(format, formatProvider);
			string text2 = this.Row1.ToString(format, formatProvider);
			return text + "\n" + text2;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000DAC7 File Offset: 0x0000BCC7
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector4, Vector4>(this.Row0, this.Row1);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000DADA File Offset: 0x0000BCDA
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix2x4 && this.Equals((Matrix2x4)obj);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000DAF2 File Offset: 0x0000BCF2
		public readonly bool Equals(Matrix2x4 other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1;
		}

		// Token: 0x04000046 RID: 70
		public Vector4 Row0;

		// Token: 0x04000047 RID: 71
		public Vector4 Row1;

		// Token: 0x04000048 RID: 72
		public static readonly Matrix2x4 Zero = new Matrix2x4(Vector4.Zero, Vector4.Zero);
	}
}
