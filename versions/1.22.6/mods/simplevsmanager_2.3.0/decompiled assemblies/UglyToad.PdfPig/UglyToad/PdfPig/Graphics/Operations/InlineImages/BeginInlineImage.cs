using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.InlineImages
{
	// Token: 0x0200014E RID: 334
	public class BeginInlineImage : IGraphicsStateOperation
	{
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x0002516C File Offset: 0x0002336C
		public string Operator
		{
			get
			{
				return "BI";
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00025173 File Offset: 0x00023373
		private BeginInlineImage()
		{
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0002517B File Offset: 0x0002337B
		public void Run(IOperationContext operationContext)
		{
			operationContext.BeginInlineImage();
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00025183 File Offset: 0x00023383
		public void Write(Stream stream)
		{
			stream.WriteText("BI", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00025197 File Offset: 0x00023397
		public override string ToString()
		{
			return "BI";
		}

		// Token: 0x040003A5 RID: 933
		public const string Symbol = "BI";

		// Token: 0x040003A6 RID: 934
		public static readonly BeginInlineImage Value = new BeginInlineImage();
	}
}
