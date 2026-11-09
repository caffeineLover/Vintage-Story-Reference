using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.Compatibility
{
	// Token: 0x02000159 RID: 345
	public class EndCompatibilitySection : IGraphicsStateOperation
	{
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00025826 File Offset: 0x00023A26
		public string Operator
		{
			get
			{
				return "EX";
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0002582D File Offset: 0x00023A2D
		private EndCompatibilitySection()
		{
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00025835 File Offset: 0x00023A35
		public void Run(IOperationContext operationContext)
		{
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00025837 File Offset: 0x00023A37
		public void Write(Stream stream)
		{
			stream.WriteText("EX", false);
			stream.WriteNewLine();
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0002584B File Offset: 0x00023A4B
		public override string ToString()
		{
			return "EX";
		}

		// Token: 0x040003BB RID: 955
		public const string Symbol = "EX";

		// Token: 0x040003BC RID: 956
		public static readonly EndCompatibilitySection Value = new EndCompatibilitySection();
	}
}
