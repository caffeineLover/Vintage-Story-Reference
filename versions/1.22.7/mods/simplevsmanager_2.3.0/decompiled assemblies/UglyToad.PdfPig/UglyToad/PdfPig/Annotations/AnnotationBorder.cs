using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Annotations
{
	// Token: 0x020001FA RID: 506
	public class AnnotationBorder
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x0003D537 File Offset: 0x0003B737
		public static AnnotationBorder Default { get; } = new AnnotationBorder(0m, 0m, 1m, null);

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x0003D53E File Offset: 0x0003B73E
		public decimal HorizontalCornerRadius { get; }

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x0003D546 File Offset: 0x0003B746
		public decimal VerticalCornerRadius { get; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x0003D54E File Offset: 0x0003B74E
		public decimal BorderWidth { get; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x0003D556 File Offset: 0x0003B756
		[CanBeNull]
		public IReadOnlyList<decimal> LineDashPattern { get; }

		// Token: 0x06000C50 RID: 3152 RVA: 0x0003D55E File Offset: 0x0003B75E
		public AnnotationBorder(decimal horizontalCornerRadius, decimal verticalCornerRadius, decimal borderWidth, IReadOnlyList<decimal> lineDashPattern)
		{
			this.HorizontalCornerRadius = horizontalCornerRadius;
			this.VerticalCornerRadius = verticalCornerRadius;
			this.BorderWidth = borderWidth;
			this.LineDashPattern = lineDashPattern;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0003D584 File Offset: 0x0003B784
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.HorizontalCornerRadius);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.VerticalCornerRadius);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.BorderWidth);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
