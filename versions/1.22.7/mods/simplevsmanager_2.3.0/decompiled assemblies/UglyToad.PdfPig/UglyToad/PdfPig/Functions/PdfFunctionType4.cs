using System;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Functions.Type4;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Functions
{
	// Token: 0x020001AC RID: 428
	internal sealed class PdfFunctionType4 : PdfFunction
	{
		// Token: 0x06000A6D RID: 2669 RVA: 0x000343B0 File Offset: 0x000325B0
		internal PdfFunctionType4(StreamToken function, ArrayToken domain, ArrayToken range) : base(function, domain, range)
		{
			byte[] array = Enumerable.ToArray<byte>(base.FunctionStream.Data);
			string @string = OtherEncodings.Iso88591.GetString(array);
			this.instructions = InstructionSequenceBuilder.Parse(@string);
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x000343FA File Offset: 0x000325FA
		public override FunctionTypes FunctionType
		{
			get
			{
				return FunctionTypes.PostScript;
			}
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00034400 File Offset: 0x00032600
		public override double[] Eval(params double[] input)
		{
			ExecutionContext executionContext = new ExecutionContext(this.operators);
			for (int i = 0; i < input.Length; i++)
			{
				PdfRange domainForInput = base.GetDomainForInput(i);
				double num = PdfFunction.ClipToRange(input[i], domainForInput.Min, domainForInput.Max);
				executionContext.Stack.Push(num);
			}
			this.instructions.Execute(executionContext);
			int numberOfOutputParameters = base.NumberOfOutputParameters;
			int count = executionContext.Stack.Count;
			if (count < numberOfOutputParameters)
			{
				throw new ArgumentOutOfRangeException(string.Concat(new string[]
				{
					"The type 4 function returned ",
					count.ToString(),
					" values but the Range entry indicates that ",
					numberOfOutputParameters.ToString(),
					" values be returned."
				}));
			}
			double[] array = new double[numberOfOutputParameters];
			for (int j = numberOfOutputParameters - 1; j >= 0; j--)
			{
				PdfRange rangeForOutput = base.GetRangeForOutput(j);
				array[j] = executionContext.PopReal();
				array[j] = PdfFunction.ClipToRange(array[j], rangeForOutput.Min, rangeForOutput.Max);
			}
			return array;
		}

		// Token: 0x0400051A RID: 1306
		private readonly Operators operators = new Operators();

		// Token: 0x0400051B RID: 1307
		private readonly InstructionSequence instructions;
	}
}
