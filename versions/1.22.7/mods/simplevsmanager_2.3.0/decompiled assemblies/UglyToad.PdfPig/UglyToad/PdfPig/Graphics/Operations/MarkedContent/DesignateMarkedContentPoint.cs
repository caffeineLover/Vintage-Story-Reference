using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Writer;

namespace UglyToad.PdfPig.Graphics.Operations.MarkedContent
{
	// Token: 0x0200014B RID: 331
	public class DesignateMarkedContentPoint : IGraphicsStateOperation
	{
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x00024FCA File Offset: 0x000231CA
		public string Operator
		{
			get
			{
				return "MP";
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x00024FD1 File Offset: 0x000231D1
		public NameToken Name { get; }

		// Token: 0x06000784 RID: 1924 RVA: 0x00024FD9 File Offset: 0x000231D9
		public DesignateMarkedContentPoint(NameToken name)
		{
			this.Name = name;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00024FE8 File Offset: 0x000231E8
		public void Run(IOperationContext operationContext)
		{
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00024FEA File Offset: 0x000231EA
		public void Write(Stream stream)
		{
			DesignateMarkedContentPoint.TokenWriter.WriteToken(this.Name, stream);
			stream.WriteWhiteSpace();
			stream.WriteText("MP", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00025018 File Offset: 0x00023218
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("MP");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400039B RID: 923
		private static readonly TokenWriter TokenWriter = new TokenWriter();

		// Token: 0x0400039C RID: 924
		public const string Symbol = "MP";
	}
}
