using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000180 RID: 384
	public abstract class Shading
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00029219 File Offset: 0x00027419
		public DictionaryToken ShadingDictionary { get; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00029221 File Offset: 0x00027421
		public ShadingType ShadingType { get; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00029229 File Offset: 0x00027429
		public ColorSpaceDetails ColorSpace { get; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00029231 File Offset: 0x00027431
		public double[] Background { get; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00029239 File Offset: 0x00027439
		public PdfRectangle? BBox { get; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00029241 File Offset: 0x00027441
		public bool AntiAlias { get; }

		// Token: 0x060008F6 RID: 2294 RVA: 0x00029249 File Offset: 0x00027449
		protected internal Shading(ShadingType shadingType, bool antiAlias, DictionaryToken shadingDictionary, ColorSpaceDetails colorSpace, PdfRectangle? bbox, double[] background)
		{
			this.ShadingType = shadingType;
			this.AntiAlias = antiAlias;
			this.ShadingDictionary = shadingDictionary;
			this.ColorSpace = colorSpace;
			this.BBox = bbox;
			this.Background = background;
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060008F7 RID: 2295
		public abstract PdfFunction[] Functions { get; }

		// Token: 0x060008F8 RID: 2296 RVA: 0x00029280 File Offset: 0x00027480
		public double[] Eval(params double[] input)
		{
			if (this.Functions == null || this.Functions.Length == 0)
			{
				return input;
			}
			if (this.Functions.Length == 1)
			{
				return Shading.Clamp(this.Functions[0].Eval(input));
			}
			double[] array = new double[this.Functions.Length];
			for (int i = 0; i < this.Functions.Length; i++)
			{
				double[] array2 = this.Functions[i].Eval(input);
				array[i] = array2[0];
			}
			return Shading.Clamp(array);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x000292FC File Offset: 0x000274FC
		private static double[] Clamp(double[] input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] < 0.0)
				{
					input[i] = 0.0;
				}
				else if (input[i] > 1.0)
				{
					input[i] = 1.0;
				}
			}
			return input;
		}
	}
}
