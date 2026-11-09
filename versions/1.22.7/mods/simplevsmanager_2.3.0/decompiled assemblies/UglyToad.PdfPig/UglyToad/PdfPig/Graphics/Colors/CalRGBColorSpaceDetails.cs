using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Util;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x0200016E RID: 366
	public sealed class CalRGBColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x00027083 File Offset: 0x00025283
		public override int NumberOfColorComponents
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x00027086 File Offset: 0x00025286
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.NumberOfColorComponents;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0002708E File Offset: 0x0002528E
		public IReadOnlyList<double> WhitePoint { get; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00027096 File Offset: 0x00025296
		public IReadOnlyList<double> BlackPoint { get; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0002709E File Offset: 0x0002529E
		public IReadOnlyList<double> Gamma { get; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x000270A6 File Offset: 0x000252A6
		public IReadOnlyList<double> Matrix { get; }

		// Token: 0x06000873 RID: 2163 RVA: 0x000270B0 File Offset: 0x000252B0
		public CalRGBColorSpaceDetails([NotNull] IReadOnlyList<double> whitePoint, [CanBeNull] IReadOnlyList<double> blackPoint, [CanBeNull] IReadOnlyList<double> gamma, [CanBeNull] IReadOnlyList<double> matrix) : base(ColorSpace.CalRGB)
		{
			if (whitePoint == null)
			{
				throw new ArgumentNullException("whitePoint");
			}
			this.WhitePoint = whitePoint;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this.WhitePoint.Count != 3)
			{
				string text = "whitePoint";
				defaultInterpolatedStringHandler..ctor(55, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Must consist of exactly three numbers, but was passed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(whitePoint.Count);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text, whitePoint, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.BlackPoint = (blackPoint ?? Enumerable.ToArray<double>(new double[3]));
			if (this.BlackPoint.Count != 3)
			{
				string text2 = "blackPoint";
				defaultInterpolatedStringHandler..ctor(55, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Must consist of exactly three numbers, but was passed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(blackPoint.Count);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text2, blackPoint, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Gamma = (gamma ?? Enumerable.ToArray<double>(new double[]
			{
				1.0,
				1.0,
				1.0
			}));
			if (this.Gamma.Count != 3)
			{
				string text3 = "gamma";
				defaultInterpolatedStringHandler..ctor(55, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Must consist of exactly three numbers, but was passed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(gamma.Count);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text3, gamma, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Matrix = (matrix ?? Enumerable.ToArray<double>(new double[]
			{
				1.0,
				0.0,
				0.0,
				0.0,
				1.0,
				0.0,
				0.0,
				0.0,
				1.0
			}));
			if (this.Matrix.Count != 9)
			{
				string text4 = "matrix";
				defaultInterpolatedStringHandler..ctor(54, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Must consist of exactly nine numbers, but was passed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(matrix.Count);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text4, matrix, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.colorSpaceTransformer = new CIEBasedColorSpaceTransformer(new ValueTuple<double, double, double>(this.WhitePoint[0], this.WhitePoint[1], this.WhitePoint[2]), RGBWorkingSpace.sRGB)
			{
				DecoderABC = (([TupleElementNames(new string[]
				{
					"A",
					"B",
					"C"
				})] ValueTuple<double, double, double> color) => new ValueTuple<double, double, double>(Math.Pow(color.Item1, this.Gamma[0]), Math.Pow(color.Item2, this.Gamma[1]), Math.Pow(color.Item3, this.Gamma[2]))),
				MatrixABC = new Matrix3x3(this.Matrix[0], this.Matrix[3], this.Matrix[6], this.Matrix[1], this.Matrix[4], this.Matrix[7], this.Matrix[2], this.Matrix[5], this.Matrix[8])
			};
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0002733C File Offset: 0x0002553C
		private RGBColor TransformToRGB([TupleElementNames(new string[]
		{
			"A",
			"B",
			"C"
		})] ValueTuple<double, double, double> colorAbc)
		{
			ValueTuple<double, double, double> valueTuple = this.colorSpaceTransformer.TransformToRGB(new ValueTuple<double, double, double>(colorAbc.Item1, colorAbc.Item2, colorAbc.Item3));
			double item = valueTuple.Item1;
			double item2 = valueTuple.Item2;
			double item3 = valueTuple.Item3;
			return new RGBColor(item, item2, item3);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00027388 File Offset: 0x00025588
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded)
		{
			List<byte> list = new List<byte>();
			for (int i = 0; i < decoded.Count; i += 3)
			{
				double[] array = this.Process(new double[]
				{
					(double)decoded[i] / 255.0,
					(double)decoded[i + 1] / 255.0,
					(double)decoded[i + 2] / 255.0
				});
				list.Add(ColorSpaceDetails.ConvertToByte(array[0]));
				list.Add(ColorSpaceDetails.ConvertToByte(array[1]));
				list.Add(ColorSpaceDetails.ConvertToByte(array[2]));
			}
			return list;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0002742C File Offset: 0x0002562C
		internal override double[] Process(params double[] values)
		{
			ValueTuple<double, double, double> valueTuple = this.colorSpaceTransformer.TransformToRGB(new ValueTuple<double, double, double>(values[0], values[1], values[2]));
			double item = valueTuple.Item1;
			double item2 = valueTuple.Item2;
			double item3 = valueTuple.Item3;
			return new double[]
			{
				item,
				item2,
				item3
			};
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00027478 File Offset: 0x00025678
		public override IColor GetColor(params double[] values)
		{
			if (values == null || values.Length != this.NumberOfColorComponents)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(45, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid number of imputs, expecting ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.NumberOfColorComponents);
				defaultInterpolatedStringHandler.AppendLiteral(" but got ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(values.Length);
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "values");
			}
			return this.TransformToRGB(new ValueTuple<double, double, double>(values[0], values[1], values[2]));
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x000274F2 File Offset: 0x000256F2
		public override IColor GetInitializeColor()
		{
			return this.TransformToRGB(new ValueTuple<double, double, double>(0.0, 0.0, 0.0));
		}

		// Token: 0x04000402 RID: 1026
		private readonly CIEBasedColorSpaceTransformer colorSpaceTransformer;
	}
}
