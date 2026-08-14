using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Functions
{
	// Token: 0x020001AB RID: 427
	internal sealed class PdfFunctionType3 : PdfFunction
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x000340EC File Offset: 0x000322EC
		internal PdfFunctionType3(DictionaryToken function, ArrayToken domain, ArrayToken range, IReadOnlyList<PdfFunction> functionsArray, ArrayToken bounds, ArrayToken encode) : base(function, domain, range)
		{
			if (functionsArray == null || functionsArray.Count == 0)
			{
				throw new ArgumentNullException("functionsArray");
			}
			this.FunctionsArray = functionsArray;
			this.Bounds = bounds;
			this.Encode = encode;
			this.boundsValues = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(this.Bounds.Data), (NumericToken t) => t.Double));
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00034174 File Offset: 0x00032374
		internal PdfFunctionType3(StreamToken function, ArrayToken domain, ArrayToken range, IReadOnlyList<PdfFunction> functionsArray, ArrayToken bounds, ArrayToken encode) : base(function, domain, range)
		{
			if (functionsArray == null || functionsArray.Count == 0)
			{
				throw new ArgumentNullException("functionsArray");
			}
			this.FunctionsArray = functionsArray;
			this.Bounds = bounds;
			this.Encode = encode;
			this.boundsValues = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(this.Bounds.Data), (NumericToken t) => t.Double));
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x000341F9 File Offset: 0x000323F9
		public override FunctionTypes FunctionType
		{
			get
			{
				return FunctionTypes.Stitching;
			}
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x000341FC File Offset: 0x000323FC
		public override double[] Eval(params double[] input)
		{
			PdfFunction pdfFunction = null;
			double num = input[0];
			PdfRange domainForInput = base.GetDomainForInput(0);
			num = PdfFunction.ClipToRange(num, domainForInput.Min, domainForInput.Max);
			if (this.FunctionsArray.Count == 1)
			{
				pdfFunction = this.FunctionsArray[0];
				PdfRange encodeForParameter = this.GetEncodeForParameter(0);
				num = PdfFunction.Interpolate(num, domainForInput.Min, domainForInput.Max, encodeForParameter.Min, encodeForParameter.Max);
			}
			else
			{
				int num2 = this.boundsValues.Length;
				double[] array = new double[num2 + 2];
				int num3 = array.Length;
				array[0] = domainForInput.Min;
				array[num3 - 1] = domainForInput.Max;
				Array.Copy(this.boundsValues, 0, array, 1, num2);
				for (int i = 0; i < num3 - 1; i++)
				{
					if (num >= array[i] && (num < array[i + 1] || (i == num3 - 2 && num == array[i + 1])))
					{
						pdfFunction = this.FunctionsArray[i];
						PdfRange encodeForParameter2 = this.GetEncodeForParameter(i);
						num = PdfFunction.Interpolate(num, array[i], array[i + 1], encodeForParameter2.Min, encodeForParameter2.Max);
						break;
					}
				}
			}
			if (pdfFunction == null)
			{
				throw new IOException("partition not found in type 3 function");
			}
			double[] input2 = new double[]
			{
				num
			};
			double[] inputValues = pdfFunction.Eval(input2);
			return base.ClipToRange(inputValues);
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x00034359 File Offset: 0x00032559
		public IReadOnlyList<PdfFunction> FunctionsArray { get; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x00034361 File Offset: 0x00032561
		public ArrayToken Bounds { get; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x00034369 File Offset: 0x00032569
		public ArrayToken Encode { get; }

		// Token: 0x06000A6C RID: 2668 RVA: 0x00034371 File Offset: 0x00032571
		private PdfRange GetEncodeForParameter(int n)
		{
			return new PdfRange(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(this.Encode.Data), (NumericToken t) => t.Double), n);
		}

		// Token: 0x04000516 RID: 1302
		private readonly double[] boundsValues;
	}
}
