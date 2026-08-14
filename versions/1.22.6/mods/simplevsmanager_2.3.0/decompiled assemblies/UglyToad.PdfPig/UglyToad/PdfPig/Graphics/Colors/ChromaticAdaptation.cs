using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000162 RID: 354
	internal class ChromaticAdaptation
	{
		// Token: 0x060007F8 RID: 2040 RVA: 0x00025A3C File Offset: 0x00023C3C
		public ChromaticAdaptation([TupleElementNames(new string[]
		{
			"Xws",
			"Yws",
			"Zws"
		})] ValueTuple<double, double, double> sourceReferenceWhite, [TupleElementNames(new string[]
		{
			"Xwd",
			"Ywd",
			"Zwd"
		})] ValueTuple<double, double, double> destinationReferenceWhite, ChromaticAdaptation.Method method = ChromaticAdaptation.Method.Bradford)
		{
			Matrix3x3 coneResponseDomain = ChromaticAdaptation.GetConeResponseDomain(method);
			Matrix3x3 matrix3x = coneResponseDomain.Inverse();
			ValueTuple<double, double, double> valueTuple = coneResponseDomain.Multiply(sourceReferenceWhite);
			double item = valueTuple.Item1;
			double item2 = valueTuple.Item2;
			double item3 = valueTuple.Item3;
			ValueTuple<double, double, double> valueTuple2 = coneResponseDomain.Multiply(destinationReferenceWhite);
			double item4 = valueTuple2.Item1;
			double item5 = valueTuple2.Item2;
			double item6 = valueTuple2.Item3;
			Matrix3x3 matrix = new Matrix3x3(item4 / item, 0.0, 0.0, 0.0, item5 / item2, 0.0, 0.0, 0.0, item6 / item3);
			this.adaptationMatrix = matrix3x.Multiply(matrix).Multiply(coneResponseDomain);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00025AF5 File Offset: 0x00023CF5
		[return: TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})]
		public ValueTuple<double, double, double> Transform([TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})] ValueTuple<double, double, double> sourceColor)
		{
			return this.adaptationMatrix.Multiply(sourceColor);
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00025B04 File Offset: 0x00023D04
		private static Matrix3x3 GetConeResponseDomain(ChromaticAdaptation.Method method)
		{
			switch (method)
			{
			case ChromaticAdaptation.Method.XYZScaling:
				return new Matrix3x3(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0);
			case ChromaticAdaptation.Method.Bradford:
				return new Matrix3x3(0.8951, 0.2664, -0.1614, -0.7502, 1.7135, 0.0367, 0.0389, -0.0685, 1.0296);
			case ChromaticAdaptation.Method.VonKries:
				return new Matrix3x3(0.40024, 0.7076, -0.08081, -0.2263, 1.16532, 0.0457, 0.0, 0.0, 0.91822);
			default:
				return ChromaticAdaptation.GetConeResponseDomain(ChromaticAdaptation.Method.Bradford);
			}
		}

		// Token: 0x040003D1 RID: 977
		private readonly Matrix3x3 adaptationMatrix;

		// Token: 0x0200026D RID: 621
		public enum Method
		{
			// Token: 0x0400085A RID: 2138
			XYZScaling,
			// Token: 0x0400085B RID: 2139
			Bradford,
			// Token: 0x0400085C RID: 2140
			VonKries
		}
	}
}
