using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public struct Matrix3x4 : IEquatable<Matrix3x4>, IFormattable
	{
		// Token: 0x06000580 RID: 1408 RVA: 0x00013959 File Offset: 0x00011B59
		public Matrix3x4(Vector4 row0, Vector4 row1, Vector4 row2)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00013970 File Offset: 0x00011B70
		public Matrix3x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23)
		{
			this.Row0 = new Vector4(m00, m01, m02, m03);
			this.Row1 = new Vector4(m10, m11, m12, m13);
			this.Row2 = new Vector4(m20, m21, m22, m23);
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x000139A8 File Offset: 0x00011BA8
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x000139D0 File Offset: 0x00011BD0
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

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x00013A05 File Offset: 0x00011C05
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x00013A2D File Offset: 0x00011C2D
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

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00013A62 File Offset: 0x00011C62
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00013A8A File Offset: 0x00011C8A
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

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00013ABF File Offset: 0x00011CBF
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x00013AE7 File Offset: 0x00011CE7
		public Vector3 Column3
		{
			readonly get
			{
				return new Vector3(this.Row0.W, this.Row1.W, this.Row2.W);
			}
			set
			{
				this.Row0.W = value.X;
				this.Row1.W = value.Y;
				this.Row2.W = value.Z;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00013B1C File Offset: 0x00011D1C
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00013B29 File Offset: 0x00011D29
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

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00013B37 File Offset: 0x00011D37
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x00013B44 File Offset: 0x00011D44
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

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x00013B52 File Offset: 0x00011D52
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x00013B5F File Offset: 0x00011D5F
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

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x00013B6D File Offset: 0x00011D6D
		// (set) Token: 0x06000591 RID: 1425 RVA: 0x00013B7A File Offset: 0x00011D7A
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

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x00013B88 File Offset: 0x00011D88
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x00013B95 File Offset: 0x00011D95
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

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00013BA3 File Offset: 0x00011DA3
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x00013BB0 File Offset: 0x00011DB0
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

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00013BBE File Offset: 0x00011DBE
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x00013BCB File Offset: 0x00011DCB
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

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x00013BD9 File Offset: 0x00011DD9
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x00013BE6 File Offset: 0x00011DE6
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

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x00013BF4 File Offset: 0x00011DF4
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x00013C01 File Offset: 0x00011E01
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

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00013C0F File Offset: 0x00011E0F
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x00013C1C File Offset: 0x00011E1C
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

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x00013C2A File Offset: 0x00011E2A
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x00013C37 File Offset: 0x00011E37
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

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x00013C45 File Offset: 0x00011E45
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x00013C52 File Offset: 0x00011E52
		public float M34
		{
			readonly get
			{
				return this.Row2.W;
			}
			set
			{
				this.Row2.W = value;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00013C60 File Offset: 0x00011E60
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x00013C88 File Offset: 0x00011E88
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

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00013CBD File Offset: 0x00011EBD
		public readonly float Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y + this.Row2.Z;
			}
		}

		// Token: 0x1700014D RID: 333
		public float this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 3 && columnIndex < 4)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0f;
			}
			set
			{
				if (rowIndex < 3 && columnIndex < 4)
				{
					this.GetRowUnsafe(this, rowIndex)[columnIndex] = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to set this matrix at: ({0}, {1})", rowIndex, columnIndex));
			}
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00013D50 File Offset: 0x00011F50
		[MethodImpl(256)]
		private readonly ref Vector4 GetRowUnsafe(in Matrix3x4 m, int index)
		{
			return Unsafe.Add<Vector4>(Unsafe.AsRef<Vector4>(ref m.Row0), index);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00013D63 File Offset: 0x00011F63
		public void Invert()
		{
			this = Matrix3x4.Invert(this);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00013D78 File Offset: 0x00011F78
		public readonly Matrix3x4 Inverted()
		{
			Matrix3x4 result = this;
			result.Invert();
			return result;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00013D94 File Offset: 0x00011F94
		public readonly Matrix4x3 Transposed()
		{
			return Matrix3x4.Transpose(this);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00013DA1 File Offset: 0x00011FA1
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			this = Matrix3x4.Swizzle(this, rowForRow0, rowForRow1, rowForRow2);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00013DB8 File Offset: 0x00011FB8
		public readonly Matrix3x4 Swizzled(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3x4 result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2);
			return result;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00013DD8 File Offset: 0x00011FD8
		public static void CreateFromAxisAngle(Vector3 axis, float angle, out Matrix3x4 result)
		{
			axis.Normalize();
			float x = axis.X;
			float y = axis.Y;
			float z = axis.Z;
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
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
			result.Row0.W = 0f;
			result.Row1.X = num5 + num12;
			result.Row1.Y = num7 + num;
			result.Row1.Z = num8 - num10;
			result.Row1.W = 0f;
			result.Row2.X = num6 - num11;
			result.Row2.Y = num8 + num10;
			result.Row2.Z = num9 + num;
			result.Row2.W = 0f;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00013F0C File Offset: 0x0001210C
		public static Matrix3x4 CreateFromAxisAngle(Vector3 axis, float angle)
		{
			Matrix3x4 result;
			Matrix3x4.CreateFromAxisAngle(axis, angle, out result);
			return result;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00013F24 File Offset: 0x00012124
		public static void CreateFromQuaternion(in Quaternion q, out Matrix3x4 result)
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
			result.Row0.W = 0f;
			result.Row1.W = 0f;
			result.Row2.W = 0f;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x000140BC File Offset: 0x000122BC
		public static Matrix3x4 CreateFromQuaternion(Quaternion q)
		{
			Matrix3x4 result;
			Matrix3x4.CreateFromQuaternion(q, out result);
			return result;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000140D4 File Offset: 0x000122D4
		public static void CreateRotationX(float angle, out Matrix3x4 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result.Row0.X = 1f;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row0.W = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = num;
			result.Row1.Z = num2;
			result.Row1.W = 0f;
			result.Row2.X = 0f;
			result.Row2.Y = -num2;
			result.Row2.Z = num;
			result.Row2.W = 0f;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000141A0 File Offset: 0x000123A0
		public static Matrix3x4 CreateRotationX(float angle)
		{
			Matrix3x4 result;
			Matrix3x4.CreateRotationX(angle, out result);
			return result;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000141B8 File Offset: 0x000123B8
		public static void CreateRotationY(float angle, out Matrix3x4 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = 0f;
			result.Row0.Z = -num2;
			result.Row0.W = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = 1f;
			result.Row1.Z = 0f;
			result.Row1.W = 0f;
			result.Row2.X = num2;
			result.Row2.Y = 0f;
			result.Row2.Z = num;
			result.Row2.W = 0f;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00014284 File Offset: 0x00012484
		public static Matrix3x4 CreateRotationY(float angle)
		{
			Matrix3x4 result;
			Matrix3x4.CreateRotationY(angle, out result);
			return result;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001429C File Offset: 0x0001249C
		public static void CreateRotationZ(float angle, out Matrix3x4 result)
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
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row2.Z = 1f;
			result.Row2.W = 0f;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00014368 File Offset: 0x00012568
		public static Matrix3x4 CreateRotationZ(float angle)
		{
			Matrix3x4 result;
			Matrix3x4.CreateRotationZ(angle, out result);
			return result;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00014380 File Offset: 0x00012580
		public static void CreateTranslation(float x, float y, float z, out Matrix3x4 result)
		{
			result.Row0.X = 1f;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row0.W = x;
			result.Row1.X = 0f;
			result.Row1.Y = 1f;
			result.Row1.Z = 0f;
			result.Row1.W = y;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row2.Z = 1f;
			result.Row2.W = z;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00014444 File Offset: 0x00012644
		public static void CreateTranslation(in Vector3 vector, out Matrix3x4 result)
		{
			result.Row0.X = 1f;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row0.W = vector.X;
			result.Row1.X = 0f;
			result.Row1.Y = 1f;
			result.Row1.Z = 0f;
			result.Row1.W = vector.Y;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row2.Z = 1f;
			result.Row2.W = vector.Z;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00014514 File Offset: 0x00012714
		public static Matrix3x4 CreateTranslation(float x, float y, float z)
		{
			Matrix3x4 result;
			Matrix3x4.CreateTranslation(x, y, z, out result);
			return result;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001452C File Offset: 0x0001272C
		public static Matrix3x4 CreateTranslation(Vector3 vector)
		{
			Matrix3x4 result;
			Matrix3x4.CreateTranslation(vector.X, vector.Y, vector.Z, out result);
			return result;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00014553 File Offset: 0x00012753
		public static Matrix3x4 CreateScale(float scale)
		{
			return Matrix3x4.CreateScale(scale, scale, scale);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001455D File Offset: 0x0001275D
		public static Matrix3x4 CreateScale(Vector3 scale)
		{
			return Matrix3x4.CreateScale(scale.X, scale.Y, scale.Z);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00014578 File Offset: 0x00012778
		public static Matrix3x4 CreateScale(float x, float y, float z)
		{
			Matrix3x4 result;
			result.Row0.X = x;
			result.Row0.Y = 0f;
			result.Row0.Z = 0f;
			result.Row0.W = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = y;
			result.Row1.Z = 0f;
			result.Row1.W = 0f;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row2.Z = z;
			result.Row2.W = 0f;
			return result;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00014648 File Offset: 0x00012848
		public static Matrix3 Mult(Matrix3x4 left, Matrix4x3 right)
		{
			Matrix3 result;
			Matrix3x4.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00014664 File Offset: 0x00012864
		public static void Mult(in Matrix3x4 left, in Matrix4x3 right, out Matrix3 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float z = left.Row0.Z;
			float w = left.Row0.W;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float z2 = left.Row1.Z;
			float w2 = left.Row1.W;
			float x3 = left.Row2.X;
			float y3 = left.Row2.Y;
			float z3 = left.Row2.Z;
			float w3 = left.Row2.W;
			float x4 = right.Row0.X;
			float y4 = right.Row0.Y;
			float z4 = right.Row0.Z;
			float x5 = right.Row1.X;
			float y5 = right.Row1.Y;
			float z5 = right.Row1.Z;
			float x6 = right.Row2.X;
			float y6 = right.Row2.Y;
			float z6 = right.Row2.Z;
			float x7 = right.Row3.X;
			float y7 = right.Row3.Y;
			float z7 = right.Row3.Z;
			result.Row0.X = x * x4 + y * x5 + z * x6 + w * x7;
			result.Row0.Y = x * y4 + y * y5 + z * y6 + w * y7;
			result.Row0.Z = x * z4 + y * z5 + z * z6 + w * z7;
			result.Row1.X = x2 * x4 + y2 * x5 + z2 * x6 + w2 * x7;
			result.Row1.Y = x2 * y4 + y2 * y5 + z2 * y6 + w2 * y7;
			result.Row1.Z = x2 * z4 + y2 * z5 + z2 * z6 + w2 * z7;
			result.Row2.X = x3 * x4 + y3 * x5 + z3 * x6 + w3 * x7;
			result.Row2.Y = x3 * y4 + y3 * y5 + z3 * y6 + w3 * y7;
			result.Row2.Z = x3 * z4 + y3 * z5 + z3 * z6 + w3 * z7;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000148CC File Offset: 0x00012ACC
		public static Matrix3x4 Mult(Matrix3x4 left, Matrix3x4 right)
		{
			Matrix3x4 result;
			Matrix3x4.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000148E8 File Offset: 0x00012AE8
		public static void Mult(in Matrix3x4 left, in Matrix3x4 right, out Matrix3x4 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float z = left.Row0.Z;
			float w = left.Row0.W;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float z2 = left.Row1.Z;
			float w2 = left.Row1.W;
			float x3 = left.Row2.X;
			float y3 = left.Row2.Y;
			float z3 = left.Row2.Z;
			float w3 = left.Row2.W;
			float x4 = right.Row0.X;
			float y4 = right.Row0.Y;
			float z4 = right.Row0.Z;
			float w4 = right.Row0.W;
			float x5 = right.Row1.X;
			float y5 = right.Row1.Y;
			float z5 = right.Row1.Z;
			float w5 = right.Row1.W;
			float x6 = right.Row2.X;
			float y6 = right.Row2.Y;
			float z6 = right.Row2.Z;
			float w6 = right.Row2.W;
			result.Row0.X = x * x4 + y * x5 + z * x6;
			result.Row0.Y = x * y4 + y * y5 + z * y6;
			result.Row0.Z = x * z4 + y * z5 + z * z6;
			result.Row0.W = x * w4 + y * w5 + z * w6 + w;
			result.Row1.X = x2 * x4 + y2 * x5 + z2 * x6;
			result.Row1.Y = x2 * y4 + y2 * y5 + z2 * y6;
			result.Row1.Z = x2 * z4 + y2 * z5 + z2 * z6;
			result.Row1.W = x2 * w4 + y2 * w5 + z2 * w6 + w2;
			result.Row2.X = x3 * x4 + y3 * x5 + z3 * x6;
			result.Row2.Y = x3 * y4 + y3 * y5 + z3 * y6;
			result.Row2.Z = x3 * z4 + y3 * z5 + z3 * z6;
			result.Row2.W = x3 * w4 + y3 * w5 + z3 * w6 + w3;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00014B78 File Offset: 0x00012D78
		public static Matrix3x4 Mult(Matrix3x4 left, float right)
		{
			Matrix3x4 result;
			Matrix3x4.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00014B90 File Offset: 0x00012D90
		public static void Mult(in Matrix3x4 left, float right, out Matrix3x4 result)
		{
			result.Row0 = left.Row0 * right;
			result.Row1 = left.Row1 * right;
			result.Row2 = left.Row2 * right;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00014BC8 File Offset: 0x00012DC8
		public static Matrix3x4 Add(Matrix3x4 left, Matrix3x4 right)
		{
			Matrix3x4 result;
			Matrix3x4.Add(left, right, out result);
			return result;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00014BE4 File Offset: 0x00012DE4
		public static void Add(in Matrix3x4 left, in Matrix3x4 right, out Matrix3x4 result)
		{
			result.Row0 = left.Row0 + right.Row0;
			result.Row1 = left.Row1 + right.Row1;
			result.Row2 = left.Row2 + right.Row2;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00014C38 File Offset: 0x00012E38
		public static Matrix3x4 Subtract(Matrix3x4 left, Matrix3x4 right)
		{
			Matrix3x4 result;
			Matrix3x4.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00014C54 File Offset: 0x00012E54
		public static void Subtract(in Matrix3x4 left, in Matrix3x4 right, out Matrix3x4 result)
		{
			result.Row0 = left.Row0 - right.Row0;
			result.Row1 = left.Row1 - right.Row1;
			result.Row2 = left.Row2 - right.Row2;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00014CA8 File Offset: 0x00012EA8
		public static Matrix3x4 Invert(Matrix3x4 mat)
		{
			Matrix3x4 result;
			Matrix3x4.Invert(mat, out result);
			return result;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00014CC0 File Offset: 0x00012EC0
		public static void Invert(in Matrix3x4 mat, out Matrix3x4 result)
		{
			Matrix3 matrix = new Matrix3(mat.Column0, mat.Column1, mat.Column2);
			matrix.Row0 /= matrix.Row0.LengthSquared;
			matrix.Row1 /= matrix.Row1.LengthSquared;
			matrix.Row2 /= matrix.Row2.LengthSquared;
			Vector3 right = new Vector3(mat.Row0.W, mat.Row1.W, mat.Row2.W);
			result.Row0 = new Vector4(matrix.Row0, -Vector3.Dot(matrix.Row0, right));
			result.Row1 = new Vector4(matrix.Row1, -Vector3.Dot(matrix.Row1, right));
			result.Row2 = new Vector4(matrix.Row2, -Vector3.Dot(matrix.Row2, right));
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00014DD1 File Offset: 0x00012FD1
		public static Matrix4x3 Transpose(Matrix3x4 mat)
		{
			return new Matrix4x3(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00014DF4 File Offset: 0x00012FF4
		public static void Transpose(in Matrix3x4 mat, out Matrix4x3 result)
		{
			result.Row0 = mat.Column0;
			result.Row1 = mat.Column1;
			result.Row2 = mat.Column2;
			result.Row3 = mat.Column3;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00014E28 File Offset: 0x00013028
		public static Matrix3x4 Swizzle(Matrix3x4 mat, int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3x4 result;
			Matrix3x4.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, out result);
			return result;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00014E44 File Offset: 0x00013044
		public static void Swizzle(in Matrix3x4 mat, int rowForRow0, int rowForRow1, int rowForRow2, out Matrix3x4 result)
		{
			Vector4 vector;
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

		// Token: 0x060005CE RID: 1486 RVA: 0x00014F47 File Offset: 0x00013147
		public static Matrix3 operator *(Matrix3x4 left, Matrix4x3 right)
		{
			return Matrix3x4.Mult(left, right);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00014F50 File Offset: 0x00013150
		public static Matrix3x4 operator *(Matrix3x4 left, Matrix3x4 right)
		{
			return Matrix3x4.Mult(left, right);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00014F59 File Offset: 0x00013159
		public static Matrix3x4 operator *(Matrix3x4 left, float right)
		{
			return Matrix3x4.Mult(left, right);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00014F62 File Offset: 0x00013162
		public static Matrix3x4 operator +(Matrix3x4 left, Matrix3x4 right)
		{
			return Matrix3x4.Add(left, right);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00014F6B File Offset: 0x0001316B
		public static Matrix3x4 operator -(Matrix3x4 left, Matrix3x4 right)
		{
			return Matrix3x4.Subtract(left, right);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00014F74 File Offset: 0x00013174
		public static bool operator ==(Matrix3x4 left, Matrix3x4 right)
		{
			return left.Equals(right);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00014F7E File Offset: 0x0001317E
		public static bool operator !=(Matrix3x4 left, Matrix3x4 right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00014F8B File Offset: 0x0001318B
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00014F95 File Offset: 0x00013195
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00014F9F File Offset: 0x0001319F
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00014FAC File Offset: 0x000131AC
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

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001500A File Offset: 0x0001320A
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector4, Vector4, Vector4>(this.Row0, this.Row1, this.Row2);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00015023 File Offset: 0x00013223
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix3x4 && this.Equals((Matrix3x4)obj);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001503B File Offset: 0x0001323B
		public readonly bool Equals(Matrix3x4 other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2;
		}

		// Token: 0x0400005E RID: 94
		public Vector4 Row0;

		// Token: 0x0400005F RID: 95
		public Vector4 Row1;

		// Token: 0x04000060 RID: 96
		public Vector4 Row2;

		// Token: 0x04000061 RID: 97
		public static readonly Matrix3x4 Zero = new Matrix3x4(Vector4.Zero, Vector4.Zero, Vector4.Zero);
	}
}
