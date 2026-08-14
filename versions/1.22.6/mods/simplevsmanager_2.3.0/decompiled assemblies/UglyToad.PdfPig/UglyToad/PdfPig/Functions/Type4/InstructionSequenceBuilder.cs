using System;
using System.Collections.Generic;
using System.Globalization;

namespace UglyToad.PdfPig.Functions.Type4
{
	// Token: 0x020001B2 RID: 434
	internal sealed class InstructionSequenceBuilder : Parser.AbstractSyntaxHandler
	{
		// Token: 0x06000A84 RID: 2692 RVA: 0x00034757 File Offset: 0x00032957
		private InstructionSequenceBuilder()
		{
			this.seqStack.Push(this.mainSequence);
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00034786 File Offset: 0x00032986
		public InstructionSequence GetInstructionSequence()
		{
			return this.mainSequence;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00034790 File Offset: 0x00032990
		public static InstructionSequence Parse(string text)
		{
			InstructionSequenceBuilder instructionSequenceBuilder = new InstructionSequenceBuilder();
			Parser.Parse(text, instructionSequenceBuilder);
			return instructionSequenceBuilder.GetInstructionSequence();
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x000347B0 File Offset: 0x000329B0
		private InstructionSequence GetCurrentSequence()
		{
			return this.seqStack.Peek();
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x000347C0 File Offset: 0x000329C0
		public void Token(char[] text)
		{
			string text2 = string.Concat<char>(text);
			this.Token(text2);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x000347DC File Offset: 0x000329DC
		public override void Token(string token)
		{
			if ("{".Equals(token))
			{
				InstructionSequence instructionSequence = new InstructionSequence();
				this.GetCurrentSequence().AddProc(instructionSequence);
				this.seqStack.Push(instructionSequence);
				return;
			}
			if ("}".Equals(token))
			{
				this.seqStack.Pop();
				return;
			}
			int value;
			if (int.TryParse(token, 7, CultureInfo.InvariantCulture, ref value))
			{
				this.GetCurrentSequence().AddInteger(value);
				return;
			}
			double value2;
			if (double.TryParse(token, 167, CultureInfo.InvariantCulture, ref value2))
			{
				this.GetCurrentSequence().AddReal(value2);
				return;
			}
			this.GetCurrentSequence().AddName(token);
		}

		// Token: 0x0400051F RID: 1311
		private readonly InstructionSequence mainSequence = new InstructionSequence();

		// Token: 0x04000520 RID: 1312
		private readonly Stack<InstructionSequence> seqStack = new Stack<InstructionSequence>();
	}
}
