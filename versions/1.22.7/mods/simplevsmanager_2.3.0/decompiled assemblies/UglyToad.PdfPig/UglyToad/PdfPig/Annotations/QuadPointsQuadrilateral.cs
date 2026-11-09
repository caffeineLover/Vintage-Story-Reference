using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Annotations
{
	// Token: 0x02000202 RID: 514
	public class QuadPointsQuadrilateral
	{
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0003DD60 File Offset: 0x0003BF60
		public IReadOnlyList<PdfPoint> Points { get; }

		// Token: 0x06000C60 RID: 3168 RVA: 0x0003DD68 File Offset: 0x0003BF68
		public QuadPointsQuadrilateral(IReadOnlyList<PdfPoint> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (points.Count != 4)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(75, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Quadpoints quadrilateral should only contain 4 points, instead got ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(points.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" points.");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Points = points;
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0003DDD8 File Offset: 0x0003BFD8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(10, 4);
			defaultInterpolatedStringHandler.AppendLiteral("[ ");
			defaultInterpolatedStringHandler.AppendFormatted<PdfPoint>(this.Points[0]);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<PdfPoint>(this.Points[1]);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<PdfPoint>(this.Points[2]);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<PdfPoint>(this.Points[3]);
			defaultInterpolatedStringHandler.AppendLiteral(" ]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
