using System;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Functions
{
	// Token: 0x020001AA RID: 426
	internal sealed class PdfFunctionType2 : PdfFunction
	{
		// Token: 0x06000A5D RID: 2653 RVA: 0x00033F89 File Offset: 0x00032189
		internal PdfFunctionType2(DictionaryToken function, ArrayToken domain, ArrayToken range, ArrayToken c0, ArrayToken c1, double n) : base(function, domain, range)
		{
			this.C0 = c0;
			this.C1 = c1;
			this.N = n;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00033FAC File Offset: 0x000321AC
		internal PdfFunctionType2(StreamToken function, ArrayToken domain, ArrayToken range, ArrayToken c0, ArrayToken c1, double n) : base(function, domain, range)
		{
			this.C0 = c0;
			this.C1 = c1;
			this.N = n;
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00033FCF File Offset: 0x000321CF
		public override FunctionTypes FunctionType
		{
			get
			{
				return FunctionTypes.Exponential;
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00033FD4 File Offset: 0x000321D4
		public override double[] Eval(params double[] input)
		{
			double num = Math.Pow(input[0], this.N);
			double[] array = new double[Math.Min(this.C0.Length, this.C1.Length)];
			for (int i = 0; i < array.Length; i++)
			{
				double @double = ((NumericToken)this.C0[i]).Double;
				double double2 = ((NumericToken)this.C1[i]).Double;
				array[i] = @double + num * (double2 - @double);
			}
			return base.ClipToRange(array);
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00034060 File Offset: 0x00032260
		public ArrayToken C0 { get; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00034068 File Offset: 0x00032268
		public ArrayToken C1 { get; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00034070 File Offset: 0x00032270
		public double N { get; }

		// Token: 0x06000A64 RID: 2660 RVA: 0x00034078 File Offset: 0x00032278
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "FunctionType2{C0: ";
			int num = 1;
			ArrayToken c = this.C0;
			array[num] = ((c != null) ? c.ToString() : null);
			array[2] = " C1: ";
			int num2 = 3;
			ArrayToken c2 = this.C1;
			array[num2] = ((c2 != null) ? c2.ToString() : null);
			array[4] = " N: ";
			array[5] = this.N.ToString();
			array[6] = "}";
			return string.Concat(array);
		}
	}
}
