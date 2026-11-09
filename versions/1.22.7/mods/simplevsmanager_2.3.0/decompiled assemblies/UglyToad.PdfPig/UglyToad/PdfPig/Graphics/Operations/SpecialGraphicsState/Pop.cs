using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.SpecialGraphicsState
{
	// Token: 0x02000135 RID: 309
	public class Pop : IGraphicsStateOperation
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x000241BF File Offset: 0x000223BF
		public string Operator
		{
			get
			{
				return "Q";
			}
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x000241C6 File Offset: 0x000223C6
		private Pop()
		{
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x000241CE File Offset: 0x000223CE
		public void Run(IOperationContext operationContext)
		{
			if (operationContext.StackSize > 1)
			{
				operationContext.PopState();
				return;
			}
			throw new InvalidOperationException("Cannot execute a pop of the graphics state stack, it would leave the stack empty.");
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000241EA File Offset: 0x000223EA
		public void Write(Stream stream)
		{
			stream.WriteText("Q", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x000241FE File Offset: 0x000223FE
		public override string ToString()
		{
			return "Q";
		}

		// Token: 0x0400035B RID: 859
		public const string Symbol = "Q";

		// Token: 0x0400035C RID: 860
		public static readonly Pop Value = new Pop();
	}
}
