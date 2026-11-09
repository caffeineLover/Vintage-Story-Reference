using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000011 RID: 17
	[Serializable]
	public struct Matrix2d : IEquatable<Matrix2d>, IFormattable
	{
		// Token: 0x060002E0 RID: 736 RVA: 0x00009F30 File Offset: 0x00008130
		public Matrix2d(Vector2d row0, Vector2d row1)
		{
			this.Row0 = row0;
			this.Row1 = row1;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00009F40 File Offset: 0x00008140
		public Matrix2d(double m00, double m01, double m10, double m11)
		{
			this.Row0 = new Vector2d(m00, m01);
			this.Row1 = new Vector2d(m10, m11);
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00009F60 File Offset: 0x00008160
		public readonly double Determinant
		{
			get
			{
				double x = this.Row0.X;
				double y = this.Row0.Y;
				double x2 = this.Row1.X;
				double y2 = this.Row1.Y;
				return x * y2 - y * x2;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00009FA2 File Offset: 0x000081A2
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x00009FBF File Offset: 0x000081BF
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x00009FE3 File Offset: 0x000081E3
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000A000 File Offset: 0x00008200
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

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000A024 File Offset: 0x00008224
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000A031 File Offset: 0x00008231
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000A03F File Offset: 0x0000823F
		// (set) Token: 0x060002EA RID: 746 RVA: 0x0000A04C File Offset: 0x0000824C
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

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000A05A File Offset: 0x0000825A
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0000A067 File Offset: 0x00008267
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000A075 File Offset: 0x00008275
		// (set) Token: 0x060002EE RID: 750 RVA: 0x0000A082 File Offset: 0x00008282
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000A090 File Offset: 0x00008290
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x0000A0AD File Offset: 0x000082AD
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

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000A0D1 File Offset: 0x000082D1
		public readonly double Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y;
			}
		}

		// Token: 0x170000CE RID: 206
		public double this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 2 && columnIndex < 2)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0.0;
			}
			set
			{
				if (rowIndex < 2 && columnIndex < 2)
				{
					this.GetRowUnsafe(this, rowIndex)[columnIndex] = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to set this matrix at: ({0}, {1})", rowIndex, columnIndex));
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000A15C File Offset: 0x0000835C
		[MethodImpl(256)]
		private readonly ref Vector2d GetRowUnsafe(in Matrix2d m, int index)
		{
			return Unsafe.Add<Vector2d>(Unsafe.AsRef<Vector2d>(ref m.Row0), index);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000A16F File Offset: 0x0000836F
		public void Invert()
		{
			this = Matrix2d.Invert(this);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000A184 File Offset: 0x00008384
		public readonly Matrix2d Inverted()
		{
			Matrix2d result = this;
			result.Invert();
			return result;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000A1A0 File Offset: 0x000083A0
		public void Transpose()
		{
			this = Matrix2d.Transpose(this);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000A1B4 File Offset: 0x000083B4
		public readonly Matrix2d Transposed()
		{
			Matrix2d result = this;
			result.Transpose();
			return result;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000A1D0 File Offset: 0x000083D0
		public void Swizzle(int rowForRow0, int rowForRow1)
		{
			this = Matrix2d.Swizzle(this, rowForRow0, rowForRow1);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000A1E8 File Offset: 0x000083E8
		public readonly Matrix2d Swizzled(int rowForRow0, int rowForRow1)
		{
			Matrix2d result = this;
			result.Swizzle(rowForRow0, rowForRow1);
			return result;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000A208 File Offset: 0x00008408
		public static void CreateRotation(double angle, out Matrix2d result)
		{
			double num = Math.Cos(angle);
			double num2 = Math.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row1.X = -num2;
			result.Row1.Y = num;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000A254 File Offset: 0x00008454
		public static Matrix2d CreateRotation(double angle)
		{
			Matrix2d result;
			Matrix2d.CreateRotation(angle, out result);
			return result;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000A26C File Offset: 0x0000846C
		public static void CreateScale(double scale, out Matrix2d result)
		{
			result.Row0.X = scale;
			result.Row0.Y = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = scale;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000A2BC File Offset: 0x000084BC
		public static Matrix2d CreateScale(double scale)
		{
			Matrix2d result;
			Matrix2d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000A2D4 File Offset: 0x000084D4
		public static void CreateScale(Vector2d scale, out Matrix2d result)
		{
			result.Row0.X = scale.X;
			result.Row0.Y = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = scale.Y;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000A32C File Offset: 0x0000852C
		public static Matrix2d CreateScale(Vector2d scale)
		{
			Matrix2d result;
			Matrix2d.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000A344 File Offset: 0x00008544
		public static void CreateScale(double x, double y, out Matrix2d result)
		{
			result.Row0.X = x;
			result.Row0.Y = 0.0;
			result.Row1.X = 0.0;
			result.Row1.Y = y;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000A394 File Offset: 0x00008594
		public static Matrix2d CreateScale(double x, double y)
		{
			Matrix2d result;
			Matrix2d.CreateScale(x, y, out result);
			return result;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000A3AC File Offset: 0x000085AC
		public static Matrix2d CreateSwizzle(int rowForRow0, int rowForRow1)
		{
			Matrix2d result;
			Matrix2d.CreateSwizzle(rowForRow0, rowForRow1, out result);
			return result;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000A3C4 File Offset: 0x000085C4
		public static void CreateSwizzle(int rowForRow0, int rowForRow1, out Matrix2d result)
		{
			if (rowForRow0 < 0 || rowForRow0 >= 2)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow0", rowForRow0));
			}
			if (rowForRow1 < 0 || rowForRow1 >= 2)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow1", rowForRow1));
			}
			result = Matrix2d.Zero;
			result[0, rowForRow0] = 1.0;
			result[1, rowForRow1] = 1.0;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000A444 File Offset: 0x00008644
		public static void Mult(in Matrix2d left, double right, out Matrix2d result)
		{
			result.Row0.X = left.Row0.X * right;
			result.Row0.Y = left.Row0.Y * right;
			result.Row1.X = left.Row1.X * right;
			result.Row1.Y = left.Row1.Y * right;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000A4B4 File Offset: 0x000086B4
		public static Matrix2d Mult(Matrix2d left, double right)
		{
			Matrix2d result;
			Matrix2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000A4CC File Offset: 0x000086CC
		public static void Mult(in Matrix2d left, in Matrix2d right, out Matrix2d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double x3 = right.Row0.X;
			double y3 = right.Row0.Y;
			double x4 = right.Row1.X;
			double y4 = right.Row1.Y;
			result.Row0.X = x * x3 + y * x4;
			result.Row0.Y = x * y3 + y * y4;
			result.Row1.X = x2 * x3 + y2 * x4;
			result.Row1.Y = x2 * y3 + y2 * y4;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000A590 File Offset: 0x00008790
		public static Matrix2d Mult(Matrix2d left, Matrix2d right)
		{
			Matrix2d result;
			Matrix2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000A5AC File Offset: 0x000087AC
		public static void Mult(in Matrix2d left, in Matrix2x3d right, out Matrix2x3d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double x3 = right.Row0.X;
			double y3 = right.Row0.Y;
			double z = right.Row0.Z;
			double x4 = right.Row1.X;
			double y4 = right.Row1.Y;
			double z2 = right.Row1.Z;
			result.Row0.X = x * x3 + y * x4;
			result.Row0.Y = x * y3 + y * y4;
			result.Row0.Z = x * z + y * z2;
			result.Row1.X = x2 * x3 + y2 * x4;
			result.Row1.Y = x2 * y3 + y2 * y4;
			result.Row1.Z = x2 * z + y2 * z2;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000A6B0 File Offset: 0x000088B0
		public static Matrix2x3d Mult(Matrix2d left, Matrix2x3d right)
		{
			Matrix2x3d result;
			Matrix2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000A6CC File Offset: 0x000088CC
		public static void Mult(in Matrix2d left, in Matrix2x4d right, out Matrix2x4d result)
		{
			double x = left.Row0.X;
			double y = left.Row0.Y;
			double x2 = left.Row1.X;
			double y2 = left.Row1.Y;
			double x3 = right.Row0.X;
			double y3 = right.Row0.Y;
			double z = right.Row0.Z;
			double w = right.Row0.W;
			double x4 = right.Row1.X;
			double y4 = right.Row1.Y;
			double z2 = right.Row1.Z;
			double w2 = right.Row1.W;
			result.Row0.X = x * x3 + y * x4;
			result.Row0.Y = x * y3 + y * y4;
			result.Row0.Z = x * z + y * z2;
			result.Row0.W = x * w + y * w2;
			result.Row1.X = x2 * x3 + y2 * x4;
			result.Row1.Y = x2 * y3 + y2 * y4;
			result.Row1.Z = x2 * z + y2 * z2;
			result.Row1.W = x2 * w + y2 * w2;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000A814 File Offset: 0x00008A14
		public static Matrix2x4d Mult(Matrix2d left, Matrix2x4d right)
		{
			Matrix2x4d result;
			Matrix2d.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000A830 File Offset: 0x00008A30
		public static void Add(in Matrix2d left, in Matrix2d right, out Matrix2d result)
		{
			result.Row0.X = left.Row0.X + right.Row0.X;
			result.Row0.Y = left.Row0.Y + right.Row0.Y;
			result.Row1.X = left.Row1.X + right.Row1.X;
			result.Row1.Y = left.Row1.Y + right.Row1.Y;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000A8C8 File Offset: 0x00008AC8
		public static Matrix2d Add(Matrix2d left, Matrix2d right)
		{
			Matrix2d result;
			Matrix2d.Add(left, right, out result);
			return result;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000A8E4 File Offset: 0x00008AE4
		public static void Subtract(in Matrix2d left, in Matrix2d right, out Matrix2d result)
		{
			result.Row0.X = left.Row0.X - right.Row0.X;
			result.Row0.Y = left.Row0.Y - right.Row0.Y;
			result.Row1.X = left.Row1.X - right.Row1.X;
			result.Row1.Y = left.Row1.Y - right.Row1.Y;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000A97C File Offset: 0x00008B7C
		public static Matrix2d Subtract(Matrix2d left, Matrix2d right)
		{
			Matrix2d result;
			Matrix2d.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000A998 File Offset: 0x00008B98
		public static void Invert(in Matrix2d mat, out Matrix2d result)
		{
			double num = mat.Row0.X * mat.Row1.Y - mat.Row0.Y * mat.Row1.X;
			if (num == 0.0)
			{
				throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
			}
			double num2 = 1.0 / num;
			double x = mat.Row0.X;
			result.Row0.X = mat.Row1.Y * num2;
			result.Row0.Y = -mat.Row0.Y * num2;
			result.Row1.X = -mat.Row1.X * num2;
			result.Row1.Y = x * num2;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000AA5C File Offset: 0x00008C5C
		public static Matrix2d Invert(Matrix2d mat)
		{
			Matrix2d result;
			Matrix2d.Invert(mat, out result);
			return result;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000AA74 File Offset: 0x00008C74
		public static void Transpose(in Matrix2d mat, out Matrix2d result)
		{
			result.Row0.X = mat.Row0.X;
			result.Row0.Y = mat.Row1.X;
			result.Row1.X = mat.Row0.Y;
			result.Row1.Y = mat.Row1.Y;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000AADC File Offset: 0x00008CDC
		public static Matrix2d Transpose(Matrix2d mat)
		{
			Matrix2d result;
			Matrix2d.Transpose(mat, out result);
			return result;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000AAF4 File Offset: 0x00008CF4
		public static Matrix2d Swizzle(Matrix2d mat, int rowForRow0, int rowForRow1)
		{
			Matrix2d result;
			Matrix2d.Swizzle(mat, rowForRow0, rowForRow1, out result);
			return result;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000AB10 File Offset: 0x00008D10
		public static void Swizzle(in Matrix2d mat, int rowForRow0, int rowForRow1, out Matrix2d result)
		{
			Vector2d vector2d;
			if (rowForRow0 != 0)
			{
				if (rowForRow0 != 1)
				{
					throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow0", rowForRow0));
				}
				vector2d = mat.Row1;
			}
			else
			{
				vector2d = mat.Row0;
			}
			result.Row0 = vector2d;
			if (rowForRow1 != 0)
			{
				if (rowForRow1 != 1)
				{
					throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow1", rowForRow1));
				}
				vector2d = mat.Row1;
			}
			else
			{
				vector2d = mat.Row0;
			}
			result.Row1 = vector2d;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000AB97 File Offset: 0x00008D97
		public static Matrix2d operator *(double left, Matrix2d right)
		{
			return Matrix2d.Mult(right, left);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000ABA0 File Offset: 0x00008DA0
		public static Matrix2d operator *(Matrix2d left, double right)
		{
			return Matrix2d.Mult(left, right);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000ABA9 File Offset: 0x00008DA9
		public static Matrix2d operator *(Matrix2d left, Matrix2d right)
		{
			return Matrix2d.Mult(left, right);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000ABB2 File Offset: 0x00008DB2
		public static Matrix2x3d operator *(Matrix2d left, Matrix2x3d right)
		{
			return Matrix2d.Mult(left, right);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000ABBB File Offset: 0x00008DBB
		public static Matrix2x4d operator *(Matrix2d left, Matrix2x4d right)
		{
			return Matrix2d.Mult(left, right);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000ABC4 File Offset: 0x00008DC4
		public static Matrix2d operator +(Matrix2d left, Matrix2d right)
		{
			return Matrix2d.Add(left, right);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000ABCD File Offset: 0x00008DCD
		public static Matrix2d operator -(Matrix2d left, Matrix2d right)
		{
			return Matrix2d.Subtract(left, right);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000ABD6 File Offset: 0x00008DD6
		public static bool operator ==(Matrix2d left, Matrix2d right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000ABE0 File Offset: 0x00008DE0
		public static bool operator !=(Matrix2d left, Matrix2d right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000ABED File Offset: 0x00008DED
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000ABF7 File Offset: 0x00008DF7
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000AC01 File Offset: 0x00008E01
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000AC0C File Offset: 0x00008E0C
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			string text = this.Row0.ToString(format, formatProvider);
			string text2 = this.Row1.ToString(format, formatProvider);
			return text + "\n" + text2;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000AC3F File Offset: 0x00008E3F
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector2d, Vector2d>(this.Row0, this.Row1);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000AC52 File Offset: 0x00008E52
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix2d && this.Equals((Matrix2d)obj);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000AC6A File Offset: 0x00008E6A
		public readonly bool Equals(Matrix2d other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1;
		}

		// Token: 0x0400003C RID: 60
		public Vector2d Row0;

		// Token: 0x0400003D RID: 61
		public Vector2d Row1;

		// Token: 0x0400003E RID: 62
		public static readonly Matrix2d Identity = new Matrix2d(Vector2d.UnitX, Vector2d.UnitY);

		// Token: 0x0400003F RID: 63
		public static readonly Matrix2d Zero = new Matrix2d(Vector2d.Zero, Vector2d.Zero);
	}
}
