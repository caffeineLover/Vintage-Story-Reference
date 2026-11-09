using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x0200000F RID: 15
	public struct PdfRectangle
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000046 RID: 70 RVA: 0x0000293E File Offset: 0x00000B3E
		public readonly PdfPoint TopLeft { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002946 File Offset: 0x00000B46
		public readonly PdfPoint TopRight { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000048 RID: 72 RVA: 0x0000294E File Offset: 0x00000B4E
		public readonly PdfPoint BottomRight { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002956 File Offset: 0x00000B56
		public readonly PdfPoint BottomLeft { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002960 File Offset: 0x00000B60
		public PdfPoint Centroid
		{
			get
			{
				double x = (this.BottomRight.X + this.TopRight.X + this.TopLeft.X + this.BottomLeft.X) / 4.0;
				double y = (this.BottomRight.Y + this.TopRight.Y + this.TopLeft.Y + this.BottomLeft.Y) / 4.0;
				return new PdfPoint(x, y);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000029FE File Offset: 0x00000BFE
		public double Width
		{
			get
			{
				if (double.IsNaN(this.width))
				{
					this.GetWidthHeight();
				}
				return this.width;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002A19 File Offset: 0x00000C19
		public double Height
		{
			get
			{
				if (double.IsNaN(this.height))
				{
					this.GetWidthHeight();
				}
				return this.height;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002A34 File Offset: 0x00000C34
		public double Rotation
		{
			get
			{
				return this.GetT() * 180.0 / 3.141592653589793;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002A50 File Offset: 0x00000C50
		public double Area
		{
			get
			{
				return Math.Abs(this.Width * this.Height);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002A64 File Offset: 0x00000C64
		public double Left
		{
			get
			{
				if (this.TopLeft.X >= this.TopRight.X)
				{
					return this.TopRight.X;
				}
				return this.TopLeft.X;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002AAC File Offset: 0x00000CAC
		public double Top
		{
			get
			{
				if (this.TopLeft.Y <= this.BottomLeft.Y)
				{
					return this.BottomLeft.Y;
				}
				return this.TopLeft.Y;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public double Right
		{
			get
			{
				if (this.BottomRight.X <= this.BottomLeft.X)
				{
					return this.BottomLeft.X;
				}
				return this.BottomRight.X;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002B3C File Offset: 0x00000D3C
		public double Bottom
		{
			get
			{
				if (this.BottomRight.Y >= this.TopRight.Y)
				{
					return this.TopRight.Y;
				}
				return this.BottomRight.Y;
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002B84 File Offset: 0x00000D84
		public PdfRectangle(PdfPoint bottomLeft, PdfPoint topRight)
		{
			this = new PdfRectangle(bottomLeft.X, bottomLeft.Y, topRight.X, topRight.Y);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002BA8 File Offset: 0x00000DA8
		public PdfRectangle(short x1, short y1, short x2, short y2)
		{
			this = new PdfRectangle((double)x1, (double)y1, (double)x2, (double)y2);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002BB9 File Offset: 0x00000DB9
		public PdfRectangle(double x1, double y1, double x2, double y2)
		{
			this = new PdfRectangle(new PdfPoint(x1, y2), new PdfPoint(x2, y2), new PdfPoint(x1, y1), new PdfPoint(x2, y1));
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002BDF File Offset: 0x00000DDF
		public PdfRectangle(PdfPoint topLeft, PdfPoint topRight, PdfPoint bottomLeft, PdfPoint bottomRight)
		{
			this.TopLeft = topLeft;
			this.TopRight = topRight;
			this.BottomLeft = bottomLeft;
			this.BottomRight = bottomRight;
			this.width = double.NaN;
			this.height = double.NaN;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002C1C File Offset: 0x00000E1C
		public PdfRectangle Translate(double dx, double dy)
		{
			return new PdfRectangle(this.TopLeft.Translate(dx, dy), this.TopRight.Translate(dx, dy), this.BottomLeft.Translate(dx, dy), this.BottomRight.Translate(dx, dy));
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002C70 File Offset: 0x00000E70
		private double GetT()
		{
			if (!this.BottomRight.Equals(this.BottomLeft))
			{
				return Math.Atan2(this.BottomRight.Y - this.BottomLeft.Y, this.BottomRight.X - this.BottomLeft.X);
			}
			return Math.Atan2(this.TopLeft.Y - this.BottomLeft.Y, this.TopLeft.X - this.BottomLeft.X) - 1.5707963267948966;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002D28 File Offset: 0x00000F28
		private void GetWidthHeight()
		{
			double t = this.GetT();
			double num = Math.Cos(t);
			double num2 = Math.Sin(t);
			TransformationMatrix transformationMatrix = new TransformationMatrix(num, -num2, 0.0, num2, num, 0.0, 0.0, 0.0, 1.0);
			PdfPoint pdfPoint = transformationMatrix.Transform(this.BottomLeft);
			this.width = Math.Abs(transformationMatrix.Transform(this.BottomRight).X - pdfPoint.X);
			this.height = Math.Abs(transformationMatrix.Transform(this.TopLeft).Y - pdfPoint.Y);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002DE4 File Offset: 0x00000FE4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(6, 3);
			defaultInterpolatedStringHandler.AppendLiteral("[");
			defaultInterpolatedStringHandler.AppendFormatted<PdfPoint>(this.TopLeft);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Width.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Height.ToString(CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000018 RID: 24
		private double width;

		// Token: 0x04000019 RID: 25
		private double height;
	}
}
