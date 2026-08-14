using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	public struct Matrix2 : IEquatable<Matrix2>, IFormattable
	{
		// Token: 0x06000298 RID: 664 RVA: 0x000091F0 File Offset: 0x000073F0
		public Matrix2(Vector2 row0, Vector2 row1)
		{
			this.Row0 = row0;
			this.Row1 = row1;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00009200 File Offset: 0x00007400
		public Matrix2(float m00, float m01, float m10, float m11)
		{
			this.Row0 = new Vector2(m00, m01);
			this.Row1 = new Vector2(m10, m11);
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00009220 File Offset: 0x00007420
		public readonly float Determinant
		{
			get
			{
				float x = this.Row0.X;
				float y = this.Row0.Y;
				float x2 = this.Row1.X;
				float y2 = this.Row1.Y;
				return x * y2 - y * x2;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600029B RID: 667 RVA: 0x00009262 File Offset: 0x00007462
		// (set) Token: 0x0600029C RID: 668 RVA: 0x0000927F File Offset: 0x0000747F
		public Vector2 Column0
		{
			readonly get
			{
				return new Vector2(this.Row0.X, this.Row1.X);
			}
			set
			{
				this.Row0.X = value.X;
				this.Row1.X = value.Y;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600029D RID: 669 RVA: 0x000092A3 File Offset: 0x000074A3
		// (set) Token: 0x0600029E RID: 670 RVA: 0x000092C0 File Offset: 0x000074C0
		public Vector2 Column1
		{
			readonly get
			{
				return new Vector2(this.Row0.Y, this.Row1.Y);
			}
			set
			{
				this.Row0.Y = value.X;
				this.Row1.Y = value.Y;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600029F RID: 671 RVA: 0x000092E4 File Offset: 0x000074E4
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x000092F1 File Offset: 0x000074F1
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

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x000092FF File Offset: 0x000074FF
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0000930C File Offset: 0x0000750C
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

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000931A File Offset: 0x0000751A
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x00009327 File Offset: 0x00007527
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

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00009335 File Offset: 0x00007535
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x00009342 File Offset: 0x00007542
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

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00009350 File Offset: 0x00007550
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x0000936D File Offset: 0x0000756D
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

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x00009391 File Offset: 0x00007591
		public readonly float Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y;
			}
		}

		// Token: 0x170000C4 RID: 196
		public float this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 2 && columnIndex < 2)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0f;
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

		// Token: 0x060002AC RID: 684 RVA: 0x00009418 File Offset: 0x00007618
		[MethodImpl(256)]
		private readonly ref Vector2 GetRowUnsafe(in Matrix2 m, int index)
		{
			return Unsafe.Add<Vector2>(Unsafe.AsRef<Vector2>(ref m.Row0), index);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000942B File Offset: 0x0000762B
		public void Invert()
		{
			this = Matrix2.Invert(this);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00009440 File Offset: 0x00007640
		public readonly Matrix2 Inverted()
		{
			Matrix2 result = this;
			result.Invert();
			return result;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000945C File Offset: 0x0000765C
		public void Transpose()
		{
			this = Matrix2.Transpose(this);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00009470 File Offset: 0x00007670
		public readonly Matrix2 Transposed()
		{
			Matrix2 result = this;
			result.Transpose();
			return result;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000948C File Offset: 0x0000768C
		public void Swizzle(int rowForRow0, int rowForRow1)
		{
			this = Matrix2.Swizzle(this, rowForRow0, rowForRow1);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000094A4 File Offset: 0x000076A4
		public readonly Matrix2 Swizzled(int rowForRow0, int rowForRow1)
		{
			Matrix2 result = this;
			result.Swizzle(rowForRow0, rowForRow1);
			return result;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000094C4 File Offset: 0x000076C4
		public static void CreateRotation(float angle, out Matrix2 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row1.X = -num2;
			result.Row1.Y = num;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00009510 File Offset: 0x00007710
		public static Matrix2 CreateRotation(float angle)
		{
			Matrix2 result;
			Matrix2.CreateRotation(angle, out result);
			return result;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00009526 File Offset: 0x00007726
		public static void CreateScale(float scale, out Matrix2 result)
		{
			result.Row0.X = scale;
			result.Row0.Y = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = scale;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00009560 File Offset: 0x00007760
		public static Matrix2 CreateScale(float scale)
		{
			Matrix2 result;
			Matrix2.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00009578 File Offset: 0x00007778
		public static void CreateScale(Vector2 scale, out Matrix2 result)
		{
			result.Row0.X = scale.X;
			result.Row0.Y = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = scale.Y;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000095C8 File Offset: 0x000077C8
		public static Matrix2 CreateScale(Vector2 scale)
		{
			Matrix2 result;
			Matrix2.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000095DE File Offset: 0x000077DE
		public static void CreateScale(float x, float y, out Matrix2 result)
		{
			result.Row0.X = x;
			result.Row0.Y = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = y;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00009618 File Offset: 0x00007818
		public static Matrix2 CreateScale(float x, float y)
		{
			Matrix2 result;
			Matrix2.CreateScale(x, y, out result);
			return result;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00009630 File Offset: 0x00007830
		public static Matrix2 CreateSwizzle(int rowForRow0, int rowForRow1)
		{
			Matrix2 result;
			Matrix2.CreateSwizzle(rowForRow0, rowForRow1, out result);
			return result;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00009648 File Offset: 0x00007848
		public static void CreateSwizzle(int rowForRow0, int rowForRow1, out Matrix2 result)
		{
			if (rowForRow0 < 0 || rowForRow0 >= 2)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow0", rowForRow0));
			}
			if (rowForRow1 < 0 || rowForRow1 >= 2)
			{
				throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow1", rowForRow1));
			}
			result = Matrix2.Zero;
			result[0, rowForRow0] = 1f;
			result[1, rowForRow1] = 1f;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000096C0 File Offset: 0x000078C0
		public static void Mult(in Matrix2 left, float right, out Matrix2 result)
		{
			result.Row0.X = left.Row0.X * right;
			result.Row0.Y = left.Row0.Y * right;
			result.Row1.X = left.Row1.X * right;
			result.Row1.Y = left.Row1.Y * right;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00009730 File Offset: 0x00007930
		public static Matrix2 Mult(Matrix2 left, float right)
		{
			Matrix2 result;
			Matrix2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00009748 File Offset: 0x00007948
		public static void Mult(in Matrix2 left, in Matrix2 right, out Matrix2 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float x3 = right.Row0.X;
			float y3 = right.Row0.Y;
			float x4 = right.Row1.X;
			float y4 = right.Row1.Y;
			result.Row0.X = x * x3 + y * x4;
			result.Row0.Y = x * y3 + y * y4;
			result.Row1.X = x2 * x3 + y2 * x4;
			result.Row1.Y = x2 * y3 + y2 * y4;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000980C File Offset: 0x00007A0C
		public static Matrix2 Mult(Matrix2 left, Matrix2 right)
		{
			Matrix2 result;
			Matrix2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00009828 File Offset: 0x00007A28
		public static void Mult(in Matrix2 left, in Matrix2x3 right, out Matrix2x3 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float x3 = right.Row0.X;
			float y3 = right.Row0.Y;
			float z = right.Row0.Z;
			float x4 = right.Row1.X;
			float y4 = right.Row1.Y;
			float z2 = right.Row1.Z;
			result.Row0.X = x * x3 + y * x4;
			result.Row0.Y = x * y3 + y * y4;
			result.Row0.Z = x * z + y * z2;
			result.Row1.X = x2 * x3 + y2 * x4;
			result.Row1.Y = x2 * y3 + y2 * y4;
			result.Row1.Z = x2 * z + y2 * z2;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000992C File Offset: 0x00007B2C
		public static Matrix2x3 Mult(Matrix2 left, Matrix2x3 right)
		{
			Matrix2x3 result;
			Matrix2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00009948 File Offset: 0x00007B48
		public static void Mult(in Matrix2 left, in Matrix2x4 right, out Matrix2x4 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float x3 = right.Row0.X;
			float y3 = right.Row0.Y;
			float z = right.Row0.Z;
			float w = right.Row0.W;
			float x4 = right.Row1.X;
			float y4 = right.Row1.Y;
			float z2 = right.Row1.Z;
			float w2 = right.Row1.W;
			result.Row0.X = x * x3 + y * x4;
			result.Row0.Y = x * y3 + y * y4;
			result.Row0.Z = x * z + y * z2;
			result.Row0.W = x * w + y * w2;
			result.Row1.X = x2 * x3 + y2 * x4;
			result.Row1.Y = x2 * y3 + y2 * y4;
			result.Row1.Z = x2 * z + y2 * z2;
			result.Row1.W = x2 * w + y2 * w2;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00009A90 File Offset: 0x00007C90
		public static Matrix2x4 Mult(Matrix2 left, Matrix2x4 right)
		{
			Matrix2x4 result;
			Matrix2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00009AAC File Offset: 0x00007CAC
		public static void Add(in Matrix2 left, in Matrix2 right, out Matrix2 result)
		{
			result.Row0.X = left.Row0.X + right.Row0.X;
			result.Row0.Y = left.Row0.Y + right.Row0.Y;
			result.Row1.X = left.Row1.X + right.Row1.X;
			result.Row1.Y = left.Row1.Y + right.Row1.Y;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00009B44 File Offset: 0x00007D44
		public static Matrix2 Add(Matrix2 left, Matrix2 right)
		{
			Matrix2 result;
			Matrix2.Add(left, right, out result);
			return result;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00009B60 File Offset: 0x00007D60
		public static void Subtract(in Matrix2 left, in Matrix2 right, out Matrix2 result)
		{
			result.Row0.X = left.Row0.X - right.Row0.X;
			result.Row0.Y = left.Row0.Y - right.Row0.Y;
			result.Row1.X = left.Row1.X - right.Row1.X;
			result.Row1.Y = left.Row1.Y - right.Row1.Y;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00009BF8 File Offset: 0x00007DF8
		public static Matrix2 Subtract(Matrix2 left, Matrix2 right)
		{
			Matrix2 result;
			Matrix2.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00009C14 File Offset: 0x00007E14
		public static void Invert(in Matrix2 mat, out Matrix2 result)
		{
			float num = mat.Row0.X * mat.Row1.Y - mat.Row0.Y * mat.Row1.X;
			if (num == 0f)
			{
				throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
			}
			float num2 = 1f / num;
			float x = mat.Row0.X;
			result.Row0.X = mat.Row1.Y * num2;
			result.Row0.Y = -mat.Row0.Y * num2;
			result.Row1.X = -mat.Row1.X * num2;
			result.Row1.Y = x * num2;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00009CD0 File Offset: 0x00007ED0
		public static Matrix2 Invert(Matrix2 mat)
		{
			Matrix2 result;
			Matrix2.Invert(mat, out result);
			return result;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00009CE8 File Offset: 0x00007EE8
		public static void Transpose(in Matrix2 mat, out Matrix2 result)
		{
			result.Row0.X = mat.Row0.X;
			result.Row0.Y = mat.Row1.X;
			result.Row1.X = mat.Row0.Y;
			result.Row1.Y = mat.Row1.Y;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00009D50 File Offset: 0x00007F50
		public static Matrix2 Transpose(Matrix2 mat)
		{
			Matrix2 result;
			Matrix2.Transpose(mat, out result);
			return result;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00009D68 File Offset: 0x00007F68
		public static Matrix2 Swizzle(Matrix2 mat, int rowForRow0, int rowForRow1)
		{
			Matrix2 result;
			Matrix2.Swizzle(mat, rowForRow0, rowForRow1, out result);
			return result;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00009D84 File Offset: 0x00007F84
		public static void Swizzle(in Matrix2 mat, int rowForRow0, int rowForRow1, out Matrix2 result)
		{
			Vector2 vector;
			if (rowForRow0 != 0)
			{
				if (rowForRow0 != 1)
				{
					throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow0", rowForRow0));
				}
				vector = mat.Row1;
			}
			else
			{
				vector = mat.Row0;
			}
			result.Row0 = vector;
			if (rowForRow1 != 0)
			{
				if (rowForRow1 != 1)
				{
					throw new IndexOutOfRangeException(string.Format("{0} must be a number between 0 and 1. Got {1}.", "rowForRow1", rowForRow1));
				}
				vector = mat.Row1;
			}
			else
			{
				vector = mat.Row0;
			}
			result.Row1 = vector;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00009E0B File Offset: 0x0000800B
		public static Matrix2 operator *(float left, Matrix2 right)
		{
			return Matrix2.Mult(right, left);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00009E14 File Offset: 0x00008014
		public static Matrix2 operator *(Matrix2 left, float right)
		{
			return Matrix2.Mult(left, right);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00009E1D File Offset: 0x0000801D
		public static Matrix2 operator *(Matrix2 left, Matrix2 right)
		{
			return Matrix2.Mult(left, right);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00009E26 File Offset: 0x00008026
		public static Matrix2x3 operator *(Matrix2 left, Matrix2x3 right)
		{
			return Matrix2.Mult(left, right);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00009E2F File Offset: 0x0000802F
		public static Matrix2x4 operator *(Matrix2 left, Matrix2x4 right)
		{
			return Matrix2.Mult(left, right);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00009E38 File Offset: 0x00008038
		public static Matrix2 operator +(Matrix2 left, Matrix2 right)
		{
			return Matrix2.Add(left, right);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00009E41 File Offset: 0x00008041
		public static Matrix2 operator -(Matrix2 left, Matrix2 right)
		{
			return Matrix2.Subtract(left, right);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00009E4A File Offset: 0x0000804A
		public static bool operator ==(Matrix2 left, Matrix2 right)
		{
			return left.Equals(right);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00009E54 File Offset: 0x00008054
		public static bool operator !=(Matrix2 left, Matrix2 right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00009E61 File Offset: 0x00008061
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00009E6B File Offset: 0x0000806B
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00009E75 File Offset: 0x00008075
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00009E80 File Offset: 0x00008080
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			string text = this.Row0.ToString(format, formatProvider);
			string text2 = this.Row1.ToString(format, formatProvider);
			return text + "\n" + text2;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00009EB3 File Offset: 0x000080B3
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector2, Vector2>(this.Row0, this.Row1);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00009EC6 File Offset: 0x000080C6
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix2 && this.Equals((Matrix2)obj);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00009EDE File Offset: 0x000080DE
		public readonly bool Equals(Matrix2 other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1;
		}

		// Token: 0x04000038 RID: 56
		public Vector2 Row0;

		// Token: 0x04000039 RID: 57
		public Vector2 Row1;

		// Token: 0x0400003A RID: 58
		public static readonly Matrix2 Identity = new Matrix2(Vector2.UnitX, Vector2.UnitY);

		// Token: 0x0400003B RID: 59
		public static readonly Matrix2 Zero = new Matrix2(Vector2.Zero, Vector2.Zero);
	}
}
