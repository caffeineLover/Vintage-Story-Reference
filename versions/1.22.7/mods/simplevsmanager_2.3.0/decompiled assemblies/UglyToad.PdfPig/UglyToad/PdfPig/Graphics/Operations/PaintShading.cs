using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x02000114 RID: 276
	public class PaintShading : IGraphicsStateOperation
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x000221FB File Offset: 0x000203FB
		public string Operator
		{
			get
			{
				return "sh";
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x00022202 File Offset: 0x00020402
		public NameToken Name { get; }

		// Token: 0x06000601 RID: 1537 RVA: 0x0002220A File Offset: 0x0002040A
		public PaintShading(NameToken name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00022228 File Offset: 0x00020428
		public void Run(IOperationContext operationContext)
		{
			operationContext.PaintShading(this.Name);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00022236 File Offset: 0x00020436
		public void Write(Stream stream)
		{
			stream.WriteText("/" + this.Name.Data, false);
			stream.WriteWhiteSpace();
			stream.WriteText("sh", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0002226C File Offset: 0x0002046C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("sh");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040002FA RID: 762
		public const string Symbol = "sh";
	}
}
