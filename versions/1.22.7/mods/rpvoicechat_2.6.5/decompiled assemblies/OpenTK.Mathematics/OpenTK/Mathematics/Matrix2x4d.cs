using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public struct Matrix2x4d : IEquatable<Matrix2x4d>, IFormattable
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x0000DB30 File Offset: 0x0000BD30
		public Matrix2x4d(Vector4d row0, Vector4d row1)
		{
			this.Row0 = row0;
			this.Row1 = row1;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000DB40 File Offset: 0x0000BD40
		public Matrix2x4d(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13)
		{
			this.Row0 = new Vector4d(m00, m01, m02, m03);
			this.Row1 = new Vector4d(m10, m11, m12, m13);
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0000DB65 File Offset: 0x0000BD65
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x0000DB82 File Offset: 0x0000BD82
		public Vector2d Column0
		{
			readonly get
			{
				return new Vector2d(this.Row0.X, this.Row1.X);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.X = value.Y;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000DBA6 File Offset: 0x0000BDA6
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x0000DBC3 File Offset: 0x0000BDC3
		public Vector2d Column1
		{
			readonly get
			{
				return new Vector2d(this.Row0.Y, this.Row1.Y);
			}
			set
			{
				this.Row0.Y = value.X;
				this.Row1.Y = value.Y;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000DBE7 File Offset: 0x0000BDE7
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x0000DC04 File Offset: 0x0000BE04
		public Vector2d Column2
		{
			readonly get
			{
				return new Vector2d(this.Row0.Z, this.Row1.Z);
			}
			set
			{
				this.Row0.Z = value.X;
				this.Row1.Z = value.Y;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0000DC28 File Offset: 0x0000BE28
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x0000DC45 File Offset: 0x0000BE45
		public Vector2d Column3
		{
			readonly get
			{
				return new Vector2d(this.Row0.W, this.Row1.W);
			}
			set
			{
				this.Row0.W = value.X;
				this.Row1.W = value.Y;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0000DC69 File Offset: 0x0000BE69
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x0000DC76 File Offset: 0x0000BE76
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

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0000DC84 File Offset: 0x0000BE84
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x0000DC91 File Offset: 0x0000BE91
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

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000DC9F File Offset: 0x0000BE9F
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x0000DCAC File Offset: 0x0000BEAC
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

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0000DCBA File Offset: 0x0000BEBA
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x0000DCC7 File Offset: 0x0000BEC7
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

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0000DCD5 File Offset: 0x0000BED5
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x0000DCE2 File Offset: 0x0000BEE2
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

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000DCF0 File Offset: 0x0000BEF0
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x0000DCFD File Offset: 0x0000BEFD
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

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0000DD0B File Offset: 0x0000BF0B
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x0000DD18 File Offset: 0x0000BF18
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

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0000DD26 File Offset: 0x0000BF26
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x0000DD33 File Offset: 0x0000BF33
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

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0000DD41 File Offset: 0x0000BF41
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x0000DD5E File Offset: 0x0000BF5E
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

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0000DD82 File Offset: 0x0000BF82
		public readonly double Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y;
			}
		}

		// Token: 0x17000104 RID: 260
		public double this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 2 && columnIndex < 4)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0.0;
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

		// Token: 0x0600041F RID: 1055 RVA: 0x0000DE0D File Offset: 0x0000C00D
		[MethodImpl(256)]
		private readonly ref Vector4d GetRowUnsafe(in Matrix2x4d m, int index)
		{
			return Unsafe.Add<Vector4d>(Unsafe.AsRef<Vector4d>(ref m.Row0), index);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000DE20 File Offset: 0x0000C020
		public readonly Matrix4x2d Transposed()
		{
			return Matrix2x4d.Transpose(this);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000DE2D File Offset: 0x0000C02D
		public void Swizzle(int rowForRow0, int rowForRow1)
		{
			this = Matrix2x4d.Swizzle(this, rowForRow0, rowForRow1);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000DE44 File Offset: 0x0000C044
		public readonly Matrix2x4d Swizzled(int rowForRow0, int rowForRow1)
		{
			Matrix2x4d result = this;
			result.Swizzle(rowForRow0, rowForRow1);
			return result;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000DE64 File Offset: 0x0000C064
		public static void CreateRotation(double angle, out Matrix2x4d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row0.Z = 0.0;
			result.Row0.W = 0.0;
			result.Row1.X = -num2;
			result.Row1.Y = num;
			result.Row1.Z = 0.0;
			result.Row1.W = 0.0;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000DF00 File Offset: 0x0000C100
		public static Matrix2x4d CreateRotation(double angle)
		{
			Matrix2x4d result;
			Matrix2x4d.CreateRotation(angle, out result);
			return result;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000DF18 File Offset: 0x0000C118
		public static void CreateScale(double scale, out Matrix2x4d result)
		{
			result.Row0.X = scale;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row0.W = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = scale;
			result.Row1.Z = 0.0;
			result.Row1.W = 0.0;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000DFB8 File Offset: 0x0000C1B8
		public static Matrix2x4d CreateScale(double scale)
		{
			Matrix2x4d result;
			Matrix2x4d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000DFD0 File Offset: 0x0000C1D0
		public static void CreateScale(Vector2d scale, out Matrix2x4d result)
		{
			result.Row0.X = scale.X;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row0.W = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = scale.Y;
			result.Row1.Z = 0.0;
			result.Row1.W = 0.0;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000E078 File Offset: 0x0000C278
		public static Matrix2x4d CreateScale(Vector2d scale)
		{
			Matrix2x4d result;
			Matrix2x4d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000E090 File Offset: 0x0000C290
		public static void CreateScale(double x, double y, out Matrix2x4d result)
		{
			result.Row0.X = x;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row0.W = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = y;
			result.Row1.Z = 0.0;
			result.Row1.W = 0.0;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000E130 File Offset: 0x0000C330
		public static Matrix2x4d CreateScale(double x, double y)
		{
			Matrix2x4d result;
			Matrix2x4d.CreateScale(x, y, out result);
			return result;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000E148 File Offset: 0x0000C348
		public static void Mult(in Matrix2x4d left, double right, out Matrix2x4d result)
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

		// Token: 0x0600042C RID: 1068 RVA: 0x0000E218 File Offset: 0x0000C418
		public static Matrix2x4d Mult(Matrix2x4d left, double right)
		{
			Matrix2x4d result;
			Matrix2x4d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000E230 File Offset: 0x0000C430
		public static void Mult(in Matrix2x4d left, in Matrix4x2 right, out Matrix2d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double w = left.Row0.W;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
			double w2 = left.Row1.W;
			double num = (double)right.Row0.X;
			double num2 = (double)right.Row0.Y;
			double num3 = (double)right.Row1.X;
			double num4 = (double)right.Row1.Y;
			double num5 = (double)right.Row2.X;
			double num6 = (double)right.Row2.Y;
			double num7 = (double)right.Row3.X;
			double num8 = (double)right.Row3.Y;
			result.Row0.X = x * num + y * num3 + z * num5 + w * num7;
			result.Row0.Y = x * num2 + y * num4 + z * num6 + w * num8;
			result.Row1.X = x2 * num + y2 * num3 + z2 * num5 + w2 * num7;
			result.Row1.Y = x2 * num2 + y2 * num4 + z2 * num6 + w2 * num8;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000E394 File Offset: 0x0000C594
		public static Matrix2d Mult(Matrix2x4d left, Matrix4x2 right)
		{
			Matrix2d result;
			Matrix2x4d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
		public static void Mult(in Matrix2x4d left, in Matrix4x3 right, out Matrix2x3d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double w = left.Row0.W;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
			double w2 = left.Row1.W;
			double num = (double)right.Row0.X;
			double num2 = (double)right.Row0.Y;
			double num3 = (double)right.Row0.Z;
			double num4 = (double)right.Row1.X;
			double num5 = (double)right.Row1.Y;
			double num6 = (double)right.Row1.Z;
			double num7 = (double)right.Row2.X;
			double num8 = (double)right.Row2.Y;
			double num9 = (double)right.Row2.Z;
			double num10 = (double)right.Row3.X;
			double num11 = (double)right.Row3.Y;
			double num12 = (double)right.Row3.Z;
			result.Row0.X = x * num + y * num4 + z * num7 + w * num10;
			result.Row0.Y = x * num2 + y * num5 + z * num8 + w * num11;
			result.Row0.Z = x * num3 + y * num6 + z * num9 + w * num12;
			result.Row1.X = x2 * num + y2 * num4 + z2 * num7 + w2 * num10;
			result.Row1.Y = x2 * num2 + y2 * num5 + z2 * num8 + w2 * num11;
			result.Row1.Z = x2 * num3 + y2 * num6 + z2 * num9 + w2 * num12;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000E58C File Offset: 0x0000C78C
		public static Matrix2x3d Mult(Matrix2x4d left, Matrix4x3 right)
		{
			Matrix2x3d result;
			Matrix2x4d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000E5A8 File Offset: 0x0000C7A8
		public static void Mult(in Matrix2x4d left, in Matrix4 right, out Matrix2x4d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double w = left.Row0.W;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
			double w2 = left.Row1.W;
			double num = (double)right.Row0.X;
			double num2 = (double)right.Row0.Y;
			double num3 = (double)right.Row0.Z;
			double num4 = (double)right.Row0.W;
			double num5 = (double)right.Row1.X;
			double num6 = (double)right.Row1.Y;
			double num7 = (double)right.Row1.Z;
			double num8 = (double)right.Row1.W;
			double num9 = (double)right.Row2.X;
			double num10 = (double)right.Row2.Y;
			double num11 = (double)right.Row2.Z;
			double num12 = (double)right.Row2.W;
			double num13 = (double)right.Row3.X;
			double num14 = (double)right.Row3.Y;
			double num15 = (double)right.Row3.Z;
			double num16 = (double)right.Row3.W;
			result.Row0.X = x * num + y * num5 + z * num9 + w * num13;
			result.Row0.Y = x * num2 + y * num6 + z * num10 + w * num14;
			result.Row0.Z = x * num3 + y * num7 + z * num11 + w * num15;
			result.Row0.W = x * num4 + y * num8 + z * num12 + w * num16;
			result.Row1.X = x2 * num + y2 * num5 + z2 * num9 + w2 * num13;
			result.Row1.Y = x2 * num2 + y2 * num6 + z2 * num10 + w2 * num14;
			result.Row1.Z = x2 * num3 + y2 * num7 + z2 * num11 + w2 * num15;
			result.Row1.W = x2 * num4 + y2 * num8 + z2 * num12 + w2 * num16;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000E7FC File Offset: 0x0000C9FC
		public static Matrix2x4d Mult(Matrix2x4d left, Matrix4 right)
		{
			Matrix2x4d result;
			Matrix2x4d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000E818 File Offset: 0x0000CA18
		public static void Add(in Matrix2x4d left, in Matrix2x4d right, out Matrix2x4d result)
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

		// Token: 0x06000434 RID: 1076 RVA: 0x0000E938 File Offset: 0x0000CB38
		public static Matrix2x4d Add(Matrix2x4d left, Matrix2x4d right)
		{
			Matrix2x4d result;
			Matrix2x4d.Add(left, right, out result);
			return result;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000E954 File Offset: 0x0000CB54
		public static void Subtract(in Matrix2x4d left, in Matrix2x4d right, out Matrix2x4d result)
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

		// Token: 0x06000436 RID: 1078 RVA: 0x0000EA74 File Offset: 0x0000CC74
		public static Matrix2x4d Subtract(Matrix2x4d left, Matrix2x4d right)
		{
			Matrix2x4d result;
			Matrix2x4d.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000EA90 File Offset: 0x0000CC90
		public static void Transpose(in Matrix2x4d mat, out Matrix4x2d result)
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

		// Token: 0x06000438 RID: 1080 RVA: 0x0000EB50 File Offset: 0x0000CD50
		public static Matrix4x2d Transpose(Matrix2x4d mat)
		{
			Matrix4x2d result;
			Matrix2x4d.Transpose(mat, out result);
			return result;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000EB68 File Offset: 0x0000CD68
		public static Matrix2x4d Swizzle(Matrix2x4d mat, int rowForRow0, int rowForRow1)
		{
			Matrix2x4d result;
			Matrix2x4d.Swizzle(mat, rowForRow0, rowForRow1, out result);
			return result;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000EB84 File Offset: 0x0000CD84
		public static void Swizzle(in Matrix2x4d mat, int rowForRow0, int rowForRow1, out Matrix2x4d result)
		{
			Vector4d vector4d;
			if (rowForRow0 != 0)
			{
				if (rowForRow0 != 1)
				{
					throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow0", rowForRow0));
				}
				vector4d = mat.Row1;
			}
			else
			{
				vector4d = mat.Row0;
			}
			result.Row0 = vector4d;
			if (rowForRow1 != 0)
			{
				if (rowForRow1 != 1)
				{
					throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow1", rowForRow1));
				}
				vector4d = mat.Row1;
			}
			else
			{
				vector4d = mat.Row0;
			}
			result.Row1 = vector4d;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000EC0B File Offset: 0x0000CE0B
		public static Matrix2x4d operator *(double left, Matrix2x4d right)
		{
			return Matrix2x4d.Mult(right, left);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000EC14 File Offset: 0x0000CE14
		public static Matrix2x4d operator *(Matrix2x4d left, double right)
		{
			return Matrix2x4d.Mult(left, right);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000EC1D File Offset: 0x0000CE1D
		public static Matrix2d operator *(Matrix2x4d left, Matrix4x2 right)
		{
			return Matrix2x4d.Mult(left, right);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000EC26 File Offset: 0x0000CE26
		public static Matrix2x3d operator *(Matrix2x4d left, Matrix4x3 right)
		{
			return Matrix2x4d.Mult(left, right);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000EC2F File Offset: 0x0000CE2F
		public static Matrix2x4d operator *(Matrix2x4d left, Matrix4 right)
		{
			return Matrix2x4d.Mult(left, right);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000EC38 File Offset: 0x0000CE38
		public static Matrix2x4d operator +(Matrix2x4d left, Matrix2x4d right)
		{
			return Matrix2x4d.Add(left, right);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000EC41 File Offset: 0x0000CE41
		public static Matrix2x4d operator -(Matrix2x4d left, Matrix2x4d right)
		{
			return Matrix2x4d.Subtract(left, right);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000EC4A File Offset: 0x0000CE4A
		public static bool operator ==(Matrix2x4d left, Matrix2x4d right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000EC54 File Offset: 0x0000CE54
		public static bool operator !=(Matrix2x4d left, Matrix2x4d right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000EC61 File Offset: 0x0000CE61
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000EC6B File Offset: 0x0000CE6B
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000EC75 File Offset: 0x0000CE75
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000EC80 File Offset: 0x0000CE80
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			string text = this.Row0.ToString(format, formatProvider);
			string text2 = this.Row1.ToString(format, formatProvider);
			return text + "\n" + text2;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000ECB3 File Offset: 0x0000CEB3
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector4d, Vector4d>(this.Row0, this.Row1);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000ECC6 File Offset: 0x0000CEC6
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix2x4d && this.Equals((Matrix2x4d)obj);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000ECDE File Offset: 0x0000CEDE
		public readonly bool Equals(Matrix2x4d other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1;
		}

		// Token: 0x04000049 RID: 73
		public Vector4d Row0;

		// Token: 0x0400004A RID: 74
		public Vector4d Row1;

		// Token: 0x0400004B RID: 75
		public static readonly Matrix2x4d Zero = new Matrix2x4d(Vector4d.Zero, Vector4d.Zero);
	}
}
