using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry.ClipperLibrary;
using UglyToad.PdfPig.Graphics;

namespace UglyToad.PdfPig.Geometry
{
	// Token: 0x0200018A RID: 394
	public static class GeometryExtensions
	{
		// Token: 0x06000925 RID: 2341 RVA: 0x00029A80 File Offset: 0x00027C80
		private static bool ccw(PdfPoint point1, PdfPoint point2, PdfPoint point3)
		{
			return (point2.X - point1.X) * (point3.Y - point1.Y) > (point2.Y - point1.Y) * (point3.X - point1.X);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00029ACD File Offset: 0x00027CCD
		public static double DotProduct(this PdfPoint point1, PdfPoint point2)
		{
			return point1.X * point2.X + point1.Y * point2.Y;
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00029AEE File Offset: 0x00027CEE
		public static PdfPoint Add(this PdfPoint point1, PdfPoint point2)
		{
			return new PdfPoint(point1.X + point2.X, point1.Y + point2.Y);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00029B13 File Offset: 0x00027D13
		public static PdfPoint Subtract(this PdfPoint point1, PdfPoint point2)
		{
			return new PdfPoint(point1.X - point2.X, point1.Y - point2.Y);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00029B38 File Offset: 0x00027D38
		private static PdfRectangle ParametricPerpendicularProjection(IReadOnlyList<PdfPoint> polygon)
		{
			if (polygon == null || polygon.Count == 0)
			{
				throw new ArgumentException("ParametricPerpendicularProjection(): polygon cannot be null and must contain at least one point.", "polygon");
			}
			if (polygon.Count == 1)
			{
				return new PdfRectangle(polygon[0], polygon[0]);
			}
			if (polygon.Count == 2)
			{
				return new PdfRectangle(polygon[0], polygon[1]);
			}
			double[] array = new double[8];
			double num = double.PositiveInfinity;
			int i = 1;
			int num2 = 0;
			double num3 = double.NaN;
			double num4 = double.NaN;
			double num5 = double.NaN;
			double num6 = double.NaN;
			double num7 = double.NaN;
			double num8 = double.NaN;
			do
			{
				PdfPoint pdfPoint = polygon[num2];
				PdfPoint pdfPoint2 = polygon[i];
				double num9 = pdfPoint2.X - pdfPoint.X;
				double num10 = pdfPoint2.Y - pdfPoint.Y;
				double num11 = 1.0 / (num9 * num9 + num10 * num10);
				double num12 = 1.0;
				double num13 = 0.0;
				double num14 = 0.0;
				int num15 = -1;
				for (i = 0; i < polygon.Count; i++)
				{
					pdfPoint2 = polygon[i];
					double num16 = pdfPoint2.X - pdfPoint.X;
					double num17 = pdfPoint2.Y - pdfPoint.Y;
					double num18 = (num16 * num9 + num17 * num10) * num11;
					double num19 = num18 * num9 + pdfPoint.X;
					double num20 = num18 * num10 + pdfPoint.Y;
					num16 = num19 - pdfPoint2.X;
					num17 = num20 - pdfPoint2.Y;
					double num21 = num16 * num16 + num17 * num17;
					if (num18 < num12)
					{
						num12 = num18;
						num5 = num19;
						num6 = num20;
					}
					if (num18 > num13)
					{
						num13 = num18;
						num7 = num19;
						num8 = num20;
					}
					if (num21 > num14)
					{
						num14 = num21;
						num3 = num19;
						num4 = num20;
						num15 = i;
					}
				}
				if (num15 != -1)
				{
					PdfPoint pdfPoint3 = polygon[num15];
					double num22 = pdfPoint3.X - num3;
					double num23 = pdfPoint3.Y - num4;
					double num24 = num7 + num22;
					double num25 = num8 + num23;
					double num26 = num5 + num22;
					double num27 = num6 + num23;
					double num16 = num7 - num5;
					double num17 = num8 - num6;
					double num28 = (num16 * num16 + num17 * num17) * num14;
					if (num28 < num)
					{
						num = num28;
						array = new double[]
						{
							num5,
							num6,
							num7,
							num8,
							num24,
							num25,
							num26,
							num27
						};
					}
				}
				num2++;
				i = num2 + 1;
				if (i == polygon.Count)
				{
					i = 0;
				}
			}
			while (num2 != polygon.Count);
			return new PdfRectangle(new PdfPoint(array[4], array[5]), new PdfPoint(array[6], array[7]), new PdfPoint(array[2], array[3]), new PdfPoint(array[0], array[1]));
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00029E19 File Offset: 0x00028019
		public static PdfRectangle MinimumAreaRectangle(IEnumerable<PdfPoint> points)
		{
			if (points == null || !Enumerable.Any<PdfPoint>(points))
			{
				throw new ArgumentException("MinimumAreaRectangle(): points cannot be null and must contain at least one point.", "points");
			}
			return GeometryExtensions.ParametricPerpendicularProjection(Enumerable.ToList<PdfPoint>(GeometryExtensions.GrahamScan(Enumerable.Distinct<PdfPoint>(points))));
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00029E54 File Offset: 0x00028054
		public static PdfRectangle OrientedBoundingBox(IReadOnlyList<PdfPoint> points)
		{
			if (points == null || points.Count < 2)
			{
				throw new ArgumentException("OrientedBoundingBox(): points cannot be null and must contain at least two points.", "points");
			}
			double num = Enumerable.Average<PdfPoint>(points, (PdfPoint p) => p.X);
			double num2 = Enumerable.Average<PdfPoint>(points, (PdfPoint p) => p.Y);
			double num3 = 0.0;
			double num4 = 0.0;
			for (int i = 0; i < points.Count; i++)
			{
				PdfPoint pdfPoint = points[i];
				double num5 = pdfPoint.X - num;
				double num6 = pdfPoint.Y - num2;
				num3 += num5 * num6;
				num4 += num5 * num5;
			}
			double num7 = Math.Atan(num3 / num4);
			double num8 = Math.Cos(num7);
			double num9 = Math.Sin(num7);
			TransformationMatrix inverseRotation = new TransformationMatrix(num8, -num9, 0.0, num9, num8, 0.0, 0.0, 0.0, 1.0);
			PdfPoint[] array = Enumerable.ToArray<PdfPoint>(Enumerable.Select<PdfPoint, PdfPoint>(points, (PdfPoint p) => inverseRotation.Transform(p)));
			PdfRectangle pdfRectangle;
			pdfRectangle..ctor(Enumerable.Min<PdfPoint>(array, (PdfPoint p) => p.X), Enumerable.Min<PdfPoint>(array, (PdfPoint p) => p.Y), Enumerable.Max<PdfPoint>(array, (PdfPoint p) => p.X), Enumerable.Max<PdfPoint>(array, (PdfPoint p) => p.Y));
			TransformationMatrix transformationMatrix;
			transformationMatrix..ctor(num8, num9, 0.0, -num9, num8, 0.0, 0.0, 0.0, 1.0);
			return transformationMatrix.Transform(pdfRectangle);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0002A084 File Offset: 0x00028284
		public static IEnumerable<PdfPoint> GrahamScan(IEnumerable<PdfPoint> points)
		{
			if (points == null || !Enumerable.Any<PdfPoint>(points))
			{
				throw new ArgumentException("GrahamScan(): points cannot be null and must contain at least one point.", "points");
			}
			if (Enumerable.Count<PdfPoint>(points) < 3)
			{
				return points;
			}
			Stack<PdfPoint> stack = new Stack<PdfPoint>();
			List<PdfPoint> list = Enumerable.ToList<PdfPoint>(Enumerable.ThenBy<PdfPoint, double>(Enumerable.OrderBy<PdfPoint, double>(points, (PdfPoint p) => p.X), (PdfPoint p) => p.Y));
			PdfPoint P0 = list[0];
			IEnumerable<IGrouping<double, PdfPoint>> enumerable = Enumerable.OrderBy<IGrouping<double, PdfPoint>, double>(Enumerable.GroupBy<PdfPoint, double>(Enumerable.Skip<PdfPoint>(list, 1), (PdfPoint p) => GeometryExtensions.<GrahamScan>g__polarAngle|8_0(P0, p)), (IGrouping<double, PdfPoint> g) => g.Key);
			list = new List<PdfPoint>();
			Func<PdfPoint, double> <>9__5;
			foreach (IGrouping<double, PdfPoint> grouping in enumerable)
			{
				if (Enumerable.Count<PdfPoint>(grouping) == 1)
				{
					list.Add(Enumerable.First<PdfPoint>(grouping));
				}
				else
				{
					List<PdfPoint> list2 = list;
					IEnumerable<PdfPoint> enumerable2 = grouping;
					Func<PdfPoint, double> func;
					if ((func = <>9__5) == null)
					{
						func = (<>9__5 = delegate(PdfPoint p)
						{
							double num = p.X - P0.X;
							double num2 = p.Y - P0.Y;
							return num * num + num2 * num2;
						});
					}
					list2.Add(Enumerable.First<PdfPoint>(Enumerable.OrderByDescending<PdfPoint, double>(enumerable2, func)));
				}
			}
			if (list.Count < 2)
			{
				return new PdfPoint[]
				{
					P0,
					list[0]
				};
			}
			stack.Push(P0);
			stack.Push(list[0]);
			stack.Push(list[1]);
			for (int i = 2; i < list.Count; i++)
			{
				PdfPoint pdfPoint = list[i];
				while (stack.Count > 1 && !GeometryExtensions.ccw(Enumerable.ElementAt<PdfPoint>(stack, 1), stack.Peek(), pdfPoint))
				{
					stack.Pop();
				}
				stack.Push(pdfPoint);
			}
			return stack;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0002A288 File Offset: 0x00028488
		public static bool Contains(this PdfRectangle rectangle, PdfPoint point, bool includeBorder = false)
		{
			if (Math.Abs(rectangle.Rotation) >= 1E-05)
			{
				double num = GeometryExtensions.<Contains>g__area|9_0(rectangle.BottomLeft, point, rectangle.TopLeft);
				double num2 = GeometryExtensions.<Contains>g__area|9_0(rectangle.TopLeft, point, rectangle.TopRight);
				double num3 = GeometryExtensions.<Contains>g__area|9_0(rectangle.TopRight, point, rectangle.BottomRight);
				double num4 = GeometryExtensions.<Contains>g__area|9_0(rectangle.BottomRight, point, rectangle.BottomLeft);
				return num + num2 + num3 + num4 - rectangle.Area <= 1E-05 && ((num >= 1E-05 && num2 >= 1E-05 && num3 >= 1E-05 && num4 >= 1E-05) || includeBorder);
			}
			if (includeBorder)
			{
				return point.X >= rectangle.Left && point.X <= rectangle.Right && point.Y >= rectangle.Bottom && point.Y <= rectangle.Top;
			}
			return point.X > rectangle.Left && point.X < rectangle.Right && point.Y > rectangle.Bottom && point.Y < rectangle.Top;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002A3E0 File Offset: 0x000285E0
		public static bool Contains(this PdfRectangle rectangle, PdfRectangle other, bool includeBorder = false)
		{
			return rectangle.Contains(other.BottomLeft, includeBorder) && rectangle.Contains(other.TopRight, includeBorder) && rectangle.Contains(other.BottomRight, includeBorder) && rectangle.Contains(other.TopLeft, includeBorder);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002A438 File Offset: 0x00028638
		public static bool IntersectsWith(this PdfRectangle rectangle, PdfRectangle other)
		{
			if (Math.Abs(rectangle.Rotation) < 1E-05 && Math.Abs(other.Rotation) < 1E-05)
			{
				return rectangle.Left <= other.Right && other.Left <= rectangle.Right && rectangle.Top >= other.Bottom && other.Top >= rectangle.Bottom;
			}
			PdfRectangle rectangle2 = rectangle.Normalise();
			PdfRectangle other2 = other.Normalise();
			return (Math.Abs(rectangle2.Rotation) >= 1E-05 || Math.Abs(other2.Rotation) >= 1E-05 || rectangle2.IntersectsWith(other2)) && (rectangle.Contains(other.BottomLeft, false) || rectangle.Contains(other.TopRight, false) || rectangle.Contains(other.TopLeft, false) || rectangle.Contains(other.BottomRight, false) || other.Contains(rectangle.BottomLeft, false) || other.Contains(rectangle.TopRight, false) || other.Contains(rectangle.TopLeft, false) || other.Contains(rectangle.BottomRight, false) || GeometryExtensions.IntersectsWith(rectangle.BottomLeft, rectangle.BottomRight, other.BottomLeft, other.BottomRight) || GeometryExtensions.IntersectsWith(rectangle.BottomLeft, rectangle.BottomRight, other.BottomRight, other.TopRight) || GeometryExtensions.IntersectsWith(rectangle.BottomLeft, rectangle.BottomRight, other.TopRight, other.TopLeft) || GeometryExtensions.IntersectsWith(rectangle.BottomLeft, rectangle.BottomRight, other.TopLeft, other.BottomLeft) || GeometryExtensions.IntersectsWith(rectangle.BottomRight, rectangle.TopRight, other.BottomLeft, other.BottomRight) || GeometryExtensions.IntersectsWith(rectangle.BottomRight, rectangle.TopRight, other.BottomRight, other.TopRight) || GeometryExtensions.IntersectsWith(rectangle.BottomRight, rectangle.TopRight, other.TopRight, other.TopLeft) || GeometryExtensions.IntersectsWith(rectangle.BottomRight, rectangle.TopRight, other.TopLeft, other.BottomLeft) || GeometryExtensions.IntersectsWith(rectangle.TopRight, rectangle.TopLeft, other.BottomLeft, other.BottomRight) || GeometryExtensions.IntersectsWith(rectangle.TopRight, rectangle.TopLeft, other.BottomRight, other.TopRight) || GeometryExtensions.IntersectsWith(rectangle.TopRight, rectangle.TopLeft, other.TopRight, other.TopLeft) || GeometryExtensions.IntersectsWith(rectangle.TopRight, rectangle.TopLeft, other.TopLeft, other.BottomLeft) || GeometryExtensions.IntersectsWith(rectangle.TopLeft, rectangle.BottomLeft, other.BottomLeft, other.BottomRight) || GeometryExtensions.IntersectsWith(rectangle.TopLeft, rectangle.BottomLeft, other.BottomRight, other.TopRight) || GeometryExtensions.IntersectsWith(rectangle.TopLeft, rectangle.BottomLeft, other.TopRight, other.TopLeft) || GeometryExtensions.IntersectsWith(rectangle.TopLeft, rectangle.BottomLeft, other.TopLeft, other.BottomLeft));
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002A7E4 File Offset: 0x000289E4
		public static PdfRectangle? Intersect(this PdfRectangle rectangle, PdfRectangle other)
		{
			if (!rectangle.IntersectsWith(other))
			{
				return default(PdfRectangle?);
			}
			return new PdfRectangle?(new PdfRectangle(Math.Max(rectangle.BottomLeft.X, other.BottomLeft.X), Math.Max(rectangle.BottomLeft.Y, other.BottomLeft.Y), Math.Min(rectangle.TopRight.X, other.TopRight.X), Math.Min(rectangle.TopRight.Y, other.TopRight.Y)));
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0002A89C File Offset: 0x00028A9C
		public static PdfRectangle Normalise(this PdfRectangle rectangle)
		{
			PdfPoint[] array = new PdfPoint[]
			{
				rectangle.BottomLeft,
				rectangle.BottomRight,
				rectangle.TopLeft,
				rectangle.TopRight
			};
			return new PdfRectangle(Enumerable.Min<PdfPoint>(array, (PdfPoint p) => p.X), Enumerable.Min<PdfPoint>(array, (PdfPoint p) => p.Y), Enumerable.Max<PdfPoint>(array, (PdfPoint p) => p.X), Enumerable.Max<PdfPoint>(array, (PdfPoint p) => p.Y));
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0002A981 File Offset: 0x00028B81
		public static bool IntersectsWith(this PdfRectangle rectangle, PdfLine line)
		{
			return GeometryExtensions.IntersectsWith(rectangle, line.Point1, line.Point2);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002A998 File Offset: 0x00028B98
		public static PdfLine? Intersect(this PdfRectangle rectangle, PdfLine line)
		{
			PdfPoint[] array = GeometryExtensions.Intersect(rectangle, line.Point1, line.Point2);
			if (array != null)
			{
				return new PdfLine?(new PdfLine(array[0], array[1]));
			}
			return default(PdfLine?);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0002A9E0 File Offset: 0x00028BE0
		public static List<PdfLine> Intersect(this PdfRectangle rectangle, List<PdfLine> lines)
		{
			Clipper clipper = new Clipper(0);
			clipper.AddPath(Enumerable.ToList<ClipperIntPoint>(rectangle.ToClipperPolygon()), ClipperPolyType.Clip, true);
			foreach (PdfLine line in lines)
			{
				clipper.AddPath(line.ToClipperIntPoint(), ClipperPolyType.Subject, false);
			}
			ClipperPolyTree clipperPolyTree = new ClipperPolyTree();
			if (clipper.Execute(ClipperClipType.Intersection, clipperPolyTree, ClipperPolyFillType.EvenOdd))
			{
				List<PdfLine> list = new List<PdfLine>();
				foreach (ClipperPolyNode clipperPolyNode in clipperPolyTree.Children)
				{
					list.Add(new PdfLine(new PdfPoint((double)clipperPolyNode.Contour[0].X / 10000.0, (double)clipperPolyNode.Contour[0].Y / 10000.0), new PdfPoint((double)clipperPolyNode.Contour[1].X / 10000.0, (double)clipperPolyNode.Contour[1].Y / 10000.0)));
				}
				return list;
			}
			return new List<PdfLine>();
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0002AB44 File Offset: 0x00028D44
		public static bool Contains(this PdfLine line, PdfPoint point)
		{
			return GeometryExtensions.Contains(line.Point1, line.Point2, point);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0002AB5A File Offset: 0x00028D5A
		public static bool IntersectsWith(this PdfLine line, PdfLine other)
		{
			return GeometryExtensions.IntersectsWith(line.Point1, line.Point2, other.Point1, other.Point2);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0002AB7D File Offset: 0x00028D7D
		public static bool IntersectsWith(this PdfLine line, PdfSubpath.Line other)
		{
			return GeometryExtensions.IntersectsWith(line.Point1, line.Point2, other.From, other.To);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0002AB9E File Offset: 0x00028D9E
		public static PdfPoint? Intersect(this PdfLine line, PdfLine other)
		{
			return GeometryExtensions.Intersect(line.Point1, line.Point2, other.Point1, other.Point2);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0002ABC1 File Offset: 0x00028DC1
		public static PdfPoint? Intersect(this PdfLine line, PdfSubpath.Line other)
		{
			return GeometryExtensions.Intersect(line.Point1, line.Point2, other.From, other.To);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0002ABE2 File Offset: 0x00028DE2
		public static bool ParallelTo(this PdfLine line, PdfLine other)
		{
			return GeometryExtensions.ParallelTo(line.Point1, line.Point2, other.Point1, other.Point2);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002AC05 File Offset: 0x00028E05
		public static bool ParallelTo(this PdfLine line, PdfSubpath.Line other)
		{
			return GeometryExtensions.ParallelTo(line.Point1, line.Point2, other.From, other.To);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002AC26 File Offset: 0x00028E26
		public static PdfLine? Intersect(this PdfLine line, PdfRectangle rectangle)
		{
			return rectangle.Intersect(line);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002AC2F File Offset: 0x00028E2F
		public static bool IntersectsWith(this PdfLine line, PdfRectangle rectangle)
		{
			return rectangle.IntersectsWith(line);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002AC38 File Offset: 0x00028E38
		public static bool Contains(this PdfSubpath.Line line, PdfPoint point)
		{
			return GeometryExtensions.Contains(line.From, line.To, point);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002AC4C File Offset: 0x00028E4C
		public static bool IntersectsWith(this PdfSubpath.Line line, PdfSubpath.Line other)
		{
			return GeometryExtensions.IntersectsWith(line.From, line.To, other.From, other.To);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002AC6B File Offset: 0x00028E6B
		public static bool IntersectsWith(this PdfSubpath.Line line, PdfLine other)
		{
			return GeometryExtensions.IntersectsWith(line.From, line.To, other.Point1, other.Point2);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002AC8C File Offset: 0x00028E8C
		public static PdfPoint? Intersect(this PdfSubpath.Line line, PdfSubpath.Line other)
		{
			return GeometryExtensions.Intersect(line.From, line.To, other.From, other.To);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002ACAB File Offset: 0x00028EAB
		public static PdfPoint? Intersect(this PdfSubpath.Line line, PdfLine other)
		{
			return GeometryExtensions.Intersect(line.From, line.To, other.Point1, other.Point2);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002ACCC File Offset: 0x00028ECC
		public static bool ParallelTo(this PdfSubpath.Line line, PdfSubpath.Line other)
		{
			return GeometryExtensions.ParallelTo(line.From, line.To, other.From, other.To);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002ACEB File Offset: 0x00028EEB
		public static bool ParallelTo(this PdfSubpath.Line line, PdfLine other)
		{
			return GeometryExtensions.ParallelTo(line.From, line.To, other.Point1, other.Point2);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002AD0C File Offset: 0x00028F0C
		private static bool Contains(PdfPoint pl1, PdfPoint pl2, PdfPoint point)
		{
			if (Math.Abs(pl2.X - pl1.X) < 1E-05)
			{
				return Math.Abs(point.X - pl2.X) < 1E-05 && (double)Math.Abs(Math.Sign(point.Y - pl2.Y) - Math.Sign(point.Y - pl1.Y)) > 1E-05;
			}
			if (Math.Abs(pl2.Y - pl1.Y) < 1E-05)
			{
				return Math.Abs(point.Y - pl2.Y) < 1E-05 && (double)Math.Abs(Math.Sign(point.X - pl2.X) - Math.Sign(point.X - pl1.X)) > 1E-05;
			}
			double num = (point.X - pl1.X) / (pl2.X - pl1.X);
			double num2 = (point.Y - pl1.Y) / (pl2.Y - pl1.Y);
			return Math.Abs(num - num2) <= 1E-05 && num >= 0.0 && num - 1.0 <= 1E-05;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0002AE8B File Offset: 0x0002908B
		public static bool IntersectsWith(PdfPoint p11, PdfPoint p12, PdfPoint p21, PdfPoint p22)
		{
			return GeometryExtensions.ccw(p11, p12, p21) != GeometryExtensions.ccw(p11, p12, p22) && GeometryExtensions.ccw(p21, p22, p11) != GeometryExtensions.ccw(p21, p22, p12);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0002AEB8 File Offset: 0x000290B8
		private static PdfPoint? Intersect(PdfPoint p11, PdfPoint p12, PdfPoint p21, PdfPoint p22)
		{
			if (!GeometryExtensions.IntersectsWith(p11, p12, p21, p22))
			{
				return default(PdfPoint?);
			}
			ValueTuple<double, double> slopeIntercept = GeometryExtensions.GetSlopeIntercept(p11, p12);
			double item = slopeIntercept.Item1;
			double item2 = slopeIntercept.Item2;
			ValueTuple<double, double> slopeIntercept2 = GeometryExtensions.GetSlopeIntercept(p21, p22);
			double item3 = slopeIntercept2.Item1;
			double item4 = slopeIntercept2.Item2;
			if (double.IsNaN(item))
			{
				double num = item2;
				double num2 = item3 * num + item4;
				return new PdfPoint?(new PdfPoint(num, num2));
			}
			if (double.IsNaN(item3))
			{
				double num3 = item4;
				double num4 = item * num3 + item2;
				return new PdfPoint?(new PdfPoint(num3, num4));
			}
			double num5 = (item4 - item2) / (item - item3);
			double num6 = item * num5 + item2;
			return new PdfPoint?(new PdfPoint(num5, num6));
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0002AF68 File Offset: 0x00029168
		private static PdfPoint[] Intersect(PdfRectangle rectangle, PdfPoint pl1, PdfPoint pl2)
		{
			Clipper clipper = new Clipper(0);
			clipper.AddPath(Enumerable.ToList<ClipperIntPoint>(rectangle.ToClipperPolygon()), ClipperPolyType.Clip, true);
			List<ClipperIntPoint> list = new List<ClipperIntPoint>();
			list.Add(pl1.ToClipperIntPoint());
			list.Add(pl2.ToClipperIntPoint());
			clipper.AddPath(list, ClipperPolyType.Subject, false);
			ClipperPolyTree clipperPolyTree = new ClipperPolyTree();
			if (!clipper.Execute(ClipperClipType.Intersection, clipperPolyTree, ClipperPolyFillType.EvenOdd))
			{
				return null;
			}
			if (clipperPolyTree.Children.Count == 0)
			{
				return null;
			}
			if (clipperPolyTree.Children.Count == 1)
			{
				ClipperPolyNode clipperPolyNode = clipperPolyTree.Children[0];
				return new PdfPoint[]
				{
					new PdfPoint((double)clipperPolyNode.Contour[0].X / 10000.0, (double)clipperPolyNode.Contour[0].Y / 10000.0),
					new PdfPoint((double)clipperPolyNode.Contour[1].X / 10000.0, (double)clipperPolyNode.Contour[1].Y / 10000.0)
				};
			}
			throw new ArgumentException("GeometryExtensions.Intersect(PdfRectangle, PdfPoint, PdfPoint): more than one solution found.");
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002B090 File Offset: 0x00029290
		public static bool IntersectsWith(PdfRectangle rectangle, PdfPoint pl1, PdfPoint pl2)
		{
			Clipper clipper = new Clipper(0);
			clipper.AddPath(Enumerable.ToList<ClipperIntPoint>(rectangle.ToClipperPolygon()), ClipperPolyType.Clip, true);
			List<ClipperIntPoint> list = new List<ClipperIntPoint>();
			list.Add(pl1.ToClipperIntPoint());
			list.Add(pl2.ToClipperIntPoint());
			clipper.AddPath(list, ClipperPolyType.Subject, false);
			ClipperPolyTree clipperPolyTree = new ClipperPolyTree();
			return clipper.Execute(ClipperClipType.Intersection, clipperPolyTree, ClipperPolyFillType.EvenOdd) && clipperPolyTree.Children.Count > 0;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002B100 File Offset: 0x00029300
		private static bool ParallelTo(PdfPoint p11, PdfPoint p12, PdfPoint p21, PdfPoint p22)
		{
			return Math.Abs((p12.Y - p11.Y) * (p22.X - p21.X) - (p22.Y - p21.Y) * (p12.X - p11.X)) < 1E-05;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002B15C File Offset: 0x0002935C
		public static ValueTuple<PdfSubpath.BezierCurve, PdfSubpath.BezierCurve> Split(this PdfSubpath.BezierCurve bezierCurve, double tau)
		{
			PdfPoint[][] array = new PdfPoint[][]
			{
				new PdfPoint[]
				{
					bezierCurve.StartPoint,
					bezierCurve.FirstControlPoint,
					bezierCurve.SecondControlPoint,
					bezierCurve.EndPoint
				},
				new PdfPoint[3],
				new PdfPoint[2],
				new PdfPoint[1]
			};
			for (int i = 1; i <= 3; i++)
			{
				for (int j = 0; j <= 3 - i; j++)
				{
					double num = (1.0 - tau) * array[i - 1][j].X + tau * array[i - 1][j + 1].X;
					double num2 = (1.0 - tau) * array[i - 1][j].Y + tau * array[i - 1][j + 1].Y;
					array[i][j] = new PdfPoint(num, num2);
				}
			}
			return new ValueTuple<PdfSubpath.BezierCurve, PdfSubpath.BezierCurve>(new PdfSubpath.BezierCurve(array[0][0], array[1][0], array[2][0], array[3][0]), new PdfSubpath.BezierCurve(array[3][0], array[2][1], array[1][2], array[0][3]));
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002B2B5 File Offset: 0x000294B5
		public static bool IntersectsWith(this PdfSubpath.BezierCurve bezierCurve, PdfLine line)
		{
			return GeometryExtensions.IntersectsWith(bezierCurve, line.Point1, line.Point2);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002B2CB File Offset: 0x000294CB
		public static bool IntersectsWith(this PdfSubpath.BezierCurve bezierCurve, PdfSubpath.Line line)
		{
			return GeometryExtensions.IntersectsWith(bezierCurve, line.From, line.To);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002B2DF File Offset: 0x000294DF
		private static bool IntersectsWith(PdfSubpath.BezierCurve bezierCurve, PdfPoint p1, PdfPoint p2)
		{
			return GeometryExtensions.Intersect(bezierCurve, p1, p2).Length != 0;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0002B2ED File Offset: 0x000294ED
		public static PdfPoint[] Intersect(this PdfSubpath.BezierCurve bezierCurve, PdfLine line)
		{
			return GeometryExtensions.Intersect(bezierCurve, line.Point1, line.Point2);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0002B303 File Offset: 0x00029503
		public static PdfPoint[] Intersect(this PdfSubpath.BezierCurve bezierCurve, PdfSubpath.Line line)
		{
			return GeometryExtensions.Intersect(bezierCurve, line.From, line.To);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002B318 File Offset: 0x00029518
		private static PdfPoint[] Intersect(PdfSubpath.BezierCurve bezierCurve, PdfPoint p1, PdfPoint p2)
		{
			double[] array = GeometryExtensions.IntersectT(bezierCurve, p1, p2);
			if (array == null || array.Length == 0)
			{
				return EmptyArray<PdfPoint>.Instance;
			}
			List<PdfPoint> list = new List<PdfPoint>();
			foreach (double num in array)
			{
				PdfPoint pdfPoint;
				pdfPoint..ctor(PdfSubpath.BezierCurve.ValueWithT(bezierCurve.StartPoint.X, bezierCurve.FirstControlPoint.X, bezierCurve.SecondControlPoint.X, bezierCurve.EndPoint.X, num), PdfSubpath.BezierCurve.ValueWithT(bezierCurve.StartPoint.Y, bezierCurve.FirstControlPoint.Y, bezierCurve.SecondControlPoint.Y, bezierCurve.EndPoint.Y, num));
				if (GeometryExtensions.Contains(p1, p2, pdfPoint))
				{
					list.Add(pdfPoint);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002B402 File Offset: 0x00029602
		public static double[] IntersectT(this PdfSubpath.BezierCurve bezierCurve, PdfLine line)
		{
			return GeometryExtensions.IntersectT(bezierCurve, line.Point1, line.Point2);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002B418 File Offset: 0x00029618
		public static double[] IntersectT(this PdfSubpath.BezierCurve bezierCurve, PdfSubpath.Line line)
		{
			return GeometryExtensions.IntersectT(bezierCurve, line.From, line.To);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002B42C File Offset: 0x0002962C
		private static double[] IntersectT(PdfSubpath.BezierCurve bezierCurve, PdfPoint p1, PdfPoint p2)
		{
			PdfRectangle? boundingRectangle = bezierCurve.GetBoundingRectangle();
			if (boundingRectangle == null)
			{
				return null;
			}
			if (boundingRectangle.Value.Left > Math.Max(p1.X, p2.X) || Math.Min(p1.X, p2.X) > boundingRectangle.Value.Right)
			{
				return null;
			}
			if (boundingRectangle.Value.Top < Math.Min(p1.Y, p2.Y) || Math.Max(p1.Y, p2.Y) < boundingRectangle.Value.Bottom)
			{
				return null;
			}
			double num = p2.Y - p1.Y;
			double num2 = p1.X - p2.X;
			double num3 = p1.X * (p1.Y - p2.Y) + p1.Y * (p2.X - p1.X);
			double num4 = bezierCurve.StartPoint.X * num + bezierCurve.StartPoint.Y * num2;
			double num5 = 3.0 * (bezierCurve.FirstControlPoint.X * num + bezierCurve.FirstControlPoint.Y * num2);
			double num6 = 3.0 * (bezierCurve.SecondControlPoint.X * num + bezierCurve.SecondControlPoint.Y * num2);
			double num7 = bezierCurve.EndPoint.X * num + bezierCurve.EndPoint.Y * num2;
			double a = -num4 + num5 - num6 + num7;
			double b = 3.0 * num4 - 2.0 * num5 + num6;
			double c = -3.0 * num4 + num5;
			double d = num4 + num3;
			return Enumerable.ToArray<double>(Enumerable.OrderBy<double, double>(Enumerable.Where<double>(GeometryExtensions.SolveCubicEquation(a, b, c, d), (double s) => !double.IsNaN(s) && s >= -1E-05 && s - 1.0 <= 1E-05), (double s) => s));
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0002B674 File Offset: 0x00029874
		private static double CrossProduct(ClipperIntPoint pt1, ClipperIntPoint pt2, ClipperIntPoint pt3)
		{
			return (double)((pt2.X - pt1.X) * (pt3.Y - pt2.Y) - (pt2.Y - pt1.Y) * (pt3.X - pt2.X));
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002B6B0 File Offset: 0x000298B0
		private static int PointInPathsWindingCount(ClipperIntPoint pt, List<List<ClipperIntPoint>> paths)
		{
			int num = 0;
			for (int i = 0; i < paths.Count; i++)
			{
				int j = 0;
				List<ClipperIntPoint> list = paths[i];
				int count = list.Count;
				if (count >= 3)
				{
					ClipperIntPoint clipperIntPoint = list[count - 1];
					while (j < count && list[j].Y == clipperIntPoint.Y)
					{
						j++;
					}
					if (j != count)
					{
						bool flag = clipperIntPoint.Y < pt.Y;
						while (j < count)
						{
							if (flag)
							{
								while (j < count && list[j].Y < pt.Y)
								{
									j++;
								}
								if (j == count)
								{
									break;
								}
								if (j > 0)
								{
									clipperIntPoint = list[j - 1];
								}
								double num2 = GeometryExtensions.CrossProduct(clipperIntPoint, list[j], pt);
								if (num2 == 0.0)
								{
									return int.MaxValue;
								}
								if (num2 < 0.0)
								{
									num--;
								}
							}
							else
							{
								while (j < count && list[j].Y > pt.Y)
								{
									j++;
								}
								if (j == count)
								{
									break;
								}
								if (j > 0)
								{
									clipperIntPoint = list[j - 1];
								}
								double num3 = GeometryExtensions.CrossProduct(clipperIntPoint, list[j], pt);
								if (num3 == 0.0)
								{
									return int.MaxValue;
								}
								if (num3 > 0.0)
								{
									num++;
								}
							}
							j++;
							flag = !flag;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0002B828 File Offset: 0x00029A28
		private static bool PointInPaths(ClipperIntPoint pt, List<List<ClipperIntPoint>> paths, ClipperPolyFillType fillRule, bool includeBorder)
		{
			int num = GeometryExtensions.PointInPathsWindingCount(pt, paths);
			if (num == 2147483647)
			{
				return includeBorder;
			}
			if (fillRule == ClipperPolyFillType.EvenOdd || fillRule != ClipperPolyFillType.NonZero)
			{
				return num % 2 != 0;
			}
			return num != 0;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0002B859 File Offset: 0x00029A59
		public static bool Contains(this PdfSubpath subpath, PdfPoint point, bool includeBorder = false)
		{
			ClipperIntPoint pt = point.ToClipperIntPoint();
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>();
			list.Add(Enumerable.ToList<ClipperIntPoint>(subpath.ToClipperPolygon()));
			return GeometryExtensions.PointInPaths(pt, list, ClipperPolyFillType.EvenOdd, includeBorder);
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002B880 File Offset: 0x00029A80
		public static bool Contains(this PdfSubpath subpath, PdfRectangle rectangle, bool includeBorder = false)
		{
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>();
			list.Add(Enumerable.ToList<ClipperIntPoint>(subpath.ToClipperPolygon()));
			List<List<ClipperIntPoint>> paths = list;
			using (IEnumerator<ClipperIntPoint> enumerator = rectangle.ToClipperPolygon().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!GeometryExtensions.PointInPaths(enumerator.Current, paths, ClipperPolyFillType.EvenOdd, includeBorder))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002B8F0 File Offset: 0x00029AF0
		public static bool Contains(this PdfSubpath subpath, PdfSubpath other, bool includeBorder = false)
		{
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>();
			list.Add(Enumerable.ToList<ClipperIntPoint>(subpath.ToClipperPolygon()));
			List<List<ClipperIntPoint>> paths = list;
			using (IEnumerator<ClipperIntPoint> enumerator = other.ToClipperPolygon().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!GeometryExtensions.PointInPaths(enumerator.Current, paths, ClipperPolyFillType.EvenOdd, includeBorder))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002B960 File Offset: 0x00029B60
		public static double GetArea(this PdfPath path)
		{
			List<List<ClipperIntPoint>> list = Clipper.SimplifyPolygons(Enumerable.ToList<List<ClipperIntPoint>>(Enumerable.Select<PdfSubpath, List<ClipperIntPoint>>(path, (PdfSubpath sp) => Enumerable.ToList<ClipperIntPoint>(sp.ToClipperPolygon()))), (path.FillingRule == 2) ? ClipperPolyFillType.NonZero : ClipperPolyFillType.EvenOdd);
			double num = 0.0;
			foreach (List<ClipperIntPoint> poly in list)
			{
				num += Clipper.Area(poly);
			}
			return num;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002B9F8 File Offset: 0x00029BF8
		public static bool Contains(this PdfPath path, PdfPoint point, bool includeBorder = false)
		{
			List<List<ClipperIntPoint>> paths = Enumerable.ToList<List<ClipperIntPoint>>(Enumerable.Select<PdfSubpath, List<ClipperIntPoint>>(path, (PdfSubpath sp) => Enumerable.ToList<ClipperIntPoint>(sp.ToClipperPolygon())));
			return GeometryExtensions.PointInPaths(point.ToClipperIntPoint(), paths, (path.FillingRule == 2) ? ClipperPolyFillType.NonZero : ClipperPolyFillType.EvenOdd, includeBorder);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0002BA4C File Offset: 0x00029C4C
		public static bool Contains(this PdfPath path, PdfRectangle rectangle, bool includeBorder = false)
		{
			List<List<ClipperIntPoint>> paths = Enumerable.ToList<List<ClipperIntPoint>>(Enumerable.Select<PdfSubpath, List<ClipperIntPoint>>(path, (PdfSubpath sp) => Enumerable.ToList<ClipperIntPoint>(sp.ToClipperPolygon())));
			ClipperPolyFillType fillRule = (path.FillingRule == 2) ? ClipperPolyFillType.NonZero : ClipperPolyFillType.EvenOdd;
			using (IEnumerator<ClipperIntPoint> enumerator = rectangle.ToClipperPolygon().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!GeometryExtensions.PointInPaths(enumerator.Current, paths, fillRule, includeBorder))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0002BADC File Offset: 0x00029CDC
		public static bool Contains(this PdfPath path, PdfSubpath subpath, bool includeBorder = false)
		{
			List<List<ClipperIntPoint>> paths = Enumerable.ToList<List<ClipperIntPoint>>(Enumerable.Select<PdfSubpath, List<ClipperIntPoint>>(path, (PdfSubpath sp) => Enumerable.ToList<ClipperIntPoint>(sp.ToClipperPolygon())));
			ClipperPolyFillType fillRule = (path.FillingRule == 2) ? ClipperPolyFillType.NonZero : ClipperPolyFillType.EvenOdd;
			using (IEnumerator<ClipperIntPoint> enumerator = subpath.ToClipperPolygon().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!GeometryExtensions.PointInPaths(enumerator.Current, paths, fillRule, includeBorder))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0002BB6C File Offset: 0x00029D6C
		public static bool Contains(this PdfPath path, PdfPath other, bool includeBorder = false)
		{
			List<List<ClipperIntPoint>> paths = Enumerable.ToList<List<ClipperIntPoint>>(Enumerable.Select<PdfSubpath, List<ClipperIntPoint>>(path, (PdfSubpath sp) => Enumerable.ToList<ClipperIntPoint>(sp.ToClipperPolygon())));
			ClipperPolyFillType fillRule = (path.FillingRule == 2) ? ClipperPolyFillType.NonZero : ClipperPolyFillType.EvenOdd;
			foreach (PdfSubpath pdfPath in other)
			{
				using (IEnumerator<ClipperIntPoint> enumerator2 = pdfPath.ToClipperPolygon().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (!GeometryExtensions.PointInPaths(enumerator2.Current, paths, fillRule, includeBorder))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0002BC34 File Offset: 0x00029E34
		[return: TupleElementNames(new string[]
		{
			"Slope",
			"Intercept"
		})]
		private static ValueTuple<double, double> GetSlopeIntercept(PdfPoint point1, PdfPoint point2)
		{
			if (Math.Abs(point1.X - point2.X) > 1E-05)
			{
				double num = (point2.Y - point1.Y) / (point2.X - point1.X);
				double num2 = point2.Y - num * point2.X;
				return new ValueTuple<double, double>(num, num2);
			}
			return new ValueTuple<double, double>(double.NaN, point1.X);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0002BCAF File Offset: 0x00029EAF
		private static double CubicRoot(double d)
		{
			if (d < 0.0)
			{
				return -Math.Pow(-d, 0.3333333333333333);
			}
			return Math.Pow(d, 0.3333333333333333);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0002BCE0 File Offset: 0x00029EE0
		private static double[] SolveCubicEquation(double a, double b, double c, double d)
		{
			if (Math.Abs(a) > 1E-05)
			{
				double num = a * a;
				double num2 = num * a;
				double num3 = b * b * b;
				double num4 = a * b * c;
				double num5 = b / (3.0 * a);
				double num6 = (3.0 * a * c - b * b) / (9.0 * num);
				double num7 = (9.0 * num4 - 27.0 * num * d - 2.0 * num3) / (54.0 * num2);
				double num8 = num6 * num6 * num6 + num7 * num7;
				double num9 = double.NaN;
				double num10 = double.NaN;
				double num15;
				if (num8 >= 0.0)
				{
					double num11 = Math.Sqrt(num8);
					double num12 = GeometryExtensions.CubicRoot(num7 + num11);
					double num13 = GeometryExtensions.CubicRoot(num7 - num11);
					double num14 = num12 + num13;
					num15 = num14 - num5;
					if (Math.Abs(0.86602540378444 * (num12 - num13)) <= 1E-05)
					{
						num9 = -num14 / 2.0 - num5;
					}
				}
				else
				{
					double p_ = num6 * 3.0;
					double q_ = -num7 * 2.0;
					num15 = GeometryExtensions.<SolveCubicEquation>g__vietTrigonometricSolution|64_0(p_, q_, 0.0) - num5;
					num9 = GeometryExtensions.<SolveCubicEquation>g__vietTrigonometricSolution|64_0(p_, q_, 1.0) - num5;
					num10 = GeometryExtensions.<SolveCubicEquation>g__vietTrigonometricSolution|64_0(p_, q_, 2.0) - num5;
				}
				return new double[]
				{
					num15,
					num9,
					num10
				};
			}
			double num16 = c * c - 4.0 * b * d;
			if (num16 >= 0.0)
			{
				double num17 = Math.Sqrt(num16);
				double num18 = 1.0 / (2.0 * b);
				double num19 = (-c + num17) * num18;
				double num20 = (-c - num17) * num18;
				return new double[]
				{
					num19,
					num20
				};
			}
			return EmptyArray<double>.Instance;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0002BEF0 File Offset: 0x0002A0F0
		internal static string ToSvg(this PdfSubpath p, double height)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PdfSubpath.IPathCommand pathCommand in p.Commands)
			{
				pathCommand.WriteSvg(stringBuilder, height);
			}
			if (stringBuilder.Length == 0)
			{
				return string.Empty;
			}
			if (stringBuilder.get_Chars(stringBuilder.Length - 1) == ' ')
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0002BF78 File Offset: 0x0002A178
		internal static string ToFullSvg(this PdfSubpath p, double height)
		{
			string text = p.ToSvg(height);
			PdfRectangle? boundingRectangle = p.GetBoundingRectangle();
			List<PdfRectangle> list = new List<PdfRectangle>();
			foreach (PdfSubpath.IPathCommand pathCommand in p.Commands)
			{
				PdfRectangle? boundingRectangle2 = pathCommand.GetBoundingRectangle();
				if (boundingRectangle2 != null)
				{
					list.Add(boundingRectangle2.Value);
				}
			}
			string text2 = "<path d='" + text + "' stroke='cyan' stroke-width='3'></path>";
			string text3 = (boundingRectangle != null) ? GeometryExtensions.<ToFullSvg>g__BboxToRect|66_0(boundingRectangle.Value, "yellow") : string.Empty;
			string text4 = string.Join(" ", Enumerable.Select<PdfRectangle, string>(list, (PdfRectangle x) => GeometryExtensions.<ToFullSvg>g__BboxToRect|66_0(x, "gray")));
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(95, 3);
			defaultInterpolatedStringHandler.AppendLiteral("<svg width='500' height='500'><g transform=\"scale(0.2, -0.2) translate(100, -700)\">");
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(text3);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(text4);
			defaultInterpolatedStringHandler.AppendLiteral("</g></svg>");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002C0B4 File Offset: 0x0002A2B4
		[CompilerGenerated]
		internal static double <GrahamScan>g__polarAngle|8_0(PdfPoint point1, PdfPoint point2)
		{
			return Math.Atan2(point2.Y - point1.Y, point2.X - point1.X) % 3.141592653589793;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0002C0E4 File Offset: 0x0002A2E4
		[CompilerGenerated]
		internal static double <Contains>g__area|9_0(PdfPoint p1, PdfPoint p2, PdfPoint p3)
		{
			return Math.Abs(p2.X * p1.Y - p1.X * p2.Y + (p3.X * p2.Y - p2.X * p3.Y) + (p1.X * p3.Y - p3.X * p1.Y)) / 2.0;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0002C160 File Offset: 0x0002A360
		[CompilerGenerated]
		internal static double <SolveCubicEquation>g__vietTrigonometricSolution|64_0(double p_, double q_, double k)
		{
			return 2.0 * Math.Sqrt(-p_ / 3.0) * Math.Cos(0.3333333333333333 * Math.Acos(3.0 * q_ / (2.0 * p_) * Math.Sqrt(-3.0 / p_)) - 6.283185307179586 * k / 3.0);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0002C1DC File Offset: 0x0002A3DC
		[CompilerGenerated]
		internal static string <ToFullSvg>g__BboxToRect|66_0(PdfRectangle box, string stroke)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(81, 5);
			defaultInterpolatedStringHandler.AppendLiteral("<rect x='");
			defaultInterpolatedStringHandler.AppendFormatted<double>(box.Left);
			defaultInterpolatedStringHandler.AppendLiteral("' y='");
			defaultInterpolatedStringHandler.AppendFormatted<double>(box.Bottom);
			defaultInterpolatedStringHandler.AppendLiteral("' width='");
			defaultInterpolatedStringHandler.AppendFormatted<double>(box.Width);
			defaultInterpolatedStringHandler.AppendLiteral("' height='");
			defaultInterpolatedStringHandler.AppendFormatted<double>(box.Height);
			defaultInterpolatedStringHandler.AppendLiteral("' stroke-width='2' fill='none' stroke='");
			defaultInterpolatedStringHandler.AppendFormatted(stroke);
			defaultInterpolatedStringHandler.AppendLiteral("'></rect>");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400047E RID: 1150
		private const double epsilon = 1E-05;

		// Token: 0x0400047F RID: 1151
		private const double OneThird = 0.3333333333333333;

		// Token: 0x04000480 RID: 1152
		private const double SqrtOfThree = 1.73205080756888;
	}
}
