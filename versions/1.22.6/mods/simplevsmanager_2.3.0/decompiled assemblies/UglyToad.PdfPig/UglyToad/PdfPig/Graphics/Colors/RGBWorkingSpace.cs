using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x0200017F RID: 383
	internal class RGBWorkingSpace
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00028827 File Offset: 0x00026A27
		// (set) Token: 0x060008E4 RID: 2276 RVA: 0x0002882F File Offset: 0x00026A2F
		public Func<double, double> GammaCorrection { get; private set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00028838 File Offset: 0x00026A38
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x00028840 File Offset: 0x00026A40
		[TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})]
		public ValueTuple<double, double, double> ReferenceWhite { [return: TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})] get; [param: TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})] private set; }

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x00028849 File Offset: 0x00026A49
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x00028851 File Offset: 0x00026A51
		[TupleElementNames(new string[]
		{
			"x",
			"y",
			"Y"
		})]
		public ValueTuple<double, double, double> RedPrimary { [return: TupleElementNames(new string[]
		{
			"x",
			"y",
			"Y"
		})] get; [param: TupleElementNames(new string[]
		{
			"x",
			"y",
			"Y"
		})] private set; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0002885A File Offset: 0x00026A5A
		// (set) Token: 0x060008EA RID: 2282 RVA: 0x00028862 File Offset: 0x00026A62
		[TupleElementNames(new string[]
		{
			"x",
			"y",
			"Y"
		})]
		public ValueTuple<double, double, double> BluePrimary { [return: TupleElementNames(new string[]
		{
			"x",
			"y",
			"Y"
		})] get; [param: TupleElementNames(new string[]
		{
			"x",
			"y",
			"Y"
		})] private set; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0002886B File Offset: 0x00026A6B
		// (set) Token: 0x060008EC RID: 2284 RVA: 0x00028873 File Offset: 0x00026A73
		[TupleElementNames(new string[]
		{
			"x",
			"y",
			"Y"
		})]
		public ValueTuple<double, double, double> GreenPrimary { [return: TupleElementNames(new string[]
		{
			"x",
			"y",
			"Y"
		})] get; [param: TupleElementNames(new string[]
		{
			"x",
			"y",
			"Y"
		})] private set; }

		// Token: 0x060008ED RID: 2285 RVA: 0x0002887C File Offset: 0x00026A7C
		private static Func<double, double> CreateGammaFunc(double gamma)
		{
			return delegate(double val)
			{
				double num = Math.Pow(val, 1.0 / gamma);
				if (!double.IsNaN(num))
				{
					return num;
				}
				return 0.0;
			};
		}

		// Token: 0x0400043A RID: 1082
		public static readonly RGBWorkingSpace.XYZReferenceWhite ReferenceWhites = new RGBWorkingSpace.XYZReferenceWhite();

		// Token: 0x0400043B RID: 1083
		public static readonly RGBWorkingSpace AdobeRGB1998 = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D65,
			RedPrimary = new ValueTuple<double, double, double>(0.64, 0.33, 0.297361),
			GreenPrimary = new ValueTuple<double, double, double>(0.21, 0.71, 0.627355),
			BluePrimary = new ValueTuple<double, double, double>(0.15, 0.06, 0.075285)
		};

		// Token: 0x0400043C RID: 1084
		public static readonly RGBWorkingSpace AppleRGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(1.8),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D65,
			RedPrimary = new ValueTuple<double, double, double>(0.625, 0.34, 0.244634),
			GreenPrimary = new ValueTuple<double, double, double>(0.28, 0.595, 0.672034),
			BluePrimary = new ValueTuple<double, double, double>(0.155, 0.07, 0.083332)
		};

		// Token: 0x0400043D RID: 1085
		public static readonly RGBWorkingSpace BestRGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D50,
			RedPrimary = new ValueTuple<double, double, double>(0.7347, 0.2653, 0.228457),
			GreenPrimary = new ValueTuple<double, double, double>(0.215, 0.775, 0.737352),
			BluePrimary = new ValueTuple<double, double, double>(0.13, 0.035, 0.034191)
		};

		// Token: 0x0400043E RID: 1086
		public static readonly RGBWorkingSpace BetaRGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D50,
			RedPrimary = new ValueTuple<double, double, double>(0.6888, 0.3112, 0.303273),
			GreenPrimary = new ValueTuple<double, double, double>(0.1986, 0.7551, 0.663786),
			BluePrimary = new ValueTuple<double, double, double>(0.1265, 0.0352, 0.032941)
		};

		// Token: 0x0400043F RID: 1087
		public static readonly RGBWorkingSpace BruceRGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D65,
			RedPrimary = new ValueTuple<double, double, double>(0.64, 0.33, 0.240995),
			GreenPrimary = new ValueTuple<double, double, double>(0.28, 0.65, 0.683554),
			BluePrimary = new ValueTuple<double, double, double>(0.15, 0.06, 0.075452)
		};

		// Token: 0x04000440 RID: 1088
		public static readonly RGBWorkingSpace CIE_RGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.E,
			RedPrimary = new ValueTuple<double, double, double>(0.735, 0.265, 0.176204),
			GreenPrimary = new ValueTuple<double, double, double>(0.274, 0.717, 0.812985),
			BluePrimary = new ValueTuple<double, double, double>(0.167, 0.009, 0.010811)
		};

		// Token: 0x04000441 RID: 1089
		public static readonly RGBWorkingSpace ColorMatchRGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(1.8),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D50,
			RedPrimary = new ValueTuple<double, double, double>(0.63, 0.34, 0.274884),
			GreenPrimary = new ValueTuple<double, double, double>(0.295, 0.605, 0.658132),
			BluePrimary = new ValueTuple<double, double, double>(0.15, 0.075, 0.066985)
		};

		// Token: 0x04000442 RID: 1090
		public static readonly RGBWorkingSpace DonRGB4 = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D50,
			RedPrimary = new ValueTuple<double, double, double>(0.696, 0.3, 0.27835),
			GreenPrimary = new ValueTuple<double, double, double>(0.215, 0.765, 0.68797),
			BluePrimary = new ValueTuple<double, double, double>(0.13, 0.035, 0.03368)
		};

		// Token: 0x04000443 RID: 1091
		public static readonly RGBWorkingSpace EktaSpacePS5 = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D50,
			RedPrimary = new ValueTuple<double, double, double>(0.695, 0.305, 0.260629),
			GreenPrimary = new ValueTuple<double, double, double>(0.26, 0.7, 0.734946),
			BluePrimary = new ValueTuple<double, double, double>(0.11, 0.005, 0.004425)
		};

		// Token: 0x04000444 RID: 1092
		public static readonly RGBWorkingSpace NTSC_RGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.C,
			RedPrimary = new ValueTuple<double, double, double>(0.67, 0.33, 0.298839),
			GreenPrimary = new ValueTuple<double, double, double>(0.21, 0.71, 0.586811),
			BluePrimary = new ValueTuple<double, double, double>(0.14, 0.08, 0.11435)
		};

		// Token: 0x04000445 RID: 1093
		public static readonly RGBWorkingSpace PAL_SECAM_RGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D65,
			RedPrimary = new ValueTuple<double, double, double>(0.64, 0.33, 0.222021),
			GreenPrimary = new ValueTuple<double, double, double>(0.29, 0.6, 0.706645),
			BluePrimary = new ValueTuple<double, double, double>(0.15, 0.06, 0.071334)
		};

		// Token: 0x04000446 RID: 1094
		public static readonly RGBWorkingSpace ProPhotoRGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(1.8),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D50,
			RedPrimary = new ValueTuple<double, double, double>(0.7347, 0.2653, 0.28804),
			GreenPrimary = new ValueTuple<double, double, double>(0.1596, 0.8404, 0.711874),
			BluePrimary = new ValueTuple<double, double, double>(0.0366, 0.0001, 8.6E-05)
		};

		// Token: 0x04000447 RID: 1095
		public static readonly RGBWorkingSpace SMPTE_C_RGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D65,
			RedPrimary = new ValueTuple<double, double, double>(0.63, 0.34, 0.212395),
			GreenPrimary = new ValueTuple<double, double, double>(0.31, 0.595, 0.701049),
			BluePrimary = new ValueTuple<double, double, double>(0.155, 0.07, 0.086556)
		};

		// Token: 0x04000448 RID: 1096
		public static readonly RGBWorkingSpace sRGB = new RGBWorkingSpace
		{
			GammaCorrection = delegate(double val)
			{
				if (val > 0.0031308)
				{
					return 1.055 * Math.Pow(val, 0.4166666666666667) - 0.055;
				}
				return 12.92 * val;
			},
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D65,
			RedPrimary = new ValueTuple<double, double, double>(0.64, 0.33, 0.212656),
			GreenPrimary = new ValueTuple<double, double, double>(0.3, 0.6, 0.715158),
			BluePrimary = new ValueTuple<double, double, double>(0.15, 0.06, 0.072186)
		};

		// Token: 0x04000449 RID: 1097
		public static readonly RGBWorkingSpace WideGamutRGB = new RGBWorkingSpace
		{
			GammaCorrection = RGBWorkingSpace.CreateGammaFunc(2.2),
			ReferenceWhite = RGBWorkingSpace.ReferenceWhites.D50,
			RedPrimary = new ValueTuple<double, double, double>(0.735, 0.265, 0.258187),
			GreenPrimary = new ValueTuple<double, double, double>(0.115, 0.826, 0.724938),
			BluePrimary = new ValueTuple<double, double, double>(0.157, 0.018, 0.016875)
		};

		// Token: 0x02000273 RID: 627
		internal class XYZReferenceWhite
		{
			// Token: 0x06000E30 RID: 3632 RVA: 0x00041D74 File Offset: 0x0003FF74
			internal XYZReferenceWhite()
			{
			}

			// Token: 0x0400086E RID: 2158
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> A = new ValueTuple<double, double, double>(1.0985, 1.0, 0.35585);

			// Token: 0x0400086F RID: 2159
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> B = new ValueTuple<double, double, double>(0.99072, 1.0, 0.85223);

			// Token: 0x04000870 RID: 2160
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> C = new ValueTuple<double, double, double>(0.98074, 1.0, 1.18232);

			// Token: 0x04000871 RID: 2161
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> D50 = new ValueTuple<double, double, double>(0.96422, 1.0, 0.82521);

			// Token: 0x04000872 RID: 2162
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> D55 = new ValueTuple<double, double, double>(0.95682, 1.0, 0.92149);

			// Token: 0x04000873 RID: 2163
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> D65 = new ValueTuple<double, double, double>(0.95047, 1.0, 1.08883);

			// Token: 0x04000874 RID: 2164
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> D75 = new ValueTuple<double, double, double>(0.94972, 1.0, 1.22638);

			// Token: 0x04000875 RID: 2165
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> E = new ValueTuple<double, double, double>(1.0, 1.0, 1.0);

			// Token: 0x04000876 RID: 2166
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> F2 = new ValueTuple<double, double, double>(0.99186, 1.0, 0.67393);

			// Token: 0x04000877 RID: 2167
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> F7 = new ValueTuple<double, double, double>(0.95041, 1.0, 1.08747);

			// Token: 0x04000878 RID: 2168
			[TupleElementNames(new string[]
			{
				"X",
				"Y",
				"Z"
			})]
			public readonly ValueTuple<double, double, double> F11 = new ValueTuple<double, double, double>(1.00962, 1.0, 0.6435);
		}
	}
}
