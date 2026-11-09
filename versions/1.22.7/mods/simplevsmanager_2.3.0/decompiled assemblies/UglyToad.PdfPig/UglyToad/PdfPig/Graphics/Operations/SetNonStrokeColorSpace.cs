using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Writer;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x0200011A RID: 282
	public class SetNonStrokeColorSpace : IGraphicsStateOperation
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x00022830 File Offset: 0x00020A30
		public string Operator
		{
			get
			{
				return "cs";
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00022837 File Offset: 0x00020A37
		public NameToken Name { get; }

		// Token: 0x0600062D RID: 1581 RVA: 0x0002283F File Offset: 0x00020A3F
		public SetNonStrokeColorSpace(NameToken name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0002285D File Offset: 0x00020A5D
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetNonStrokingColorspace(this.Name, null);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00022876 File Offset: 0x00020A76
		public void Write(Stream stream)
		{
			SetNonStrokeColorSpace.TokenWriter.WriteToken(this.Name, stream);
			stream.WriteText("cs", false);
			stream.WriteNewLine();
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0002289C File Offset: 0x00020A9C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("cs");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400030D RID: 781
		private static readonly TokenWriter TokenWriter = new TokenWriter();

		// Token: 0x0400030E RID: 782
		public const string Symbol = "cs";
	}
}
