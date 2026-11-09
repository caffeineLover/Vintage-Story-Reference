using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public struct Matrix3d : IEquatable<Matrix3d>, IFormattable
	{
		// Token: 0x060004A1 RID: 1185 RVA: 0x0001044A File Offset: 0x0000E64A
		public Matrix3d(Vector3d row0, Vector3d row1, Vector3d row2)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00010461 File Offset: 0x0000E661
		public Matrix3d(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22)
		{
			this.Row0 = new Vector3d(m00, m01, m02);
			this.Row1 = new Vector3d(m10, m11, m12);
			this.Row2 = new Vector3d(m20, m21, m22);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00010493 File Offset: 0x0000E693
		public Matrix3d(Matrix4d matrix)
		{
			this.Row0 = matrix.Row0.Xyz;
			this.Row1 = matrix.Row1.Xyz;
			this.Row2 = matrix.Row2.Xyz;
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x000104CC File Offset: 0x0000E6CC
		public readonly double Determinant
		{
			get
			{
				double x = this.Row0.X;
				double y = this.Row0.Y;
				double z = this.Row0.Z;
				double x2 = this.Row1.X;
				double y2 = this.Row1.Y;
				double z2 = this.Row1.Z;
				double x3 = this.Row2.X;
				double y3 = this.Row2.Y;
				double z3 = this.Row2.Z;
				return x * y2 * z3 + y * z2 * x3 + z * x2 * y3 - z * y2 * x3 - x * z2 * y3 - y * x2 * z3;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00010577 File Offset: 0x0000E777
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x0001059F File Offset: 0x0000E79F
		public Vector3d Column0
		{
			readonly get
			{
				return new Vector3d(this.Row0.X, this.Row1.X, this.Row2.X);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.X = value.Y;
				this.Row2.X = value.Z;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x000105D4 File Offset: 0x0000E7D4
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x000105FC File Offset: 0x0000E7FC
		public Vector3d Column1
		{
			readonly get
			{
				return new Vector3d(this.Row0.Y, this.Row1.Y, this.Row2.Y);
			}
			set
			{
				this.Row0.Y = value.X;
				this.Row1.Y = value.Y;
				this.Row2.Y = value.Z;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00010631 File Offset: 0x0000E831
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x00010659 File Offset: 0x0000E859
		public Vector3d Column2
		{
			readonly get
			{
				return new Vector3d(this.Row0.Z, this.Row1.Z, this.Row2.Z);
			}
			set
			{
				this.Row0.Z = value.X;
				this.Row1.Z = value.Y;
				this.Row2.Z = value.Z;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001068E File Offset: 0x0000E88E
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x0001069B File Offset: 0x0000E89B
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

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x000106A9 File Offset: 0x0000E8A9
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x000106B6 File Offset: 0x0000E8B6
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

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x000106C4 File Offset: 0x0000E8C4
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x000106D1 File Offset: 0x0000E8D1
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

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x000106DF File Offset: 0x0000E8DF
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x000106EC File Offset: 0x0000E8EC
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

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x000106FA File Offset: 0x0000E8FA
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x00010707 File Offset: 0x0000E907
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

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00010715 File Offset: 0x0000E915
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00010722 File Offset: 0x0000E922
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

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00010730 File Offset: 0x0000E930
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x0001073D File Offset: 0x0000E93D
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

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0001074B File Offset: 0x0000E94B
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x00010758 File Offset: 0x0000E958
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

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00010766 File Offset: 0x0000E966
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x00010773 File Offset: 0x0000E973
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

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00010781 File Offset: 0x0000E981
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x000107A9 File Offset: 0x0000E9A9
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

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x000107DE File Offset: 0x0000E9DE
		public readonly double Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y + this.Row2.Z;
			}
		}

		// Token: 0x17000124 RID: 292
		public double this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 3 && columnIndex < 3)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0.0;
			}
			set
			{
				if (rowIndex < 3 && columnIndex < 3)
				{
					this.GetRowUnsafe(this, rowIndex)[columnIndex] = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to set this matrix at: ({0}, {1})", rowIndex, columnIndex));
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00010875 File Offset: 0x0000EA75
		[MethodImpl(256)]
		private readonly ref Vector3d GetRowUnsafe(in Matrix3d m, int index)
		{
			return Unsafe.Add<Vector3d>(Unsafe.AsRef<Vector3d>(ref m.Row0), index);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00010888 File Offset: 0x0000EA88
		public void Invert()
		{
			this = Matrix3d.Invert(this);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001089C File Offset: 0x0000EA9C
		public readonly Matrix3d Inverted()
		{
			Matrix3d result = this;
			if (result.Determinant != 0.0)
			{
				result.Invert();
			}
			return result;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000108CA File Offset: 0x0000EACA
		public void Transpose()
		{
			this = Matrix3d.Transpose(this);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000108E0 File Offset: 0x0000EAE0
		public readonly Matrix3d Transposed()
		{
			Matrix3d result = this;
			result.Transpose();
			return result;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000108FC File Offset: 0x0000EAFC
		public void Normalize()
		{
			double determinant = this.Determinant;
			this.Row0 /= determinant;
			this.Row1 /= determinant;
			this.Row2 /= determinant;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00010948 File Offset: 0x0000EB48
		public readonly Matrix3d Normalized()
		{
			Matrix3d result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00010964 File Offset: 0x0000EB64
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			this = Matrix3d.Swizzle(this, rowForRow0, rowForRow1, rowForRow2);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001097C File Offset: 0x0000EB7C
		public readonly Matrix3d Swizzled(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3d result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2);
			return result;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001099C File Offset: 0x0000EB9C
		public readonly Matrix3d ClearScale()
		{
			Matrix3d result = this;
			result.Row0 = result.Row0.Normalized();
			result.Row1 = result.Row1.Normalized();
			result.Row2 = result.Row2.Normalized();
			return result;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000109EC File Offset: 0x0000EBEC
		public readonly Matrix3d ClearRotation()
		{
			Matrix3d result = this;
			result.Row0 = new Vector3d(result.Row0.Length, 0.0, 0.0);
			result.Row1 = new Vector3d(0.0, result.Row1.Length, 0.0);
			result.Row2 = new Vector3d(0.0, 0.0, result.Row2.Length);
			return result;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00010A7F File Offset: 0x0000EC7F
		public readonly Vector3d ExtractScale()
		{
			return new Vector3d(this.Row0.Length, this.Row1.Length, this.Row2.Length);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00010AA8 File Offset: 0x0000ECA8
		public readonly Quaterniond ExtractRotation(bool rowNormalize = true)
		{
			Vector3d vector3d = this.Row0;
			Vector3d vector3d2 = this.Row1;
			Vector3d vector3d3 = this.Row2;
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

		// Token: 0x060004CF RID: 1231 RVA: 0x00010DF4 File Offset: 0x0000EFF4
		public static void CreateFromAxisAngle(Vector3d axis, double angle, out Matrix3d result)
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
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00010EFC File Offset: 0x0000F0FC
		public static Matrix3d CreateFromAxisAngle(Vector3d axis, double angle)
		{
			Matrix3d result;
			Matrix3d.CreateFromAxisAngle(axis, angle, out result);
			return result;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00010F14 File Offset: 0x0000F114
		public static void CreateFromQuaternion(in Quaterniond q, out Matrix3d result)
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
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001108C File Offset: 0x0000F28C
		public static Matrix3d CreateFromQuaternion(Quaterniond q)
		{
			Matrix3d result;
			Matrix3d.CreateFromQuaternion(q, out result);
			return result;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x000110A4 File Offset: 0x0000F2A4
		public static void CreateRotationX(double angle, out Matrix3d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result = Matrix3d.Identity;
			result.Row1.Y = num;
			result.Row1.Z = num2;
			result.Row2.Y = -num2;
			result.Row2.Z = num;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000110FC File Offset: 0x0000F2FC
		public static Matrix3d CreateRotationX(double angle)
		{
			Matrix3d result;
			Matrix3d.CreateRotationX(angle, out result);
			return result;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00011114 File Offset: 0x0000F314
		public static void CreateRotationY(double angle, out Matrix3d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result = Matrix3d.Identity;
			result.Row0.X = num;
			result.Row0.Z = -num2;
			result.Row2.X = num2;
			result.Row2.Z = num;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001116C File Offset: 0x0000F36C
		public static Matrix3d CreateRotationY(double angle)
		{
			Matrix3d result;
			Matrix3d.CreateRotationY(angle, out result);
			return result;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00011184 File Offset: 0x0000F384
		public static void CreateRotationZ(double angle, out Matrix3d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result = Matrix3d.Identity;
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row1.X = -num2;
			result.Row1.Y = num;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x000111DC File Offset: 0x0000F3DC
		public static Matrix3d CreateRotationZ(double angle)
		{
			Matrix3d result;
			Matrix3d.CreateRotationZ(angle, out result);
			return result;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000111F4 File Offset: 0x0000F3F4
		public static Matrix3d CreateScale(double scale)
		{
			Matrix3d result;
			Matrix3d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001120C File Offset: 0x0000F40C
		public static Matrix3d CreateScale(Vector3d scale)
		{
			Matrix3d result;
			Matrix3d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00011224 File Offset: 0x0000F424
		public static Matrix3d CreateScale(double x, double y, double z)
		{
			Matrix3d result;
			Matrix3d.CreateScale(x, y, z, out result);
			return result;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001123C File Offset: 0x0000F43C
		public static void CreateScale(double scale, out Matrix3d result)
		{
			result = Matrix3d.Identity;
			result.Row0.X = scale;
			result.Row1.Y = scale;
			result.Row2.Z = scale;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001126D File Offset: 0x0000F46D
		public static void CreateScale(in Vector3d scale, out Matrix3d result)
		{
			result = Matrix3d.Identity;
			result.Row0.X = scale.X;
			result.Row1.Y = scale.Y;
			result.Row2.Z = scale.Z;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x000112AD File Offset: 0x0000F4AD
		public static void CreateScale(double x, double y, double z, out Matrix3d result)
		{
			result = Matrix3d.Identity;
			result.Row0.X = x;
			result.Row1.Y = y;
			result.Row2.Z = z;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000112E0 File Offset: 0x0000F4E0
		public static Matrix3d CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3d result;
			Matrix3d.CreateSwizzle(rowForRow0, rowForRow1, rowForRow2, out result);
			return result;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000112F8 File Offset: 0x0000F4F8
		public static void CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2, out Matrix3d result)
		{
			if (rowForRow0 < 0 || rowForRow0 >= 3)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow0", rowForRow0));
			}
			if (rowForRow1 < 0 || rowForRow1 >= 3)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow1", rowForRow1));
			}
			if (rowForRow2 < 0 || rowForRow2 >= 3)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow2", rowForRow2));
			}
			result = Matrix3d.Zero;
			result[0, rowForRow0] = 1.0;
			result[1, rowForRow1] = 1.0;
			result[2, rowForRow2] = 1.0;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000113AC File Offset: 0x0000F5AC
		public static Matrix3d Add(Matrix3d left, Matrix3d right)
		{
			Matrix3d result;
			Matrix3d.Add(left, right, out result);
			return result;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x000113C8 File Offset: 0x0000F5C8
		public static void Add(in Matrix3d left, in Matrix3d right, out Matrix3d result)
		{
			Vector3d.Add(left.Row0, right.Row0, out result.Row0);
			Vector3d.Add(left.Row1, right.Row1, out result.Row1);
			Vector3d.Add(left.Row2, right.Row2, out result.Row2);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001141C File Offset: 0x0000F61C
		public static Matrix3d Mult(Matrix3d left, Matrix3d right)
		{
			Matrix3d result;
			Matrix3d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00011438 File Offset: 0x0000F638
		public static void Mult(in Matrix3d left, in Matrix3d right, out Matrix3d result)
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
			double x4 = right.Row0.X;
			double y4 = right.Row0.Y;
			double z4 = right.Row0.Z;
			double x5 = right.Row1.X;
			double y5 = right.Row1.Y;
			double z5 = right.Row1.Z;
			double x6 = right.Row2.X;
			double y6 = right.Row2.Y;
			double z6 = right.Row2.Z;
			result.Row0.X = x * x4 + y * x5 + z * x6;
			result.Row0.Y = x * y4 + y * y5 + z * y6;
			result.Row0.Z = x * z4 + y * z5 + z * z6;
			result.Row1.X = x2 * x4 + y2 * x5 + z2 * x6;
			result.Row1.Y = x2 * y4 + y2 * y5 + z2 * y6;
			result.Row1.Z = x2 * z4 + y2 * z5 + z2 * z6;
			result.Row2.X = x3 * x4 + y3 * x5 + z3 * x6;
			result.Row2.Y = x3 * y4 + y3 * y5 + z3 * y6;
			result.Row2.Z = x3 * z4 + y3 * z5 + z3 * z6;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001161C File Offset: 0x0000F81C
		public static void Invert(in Matrix3d matrix, out Matrix3d result)
		{
			double x = matrix.Row0.X;
			double y = matrix.Row0.Y;
			double z = matrix.Row0.Z;
			double x2 = matrix.Row1.X;
			double y2 = matrix.Row1.Y;
			double z2 = matrix.Row1.Z;
			double x3 = matrix.Row2.X;
			double y3 = matrix.Row2.Y;
			double z3 = matrix.Row2.Z;
			double num = y2 * z3 - z2 * y3;
			double num2 = -x2 * z3 + z2 * x3;
			double num3 = x2 * y3 - y2 * x3;
			double num4 = x * num + y * num2 + z * num3;
			if (num4 == 0.0)
			{
				throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
			}
			result.Row0.X = num;
			result.Row0.Y = -y * z3 + z * y3;
			result.Row0.Z = y * z2 - z * y2;
			result.Row1.X = num2;
			result.Row1.Y = x * z3 - z * x3;
			result.Row1.Z = -x * z2 + z * x2;
			result.Row2.X = num3;
			result.Row2.Y = -x * y3 + y * x3;
			result.Row2.Z = x * y2 - y * x2;
			num4 = 1.0 / num4;
			result.Row0.X = result.Row0.X * num4;
			result.Row0.Y = result.Row0.Y * num4;
			result.Row0.Z = result.Row0.Z * num4;
			result.Row1.X = result.Row1.X * num4;
			result.Row1.Y = result.Row1.Y * num4;
			result.Row1.Z = result.Row1.Z * num4;
			result.Row2.X = result.Row2.X * num4;
			result.Row2.Y = result.Row2.Y * num4;
			result.Row2.Z = result.Row2.Z * num4;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00011830 File Offset: 0x0000FA30
		public static Matrix3d Invert(Matrix3d mat)
		{
			Matrix3d result;
			Matrix3d.Invert(mat, out result);
			return result;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00011847 File Offset: 0x0000FA47
		public static Matrix3d Transpose(Matrix3d mat)
		{
			return new Matrix3d(mat.Column0, mat.Column1, mat.Column2);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00011863 File Offset: 0x0000FA63
		public static void Transpose(in Matrix3d mat, out Matrix3d result)
		{
			result.Row0 = mat.Column0;
			result.Row1 = mat.Column1;
			result.Row2 = mat.Column2;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001188C File Offset: 0x0000FA8C
		public static Matrix3d Swizzle(Matrix3d mat, int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3d result;
			Matrix3d.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, out result);
			return result;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000118A8 File Offset: 0x0000FAA8
		public static void Swizzle(in Matrix3d mat, int rowForRow0, int rowForRow1, int rowForRow2, out Matrix3d result)
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
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow0", rowForRow0));
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
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow1", rowForRow1));
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
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow2", rowForRow2));
			}
			result.Row2 = vector3d;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000119AB File Offset: 0x0000FBAB
		public static Matrix3d operator *(Matrix3d left, Matrix3d right)
		{
			return Matrix3d.Mult(left, right);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000119B4 File Offset: 0x0000FBB4
		public static bool operator ==(Matrix3d left, Matrix3d right)
		{
			return left.Equals(right);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000119BE File Offset: 0x0000FBBE
		public static bool operator !=(Matrix3d left, Matrix3d right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000119CB File Offset: 0x0000FBCB
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x000119D5 File Offset: 0x0000FBD5
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x000119DF File Offset: 0x0000FBDF
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x000119EC File Offset: 0x0000FBEC
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			string text = this.Row0.ToString(format, formatProvider);
			string text2 = this.Row1.ToString(format, formatProvider);
			string text3 = this.Row2.ToString(format, formatProvider);
			return string.Concat(new string[]
			{
				text,
				"\n",
				text2,
				"\n",
				text3
			});
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00011A4A File Offset: 0x0000FC4A
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3d, Vector3d, Vector3d>(this.Row0, this.Row1, this.Row2);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00011A63 File Offset: 0x0000FC63
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix3d && this.Equals((Matrix3d)obj);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00011A7B File Offset: 0x0000FC7B
		public readonly bool Equals(Matrix3d other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2;
		}

		// Token: 0x04000051 RID: 81
		public Vector3d Row0;

		// Token: 0x04000052 RID: 82
		public Vector3d Row1;

		// Token: 0x04000053 RID: 83
		public Vector3d Row2;

		// Token: 0x04000054 RID: 84
		public static readonly Matrix3d Identity = new Matrix3d(Vector3d.UnitX, Vector3d.UnitY, Vector3d.UnitZ);

		// Token: 0x04000055 RID: 85
		public static readonly Matrix3d Zero = new Matrix3d(Vector3d.Zero, Vector3d.Zero, Vector3d.Zero);
	}
}
