using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.DrawingCanvases;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Elements
{
	// Token: 0x020000F5 RID: 245
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class StyledBox : ContainerElement
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001345B File Offset: 0x0001165B
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x00013463 File Offset: 0x00011663
		public float BorderLeft { get; set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0001346C File Offset: 0x0001166C
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x00013474 File Offset: 0x00011674
		public float BorderTop { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001347D File Offset: 0x0001167D
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x00013485 File Offset: 0x00011685
		public float BorderRight { get; set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x0001348E File Offset: 0x0001168E
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x00013496 File Offset: 0x00011696
		public float BorderBottom { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x0001349F File Offset: 0x0001169F
		private bool HasBorder
		{
			get
			{
				return this.BorderLeft > 0f || this.BorderTop > 0f || this.BorderRight > 0f || this.BorderBottom > 0f;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x000134D7 File Offset: 0x000116D7
		private bool HasFullBorder
		{
			get
			{
				return this.BorderLeft > 0f && this.BorderTop > 0f && this.BorderRight > 0f && this.BorderBottom > 0f;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x0001350F File Offset: 0x0001170F
		private bool HasUniformBorder
		{
			get
			{
				return this.BorderLeft == this.BorderRight && this.BorderTop == this.BorderBottom && this.BorderLeft == this.BorderTop;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x0001353D File Offset: 0x0001173D
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x00013545 File Offset: 0x00011745
		public float BorderRadiusTopLeft { get; set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0001354E File Offset: 0x0001174E
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x00013556 File Offset: 0x00011756
		public float BorderRadiusTopRight { get; set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0001355F File Offset: 0x0001175F
		// (set) Token: 0x06000806 RID: 2054 RVA: 0x00013567 File Offset: 0x00011767
		public float BorderRadiusBottomLeft { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x00013570 File Offset: 0x00011770
		// (set) Token: 0x06000808 RID: 2056 RVA: 0x00013578 File Offset: 0x00011778
		public float BorderRadiusBottomRight { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00013581 File Offset: 0x00011781
		private bool HasRoundedCorners
		{
			get
			{
				return this.BorderRadiusTopLeft > 0f || this.BorderRadiusTopRight > 0f || this.BorderRadiusBottomLeft > 0f || this.BorderRadiusBottomRight > 0f;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x000135B9 File Offset: 0x000117B9
		private bool HasUniformRoundedCorners
		{
			get
			{
				return this.BorderRadiusTopLeft == this.BorderRadiusTopRight && this.BorderRadiusBottomLeft == this.BorderRadiusBottomRight && this.BorderRadiusTopLeft == this.BorderRadiusBottomLeft;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x000135E7 File Offset: 0x000117E7
		// (set) Token: 0x0600080C RID: 2060 RVA: 0x000135EF File Offset: 0x000117EF
		public float? BorderAlignment { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x000135F8 File Offset: 0x000117F8
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x00013600 File Offset: 0x00011800
		public Color BackgroundColor { get; set; } = Colors.Transparent;

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x00013609 File Offset: 0x00011809
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x00013611 File Offset: 0x00011811
		public Color[] BackgroundGradientColors { get; set; } = Array.Empty<Color>();

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0001361A File Offset: 0x0001181A
		// (set) Token: 0x06000812 RID: 2066 RVA: 0x00013622 File Offset: 0x00011822
		public float? BackgroundGradientAngle { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x0001362B File Offset: 0x0001182B
		// (set) Token: 0x06000814 RID: 2068 RVA: 0x00013633 File Offset: 0x00011833
		public Color BorderColor { get; set; } = Colors.Transparent;

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0001363C File Offset: 0x0001183C
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x00013644 File Offset: 0x00011844
		public Color[] BorderGradientColors { get; set; } = Array.Empty<Color>();

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0001364D File Offset: 0x0001184D
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x00013655 File Offset: 0x00011855
		public float? BorderGradientAngle { get; set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x0001365E File Offset: 0x0001185E
		private bool HasSimpleStyle
		{
			get
			{
				return this.BackgroundGradientColors.Length == 0 && this.BorderGradientColors.Length == 0;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00013675 File Offset: 0x00011875
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x0001367D File Offset: 0x0001187D
		[Nullable(2)]
		public BoxShadowStyle Shadow { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x0600081C RID: 2076 RVA: 0x00013688 File Offset: 0x00011888
		internal void AdjustBorderAlignment()
		{
			if (this.BorderAlignment != null)
			{
				return;
			}
			this.BorderAlignment = new float?(this.HasRoundedCorners ? 0f : 0.5f);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x000136C8 File Offset: 0x000118C8
		internal override void Draw(Size availableSpace)
		{
			this.AdjustBorderAlignment();
			if (base.Canvas is DiscardDrawingCanvas)
			{
				base.Draw(availableSpace);
				return;
			}
			using (SkPaint backgroundPaint = this.GetBackgroundPaint(availableSpace))
			{
				using (SkPaint borderPaint = this.GetBorderPaint(availableSpace))
				{
					if (this.HasFullBorder && this.HasUniformBorder && !this.HasRoundedCorners && this.HasSimpleStyle && this.BorderAlignment.GetValueOrDefault() == 0.5f && this.Shadow == null)
					{
						if (backgroundPaint != null)
						{
							base.Canvas.SetSemanticNodeId(-8);
							base.Canvas.DrawRectangle(Position.Zero, availableSpace, backgroundPaint);
						}
						base.Draw(availableSpace);
						if (borderPaint != null)
						{
							borderPaint.SetStroke(this.BorderLeft);
							base.Canvas.SetSemanticNodeId(-6);
							base.Canvas.DrawRectangle(Position.Zero, availableSpace, borderPaint);
						}
					}
					else
					{
						SkRoundedRect primaryBorderRect = this.GetPrimaryBorderRect(availableSpace);
						SkRoundedRect outerRect = this.GetOuterRect(availableSpace);
						SkRoundedRect innerRect = this.GetInnerRect(availableSpace);
						if (this.Shadow != null)
						{
							SkRoundedRect shadowRect = this.ExpandRoundedRect(primaryBorderRect, this.Shadow.Spread);
							SkBoxShadow shadow = new SkBoxShadow
							{
								OffsetX = this.Shadow.OffsetX,
								OffsetY = this.Shadow.OffsetY,
								Blur = this.Shadow.Blur,
								Color = this.Shadow.Color
							};
							base.Canvas.SetSemanticNodeId(-8);
							base.Canvas.DrawShadow(shadowRect, shadow);
						}
						if (this.HasRoundedCorners)
						{
							base.Canvas.Save();
							base.Canvas.ClipRoundedRectangle(primaryBorderRect);
						}
						if (backgroundPaint != null)
						{
							base.Canvas.SetSemanticNodeId(-8);
							base.Canvas.DrawRectangle(Position.Zero, availableSpace, backgroundPaint);
						}
						base.Draw(availableSpace);
						if (this.HasRoundedCorners)
						{
							base.Canvas.Restore();
						}
						if (borderPaint != null)
						{
							base.Canvas.SetSemanticNodeId(-6);
							base.Canvas.DrawComplexBorder(innerRect, outerRect, borderPaint);
						}
					}
				}
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00013914 File Offset: 0x00011B14
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"start",
			"end"
		})]
		private ValueTuple<Position, Position> GetLinearGradientPositions(Size availableSpace, float angle)
		{
			if (angle == 0f)
			{
				return new ValueTuple<Position, Position>(Position.Zero, new Position(availableSpace.Width, 0f));
			}
			if (angle == 90f)
			{
				return new ValueTuple<Position, Position>(Position.Zero, new Position(0f, availableSpace.Height));
			}
			if (angle == 180f)
			{
				return new ValueTuple<Position, Position>(new Position(availableSpace.Width, 0f), new Position(0f, 0f));
			}
			if (angle == 270f)
			{
				return new ValueTuple<Position, Position>(new Position(0f, availableSpace.Height), new Position(availableSpace.Width, availableSpace.Height));
			}
			IEnumerable<Position> enumerable = new Position[]
			{
				Position.Zero,
				new Position(availableSpace.Width, 0f),
				new Position(availableSpace.Width, availableSpace.Height),
				new Position(0f, availableSpace.Height)
			};
			double angleInRadians = 3.141592653589793 * (double)angle / 180.0;
			Position linePoint = new Position(availableSpace.Width / 2f, availableSpace.Height / 2f);
			Position[] array = Enumerable.ToArray<Position>(Enumerable.Select<Position, Position>(enumerable, (Position point) => StyledBox.<GetLinearGradientPositions>g__ProjectPointOntoLine|78_3(linePoint, (float)angleInRadians, point)));
			Position position = Enumerable.First<Position>(Enumerable.OrderBy<Position, float>(array, (Position p) => p.X));
			Position position2 = Enumerable.First<Position>(Enumerable.OrderByDescending<Position, float>(array, (Position p) => p.X));
			return new ValueTuple<Position, Position>(position, position2);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00013AD4 File Offset: 0x00011CD4
		[NullableContext(2)]
		private SkPaint GetBorderPaint(Size availableSpace)
		{
			if (this.BorderGradientColors.Length != 0)
			{
				SkPaint skPaint = new SkPaint();
				ValueTuple<Position, Position> linearGradientPositions = this.GetLinearGradientPositions(availableSpace, this.BorderGradientAngle.GetValueOrDefault());
				skPaint.SetLinearGradient(linearGradientPositions.Item1, linearGradientPositions.Item2, this.BorderGradientColors);
				return skPaint;
			}
			if (this.BorderColor.Hex != Colors.Transparent.Hex)
			{
				SkPaint skPaint2 = new SkPaint();
				skPaint2.SetSolidColor(this.BorderColor);
				return skPaint2;
			}
			return null;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00013B50 File Offset: 0x00011D50
		[NullableContext(2)]
		private SkPaint GetBackgroundPaint(Size availableSpace)
		{
			if (this.BackgroundGradientColors.Length != 0)
			{
				SkPaint skPaint = new SkPaint();
				ValueTuple<Position, Position> linearGradientPositions = this.GetLinearGradientPositions(availableSpace, this.BackgroundGradientAngle.GetValueOrDefault());
				skPaint.SetLinearGradient(linearGradientPositions.Item1, linearGradientPositions.Item2, this.BackgroundGradientColors);
				return skPaint;
			}
			if (this.BackgroundColor.Hex != Colors.Transparent.Hex)
			{
				SkPaint skPaint2 = new SkPaint();
				skPaint2.SetSolidColor(this.BackgroundColor);
				return skPaint2;
			}
			return null;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00013BCC File Offset: 0x00011DCC
		private SkRoundedRect GetPrimaryBorderRect(Size availableSpace)
		{
			return new SkRoundedRect
			{
				Rect = new SkRect
				{
					Left = 0f,
					Top = 0f,
					Right = availableSpace.Width,
					Bottom = availableSpace.Height
				},
				TopLeftRadius = new SkPoint(this.BorderRadiusTopLeft, this.BorderRadiusTopLeft),
				TopRightRadius = new SkPoint(this.BorderRadiusTopRight, this.BorderRadiusTopRight),
				BottomLeftRadius = new SkPoint(this.BorderRadiusBottomLeft, this.BorderRadiusBottomLeft),
				BottomRightRadius = new SkPoint(this.BorderRadiusBottomRight, this.BorderRadiusBottomRight)
			};
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00013C84 File Offset: 0x00011E84
		private SkRoundedRect GetBorderRectExpandedWithBorderThickness(Size availableSpace, float borderThicknessExpansionFactor)
		{
			SkRoundedRect primaryBorderRect = this.GetPrimaryBorderRect(availableSpace);
			return this.ExpandRoundedRect(primaryBorderRect, borderThicknessExpansionFactor * this.BorderLeft, borderThicknessExpansionFactor * this.BorderTop, borderThicknessExpansionFactor * this.BorderRight, borderThicknessExpansionFactor * this.BorderBottom);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00013CC0 File Offset: 0x00011EC0
		private SkRoundedRect GetOuterRect(Size availableSpace)
		{
			return this.GetBorderRectExpandedWithBorderThickness(availableSpace, this.BorderAlignment.Value);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00013CE4 File Offset: 0x00011EE4
		private SkRoundedRect GetInnerRect(Size availableSpace)
		{
			return this.GetBorderRectExpandedWithBorderThickness(availableSpace, this.BorderAlignment.Value - 1f);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00013D0C File Offset: 0x00011F0C
		private SkRoundedRect ExpandRoundedRect(SkRoundedRect rect, float all)
		{
			return this.ExpandRoundedRect(rect, all, all, all, all);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00013D1C File Offset: 0x00011F1C
		private SkRoundedRect ExpandRoundedRect(SkRoundedRect input, float left, float top, float right, float bottom)
		{
			SkRect rect = new SkRect
			{
				Left = input.Rect.Left - left,
				Top = input.Rect.Top - top,
				Right = input.Rect.Right + right,
				Bottom = input.Rect.Bottom + bottom
			};
			if (input.TopLeftRadius.X <= 0f && input.TopRightRadius.X <= 0f && input.BottomLeftRadius.X <= 0f && input.BottomRightRadius.X <= 0f)
			{
				return new SkRoundedRect
				{
					Rect = rect
				};
			}
			return new SkRoundedRect
			{
				Rect = rect,
				TopLeftRadius = new SkPoint
				{
					X = Math.Max(0f, input.TopLeftRadius.X + left),
					Y = Math.Max(0f, input.TopLeftRadius.Y + top)
				},
				TopRightRadius = new SkPoint
				{
					X = Math.Max(0f, input.TopRightRadius.X + right),
					Y = Math.Max(0f, input.TopRightRadius.Y + top)
				},
				BottomLeftRadius = new SkPoint
				{
					X = Math.Max(0f, input.BottomLeftRadius.X + left),
					Y = Math.Max(0f, input.BottomLeftRadius.Y + bottom)
				},
				BottomRightRadius = new SkPoint
				{
					X = Math.Max(0f, input.BottomRightRadius.X + right),
					Y = Math.Max(0f, input.BottomRightRadius.Y + bottom)
				}
			};
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00013F29 File Offset: 0x00012129
		[return: TupleElementNames(new string[]
		{
			"Type",
			"Hint"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			2
		})]
		internal IEnumerable<ValueTuple<string, string>> GetCompanionCustomContent()
		{
			StyledBox.<GetCompanionCustomContent>d__87 <GetCompanionCustomContent>d__ = new StyledBox.<GetCompanionCustomContent>d__87(-2);
			<GetCompanionCustomContent>d__.<>4__this = this;
			return <GetCompanionCustomContent>d__;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00013F70 File Offset: 0x00012170
		[CompilerGenerated]
		internal static Position <GetLinearGradientPositions>g__ProjectPointOntoLine|78_3(Position linePoint, float lineAngleRadians, Position projectionPoint)
		{
			float num = (float)Math.Cos((double)lineAngleRadians);
			float num2 = (float)Math.Sin((double)lineAngleRadians);
			float num3 = projectionPoint.X - linePoint.X;
			float num4 = projectionPoint.Y - linePoint.Y;
			float num5 = num3 * num + num4 * num2;
			return new Position(linePoint.X + num * num5, linePoint.Y + num2 * num5);
		}
	}
}
