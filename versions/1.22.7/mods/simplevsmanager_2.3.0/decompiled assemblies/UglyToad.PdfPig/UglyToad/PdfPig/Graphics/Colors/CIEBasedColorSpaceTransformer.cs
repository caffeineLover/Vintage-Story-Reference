using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000163 RID: 355
	internal class CIEBasedColorSpaceTransformer
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x00025C33 File Offset: 0x00023E33
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x00025C3B File Offset: 0x00023E3B
		[TupleElementNames(new string[]
		{
			"A",
			"B",
			"C",
			"A",
			"B",
			"C"
		})]
		public Func<ValueTuple<double, double, double>, ValueTuple<double, double, double>> DecoderABC { [return: TupleElementNames(new string[]
		{
			"A",
			"B",
			"C",
			"A",
			"B",
			"C"
		})] get; [param: TupleElementNames(new string[]
		{
			"A",
			"B",
			"C",
			"A",
			"B",
			"C"
		})] set; } = ([TupleElementNames(new string[]
		{
			"A",
			"B",
			"C"
		})] ValueTuple<double, double, double> color) => color;

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x00025C44 File Offset: 0x00023E44
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x00025C4C File Offset: 0x00023E4C
		[TupleElementNames(new string[]
		{
			"L",
			"M",
			"N",
			"L",
			"M",
			"N"
		})]
		public Func<ValueTuple<double, double, double>, ValueTuple<double, double, double>> DecoderLMN { [return: TupleElementNames(new string[]
		{
			"L",
			"M",
			"N",
			"L",
			"M",
			"N"
		})] get; [param: TupleElementNames(new string[]
		{
			"L",
			"M",
			"N",
			"L",
			"M",
			"N"
		})] set; } = ([TupleElementNames(new string[]
		{
			"L",
			"M",
			"N"
		})] ValueTuple<double, double, double> color) => color;

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00025C55 File Offset: 0x00023E55
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x00025C5D File Offset: 0x00023E5D
		public Matrix3x3 MatrixABC { get; set; } = Matrix3x3.Identity;

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00025C66 File Offset: 0x00023E66
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x00025C6E File Offset: 0x00023E6E
		public Matrix3x3 MatrixLMN { get; set; } = Matrix3x3.Identity;

		// Token: 0x06000803 RID: 2051 RVA: 0x00025C78 File Offset: 0x00023E78
		public CIEBasedColorSpaceTransformer([TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})] ValueTuple<double, double, double> sourceReferenceWhite, RGBWorkingSpace destinationWorkingSpace)
		{
			this.destinationWorkingSpace = destinationWorkingSpace;
			this.chromaticAdaptation = new ChromaticAdaptation(sourceReferenceWhite, destinationWorkingSpace.ReferenceWhite, ChromaticAdaptation.Method.Bradford);
			double item = destinationWorkingSpace.RedPrimary.Item1;
			double item2 = destinationWorkingSpace.RedPrimary.Item2;
			double item3 = destinationWorkingSpace.GreenPrimary.Item1;
			double item4 = destinationWorkingSpace.GreenPrimary.Item2;
			double item5 = destinationWorkingSpace.BluePrimary.Item1;
			double item6 = destinationWorkingSpace.BluePrimary.Item2;
			double num = item / item2;
			int num2 = 1;
			double num3 = (1.0 - item - item2) / item2;
			double num4 = item3 / item4;
			int num5 = 1;
			double num6 = (1.0 - item3 - item4) / item4;
			double num7 = item5 / item6;
			int num8 = 1;
			double num9 = (1.0 - item5 - item6) / item6;
			ValueTuple<double, double, double> valueTuple = new Matrix3x3(num, num4, num7, (double)num2, (double)num5, (double)num8, num3, num6, num9).Inverse().Multiply(destinationWorkingSpace.ReferenceWhite);
			double item7 = valueTuple.Item1;
			double item8 = valueTuple.Item2;
			double item9 = valueTuple.Item3;
			Matrix3x3 matrix3x = new Matrix3x3(item7 * num, item8 * num4, item9 * num7, item7 * (double)num2, item8 * (double)num5, item9 * (double)num8, item7 * num3, item8 * num6, item9 * num9);
			this.transformationMatrix = matrix3x.Inverse();
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00025E24 File Offset: 0x00024024
		[return: TupleElementNames(new string[]
		{
			"R",
			"G",
			"B"
		})]
		public ValueTuple<double, double, double> TransformToRGB([TupleElementNames(new string[]
		{
			"A",
			"B",
			"C"
		})] ValueTuple<double, double, double> color)
		{
			ValueTuple<double, double, double> sourceColor = this.TransformToXYZ(color);
			ValueTuple<double, double, double> vector = this.chromaticAdaptation.Transform(sourceColor);
			ValueTuple<double, double, double> valueTuple = this.transformationMatrix.Multiply(vector);
			double value = this.destinationWorkingSpace.GammaCorrection.Invoke(valueTuple.Item1);
			double value2 = this.destinationWorkingSpace.GammaCorrection.Invoke(valueTuple.Item2);
			double value3 = this.destinationWorkingSpace.GammaCorrection.Invoke(valueTuple.Item3);
			return new ValueTuple<double, double, double>(CIEBasedColorSpaceTransformer.Clamp(value), CIEBasedColorSpaceTransformer.Clamp(value2), CIEBasedColorSpaceTransformer.Clamp(value3));
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00025EB0 File Offset: 0x000240B0
		[return: TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})]
		private ValueTuple<double, double, double> TransformToXYZ([TupleElementNames(new string[]
		{
			"A",
			"B",
			"C"
		})] ValueTuple<double, double, double> color)
		{
			ValueTuple<double, double, double> vector = this.DecoderABC.Invoke(color);
			ValueTuple<double, double, double> valueTuple = this.MatrixABC.Multiply(vector);
			ValueTuple<double, double, double> vector2 = this.DecoderLMN.Invoke(valueTuple);
			return this.MatrixLMN.Multiply(vector2);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00025EF0 File Offset: 0x000240F0
		private static double Clamp(double value)
		{
			if (value < 0.0)
			{
				return 0.0;
			}
			if (value <= 1.0)
			{
				return value;
			}
			return 1.0;
		}

		// Token: 0x040003D2 RID: 978
		private readonly RGBWorkingSpace destinationWorkingSpace;

		// Token: 0x040003D3 RID: 979
		private readonly Matrix3x3 transformationMatrix;

		// Token: 0x040003D4 RID: 980
		private readonly ChromaticAdaptation chromaticAdaptation;
	}
}
