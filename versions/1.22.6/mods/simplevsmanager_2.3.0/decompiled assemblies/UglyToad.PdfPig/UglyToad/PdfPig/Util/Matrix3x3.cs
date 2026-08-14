using System;
using System.Collections;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000032 RID: 50
	internal class Matrix3x3 : IEnumerable<double>, IEnumerable, IEquatable<Matrix3x3>
	{
		// Token: 0x06000139 RID: 313 RVA: 0x0000BF84 File Offset: 0x0000A184
		public Matrix3x3(double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33)
		{
			this.m11 = m11;
			this.m12 = m12;
			this.m13 = m13;
			this.m21 = m21;
			this.m22 = m22;
			this.m23 = m23;
			this.m31 = m31;
			this.m32 = m32;
			this.m33 = m33;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000BFDC File Offset: 0x0000A1DC
		public IEnumerator<double> GetEnumerator()
		{
			yield return this.m11;
			yield return this.m12;
			yield return this.m13;
			yield return this.m21;
			yield return this.m22;
			yield return this.m23;
			yield return this.m31;
			yield return this.m32;
			yield return this.m33;
			yield break;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000BFEC File Offset: 0x0000A1EC
		public Matrix3x3 Inverse()
		{
			double determinant = this.GetDeterminant();
			if (determinant == 0.0)
			{
				return null;
			}
			Matrix3x3 matrix3x = this.Transpose();
			double num = matrix3x.m22 * matrix3x.m33 - matrix3x.m23 * matrix3x.m32;
			double num2 = matrix3x.m21 * matrix3x.m33 - matrix3x.m23 * matrix3x.m31;
			double num3 = matrix3x.m21 * matrix3x.m32 - matrix3x.m22 * matrix3x.m31;
			double num4 = matrix3x.m12 * matrix3x.m33 - matrix3x.m13 * matrix3x.m32;
			double num5 = matrix3x.m11 * matrix3x.m33 - matrix3x.m13 * matrix3x.m31;
			double num6 = matrix3x.m11 * matrix3x.m32 - matrix3x.m12 * matrix3x.m31;
			double num7 = matrix3x.m12 * matrix3x.m23 - matrix3x.m13 * matrix3x.m22;
			double num8 = matrix3x.m11 * matrix3x.m23 - matrix3x.m13 * matrix3x.m21;
			double num9 = matrix3x.m11 * matrix3x.m22 - matrix3x.m12 * matrix3x.m21;
			return new Matrix3x3(num, -num2, num3, -num4, num5, -num6, num7, -num8, num9).Multiply(1.0 / determinant);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000C140 File Offset: 0x0000A340
		public Matrix3x3 Multiply(double factor)
		{
			return new Matrix3x3(this.m11 * factor, this.m12 * factor, this.m13 * factor, this.m21 * factor, this.m22 * factor, this.m23 * factor, this.m31 * factor, this.m32 * factor, this.m33 * factor);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000C19C File Offset: 0x0000A39C
		public ValueTuple<double, double, double> Multiply(ValueTuple<double, double, double> vector)
		{
			return new ValueTuple<double, double, double>(this.m11 * vector.Item1 + this.m12 * vector.Item2 + this.m13 * vector.Item3, this.m21 * vector.Item1 + this.m22 * vector.Item2 + this.m23 * vector.Item3, this.m31 * vector.Item1 + this.m32 * vector.Item2 + this.m33 * vector.Item3);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000C22C File Offset: 0x0000A42C
		public Matrix3x3 Multiply(Matrix3x3 matrix)
		{
			return new Matrix3x3(this.m11 * matrix.m11 + this.m12 * matrix.m21 + this.m13 * matrix.m31, this.m11 * matrix.m12 + this.m12 * matrix.m22 + this.m13 * matrix.m32, this.m11 * matrix.m13 + this.m12 * matrix.m23 + this.m13 * matrix.m33, this.m21 * matrix.m11 + this.m22 * matrix.m21 + this.m23 * matrix.m31, this.m21 * matrix.m12 + this.m22 * matrix.m22 + this.m23 * matrix.m32, this.m21 * matrix.m13 + this.m22 * matrix.m23 + this.m23 * matrix.m33, this.m31 * matrix.m11 + this.m32 * matrix.m21 + this.m33 * matrix.m31, this.m31 * matrix.m12 + this.m32 * matrix.m22 + this.m33 * matrix.m32, this.m31 * matrix.m13 + this.m32 * matrix.m23 + this.m33 * matrix.m33);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000C3B0 File Offset: 0x0000A5B0
		public Matrix3x3 Transpose()
		{
			return new Matrix3x3(this.m11, this.m21, this.m31, this.m12, this.m22, this.m32, this.m13, this.m23, this.m33);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000C3F8 File Offset: 0x0000A5F8
		public override bool Equals(object obj)
		{
			Matrix3x3 other = obj as Matrix3x3;
			return this.Equals(other);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000C414 File Offset: 0x0000A614
		public bool Equals(Matrix3x3 other)
		{
			return other != null && (this == other || (this.m11 == other.m11 && this.m12 == other.m12 && this.m13 == other.m13 && this.m21 == other.m21 && this.m22 == other.m22 && this.m23 == other.m23 && this.m31 == other.m31 && this.m32 == other.m32 && this.m33 == other.m33));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000C4AC File Offset: 0x0000A6AC
		public override int GetHashCode()
		{
			return new ValueTuple<double, double, double, double, double, double, double, ValueTuple<double, double>>(this.m11, this.m12, this.m13, this.m21, this.m22, this.m23, this.m31, new ValueTuple<double, double>(this.m32, this.m33)).GetHashCode();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000C508 File Offset: 0x0000A708
		private double GetDeterminant()
		{
			double num = this.m22 * this.m33 - this.m23 * this.m32;
			double num2 = this.m21 * this.m33 - this.m23 * this.m31;
			double num3 = this.m21 * this.m32 - this.m22 * this.m31;
			return this.m11 * num - this.m12 * num2 + this.m13 * num3;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000C583 File Offset: 0x0000A783
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040000AB RID: 171
		public static readonly Matrix3x3 Identity = new Matrix3x3(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0);

		// Token: 0x040000AC RID: 172
		private readonly double m11;

		// Token: 0x040000AD RID: 173
		private readonly double m12;

		// Token: 0x040000AE RID: 174
		private readonly double m13;

		// Token: 0x040000AF RID: 175
		private readonly double m21;

		// Token: 0x040000B0 RID: 176
		private readonly double m22;

		// Token: 0x040000B1 RID: 177
		private readonly double m23;

		// Token: 0x040000B2 RID: 178
		private readonly double m31;

		// Token: 0x040000B3 RID: 179
		private readonly double m32;

		// Token: 0x040000B4 RID: 180
		private readonly double m33;
	}
}
