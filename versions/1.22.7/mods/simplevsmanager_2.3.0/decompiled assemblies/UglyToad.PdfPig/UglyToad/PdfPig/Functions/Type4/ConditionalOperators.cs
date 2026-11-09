using System;

namespace UglyToad.PdfPig.Functions.Type4
{
	// Token: 0x020001AF RID: 431
	internal sealed class ConditionalOperators
	{
		// Token: 0x06000A74 RID: 2676 RVA: 0x00034533 File Offset: 0x00032733
		private ConditionalOperators()
		{
		}

		// Token: 0x0200029E RID: 670
		internal sealed class If : Operator
		{
			// Token: 0x06000EB9 RID: 3769 RVA: 0x000432C0 File Offset: 0x000414C0
			public void Execute(ExecutionContext context)
			{
				InstructionSequence instructionSequence = (InstructionSequence)context.Stack.Pop();
				if ((bool)context.Stack.Pop())
				{
					instructionSequence.Execute(context);
				}
			}
		}

		// Token: 0x0200029F RID: 671
		internal sealed class IfElse : Operator
		{
			// Token: 0x06000EBB RID: 3771 RVA: 0x00043300 File Offset: 0x00041500
			public void Execute(ExecutionContext context)
			{
				InstructionSequence instructionSequence = (InstructionSequence)context.Stack.Pop();
				InstructionSequence instructionSequence2 = (InstructionSequence)context.Stack.Pop();
				if (Convert.ToBoolean(context.Stack.Pop()))
				{
					instructionSequence2.Execute(context);
					return;
				}
				instructionSequence.Execute(context);
			}
		}
	}
}
