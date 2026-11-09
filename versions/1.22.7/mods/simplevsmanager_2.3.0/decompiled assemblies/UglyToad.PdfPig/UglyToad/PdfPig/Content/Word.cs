using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001F7 RID: 503
	public class Word
	{
		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x0003C8A6 File Offset: 0x0003AAA6
		public string Text { get; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x0003C8AE File Offset: 0x0003AAAE
		public TextOrientation TextOrientation { get; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0003C8B6 File Offset: 0x0003AAB6
		public PdfRectangle BoundingBox { get; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x0003C8BE File Offset: 0x0003AABE
		public string FontName { get; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0003C8C6 File Offset: 0x0003AAC6
		public IReadOnlyList<Letter> Letters { get; }

		// Token: 0x06000C2F RID: 3119 RVA: 0x0003C8D0 File Offset: 0x0003AAD0
		public Word(IReadOnlyList<Letter> letters)
		{
			if (letters == null)
			{
				throw new ArgumentNullException("letters");
			}
			if (letters.Count == 0)
			{
				throw new ArgumentException("Empty letters provided.", "letters");
			}
			this.Letters = letters;
			TextOrientation textOrientation = letters[0].TextOrientation;
			if (textOrientation != TextOrientation.Other)
			{
				using (IEnumerator<Letter> enumerator = letters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.TextOrientation != textOrientation)
						{
							textOrientation = TextOrientation.Other;
							break;
						}
					}
				}
			}
			Tuple<string, PdfRectangle> tuple;
			switch (textOrientation)
			{
			case TextOrientation.Horizontal:
				tuple = this.GetBoundingBoxH(letters);
				goto IL_BE;
			case TextOrientation.Rotate180:
				tuple = this.GetBoundingBox180(letters);
				goto IL_BE;
			case TextOrientation.Rotate90:
				tuple = this.GetBoundingBox90(letters);
				goto IL_BE;
			case TextOrientation.Rotate270:
				tuple = this.GetBoundingBox270(letters);
				goto IL_BE;
			}
			tuple = this.GetBoundingBoxOther(letters);
			IL_BE:
			this.Text = tuple.Item1;
			this.BoundingBox = tuple.Item2;
			this.FontName = letters[0].FontName;
			this.TextOrientation = textOrientation;
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x0003C9DC File Offset: 0x0003ABDC
		private Tuple<string, PdfRectangle> GetBoundingBoxH(IReadOnlyList<Letter> letters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			double num = double.MaxValue;
			double num2 = double.MinValue;
			double num3 = double.MaxValue;
			double num4 = double.MinValue;
			for (int i = 0; i < letters.Count; i++)
			{
				Letter letter = letters[i];
				stringBuilder.Append(letter.Value);
				if (letter.StartBaseLine.X < num)
				{
					num = letter.StartBaseLine.X;
				}
				if (letter.StartBaseLine.Y < num3)
				{
					num3 = letter.StartBaseLine.Y;
				}
				double num5 = letter.StartBaseLine.X + Math.Max(letter.Width, letter.GlyphRectangle.Width);
				if (num5 > num2)
				{
					num2 = num5;
				}
				if (letter.GlyphRectangle.TopLeft.Y > num4)
				{
					num4 = letter.GlyphRectangle.TopLeft.Y;
				}
			}
			return new Tuple<string, PdfRectangle>(stringBuilder.ToString(), new PdfRectangle(num, num3, num2, num4));
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0003CB18 File Offset: 0x0003AD18
		private Tuple<string, PdfRectangle> GetBoundingBox180(IReadOnlyList<Letter> letters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			double num = double.MinValue;
			double num2 = double.MinValue;
			double num3 = double.MaxValue;
			double num4 = double.MaxValue;
			for (int i = 0; i < letters.Count; i++)
			{
				Letter letter = letters[i];
				stringBuilder.Append(letter.Value);
				if (letter.StartBaseLine.X > num)
				{
					num = letter.StartBaseLine.X;
				}
				if (letter.StartBaseLine.Y > num2)
				{
					num2 = letter.StartBaseLine.Y;
				}
				double num5 = letter.StartBaseLine.X - Math.Max(letter.Width, letter.GlyphRectangle.Width);
				if (num5 < num3)
				{
					num3 = num5;
				}
				if (letter.GlyphRectangle.TopRight.Y < num4)
				{
					num4 = letter.GlyphRectangle.TopRight.Y;
				}
			}
			return new Tuple<string, PdfRectangle>(stringBuilder.ToString(), new PdfRectangle(num, num2, num3, num4));
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x0003CC54 File Offset: 0x0003AE54
		private Tuple<string, PdfRectangle> GetBoundingBox90(IReadOnlyList<Letter> letters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			double num = double.MaxValue;
			double num2 = double.MaxValue;
			double num3 = double.MinValue;
			double num4 = double.MinValue;
			for (int i = 0; i < letters.Count; i++)
			{
				Letter letter = letters[i];
				stringBuilder.Append(letter.Value);
				if (letter.StartBaseLine.X < num)
				{
					num = letter.StartBaseLine.X;
				}
				if (letter.EndBaseLine.Y < num2)
				{
					num2 = letter.EndBaseLine.Y;
				}
				double num5 = letter.StartBaseLine.X + letter.GlyphRectangle.Height;
				if (num5 > num3)
				{
					num3 = num5;
				}
				if (letter.GlyphRectangle.BottomLeft.Y > num4)
				{
					num4 = letter.GlyphRectangle.BottomLeft.Y;
				}
			}
			return new Tuple<string, PdfRectangle>(stringBuilder.ToString(), new PdfRectangle(new PdfPoint(num3, num4), new PdfPoint(num3, num2), new PdfPoint(num, num4), new PdfPoint(num, num2)));
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x0003CD9C File Offset: 0x0003AF9C
		private Tuple<string, PdfRectangle> GetBoundingBox270(IReadOnlyList<Letter> letters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			double num = double.MaxValue;
			double num2 = double.MinValue;
			double num3 = double.MaxValue;
			double num4 = double.MinValue;
			for (int i = 0; i < letters.Count; i++)
			{
				Letter letter = letters[i];
				stringBuilder.Append(letter.Value);
				if (letter.StartBaseLine.X > num2)
				{
					num2 = letter.StartBaseLine.X;
				}
				if (letter.StartBaseLine.Y < num3)
				{
					num3 = letter.StartBaseLine.Y;
				}
				double num5 = letter.StartBaseLine.X - letter.GlyphRectangle.Height;
				if (num5 < num)
				{
					num = num5;
				}
				if (letter.GlyphRectangle.BottomRight.Y > num4)
				{
					num4 = letter.GlyphRectangle.BottomRight.Y;
				}
			}
			return new Tuple<string, PdfRectangle>(stringBuilder.ToString(), new PdfRectangle(new PdfPoint(num, num3), new PdfPoint(num, num4), new PdfPoint(num2, num3), new PdfPoint(num2, num4)));
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x0003CEE4 File Offset: 0x0003B0E4
		private Tuple<string, PdfRectangle> GetBoundingBoxOther(IReadOnlyList<Letter> letters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < letters.Count; i++)
			{
				stringBuilder.Append(letters[i].Value);
			}
			if (letters.Count == 1)
			{
				return new Tuple<string, PdfRectangle>(stringBuilder.ToString(), letters[0].GlyphRectangle);
			}
			List<PdfPoint> list = Enumerable.ToList<PdfPoint>(Enumerable.SelectMany<Letter, PdfPoint>(letters, (Letter r) => new PdfPoint[]
			{
				r.StartBaseLine,
				r.EndBaseLine
			}));
			double num = Enumerable.Average<PdfPoint>(list, (PdfPoint p) => p.X);
			double num2 = Enumerable.Average<PdfPoint>(list, (PdfPoint p) => p.Y);
			double num3 = 0.0;
			double num4 = 0.0;
			for (int j = 0; j < list.Count; j++)
			{
				PdfPoint pdfPoint = list[j];
				double num5 = pdfPoint.X - num;
				double num6 = pdfPoint.Y - num2;
				num3 += num5 * num6;
				num4 += num5 * num5;
			}
			double num7 = 0.0;
			double num8 = 1.0;
			if (num4 > 0.001)
			{
				double num9 = Math.Atan(num3 / num4);
				num7 = Math.Cos(num9);
				num8 = Math.Sin(num9);
			}
			TransformationMatrix inverseRotation = new TransformationMatrix(num7, -num8, 0.0, num8, num7, 0.0, 0.0, 0.0, 1.0);
			IEnumerable<PdfPoint> enumerable = Enumerable.Select<PdfPoint, PdfPoint>(Enumerable.Distinct<PdfPoint>(Enumerable.SelectMany<Letter, PdfPoint>(letters, (Letter r) => new PdfPoint[]
			{
				r.StartBaseLine,
				r.EndBaseLine,
				r.GlyphRectangle.TopLeft,
				r.GlyphRectangle.TopRight
			})), (PdfPoint p) => inverseRotation.Transform(p));
			PdfRectangle pdfRectangle;
			pdfRectangle..ctor(Enumerable.Min<PdfPoint>(enumerable, (PdfPoint p) => p.X), Enumerable.Min<PdfPoint>(enumerable, (PdfPoint p) => p.Y), Enumerable.Max<PdfPoint>(enumerable, (PdfPoint p) => p.X), Enumerable.Max<PdfPoint>(enumerable, (PdfPoint p) => p.Y));
			TransformationMatrix transformationMatrix;
			transformationMatrix..ctor(num7, num8, 0.0, -num8, num7, 0.0, 0.0, 0.0, 1.0);
			PdfRectangle pdfRectangle2 = transformationMatrix.Transform(pdfRectangle);
			PdfRectangle pdfRectangle3;
			pdfRectangle3..ctor(pdfRectangle2.BottomLeft, pdfRectangle2.TopLeft, pdfRectangle2.BottomRight, pdfRectangle2.TopRight);
			PdfRectangle pdfRectangle4;
			pdfRectangle4..ctor(pdfRectangle2.BottomRight, pdfRectangle2.BottomLeft, pdfRectangle2.TopRight, pdfRectangle2.TopLeft);
			PdfRectangle pdfRectangle5;
			pdfRectangle5..ctor(pdfRectangle2.TopRight, pdfRectangle2.BottomRight, pdfRectangle2.TopLeft, pdfRectangle2.BottomLeft);
			Letter letter = letters[0];
			Letter letter2 = letters[letters.Count - 1];
			double num10 = Math.Atan2(letter2.EndBaseLine.Y - letter.StartBaseLine.Y, letter2.EndBaseLine.X - letter.StartBaseLine.X) * 180.0 / 3.141592653589793;
			double num11 = Math.Abs(Word.BoundAngle180(pdfRectangle2.Rotation - num10));
			double num12 = Math.Abs(Word.BoundAngle180(pdfRectangle3.Rotation - num10));
			if (num12 < num11)
			{
				num11 = num12;
				pdfRectangle2 = pdfRectangle3;
			}
			double num13 = Math.Abs(Word.BoundAngle180(pdfRectangle4.Rotation - num10));
			if (num13 < num11)
			{
				num11 = num13;
				pdfRectangle2 = pdfRectangle4;
			}
			if (Math.Abs(Word.BoundAngle180(pdfRectangle5.Rotation - num10)) < num11)
			{
				pdfRectangle2 = pdfRectangle5;
			}
			return new Tuple<string, PdfRectangle>(stringBuilder.ToString(), pdfRectangle2);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0003D323 File Offset: 0x0003B523
		private static double BoundAngle180(double angle)
		{
			angle = (angle + 180.0) % 360.0;
			if (angle < 0.0)
			{
				angle += 360.0;
			}
			return angle - 180.0;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x0003D360 File Offset: 0x0003B560
		public override string ToString()
		{
			return this.Text;
		}
	}
}
