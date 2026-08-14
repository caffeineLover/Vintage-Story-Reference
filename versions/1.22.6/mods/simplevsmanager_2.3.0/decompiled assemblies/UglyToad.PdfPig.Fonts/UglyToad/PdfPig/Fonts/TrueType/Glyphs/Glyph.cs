using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Glyphs
{
	// Token: 0x02000066 RID: 102
	internal class Glyph : IGlyphDescription, IMergeableGlyph, ITransformableGlyph
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000CF45 File Offset: 0x0000B145
		public PdfRectangle Bounds { get; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000CF4D File Offset: 0x0000B14D
		public byte[] Instructions { get; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000CF55 File Offset: 0x0000B155
		public ushort[] EndPointsOfContours { get; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000CF5D File Offset: 0x0000B15D
		public GlyphPoint[] Points { get; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000CF65 File Offset: 0x0000B165
		public bool IsSimple { get; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000CF6D File Offset: 0x0000B16D
		public bool IsEmpty
		{
			get
			{
				return this.Points.Length == 0;
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000CF79 File Offset: 0x0000B179
		public Glyph(bool isSimple, byte[] instructions, ushort[] endPointsOfContours, GlyphPoint[] points, PdfRectangle bounds)
		{
			this.IsSimple = isSimple;
			this.Instructions = instructions;
			this.EndPointsOfContours = endPointsOfContours;
			this.Points = points;
			this.Bounds = bounds;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000CFA6 File Offset: 0x0000B1A6
		public static IGlyphDescription Empty(PdfRectangle bounds)
		{
			return new Glyph(true, new byte[0], new ushort[0], new GlyphPoint[0], bounds);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000CFC4 File Offset: 0x0000B1C4
		public IGlyphDescription DeepClone()
		{
			byte[] array = new byte[this.Instructions.Length];
			Array.Copy(this.Instructions, array, this.Instructions.Length);
			ushort[] array2 = new ushort[this.EndPointsOfContours.Length];
			Array.Copy(this.EndPointsOfContours, array2, this.EndPointsOfContours.Length);
			GlyphPoint[] array3 = new GlyphPoint[this.Points.Length];
			Array.Copy(this.Points, array3, this.Points.Length);
			return new Glyph(false, array, array2, array3, this.Bounds);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000D048 File Offset: 0x0000B248
		public IGlyphDescription Merge(IGlyphDescription glyph)
		{
			GlyphPoint[] points = this.MergePoints(glyph);
			ushort[] endPointsOfContours = this.MergeContourEndPoints(glyph);
			return new Glyph(false, this.Instructions, endPointsOfContours, points, this.Bounds);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000D07C File Offset: 0x0000B27C
		private GlyphPoint[] MergePoints(IGlyphDescription glyph)
		{
			GlyphPoint[] array = new GlyphPoint[this.Points.Length + glyph.Points.Length];
			for (int i = 0; i < this.Points.Length; i++)
			{
				array[i] = this.Points[i];
			}
			for (int j = 0; j < glyph.Points.Length; j++)
			{
				array[j + this.Points.Length] = glyph.Points[j];
			}
			return array;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000D0F8 File Offset: 0x0000B2F8
		private ushort[] MergeContourEndPoints(IGlyphDescription glyph)
		{
			int num = (int)(this.EndPointsOfContours[this.EndPointsOfContours.Length - 1] + 1);
			ushort[] array = new ushort[this.EndPointsOfContours.Length + glyph.EndPointsOfContours.Length];
			for (int i = 0; i < this.EndPointsOfContours.Length; i++)
			{
				array[i] = this.EndPointsOfContours[i];
			}
			for (int j = 0; j < glyph.EndPointsOfContours.Length; j++)
			{
				array[j + this.EndPointsOfContours.Length] = (ushort)((int)glyph.EndPointsOfContours[j] + num);
			}
			return array;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000D17C File Offset: 0x0000B37C
		public IGlyphDescription Transform(CompositeTransformMatrix3By2 matrix)
		{
			GlyphPoint[] array = new GlyphPoint[this.Points.Length];
			for (int i = this.Points.Length - 1; i >= 0; i--)
			{
				GlyphPoint glyphPoint = this.Points[i];
				PdfPoint source = matrix.ScaleAndRotate(new PdfPoint((int)glyphPoint.X, (int)glyphPoint.Y));
				source = matrix.Translate(source);
				array[i] = new GlyphPoint((short)source.X, (short)source.Y, glyphPoint.IsOnCurve, glyphPoint.IsEndOfContour);
			}
			return new Glyph(this.IsSimple, this.Instructions, this.EndPointsOfContours, array, this.Bounds);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000D225 File Offset: 0x0000B425
		public bool TryGetGlyphPath(out IReadOnlyList<PdfSubpath> subpaths)
		{
			subpaths = EmptyArray<PdfSubpath>.Instance;
			if (this.Points == null)
			{
				return false;
			}
			if (this.Points.Length != 0)
			{
				subpaths = Glyph.CalculatePath(this.Points);
			}
			return true;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000D250 File Offset: 0x0000B450
		private static IReadOnlyList<PdfSubpath> CalculatePath(GlyphPoint[] points)
		{
			List<PdfSubpath> list = new List<PdfSubpath>();
			int num = 0;
			for (int i = 0; i < points.Length; i++)
			{
				if (points[i].IsEndOfContour)
				{
					PdfSubpath pdfSubpath = new PdfSubpath();
					GlyphPoint glyphPoint = points[num];
					GlyphPoint glyphPoint2 = points[i];
					List<GlyphPoint> list2 = new List<GlyphPoint>();
					for (int j = num; j <= i; j++)
					{
						list2.Add(points[j]);
					}
					if (points[num].IsOnCurve)
					{
						list2.Add(glyphPoint);
					}
					else if (points[i].IsOnCurve)
					{
						list2.Insert(0, glyphPoint2);
					}
					else
					{
						GlyphPoint glyphPoint3 = Glyph.midValue(glyphPoint, glyphPoint2);
						list2.Insert(0, glyphPoint3);
						list2.Add(glyphPoint3);
					}
					pdfSubpath.MoveTo((double)list2[0].X, (double)list2[0].Y);
					for (int k = 1; k < list2.Count; k++)
					{
						GlyphPoint point = list2[k];
						if (point.IsOnCurve)
						{
							pdfSubpath.LineTo((double)point.X, (double)point.Y);
						}
						else if (list2[k + 1].IsOnCurve)
						{
							GlyphPoint glyphPoint4 = list2[k - 1];
							GlyphPoint glyphPoint5 = list2[k + 1];
							pdfSubpath.BezierCurveTo((double)glyphPoint4.X, (double)glyphPoint4.Y, (double)point.X, (double)point.Y, (double)glyphPoint5.X, (double)glyphPoint5.Y);
							k++;
						}
						else
						{
							GlyphPoint glyphPoint6 = list2[k - 1];
							GlyphPoint glyphPoint7 = Glyph.midValue(point, list2[k + 1]);
							pdfSubpath.BezierCurveTo((double)glyphPoint6.X, (double)glyphPoint6.Y, (double)point.X, (double)point.Y, (double)glyphPoint7.X, (double)glyphPoint7.Y);
						}
					}
					pdfSubpath.CloseSubpath();
					list.Add(pdfSubpath);
					num = i + 1;
				}
			}
			return list;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000D465 File Offset: 0x0000B665
		private static short midValue(short a, short b)
		{
			return a + (b - a) / 2;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000D46F File Offset: 0x0000B66F
		private static GlyphPoint midValue(GlyphPoint point1, GlyphPoint point2)
		{
			return new GlyphPoint(Glyph.midValue(point1.X, point2.X), Glyph.midValue(point1.Y, point2.Y), true, false);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000D4A0 File Offset: 0x0000B6A0
		public override string ToString()
		{
			string text = this.IsSimple ? "S" : "C";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(28, 4);
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(": Width ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Bounds.Width);
			defaultInterpolatedStringHandler.AppendLiteral(", Height: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.Bounds.Height);
			defaultInterpolatedStringHandler.AppendLiteral(", Points: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Points.Length);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
