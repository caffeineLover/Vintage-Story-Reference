using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000010 RID: 16
	public class PdfSubpath
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002E72 File Offset: 0x00001072
		public IReadOnlyList<PdfSubpath.IPathCommand> Commands
		{
			get
			{
				return this.commands;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002E7A File Offset: 0x0000107A
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002E82 File Offset: 0x00001082
		public bool IsDrawnAsRectangle { get; internal set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002E8B File Offset: 0x0000108B
		public bool IsClockwise
		{
			get
			{
				return this.IsClosed() && this.shoeLaceSum > 0.0;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002EA8 File Offset: 0x000010A8
		public bool IsCounterClockwise
		{
			get
			{
				return this.IsClosed() && this.shoeLaceSum < 0.0;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002EC8 File Offset: 0x000010C8
		public PdfPoint GetCentroid()
		{
			List<PdfSubpath.IPathCommand> list = Enumerable.ToList<PdfSubpath.IPathCommand>(Enumerable.Where<PdfSubpath.IPathCommand>(this.commands, (PdfSubpath.IPathCommand c) => c is PdfSubpath.Line || c is PdfSubpath.BezierCurve));
			if (list.Count == 0)
			{
				return default(PdfPoint);
			}
			List<PdfPoint> list2 = Enumerable.ToList<PdfPoint>(Enumerable.Select<PdfSubpath.IPathCommand, PdfPoint>(list, new Func<PdfSubpath.IPathCommand, PdfPoint>(PdfSubpath.GetStartPoint)));
			list2.AddRange(Enumerable.Select<PdfSubpath.IPathCommand, PdfPoint>(list, new Func<PdfSubpath.IPathCommand, PdfPoint>(PdfSubpath.GetEndPoint)));
			return new PdfPoint(Enumerable.Average<PdfPoint>(list2, (PdfPoint p) => p.X), Enumerable.Average<PdfPoint>(list2, (PdfPoint p) => p.Y));
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002F98 File Offset: 0x00001198
		internal static PdfPoint GetStartPoint(PdfSubpath.IPathCommand command)
		{
			PdfSubpath.Line line = command as PdfSubpath.Line;
			if (line != null)
			{
				return line.From;
			}
			PdfSubpath.BezierCurve bezierCurve = command as PdfSubpath.BezierCurve;
			if (bezierCurve != null)
			{
				return bezierCurve.StartPoint;
			}
			PdfSubpath.Move move = command as PdfSubpath.Move;
			if (move != null)
			{
				return move.Location;
			}
			throw new ArgumentException();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002FE0 File Offset: 0x000011E0
		internal static PdfPoint GetEndPoint(PdfSubpath.IPathCommand command)
		{
			PdfSubpath.Line line = command as PdfSubpath.Line;
			if (line != null)
			{
				return line.To;
			}
			PdfSubpath.BezierCurve bezierCurve = command as PdfSubpath.BezierCurve;
			if (bezierCurve != null)
			{
				return bezierCurve.EndPoint;
			}
			PdfSubpath.Move move = command as PdfSubpath.Move;
			if (move != null)
			{
				return move.Location;
			}
			throw new ArgumentException();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003028 File Offset: 0x00001228
		internal PdfSubpath Simplify(int n = 4)
		{
			PdfSubpath pdfSubpath = new PdfSubpath();
			PdfPoint startPoint = PdfSubpath.GetStartPoint(Enumerable.First<PdfSubpath.IPathCommand>(this.Commands));
			pdfSubpath.MoveTo(startPoint.X, startPoint.Y);
			foreach (PdfSubpath.IPathCommand pathCommand in this.Commands)
			{
				PdfSubpath.Line line = pathCommand as PdfSubpath.Line;
				if (line != null)
				{
					pdfSubpath.LineTo(line.To.X, line.To.Y);
				}
				else
				{
					PdfSubpath.BezierCurve bezierCurve = pathCommand as PdfSubpath.BezierCurve;
					if (bezierCurve != null)
					{
						foreach (PdfSubpath.Line line2 in bezierCurve.ToLines(n))
						{
							pdfSubpath.LineTo(line2.To.X, line2.To.Y);
						}
					}
				}
			}
			if (this.IsClosed())
			{
				PdfPoint startPoint2 = PdfSubpath.GetStartPoint(Enumerable.First<PdfSubpath.IPathCommand>(pdfSubpath.Commands));
				if (!startPoint2.Equals(PdfSubpath.GetEndPoint(Enumerable.Last<PdfSubpath.IPathCommand>(pdfSubpath.Commands))))
				{
					pdfSubpath.LineTo(startPoint2.X, startPoint2.Y);
				}
			}
			return pdfSubpath;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003198 File Offset: 0x00001398
		public void MoveTo(double x, double y)
		{
			this.currentPosition = new PdfPoint?(new PdfPoint(x, y));
			this.commands.Add(new PdfSubpath.Move(this.currentPosition.Value));
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000031C8 File Offset: 0x000013C8
		public void LineTo(double x, double y)
		{
			if (this.currentPosition != null)
			{
				this.shoeLaceSum += (x - this.currentPosition.Value.X) * (y + this.currentPosition.Value.Y);
				PdfPoint pdfPoint = new PdfPoint(x, y);
				this.commands.Add(new PdfSubpath.Line(this.currentPosition.Value, pdfPoint));
				this.currentPosition = new PdfPoint?(pdfPoint);
				return;
			}
			throw new ArgumentNullException("LineTo(): currentPosition is null.");
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003256 File Offset: 0x00001456
		public void Rectangle(double x, double y, double width, double height)
		{
			this.MoveTo(x, y);
			this.LineTo(x + width, y);
			this.LineTo(x + width, y + height);
			this.LineTo(x, y + height);
			this.CloseSubpath();
			this.IsDrawnAsRectangle = true;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003290 File Offset: 0x00001490
		public void BezierCurveTo(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			if (this.currentPosition != null)
			{
				this.shoeLaceSum += (x1 - this.currentPosition.Value.X) * (y1 + this.currentPosition.Value.Y);
				this.shoeLaceSum += (x2 - x1) * (y2 + y1);
				this.shoeLaceSum += (x3 - x2) * (y3 + y2);
				PdfPoint pdfPoint = new PdfPoint(x3, y3);
				this.commands.Add(new PdfSubpath.BezierCurve(this.currentPosition.Value, new PdfPoint(x1, y1), new PdfPoint(x2, y2), pdfPoint));
				this.currentPosition = new PdfPoint?(pdfPoint);
				return;
			}
			throw new ArgumentNullException("BezierCurveTo(): currentPosition is null.");
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003360 File Offset: 0x00001560
		public void CloseSubpath()
		{
			if (this.currentPosition != null)
			{
				PdfPoint startPoint = PdfSubpath.GetStartPoint(Enumerable.First<PdfSubpath.IPathCommand>(this.commands));
				if (!startPoint.Equals(this.currentPosition.Value))
				{
					this.shoeLaceSum += (startPoint.X - this.currentPosition.Value.X) * (startPoint.Y + this.currentPosition.Value.Y);
				}
			}
			this.commands.Add(new PdfSubpath.Close());
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003400 File Offset: 0x00001600
		public bool IsClosed()
		{
			int num = 0;
			PdfSubpath.IPathCommand pathCommand = null;
			PdfSubpath.IPathCommand pathCommand2 = null;
			for (int i = this.Commands.Count - 1; i >= 0; i--)
			{
				PdfSubpath.IPathCommand pathCommand3 = this.Commands[i];
				if (pathCommand3 is PdfSubpath.Close)
				{
					return true;
				}
				if (pathCommand3 is PdfSubpath.Line || pathCommand3 is PdfSubpath.BezierCurve || pathCommand3 is PdfSubpath.Move)
				{
					if (pathCommand == null)
					{
						pathCommand = pathCommand3;
					}
					pathCommand2 = pathCommand3;
					num++;
				}
			}
			return num >= 2 && pathCommand != null && pathCommand2 != null && PdfSubpath.GetStartPoint(pathCommand2).Equals(PdfSubpath.GetEndPoint(pathCommand));
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000034A0 File Offset: 0x000016A0
		public PdfRectangle? GetBoundingRectangle()
		{
			if (this.commands.Count == 0)
			{
				return default(PdfRectangle?);
			}
			double num = double.MaxValue;
			double num2 = double.MinValue;
			double num3 = double.MaxValue;
			double num4 = double.MinValue;
			foreach (PdfSubpath.IPathCommand pathCommand in this.commands)
			{
				PdfRectangle? boundingRectangle = pathCommand.GetBoundingRectangle();
				if (boundingRectangle != null)
				{
					if (boundingRectangle.Value.Left < num)
					{
						num = boundingRectangle.Value.Left;
					}
					if (boundingRectangle.Value.Right > num2)
					{
						num2 = boundingRectangle.Value.Right;
					}
					if (boundingRectangle.Value.Bottom < num3)
					{
						num3 = boundingRectangle.Value.Bottom;
					}
					if (boundingRectangle.Value.Top > num4)
					{
						num4 = boundingRectangle.Value.Top;
					}
				}
			}
			if (num == 1.7976931348623157E+308 || num2 == -1.7976931348623157E+308 || num3 == 1.7976931348623157E+308 || num4 == -1.7976931348623157E+308)
			{
				return default(PdfRectangle?);
			}
			return new PdfRectangle?(new PdfRectangle(num, num3, num2, num4));
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003620 File Offset: 0x00001820
		public PdfRectangle? GetDrawnRectangle()
		{
			if (!this.IsDrawnAsRectangle || this.Commands.Count != 5)
			{
				return default(PdfRectangle?);
			}
			PdfSubpath.Move move = this.Commands[0] as PdfSubpath.Move;
			if (move != null)
			{
				PdfSubpath.Line line = this.Commands[1] as PdfSubpath.Line;
				if (line != null)
				{
					PdfSubpath.Line line2 = this.Commands[2] as PdfSubpath.Line;
					if (line2 != null && this.Commands[3] is PdfSubpath.Line && this.Commands[4] is PdfSubpath.Close)
					{
						if (!line.From.Equals(move.Location) || line.To.Y != move.Location.Y)
						{
							return default(PdfRectangle?);
						}
						double num = line.To.X - move.Location.X;
						if (!line2.From.Equals(line.To) || line2.To.X != line.To.X)
						{
							return default(PdfRectangle?);
						}
						double num2 = line2.To.Y - line.To.Y;
						return new PdfRectangle?(new PdfRectangle(move.Location, new PdfPoint(move.Location.X + num, move.Location.Y + num2)));
					}
				}
			}
			return default(PdfRectangle?);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000037D4 File Offset: 0x000019D4
		public static PdfRectangle? GetBoundingRectangle(IReadOnlyList<PdfSubpath> path)
		{
			if (path == null || path.Count == 0)
			{
				return default(PdfRectangle?);
			}
			List<PdfRectangle> list = Enumerable.ToList<PdfRectangle>(Enumerable.Select<PdfRectangle?, PdfRectangle>(Enumerable.Where<PdfRectangle?>(Enumerable.Select<PdfSubpath, PdfRectangle?>(path, (PdfSubpath x) => x.GetBoundingRectangle()), (PdfRectangle? x) => x != null), (PdfRectangle? x) => x.Value));
			if (list.Count == 0)
			{
				return default(PdfRectangle?);
			}
			double x3 = Enumerable.Min<PdfRectangle>(list, (PdfRectangle x) => x.Left);
			double y = Enumerable.Min<PdfRectangle>(list, (PdfRectangle x) => x.Bottom);
			double x2 = Enumerable.Max<PdfRectangle>(list, (PdfRectangle x) => x.Right);
			double y2 = Enumerable.Max<PdfRectangle>(list, (PdfRectangle x) => x.Top);
			return new PdfRectangle?(new PdfRectangle(x3, y, x2, y2));
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003928 File Offset: 0x00001B28
		public override bool Equals(object obj)
		{
			PdfSubpath pdfSubpath = obj as PdfSubpath;
			if (pdfSubpath == null || this.Commands.Count != pdfSubpath.Commands.Count)
			{
				return false;
			}
			for (int i = 0; i < this.Commands.Count; i++)
			{
				if (!this.Commands[i].Equals(pdfSubpath.Commands[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003994 File Offset: 0x00001B94
		public override int GetHashCode()
		{
			int num = this.Commands.Count + 1;
			for (int i = 0; i < this.Commands.Count; i++)
			{
				num = num * (i + 1) * 17 + this.Commands[i].GetHashCode();
			}
			return num;
		}

		// Token: 0x0400001A RID: 26
		private readonly List<PdfSubpath.IPathCommand> commands = new List<PdfSubpath.IPathCommand>();

		// Token: 0x0400001C RID: 28
		private PdfPoint? currentPosition;

		// Token: 0x0400001D RID: 29
		private double shoeLaceSum;

		// Token: 0x02000019 RID: 25
		public interface IPathCommand
		{
			// Token: 0x060000B9 RID: 185
			PdfRectangle? GetBoundingRectangle();

			// Token: 0x060000BA RID: 186
			void WriteSvg(StringBuilder builder, double height);
		}

		// Token: 0x0200001A RID: 26
		public class Close : PdfSubpath.IPathCommand
		{
			// Token: 0x060000BB RID: 187 RVA: 0x00005F08 File Offset: 0x00004108
			public PdfRectangle? GetBoundingRectangle()
			{
				return default(PdfRectangle?);
			}

			// Token: 0x060000BC RID: 188 RVA: 0x00005F1E File Offset: 0x0000411E
			public void WriteSvg(StringBuilder builder, double height)
			{
				builder.Append("Z ");
			}

			// Token: 0x060000BD RID: 189 RVA: 0x00005F2C File Offset: 0x0000412C
			public override bool Equals(object obj)
			{
				return obj is PdfSubpath.Close;
			}

			// Token: 0x060000BE RID: 190 RVA: 0x00005F37 File Offset: 0x00004137
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		// Token: 0x0200001B RID: 27
		public class Move : PdfSubpath.IPathCommand
		{
			// Token: 0x17000027 RID: 39
			// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005F47 File Offset: 0x00004147
			public PdfPoint Location { get; }

			// Token: 0x060000C1 RID: 193 RVA: 0x00005F4F File Offset: 0x0000414F
			public Move(PdfPoint location)
			{
				this.Location = location;
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x00005F60 File Offset: 0x00004160
			public PdfRectangle? GetBoundingRectangle()
			{
				return default(PdfRectangle?);
			}

			// Token: 0x060000C3 RID: 195 RVA: 0x00005F78 File Offset: 0x00004178
			public void WriteSvg(StringBuilder builder, double height)
			{
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
				appendInterpolatedStringHandler..ctor(4, 2, builder);
				appendInterpolatedStringHandler.AppendLiteral("M ");
				appendInterpolatedStringHandler.AppendFormatted<double>(this.Location.X);
				appendInterpolatedStringHandler.AppendLiteral(" ");
				appendInterpolatedStringHandler.AppendFormatted<double>(height - this.Location.Y);
				appendInterpolatedStringHandler.AppendLiteral(" ");
				builder.Append(ref appendInterpolatedStringHandler);
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x00005FEC File Offset: 0x000041EC
			public override bool Equals(object obj)
			{
				PdfSubpath.Move move = obj as PdfSubpath.Move;
				return move != null && this.Location.Equals(move.Location);
			}

			// Token: 0x060000C5 RID: 197 RVA: 0x00006024 File Offset: 0x00004224
			public override int GetHashCode()
			{
				return this.Location.GetHashCode();
			}
		}

		// Token: 0x0200001C RID: 28
		public class Line : PdfSubpath.IPathCommand
		{
			// Token: 0x17000028 RID: 40
			// (get) Token: 0x060000C6 RID: 198 RVA: 0x00006045 File Offset: 0x00004245
			public PdfPoint From { get; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000604D File Offset: 0x0000424D
			public PdfPoint To { get; }

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x060000C8 RID: 200 RVA: 0x00006058 File Offset: 0x00004258
			public double Length
			{
				get
				{
					double num = this.From.X - this.To.X;
					double num2 = this.From.Y - this.To.Y;
					return Math.Sqrt(num * num + num2 * num2);
				}
			}

			// Token: 0x060000C9 RID: 201 RVA: 0x000060AB File Offset: 0x000042AB
			public Line(PdfPoint from, PdfPoint to)
			{
				this.From = from;
				this.To = to;
			}

			// Token: 0x060000CA RID: 202 RVA: 0x000060C1 File Offset: 0x000042C1
			public PdfRectangle? GetBoundingRectangle()
			{
				return new PdfRectangle?(new PdfRectangle(this.From, this.To));
			}

			// Token: 0x060000CB RID: 203 RVA: 0x000060DC File Offset: 0x000042DC
			public void WriteSvg(StringBuilder builder, double height)
			{
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
				appendInterpolatedStringHandler..ctor(4, 2, builder);
				appendInterpolatedStringHandler.AppendLiteral("L ");
				appendInterpolatedStringHandler.AppendFormatted<double>(this.To.X);
				appendInterpolatedStringHandler.AppendLiteral(" ");
				appendInterpolatedStringHandler.AppendFormatted<double>(height - this.To.Y);
				appendInterpolatedStringHandler.AppendLiteral(" ");
				builder.Append(ref appendInterpolatedStringHandler);
			}

			// Token: 0x060000CC RID: 204 RVA: 0x00006150 File Offset: 0x00004350
			public override bool Equals(object obj)
			{
				PdfSubpath.Line line = obj as PdfSubpath.Line;
				return line != null && this.From.Equals(line.From) && this.To.Equals(line.To);
			}

			// Token: 0x060000CD RID: 205 RVA: 0x000061AC File Offset: 0x000043AC
			public override int GetHashCode()
			{
				return new ValueTuple<PdfPoint, PdfPoint>(this.From, this.To).GetHashCode();
			}
		}

		// Token: 0x0200001D RID: 29
		public class BezierCurve : PdfSubpath.IPathCommand
		{
			// Token: 0x1700002B RID: 43
			// (get) Token: 0x060000CE RID: 206 RVA: 0x000061D8 File Offset: 0x000043D8
			public PdfPoint StartPoint { get; }

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060000CF RID: 207 RVA: 0x000061E0 File Offset: 0x000043E0
			public PdfPoint FirstControlPoint { get; }

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x060000D0 RID: 208 RVA: 0x000061E8 File Offset: 0x000043E8
			public PdfPoint SecondControlPoint { get; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000D1 RID: 209 RVA: 0x000061F0 File Offset: 0x000043F0
			public PdfPoint EndPoint { get; }

			// Token: 0x060000D2 RID: 210 RVA: 0x000061F8 File Offset: 0x000043F8
			public BezierCurve(PdfPoint startPoint, PdfPoint firstControlPoint, PdfPoint secondControlPoint, PdfPoint endPoint)
			{
				this.StartPoint = startPoint;
				this.FirstControlPoint = firstControlPoint;
				this.SecondControlPoint = secondControlPoint;
				this.EndPoint = endPoint;
			}

			// Token: 0x060000D3 RID: 211 RVA: 0x00006220 File Offset: 0x00004420
			public PdfRectangle? GetBoundingRectangle()
			{
				double num;
				double num2;
				if (this.StartPoint.X <= this.EndPoint.X)
				{
					num = this.StartPoint.X;
					num2 = this.EndPoint.X;
				}
				else
				{
					num = this.EndPoint.X;
					num2 = this.StartPoint.X;
				}
				double num3;
				double num4;
				if (this.StartPoint.Y <= this.EndPoint.Y)
				{
					num3 = this.StartPoint.Y;
					num4 = this.EndPoint.Y;
				}
				else
				{
					num3 = this.EndPoint.Y;
					num4 = this.StartPoint.Y;
				}
				ValueTuple<double, double> valueTuple;
				if (this.TrySolveQuadratic(true, num, num2, out valueTuple))
				{
					num = valueTuple.Item1;
					num2 = valueTuple.Item2;
				}
				ValueTuple<double, double> valueTuple2;
				if (this.TrySolveQuadratic(false, num3, num4, out valueTuple2))
				{
					num3 = valueTuple2.Item1;
					num4 = valueTuple2.Item2;
				}
				return new PdfRectangle?(new PdfRectangle(num, num3, num2, num4));
			}

			// Token: 0x060000D4 RID: 212 RVA: 0x0000633C File Offset: 0x0000453C
			public void WriteSvg(StringBuilder builder, double height)
			{
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
				appendInterpolatedStringHandler..ctor(10, 6, builder);
				appendInterpolatedStringHandler.AppendLiteral("C ");
				appendInterpolatedStringHandler.AppendFormatted<double>(this.FirstControlPoint.X);
				appendInterpolatedStringHandler.AppendLiteral(" ");
				appendInterpolatedStringHandler.AppendFormatted<double>(height - this.FirstControlPoint.Y);
				appendInterpolatedStringHandler.AppendLiteral(", ");
				appendInterpolatedStringHandler.AppendFormatted<double>(this.SecondControlPoint.X);
				appendInterpolatedStringHandler.AppendLiteral(" ");
				appendInterpolatedStringHandler.AppendFormatted<double>(height - this.SecondControlPoint.Y);
				appendInterpolatedStringHandler.AppendLiteral(", ");
				appendInterpolatedStringHandler.AppendFormatted<double>(this.EndPoint.X);
				appendInterpolatedStringHandler.AppendLiteral(" ");
				appendInterpolatedStringHandler.AppendFormatted<double>(height - this.EndPoint.Y);
				appendInterpolatedStringHandler.AppendLiteral(" ");
				builder.Append(ref appendInterpolatedStringHandler);
			}

			// Token: 0x060000D5 RID: 213 RVA: 0x00006438 File Offset: 0x00004638
			private bool TrySolveQuadratic(bool isX, double currentMin, double currentMax, [TupleElementNames(new string[]
			{
				"min",
				"max"
			})] out ValueTuple<double, double> solutions)
			{
				solutions = default(ValueTuple<double, double>);
				double num = isX ? this.StartPoint.X : this.StartPoint.Y;
				double num2 = isX ? this.FirstControlPoint.X : this.FirstControlPoint.Y;
				double num3 = isX ? this.SecondControlPoint.X : this.SecondControlPoint.Y;
				double num4 = isX ? this.EndPoint.X : this.EndPoint.Y;
				double num5 = 3.0 * (num2 - num);
				double num6 = 6.0 * (num3 - num2);
				double num7 = 3.0 * (num4 - num3);
				double num8 = num5 - num6 + num7;
				double num9 = num6 - num5 - num5;
				double num10 = num5;
				double num11 = num9 * num9 - 4.0 * num8 * num10;
				if (num11 < 0.0)
				{
					return false;
				}
				double num12 = Math.Sqrt(num11);
				double num13 = 2.0 * num8;
				double num14 = (-num9 + num12) / num13;
				double num15 = (-num9 - num12) / num13;
				if (num14 >= 0.0 && num14 <= 1.0)
				{
					double num16 = PdfSubpath.BezierCurve.ValueWithT(num, num2, num3, num4, num14);
					if (num16 < currentMin)
					{
						currentMin = num16;
					}
					if (num16 > currentMax)
					{
						currentMax = num16;
					}
				}
				if (num15 >= 0.0 && num15 <= 1.0)
				{
					double num17 = PdfSubpath.BezierCurve.ValueWithT(num, num2, num3, num4, num15);
					if (num17 < currentMin)
					{
						currentMin = num17;
					}
					if (num17 > currentMax)
					{
						currentMax = num17;
					}
				}
				solutions = new ValueTuple<double, double>(currentMin, currentMax);
				return true;
			}

			// Token: 0x060000D6 RID: 214 RVA: 0x00006600 File Offset: 0x00004800
			public static double ValueWithT(double p1, double p2, double p3, double p4, double t)
			{
				double num = 1.0 - t;
				return num * num * num * p1 + 3.0 * (num * num) * t * p2 + 3.0 * num * (t * t) * p3 + t * t * t * p4;
			}

			// Token: 0x060000D7 RID: 215 RVA: 0x00006654 File Offset: 0x00004854
			public IReadOnlyList<PdfSubpath.Line> ToLines(int n)
			{
				if (n < 1)
				{
					throw new ArgumentException("BezierCurve.ToLines(): n must be greater than 0.");
				}
				List<PdfSubpath.Line> list = new List<PdfSubpath.Line>();
				PdfPoint from = this.StartPoint;
				for (int i = 1; i <= n; i++)
				{
					double t = (double)i / (double)n;
					PdfPoint pdfPoint = new PdfPoint(PdfSubpath.BezierCurve.ValueWithT(this.StartPoint.X, this.FirstControlPoint.X, this.SecondControlPoint.X, this.EndPoint.X, t), PdfSubpath.BezierCurve.ValueWithT(this.StartPoint.Y, this.FirstControlPoint.Y, this.SecondControlPoint.Y, this.EndPoint.Y, t));
					list.Add(new PdfSubpath.Line(from, pdfPoint));
					from = pdfPoint;
				}
				return list;
			}

			// Token: 0x060000D8 RID: 216 RVA: 0x00006734 File Offset: 0x00004934
			public override bool Equals(object obj)
			{
				PdfSubpath.BezierCurve bezierCurve = obj as PdfSubpath.BezierCurve;
				return bezierCurve != null && (this.StartPoint.Equals(bezierCurve.StartPoint) && this.FirstControlPoint.Equals(bezierCurve.FirstControlPoint) && this.SecondControlPoint.Equals(bezierCurve.SecondControlPoint)) && this.EndPoint.Equals(bezierCurve.EndPoint);
			}

			// Token: 0x060000D9 RID: 217 RVA: 0x000067D4 File Offset: 0x000049D4
			public override int GetHashCode()
			{
				return new ValueTuple<PdfPoint, PdfPoint, PdfPoint, PdfPoint>(this.StartPoint, this.FirstControlPoint, this.SecondControlPoint, this.EndPoint).GetHashCode();
			}
		}
	}
}
