using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Writer;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x02000120 RID: 288
	public class SetStrokeColorSpace : IGraphicsStateOperation
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00022E6C File Offset: 0x0002106C
		public string Operator
		{
			get
			{
				return "CS";
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00022E73 File Offset: 0x00021073
		public NameToken Name { get; }

		// Token: 0x0600065A RID: 1626 RVA: 0x00022E7B File Offset: 0x0002107B
		public SetStrokeColorSpace(NameToken name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00022E99 File Offset: 0x00021099
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetStrokingColorspace(this.Name, null);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00022EB2 File Offset: 0x000210B2
		public void Write(Stream stream)
		{
			SetStrokeColorSpace.TokenWriter.WriteToken(this.Name, stream);
			stream.WriteText("CS", false);
			stream.WriteNewLine();
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00022ED8 File Offset: 0x000210D8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("CS");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000321 RID: 801
		private static readonly TokenWriter TokenWriter = new TokenWriter();

		// Token: 0x04000322 RID: 802
		public const string Symbol = "CS";
	}
}
