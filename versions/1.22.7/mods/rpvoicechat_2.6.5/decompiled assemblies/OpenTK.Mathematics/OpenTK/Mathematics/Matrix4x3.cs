using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000020 RID: 32
	[Serializable]
	public struct Matrix4x3 : IEquatable<Matrix4x3>, IFormattable
	{
		// Token: 0x060007DC RID: 2012 RVA: 0x0001EF7D File Offset: 0x0001D17D
		public Matrix4x3(Vector3 row0, Vector3 row1, Vector3 row2, Vector3 row3)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
			this.Row3 = row3;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001EF9C File Offset: 0x0001D19C
		public Matrix4x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22, float m30, float m31, float m32)
		{
			this.Row0 = new Vector3(m00, m01, m02);
			this.Row1 = new Vector3(m10, m11, m12);
			this.Row2 = new Vector3(m20, m21, m22);
			this.Row3 = new Vector3(m30, m31, m32);
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x0001EFEA File Offset: 0x0001D1EA
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x0001F020 File Offset: 0x0001D220
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

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x0001F071 File Offset: 0x0001D271
		// (set) Token: 0x060007E1 RID: 2017 RVA: 0x0001F0A4 File Offset: 0x0001D2A4
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

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0001F0F5 File Offset: 0x0001D2F5
		// (set) Token: 0x060007E3 RID: 2019 RVA: 0x0001F128 File Offset: 0x0001D328
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

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0001F179 File Offset: 0x0001D379
		// (set) Token: 0x060007E5 RID: 2021 RVA: 0x0001F186 File Offset: 0x0001D386
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

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0001F194 File Offset: 0x0001D394
		// (set) Token: 0x060007E7 RID: 2023 RVA: 0x0001F1A1 File Offset: 0x0001D3A1
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

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0001F1AF File Offset: 0x0001D3AF
		// (set) Token: 0x060007E9 RID: 2025 RVA: 0x0001F1BC File Offset: 0x0001D3BC
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

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x0001F1CA File Offset: 0x0001D3CA
		// (set) Token: 0x060007EB RID: 2027 RVA: 0x0001F1D7 File Offset: 0x0001D3D7
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

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0001F1E5 File Offset: 0x0001D3E5
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x0001F1F2 File Offset: 0x0001D3F2
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

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x0001F200 File Offset: 0x0001D400
		// (set) Token: 0x060007EF RID: 2031 RVA: 0x0001F20D File Offset: 0x0001D40D
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

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x0001F21B File Offset: 0x0001D41B
		// (set) Token: 0x060007F1 RID: 2033 RVA: 0x0001F228 File Offset: 0x0001D428
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

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0001F236 File Offset: 0x0001D436
		// (set) Token: 0x060007F3 RID: 2035 RVA: 0x0001F243 File Offset: 0x0001D443
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

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0001F251 File Offset: 0x0001D451
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x0001F25E File Offset: 0x0001D45E
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

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001F26C File Offset: 0x0001D46C
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x0001F279 File Offset: 0x0001D479
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

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0001F287 File Offset: 0x0001D487
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x0001F294 File Offset: 0x0001D494
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

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001F2A2 File Offset: 0x0001D4A2
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x0001F2AF File Offset: 0x0001D4AF
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

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x0001F2BD File Offset: 0x0001D4BD
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x0001F2E5 File Offset: 0x0001D4E5
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

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x0001F31A File Offset: 0x0001D51A
		public readonly float Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y + this.Row2.Z;
			}
		}

		// Token: 0x170001BC RID: 444
		public float this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 4 && columnIndex < 3)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0f;
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

		// Token: 0x06000801 RID: 2049 RVA: 0x0001F3AD File Offset: 0x0001D5AD
		[MethodImpl(256)]
		private readonly ref Vector3 GetRowUnsafe(in Matrix4x3 m, int index)
		{
			return Unsafe.Add<Vector3>(Unsafe.AsRef<Vector3>(ref m.Row0), index);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001F3C0 File Offset: 0x0001D5C0
		public void Invert()
		{
			this = Matrix4x3.Invert(this);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001F3D4 File Offset: 0x0001D5D4
		public readonly Matrix4x3 Inverted()
		{
			Matrix4x3 result = this;
			result.Invert();
			return result;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001F3F0 File Offset: 0x0001D5F0
		public readonly Matrix3x4 Transposed()
		{
			return Matrix4x3.Transpose(this);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001F3FD File Offset: 0x0001D5FD
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			this = Matrix4x3.Swizzle(this, rowForRow0, rowForRow1, rowForRow2, rowForRow3);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001F418 File Offset: 0x0001D618
		public readonly Matrix4x3 Swizzled(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4x3 result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2, rowForRow3);
			return result;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001F43C File Offset: 0x0001D63C
		public static void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix4x3 result)
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
			result.Row3.X = 0f;
			result.Row3.Y = 0f;
			result.Row3.Z = 0f;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001F570 File Offset: 0x0001D770
		public static Matrix4x3 CreateFromAxisAngle(Vector3 axis, float angle)
		{
			Matrix4x3 result;
			Matrix4x3.CreateFromAxisAngle(axis, angle, out result);
			return result;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001F588 File Offset: 0x0001D788
		public static void CreateFromQuaternion(in Quaternion q, out Matrix4x3 result)
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
			result.Row3 = new Vector3(0f, 0f, 0f);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001F708 File Offset: 0x0001D908
		public static Matrix4x3 CreateFromQuaternion(Quaternion q)
		{
			Matrix4x3 result;
			Matrix4x3.CreateFromQuaternion(q, out result);
			return result;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001F720 File Offset: 0x0001D920
		public static void CreateRotationX(float angle, out Matrix4x3 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result.Row0.X = 1f;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = num;
			result.Row1.Z = num2;
			result.Row2.X = 0f;
			result.Row2.Y = -num2;
			result.Row2.Z = num;
			result.Row3.X = 0f;
			result.Row3.Y = 0f;
			result.Row3.Z = 0f;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001F7EC File Offset: 0x0001D9EC
		public static Matrix4x3 CreateRotationX(float angle)
		{
			Matrix4x3 result;
			Matrix4x3.CreateRotationX(angle, out result);
			return result;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001F804 File Offset: 0x0001DA04
		public static void CreateRotationY(float angle, out Matrix4x3 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = 0f;
			result.Row0.Z = -num2;
			result.Row1.X = 0f;
			result.Row1.Y = 1f;
			result.Row1.Z = 0f;
			result.Row2.X = num2;
			result.Row2.Y = 0f;
			result.Row2.Z = num;
			result.Row3.X = 0f;
			result.Row3.Y = 0f;
			result.Row3.Z = 0f;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001F8D0 File Offset: 0x0001DAD0
		public static Matrix4x3 CreateRotationY(float angle)
		{
			Matrix4x3 result;
			Matrix4x3.CreateRotationY(angle, out result);
			return result;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001F8E8 File Offset: 0x0001DAE8
		public static void CreateRotationZ(float angle, out Matrix4x3 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row0.Z = 0f;
			result.Row1.X = -num2;
			result.Row1.Y = num;
			result.Row1.Z = 0f;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row2.Z = 1f;
			result.Row3.X = 0f;
			result.Row3.Y = 0f;
			result.Row3.Z = 0f;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001F9B4 File Offset: 0x0001DBB4
		public static Matrix4x3 CreateRotationZ(float angle)
		{
			Matrix4x3 result;
			Matrix4x3.CreateRotationZ(angle, out result);
			return result;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001F9CC File Offset: 0x0001DBCC
		public static void CreateTranslation(float x, float y, float z, out Matrix4x3 result)
		{
			result.Row0.X = 1f;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = 1f;
			result.Row1.Z = 0f;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row2.Z = 1f;
			result.Row3.X = x;
			result.Row3.Y = y;
			result.Row3.Z = z;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001FA90 File Offset: 0x0001DC90
		public static void CreateTranslation(in Vector3 vector, out Matrix4x3 result)
		{
			result.Row0.X = 1f;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = 1f;
			result.Row1.Z = 0f;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row2.Z = 1f;
			result.Row3.X = vector.X;
			result.Row3.Y = vector.Y;
			result.Row3.Z = vector.Z;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001FB60 File Offset: 0x0001DD60
		public static Matrix4x3 CreateTranslation(float x, float y, float z)
		{
			Matrix4x3 result;
			Matrix4x3.CreateTranslation(x, y, z, out result);
			return result;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001FB78 File Offset: 0x0001DD78
		public static Matrix4x3 CreateTranslation(Vector3 vector)
		{
			Matrix4x3 result;
			Matrix4x3.CreateTranslation(vector.X, vector.Y, vector.Z, out result);
			return result;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001FB9F File Offset: 0x0001DD9F
		public static Matrix4x3 CreateScale(float scale)
		{
			return Matrix4x3.CreateScale(scale, scale, scale);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001FBA9 File Offset: 0x0001DDA9
		public static Matrix4x3 CreateScale(Vector3 scale)
		{
			return Matrix4x3.CreateScale(scale.X, scale.Y, scale.Z);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001FBC4 File Offset: 0x0001DDC4
		public static Matrix4x3 CreateScale(float x, float y, float z)
		{
			Matrix4x3 result;
			result.Row0.X = x;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = y;
			result.Row1.Z = 0f;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row2.Z = z;
			result.Row3.X = 0f;
			result.Row3.Y = 0f;
			result.Row3.Z = 0f;
			return result;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001FC94 File Offset: 0x0001DE94
		public static Matrix4 Mult(Matrix4x3 left, Matrix3x4 right)
		{
			Matrix4 result;
			Matrix4x3.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001FCB0 File Offset: 0x0001DEB0
		public static void Mult(in Matrix4x3 left, in Matrix3x4 right, out Matrix4 result)
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
			float x4 = left.Row3.X;
			float y4 = left.Row3.Y;
			float z4 = left.Row3.Z;
			float x5 = right.Row0.X;
			float y5 = right.Row0.Y;
			float z5 = right.Row0.Z;
			float w = right.Row0.W;
			float x6 = right.Row1.X;
			float y6 = right.Row1.Y;
			float z6 = right.Row1.Z;
			float w2 = right.Row1.W;
			float x7 = right.Row2.X;
			float y7 = right.Row2.Y;
			float z7 = right.Row2.Z;
			float w3 = right.Row2.W;
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

		// Token: 0x0600081A RID: 2074 RVA: 0x0001FFA4 File Offset: 0x0001E1A4
		public static Matrix4x3 Mult(Matrix4x3 left, Matrix4x3 right)
		{
			Matrix4x3 result;
			Matrix4x3.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001FFC0 File Offset: 0x0001E1C0
		public static void Mult(in Matrix4x3 left, in Matrix4x3 right, out Matrix4x3 result)
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
			float x4 = left.Row3.X;
			float y4 = left.Row3.Y;
			float z4 = left.Row3.Z;
			float x5 = right.Row0.X;
			float y5 = right.Row0.Y;
			float z5 = right.Row0.Z;
			float x6 = right.Row1.X;
			float y6 = right.Row1.Y;
			float z6 = right.Row1.Z;
			float x7 = right.Row2.X;
			float y7 = right.Row2.Y;
			float z7 = right.Row2.Z;
			float x8 = right.Row3.X;
			float y8 = right.Row3.Y;
			float z8 = right.Row3.Z;
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

		// Token: 0x0600081C RID: 2076 RVA: 0x0002026C File Offset: 0x0001E46C
		public static Matrix4x3 Mult(Matrix4x3 left, float right)
		{
			Matrix4x3 result;
			Matrix4x3.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00020284 File Offset: 0x0001E484
		public static void Mult(in Matrix4x3 left, float right, out Matrix4x3 result)
		{
			result.Row0 = left.Row0 * right;
			result.Row1 = left.Row1 * right;
			result.Row2 = left.Row2 * right;
			result.Row3 = left.Row3 * right;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x000202DC File Offset: 0x0001E4DC
		public static Matrix4x3 Add(Matrix4x3 left, Matrix4x3 right)
		{
			Matrix4x3 result;
			Matrix4x3.Add(left, right, out result);
			return result;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x000202F8 File Offset: 0x0001E4F8
		public static void Add(in Matrix4x3 left, in Matrix4x3 right, out Matrix4x3 result)
		{
			result.Row0 = left.Row0 + right.Row0;
			result.Row1 = left.Row1 + right.Row1;
			result.Row2 = left.Row2 + right.Row2;
			result.Row3 = left.Row3 + right.Row3;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00020364 File Offset: 0x0001E564
		public static Matrix4x3 Subtract(Matrix4x3 left, Matrix4x3 right)
		{
			Matrix4x3 result;
			Matrix4x3.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00020380 File Offset: 0x0001E580
		public static void Subtract(in Matrix4x3 left, in Matrix4x3 right, out Matrix4x3 result)
		{
			result.Row0 = left.Row0 - right.Row0;
			result.Row1 = left.Row1 - right.Row1;
			result.Row2 = left.Row2 - right.Row2;
			result.Row3 = left.Row3 - right.Row3;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x000203EC File Offset: 0x0001E5EC
		public static Matrix4x3 Invert(Matrix4x3 mat)
		{
			Matrix4x3 result;
			Matrix4x3.Invert(mat, out result);
			return result;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00020404 File Offset: 0x0001E604
		public static void Invert(in Matrix4x3 mat, out Matrix4x3 result)
		{
			Matrix3 matrix = new Matrix3(mat.Column0.Xyz, mat.Column1.Xyz, mat.Column2.Xyz);
			matrix.Row0 /= matrix.Row0.LengthSquared;
			matrix.Row1 /= matrix.Row1.LengthSquared;
			matrix.Row2 /= matrix.Row2.LengthSquared;
			Vector3 row = mat.Row3;
			result.Row0 = matrix.Row0;
			result.Row1 = matrix.Row1;
			result.Row2 = matrix.Row2;
			result.Row3 = new Vector3(-Vector3.Dot(matrix.Row0, row), -Vector3.Dot(matrix.Row1, row), -Vector3.Dot(matrix.Row2, row));
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00020508 File Offset: 0x0001E708
		public static Matrix3x4 Transpose(Matrix4x3 mat)
		{
			return new Matrix3x4(mat.Column0, mat.Column1, mat.Column2);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00020524 File Offset: 0x0001E724
		public static void Transpose(in Matrix4x3 mat, out Matrix3x4 result)
		{
			result.Row0 = mat.Column0;
			result.Row1 = mat.Column1;
			result.Row2 = mat.Column2;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0002054C File Offset: 0x0001E74C
		public static Matrix4x3 Swizzle(Matrix4x3 mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4x3 result;
			Matrix4x3.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, rowForRow3, out result);
			return result;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00020568 File Offset: 0x0001E768
		public static void Swizzle(in Matrix4x3 mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3, out Matrix4x3 result)
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

		// Token: 0x06000828 RID: 2088 RVA: 0x000206F3 File Offset: 0x0001E8F3
		public static Matrix4 operator *(Matrix4x3 left, Matrix3x4 right)
		{
			return Matrix4x3.Mult(left, right);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x000206FC File Offset: 0x0001E8FC
		public static Matrix4x3 operator *(Matrix4x3 left, Matrix4x3 right)
		{
			return Matrix4x3.Mult(left, right);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00020705 File Offset: 0x0001E905
		public static Matrix4x3 operator *(Matrix4x3 left, float right)
		{
			return Matrix4x3.Mult(left, right);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0002070E File Offset: 0x0001E90E
		public static Matrix4x3 operator +(Matrix4x3 left, Matrix4x3 right)
		{
			return Matrix4x3.Add(left, right);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00020717 File Offset: 0x0001E917
		public static Matrix4x3 operator -(Matrix4x3 left, Matrix4x3 right)
		{
			return Matrix4x3.Subtract(left, right);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00020720 File Offset: 0x0001E920
		public static bool operator ==(Matrix4x3 left, Matrix4x3 right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0002072A File Offset: 0x0001E92A
		public static bool operator !=(Matrix4x3 left, Matrix4x3 right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00020737 File Offset: 0x0001E937
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00020741 File Offset: 0x0001E941
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0002074B File Offset: 0x0001E94B
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00020758 File Offset: 0x0001E958
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

		// Token: 0x06000833 RID: 2099 RVA: 0x000207D0 File Offset: 0x0001E9D0
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3, Vector3, Vector3, Vector3>(this.Row0, this.Row1, this.Row2, this.Row3);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x000207EF File Offset: 0x0001E9EF
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix4x3 && this.Equals((Matrix4x3)obj);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00020808 File Offset: 0x0001EA08
		public readonly bool Equals(Matrix4x3 other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2 && this.Row3 == other.Row3;
		}

		// Token: 0x0400007C RID: 124
		public Vector3 Row0;

		// Token: 0x0400007D RID: 125
		public Vector3 Row1;

		// Token: 0x0400007E RID: 126
		public Vector3 Row2;

		// Token: 0x0400007F RID: 127
		public Vector3 Row3;

		// Token: 0x04000080 RID: 128
		public static readonly Matrix4x3 Zero = new Matrix4x3(Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero);
	}
}
