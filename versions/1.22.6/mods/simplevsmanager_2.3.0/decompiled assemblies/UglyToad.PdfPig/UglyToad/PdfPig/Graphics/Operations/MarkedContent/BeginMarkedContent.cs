using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Writer;

namespace UglyToad.PdfPig.Graphics.Operations.MarkedContent
{
	// Token: 0x02000149 RID: 329
	public class BeginMarkedContent : IGraphicsStateOperation
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x00024E41 File Offset: 0x00023041
		public string Operator
		{
			get
			{
				return "BMC";
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x00024E48 File Offset: 0x00023048
		public NameToken Name { get; }

		// Token: 0x06000774 RID: 1908 RVA: 0x00024E50 File Offset: 0x00023050
		public BeginMarkedContent(NameToken name)
		{
			this.Name = name;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00024E5F File Offset: 0x0002305F
		public void Run(IOperationContext operationContext)
		{
			operationContext.BeginMarkedContent(this.Name, null, null);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00024E6F File Offset: 0x0002306F
		public void Write(Stream stream)
		{
			BeginMarkedContent.TokenWriter.WriteToken(this.Name, stream);
			stream.WriteWhiteSpace();
			stream.WriteText("BMC", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00024E9C File Offset: 0x0002309C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("BMC");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000393 RID: 915
		private static readonly TokenWriter TokenWriter = new TokenWriter();

		// Token: 0x04000394 RID: 916
		public const string Symbol = "BMC";
	}
}
