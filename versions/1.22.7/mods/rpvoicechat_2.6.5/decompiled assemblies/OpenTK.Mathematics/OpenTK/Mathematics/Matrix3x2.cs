using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public struct Matrix3x2 : IEquatable<Matrix3x2>, IFormattable
	{
		// Token: 0x060004F6 RID: 1270 RVA: 0x00011AEA File Offset: 0x0000FCEA
		public Matrix3x2(Vector2 row0, Vector2 row1, Vector2 row2)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00011B01 File Offset: 0x0000FD01
		public Matrix3x2(float m00, float m01, float m10, float m11, float m20, float m21)
		{
			this.Row0 = new Vector2(m00, m01);
			this.Row1 = new Vector2(m10, m11);
			this.Row2 = new Vector2(m20, m21);
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00011B2D File Offset: 0x0000FD2D
		// (set) Token: 0x060004F9 RID: 1273 RVA: 0x00011B55 File Offset: 0x0000FD55
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00011B8A File Offset: 0x0000FD8A
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x00011BB2 File Offset: 0x0000FDB2
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

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00011BE7 File Offset: 0x0000FDE7
		// (set) Token: 0x060004FD RID: 1277 RVA: 0x00011BF4 File Offset: 0x0000FDF4
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x00011C02 File Offset: 0x0000FE02
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x00011C0F File Offset: 0x0000FE0F
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

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x00011C1D File Offset: 0x0000FE1D
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x00011C2A File Offset: 0x0000FE2A
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

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x00011C38 File Offset: 0x0000FE38
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x00011C45 File Offset: 0x0000FE45
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

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00011C53 File Offset: 0x0000FE53
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x00011C60 File Offset: 0x0000FE60
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

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00011C6E File Offset: 0x0000FE6E
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00011C7B File Offset: 0x0000FE7B
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

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x00011C89 File Offset: 0x0000FE89
		// (set) Token: 0x06000509 RID: 1289 RVA: 0x00011CA6 File Offset: 0x0000FEA6
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

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00011CCA File Offset: 0x0000FECA
		public readonly float Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y;
			}
		}

		// Token: 0x1700012F RID: 303
		public float this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 3 && columnIndex < 2)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0f;
			}
			set
			{
				if (rowIndex < 3 && columnIndex < 2)
				{
					this.GetRowUnsafe(this, rowIndex)[columnIndex] = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to set this matrix at: ({0}, {1})", rowIndex, columnIndex));
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00011D51 File Offset: 0x0000FF51
		[MethodImpl(256)]
		private readonly ref Vector2 GetRowUnsafe(in Matrix3x2 m, int index)
		{
			return Unsafe.Add<Vector2>(Unsafe.AsRef<Vector2>(ref m.Row0), index);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00011D64 File Offset: 0x0000FF64
		public readonly Matrix2x3 Transposed()
		{
			return Matrix3x2.Transpose(this);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00011D71 File Offset: 0x0000FF71
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			this = Matrix3x2.Swizzle(this, rowForRow0, rowForRow1, rowForRow2);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00011D88 File Offset: 0x0000FF88
		public readonly Matrix3x2 Swizzled(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3x2 result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2);
			return result;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00011DA8 File Offset: 0x0000FFA8
		public static void CreateRotation(float angle, out Matrix3x2 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row1.X = -num2;
			result.Row1.Y = num;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00011E14 File Offset: 0x00010014
		public static Matrix3x2 CreateRotation(float angle)
		{
			Matrix3x2 result;
			Matrix3x2.CreateRotation(angle, out result);
			return result;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00011E2C File Offset: 0x0001002C
		public static void CreateScale(float scale, out Matrix3x2 result)
		{
			result.Row0.X = scale;
			result.Row0.Y = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = scale;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00011E94 File Offset: 0x00010094
		public static Matrix3x2 CreateScale(float scale)
		{
			Matrix3x2 result;
			Matrix3x2.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00011EAC File Offset: 0x000100AC
		public static void CreateScale(Vector2 scale, out Matrix3x2 result)
		{
			result.Row0.X = scale.X;
			result.Row0.Y = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = scale.Y;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00011F1C File Offset: 0x0001011C
		public static Matrix3x2 CreateScale(Vector2 scale)
		{
			Matrix3x2 result;
			Matrix3x2.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00011F34 File Offset: 0x00010134
		public static void CreateScale(float x, float y, out Matrix3x2 result)
		{
			result.Row0.X = x;
			result.Row0.Y = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = y;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00011F9C File Offset: 0x0001019C
		public static Matrix3x2 CreateScale(float x, float y)
		{
			Matrix3x2 result;
			Matrix3x2.CreateScale(x, y, out result);
			return result;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00011FB4 File Offset: 0x000101B4
		public static void Mult(in Matrix3x2 left, float right, out Matrix3x2 result)
		{
			result.Row0.X = left.Row0.X * right;
			result.Row0.Y = left.Row0.Y * right;
			result.Row1.X = left.Row1.X * right;
			result.Row1.Y = left.Row1.Y * right;
			result.Row2.X = left.Row2.X * right;
			result.Row2.Y = left.Row2.Y * right;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00012054 File Offset: 0x00010254
		public static Matrix3x2 Mult(Matrix3x2 left, float right)
		{
			Matrix3x2 result;
			Matrix3x2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001206C File Offset: 0x0001026C
		public static void Mult(in Matrix3x2 left, in Matrix2 right, out Matrix3x2 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float x3 = left.Row2.X;
			float y3 = left.Row2.Y;
			float x4 = right.Row0.X;
			float y4 = right.Row0.Y;
			float x5 = right.Row1.X;
			float y5 = right.Row1.Y;
			result.Row0.X = x * x4 + y * x5;
			result.Row0.Y = x * y4 + y * y5;
			result.Row1.X = x2 * x4 + y2 * x5;
			result.Row1.Y = x2 * y4 + y2 * y5;
			result.Row2.X = x3 * x4 + y3 * x5;
			result.Row2.Y = x3 * y4 + y3 * y5;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00012174 File Offset: 0x00010374
		public static Matrix3x2 Mult(Matrix3x2 left, Matrix2 right)
		{
			Matrix3x2 result;
			Matrix3x2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00012190 File Offset: 0x00010390
		public static void Mult(in Matrix3x2 left, in Matrix2x3 right, out Matrix3 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float x3 = left.Row2.X;
			float y3 = left.Row2.Y;
			float x4 = right.Row0.X;
			float y4 = right.Row0.Y;
			float z = right.Row0.Z;
			float x5 = right.Row1.X;
			float y5 = right.Row1.Y;
			float z2 = right.Row1.Z;
			result.Row0.X = x * x4 + y * x5;
			result.Row0.Y = x * y4 + y * y5;
			result.Row0.Z = x * z + y * z2;
			result.Row1.X = x2 * x4 + y2 * x5;
			result.Row1.Y = x2 * y4 + y2 * y5;
			result.Row1.Z = x2 * z + y2 * z2;
			result.Row2.X = x3 * x4 + y3 * x5;
			result.Row2.Y = x3 * y4 + y3 * y5;
			result.Row2.Z = x3 * z + y3 * z2;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000122F0 File Offset: 0x000104F0
		public static Matrix3 Mult(Matrix3x2 left, Matrix2x3 right)
		{
			Matrix3 result;
			Matrix3x2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001230C File Offset: 0x0001050C
		public static void Mult(in Matrix3x2 left, in Matrix2x4 right, out Matrix3x4 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float x3 = left.Row2.X;
			float y3 = left.Row2.Y;
			float x4 = right.Row0.X;
			float y4 = right.Row0.Y;
			float z = right.Row0.Z;
			float w = right.Row0.W;
			float x5 = right.Row1.X;
			float y5 = right.Row1.Y;
			float z2 = right.Row1.Z;
			float w2 = right.Row1.W;
			result.Row0.X = x * x4 + y * x5;
			result.Row0.Y = x * y4 + y * y5;
			result.Row0.Z = x * z + y * z2;
			result.Row0.W = x * w + y * w2;
			result.Row1.X = x2 * x4 + y2 * x5;
			result.Row1.Y = x2 * y4 + y2 * y5;
			result.Row1.Z = x2 * z + y2 * z2;
			result.Row1.W = x2 * w + y2 * w2;
			result.Row2.X = x3 * x4 + y3 * x5;
			result.Row2.Y = x3 * y4 + y3 * y5;
			result.Row2.Z = x3 * z + y3 * z2;
			result.Row2.W = x3 * w + y3 * w2;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x000124C4 File Offset: 0x000106C4
		public static Matrix3x4 Mult(Matrix3x2 left, Matrix2x4 right)
		{
			Matrix3x4 result;
			Matrix3x2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x000124E0 File Offset: 0x000106E0
		public static void Add(in Matrix3x2 left, in Matrix3x2 right, out Matrix3x2 result)
		{
			result.Row0.X = left.Row0.X + right.Row0.X;
			result.Row0.Y = left.Row0.Y + right.Row0.Y;
			result.Row1.X = left.Row1.X + right.Row1.X;
			result.Row1.Y = left.Row1.Y + right.Row1.Y;
			result.Row2.X = left.Row2.X + right.Row2.X;
			result.Row2.Y = left.Row2.Y + right.Row2.Y;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000125BC File Offset: 0x000107BC
		public static Matrix3x2 Add(Matrix3x2 left, Matrix3x2 right)
		{
			Matrix3x2 result;
			Matrix3x2.Add(left, right, out result);
			return result;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000125D8 File Offset: 0x000107D8
		public static void Subtract(in Matrix3x2 left, in Matrix3x2 right, out Matrix3x2 result)
		{
			result.Row0.X = left.Row0.X - right.Row0.X;
			result.Row0.Y = left.Row0.Y - right.Row0.Y;
			result.Row1.X = left.Row1.X - right.Row1.X;
			result.Row1.Y = left.Row1.Y - right.Row1.Y;
			result.Row2.X = left.Row2.X - right.Row2.X;
			result.Row2.Y = left.Row2.Y - right.Row2.Y;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000126B4 File Offset: 0x000108B4
		public static Matrix3x2 Subtract(Matrix3x2 left, Matrix3x2 right)
		{
			Matrix3x2 result;
			Matrix3x2.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000126D0 File Offset: 0x000108D0
		public static void Transpose(in Matrix3x2 mat, out Matrix2x3 result)
		{
			result.Row0.X = mat.Row0.X;
			result.Row0.Y = mat.Row1.X;
			result.Row0.Z = mat.Row2.X;
			result.Row1.X = mat.Row0.Y;
			result.Row1.Y = mat.Row1.Y;
			result.Row1.Z = mat.Row2.Y;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00012764 File Offset: 0x00010964
		public static Matrix2x3 Transpose(Matrix3x2 mat)
		{
			Matrix2x3 result;
			Matrix3x2.Transpose(mat, out result);
			return result;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001277C File Offset: 0x0001097C
		public static Matrix3x2 Swizzle(Matrix3x2 mat, int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3x2 result;
			Matrix3x2.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, out result);
			return result;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00012798 File Offset: 0x00010998
		public static void Swizzle(in Matrix3x2 mat, int rowForRow0, int rowForRow1, int rowForRow2, out Matrix3x2 result)
		{
			Vector2 vector;
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

		// Token: 0x06000529 RID: 1321 RVA: 0x0001289B File Offset: 0x00010A9B
		public static Matrix3x2 operator *(float left, Matrix3x2 right)
		{
			return Matrix3x2.Mult(right, left);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000128A4 File Offset: 0x00010AA4
		public static Matrix3x2 operator *(Matrix3x2 left, float right)
		{
			return Matrix3x2.Mult(left, right);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x000128AD File Offset: 0x00010AAD
		public static Matrix3x2 operator *(Matrix3x2 left, Matrix2 right)
		{
			return Matrix3x2.Mult(left, right);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000128B6 File Offset: 0x00010AB6
		public static Matrix3 operator *(Matrix3x2 left, Matrix2x3 right)
		{
			return Matrix3x2.Mult(left, right);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000128BF File Offset: 0x00010ABF
		public static Matrix3x4 operator *(Matrix3x2 left, Matrix2x4 right)
		{
			return Matrix3x2.Mult(left, right);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000128C8 File Offset: 0x00010AC8
		public static Matrix3x2 operator +(Matrix3x2 left, Matrix3x2 right)
		{
			return Matrix3x2.Add(left, right);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000128D1 File Offset: 0x00010AD1
		public static Matrix3x2 operator -(Matrix3x2 left, Matrix3x2 right)
		{
			return Matrix3x2.Subtract(left, right);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000128DA File Offset: 0x00010ADA
		public static bool operator ==(Matrix3x2 left, Matrix3x2 right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000128E4 File Offset: 0x00010AE4
		public static bool operator !=(Matrix3x2 left, Matrix3x2 right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000128F1 File Offset: 0x00010AF1
		public unsafe static explicit operator Matrix3x2(Matrix3x2 mat)
		{
			return *Unsafe.As<Matrix3x2, Matrix3x2>(ref mat);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000128FF File Offset: 0x00010AFF
		public unsafe static explicit operator Matrix3x2(Matrix3x2 mat)
		{
			return *Unsafe.As<Matrix3x2, Matrix3x2>(ref mat);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001290D File Offset: 0x00010B0D
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00012917 File Offset: 0x00010B17
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00012921 File Offset: 0x00010B21
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001292C File Offset: 0x00010B2C
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

		// Token: 0x06000538 RID: 1336 RVA: 0x0001298A File Offset: 0x00010B8A
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector2, Vector2, Vector2>(this.Row0, this.Row1, this.Row2);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x000129A3 File Offset: 0x00010BA3
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix3x2 && this.Equals((Matrix3x2)obj);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x000129BB File Offset: 0x00010BBB
		public readonly bool Equals(Matrix3x2 other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2;
		}

		// Token: 0x04000056 RID: 86
		public Vector2 Row0;

		// Token: 0x04000057 RID: 87
		public Vector2 Row1;

		// Token: 0x04000058 RID: 88
		public Vector2 Row2;

		// Token: 0x04000059 RID: 89
		public static readonly Matrix3x2 Zero = new Matrix3x2(Vector2.Zero, Vector2.Zero, Vector2.Zero);
	}
}
