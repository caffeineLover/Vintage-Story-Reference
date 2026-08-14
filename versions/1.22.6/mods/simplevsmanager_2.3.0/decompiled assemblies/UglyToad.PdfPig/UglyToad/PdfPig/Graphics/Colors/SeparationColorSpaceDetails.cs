using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x0200016C RID: 364
	public sealed class SeparationColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x00026B47 File Offset: 0x00024D47
		public override int NumberOfColorComponents
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x00026B4A File Offset: 0x00024D4A
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.AlternateColorSpace.NumberOfColorComponents;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x00026B57 File Offset: 0x00024D57
		public NameToken Name { get; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x00026B5F File Offset: 0x00024D5F
		public ColorSpaceDetails AlternateColorSpace { get; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x00026B67 File Offset: 0x00024D67
		public PdfFunction TintFunction { get; }

		// Token: 0x0600085B RID: 2139 RVA: 0x00026B6F File Offset: 0x00024D6F
		public SeparationColorSpaceDetails(NameToken name, ColorSpaceDetails alternateColorSpaceDetails, PdfFunction tintFunction) : base(ColorSpace.Separation)
		{
			this.Name = name;
			this.AlternateColorSpace = alternateColorSpaceDetails;
			this.TintFunction = tintFunction;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00026B9C File Offset: 0x00024D9C
		internal override double[] Process(params double[] values)
		{
			double[] values2 = this.TintFunction.Eval(new double[]
			{
				values[0]
			});
			return this.AlternateColorSpace.Process(values2);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00026BD0 File Offset: 0x00024DD0
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
			return this.cache.GetOrAdd(values[0], delegate(double v)
			{
				double[] values2 = this.TintFunction.Eval(new double[]
				{
					v
				});
				return this.AlternateColorSpace.GetColor(values2);
			});
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00026C50 File Offset: 0x00024E50
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> values)
		{
			Dictionary<int, double[]> dictionary = new Dictionary<int, double[]>();
			List<byte> list = new List<byte>();
			for (int i = 0; i < values.Count; i += 3)
			{
				byte b = values[i++];
				double[] array;
				if (!dictionary.TryGetValue((int)b, ref array))
				{
					array = this.Process(new double[]
					{
						(double)b / 255.0
					});
					dictionary[(int)b] = array;
				}
				for (int j = 0; j < array.Length; j++)
				{
					list.Add(ColorSpaceDetails.ConvertToByte(array[j]));
				}
			}
			return list;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00026CDB File Offset: 0x00024EDB
		public override IColor GetInitializeColor()
		{
			return this.GetColor(new double[]
			{
				1.0
			});
		}

		// Token: 0x040003FA RID: 1018
		private readonly ConcurrentDictionary<double, IColor> cache = new ConcurrentDictionary<double, IColor>();
	}
}
