using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x0200010D RID: 269
	internal static class PerformantRectangleTransformer
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x00020124 File Offset: 0x0001E324
		public static PdfRectangle Transform(TransformationMatrix first, TransformationMatrix second, TransformationMatrix third, PdfRectangle rectangle)
		{
			PdfPoint topLeft = rectangle.TopLeft;
			PdfPoint topRight = rectangle.TopRight;
			PdfPoint bottomLeft = rectangle.BottomLeft;
			PdfPoint bottomRight = rectangle.BottomRight;
			double num = topLeft.X;
			double num2 = topLeft.Y;
			double num3 = topRight.X;
			double num4 = topRight.Y;
			double num5 = bottomLeft.X;
			double num6 = bottomLeft.Y;
			double num7 = bottomRight.X;
			double num8 = bottomRight.Y;
			double num9 = first.A * num + first.C * num2 + first.E;
			double num10 = first.B * num + first.D * num2 + first.F;
			num = num9;
			num2 = num10;
			num9 = first.A * num3 + first.C * num4 + first.E;
			double num11 = first.B * num3 + first.D * num4 + first.F;
			num3 = num9;
			num4 = num11;
			num9 = first.A * num5 + first.C * num6 + first.E;
			double num12 = first.B * num5 + first.D * num6 + first.F;
			num5 = num9;
			num6 = num12;
			num9 = first.A * num7 + first.C * num8 + first.E;
			double num13 = first.B * num7 + first.D * num8 + first.F;
			num7 = num9;
			num8 = num13;
			num9 = second.A * num + second.C * num2 + second.E;
			double num14 = second.B * num + second.D * num2 + second.F;
			num = num9;
			num2 = num14;
			num9 = second.A * num3 + second.C * num4 + second.E;
			double num15 = second.B * num3 + second.D * num4 + second.F;
			num3 = num9;
			num4 = num15;
			num9 = second.A * num5 + second.C * num6 + second.E;
			double num16 = second.B * num5 + second.D * num6 + second.F;
			num5 = num9;
			num6 = num16;
			num9 = second.A * num7 + second.C * num8 + second.E;
			double num17 = second.B * num7 + second.D * num8 + second.F;
			num7 = num9;
			num8 = num17;
			num9 = third.A * num + third.C * num2 + third.E;
			double num18 = third.B * num + third.D * num2 + third.F;
			num = num9;
			num2 = num18;
			num9 = third.A * num3 + third.C * num4 + third.E;
			double num19 = third.B * num3 + third.D * num4 + third.F;
			num3 = num9;
			num4 = num19;
			num9 = third.A * num5 + third.C * num6 + third.E;
			double num20 = third.B * num5 + third.D * num6 + third.F;
			num5 = num9;
			num6 = num20;
			num9 = third.A * num7 + third.C * num8 + third.E;
			double num21 = third.B * num7 + third.D * num8 + third.F;
			num7 = num9;
			num8 = num21;
			return new PdfRectangle(new PdfPoint(num, num2), new PdfPoint(num3, num4), new PdfPoint(num5, num6), new PdfPoint(num7, num8));
		}
	}
}
