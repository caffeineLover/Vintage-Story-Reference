using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Geometry.ClipperLibrary;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig.Logging;

namespace UglyToad.PdfPig.Geometry
{
	// Token: 0x02000189 RID: 393
	internal static class ClippingExtensions
	{
		// Token: 0x06000920 RID: 2336 RVA: 0x000295C4 File Offset: 0x000277C4
		public static PdfPath Clip(this PdfPath clipping, PdfPath subject, ILog log = null)
		{
			if (clipping == null)
			{
				throw new ArgumentNullException("clipping", "Clip: the clipping path cannot be null.");
			}
			if (!clipping.IsClipping)
			{
				throw new ArgumentException("Clip: the clipping path does not have the IsClipping flag set to true.", "clipping");
			}
			if (subject == null)
			{
				throw new ArgumentNullException("subject", "Clip: the subject path cannot be null.");
			}
			if (subject.Count == 0)
			{
				return subject;
			}
			Clipper clipper = new Clipper(0);
			foreach (PdfSubpath pdfSubpath in clipping)
			{
				if (pdfSubpath.Commands.Count != 0)
				{
					if (!pdfSubpath.IsClosed())
					{
						pdfSubpath.CloseSubpath();
					}
					if (!clipper.AddPath(Enumerable.ToList<ClipperIntPoint>(pdfSubpath.ToClipperPolygon()), ClipperPolyType.Clip, true) && log != null)
					{
						log.Error("ClippingExtensions.Clip(): failed to add clipping subpath.");
					}
				}
			}
			bool flag = subject.IsFilled || subject.IsClipping;
			foreach (PdfSubpath pdfSubpath2 in subject)
			{
				if (pdfSubpath2.Commands.Count != 0)
				{
					if (flag && !pdfSubpath2.IsClosed())
					{
						if (Enumerable.Count<PdfSubpath.IPathCommand>(pdfSubpath2.Commands, (PdfSubpath.IPathCommand sp) => sp is PdfSubpath.Line) < 2)
						{
							if (Enumerable.Count<PdfSubpath.IPathCommand>(pdfSubpath2.Commands, (PdfSubpath.IPathCommand sp) => sp is PdfSubpath.BezierCurve) == 0)
							{
								flag = false;
							}
						}
					}
					if (flag && !pdfSubpath2.IsClosed())
					{
						pdfSubpath2.CloseSubpath();
					}
					if (!clipper.AddPath(Enumerable.ToList<ClipperIntPoint>(pdfSubpath2.ToClipperPolygon()), ClipperPolyType.Subject, flag) && log != null)
					{
						log.Error("ClippingExtensions.Clip(): failed to add subject subpath for clipping.");
					}
				}
			}
			ClipperPolyFillType clipFillType = (clipping.FillingRule == 2) ? ClipperPolyFillType.NonZero : ClipperPolyFillType.EvenOdd;
			ClipperPolyFillType subjFillType = (subject.FillingRule == 2) ? ClipperPolyFillType.NonZero : ClipperPolyFillType.EvenOdd;
			if (!flag)
			{
				PdfPath pdfPath = subject.CloneEmpty();
				ClipperPolyTree clipperPolyTree = new ClipperPolyTree();
				if (clipper.Execute(ClipperClipType.Intersection, clipperPolyTree, subjFillType, clipFillType))
				{
					foreach (ClipperPolyNode clipperPolyNode in clipperPolyTree.Children)
					{
						if (clipperPolyNode.Contour.Count > 0)
						{
							PdfSubpath pdfSubpath3 = new PdfSubpath();
							pdfSubpath3.MoveTo((double)clipperPolyNode.Contour[0].X / 10000.0, (double)clipperPolyNode.Contour[0].Y / 10000.0);
							for (int i = 1; i < clipperPolyNode.Contour.Count; i++)
							{
								pdfSubpath3.LineTo((double)clipperPolyNode.Contour[i].X / 10000.0, (double)clipperPolyNode.Contour[i].Y / 10000.0);
							}
							pdfPath.Add(pdfSubpath3);
						}
					}
					if (pdfPath.Count > 0)
					{
						return pdfPath;
					}
				}
				return null;
			}
			PdfPath pdfPath2 = subject.CloneEmpty();
			List<List<ClipperIntPoint>> list = new List<List<ClipperIntPoint>>();
			if (!clipper.Execute(ClipperClipType.Intersection, list, subjFillType, clipFillType))
			{
				return null;
			}
			foreach (List<ClipperIntPoint> list2 in list)
			{
				if (list2.Count > 0)
				{
					PdfSubpath pdfSubpath4 = new PdfSubpath();
					pdfSubpath4.MoveTo((double)list2[0].X / 10000.0, (double)list2[0].Y / 10000.0);
					for (int j = 1; j < list2.Count; j++)
					{
						pdfSubpath4.LineTo((double)list2[j].X / 10000.0, (double)list2[j].Y / 10000.0);
					}
					pdfSubpath4.CloseSubpath();
					pdfPath2.Add(pdfSubpath4);
				}
			}
			if (pdfPath2.Count > 0)
			{
				return pdfPath2;
			}
			return null;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00029A0C File Offset: 0x00027C0C
		internal static IEnumerable<ClipperIntPoint> ToClipperPolygon(this PdfSubpath pdfPath)
		{
			if (pdfPath.Commands.Count == 0)
			{
				yield break;
			}
			PdfSubpath.Move move = pdfPath.Commands[0] as PdfSubpath.Move;
			if (move == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ToClipperPolygon(): First command is not a Move command. Type is '");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(pdfPath.Commands[0].GetType());
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "pdfPath");
			}
			ClipperIntPoint movePoint = move.Location.ToClipperIntPoint();
			yield return movePoint;
			if (pdfPath.Commands.Count == 1)
			{
				yield break;
			}
			int num;
			for (int i = 1; i < pdfPath.Commands.Count; i = num + 1)
			{
				PdfSubpath.IPathCommand pathCommand = pdfPath.Commands[i];
				if (pathCommand is PdfSubpath.Move)
				{
					throw new ArgumentException("ToClipperPolygon(): only one move allowed per subpath.", "pdfPath");
				}
				PdfSubpath.Line line = pathCommand as PdfSubpath.Line;
				if (line != null)
				{
					yield return line.From.ToClipperIntPoint();
					yield return line.To.ToClipperIntPoint();
				}
				else
				{
					PdfSubpath.BezierCurve bezierCurve = pathCommand as PdfSubpath.BezierCurve;
					if (bezierCurve != null)
					{
						foreach (PdfSubpath.Line lineB in bezierCurve.ToLines(10))
						{
							yield return lineB.From.ToClipperIntPoint();
							yield return lineB.To.ToClipperIntPoint();
							lineB = null;
						}
						IEnumerator<PdfSubpath.Line> enumerator = null;
					}
					else if (pathCommand is PdfSubpath.Close)
					{
						yield return movePoint;
					}
				}
				line = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00029A1C File Offset: 0x00027C1C
		internal static IEnumerable<ClipperIntPoint> ToClipperPolygon(this PdfRectangle rectangle)
		{
			yield return rectangle.BottomLeft.ToClipperIntPoint();
			yield return rectangle.TopLeft.ToClipperIntPoint();
			yield return rectangle.TopRight.ToClipperIntPoint();
			yield return rectangle.BottomRight.ToClipperIntPoint();
			yield break;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00029A2C File Offset: 0x00027C2C
		internal static ClipperIntPoint ToClipperIntPoint(this PdfPoint point)
		{
			return new ClipperIntPoint(point.X * 10000.0, point.Y * 10000.0);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00029A55 File Offset: 0x00027C55
		internal static List<ClipperIntPoint> ToClipperIntPoint(this PdfLine line)
		{
			List<ClipperIntPoint> list = new List<ClipperIntPoint>();
			list.Add(line.Point1.ToClipperIntPoint());
			list.Add(line.Point2.ToClipperIntPoint());
			return list;
		}

		// Token: 0x0400047C RID: 1148
		public const double Factor = 10000.0;

		// Token: 0x0400047D RID: 1149
		private const int LinesInCurve = 10;
	}
}
