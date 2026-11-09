using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.PdfFonts;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001E8 RID: 488
	public class Letter
	{
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x00039E68 File Offset: 0x00038068
		public string Value { get; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x00039E70 File Offset: 0x00038070
		public TextOrientation TextOrientation { get; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x00039E78 File Offset: 0x00038078
		public PdfPoint Location
		{
			get
			{
				return this.StartBaseLine;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x00039E80 File Offset: 0x00038080
		public PdfPoint StartBaseLine { get; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x00039E88 File Offset: 0x00038088
		public PdfPoint EndBaseLine { get; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x00039E90 File Offset: 0x00038090
		public double Width { get; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x00039E98 File Offset: 0x00038098
		public PdfRectangle GlyphRectangle { get; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00039EA0 File Offset: 0x000380A0
		public double FontSize { get; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00039EA8 File Offset: 0x000380A8
		public string FontName
		{
			get
			{
				FontDetails font = this.Font;
				if (font == null)
				{
					return null;
				}
				return font.Name;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00039EBB File Offset: 0x000380BB
		public FontDetails Font { get; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x00039EC3 File Offset: 0x000380C3
		public TextRenderingMode RenderingMode { get; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x00039ECB File Offset: 0x000380CB
		public IColor Color { get; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x00039ED3 File Offset: 0x000380D3
		public IColor StrokeColor { get; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x00039EDB File Offset: 0x000380DB
		public IColor FillColor { get; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x00039EE3 File Offset: 0x000380E3
		public double PointSize { get; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x00039EEB File Offset: 0x000380EB
		public int TextSequence { get; }

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00039EF4 File Offset: 0x000380F4
		public Letter(string value, PdfRectangle glyphRectangle, PdfPoint startBaseLine, PdfPoint endBaseLine, double width, double fontSize, FontDetails font, TextRenderingMode renderingMode, IColor strokeColor, IColor fillColor, double pointSize, int textSequence)
		{
			this.Value = value;
			this.GlyphRectangle = glyphRectangle;
			this.StartBaseLine = startBaseLine;
			this.EndBaseLine = endBaseLine;
			this.Width = width;
			this.FontSize = fontSize;
			this.Font = font;
			this.RenderingMode = renderingMode;
			if (renderingMode == 1)
			{
				this.Color = (this.StrokeColor = (strokeColor ?? GrayColor.Black));
				this.FillColor = fillColor;
			}
			else
			{
				this.Color = (this.FillColor = (fillColor ?? GrayColor.Black));
				this.StrokeColor = strokeColor;
			}
			this.PointSize = pointSize;
			this.TextSequence = textSequence;
			this.TextOrientation = this.GetTextOrientation();
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00039FAC File Offset: 0x000381AC
		private TextOrientation GetTextOrientation()
		{
			if (Math.Abs(this.StartBaseLine.Y - this.EndBaseLine.Y) < 0.0001)
			{
				if (this.StartBaseLine.X > this.EndBaseLine.X)
				{
					return TextOrientation.Rotate180;
				}
				return TextOrientation.Horizontal;
			}
			else
			{
				if (Math.Abs(this.StartBaseLine.X - this.EndBaseLine.X) >= 0.0001)
				{
					return TextOrientation.Other;
				}
				if (this.StartBaseLine.Y > this.EndBaseLine.Y)
				{
					return TextOrientation.Rotate90;
				}
				return TextOrientation.Rotate270;
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0003A058 File Offset: 0x00038258
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 4);
			defaultInterpolatedStringHandler.AppendFormatted(this.Value);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<PdfPoint>(this.Location);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(this.FontName);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(this.PointSize);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
