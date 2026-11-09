using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000014 RID: 20
	public struct TransformationMatrix
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00004190 File Offset: 0x00002390
		public static TransformationMatrix GetTranslationMatrix(double x, double y)
		{
			return new TransformationMatrix(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, x, y, 1.0);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000041E4 File Offset: 0x000023E4
		public static TransformationMatrix GetScaleMatrix(double scaleX, double scaleY)
		{
			return new TransformationMatrix(scaleX, 0.0, 0.0, 0.0, scaleY, 0.0, 0.0, 0.0, 1.0);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004238 File Offset: 0x00002438
		public static TransformationMatrix GetRotationMatrix(double degreesCounterclockwise)
		{
			double num = degreesCounterclockwise % 360.0;
			if (num < 0.0)
			{
				num += 360.0;
			}
			double num2;
			double num3;
			if (num <= 90.0)
			{
				if (num != 0.0)
				{
					if (num != 90.0)
					{
						goto IL_C6;
					}
					num2 = 0.0;
					num3 = 1.0;
					goto IL_E8;
				}
			}
			else
			{
				if (num == 180.0)
				{
					num2 = -1.0;
					num3 = 0.0;
					goto IL_E8;
				}
				if (num == 270.0)
				{
					num2 = 0.0;
					num3 = -1.0;
					goto IL_E8;
				}
				if (num != 360.0)
				{
					goto IL_C6;
				}
			}
			num2 = 1.0;
			num3 = 0.0;
			goto IL_E8;
			IL_C6:
			num2 = Math.Cos(degreesCounterclockwise * 0.017453292519943295);
			num3 = Math.Sin(degreesCounterclockwise * 0.017453292519943295);
			IL_E8:
			return new TransformationMatrix(num2, num3, 0.0, -num3, num2, 0.0, 0.0, 0.0, 1.0);
		}

		// Token: 0x17000026 RID: 38
		public double this[int row, int col]
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (row >= 3)
				{
					string text = "row";
					defaultInterpolatedStringHandler..ctor(92, 2);
					defaultInterpolatedStringHandler.AppendLiteral("The transformation matrix only contains ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(3);
					defaultInterpolatedStringHandler.AppendLiteral(" rows and is zero indexed, you tried to access row ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(row);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (row < 0)
				{
					throw new ArgumentOutOfRangeException("row", "Cannot access negative rows in a matrix.");
				}
				if (col >= 3)
				{
					string text2 = "col";
					defaultInterpolatedStringHandler..ctor(98, 2);
					defaultInterpolatedStringHandler.AppendLiteral("The transformation matrix only contains ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(3);
					defaultInterpolatedStringHandler.AppendLiteral(" columns and is zero indexed, you tried to access column ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(col);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new ArgumentOutOfRangeException(text2, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (col < 0)
				{
					throw new ArgumentOutOfRangeException("col", "Cannot access negative columns in a matrix.");
				}
				switch (row)
				{
				case 0:
					switch (col)
					{
					case 0:
						return this.A;
					case 1:
						return this.B;
					case 2:
						return this.row1;
					default:
						defaultInterpolatedStringHandler..ctor(53, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Trying to access ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(row);
						defaultInterpolatedStringHandler.AppendLiteral(", ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(col);
						defaultInterpolatedStringHandler.AppendLiteral(" which was not in the value array.");
						throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					break;
				case 1:
					switch (col)
					{
					case 0:
						return this.C;
					case 1:
						return this.D;
					case 2:
						return this.row2;
					default:
						defaultInterpolatedStringHandler..ctor(53, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Trying to access ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(row);
						defaultInterpolatedStringHandler.AppendLiteral(", ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(col);
						defaultInterpolatedStringHandler.AppendLiteral(" which was not in the value array.");
						throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					break;
				case 2:
					switch (col)
					{
					case 0:
						return this.E;
					case 1:
						return this.F;
					case 2:
						return this.row3;
					default:
						defaultInterpolatedStringHandler..ctor(53, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Trying to access ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(row);
						defaultInterpolatedStringHandler.AppendLiteral(", ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(col);
						defaultInterpolatedStringHandler.AppendLiteral(" which was not in the value array.");
						throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					break;
				default:
					defaultInterpolatedStringHandler..ctor(53, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Trying to access ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(row);
					defaultInterpolatedStringHandler.AppendLiteral(", ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(col);
					defaultInterpolatedStringHandler.AppendLiteral(" which was not in the value array.");
					throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004600 File Offset: 0x00002800
		public TransformationMatrix(double[] value)
		{
			this = new TransformationMatrix(value[0], value[1], value[2], value[3], value[4], value[5], value[6], value[7], value[8]);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004630 File Offset: 0x00002830
		public TransformationMatrix(double a, double b, double r1, double c, double d, double r2, double e, double f, double r3)
		{
			this.A = a;
			this.B = b;
			this.row1 = r1;
			this.C = c;
			this.D = d;
			this.row2 = r2;
			this.E = e;
			this.F = f;
			this.row3 = r3;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004684 File Offset: 0x00002884
		public PdfPoint Transform(PdfPoint original)
		{
			double x = this.A * original.X + this.C * original.Y + this.E;
			double y = this.B * original.X + this.D * original.Y + this.F;
			return new PdfPoint(x, y);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000046E0 File Offset: 0x000028E0
		public double TransformX(double x)
		{
			return this.A * x + this.C * 0.0 + this.E;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004702 File Offset: 0x00002902
		public PdfRectangle Transform(PdfRectangle original)
		{
			return new PdfRectangle(this.Transform(original.TopLeft), this.Transform(original.TopRight), this.Transform(original.BottomLeft), this.Transform(original.BottomRight));
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004740 File Offset: 0x00002940
		public PdfSubpath Transform(PdfSubpath subpath)
		{
			PdfSubpath pdfSubpath = new PdfSubpath();
			foreach (PdfSubpath.IPathCommand pathCommand in subpath.Commands)
			{
				PdfSubpath.Move move = pathCommand as PdfSubpath.Move;
				if (move != null)
				{
					PdfPoint pdfPoint = this.Transform(move.Location);
					pdfSubpath.MoveTo(pdfPoint.X, pdfPoint.Y);
				}
				else
				{
					PdfSubpath.Line line = pathCommand as PdfSubpath.Line;
					if (line != null)
					{
						PdfPoint pdfPoint2 = this.Transform(line.To);
						pdfSubpath.LineTo(pdfPoint2.X, pdfPoint2.Y);
					}
					else
					{
						PdfSubpath.BezierCurve bezierCurve = pathCommand as PdfSubpath.BezierCurve;
						if (bezierCurve != null)
						{
							PdfPoint pdfPoint3 = this.Transform(bezierCurve.FirstControlPoint);
							PdfPoint pdfPoint4 = this.Transform(bezierCurve.SecondControlPoint);
							PdfPoint pdfPoint5 = this.Transform(bezierCurve.EndPoint);
							pdfSubpath.BezierCurveTo(pdfPoint3.X, pdfPoint3.Y, pdfPoint4.X, pdfPoint4.Y, pdfPoint5.X, pdfPoint5.Y);
						}
						else
						{
							if (!(pathCommand is PdfSubpath.Close))
							{
								throw new Exception("Unknown PdfSubpath type");
							}
							pdfSubpath.CloseSubpath();
						}
					}
				}
			}
			return pdfSubpath;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000488C File Offset: 0x00002A8C
		public IEnumerable<PdfSubpath> Transform(IEnumerable<PdfSubpath> path)
		{
			foreach (PdfSubpath subpath in path)
			{
				yield return this.Transform(subpath);
			}
			IEnumerator<PdfSubpath> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000048A8 File Offset: 0x00002AA8
		public TransformationMatrix Translate(double x, double y)
		{
			double a = this.A;
			double b = this.B;
			double r = this.row1;
			double c = this.C;
			double d = this.D;
			double r2 = this.row2;
			double e = x * this.A + y * this.C + this.E;
			double f = x * this.B + y * this.D + this.F;
			double r3 = x * this.row1 + y * this.row2 + this.row3;
			return new TransformationMatrix(a, b, r, c, d, r2, e, f, r3);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004940 File Offset: 0x00002B40
		public static TransformationMatrix FromValues(double a, double b, double c, double d, double e, double f)
		{
			return new TransformationMatrix(a, b, 0.0, c, d, 0.0, e, f, 1.0);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004978 File Offset: 0x00002B78
		public static TransformationMatrix FromValues(double a, double b, double c, double d)
		{
			return new TransformationMatrix(a, b, 0.0, c, d, 0.0, 0.0, 0.0, 1.0);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000049BB File Offset: 0x00002BBB
		public static TransformationMatrix FromArray(decimal[] values)
		{
			return TransformationMatrix.FromArray(Enumerable.ToArray<double>(Enumerable.Select<decimal, double>(values, (decimal x) => (double)x)));
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000049EC File Offset: 0x00002BEC
		public static TransformationMatrix FromArray(double[] values)
		{
			if (values.Length == 9)
			{
				return new TransformationMatrix(values);
			}
			if (values.Length == 6)
			{
				return new TransformationMatrix(values[0], values[1], 0.0, values[2], values[3], 0.0, values[4], values[5], 1.0);
			}
			if (values.Length == 4)
			{
				return new TransformationMatrix(values[0], values[1], 0.0, values[2], values[3], 0.0, 0.0, 0.0, 1.0);
			}
			throw new ArgumentException("The array must either define all 9 elements of the matrix or all 6 key elements. Instead array was: " + ((values != null) ? values.ToString() : null));
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004AA4 File Offset: 0x00002CA4
		public TransformationMatrix Multiply(TransformationMatrix matrix)
		{
			double a = this.A * matrix.A + this.B * matrix.C + this.row1 * matrix.E;
			double b = this.A * matrix.B + this.B * matrix.D + this.row1 * matrix.F;
			double r = this.A * matrix.row1 + this.B * matrix.row2 + this.row1 * matrix.row3;
			double c = this.C * matrix.A + this.D * matrix.C + this.row2 * matrix.E;
			double d = this.C * matrix.B + this.D * matrix.D + this.row2 * matrix.F;
			double r2 = this.C * matrix.row1 + this.D * matrix.row2 + this.row2 * matrix.row3;
			double e = this.E * matrix.A + this.F * matrix.C + this.row3 * matrix.E;
			double f = this.E * matrix.B + this.F * matrix.D + this.row3 * matrix.F;
			double r3 = this.E * matrix.row1 + this.F * matrix.row2 + this.row3 * matrix.row3;
			return new TransformationMatrix(a, b, r, c, d, r2, e, f, r3);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004C40 File Offset: 0x00002E40
		public TransformationMatrix Multiply(double scalar)
		{
			return new TransformationMatrix(this.A * scalar, this.B * scalar, this.row1 * scalar, this.C * scalar, this.D * scalar, this.row2 * scalar, this.E * scalar, this.F * scalar, this.row3 * scalar);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004C9C File Offset: 0x00002E9C
		public TransformationMatrix Inverse()
		{
			double num = this.D * this.row3 - this.row2 * this.F;
			double num2 = -(this.C * this.row3 - this.row2 * this.E);
			double num3 = this.C * this.F - this.D * this.E;
			double num4 = -(this.B * this.row3 - this.row1 * this.F);
			double num5 = this.A * this.row3 - this.row1 * this.E;
			double num6 = -(this.A * this.F - this.B * this.E);
			double num7 = this.B * this.row2 - this.row1 * this.D;
			double num8 = -(this.A * this.row2 - this.row1 * this.C);
			double num9 = this.A * this.D - this.B * this.C;
			double num10 = this.A * num + this.B * num2 + this.row1 * num3;
			return new TransformationMatrix(num / num10, num4 / num10, num7 / num10, num2 / num10, num5 / num10, num8 / num10, num3 / num10, num6 / num10, num9 / num10);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004DF8 File Offset: 0x00002FF8
		internal double GetScalingFactorX()
		{
			double result = this.A;
			if (this.B != 0.0 || this.C != 0.0)
			{
				result = Math.Sqrt(this.A * this.A + this.B * this.B);
			}
			return result;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004E50 File Offset: 0x00003050
		public override bool Equals(object obj)
		{
			if (obj is TransformationMatrix)
			{
				TransformationMatrix b = (TransformationMatrix)obj;
				return TransformationMatrix.Equals(this, b);
			}
			return false;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004E7C File Offset: 0x0000307C
		public static bool Equals(TransformationMatrix a, TransformationMatrix b)
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (a[i, j] != b[i, j])
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004EB8 File Offset: 0x000030B8
		public override int GetHashCode()
		{
			return ((((((((472622392 * -1521134295 + this.row1.GetHashCode()) * -1521134295 + this.row2.GetHashCode()) * -1521134295 + this.row3.GetHashCode()) * -1521134295 + this.A.GetHashCode()) * -1521134295 + this.B.GetHashCode()) * -1521134295 + this.C.GetHashCode()) * -1521134295 + this.D.GetHashCode()) * -1521134295 + this.E.GetHashCode()) * -1521134295 + this.F.GetHashCode();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004F6C File Offset: 0x0000316C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(16, 9);
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.A);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.B);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.row1);
			defaultInterpolatedStringHandler.AppendLiteral("\r\n");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.C);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.D);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.row2);
			defaultInterpolatedStringHandler.AppendLiteral("\r\n");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.E);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.F);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.row3);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400002F RID: 47
		public static TransformationMatrix Identity = new TransformationMatrix(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0);

		// Token: 0x04000030 RID: 48
		private readonly double row1;

		// Token: 0x04000031 RID: 49
		private readonly double row2;

		// Token: 0x04000032 RID: 50
		private readonly double row3;

		// Token: 0x04000033 RID: 51
		public readonly double A;

		// Token: 0x04000034 RID: 52
		public readonly double B;

		// Token: 0x04000035 RID: 53
		public readonly double C;

		// Token: 0x04000036 RID: 54
		public readonly double D;

		// Token: 0x04000037 RID: 55
		public readonly double E;

		// Token: 0x04000038 RID: 56
		public readonly double F;

		// Token: 0x04000039 RID: 57
		public const int Rows = 3;

		// Token: 0x0400003A RID: 58
		public const int Columns = 3;
	}
}
