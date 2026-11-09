using System;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x0200001E RID: 30
	[Serializable]
	public struct Matrix4x2 : IEquatable<Matrix4x2>, IFormattable
	{
		// Token: 0x0600074C RID: 1868 RVA: 0x0001C933 File Offset: 0x0001AB33
		public Matrix4x2(Vector2 row0, Vector2 row1, Vector2 row2, Vector2 row3)
		{
			this.Row0 = row0;
			this.Row1 = row1;
			this.Row2 = row2;
			this.Row3 = row3;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001C952 File Offset: 0x0001AB52
		public Matrix4x2(float m00, float m01, float m10, float m11, float m20, float m21, float m30, float m31)
		{
			this.Row0 = new Vector2(m00, m01);
			this.Row1 = new Vector2(m10, m11);
			this.Row2 = new Vector2(m20, m21);
			this.Row3 = new Vector2(m30, m31);
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x0001C98D File Offset: 0x0001AB8D
		// (set) Token: 0x0600074F RID: 1871 RVA: 0x0001C9C0 File Offset: 0x0001ABC0
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

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x0001CA11 File Offset: 0x0001AC11
		// (set) Token: 0x06000751 RID: 1873 RVA: 0x0001CA44 File Offset: 0x0001AC44
		public Vector4 Column1
		{
			readonly get
			{
				return new Vector4(this.Row0.Y, this.Row1.Y, this.Row2.Y, this.Row3.X);
			}
			set
			{
				this.Row0.Y = value.X;
				this.Row1.Y = value.Y;
				this.Row2.Y = value.Z;
				this.Row3.Y = value.W;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x0001CA95 File Offset: 0x0001AC95
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x0001CAA2 File Offset: 0x0001ACA2
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

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0001CAB0 File Offset: 0x0001ACB0
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x0001CABD File Offset: 0x0001ACBD
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

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0001CACB File Offset: 0x0001ACCB
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x0001CAD8 File Offset: 0x0001ACD8
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

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0001CAE6 File Offset: 0x0001ACE6
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x0001CAF3 File Offset: 0x0001ACF3
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

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x0001CB01 File Offset: 0x0001AD01
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x0001CB0E File Offset: 0x0001AD0E
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

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0001CB1C File Offset: 0x0001AD1C
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x0001CB29 File Offset: 0x0001AD29
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

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0001CB37 File Offset: 0x0001AD37
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x0001CB44 File Offset: 0x0001AD44
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

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001CB52 File Offset: 0x0001AD52
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x0001CB5F File Offset: 0x0001AD5F
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0001CB6D File Offset: 0x0001AD6D
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x0001CB8A File Offset: 0x0001AD8A
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

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0001CBAE File Offset: 0x0001ADAE
		public readonly float Trace
		{
			get
			{
				return this.Row0.X + this.Row1.Y;
			}
		}

		// Token: 0x1700019D RID: 413
		public float this[int rowIndex, int columnIndex]
		{
			readonly get
			{
				if (rowIndex < 4 && columnIndex < 2)
				{
					return this.GetRowUnsafe(this, rowIndex)[columnIndex];
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to access this matrix at: ({0}, {1})", rowIndex, columnIndex));
				return 0f;
			}
			set
			{
				if (rowIndex < 4 && columnIndex < 2)
				{
					this.GetRowUnsafe(this, rowIndex)[columnIndex] = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException(string.Format("You tried to set this matrix at: ({0}, {1})", rowIndex, columnIndex));
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0001CC35 File Offset: 0x0001AE35
		[MethodImpl(256)]
		private readonly ref Vector2 GetRowUnsafe(in Matrix4x2 m, int index)
		{
			return Unsafe.Add<Vector2>(Unsafe.AsRef<Vector2>(ref m.Row0), index);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0001CC48 File Offset: 0x0001AE48
		public readonly Matrix2x4 Transposed()
		{
			return Matrix4x2.Transpose(this);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0001CC55 File Offset: 0x0001AE55
		public void Swizzle(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			this = Matrix4x2.Swizzle(this, rowForRow0, rowForRow1, rowForRow2, rowForRow3);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001CC70 File Offset: 0x0001AE70
		public readonly Matrix4x2 Swizzled(int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4x2 result = this;
			result.Swizzle(rowForRow0, rowForRow1, rowForRow2, rowForRow3);
			return result;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001CC94 File Offset: 0x0001AE94
		public static void CreateRotation(float angle, out Matrix4x2 result)
		{
			float num = MathF.Cos(angle);
			float num2 = MathF.Sin(angle);
			result.Row0.X = num;
			result.Row0.Y = num2;
			result.Row1.X = -num2;
			result.Row1.Y = num;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row3.X = 0f;
			result.Row3.Y = 0f;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001CD20 File Offset: 0x0001AF20
		public static Matrix4x2 CreateRotation(float angle)
		{
			Matrix4x2 result;
			Matrix4x2.CreateRotation(angle, out result);
			return result;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001CD38 File Offset: 0x0001AF38
		public static void CreateScale(float scale, out Matrix4x2 result)
		{
			result.Row0.X = scale;
			result.Row0.Y = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = scale;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row3.X = 0f;
			result.Row3.Y = 0f;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001CDC0 File Offset: 0x0001AFC0
		public static Matrix4x2 CreateScale(float scale)
		{
			Matrix4x2 result;
			Matrix4x2.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001CDD8 File Offset: 0x0001AFD8
		public static void CreateScale(Vector2 scale, out Matrix4x2 result)
		{
			result.Row0.X = scale.X;
			result.Row0.Y = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = scale.Y;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row3.X = 0f;
			result.Row3.Y = 0f;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001CE68 File Offset: 0x0001B068
		public static Matrix4x2 CreateScale(Vector2 scale)
		{
			Matrix4x2 result;
			Matrix4x2.CreateScale(scale, out result);
			return result;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0001CE80 File Offset: 0x0001B080
		public static void CreateScale(float x, float y, out Matrix4x2 result)
		{
			result.Row0.X = x;
			result.Row0.Y = 0f;
			result.Row1.X = 0f;
			result.Row1.Y = y;
			result.Row2.X = 0f;
			result.Row2.Y = 0f;
			result.Row3.X = 0f;
			result.Row3.Y = 0f;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0001CF08 File Offset: 0x0001B108
		public static Matrix4x2 CreateScale(float x, float y)
		{
			Matrix4x2 result;
			Matrix4x2.CreateScale(x, y, out result);
			return result;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001CF20 File Offset: 0x0001B120
		public static void Mult(in Matrix4x2 left, float right, out Matrix4x2 result)
		{
			result.Row0.X = left.Row0.X * right;
			result.Row0.Y = left.Row0.Y * right;
			result.Row1.X = left.Row1.X * right;
			result.Row1.Y = left.Row1.Y * right;
			result.Row2.X = left.Row2.X * right;
			result.Row2.Y = left.Row2.Y * right;
			result.Row3.X = left.Row3.X * right;
			result.Row3.Y = left.Row3.Y * right;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001CFF0 File Offset: 0x0001B1F0
		public static Matrix4x2 Mult(Matrix4x2 left, float right)
		{
			Matrix4x2 result;
			Matrix4x2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001D008 File Offset: 0x0001B208
		public static void Mult(in Matrix4x2 left, in Matrix2 right, out Matrix4x2 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float x3 = left.Row2.X;
			float y3 = left.Row2.Y;
			float x4 = left.Row3.X;
			float y4 = left.Row3.Y;
			float x5 = right.Row0.X;
			float y5 = right.Row0.Y;
			float x6 = right.Row1.X;
			float y6 = right.Row1.Y;
			result.Row0.X = x * x5 + y * x6;
			result.Row0.Y = x * y5 + y * y6;
			result.Row1.X = x2 * x5 + y2 * x6;
			result.Row1.Y = x2 * y5 + y2 * y6;
			result.Row2.X = x3 * x5 + y3 * x6;
			result.Row2.Y = x3 * y5 + y3 * y6;
			result.Row3.X = x4 * x5 + y4 * x6;
			result.Row3.Y = x4 * y5 + y4 * y6;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001D158 File Offset: 0x0001B358
		public static Matrix4x2 Mult(Matrix4x2 left, Matrix2 right)
		{
			Matrix4x2 result;
			Matrix4x2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0001D174 File Offset: 0x0001B374
		public static void Mult(in Matrix4x2 left, in Matrix2x3 right, out Matrix4x3 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float x3 = left.Row2.X;
			float y3 = left.Row2.Y;
			float x4 = left.Row3.X;
			float y4 = left.Row3.Y;
			float x5 = right.Row0.X;
			float y5 = right.Row0.Y;
			float z = right.Row0.Z;
			float x6 = right.Row1.X;
			float y6 = right.Row1.Y;
			float z2 = right.Row1.Z;
			result.Row0.X = x * x5 + y * x6;
			result.Row0.Y = x * y5 + y * y6;
			result.Row0.Z = x * z + y * z2;
			result.Row1.X = x2 * x5 + y2 * x6;
			result.Row1.Y = x2 * y5 + y2 * y6;
			result.Row1.Z = x2 * z + y2 * z2;
			result.Row2.X = x3 * x5 + y3 * x6;
			result.Row2.Y = x3 * y5 + y3 * y6;
			result.Row2.Z = x3 * z + y3 * z2;
			result.Row3.X = x4 * x5 + y4 * x6;
			result.Row3.Y = x4 * y5 + y4 * y6;
			result.Row3.Z = x4 * z + y4 * z2;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001D330 File Offset: 0x0001B530
		public static Matrix4x3 Mult(Matrix4x2 left, Matrix2x3 right)
		{
			Matrix4x3 result;
			Matrix4x2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001D34C File Offset: 0x0001B54C
		public static void Mult(in Matrix4x2 left, in Matrix2x4 right, out Matrix4 result)
		{
			float x = left.Row0.X;
			float y = left.Row0.Y;
			float x2 = left.Row1.X;
			float y2 = left.Row1.Y;
			float x3 = left.Row2.X;
			float y3 = left.Row2.Y;
			float x4 = left.Row3.X;
			float y4 = left.Row3.Y;
			float x5 = right.Row0.X;
			float y5 = right.Row0.Y;
			float z = right.Row0.Z;
			float w = right.Row0.W;
			float x6 = right.Row1.X;
			float y6 = right.Row1.Y;
			float z2 = right.Row1.Z;
			float w2 = right.Row1.W;
			result.Row0.X = x * x5 + y * x6;
			result.Row0.Y = x * y5 + y * y6;
			result.Row0.Z = x * z + y * z2;
			result.Row0.W = x * w + y * w2;
			result.Row1.X = x2 * x5 + y2 * x6;
			result.Row1.Y = x2 * y5 + y2 * y6;
			result.Row1.Z = x2 * z + y2 * z2;
			result.Row1.W = x2 * w + y2 * w2;
			result.Row2.X = x3 * x5 + y3 * x6;
			result.Row2.Y = x3 * y5 + y3 * y6;
			result.Row2.Z = x3 * z + y3 * z2;
			result.Row2.W = x3 * w + y3 * w2;
			result.Row3.X = x4 * x5 + y4 * x6;
			result.Row3.Y = x4 * y5 + y4 * y6;
			result.Row3.Z = x4 * z + y4 * z2;
			result.Row3.W = x4 * w + y4 * w2;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001D578 File Offset: 0x0001B778
		public static Matrix4 Mult(Matrix4x2 left, Matrix2x4 right)
		{
			Matrix4 result;
			Matrix4x2.Mult(left, right, out result);
			return result;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001D594 File Offset: 0x0001B794
		public static void Add(in Matrix4x2 left, in Matrix4x2 right, out Matrix4x2 result)
		{
			result.Row0.X = left.Row0.X + right.Row0.X;
			result.Row0.Y = left.Row0.Y + right.Row0.Y;
			result.Row1.X = left.Row1.X + right.Row1.X;
			result.Row1.Y = left.Row1.Y + right.Row1.Y;
			result.Row2.X = left.Row2.X + right.Row2.X;
			result.Row2.Y = left.Row2.Y + right.Row2.Y;
			result.Row3.X = left.Row3.X + right.Row3.X;
			result.Row3.Y = left.Row3.Y + right.Row3.Y;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001D6B4 File Offset: 0x0001B8B4
		public static Matrix4x2 Add(Matrix4x2 left, Matrix4x2 right)
		{
			Matrix4x2 result;
			Matrix4x2.Add(left, right, out result);
			return result;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001D6D0 File Offset: 0x0001B8D0
		public static void Subtract(in Matrix4x2 left, in Matrix4x2 right, out Matrix4x2 result)
		{
			result.Row0.X = left.Row0.X - right.Row0.X;
			result.Row0.Y = left.Row0.Y - right.Row0.Y;
			result.Row1.X = left.Row1.X - right.Row1.X;
			result.Row1.Y = left.Row1.Y - right.Row1.Y;
			result.Row2.X = left.Row2.X - right.Row2.X;
			result.Row2.Y = left.Row2.Y - right.Row2.Y;
			result.Row3.X = left.Row3.X - right.Row3.X;
			result.Row3.Y = left.Row3.Y - right.Row3.Y;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0001D7F0 File Offset: 0x0001B9F0
		public static Matrix4x2 Subtract(Matrix4x2 left, Matrix4x2 right)
		{
			Matrix4x2 result;
			Matrix4x2.Subtract(left, right, out result);
			return result;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0001D80C File Offset: 0x0001BA0C
		public static void Transpose(in Matrix4x2 mat, out Matrix2x4 result)
		{
			result.Row0.X = mat.Row0.X;
			result.Row0.Y = mat.Row1.X;
			result.Row0.Z = mat.Row2.X;
			result.Row0.W = mat.Row3.X;
			result.Row1.X = mat.Row0.Y;
			result.Row1.Y = mat.Row1.Y;
			result.Row1.Z = mat.Row2.Y;
			result.Row1.W = mat.Row3.Y;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0001D8CC File Offset: 0x0001BACC
		public static Matrix2x4 Transpose(Matrix4x2 mat)
		{
			Matrix2x4 result;
			Matrix4x2.Transpose(mat, out result);
			return result;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001D8E4 File Offset: 0x0001BAE4
		public static Matrix4x2 Swizzle(Matrix4x2 mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3)
		{
			Matrix4x2 result;
			Matrix4x2.Swizzle(mat, rowForRow0, rowForRow1, rowForRow2, rowForRow3, out result);
			return result;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001D900 File Offset: 0x0001BB00
		public static void Swizzle(in Matrix4x2 mat, int rowForRow0, int rowForRow1, int rowForRow2, int rowForRow3, out Matrix4x2 result)
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

		// Token: 0x06000783 RID: 1923 RVA: 0x0001DA8B File Offset: 0x0001BC8B
		public static Matrix4x2 operator *(float left, Matrix4x2 right)
		{
			return Matrix4x2.Mult(right, left);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001DA94 File Offset: 0x0001BC94
		public static Matrix4x2 operator *(Matrix4x2 left, float right)
		{
			return Matrix4x2.Mult(left, right);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001DA9D File Offset: 0x0001BC9D
		public static Matrix4x2 operator *(Matrix4x2 left, Matrix2 right)
		{
			return Matrix4x2.Mult(left, right);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0001DAA6 File Offset: 0x0001BCA6
		public static Matrix4x3 operator *(Matrix4x2 left, Matrix2x3 right)
		{
			return Matrix4x2.Mult(left, right);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0001DAAF File Offset: 0x0001BCAF
		public static Matrix4 operator *(Matrix4x2 left, Matrix2x4 right)
		{
			return Matrix4x2.Mult(left, right);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0001DAB8 File Offset: 0x0001BCB8
		public static Matrix4x2 operator +(Matrix4x2 left, Matrix4x2 right)
		{
			return Matrix4x2.Add(left, right);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0001DAC1 File Offset: 0x0001BCC1
		public static Matrix4x2 operator -(Matrix4x2 left, Matrix4x2 right)
		{
			return Matrix4x2.Subtract(left, right);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001DACA File Offset: 0x0001BCCA
		public static bool operator ==(Matrix4x2 left, Matrix4x2 right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001DAD4 File Offset: 0x0001BCD4
		public static bool operator !=(Matrix4x2 left, Matrix4x2 right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001DAE1 File Offset: 0x0001BCE1
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001DAEB File Offset: 0x0001BCEB
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0001DAF5 File Offset: 0x0001BCF5
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0001DB00 File Offset: 0x0001BD00
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

		// Token: 0x06000790 RID: 1936 RVA: 0x0001DB78 File Offset: 0x0001BD78
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector2, Vector2, Vector2, Vector2>(this.Row0, this.Row1, this.Row2, this.Row3);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001DB97 File Offset: 0x0001BD97
		public override readonly bool Equals(object obj)
		{
			return obj is Matrix4x2 && this.Equals((Matrix4x2)obj);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001DBB0 File Offset: 0x0001BDB0
		public readonly bool Equals(Matrix4x2 other)
		{
			return this.Row0 == other.Row0 && this.Row1 == other.Row1 && this.Row2 == other.Row2 && this.Row3 == other.Row3;
		}

		// Token: 0x04000072 RID: 114
		public Vector2 Row0;

		// Token: 0x04000073 RID: 115
		public Vector2 Row1;

		// Token: 0x04000074 RID: 116
		public Vector2 Row2;

		// Token: 0x04000075 RID: 117
		public Vector2 Row3;

		// Token: 0x04000076 RID: 118
		public static readonly Matrix4x2 Zero = new Matrix4x2(Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero);
	}
}
