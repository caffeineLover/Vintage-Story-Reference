using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x0200016B RID: 363
	public sealed class DeviceNColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0002692E File Offset: 0x00024B2E
		public override int NumberOfColorComponents { get; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x00026936 File Offset: 0x00024B36
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.AlternateColorSpace.NumberOfColorComponents;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x00026943 File Offset: 0x00024B43
		public IReadOnlyList<NameToken> Names { get; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x0002694B File Offset: 0x00024B4B
		public ColorSpaceDetails AlternateColorSpace { get; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x00026953 File Offset: 0x00024B53
		public DeviceNColorSpaceDetails.DeviceNColorSpaceAttributes? Attributes { get; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x0002695B File Offset: 0x00024B5B
		public PdfFunction TintFunction { get; }

		// Token: 0x06000851 RID: 2129 RVA: 0x00026964 File Offset: 0x00024B64
		public DeviceNColorSpaceDetails(IReadOnlyList<NameToken> names, ColorSpaceDetails alternateColorSpaceDetails, PdfFunction tintFunction, DeviceNColorSpaceDetails.DeviceNColorSpaceAttributes? attributes = null) : base(ColorSpace.DeviceN)
		{
			this.Names = names;
			this.NumberOfColorComponents = this.Names.Count;
			this.AlternateColorSpace = alternateColorSpaceDetails;
			this.Attributes = attributes;
			this.TintFunction = tintFunction;
			base.BaseType = this.AlternateColorSpace.Type;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x000269B8 File Offset: 0x00024BB8
		internal override double[] Process(params double[] values)
		{
			double[] values2 = this.TintFunction.Eval(values);
			return this.AlternateColorSpace.Process(values2);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x000269E0 File Offset: 0x00024BE0
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
			double[] values2 = this.TintFunction.Eval(values);
			return this.AlternateColorSpace.GetColor(values2);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00026A60 File Offset: 0x00024C60
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded)
		{
			Dictionary<int, double[]> dictionary = new Dictionary<int, double[]>();
			List<byte> list = new List<byte>();
			for (int i = 0; i < decoded.Count; i += this.NumberOfColorComponents)
			{
				int num = 0;
				double[] array = new double[this.NumberOfColorComponents];
				for (int j = 0; j < this.NumberOfColorComponents; j++)
				{
					byte b = decoded[i + j];
					num = (num * 31 ^ (int)b);
					array[j] = (double)b / 255.0;
				}
				double[] array2;
				if (!dictionary.TryGetValue(num, ref array2))
				{
					array2 = this.Process(array);
					dictionary[num] = array2;
				}
				for (int k = 0; k < array2.Length; k++)
				{
					list.Add(ColorSpaceDetails.ConvertToByte(array2[k]));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00026B26 File Offset: 0x00024D26
		public override IColor GetInitializeColor()
		{
			return this.GetColor(Enumerable.ToArray<double>(Enumerable.Repeat<double>(1.0, this.NumberOfColorComponents)));
		}

		// Token: 0x02000270 RID: 624
		public readonly struct DeviceNColorSpaceAttributes
		{
			// Token: 0x1700040F RID: 1039
			// (get) Token: 0x06000E21 RID: 3617 RVA: 0x00041CCB File Offset: 0x0003FECB
			public NameToken Subtype { get; }

			// Token: 0x17000410 RID: 1040
			// (get) Token: 0x06000E22 RID: 3618 RVA: 0x00041CD3 File Offset: 0x0003FED3
			public DictionaryToken Colorants { get; }

			// Token: 0x17000411 RID: 1041
			// (get) Token: 0x06000E23 RID: 3619 RVA: 0x00041CDB File Offset: 0x0003FEDB
			public DictionaryToken Process { get; }

			// Token: 0x17000412 RID: 1042
			// (get) Token: 0x06000E24 RID: 3620 RVA: 0x00041CE3 File Offset: 0x0003FEE3
			public DictionaryToken MixingHints { get; }

			// Token: 0x06000E25 RID: 3621 RVA: 0x00041CEB File Offset: 0x0003FEEB
			public DeviceNColorSpaceAttributes()
			{
				this.Subtype = NameToken.Devicen;
				this.Colorants = null;
				this.Process = null;
				this.MixingHints = null;
			}

			// Token: 0x06000E26 RID: 3622 RVA: 0x00041D0D File Offset: 0x0003FF0D
			public DeviceNColorSpaceAttributes(NameToken subtype, DictionaryToken colorants, DictionaryToken process, DictionaryToken mixingHints)
			{
				this.Subtype = subtype;
				this.Colorants = colorants;
				this.Process = process;
				this.MixingHints = mixingHints;
			}
		}
	}
}
