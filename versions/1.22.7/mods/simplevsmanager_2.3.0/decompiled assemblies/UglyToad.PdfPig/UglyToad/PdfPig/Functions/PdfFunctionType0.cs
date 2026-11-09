using System;
using System.Collections;
using System.IO;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Functions
{
	// Token: 0x020001A9 RID: 425
	internal sealed class PdfFunctionType0 : PdfFunction
	{
		// Token: 0x06000A51 RID: 2641 RVA: 0x00033B1F File Offset: 0x00031D1F
		internal PdfFunctionType0(DictionaryToken function, ArrayToken domain, ArrayToken range, ArrayToken size, int bitsPerSample, int order, ArrayToken encode, ArrayToken decode) : base(function, domain, range)
		{
			this.Size = size;
			this.BitsPerSample = bitsPerSample;
			this.Order = order;
			this.EncodeValues = encode;
			this.DecodeValues = decode;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00033B52 File Offset: 0x00031D52
		internal PdfFunctionType0(StreamToken function, ArrayToken domain, ArrayToken range, ArrayToken size, int bitsPerSample, int order, ArrayToken encode, ArrayToken decode) : base(function, domain, range)
		{
			this.Size = size;
			this.BitsPerSample = bitsPerSample;
			this.Order = order;
			this.EncodeValues = encode;
			this.DecodeValues = decode;
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x00033B85 File Offset: 0x00031D85
		public override FunctionTypes FunctionType
		{
			get
			{
				return FunctionTypes.Sampled;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x00033B88 File Offset: 0x00031D88
		public ArrayToken Size { get; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x00033B90 File Offset: 0x00031D90
		public int BitsPerSample { get; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x00033B98 File Offset: 0x00031D98
		public int Order { get; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x00033BA0 File Offset: 0x00031DA0
		private ArrayToken EncodeValues { get; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x00033BA8 File Offset: 0x00031DA8
		private ArrayToken DecodeValues { get; }

		// Token: 0x06000A59 RID: 2649 RVA: 0x00033BB0 File Offset: 0x00031DB0
		public PdfRange? GetEncodeForParameter(int paramNum)
		{
			ArrayToken encodeValues = this.EncodeValues;
			if (encodeValues != null && encodeValues.Length >= paramNum * 2 + 1)
			{
				return new PdfRange?(new PdfRange(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(encodeValues.Data), (NumericToken t) => t.Double), paramNum));
			}
			return default(PdfRange?);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00033C18 File Offset: 0x00031E18
		public PdfRange? GetDecodeForParameter(int paramNum)
		{
			ArrayToken decodeValues = this.DecodeValues;
			if (decodeValues != null && decodeValues.Length >= paramNum * 2 + 1)
			{
				return new PdfRange?(new PdfRange(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(decodeValues.Data), (NumericToken t) => t.Double), paramNum));
			}
			return default(PdfRange?);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00033C80 File Offset: 0x00031E80
		private int[][] GetSamples()
		{
			if (this.samples == null)
			{
				int num = 1;
				int numberOfInputParameters = base.NumberOfInputParameters;
				int numberOfOutputParameters = base.NumberOfOutputParameters;
				ArrayToken size = this.Size;
				for (int i = 0; i < numberOfInputParameters; i++)
				{
					num *= (size[i] as NumericToken).Int;
				}
				this.samples = new int[num][];
				int bitsPerSample = this.BitsPerSample;
				BitArray bitArray = new BitArray(Enumerable.ToArray<byte>(base.FunctionStream.Data));
				for (int j = 0; j < num; j++)
				{
					this.samples[j] = new int[numberOfOutputParameters];
					for (int k = 0; k < numberOfOutputParameters; k++)
					{
						long num2 = 0L;
						for (int l = bitsPerSample - 1; l >= 0; l--)
						{
							num2 <<= 1;
							num2 |= (long)(bitArray[j * numberOfOutputParameters * bitsPerSample + k * bitsPerSample + l] ? 1UL : 0UL);
						}
						this.samples[j][k] = (int)num2;
					}
				}
			}
			return this.samples;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00033D84 File Offset: 0x00031F84
		public override double[] Eval(params double[] input)
		{
			double[] array = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(this.Size.Data), (NumericToken t) => t.Double));
			int bitsPerSample = this.BitsPerSample;
			double xRangeMax = Math.Pow(2.0, (double)bitsPerSample) - 1.0;
			int num = input.Length;
			int numberOfOutputParameters = base.NumberOfOutputParameters;
			int[] array2 = new int[num];
			int[] array3 = new int[num];
			input = Enumerable.ToArray<double>(input);
			for (int i = 0; i < num; i++)
			{
				PdfRange domainForInput = base.GetDomainForInput(i);
				PdfRange? encodeForParameter = this.GetEncodeForParameter(i);
				input[i] = PdfFunction.ClipToRange(input[i], domainForInput.Min, domainForInput.Max);
				input[i] = PdfFunction.Interpolate(input[i], domainForInput.Min, domainForInput.Max, encodeForParameter.Value.Min, encodeForParameter.Value.Max);
				input[i] = PdfFunction.ClipToRange(input[i], 0.0, array[i] - 1.0);
				array2[i] = (int)Math.Floor(input[i]);
				array3[i] = (int)Math.Ceiling(input[i]);
			}
			double[] array4 = new PdfFunctionType0.RInterpol(input, array2, array3, numberOfOutputParameters, this.Size, this.GetSamples()).RInterpolate();
			for (int j = 0; j < numberOfOutputParameters; j++)
			{
				PdfRange rangeForOutput = base.GetRangeForOutput(j);
				PdfRange? decodeForParameter = this.GetDecodeForParameter(j);
				if (decodeForParameter == null)
				{
					throw new IOException("Range missing in function /Decode entry");
				}
				array4[j] = PdfFunction.Interpolate(array4[j], 0.0, xRangeMax, decodeForParameter.Value.Min, decodeForParameter.Value.Max);
				array4[j] = PdfFunction.ClipToRange(array4[j], rangeForOutput.Min, rangeForOutput.Max);
			}
			return array4;
		}

		// Token: 0x0400050D RID: 1293
		private int[][] samples;

		// Token: 0x0200027E RID: 638
		internal class RInterpol
		{
			// Token: 0x06000E6C RID: 3692 RVA: 0x000426B2 File Offset: 0x000408B2
			internal RInterpol(double[] input, int[] inputPrev, int[] inputNext, int numberOfOutputValues, ArrayToken size, int[][] samples)
			{
				this.in_ = input;
				this.inPrev = inputPrev;
				this.inNext = inputNext;
				this.numberOfInputValues = input.Length;
				this.numberOfOutputValues = numberOfOutputValues;
				this.size = size;
				this.samples = samples;
			}

			// Token: 0x06000E6D RID: 3693 RVA: 0x000426F0 File Offset: 0x000408F0
			internal double[] RInterpolate()
			{
				return this.InternalRInterpol(new int[this.numberOfInputValues], 0);
			}

			// Token: 0x06000E6E RID: 3694 RVA: 0x00042704 File Offset: 0x00040904
			private double[] InternalRInterpol(int[] coord, int step)
			{
				double[] array = new double[this.numberOfOutputValues];
				if (step == this.in_.Length - 1)
				{
					if (this.inPrev[step] == this.inNext[step])
					{
						coord[step] = this.inPrev[step];
						int[] array2 = this.samples[this.CalcSampleIndex(coord)];
						for (int i = 0; i < this.numberOfOutputValues; i++)
						{
							array[i] = (double)array2[i];
						}
						return array;
					}
					coord[step] = this.inPrev[step];
					int[] array3 = this.samples[this.CalcSampleIndex(coord)];
					coord[step] = this.inNext[step];
					int[] array4 = this.samples[this.CalcSampleIndex(coord)];
					for (int j = 0; j < this.numberOfOutputValues; j++)
					{
						array[j] = PdfFunction.Interpolate(this.in_[step], (double)this.inPrev[step], (double)this.inNext[step], (double)array3[j], (double)array4[j]);
					}
					return array;
				}
				else
				{
					if (this.inPrev[step] == this.inNext[step])
					{
						coord[step] = this.inPrev[step];
						return this.InternalRInterpol(coord, step + 1);
					}
					coord[step] = this.inPrev[step];
					double[] array5 = this.InternalRInterpol(coord, step + 1);
					coord[step] = this.inNext[step];
					double[] array6 = this.InternalRInterpol(coord, step + 1);
					for (int k = 0; k < this.numberOfOutputValues; k++)
					{
						array[k] = PdfFunction.Interpolate(this.in_[step], (double)this.inPrev[step], (double)this.inNext[step], array5[k], array6[k]);
					}
					return array;
				}
			}

			// Token: 0x06000E6F RID: 3695 RVA: 0x0004288C File Offset: 0x00040A8C
			private int CalcSampleIndex(int[] vector)
			{
				double[] array = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(this.size.Data), (NumericToken t) => t.Double));
				int num = 0;
				int num2 = 1;
				int num3 = vector.Length;
				for (int i = num3 - 2; i >= 0; i--)
				{
					num2 = (int)((double)num2 * array[i]);
				}
				for (int j = num3 - 1; j >= 0; j--)
				{
					num += num2 * vector[j];
					if (j - 1 >= 0)
					{
						num2 = (int)((double)num2 / array[j - 1]);
					}
				}
				return num;
			}

			// Token: 0x040008AE RID: 2222
			private readonly double[] in_;

			// Token: 0x040008AF RID: 2223
			private readonly int[] inPrev;

			// Token: 0x040008B0 RID: 2224
			private readonly int[] inNext;

			// Token: 0x040008B1 RID: 2225
			private readonly int numberOfInputValues;

			// Token: 0x040008B2 RID: 2226
			private readonly int numberOfOutputValues;

			// Token: 0x040008B3 RID: 2227
			private readonly ArrayToken size;

			// Token: 0x040008B4 RID: 2228
			private readonly int[][] samples;
		}
	}
}
