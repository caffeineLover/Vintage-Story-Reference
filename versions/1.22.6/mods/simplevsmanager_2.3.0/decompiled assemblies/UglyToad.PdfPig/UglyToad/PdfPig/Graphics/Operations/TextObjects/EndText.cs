using System;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Graphics.Operations.TextObjects
{
	// Token: 0x02000133 RID: 307
	public class EndText : IGraphicsStateOperation
	{
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x00023F53 File Offset: 0x00022153
		public string Operator
		{
			get
			{
				return "ET";
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00023F5A File Offset: 0x0002215A
		private EndText()
		{
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00023F62 File Offset: 0x00022162
		public void Run(IOperationContext operationContext)
		{
			operationContext.TextMatrices.TextMatrix = TransformationMatrix.Identity;
			operationContext.TextMatrices.TextLineMatrix = TransformationMatrix.Identity;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00023F84 File Offset: 0x00022184
		public void Write(Stream stream)
		{
			stream.WriteText("ET", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00023F98 File Offset: 0x00022198
		public override string ToString()
		{
			return "ET";
		}

		// Token: 0x04000357 RID: 855
		public const string Symbol = "ET";

		// Token: 0x04000358 RID: 856
		public static readonly EndText Value = new EndText();
	}
}
