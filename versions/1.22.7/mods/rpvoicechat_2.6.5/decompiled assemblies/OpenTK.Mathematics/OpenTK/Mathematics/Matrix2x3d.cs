using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public struct Matrix2x3d : IEquatable<Matrix2x3d>, IFormattable
	{
		// Token: 0x0600036E RID: 878 RVA: 0x0000BB14 File Offset: 0x00009D14
		public Matrix2x3d(Vector3d row0, Vector3d row1)
		{
			this.Row0 = row0;
			this.Row1 = row1;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000BB24 File Offset: 0x00009D24
		public Matrix2x3d(double m00, double m01, double m02, double m10, double m11, double m12)
		{
			this.Row0 = new Vector3d(m00, m01, m02);
			this.Row1 = new Vector3d(m10, m11, m12);
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000BB45 File Offset: 0x00009D45
		// (set) Token: 0x06000371 RID: 881 RVA: 0x0000BB62 File Offset: 0x00009D62
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

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000BB86 File Offset: 0x00009D86
		// (set) Token: 0x06000373 RID: 883 RVA: 0x0000BBA3 File Offset: 0x00009DA3
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

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000BBC7 File Offset: 0x00009DC7
		// (set) Token: 0x06000375 RID: 885 RVA: 0x0000BBE4 File Offset: 0x00009DE4
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

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0000BC08 File Offset: 0x00009E08
		// (set) Token: 0x06000377 RID: 887 RVA: 0x0000BC15 File Offset: 0x00009E15
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

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0000BC23 File Offset: 0x00009E23
		// (set) Token: 0x06000379 RID: 889 RVA: 0x0000BC30 File Offset: 0x00009E30
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

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000BC3E File Offset: 0x00009E3E
		// (set) Token: 0x0600037B RID: 891 RVA: 0x0000BC4B File Offset: 0x00009E4B
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000BC59 File Offset: 0x00009E59
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0000BC66 File Offset: 0x00009E66
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

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0000BC74 File Offset: 0x00009E74
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0000BC81 File Offset: 0x00009E81
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000BC8F File Offset: 0x00009E8F
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000BC9C File Offset: 0x00009E9C
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000BCAA File Offset: 0x00009EAA
		// (set) Token: 0x06000383 RID: 899 RVA: 0x0000BCC7 File Offset: 0x00009EC7
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

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0000BCEB File Offset: 0x00009EEB
		public readonly double Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y;
			}
		}

		// Token: 0x170000E6 RID: 230
		public double this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 2 && columnIndex < 3)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0.0;
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

		// Token: 0x06000387 RID: 903 RVA: 0x0000BD76 File Offset: 0x00009F76
		[MethodImpl(256)]
		private readonly ref Vector3d GetRowUnsafe(in Matrix2x3d m, int index)
		{
			return Unsafe.Add<Vector3d>(Unsafe.AsRef<Vector3d>(ref m.Row0), index);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000BD89 File Offset: 0x00009F89
		public readonly Matrix3x2d Transposed()
		{
			return Matrix2x3d.Transpose(this);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000BD96 File Offset: 0x00009F96
		public void Swizzle(int rowForRow0, int rowForRow1)
		{
			this = Matrix2x3d.Swizzle(this, rowForRow0, rowForRow1);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000BDAC File Offset: 0x00009FAC
		public readonly Matrix2x3d Swizzled(int rowForRow0, int rowForRow1)
		{
			Matrix2x3d result = this;
			result.Swizzle(rowForRow0, rowForRow1);
			return result;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000BDCC File Offset: 0x00009FCC
		public static void CreateRotation(double angle, out Matrix2x3d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row0.Z = 0.0;
			result.Row1.X = -num2;
			result.Row1.Y = num;
			result.Row1.Z = 0.0;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000BE40 File Offset: 0x0000A040
		public static Matrix2x3d CreateRotation(double angle)
		{
			Matrix2x3d result;
			Matrix2x3d.CreateRotation(angle, out result);
			return result;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000BE58 File Offset: 0x0000A058
		public static void CreateScale(double scale, out Matrix2x3d result)
		{
			result.Row0.X = scale;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = scale;
			result.Row1.Z = 0.0;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000BED0 File Offset: 0x0000A0D0
		public static Matrix2x3d CreateScale(double scale)
		{
			Matrix2x3d result;
			Matrix2x3d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
		public static void CreateScale(Vector2d scale, out Matrix2x3d result)
		{
			result.Row0.X = scale.X;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = scale.Y;
			result.Row1.Z = 0.0;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000BF68 File Offset: 0x0000A168
		public static Matrix2x3d CreateScale(Vector2d scale)
		{
			Matrix2x3d result;
			Matrix2x3d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000BF80 File Offset: 0x0000A180
		public static void CreateScale(double x, double y, out Matrix2x3d result)
		{
			result.Row0.X = x;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = y;
			result.Row1.Z = 0.0;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000BFF8 File Offset: 0x0000A1F8
		public static Matrix2x3d CreateScale(double x, double y)
		{
			Matrix2x3d result;
			Matrix2x3d.CreateScale(x, y, out result);
			return result;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000C010 File Offset: 0x0000A210
		public static void Mult(in Matrix2x3d left, double right, out Matrix2x3d result)
		{
			result.Row0.X = left.Row0.X * right;
			result.Row0.Y = left.Row0.Y * right;
			result.Row0.Z = left.Row0.Z * right;
			result.Row1.X = left.Row1.X * right;
			result.Row1.Y = left.Row1.Y * right;
			result.Row1.Z = left.Row1.Z * right;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000C0B0 File Offset: 0x0000A2B0
		public static Matrix2x3d Mult(Matrix2x3d left, double right)
		{
			Matrix2x3d result;
			Matrix2x3d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000C0C8 File Offset: 0x0000A2C8
		public static void Mult(in Matrix2x3d left, in Matrix3x2 right, out Matrix2d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
			double num = (double)right.Row0.X;
			double num2 = (double)right.Row0.Y;
			double num3 = (double)right.Row1.X;
			double num4 = (double)right.Row1.Y;
			double num5 = (double)right.Row2.X;
			double num6 = (double)right.Row2.Y;
			result.Row0.X = x * num + y * num3 + z * num5;
			result.Row0.Y = x * num2 + y * num4 + z * num6;
			result.Row1.X = x2 * num + y2 * num3 + z2 * num5;
			result.Row1.Y = x2 * num2 + y2 * num4 + z2 * num6;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000C1DC File Offset: 0x0000A3DC
		public static Matrix2d Mult(Matrix2x3d left, Matrix3x2 right)
		{
			Matrix2d result;
			Matrix2x3d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
		public static void Mult(in Matrix2x3d left, in Matrix3 right, out Matrix2x3d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
			double num = (double)right.Row0.X;
			double num2 = (double)right.Row0.Y;
			double num3 = (double)right.Row0.Z;
			double num4 = (double)right.Row1.X;
			double num5 = (double)right.Row1.Y;
			double num6 = (double)right.Row1.Z;
			double num7 = (double)right.Row2.X;
			double num8 = (double)right.Row2.Y;
			double num9 = (double)right.Row2.Z;
			result.Row0.X = x * num + y * num4 + z * num7;
			result.Row0.Y = x * num2 + y * num5 + z * num8;
			result.Row0.Z = x * num3 + y * num6 + z * num9;
			result.Row1.X = x2 * num + y2 * num4 + z2 * num7;
			result.Row1.Y = x2 * num2 + y2 * num5 + z2 * num8;
			result.Row1.Z = x2 * num3 + y2 * num6 + z2 * num9;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000C36C File Offset: 0x0000A56C
		public static Matrix2x3d Mult(Matrix2x3d left, Matrix3 right)
		{
			Matrix2x3d result;
			Matrix2x3d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000C388 File Offset: 0x0000A588
		public static void Mult(in Matrix2x3d left, in Matrix3x4 right, out Matrix2x4d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
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
			result.Row0.X = x * num + y * num5 + z * num9;
			result.Row0.Y = x * num2 + y * num6 + z * num10;
			result.Row0.Z = x * num3 + y * num7 + z * num11;
			result.Row0.W = x * num4 + y * num8 + z * num12;
			result.Row1.X = x2 * num + y2 * num5 + z2 * num9;
			result.Row1.Y = x2 * num2 + y2 * num6 + z2 * num10;
			result.Row1.Z = x2 * num3 + y2 * num7 + z2 * num11;
			result.Row1.W = x2 * num4 + y2 * num8 + z2 * num12;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000C558 File Offset: 0x0000A758
		public static Matrix2x4d Mult(Matrix2x3d left, Matrix3x4 right)
		{
			Matrix2x4d result;
			Matrix2x3d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000C574 File Offset: 0x0000A774
		public static void Add(in Matrix2x3d left, in Matrix2x3d right, out Matrix2x3d result)
		{
			result.Row0.X = left.Row0.X + right.Row0.X;
			result.Row0.Y = left.Row0.Y + right.Row0.Y;
			result.Row0.Z = left.Row0.Z + right.Row0.Z;
			result.Row1.X = left.Row1.X + right.Row1.X;
			result.Row1.Y = left.Row1.Y + right.Row1.Y;
			result.Row1.Z = left.Row1.Z + right.Row1.Z;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000C650 File Offset: 0x0000A850
		public static Matrix2x3d Add(Matrix2x3d left, Matrix2x3d right)
		{
			Matrix2x3d result;
			Matrix2x3d.Add(left, right, out result);
			return result;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000C66C File Offset: 0x0000A86C
		public static void Subtract(in Matrix2x3d left, in Matrix2x3d right, out Matrix2x3d result)
		{
			result.Row0.X = left.Row0.X - right.Row0.X;
			result.Row0.Y = left.Row0.Y - right.Row0.Y;
			result.Row0.Z = left.Row0.Z - right.Row0.Z;
			result.Row1.X = left.Row1.X - right.Row1.X;
			result.Row1.Y = left.Row1.Y - right.Row1.Y;
			result.Row1.Z = left.Row1.Z - right.Row1.Z;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000C748 File Offset: 0x0000A948
		public static Matrix2x3d Subtract(Matrix2x3d left, Matrix2x3d right)
		{
			Matrix2x3d result;
			Matrix2x3d.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000C764 File Offset: 0x0000A964
		public static void Transpose(in Matrix2x3d mat, out Matrix3x2d result)
		{
			result.Row0.X = mat.Row0.X;
			result.Row0.Y = mat.Row1.X;
			result.Row1.X = mat.Row0.Y;
			result.Row1.Y = mat.Row1.Y;
			result.Row2.X = mat.Row0.Z;
			result.Row2.Y = mat.Row1.Z;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000C7F8 File Offset: 0x0000A9F8
		public static Matrix3x2d Transpose(Matrix2x3d mat)
		{
			Matrix3x2d result;
			Matrix2x3d.Transpose(mat, out result);
			return result;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000C810 File Offset: 0x0000AA10
		public static Matrix2x3d Swizzle(Matrix2x3d mat, int rowForRow0, int rowForRow1)
		{
			Matrix2x3d result;
			Matrix2x3d.Swizzle(mat, rowForRow0, rowForRow1, out result);
			return result;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000C82C File Offset: 0x0000AA2C
		public static void Swizzle(in Matrix2x3d mat, int rowForRow0, int rowForRow1, out Matrix2x3d result)
		{
			Vector3d vector3d;
			if (rowForRow0 != 0)
			{
				if (rowForRow0 != 1)
				{
					throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow0", rowForRow0));
				}
				vector3d = mat.Row1;
			}
			else
			{
				vector3d = mat.Row0;
			}
			result.Row0 = vector3d;
			if (rowForRow1 != 0)
			{
				if (rowForRow1 != 1)
				{
					throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow1", rowForRow1));
				}
				vector3d = mat.Row1;
			}
			else
			{
				vector3d = mat.Row0;
			}
			result.Row1 = vector3d;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000C8B3 File Offset: 0x0000AAB3
		public static Matrix2x3d operator *(double left, Matrix2x3d right)
		{
			return Matrix2x3d.Mult(right, left);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000C8BC File Offset: 0x0000AABC
		public static Matrix2x3d operator *(Matrix2x3d left, double right)
		{
			return Matrix2x3d.Mult(left, right);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000C8C5 File Offset: 0x0000AAC5
		public static Matrix2d operator *(Matrix2x3d left, Matrix3x2 right)
		{
			return Matrix2x3d.Mult(left, right);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000C8CE File Offset: 0x0000AACE
		public static Matrix2x3d operator *(Matrix2x3d left, Matrix3 right)
		{
			return Matrix2x3d.Mult(left, right);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000C8D7 File Offset: 0x0000AAD7
		public static Matrix2x4d operator *(Matrix2x3d left, Matrix3x4 right)
		{
			return Matrix2x3d.Mult(left, right);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
		public static Matrix2x3d operator +(Matrix2x3d left, Matrix2x3d right)
		{
			return Matrix2x3d.Add(left, right);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000C8E9 File Offset: 0x0000AAE9
		public static Matrix2x3d operator -(Matrix2x3d left, Matrix2x3d right)
		{
			return Matrix2x3d.Subtract(left, right);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000C8F2 File Offset: 0x0000AAF2
		public static bool operator ==(Matrix2x3d left, Matrix2x3d right)
		{
			return left.Equals(right);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000C8FC File Offset: 0x0000AAFC
		public static bool operator !=(Matrix2x3d left, Matrix2x3d right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000C909 File Offset: 0x0000AB09
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000C913 File Offset: 0x0000AB13
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000C91D File Offset: 0x0000AB1D
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000C928 File Offset: 0x0000AB28
		public string ToString(string format, IFormatProvider formatProvider)
		{
			string text = this.Row0.ToString(format, formatProvider);
			string text2 = this.Row1.ToString(format, formatProvider);
			return text + "\n" + text2;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000C95B File Offset: 0x0000AB5B
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3d, Vector3d>(this.Row0, this.Row1);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000C96E File Offset: 0x0000AB6E
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix2x3d && this.Equals((Matrix2x3d)obj);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000C986 File Offset: 0x0000AB86
		public readonly bool Equals(Matrix2x3d other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1;
		}

		// Token: 0x04000043 RID: 67
		public Vector3d Row0;

		// Token: 0x04000044 RID: 68
		public Vector3d Row1;

		// Token: 0x04000045 RID: 69
		public static readonly Matrix2x3d Zero = new Matrix2x3d(Vector3d.Zero, Vector3d.Zero);
	}
}
