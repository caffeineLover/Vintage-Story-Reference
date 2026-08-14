using System;
using System.Collections.Generic;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.InlineImages
{
	// Token: 0x02000150 RID: 336
	public class EndInlineImage : IGraphicsStateOperation
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0002529B File Offset: 0x0002349B
		public IReadOnlyList<byte> ImageData { get; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x000252A3 File Offset: 0x000234A3
		public string Operator
		{
			get
			{
				return "EI";
			}
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x000252AA File Offset: 0x000234AA
		public EndInlineImage(IReadOnlyList<byte> imageData)
		{
			if (imageData == null)
			{
				throw new ArgumentNullException("imageData");
			}
			this.ImageData = imageData;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x000252C8 File Offset: 0x000234C8
		public void Run(IOperationContext operationContext)
		{
			operationContext.EndInlineImage(this.ImageData);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x000252D6 File Offset: 0x000234D6
		public void Write(Stream stream)
		{
			stream.WriteText("EI", false);
			stream.WriteNewLine();
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x000252EA File Offset: 0x000234EA
		public override string ToString()
		{
			return "EI";
		}

		// Token: 0x040003A9 RID: 937
		public const string Symbol = "EI";
	}
}
