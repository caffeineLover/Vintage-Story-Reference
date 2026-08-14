using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000170 RID: 368
	public sealed class ICCBasedColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00027AAD File Offset: 0x00025CAD
		public override int NumberOfColorComponents { get; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x00027AB5 File Offset: 0x00025CB5
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.NumberOfColorComponents;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x00027ABD File Offset: 0x00025CBD
		[NotNull]
		public ColorSpaceDetails AlternateColorSpace { get; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x00027AC5 File Offset: 0x00025CC5
		[NotNull]
		public IReadOnlyList<double> Range { get; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00027ACD File Offset: 0x00025CCD
		[CanBeNull]
		public XmpMetadata Metadata { get; }

		// Token: 0x0600088B RID: 2187 RVA: 0x00027AD8 File Offset: 0x00025CD8
		internal ICCBasedColorSpaceDetails(int numberOfColorComponents, [CanBeNull] ColorSpaceDetails alternateColorSpaceDetails, [CanBeNull] IReadOnlyList<double> range, [CanBeNull] XmpMetadata metadata) : base(ColorSpace.ICCBased)
		{
			if (numberOfColorComponents != 1 && numberOfColorComponents != 3 && numberOfColorComponents != 4)
			{
				throw new ArgumentOutOfRangeException("numberOfColorComponents", "must be 1, 3 or 4");
			}
			this.NumberOfColorComponents = numberOfColorComponents;
			this.AlternateColorSpace = (alternateColorSpaceDetails ?? ((this.NumberOfColorComponents == 1) ? DeviceGrayColorSpaceDetails.Instance : ((this.NumberOfColorComponents == 3) ? DeviceRgbColorSpaceDetails.Instance : DeviceCmykColorSpaceDetails.Instance)));
			base.BaseType = this.AlternateColorSpace.BaseType;
			IReadOnlyList<double> readOnlyList = range;
			if (range == null)
			{
				readOnlyList = Enumerable.ToArray<double>(Enumerable.SelectMany<double[], double>(Enumerable.Select<int, double[]>(Enumerable.Range(0, numberOfColorComponents), (int x) => new double[]
				{
					default(double),
					1.0
				}), (double[] x) => x));
			}
			this.Range = readOnlyList;
			if (this.Range.Count != 2 * numberOfColorComponents)
			{
				string text = "range";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(71, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Must consist of exactly ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(2 * numberOfColorComponents);
				defaultInterpolatedStringHandler.AppendLiteral(" (2 x NumberOfColorComponents), but was passed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(range.Count);
				throw new ArgumentOutOfRangeException(text, range, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Metadata = metadata;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00027C13 File Offset: 0x00025E13
		internal override double[] Process(params double[] values)
		{
			return this.AlternateColorSpace.Process(values);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00027C24 File Offset: 0x00025E24
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
			return this.AlternateColorSpace.GetColor(values);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00027C98 File Offset: 0x00025E98
		public override IColor GetInitializeColor()
		{
			double[] values = Enumerable.ToArray<double>(Enumerable.Repeat<double>(PdfFunction.ClipToRange(0.0, this.Range[0], this.Range[1]), this.NumberOfColorComponents));
			return this.GetColor(values);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00027CE5 File Offset: 0x00025EE5
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded)
		{
			return this.AlternateColorSpace.Transform(decoded);
		}
	}
}
