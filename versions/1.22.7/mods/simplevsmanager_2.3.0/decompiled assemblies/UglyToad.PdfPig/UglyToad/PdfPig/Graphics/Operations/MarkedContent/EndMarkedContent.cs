using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.MarkedContent
{
	// Token: 0x0200014D RID: 333
	public class EndMarkedContent : IGraphicsStateOperation
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0002512E File Offset: 0x0002332E
		public string Operator
		{
			get
			{
				return "EMC";
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00025135 File Offset: 0x00023335
		private EndMarkedContent()
		{
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0002513D File Offset: 0x0002333D
		public void Run(IOperationContext operationContext)
		{
			operationContext.EndMarkedContent();
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00025145 File Offset: 0x00023345
		public void Write(Stream stream)
		{
			stream.WriteText("EMC", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00025159 File Offset: 0x00023359
		public override string ToString()
		{
			return "EMC";
		}

		// Token: 0x040003A3 RID: 931
		public const string Symbol = "EMC";

		// Token: 0x040003A4 RID: 932
		public static readonly EndMarkedContent Value = new EndMarkedContent();
	}
}
