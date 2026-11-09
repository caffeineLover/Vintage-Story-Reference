using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Elements
{
	// Token: 0x020000D9 RID: 217
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Line : Element, IStateful
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00010928 File Offset: 0x0000EB28
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x00010930 File Offset: 0x0000EB30
		public LineType Type { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00010939 File Offset: 0x0000EB39
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x00010941 File Offset: 0x0000EB41
		public Color Color { get; set; } = Colors.Black;

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0001094A File Offset: 0x0000EB4A
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00010952 File Offset: 0x0000EB52
		public float Thickness { get; set; } = 1f;

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x0001095B File Offset: 0x0000EB5B
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x00010963 File Offset: 0x0000EB63
		public float[] DashPattern { get; set; } = Array.Empty<float>();

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0001096C File Offset: 0x0000EB6C
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00010974 File Offset: 0x0000EB74
		public Color[] GradientColors { get; set; } = Array.Empty<Color>();

		// Token: 0x060006DB RID: 1755 RVA: 0x00010980 File Offset: 0x0000EB80
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			if (availableSpace.IsNegative())
			{
				return SpacePlan.Wrap("The available space is negative.");
			}
			if (this.Type == LineType.Vertical)
			{
				if (this.Thickness.IsGreaterThan(availableSpace.Width))
				{
					return SpacePlan.Wrap("The line thickness is greater than the available horizontal space.");
				}
				return SpacePlan.FullRender(this.Thickness, 0f);
			}
			else
			{
				if (this.Type != LineType.Horizontal)
				{
					throw new NotSupportedException();
				}
				if (this.Thickness.IsGreaterThan(availableSpace.Height))
				{
					return SpacePlan.Wrap("The line thickness is greater than the available vertical space.");
				}
				return SpacePlan.FullRender(0f, this.Thickness);
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00010A24 File Offset: 0x0000EC24
		internal override void Draw(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return;
			}
			Position zero = Position.Zero;
			Position end = (this.Type == LineType.Vertical) ? new Position(0f, availableSpace.Height) : new Position(availableSpace.Width, 0f);
			using (SkPaint skPaint = new SkPaint())
			{
				skPaint.SetStroke(this.Thickness);
				skPaint.SetSolidColor(this.Color);
				if (this.GradientColors.Length != 0)
				{
					skPaint.SetLinearGradient(zero, end, this.GradientColors);
				}
				if (this.DashPattern.Length != 0)
				{
					skPaint.SetDashedPathEffect(this.DashPattern);
				}
				Position vector = (this.Type == LineType.Vertical) ? new Position(this.Thickness / 2f, 0f) : new Position(0f, this.Thickness / 2f);
				base.Canvas.Translate(vector);
				base.Canvas.SetSemanticNodeId(-6);
				base.Canvas.DrawLine(zero, end, skPaint);
				base.Canvas.Translate(vector.Reverse());
				this.IsRendered = true;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00010B50 File Offset: 0x0000ED50
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x00010B58 File Offset: 0x0000ED58
		private bool IsRendered { get; set; }

		// Token: 0x060006DF RID: 1759 RVA: 0x00010B61 File Offset: 0x0000ED61
		public void ResetState(bool hardReset = false)
		{
			this.IsRendered = false;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00010B6A File Offset: 0x0000ED6A
		public object GetState()
		{
			return this.IsRendered;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00010B77 File Offset: 0x0000ED77
		public void SetState(object state)
		{
			this.IsRendered = (bool)state;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00010B88 File Offset: 0x0000ED88
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<LineType>(this.Type);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Thickness.FormatAsCompanionNumber());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
