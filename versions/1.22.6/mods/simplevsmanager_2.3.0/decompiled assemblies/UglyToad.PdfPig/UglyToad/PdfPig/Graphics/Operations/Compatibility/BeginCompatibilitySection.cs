using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.Compatibility
{
	// Token: 0x02000158 RID: 344
	public class BeginCompatibilitySection : IGraphicsStateOperation
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x000257EE File Offset: 0x000239EE
		public string Operator
		{
			get
			{
				return "BX";
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x000257F5 File Offset: 0x000239F5
		private BeginCompatibilitySection()
		{
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x000257FD File Offset: 0x000239FD
		public void Run(IOperationContext operationContext)
		{
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000257FF File Offset: 0x000239FF
		public void Write(Stream stream)
		{
			stream.WriteText("BX", false);
			stream.WriteNewLine();
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00025813 File Offset: 0x00023A13
		public override string ToString()
		{
			return "BX";
		}

		// Token: 0x040003B9 RID: 953
		public const string Symbol = "BX";

		// Token: 0x040003BA RID: 954
		public static readonly BeginCompatibilitySection Value = new BeginCompatibilitySection();
	}
}
