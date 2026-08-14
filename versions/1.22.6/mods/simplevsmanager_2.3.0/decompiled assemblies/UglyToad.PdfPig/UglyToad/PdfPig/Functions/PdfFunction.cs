using System;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Functions
{
	// Token: 0x020001A7 RID: 423
	public abstract class PdfFunction
	{
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x000338F7 File Offset: 0x00031AF7
		public DictionaryToken FunctionDictionary { get; }

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x000338FF File Offset: 0x00031AFF
		public StreamToken FunctionStream { get; }

		// Token: 0x06000A43 RID: 2627 RVA: 0x00033907 File Offset: 0x00031B07
		public PdfFunction(DictionaryToken function, ArrayToken domain, ArrayToken range)
		{
			this.FunctionDictionary = function;
			this.DomainValues = domain;
			this.RangeValues = range;
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00033932 File Offset: 0x00031B32
		public PdfFunction(StreamToken function, ArrayToken domain, ArrayToken range)
		{
			this.FunctionStream = function;
			this.DomainValues = domain;
			this.RangeValues = range;
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000A45 RID: 2629
		public abstract FunctionTypes FunctionType { get; }

		// Token: 0x06000A46 RID: 2630 RVA: 0x0003395D File Offset: 0x00031B5D
		public DictionaryToken GetDictionary()
		{
			if (this.FunctionStream != null)
			{
				return this.FunctionStream.StreamDictionary;
			}
			return this.FunctionDictionary;
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00033979 File Offset: 0x00031B79
		public int NumberOfOutputParameters
		{
			get
			{
				if (this.numberOfOutputValues == -1)
				{
					if (this.RangeValues == null)
					{
						this.numberOfOutputValues = 0;
					}
					else
					{
						this.numberOfOutputValues = this.RangeValues.Length / 2;
					}
				}
				return this.numberOfOutputValues;
			}
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x000339AE File Offset: 0x00031BAE
		public PdfRange GetRangeForOutput(int n)
		{
			return new PdfRange(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(this.RangeValues.Data), (NumericToken t) => t.Double), n);
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x000339EC File Offset: 0x00031BEC
		public int NumberOfInputParameters
		{
			get
			{
				if (this.numberOfInputValues == -1)
				{
					ArrayToken domainValues = this.DomainValues;
					this.numberOfInputValues = domainValues.Length / 2;
				}
				return this.numberOfInputValues;
			}
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00033A1D File Offset: 0x00031C1D
		public PdfRange GetDomainForInput(int n)
		{
			return new PdfRange(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(this.DomainValues.Data), (NumericToken t) => t.Double), n);
		}

		// Token: 0x06000A4B RID: 2635
		public abstract double[] Eval(params double[] input);

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x00033A59 File Offset: 0x00031C59
		protected ArrayToken RangeValues { get; }

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x00033A61 File Offset: 0x00031C61
		private ArrayToken DomainValues { get; }

		// Token: 0x06000A4E RID: 2638 RVA: 0x00033A6C File Offset: 0x00031C6C
		protected double[] ClipToRange(double[] inputValues)
		{
			ArrayToken rangeValues = this.RangeValues;
			double[] array2;
			if (rangeValues != null && rangeValues.Length > 0)
			{
				double[] array = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(rangeValues.Data), (NumericToken t) => t.Double));
				int num = array.Length / 2;
				array2 = new double[num];
				for (int i = 0; i < num; i++)
				{
					int num2 = i << 1;
					array2[i] = PdfFunction.ClipToRange(inputValues[i], array[num2], array[num2 + 1]);
				}
			}
			else
			{
				array2 = inputValues;
			}
			return array2;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00033B00 File Offset: 0x00031D00
		public static double ClipToRange(double x, double rangeMin, double rangeMax)
		{
			if (x < rangeMin)
			{
				return rangeMin;
			}
			if (x > rangeMax)
			{
				return rangeMax;
			}
			return x;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00033B0F File Offset: 0x00031D0F
		protected static double Interpolate(double x, double xRangeMin, double xRangeMax, double yRangeMin, double yRangeMax)
		{
			return yRangeMin + (x - xRangeMin) * (yRangeMax - yRangeMin) / (xRangeMax - xRangeMin);
		}

		// Token: 0x04000504 RID: 1284
		private int numberOfInputValues = -1;

		// Token: 0x04000505 RID: 1285
		private int numberOfOutputValues = -1;
	}
}
