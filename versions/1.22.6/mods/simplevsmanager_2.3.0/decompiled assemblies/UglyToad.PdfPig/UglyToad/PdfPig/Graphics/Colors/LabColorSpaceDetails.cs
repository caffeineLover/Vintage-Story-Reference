using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x0200016F RID: 367
	public sealed class LabColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x00027573 File Offset: 0x00025773
		public override int NumberOfColorComponents
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x00027576 File Offset: 0x00025776
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.NumberOfColorComponents;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0002757E File Offset: 0x0002577E
		public IReadOnlyList<double> WhitePoint { get; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00027586 File Offset: 0x00025786
		public IReadOnlyList<double> BlackPoint { get; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0002758E File Offset: 0x0002578E
		public IReadOnlyList<double> Matrix { get; }

		// Token: 0x0600087F RID: 2175 RVA: 0x00027598 File Offset: 0x00025798
		public LabColorSpaceDetails([NotNull] IReadOnlyList<double> whitePoint, [CanBeNull] IReadOnlyList<double> blackPoint, [CanBeNull] IReadOnlyList<double> matrix) : base(ColorSpace.Lab)
		{
			object obj;
			if (whitePoint == null)
			{
				obj = null;
			}
			else
			{
				obj = Enumerable.ToArray<double>(Enumerable.Select<double, double>(whitePoint, (double v) => v));
			}
			object obj2 = obj;
			if (obj2 == null)
			{
				throw new ArgumentNullException("whitePoint");
			}
			this.WhitePoint = obj2;
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
			double[] array;
			if (blackPoint == null)
			{
				array = null;
			}
			else
			{
				array = Enumerable.ToArray<double>(Enumerable.Select<double, double>(blackPoint, (double v) => v));
			}
			this.BlackPoint = (array ?? new double[3]);
			if (this.BlackPoint.Count != 3)
			{
				string text2 = "blackPoint";
				defaultInterpolatedStringHandler..ctor(55, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Must consist of exactly three numbers, but was passed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(blackPoint.Count);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text2, blackPoint, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			double[] array2;
			if (matrix == null)
			{
				array2 = null;
			}
			else
			{
				array2 = Enumerable.ToArray<double>(Enumerable.Select<double, double>(matrix, (double v) => v));
			}
			double[] array3;
			if ((array3 = array2) == null)
			{
				RuntimeHelpers.InitializeArray(array3 = new double[4], fieldof(<PrivateImplementationDetails>.BB1ED8010E83BF28652A5A722817A69CA10AC6DBE8A7D9C3D9004239DC860B0B).FieldHandle);
			}
			this.Matrix = array3;
			if (this.Matrix.Count != 4)
			{
				string text3 = "matrix";
				defaultInterpolatedStringHandler..ctor(54, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Must consist of exactly four numbers, but was passed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(matrix.Count);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text3, matrix, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.colorSpaceTransformer = new CIEBasedColorSpaceTransformer(new ValueTuple<double, double, double>(this.WhitePoint[0], this.WhitePoint[1], this.WhitePoint[2]), RGBWorkingSpace.sRGB);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x000277A8 File Offset: 0x000259A8
		private RGBColor TransformToRGB([TupleElementNames(new string[]
		{
			"A",
			"B",
			"C"
		})] ValueTuple<double, double, double> colorAbc)
		{
			double[] array = this.Process(new double[]
			{
				colorAbc.Item1,
				colorAbc.Item2,
				colorAbc.Item3
			});
			return new RGBColor(array[0], array[1], array[2]);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000277EC File Offset: 0x000259EC
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

		// Token: 0x06000882 RID: 2178 RVA: 0x0002788E File Offset: 0x00025A8E
		private static double g(double x)
		{
			if (x > 0.20689655172413793)
			{
				return x * x * x;
			}
			return 0.12841854934601665 * (x - 0.13793103448275862);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x000278B8 File Offset: 0x00025AB8
		internal override double[] Process(params double[] values)
		{
			double num = PdfFunction.ClipToRange(values[1], this.Matrix[0], this.Matrix[1]);
			double num2 = PdfFunction.ClipToRange(values[2], this.Matrix[2], this.Matrix[3]);
			double num3 = (values[0] + 16.0) / 116.0;
			double x = num3 + num / 500.0;
			double x2 = num3 - num2 / 200.0;
			double num4 = this.WhitePoint[0] * LabColorSpaceDetails.g(x);
			double num5 = this.WhitePoint[1] * LabColorSpaceDetails.g(num3);
			double num6 = this.WhitePoint[2] * LabColorSpaceDetails.g(x2);
			ValueTuple<double, double, double> valueTuple = this.colorSpaceTransformer.TransformToRGB(new ValueTuple<double, double, double>(num4, num5, num6));
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

		// Token: 0x06000884 RID: 2180 RVA: 0x000279C0 File Offset: 0x00025BC0
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

		// Token: 0x06000885 RID: 2181 RVA: 0x00027A3C File Offset: 0x00025C3C
		public override IColor GetInitializeColor()
		{
			double num = PdfFunction.ClipToRange(0.0, this.Matrix[0], this.Matrix[1]);
			double num2 = PdfFunction.ClipToRange(0.0, this.Matrix[2], this.Matrix[3]);
			return this.TransformToRGB(new ValueTuple<double, double, double>(0.0, num, num2));
		}

		// Token: 0x04000407 RID: 1031
		private readonly CIEBasedColorSpaceTransformer colorSpaceTransformer;
	}
}
