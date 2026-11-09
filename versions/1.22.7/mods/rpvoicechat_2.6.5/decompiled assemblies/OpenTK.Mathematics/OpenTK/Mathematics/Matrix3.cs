using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public struct Matrix3 : IEquatable<Matrix3>, IFormattable
	{
		// Token: 0x0600044C RID: 1100 RVA: 0x0000ED1C File Offset: 0x0000CF1C
		public Matrix3(Vector3 row0, Vector3 row1, Vector3 row2)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000ED33 File Offset: 0x0000CF33
		public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
		{
			this.Row0 = new Vector3(m00, m01, m02);
			this.Row1 = new Vector3(m10, m11, m12);
			this.Row2 = new Vector3(m20, m21, m22);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000ED65 File Offset: 0x0000CF65
		public Matrix3(Matrix4 matrix)
		{
			this.Row0 = matrix.Row0.Xyz;
			this.Row1 = matrix.Row1.Xyz;
			this.Row2 = matrix.Row2.Xyz;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0000EDA0 File Offset: 0x0000CFA0
		public readonly float Determinant
		{
			get
			{
				float x = this.Row0.X;
				float y = this.Row0.Y;
				float z = this.Row0.Z;
				float x2 = this.Row1.X;
				float y2 = this.Row1.Y;
				float z2 = this.Row1.Z;
				float x3 = this.Row2.X;
				float y3 = this.Row2.Y;
				float z3 = this.Row2.Z;
				return x * y2 * z3 + y * z2 * x3 + z * x2 * y3 - z * y2 * x3 - x * z2 * y3 - y * x2 * z3;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000EE4B File Offset: 0x0000D04B
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x0000EE73 File Offset: 0x0000D073
		public Vector3 Column0
		{
			readonly get
			{
				return new Vector3(this.Row0.X, this.Row1.X, this.Row2.X);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.X = value.Y;
				this.Row2.X = value.Z;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0000EEA8 File Offset: 0x0000D0A8
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x0000EED0 File Offset: 0x0000D0D0
		public Vector3 Column1
		{
			readonly get
			{
				return new Vector3(this.Row0.Y, this.Row1.Y, this.Row2.Y);
			}
			set
			{
				this.Row0.Y = value.X;
				this.Row1.Y = value.Y;
				this.Row2.Y = value.Z;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0000EF05 File Offset: 0x0000D105
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x0000EF2D File Offset: 0x0000D12D
		public Vector3 Column2
		{
			readonly get
			{
				return new Vector3(this.Row0.Z, this.Row1.Z, this.Row2.Z);
			}
			set
			{
				this.Row0.Z = value.X;
				this.Row1.Z = value.Y;
				this.Row2.Z = value.Z;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0000EF62 File Offset: 0x0000D162
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x0000EF6F File Offset: 0x0000D16F
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

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0000EF7D File Offset: 0x0000D17D
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x0000EF8A File Offset: 0x0000D18A
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

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0000EF98 File Offset: 0x0000D198
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x0000EFA5 File Offset: 0x0000D1A5
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

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x0000EFB3 File Offset: 0x0000D1B3
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x0000EFC0 File Offset: 0x0000D1C0
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

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0000EFCE File Offset: 0x0000D1CE
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x0000EFDB File Offset: 0x0000D1DB
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

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x0000EFE9 File Offset: 0x0000D1E9
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x0000EFF6 File Offset: 0x0000D1F6
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

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x0000F004 File Offset: 0x0000D204
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x0000F011 File Offset: 0x0000D211
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

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0000F01F File Offset: 0x0000D21F
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x0000F02C File Offset: 0x0000D22C
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

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0000F03A File Offset: 0x0000D23A
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x0000F047 File Offset: 0x0000D247
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

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0000F055 File Offset: 0x0000D255
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x0000F07D File Offset: 0x0000D27D
		public Vector3 Diagonal
		{
			readonly get
			{
				return new Vector3(this.Row0.X, this.Row1.Y, this.Row2.Z);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.Y = value.Y;
				this.Row2.Z = value.Z;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0000F0B2 File Offset: 0x0000D2B2
		public readonly float Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y + this.Row2.Z;
			}
		}

		// Token: 0x17000114 RID: 276
		public float this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 3 && columnIndex < 3)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0f;
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

		// Token: 0x0600046D RID: 1133 RVA: 0x0000F145 File Offset: 0x0000D345
		[MethodImpl(256)]
		private readonly ref Vector3 GetRowUnsafe(in Matrix3 m, int index)
		{
			return Unsafe.Add<Vector3>(Unsafe.AsRef<Vector3>(ref m.Row0), index);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000F158 File Offset: 0x0000D358
		public void Invert()
		{
			this = Matrix3.Invert(this);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000F16C File Offset: 0x0000D36C
		public readonly Matrix3 Inverted()
		{
			Matrix3 result = this;
			if (result.Determinant != 0f)
			{
				result.Invert();
			}
			return result;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000F196 File Offset: 0x0000D396
		public void Transpose()
		{
			this = Matrix3.Transpose(this);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000F1AC File Offset: 0x0000D3AC
		public readonly Matrix3 Transposed()
		{
			Matrix3 result = this;
			result.Transpose();
			return result;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000F1C8 File Offset: 0x0000D3C8
		public void Normalize()
		{
			float determinant = this.Determinant;
			this.Row0 /= determinant;
			this.Row1 /= determinant;
			this.Row2 /= determinant;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000F214 File Offset: 0x0000D414
		public readonly Matrix3 Normalized()
		{
			Matrix3 result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000F230 File Offset: 0x0000D430
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			this = Matrix3.Swizzle(this, rowForRow0, rowForRow1, rowForRow2);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000F248 File Offset: 0x0000D448
		public readonly Matrix3 Swizzled(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3 result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2);
			return result;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000F268 File Offset: 0x0000D468
		public readonly Matrix3 ClearScale()
		{
			Matrix3 result = this;
			result.Row0 = result.Row0.Normalized();
			result.Row1 = result.Row1.Normalized();
			result.Row2 = result.Row2.Normalized();
			return result;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000F2B8 File Offset: 0x0000D4B8
		public readonly Matrix3 ClearRotation()
		{
			Matrix3 result = this;
			result.Row0 = new Vector3(result.Row0.Length, 0f, 0f);
			result.Row1 = new Vector3(0f, result.Row1.Length, 0f);
			result.Row2 = new Vector3(0f, 0f, result.Row2.Length);
			return result;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000F333 File Offset: 0x0000D533
		public readonly Vector3 ExtractScale()
		{
			return new Vector3(this.Row0.Length, this.Row1.Length, this.Row2.Length);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000F35C File Offset: 0x0000D55C
		public readonly Quaternion ExtractRotation(bool rowNormalize = true)
		{
			Vector3 vector = this.Row0;
			Vector3 vector2 = this.Row1;
			Vector3 vector3 = this.Row2;
			if (rowNormalize)
			{
				vector = vector.Normalized();
				vector2 = vector2.Normalized();
				vector3 = vector3.Normalized();
			}
			Quaternion result = default(Quaternion);
			double num = 0.25 * ((double)(vector[0] + vector2[1] + vector3[2]) + 1.0);
			if (num > 0.0)
			{
				double num2 = Math.Sqrt(num);
				result.W = (float)num2;
				num2 = 1.0 / (4.0 * num2);
				result.X = (float)((double)(vector2[2] - vector3[1]) * num2);
				result.Y = (float)((double)(vector3[0] - vector[2]) * num2);
				result.Z = (float)((double)(vector[1] - vector2[0]) * num2);
			}
			else if (vector[0] > vector2[1] && vector[0] > vector3[2])
			{
				double num3 = 2.0 * Math.Sqrt(1.0 + (double)vector[0] - (double)vector2[1] - (double)vector3[2]);
				result.X = (float)(0.25 * num3);
				num3 = 1.0 / num3;
				result.W = (float)((double)(vector3[1] - vector2[2]) * num3);
				result.Y = (float)((double)(vector2[0] + vector[1]) * num3);
				result.Z = (float)((double)(vector3[0] + vector[2]) * num3);
			}
			else if (vector2[1] > vector3[2])
			{
				double num4 = 2.0 * Math.Sqrt(1.0 + (double)vector2[1] - (double)vector[0] - (double)vector3[2]);
				result.Y = (float)(0.25 * num4);
				num4 = 1.0 / num4;
				result.W = (float)((double)(vector3[0] - vector[2]) * num4);
				result.X = (float)((double)(vector2[0] + vector[1]) * num4);
				result.Z = (float)((double)(vector3[1] + vector2[2]) * num4);
			}
			else
			{
				double num5 = 2.0 * Math.Sqrt(1.0 + (double)vector3[2] - (double)vector[0] - (double)vector2[1]);
				result.Z = (float)(0.25 * num5);
				num5 = 1.0 / num5;
				result.W = (float)((double)(vector2[0] - vector[1]) * num5);
				result.X = (float)((double)(vector3[0] + vector[2]) * num5);
				result.Y = (float)((double)(vector3[1] + vector2[2]) * num5);
			}
			result.Normalize();
			return result;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000F6D0 File Offset: 0x0000D8D0
		public static void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix3 result)
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
			result.Row1.X = num5 + num12;
			result.Row1.Y = num7 + num;
			result.Row1.Z = num8 - num10;
			result.Row2.X = num6 - num11;
			result.Row2.Y = num8 + num10;
			result.Row2.Z = num9 + num;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000F7D4 File Offset: 0x0000D9D4
		public static Matrix3 CreateFromAxisAngle(Vector3 axis, float angle)
		{
			Matrix3 result;
			Matrix3.CreateFromAxisAngle(axis, angle, out result);
			return result;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000F7EC File Offset: 0x0000D9EC
		public static void CreateFromQuaternion(in Quaternion q, out Matrix3 result)
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
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000F954 File Offset: 0x0000DB54
		public static Matrix3 CreateFromQuaternion(Quaternion q)
		{
			Matrix3 result;
			Matrix3.CreateFromQuaternion(q, out result);
			return result;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000F96C File Offset: 0x0000DB6C
		public static void CreateRotationX(float angle, out Matrix3 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result = Matrix3.Identity;
			result.Row1.Y = num;
			result.Row1.Z = num2;
			result.Row2.Y = -num2;
			result.Row2.Z = num;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000F9C4 File Offset: 0x0000DBC4
		public static Matrix3 CreateRotationX(float angle)
		{
			Matrix3 result;
			Matrix3.CreateRotationX(angle, out result);
			return result;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000F9DC File Offset: 0x0000DBDC
		public static void CreateRotationY(float angle, out Matrix3 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result = Matrix3.Identity;
			result.Row0.X = num;
			result.Row0.Z = -num2;
			result.Row2.X = num2;
			result.Row2.Z = num;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000FA34 File Offset: 0x0000DC34
		public static Matrix3 CreateRotationY(float angle)
		{
			Matrix3 result;
			Matrix3.CreateRotationY(angle, out result);
			return result;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000FA4C File Offset: 0x0000DC4C
		public static void CreateRotationZ(float angle, out Matrix3 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result = Matrix3.Identity;
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row1.X = -num2;
			result.Row1.Y = num;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
		public static Matrix3 CreateRotationZ(float angle)
		{
			Matrix3 result;
			Matrix3.CreateRotationZ(angle, out result);
			return result;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000FABC File Offset: 0x0000DCBC
		public static Matrix3 CreateScale(float scale)
		{
			Matrix3 result;
			Matrix3.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000FAD4 File Offset: 0x0000DCD4
		public static Matrix3 CreateScale(Vector3 scale)
		{
			Matrix3 result;
			Matrix3.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000FAEC File Offset: 0x0000DCEC
		public static Matrix3 CreateScale(float x, float y, float z)
		{
			Matrix3 result;
			Matrix3.CreateScale(x, y, z, out result);
			return result;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000FB04 File Offset: 0x0000DD04
		public static void CreateScale(float scale, out Matrix3 result)
		{
			result = Matrix3.Identity;
			result.Row0.X = scale;
			result.Row1.Y = scale;
			result.Row2.Z = scale;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000FB35 File Offset: 0x0000DD35
		public static void CreateScale(in Vector3 scale, out Matrix3 result)
		{
			result = Matrix3.Identity;
			result.Row0.X = scale.X;
			result.Row1.Y = scale.Y;
			result.Row2.Z = scale.Z;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000FB75 File Offset: 0x0000DD75
		public static void CreateScale(float x, float y, float z, out Matrix3 result)
		{
			result = Matrix3.Identity;
			result.Row0.X = x;
			result.Row1.Y = y;
			result.Row2.Z = z;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000FBA8 File Offset: 0x0000DDA8
		public static Matrix3 CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3 result;
			Matrix3.CreateSwizzle(rowForRow0, rowForRow1, rowForRow2, out result);
			return result;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
		public static void CreateSwizzle(int rowForRow0, int rowForRow1, int rowForRow2, out Matrix3 result)
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
			result = Matrix3.Zero;
			result[0, rowForRow0] = 1f;
			result[1, rowForRow1] = 1f;
			result[2, rowForRow2] = 1f;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000FC68 File Offset: 0x0000DE68
		public static Matrix3 Add(Matrix3 left, Matrix3 right)
		{
			Matrix3 result;
			Matrix3.Add(left, right, out result);
			return result;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000FC84 File Offset: 0x0000DE84
		public static void Add(in Matrix3 left, in Matrix3 right, out Matrix3 result)
		{
			Vector3.Add(left.Row0, right.Row0, out result.Row0);
			Vector3.Add(left.Row1, right.Row1, out result.Row1);
			Vector3.Add(left.Row2, right.Row2, out result.Row2);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000FCD8 File Offset: 0x0000DED8
		public static Matrix3 Mult(Matrix3 left, Matrix3 right)
		{
			Matrix3 result;
			Matrix3.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
		public static void Mult(in Matrix3 left, in Matrix3 right, out Matrix3 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float z = left.Row0.Z;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float z2 = left.Row1.Z;
			float x3 = left.Row2.X;
			float y3 = left.Row2.Y;
			float z3 = left.Row2.Z;
			float x4 = right.Row0.X;
			float y4 = right.Row0.Y;
			float z4 = right.Row0.Z;
			float x5 = right.Row1.X;
			float y5 = right.Row1.Y;
			float z5 = right.Row1.Z;
			float x6 = right.Row2.X;
			float y6 = right.Row2.Y;
			float z6 = right.Row2.Z;
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

		// Token: 0x06000490 RID: 1168 RVA: 0x0000FED8 File Offset: 0x0000E0D8
		public static void Invert(in Matrix3 matrix, out Matrix3 result)
		{
			float x = matrix.Row0.X;
			float y = matrix.Row0.Y;
			float z = matrix.Row0.Z;
			float x2 = matrix.Row1.X;
			float y2 = matrix.Row1.Y;
			float z2 = matrix.Row1.Z;
			float x3 = matrix.Row2.X;
			float y3 = matrix.Row2.Y;
			float z3 = matrix.Row2.Z;
			float num = y2 * z3 - z2 * y3;
			float num2 = -x2 * z3 + z2 * x3;
			float num3 = x2 * y3 - y2 * x3;
			float num4 = x * num + y * num2 + z * num3;
			if (num4 == 0f)
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
			num4 = 1f / num4;
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

		// Token: 0x06000491 RID: 1169 RVA: 0x000100E4 File Offset: 0x0000E2E4
		public static Matrix3 Invert(Matrix3 mat)
		{
			Matrix3 result;
			Matrix3.Invert(mat, out result);
			return result;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x000100FC File Offset: 0x0000E2FC
		public static void Transpose(in Matrix3 mat, out Matrix3 result)
		{
			result.Row0.X = mat.Row0.X;
			result.Row0.Y = mat.Row1.X;
			result.Row0.Z = mat.Row2.X;
			result.Row1.X = mat.Row0.Y;
			result.Row1.Y = mat.Row1.Y;
			result.Row1.Z = mat.Row2.Y;
			result.Row2.X = mat.Row0.Z;
			result.Row2.Y = mat.Row1.Z;
			result.Row2.Z = mat.Row2.Z;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x000101CF File Offset: 0x0000E3CF
		public static Matrix3 Transpose(Matrix3 mat)
		{
			return new Matrix3(mat.Column0, mat.Column1, mat.Column2);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x000101EC File Offset: 0x0000E3EC
		public static Matrix3 Swizzle(Matrix3 mat, int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3 result;
			Matrix3.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, out result);
			return result;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00010208 File Offset: 0x0000E408
		public static void Swizzle(in Matrix3 mat, int rowForRow0, int rowForRow1, int rowForRow2, out Matrix3 result)
		{
			Vector3 vector;
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
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow0", rowForRow0));
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
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow1", rowForRow1));
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
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow2", rowForRow2));
			}
			result.Row2 = vector;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001030B File Offset: 0x0000E50B
		public static Matrix3 operator *(Matrix3 left, Matrix3 right)
		{
			return Matrix3.Mult(left, right);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00010314 File Offset: 0x0000E514
		public static bool operator ==(Matrix3 left, Matrix3 right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001031E File Offset: 0x0000E51E
		public static bool operator !=(Matrix3 left, Matrix3 right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001032B File Offset: 0x0000E52B
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00010335 File Offset: 0x0000E535
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001033F File Offset: 0x0000E53F
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001034C File Offset: 0x0000E54C
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

		// Token: 0x0600049D RID: 1181 RVA: 0x000103AA File Offset: 0x0000E5AA
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3, Vector3, Vector3>(this.Row0, this.Row1, this.Row2);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000103C3 File Offset: 0x0000E5C3
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix3 && this.Equals((Matrix3)obj);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000103DB File Offset: 0x0000E5DB
		public readonly bool Equals(Matrix3 other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2;
		}

		// Token: 0x0400004C RID: 76
		public Vector3 Row0;

		// Token: 0x0400004D RID: 77
		public Vector3 Row1;

		// Token: 0x0400004E RID: 78
		public Vector3 Row2;

		// Token: 0x0400004F RID: 79
		public static readonly Matrix3 Identity = new Matrix3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

		// Token: 0x04000050 RID: 80
		public static readonly Matrix3 Zero = new Matrix3(Vector3.Zero, Vector3.Zero, Vector3.Zero);
	}
}
