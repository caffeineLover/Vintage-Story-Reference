using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000021 RID: 33
	[Serializable]
	public struct Matrix4x3d : IEquatable<Matrix4x3d>, IFormattable
	{
		// Token: 0x06000837 RID: 2103 RVA: 0x00020881 File Offset: 0x0001EA81
		public Matrix4x3d(Vector3d row0, Vector3d row1, Vector3d row2, Vector3d row3)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
			this.Row3 = row3;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x000208A0 File Offset: 0x0001EAA0
		public Matrix4x3d(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22, double m30, double m31, double m32)
		{
			this.Row0 = new Vector3d(m00, m01, m02);
			this.Row1 = new Vector3d(m10, m11, m12);
			this.Row2 = new Vector3d(m20, m21, m22);
			this.Row3 = new Vector3d(m30, m31, m32);
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x000208EE File Offset: 0x0001EAEE
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x00020924 File Offset: 0x0001EB24
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

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x00020975 File Offset: 0x0001EB75
		// (set) Token: 0x0600083C RID: 2108 RVA: 0x000209A8 File Offset: 0x0001EBA8
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

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x000209F9 File Offset: 0x0001EBF9
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x00020A2C File Offset: 0x0001EC2C
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

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x00020A7D File Offset: 0x0001EC7D
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x00020A8A File Offset: 0x0001EC8A
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

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x00020A98 File Offset: 0x0001EC98
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x00020AA5 File Offset: 0x0001ECA5
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

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x00020AB3 File Offset: 0x0001ECB3
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x00020AC0 File Offset: 0x0001ECC0
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

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x00020ACE File Offset: 0x0001ECCE
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x00020ADB File Offset: 0x0001ECDB
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

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x00020AE9 File Offset: 0x0001ECE9
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x00020AF6 File Offset: 0x0001ECF6
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

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x00020B04 File Offset: 0x0001ED04
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x00020B11 File Offset: 0x0001ED11
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

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x00020B1F File Offset: 0x0001ED1F
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x00020B2C File Offset: 0x0001ED2C
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

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x00020B3A File Offset: 0x0001ED3A
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x00020B47 File Offset: 0x0001ED47
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

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x00020B55 File Offset: 0x0001ED55
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x00020B62 File Offset: 0x0001ED62
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

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x00020B70 File Offset: 0x0001ED70
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x00020B7D File Offset: 0x0001ED7D
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

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x00020B8B File Offset: 0x0001ED8B
		// (set) Token: 0x06000854 RID: 2132 RVA: 0x00020B98 File Offset: 0x0001ED98
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

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x00020BA6 File Offset: 0x0001EDA6
		// (set) Token: 0x06000856 RID: 2134 RVA: 0x00020BB3 File Offset: 0x0001EDB3
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

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x00020BC1 File Offset: 0x0001EDC1
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x00020BE9 File Offset: 0x0001EDE9
		public Vector3d Diagonal
		{
			readonly get
			{
				return new Vector3d(this.Row0.X, this.Row1.Y, this.Row2.Z);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.Y = value.Y;
				this.Row2.Z = value.Z;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x00020C1E File Offset: 0x0001EE1E
		public readonly double Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y + this.Row2.Z;
			}
		}

		// Token: 0x170001CE RID: 462
		public double this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 4 && columnIndex < 3)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0.0;
			}
			set
			{
				if (rowIndex < 4 && columnIndex < 3)
				{
					this.GetRowUnsafe(this, rowIndex)[columnIndex] = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to set this matrix at: ({0}, {1})", rowIndex, columnIndex));
			}
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00020CB5 File Offset: 0x0001EEB5
		[MethodImpl(256)]
		private readonly ref Vector3d GetRowUnsafe(in Matrix4x3d m, int index)
		{
			return Unsafe.Add<Vector3d>(Unsafe.AsRef<Vector3d>(ref m.Row0), index);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00020CC8 File Offset: 0x0001EEC8
		public void Invert()
		{
			this = Matrix4x3d.Invert(this);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00020CDC File Offset: 0x0001EEDC
		public readonly Matrix4x3d Inverted()
		{
			Matrix4x3d result = this;
			result.Invert();
			return result;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00020CF8 File Offset: 0x0001EEF8
		public readonly Matrix3x4d Transposed()
		{
			return Matrix4x3d.Transpose(this);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00020D05 File Offset: 0x0001EF05
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			this = Matrix4x3d.Swizzle(this, rowForRow0, rowForRow1, rowForRow2, rowForRow3);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00020D20 File Offset: 0x0001EF20
		public readonly Matrix4x3d Swizzled(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4x3d result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2, rowForRow3);
			return result;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00020D44 File Offset: 0x0001EF44
		public static void CreateFromAxisAngle(Vector3d axis, double angle, out Matrix4x3d result)
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
			result.Row1.X = num5 + num12;
			result.Row1.Y = num7 + num;
			result.Row1.Z = num8 - num10;
			result.Row2.X = num6 - num11;
			result.Row2.Y = num8 + num10;
			result.Row2.Z = num9 + num;
			result.Row3.X = 0.0;
			result.Row3.Y = 0.0;
			result.Row3.Z = 0.0;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00020E88 File Offset: 0x0001F088
		public static Matrix4x3d CreateFromAxisAngle(Vector3d axis, double angle)
		{
			Matrix4x3d result;
			Matrix4x3d.CreateFromAxisAngle(axis, angle, out result);
			return result;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00020EA0 File Offset: 0x0001F0A0
		public static void CreateFromQuaternion(in Quaternion q, out Matrix4x3d result)
		{
			double num = (double)(q.X * q.X);
			double num2 = (double)(q.Y * q.Y);
			double num3 = (double)(q.Z * q.Z);
			double num4 = (double)(q.W * q.W);
			double num5 = (double)(q.X * q.Y);
			double num6 = (double)(q.X * q.Z);
			double num7 = (double)(q.X * q.W);
			double num8 = (double)(q.Y * q.Z);
			double num9 = (double)(q.Y * q.W);
			double num10 = (double)(q.Z * q.W);
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
			result.Row3 = new Vector3d(0.0, 0.0, 0.0);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x00021048 File Offset: 0x0001F248
		public static Matrix4x3d CreateFromQuaternion(Quaternion q)
		{
			Matrix4x3d result;
			Matrix4x3d.CreateFromQuaternion(q, out result);
			return result;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00021060 File Offset: 0x0001F260
		public static void CreateRotationX(double angle, out Matrix4x3d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0.X = 1.0;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = num;
			result.Row1.Z = num2;
			result.Row2.X = 0.0;
			result.Row2.Y = -num2;
			result.Row2.Z = num;
			result.Row3.X = 0.0;
			result.Row3.Y = 0.0;
			result.Row3.Z = 0.0;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0002114C File Offset: 0x0001F34C
		public static Matrix4x3d CreateRotationX(double angle)
		{
			Matrix4x3d result;
			Matrix4x3d.CreateRotationX(angle, out result);
			return result;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00021164 File Offset: 0x0001F364
		public static void CreateRotationY(double angle, out Matrix4x3d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = 0.0;
			result.Row0.Z = -num2;
			result.Row1.X = 0.0;
			result.Row1.Y = 1.0;
			result.Row1.Z = 0.0;
			result.Row2.X = num2;
			result.Row2.Y = 0.0;
			result.Row2.Z = num;
			result.Row3.X = 0.0;
			result.Row3.Y = 0.0;
			result.Row3.Z = 0.0;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00021250 File Offset: 0x0001F450
		public static Matrix4x3d CreateRotationY(double angle)
		{
			Matrix4x3d result;
			Matrix4x3d.CreateRotationY(angle, out result);
			return result;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00021268 File Offset: 0x0001F468
		public static void CreateRotationZ(double angle, out Matrix4x3d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row0.Z = 0.0;
			result.Row1.X = -num2;
			result.Row1.Y = num;
			result.Row1.Z = 0.0;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row2.Z = 1.0;
			result.Row3.X = 0.0;
			result.Row3.Y = 0.0;
			result.Row3.Z = 0.0;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00021354 File Offset: 0x0001F554
		public static Matrix4x3d CreateRotationZ(double angle)
		{
			Matrix4x3d result;
			Matrix4x3d.CreateRotationZ(angle, out result);
			return result;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0002136C File Offset: 0x0001F56C
		public static void CreateTranslation(double x, double y, double z, out Matrix4x3d result)
		{
			result.Row0.X = 1.0;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = 1.0;
			result.Row1.Z = 0.0;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row2.Z = 1.0;
			result.Row3.X = x;
			result.Row3.Y = y;
			result.Row3.Z = z;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00021454 File Offset: 0x0001F654
		public static void CreateTranslation(in Vector3d vector, out Matrix4x3d result)
		{
			result.Row0.X = 1.0;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = 1.0;
			result.Row1.Z = 0.0;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row2.Z = 1.0;
			result.Row3.X = vector.X;
			result.Row3.Y = vector.Y;
			result.Row3.Z = vector.Z;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00021548 File Offset: 0x0001F748
		public static Matrix4x3d CreateTranslation(double x, double y, double z)
		{
			Matrix4x3d result;
			Matrix4x3d.CreateTranslation(x, y, z, out result);
			return result;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00021560 File Offset: 0x0001F760
		public static Matrix4x3d CreateTranslation(Vector3d vector)
		{
			Matrix4x3d result;
			Matrix4x3d.CreateTranslation(vector.X, vector.Y, vector.Z, out result);
			return result;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00021587 File Offset: 0x0001F787
		public static Matrix4x3d CreateScale(double scale)
		{
			return Matrix4x3d.CreateScale(scale, scale, scale);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00021591 File Offset: 0x0001F791
		public static Matrix4x3d CreateScale(Vector3d scale)
		{
			return Matrix4x3d.CreateScale(scale.X, scale.Y, scale.Z);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x000215AC File Offset: 0x0001F7AC
		public static Matrix4x3d CreateScale(double x, double y, double z)
		{
			Matrix4x3d result;
			result.Row0.X = x;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = y;
			result.Row1.Z = 0.0;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row2.Z = z;
			result.Row3.X = 0.0;
			result.Row3.Y = 0.0;
			result.Row3.Z = 0.0;
			return result;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000216A0 File Offset: 0x0001F8A0
		public static Matrix4d Mult(Matrix4x3d left, Matrix3x4d right)
		{
			Matrix4d result;
			Matrix4x3d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x000216BC File Offset: 0x0001F8BC
		public static void Mult(in Matrix4x3d left, in Matrix3x4d right, out Matrix4d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double z3 = left.Row2.Z;
			double x4 = left.Row3.X;
			double y4 = left.Row3.Y;
			double z4 = left.Row3.Z;
			double x5 = right.Row0.X;
			double y5 = right.Row0.Y;
			double z5 = right.Row0.Z;
			double w = right.Row0.W;
			double x6 = right.Row1.X;
			double y6 = right.Row1.Y;
			double z6 = right.Row1.Z;
			double w2 = right.Row1.W;
			double x7 = right.Row2.X;
			double y7 = right.Row2.Y;
			double z7 = right.Row2.Z;
			double w3 = right.Row2.W;
			result.Row0.X = x * x5 + y * x6 + z * x7;
			result.Row0.Y = x * y5 + y * y6 + z * y7;
			result.Row0.Z = x * z5 + y * z6 + z * z7;
			result.Row0.W = x * w + y * w2 + z * w3;
			result.Row1.X = x2 * x5 + y2 * x6 + z2 * x7;
			result.Row1.Y = x2 * y5 + y2 * y6 + z2 * y7;
			result.Row1.Z = x2 * z5 + y2 * z6 + z2 * z7;
			result.Row1.W = x2 * w + y2 * w2 + z2 * w3;
			result.Row2.X = x3 * x5 + y3 * x6 + z3 * x7;
			result.Row2.Y = x3 * y5 + y3 * y6 + z3 * y7;
			result.Row2.Z = x3 * z5 + y3 * z6 + z3 * z7;
			result.Row2.W = x3 * w + y3 * w2 + z3 * w3;
			result.Row3.X = x4 * x5 + y4 * x6 + z4 * x7;
			result.Row3.Y = x4 * y5 + y4 * y6 + z4 * y7;
			result.Row3.Z = x4 * z5 + y4 * z6 + z4 * z7;
			result.Row3.W = x4 * w + y4 * w2 + z4 * w3;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000219B0 File Offset: 0x0001FBB0
		public static Matrix4x3d Mult(Matrix4x3d left, Matrix4x3d right)
		{
			Matrix4x3d result;
			Matrix4x3d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000219CC File Offset: 0x0001FBCC
		public static void Mult(in Matrix4x3d left, in Matrix4x3d right, out Matrix4x3d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double z3 = left.Row2.Z;
			double x4 = left.Row3.X;
			double y4 = left.Row3.Y;
			double z4 = left.Row3.Z;
			double x5 = right.Row0.X;
			double y5 = right.Row0.Y;
			double z5 = right.Row0.Z;
			double x6 = right.Row1.X;
			double y6 = right.Row1.Y;
			double z6 = right.Row1.Z;
			double x7 = right.Row2.X;
			double y7 = right.Row2.Y;
			double z7 = right.Row2.Z;
			double x8 = right.Row3.X;
			double y8 = right.Row3.Y;
			double z8 = right.Row3.Z;
			result.Row0.X = x * x5 + y * x6 + z * x7 + x8;
			result.Row0.Y = x * y5 + y * y6 + z * y7 + y8;
			result.Row0.Z = x * z5 + y * z6 + z * z7 + z8;
			result.Row1.X = x2 * x5 + y2 * x6 + z2 * x7 + x8;
			result.Row1.Y = x2 * y5 + y2 * y6 + z2 * y7 + y8;
			result.Row1.Z = x2 * z5 + y2 * z6 + z2 * z7 + z8;
			result.Row2.X = x3 * x5 + y3 * x6 + z3 * x7 + x8;
			result.Row2.Y = x3 * y5 + y3 * y6 + z3 * y7 + y8;
			result.Row2.Z = x3 * z5 + y3 * z6 + z3 * z7 + z8;
			result.Row3.X = x4 * x5 + y4 * x6 + z4 * x7 + x8;
			result.Row3.Y = x4 * y5 + y4 * y6 + z4 * y7 + y8;
			result.Row3.Z = x4 * z5 + y4 * z6 + z4 * z7 + z8;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00021C78 File Offset: 0x0001FE78
		public static Matrix4x3d Mult(Matrix4x3d left, double right)
		{
			Matrix4x3d result;
			Matrix4x3d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00021C90 File Offset: 0x0001FE90
		public static void Mult(in Matrix4x3d left, double right, out Matrix4x3d result)
		{
			result.Row0 = left.Row0 * right;
			result.Row1 = left.Row1 * right;
			result.Row2 = left.Row2 * right;
			result.Row3 = left.Row3 * right;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00021CE8 File Offset: 0x0001FEE8
		public static Matrix4x3d Add(Matrix4x3d left, Matrix4x3d right)
		{
			Matrix4x3d result;
			Matrix4x3d.Add(left, right, out result);
			return result;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00021D04 File Offset: 0x0001FF04
		public static void Add(in Matrix4x3d left, in Matrix4x3d right, out Matrix4x3d result)
		{
			result.Row0 = left.Row0 + right.Row0;
			result.Row1 = left.Row1 + right.Row1;
			result.Row2 = left.Row2 + right.Row2;
			result.Row3 = left.Row3 + right.Row3;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00021D70 File Offset: 0x0001FF70
		public static Matrix4x3d Subtract(Matrix4x3d left, Matrix4x3d right)
		{
			Matrix4x3d result;
			Matrix4x3d.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00021D8C File Offset: 0x0001FF8C
		public static void Subtract(in Matrix4x3d left, in Matrix4x3d right, out Matrix4x3d result)
		{
			result.Row0 = left.Row0 - right.Row0;
			result.Row1 = left.Row1 - right.Row1;
			result.Row2 = left.Row2 - right.Row2;
			result.Row3 = left.Row3 - right.Row3;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00021DF8 File Offset: 0x0001FFF8
		public static Matrix4x3d Invert(Matrix4x3d mat)
		{
			Matrix4x3d result;
			Matrix4x3d.Invert(mat, out result);
			return result;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00021E10 File Offset: 0x00020010
		public static void Invert(in Matrix4x3d mat, out Matrix4x3d result)
		{
			Matrix3d matrix3d = new Matrix3d(mat.Column0.Xyz, mat.Column1.Xyz, mat.Column2.Xyz);
			matrix3d.Row0 /= matrix3d.Row0.LengthSquared;
			matrix3d.Row1 /= matrix3d.Row1.LengthSquared;
			matrix3d.Row2 /= matrix3d.Row2.LengthSquared;
			Vector3d row = mat.Row3;
			result.Row0 = matrix3d.Row0;
			result.Row1 = matrix3d.Row1;
			result.Row2 = matrix3d.Row2;
			result.Row3 = new Vector3d(-Vector3d.Dot(matrix3d.Row0, row), -Vector3d.Dot(matrix3d.Row1, row), -Vector3d.Dot(matrix3d.Row2, row));
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00021F14 File Offset: 0x00020114
		public static Matrix3x4d Transpose(Matrix4x3d mat)
		{
			return new Matrix3x4d(mat.Column0, mat.Column1, mat.Column2);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00021F30 File Offset: 0x00020130
		public static void Transpose(in Matrix4x3d mat, out Matrix3x4d result)
		{
			result.Row0 = mat.Column0;
			result.Row1 = mat.Column1;
			result.Row2 = mat.Column2;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00021F58 File Offset: 0x00020158
		public static Matrix4x3d Swizzle(Matrix4x3d mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4x3d result;
			Matrix4x3d.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, rowForRow3, out result);
			return result;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00021F74 File Offset: 0x00020174
		public static void Swizzle(in Matrix4x3d mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3, out Matrix4x3d result)
		{
			Vector3d vector3d;
			switch (rowForRow0)
			{
			case 0:
				vector3d = mat.Row0;
				break;
			case 1:
				vector3d = mat.Row1;
				break;
			case 2:
				vector3d = mat.Row2;
				break;
			case 3:
				vector3d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow0", rowForRow0));
			}
			result.Row0 = vector3d;
			switch (rowForRow1)
			{
			case 0:
				vector3d = mat.Row0;
				break;
			case 1:
				vector3d = mat.Row1;
				break;
			case 2:
				vector3d = mat.Row2;
				break;
			case 3:
				vector3d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow1", rowForRow1));
			}
			result.Row1 = vector3d;
			switch (rowForRow2)
			{
			case 0:
				vector3d = mat.Row0;
				break;
			case 1:
				vector3d = mat.Row1;
				break;
			case 2:
				vector3d = mat.Row2;
				break;
			case 3:
				vector3d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 3. Got {1}.", "rowForRow2", rowForRow2));
			}
			result.Row2 = vector3d;
			switch (rowForRow3)
			{
			case 0:
				vector3d = mat.Row0;
				break;
			case 1:
				vector3d = mat.Row1;
				break;
			case 2:
				vector3d = mat.Row2;
				break;
			case 3:
				vector3d = mat.Row3;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow3", rowForRow3));
			}
			result.Row3 = vector3d;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x000220FF File Offset: 0x000202FF
		public static Matrix4d operator *(Matrix4x3d left, Matrix3x4d right)
		{
			return Matrix4x3d.Mult(left, right);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00022108 File Offset: 0x00020308
		public static Matrix4x3d operator *(Matrix4x3d left, Matrix4x3d right)
		{
			return Matrix4x3d.Mult(left, right);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00022111 File Offset: 0x00020311
		public static Matrix4x3d operator *(Matrix4x3d left, double right)
		{
			return Matrix4x3d.Mult(left, right);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0002211A File Offset: 0x0002031A
		public static Matrix4x3d operator +(Matrix4x3d left, Matrix4x3d right)
		{
			return Matrix4x3d.Add(left, right);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00022123 File Offset: 0x00020323
		public static Matrix4x3d operator -(Matrix4x3d left, Matrix4x3d right)
		{
			return Matrix4x3d.Subtract(left, right);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0002212C File Offset: 0x0002032C
		public static bool operator ==(Matrix4x3d left, Matrix4x3d right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00022136 File Offset: 0x00020336
		public static bool operator !=(Matrix4x3d left, Matrix4x3d right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00022143 File Offset: 0x00020343
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0002214D File Offset: 0x0002034D
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00022157 File Offset: 0x00020357
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00022164 File Offset: 0x00020364
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

		// Token: 0x0600088E RID: 2190 RVA: 0x000221DC File Offset: 0x000203DC
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3d, Vector3d, Vector3d, Vector3d>(this.Row0, this.Row1, this.Row2, this.Row3);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x000221FB File Offset: 0x000203FB
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix4x3d && this.Equals((Matrix4x3d)obj);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00022214 File Offset: 0x00020414
		public readonly bool Equals(Matrix4x3d other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2 && this.Row3 == other.Row3;
		}

		// Token: 0x04000081 RID: 129
		public Vector3d Row0;

		// Token: 0x04000082 RID: 130
		public Vector3d Row1;

		// Token: 0x04000083 RID: 131
		public Vector3d Row2;

		// Token: 0x04000084 RID: 132
		public Vector3d Row3;

		// Token: 0x04000085 RID: 133
		public static readonly Matrix4x3d Zero = new Matrix4x3d(Vector3d.Zero, Vector3d.Zero, Vector3d.Zero, Vector3d.Zero);
	}
}
