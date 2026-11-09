using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Util;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x0200016D RID: 365
	public sealed class CalGrayColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x00026D27 File Offset: 0x00024F27
		public override int NumberOfColorComponents
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x00026D2A File Offset: 0x00024F2A
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.NumberOfColorComponents;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x00026D32 File Offset: 0x00024F32
		public IReadOnlyList<double> WhitePoint { get; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x00026D3A File Offset: 0x00024F3A
		public IReadOnlyList<double> BlackPoint { get; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x00026D42 File Offset: 0x00024F42
		public double Gamma { get; }

		// Token: 0x06000866 RID: 2150 RVA: 0x00026D4C File Offset: 0x00024F4C
		public CalGrayColorSpaceDetails([NotNull] IReadOnlyList<double> whitePoint, [CanBeNull] IReadOnlyList<double> blackPoint, double? gamma) : base(ColorSpace.CalGray)
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
			this.Gamma = (gamma ?? 1.0);
			this.colorSpaceTransformer = new CIEBasedColorSpaceTransformer(new ValueTuple<double, double, double>(this.WhitePoint[0], this.WhitePoint[1], this.WhitePoint[2]), RGBWorkingSpace.sRGB)
			{
				DecoderABC = (([TupleElementNames(new string[]
				{
					"A",
					"B",
					"C"
				})] ValueTuple<double, double, double> color) => new ValueTuple<double, double, double>(Math.Pow(color.Item1, this.Gamma), Math.Pow(color.Item2, this.Gamma), Math.Pow(color.Item3, this.Gamma))),
				MatrixABC = new Matrix3x3(this.WhitePoint[0], 0.0, 0.0, 0.0, this.WhitePoint[1], 0.0, 0.0, 0.0, this.WhitePoint[2])
			};
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00026F00 File Offset: 0x00025100
		private RGBColor TransformToRGB(double colorA)
		{
			ValueTuple<double, double, double> valueTuple = this.colorSpaceTransformer.TransformToRGB(new ValueTuple<double, double, double>(colorA, colorA, colorA));
			double item = valueTuple.Item1;
			double item2 = valueTuple.Item2;
			double item3 = valueTuple.Item3;
			return new RGBColor(item, item2, item3);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00026F3C File Offset: 0x0002513C
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded)
		{
			List<byte> list = new List<byte>();
			for (int i = 0; i < decoded.Count; i++)
			{
				double num = (double)decoded[i] / 255.0;
				double[] array = this.Process(new double[]
				{
					num
				});
				list.Add(ColorSpaceDetails.ConvertToByte(array[0]));
			}
			return list;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00026F94 File Offset: 0x00025194
		internal override double[] Process(params double[] values)
		{
			double item = this.colorSpaceTransformer.TransformToRGB(new ValueTuple<double, double, double>(values[0], values[0], values[0])).Item1;
			return new double[]
			{
				item
			};
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00026FCC File Offset: 0x000251CC
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
			return this.TransformToRGB(values[0]);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0002703B File Offset: 0x0002523B
		public override IColor GetInitializeColor()
		{
			return this.GetColor(new double[1]);
		}

		// Token: 0x040003FE RID: 1022
		private readonly CIEBasedColorSpaceTransformer colorSpaceTransformer;
	}
}
