using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000019 RID: 25
	[Serializable]
	public struct Matrix3x2d : IEquatable<Matrix3x2d>, IFormattable
	{
		// Token: 0x0600053C RID: 1340 RVA: 0x00012A11 File Offset: 0x00010C11
		public Matrix3x2d(Vector2d row0, Vector2d row1, Vector2d row2)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00012A28 File Offset: 0x00010C28
		public Matrix3x2d(double m00, double m01, double m10, double m11, double m20, double m21)
		{
			this.Row0 = new Vector2d(m00, m01);
			this.Row1 = new Vector2d(m10, m11);
			this.Row2 = new Vector2d(m20, m21);
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00012A54 File Offset: 0x00010C54
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x00012A7C File Offset: 0x00010C7C
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x00012AB1 File Offset: 0x00010CB1
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x00012AD9 File Offset: 0x00010CD9
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

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00012B0E File Offset: 0x00010D0E
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x00012B1B File Offset: 0x00010D1B
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

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00012B29 File Offset: 0x00010D29
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x00012B36 File Offset: 0x00010D36
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

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00012B44 File Offset: 0x00010D44
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x00012B51 File Offset: 0x00010D51
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

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00012B5F File Offset: 0x00010D5F
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x00012B6C File Offset: 0x00010D6C
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

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00012B7A File Offset: 0x00010D7A
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x00012B87 File Offset: 0x00010D87
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

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00012B95 File Offset: 0x00010D95
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x00012BA2 File Offset: 0x00010DA2
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

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00012BB0 File Offset: 0x00010DB0
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x00012BCD File Offset: 0x00010DCD
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

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00012BF1 File Offset: 0x00010DF1
		public readonly double Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y;
			}
		}

		// Token: 0x1700013A RID: 314
		public double this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 3 && columnIndex < 2)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0.0;
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

		// Token: 0x06000553 RID: 1363 RVA: 0x00012C7C File Offset: 0x00010E7C
		[MethodImpl(256)]
		private readonly ref Vector2d GetRowUnsafe(in Matrix3x2d m, int index)
		{
			return Unsafe.Add<Vector2d>(Unsafe.AsRef<Vector2d>(ref m.Row0), index);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00012C8F File Offset: 0x00010E8F
		public readonly Matrix2x3d Transposed()
		{
			return Matrix3x2d.Transpose(this);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00012C9C File Offset: 0x00010E9C
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			this = Matrix3x2d.Swizzle(this, rowForRow0, rowForRow1, rowForRow2);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00012CB4 File Offset: 0x00010EB4
		public readonly Matrix3x2d Swizzled(int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3x2d result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2);
			return result;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00012CD4 File Offset: 0x00010ED4
		public static void CreateRotation(double angle, out Matrix3x2d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row1.X = -num2;
			result.Row1.Y = num;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00012D48 File Offset: 0x00010F48
		public static Matrix3x2d CreateRotation(double angle)
		{
			Matrix3x2d result;
			Matrix3x2d.CreateRotation(angle, out result);
			return result;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00012D60 File Offset: 0x00010F60
		public static void CreateScale(double scale, out Matrix3x2d result)
		{
			result.Row0.X = scale;
			result.Row0.Y = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = scale;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00012DD8 File Offset: 0x00010FD8
		public static Matrix3x2d CreateScale(double scale)
		{
			Matrix3x2d result;
			Matrix3x2d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00012DF0 File Offset: 0x00010FF0
		public static void CreateScale(Vector2d scale, out Matrix3x2d result)
		{
			result.Row0.X = scale.X;
			result.Row0.Y = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = scale.Y;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00012E70 File Offset: 0x00011070
		public static Matrix3x2d CreateScale(Vector2d scale)
		{
			Matrix3x2d result;
			Matrix3x2d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00012E88 File Offset: 0x00011088
		public static void CreateScale(double x, double y, out Matrix3x2d result)
		{
			result.Row0.X = x;
			result.Row0.Y = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = y;
			result.Row2.X = 0.0;
			result.Row2.Y = 0.0;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00012F00 File Offset: 0x00011100
		public static Matrix3x2d CreateScale(double x, double y)
		{
			Matrix3x2d result;
			Matrix3x2d.CreateScale(x, y, out result);
			return result;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00012F18 File Offset: 0x00011118
		public static void Mult(in Matrix3x2d left, double right, out Matrix3x2d result)
		{
			result.Row0.X = left.Row0.X * right;
			result.Row0.Y = left.Row0.Y * right;
			result.Row1.X = left.Row1.X * right;
			result.Row1.Y = left.Row1.Y * right;
			result.Row2.X = left.Row2.X * right;
			result.Row2.Y = left.Row2.Y * right;
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00012FB8 File Offset: 0x000111B8
		public static Matrix3x2d Mult(Matrix3x2d left, double right)
		{
			Matrix3x2d result;
			Matrix3x2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00012FD0 File Offset: 0x000111D0
		public static void Mult(in Matrix3x2d left, in Matrix2d right, out Matrix3x2d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double x4 = right.Row0.X;
			double y4 = right.Row0.Y;
			double x5 = right.Row1.X;
			double y5 = right.Row1.Y;
			result.Row0.X = x * x4 + y * x5;
			result.Row0.Y = x * y4 + y * y5;
			result.Row1.X = x2 * x4 + y2 * x5;
			result.Row1.Y = x2 * y4 + y2 * y5;
			result.Row2.X = x3 * x4 + y3 * x5;
			result.Row2.Y = x3 * y4 + y3 * y5;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000130D8 File Offset: 0x000112D8
		public static Matrix3x2d Mult(Matrix3x2d left, Matrix2d right)
		{
			Matrix3x2d result;
			Matrix3x2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000130F4 File Offset: 0x000112F4
		public static void Mult(in Matrix3x2d left, in Matrix2x3d right, out Matrix3d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double x4 = right.Row0.X;
			double y4 = right.Row0.Y;
			double z = right.Row0.Z;
			double x5 = right.Row1.X;
			double y5 = right.Row1.Y;
			double z2 = right.Row1.Z;
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

		// Token: 0x06000564 RID: 1380 RVA: 0x00013254 File Offset: 0x00011454
		public static Matrix3d Mult(Matrix3x2d left, Matrix2x3d right)
		{
			Matrix3d result;
			Matrix3x2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00013270 File Offset: 0x00011470
		public static void Mult(in Matrix3x2d left, in Matrix2x4d right, out Matrix3x4d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double x3 = left.Row2.X;
			double y3 = left.Row2.Y;
			double x4 = right.Row0.X;
			double y4 = right.Row0.Y;
			double z = right.Row0.Z;
			double w = right.Row0.W;
			double x5 = right.Row1.X;
			double y5 = right.Row1.Y;
			double z2 = right.Row1.Z;
			double w2 = right.Row1.W;
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

		// Token: 0x06000566 RID: 1382 RVA: 0x00013428 File Offset: 0x00011628
		public static Matrix3x4d Mult(Matrix3x2d left, Matrix2x4d right)
		{
			Matrix3x4d result;
			Matrix3x2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00013444 File Offset: 0x00011644
		public static void Add(in Matrix3x2d left, in Matrix3x2d right, out Matrix3x2d result)
		{
			result.Row0.X = left.Row0.X + right.Row0.X;
			result.Row0.Y = left.Row0.Y + right.Row0.Y;
			result.Row1.X = left.Row1.X + right.Row1.X;
			result.Row1.Y = left.Row1.Y + right.Row1.Y;
			result.Row2.X = left.Row2.X + right.Row2.X;
			result.Row2.Y = left.Row2.Y + right.Row2.Y;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00013520 File Offset: 0x00011720
		public static Matrix3x2d Add(Matrix3x2d left, Matrix3x2d right)
		{
			Matrix3x2d result;
			Matrix3x2d.Add(left, right, out result);
			return result;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001353C File Offset: 0x0001173C
		public static void Subtract(in Matrix3x2d left, in Matrix3x2d right, out Matrix3x2d result)
		{
			result.Row0.X = left.Row0.X - right.Row0.X;
			result.Row0.Y = left.Row0.Y - right.Row0.Y;
			result.Row1.X = left.Row1.X - right.Row1.X;
			result.Row1.Y = left.Row1.Y - right.Row1.Y;
			result.Row2.X = left.Row2.X - right.Row2.X;
			result.Row2.Y = left.Row2.Y - right.Row2.Y;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00013618 File Offset: 0x00011818
		public static Matrix3x2d Subtract(Matrix3x2d left, Matrix3x2d right)
		{
			Matrix3x2d result;
			Matrix3x2d.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00013634 File Offset: 0x00011834
		public static void Transpose(in Matrix3x2d mat, out Matrix2x3d result)
		{
			result.Row0.X = mat.Row0.X;
			result.Row0.Y = mat.Row1.X;
			result.Row0.Z = mat.Row2.X;
			result.Row1.X = mat.Row0.Y;
			result.Row1.Y = mat.Row1.Y;
			result.Row1.Z = mat.Row2.Y;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000136C8 File Offset: 0x000118C8
		public static Matrix2x3d Transpose(Matrix3x2d mat)
		{
			Matrix2x3d result;
			Matrix3x2d.Transpose(mat, out result);
			return result;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x000136E0 File Offset: 0x000118E0
		public static Matrix3x2d Swizzle(Matrix3x2d mat, int rowForRow0, int rowForRow1, int rowForRow2)
		{
			Matrix3x2d result;
			Matrix3x2d.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, out result);
			return result;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000136FC File Offset: 0x000118FC
		public static void Swizzle(in Matrix3x2d mat, int rowForRow0, int rowForRow1, int rowForRow2, out Matrix3x2d result)
		{
			Vector2d vector2d;
			switch (rowForRow0)
			{
			case 0:
				vector2d = mat.Row0;
				break;
			case 1:
				vector2d = mat.Row1;
				break;
			case 2:
				vector2d = mat.Row2;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow0", rowForRow0));
			}
			result.Row0 = vector2d;
			switch (rowForRow1)
			{
			case 0:
				vector2d = mat.Row0;
				break;
			case 1:
				vector2d = mat.Row1;
				break;
			case 2:
				vector2d = mat.Row2;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow1", rowForRow1));
			}
			result.Row1 = vector2d;
			switch (rowForRow2)
			{
			case 0:
				vector2d = mat.Row0;
				break;
			case 1:
				vector2d = mat.Row1;
				break;
			case 2:
				vector2d = mat.Row2;
				break;
			default:
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 2. Got {1}.", "rowForRow2", rowForRow2));
			}
			result.Row2 = vector2d;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000137FF File Offset: 0x000119FF
		public static Matrix3x2d operator *(double left, Matrix3x2d right)
		{
			return Matrix3x2d.Mult(right, left);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00013808 File Offset: 0x00011A08
		public static Matrix3x2d operator *(Matrix3x2d left, double right)
		{
			return Matrix3x2d.Mult(left, right);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00013811 File Offset: 0x00011A11
		public static Matrix3x2d operator *(Matrix3x2d left, Matrix2d right)
		{
			return Matrix3x2d.Mult(left, right);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001381A File Offset: 0x00011A1A
		public static Matrix3d operator *(Matrix3x2d left, Matrix2x3d right)
		{
			return Matrix3x2d.Mult(left, right);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00013823 File Offset: 0x00011A23
		public static Matrix3x4d operator *(Matrix3x2d left, Matrix2x4d right)
		{
			return Matrix3x2d.Mult(left, right);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001382C File Offset: 0x00011A2C
		public static Matrix3x2d operator +(Matrix3x2d left, Matrix3x2d right)
		{
			return Matrix3x2d.Add(left, right);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00013835 File Offset: 0x00011A35
		public static Matrix3x2d operator -(Matrix3x2d left, Matrix3x2d right)
		{
			return Matrix3x2d.Subtract(left, right);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001383E File Offset: 0x00011A3E
		public static bool operator ==(Matrix3x2d left, Matrix3x2d right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00013848 File Offset: 0x00011A48
		public static bool operator !=(Matrix3x2d left, Matrix3x2d right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00013855 File Offset: 0x00011A55
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001385F File Offset: 0x00011A5F
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00013869 File Offset: 0x00011A69
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00013874 File Offset: 0x00011A74
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

		// Token: 0x0600057C RID: 1404 RVA: 0x000138D2 File Offset: 0x00011AD2
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector2d, Vector2d, Vector2d>(this.Row0, this.Row1, this.Row2);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x000138EB File Offset: 0x00011AEB
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix3x2d && this.Equals((Matrix3x2d)obj);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00013903 File Offset: 0x00011B03
		public readonly bool Equals(Matrix3x2d other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2;
		}

		// Token: 0x0400005A RID: 90
		public Vector2d Row0;

		// Token: 0x0400005B RID: 91
		public Vector2d Row1;

		// Token: 0x0400005C RID: 92
		public Vector2d Row2;

		// Token: 0x0400005D RID: 93
		public static readonly Matrix3x2d Zero = new Matrix3x2d(Vector2d.Zero, Vector2d.Zero, Vector2d.Zero);
	}
}
