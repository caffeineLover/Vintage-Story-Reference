using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x0200001B RID: 27
	[Serializable]
	public struct Matrix3x4d : IEquatable<Matrix3x4d>, IFormattable
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x00015091 File Offset: 0x00013291
		public Matrix3x4d(Vector4d row0, Vector4d row1, Vector4d row2)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000150A8 File Offset: 0x000132A8
		public Matrix3x4d(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23)
		{
			this.Row0 = new Vector4d(m00, m01, m02, m03);
			this.Row1 = new Vector4d(m10, m11, m12, m13);
			this.Row2 = new Vector4d(m20, m21, m22, m23);
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x000150E0 File Offset: 0x000132E0
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x00015108 File Offset: 0x00013308
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

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0001513D File Offset: 0x0001333D
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x00015165 File Offset: 0x00013365
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

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001519A File Offset: 0x0001339A
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x000151C2 File Offset: 0x000133C2
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

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x000151F7 File Offset: 0x000133F7
		// (set) Token: 0x060005E6 RID: 1510 RVA: 0x0001521F File Offset: 0x0001341F
		public Vector3d Column3
		{
			readonly get
			{
				return new Vector3d(this.Row0.W, this.Row1.W, this.Row2.W);
			}
			set
			{
				this.Row0.W = value.X;
				this.Row1.W = value.Y;
				this.Row2.W = value.Z;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x00015254 File Offset: 0x00013454
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x00015261 File Offset: 0x00013461
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

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0001526F File Offset: 0x0001346F
		// (set) Token: 0x060005EA RID: 1514 RVA: 0x0001527C File Offset: 0x0001347C
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

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0001528A File Offset: 0x0001348A
		// (set) Token: 0x060005EC RID: 1516 RVA: 0x00015297 File Offset: 0x00013497
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

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x000152A5 File Offset: 0x000134A5
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x000152B2 File Offset: 0x000134B2
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

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x000152C0 File Offset: 0x000134C0
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x000152CD File Offset: 0x000134CD
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

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x000152DB File Offset: 0x000134DB
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x000152E8 File Offset: 0x000134E8
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

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x000152F6 File Offset: 0x000134F6
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x00015303 File Offset: 0x00013503
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

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00015311 File Offset: 0x00013511
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x0001531E File Offset: 0x0001351E
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

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001532C File Offset: 0x0001352C
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x00015339 File Offset: 0x00013539
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

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x00015347 File Offset: 0x00013547
		// (set) Token: 0x060005FA RID: 1530 RVA: 0x00015354 File Offset: 0x00013554
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

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x00015362 File Offset: 0x00013562
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x0001536F File Offset: 0x0001356F
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

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x0001537D File Offset: 0x0001357D
		// (set) Token: 0x060005FE RID: 1534 RVA: 0x0001538A File Offset: 0x0001358A
		public double M34
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

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x00015398 File Offset: 0x00013598
		// (set) Token: 0x06000600 RID: 1536 RVA: 0x000153C0 File Offset: 0x000135C0
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

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x000153F5 File Offset: 0x000135F5
		public readonly double Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y + this.Row2.Z;
			}
		}

		// Token: 0x17000160 RID: 352
		public double this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 3 && columnIndex < 4)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0.0;
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

		// Token: 0x06000604 RID: 1540 RVA: 0x0001548C File Offset: 0x0001368C
		[MethodImpl(256)]
		private readonly ref Vector4d GetRowUnsafe(in Matrix3x4d m, int index)
		{
			return Unsafe.Add<Vector4d>(Unsafe.AsRef<Vector4d>(ref m.Row0), index);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001549F File Offset: 0x0001369F
		public void Invert()
		{
			this = Matrix3x4d.Invert(this);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000154B4 File Offset: 0x000136B4
		public readonly Matrix3x4d Inverted()
		{
			Matrix3x4d result = this;
			result.Invert();
			return result;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000154D0 File Offset: 0x000136D0
		public readonly Matrix4x3d Transposed()
		{
			return Matrix3x4d.Transpose(this);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000154DD File Offset: 0x000136DD
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			this = Matrix3x4d.Swizzle(this, rowForRow0, rowForRow1, rowForRow2);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000154F4 File Offset: 0x000136F4
		public readonly Matrix3x4d Swizzled(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3x4d result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2);
			return result;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00015514 File Offset: 0x00013714
		public static void CreateFromAxisAngle(Vector3d axis, double angle, out Matrix3x4d result)
		{
			axis.Normalize();
			double x = axis.X;
			double y = axis.Y;
			double z = axis.Z;
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
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
			result.Row0.W = 0.0;
			result.Row1.X = num5 + num12;
			result.Row1.Y = num7 + num;
			result.Row1.Z = num8 - num10;
			result.Row1.W = 0.0;
			result.Row2.X = num6 - num11;
			result.Row2.Y = num8 + num10;
			result.Row2.Z = num9 + num;
			result.Row2.W = 0.0;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00015658 File Offset: 0x00013858
		public static Matrix3x4d CreateFromAxisAngle(Vector3d axis, double angle)
		{
			Matrix3x4d result;
			Matrix3x4d.CreateFromAxisAngle(axis, angle, out result);
			return result;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00015670 File Offset: 0x00013870
		public static void CreateFromQuaternion(in Quaternion q, out Matrix3x4d result)
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
			result.Row0.W = 0.0;
			result.Row1.W = 0.0;
			result.Row2.W = 0.0;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001582C File Offset: 0x00013A2C
		public static Matrix3x4d CreateFromQuaternion(Quaternion q)
		{
			Matrix3x4d result;
			Matrix3x4d.CreateFromQuaternion(q, out result);
			return result;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00015844 File Offset: 0x00013A44
		public static void CreateRotationX(double angle, out Matrix3x4d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0.X = 1.0;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row0.W = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = num;
			result.Row1.Z = num2;
			result.Row1.W = 0.0;
			result.Row2.X = 0.0;
			result.Row2.Y = -num2;
			result.Row2.Z = num;
			result.Row2.W = 0.0;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00015930 File Offset: 0x00013B30
		public static Matrix3x4d CreateRotationX(double angle)
		{
			Matrix3x4d result;
			Matrix3x4d.CreateRotationX(angle, out result);
			return result;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00015948 File Offset: 0x00013B48
		public static void CreateRotationY(double angle, out Matrix3x4d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = 0.0;
			result.Row0.Z = -num2;
			result.Row0.W = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = 1.0;
			result.Row1.Z = 0.0;
			result.Row1.W = 0.0;
			result.Row2.X = num2;
			result.Row2.Y = 0.0;
			result.Row2.Z = num;
			result.Row2.W = 0.0;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00015A34 File Offset: 0x00013C34
		public static Matrix3x4d CreateRotationY(double angle)
		{
			Matrix3x4d result;
			Matrix3x4d.CreateRotationY(angle, out result);
			return result;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00015A4C File Offset: 0x00013C4C
		public static void CreateRotationZ(double angle, out Matrix3x4d result)
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
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row2.Z = 1.0;
			result.Row2.W = 0.0;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00015B38 File Offset: 0x00013D38
		public static Matrix3x4d CreateRotationZ(double angle)
		{
			Matrix3x4d result;
			Matrix3x4d.CreateRotationZ(angle, out result);
			return result;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00015B50 File Offset: 0x00013D50
		public static void CreateTranslation(double x, double y, double z, out Matrix3x4d result)
		{
			result.Row0.X = 1.0;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row0.W = x;
			result.Row1.X = 0.0;
			result.Row1.Y = 1.0;
			result.Row1.Z = 0.0;
			result.Row1.W = y;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row2.Z = 1.0;
			result.Row2.W = z;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00015C38 File Offset: 0x00013E38
		public static void CreateTranslation(in Vector3d vector, out Matrix3x4d result)
		{
			result.Row0.X = 1.0;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row0.W = vector.X;
			result.Row1.X = 0.0;
			result.Row1.Y = 1.0;
			result.Row1.Z = 0.0;
			result.Row1.W = vector.Y;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row2.Z = 1.0;
			result.Row2.W = vector.Z;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00015D2C File Offset: 0x00013F2C
		public static Matrix3x4d CreateTranslation(double x, double y, double z)
		{
			Matrix3x4d result;
			Matrix3x4d.CreateTranslation(x, y, z, out result);
			return result;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00015D44 File Offset: 0x00013F44
		public static Matrix3x4d CreateTranslation(Vector3d vector)
		{
			Matrix3x4d result;
			Matrix3x4d.CreateTranslation(vector.X, vector.Y, vector.Z, out result);
			return result;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00015D6B File Offset: 0x00013F6B
		public static Matrix3x4d CreateScale(double scale)
		{
			return Matrix3x4d.CreateScale(scale, scale, scale);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00015D75 File Offset: 0x00013F75
		public static Matrix3x4d CreateScale(Vector3d scale)
		{
			return Matrix3x4d.CreateScale(scale.X, scale.Y, scale.Z);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00015D90 File Offset: 0x00013F90
		public static Matrix3x4d CreateScale(double x, double y, double z)
		{
			Matrix3x4d result;
			result.Row0.X = x;
			result.Row0.Y = 0.0;
			result.Row0.Z = 0.0;
			result.Row0.W = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = y;
			result.Row1.Z = 0.0;
			result.Row1.W = 0.0;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
			result.Row2.Z = z;
			result.Row2.W = 0.0;
			return result;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00015E84 File Offset: 0x00014084
		public static Matrix3d Mult(Matrix3x4d left, Matrix4x3d right)
		{
			Matrix3d result;
			Matrix3x4d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00015EA0 File Offset: 0x000140A0
		public static void Mult(in Matrix3x4d left, in Matrix4x3d right, out Matrix3d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double w = left.Row0.W;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
			double w2 = left.Row1.W;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double z3 = left.Row2.Z;
			double w3 = left.Row2.W;
			double x4 = right.Row0.X;
			double y4 = right.Row0.Y;
			double z4 = right.Row0.Z;
			double x5 = right.Row1.X;
			double y5 = right.Row1.Y;
			double z5 = right.Row1.Z;
			double x6 = right.Row2.X;
			double y6 = right.Row2.Y;
			double z6 = right.Row2.Z;
			double x7 = right.Row3.X;
			double y7 = right.Row3.Y;
			double z7 = right.Row3.Z;
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

		// Token: 0x0600061D RID: 1565 RVA: 0x00016108 File Offset: 0x00014308
		public static Matrix3x4d Mult(Matrix3x4d left, Matrix3x4d right)
		{
			Matrix3x4d result;
			Matrix3x4d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00016124 File Offset: 0x00014324
		public static void Mult(in Matrix3x4d left, in Matrix3x4d right, out Matrix3x4d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double z = left.Row0.Z;
			double w = left.Row0.W;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double z2 = left.Row1.Z;
			double w2 = left.Row1.W;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double z3 = left.Row2.Z;
			double w3 = left.Row2.W;
			double x4 = right.Row0.X;
			double y4 = right.Row0.Y;
			double z4 = right.Row0.Z;
			double w4 = right.Row0.W;
			double x5 = right.Row1.X;
			double y5 = right.Row1.Y;
			double z5 = right.Row1.Z;
			double w5 = right.Row1.W;
			double x6 = right.Row2.X;
			double y6 = right.Row2.Y;
			double z6 = right.Row2.Z;
			double w6 = right.Row2.W;
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

		// Token: 0x0600061F RID: 1567 RVA: 0x000163B4 File Offset: 0x000145B4
		public static Matrix3x4d Mult(Matrix3x4d left, double right)
		{
			Matrix3x4d result;
			Matrix3x4d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000163CC File Offset: 0x000145CC
		public static void Mult(in Matrix3x4d left, double right, out Matrix3x4d result)
		{
			result.Row0 = left.Row0 * right;
			result.Row1 = left.Row1 * right;
			result.Row2 = left.Row2 * right;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00016404 File Offset: 0x00014604
		public static Matrix3x4d Add(Matrix3x4d left, Matrix3x4d right)
		{
			Matrix3x4d result;
			Matrix3x4d.Add(left, right, out result);
			return result;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00016420 File Offset: 0x00014620
		public static void Add(in Matrix3x4d left, in Matrix3x4d right, out Matrix3x4d result)
		{
			result.Row0 = left.Row0 + right.Row0;
			result.Row1 = left.Row1 + right.Row1;
			result.Row2 = left.Row2 + right.Row2;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00016474 File Offset: 0x00014674
		public static Matrix3x4d Subtract(Matrix3x4d left, Matrix3x4d right)
		{
			Matrix3x4d result;
			Matrix3x4d.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00016490 File Offset: 0x00014690
		public static void Subtract(in Matrix3x4d left, in Matrix3x4d right, out Matrix3x4d result)
		{
			result.Row0 = left.Row0 - right.Row0;
			result.Row1 = left.Row1 - right.Row1;
			result.Row2 = left.Row2 - right.Row2;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000164E4 File Offset: 0x000146E4
		public static Matrix3x4d Invert(Matrix3x4d mat)
		{
			Matrix3x4d result;
			Matrix3x4d.Invert(mat, out result);
			return result;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000164FC File Offset: 0x000146FC
		public static void Invert(in Matrix3x4d mat, out Matrix3x4d result)
		{
			Matrix3d matrix3d = new Matrix3d(mat.Column0, mat.Column1, mat.Column2);
			matrix3d.Row0 /= matrix3d.Row0.LengthSquared;
			matrix3d.Row1 /= matrix3d.Row1.LengthSquared;
			matrix3d.Row2 /= matrix3d.Row2.LengthSquared;
			Vector3d right = new Vector3d(mat.Row0.W, mat.Row1.W, mat.Row2.W);
			result.Row0 = new Vector4d(matrix3d.Row0, -Vector3d.Dot(matrix3d.Row0, right));
			result.Row1 = new Vector4d(matrix3d.Row1, -Vector3d.Dot(matrix3d.Row1, right));
			result.Row2 = new Vector4d(matrix3d.Row2, -Vector3d.Dot(matrix3d.Row2, right));
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001660D File Offset: 0x0001480D
		public static Matrix4x3d Transpose(Matrix3x4d mat)
		{
			return new Matrix4x3d(mat.Column0, mat.Column1, mat.Column2, mat.Column3);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00016630 File Offset: 0x00014830
		public static void Transpose(in Matrix3x4d mat, out Matrix4x3d result)
		{
			result.Row0 = mat.Column0;
			result.Row1 = mat.Column1;
			result.Row2 = mat.Column2;
			result.Row3 = mat.Column3;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00016664 File Offset: 0x00014864
		public static Matrix3x4d Swizzle(Matrix3x4d mat, int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3x4d result;
			Matrix3x4d.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, out result);
			return result;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00016680 File Offset: 0x00014880
		public static void Swizzle(in Matrix3x4d mat, int rowForRow0, int rowForRow1, int rowForRow2, out Matrix3x4d result)
		{
			Vector4d vector4d;
			switch (rowForRow0)
			{
			case 0:
				vector4d = mat.Row0;
				break;
			case 1:
				vector4d = mat.Row1;
				break;
			case 2:
				vector4d = mat.Row2;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow0", rowForRow0));
			}
			result.Row0 = vector4d;
			switch (rowForRow1)
			{
			case 0:
				vector4d = mat.Row0;
				break;
			case 1:
				vector4d = mat.Row1;
				break;
			case 2:
				vector4d = mat.Row2;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow1", rowForRow1));
			}
			result.Row1 = vector4d;
			switch (rowForRow2)
			{
			case 0:
				vector4d = mat.Row0;
				break;
			case 1:
				vector4d = mat.Row1;
				break;
			case 2:
				vector4d = mat.Row2;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow2", rowForRow2));
			}
			result.Row2 = vector4d;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00016783 File Offset: 0x00014983
		public static Matrix3d operator *(Matrix3x4d left, Matrix4x3d right)
		{
			return Matrix3x4d.Mult(left, right);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001678C File Offset: 0x0001498C
		public static Matrix3x4d operator *(Matrix3x4d left, Matrix3x4d right)
		{
			return Matrix3x4d.Mult(left, right);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00016795 File Offset: 0x00014995
		public static Matrix3x4d operator *(Matrix3x4d left, double right)
		{
			return Matrix3x4d.Mult(left, right);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001679E File Offset: 0x0001499E
		public static Matrix3x4d operator +(Matrix3x4d left, Matrix3x4d right)
		{
			return Matrix3x4d.Add(left, right);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000167A7 File Offset: 0x000149A7
		public static Matrix3x4d operator -(Matrix3x4d left, Matrix3x4d right)
		{
			return Matrix3x4d.Subtract(left, right);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000167B0 File Offset: 0x000149B0
		public static bool operator ==(Matrix3x4d left, Matrix3x4d right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x000167BA File Offset: 0x000149BA
		public static bool operator !=(Matrix3x4d left, Matrix3x4d right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x000167C7 File Offset: 0x000149C7
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000167D1 File Offset: 0x000149D1
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000167DB File Offset: 0x000149DB
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000167E8 File Offset: 0x000149E8
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

		// Token: 0x06000636 RID: 1590 RVA: 0x00016846 File Offset: 0x00014A46
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector4d, Vector4d, Vector4d>(this.Row0, this.Row1, this.Row2);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001685F File Offset: 0x00014A5F
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix3x4d && this.Equals((Matrix3x4d)obj);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00016877 File Offset: 0x00014A77
		public readonly bool Equals(Matrix3x4d other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2;
		}

		// Token: 0x04000062 RID: 98
		public Vector4d Row0;

		// Token: 0x04000063 RID: 99
		public Vector4d Row1;

		// Token: 0x04000064 RID: 100
		public Vector4d Row2;

		// Token: 0x04000065 RID: 101
		public static readonly Matrix3x4d Zero = new Matrix3x4d(Vector4d.Zero, Vector4d.Zero, Vector4d.Zero);
	}
}
