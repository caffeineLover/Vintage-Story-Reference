using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics.Operations.TextState
{
	// Token: 0x02000122 RID: 290
	public class SetFontAndSize : IGraphicsStateOperation
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00022FAE File Offset: 0x000211AE
		public string Operator
		{
			get
			{
				return "Tf";
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x00022FB5 File Offset: 0x000211B5
		[NotNull]
		public NameToken Font { get; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x00022FBD File Offset: 0x000211BD
		public decimal Size { get; }

		// Token: 0x06000668 RID: 1640 RVA: 0x00022FC5 File Offset: 0x000211C5
		public SetFontAndSize(NameToken font, decimal size)
		{
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			this.Font = font;
			this.Size = size;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00022FEA File Offset: 0x000211EA
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetFontAndSize(this.Font, (double)this.Size);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00023004 File Offset: 0x00021204
		public void Write(Stream stream)
		{
			stream.WriteText(this.Font.ToString(), false);
			stream.WriteWhiteSpace();
			stream.WriteNumberText(this.Size, "Tf");
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00023030 File Offset: 0x00021230
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.Font);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Size.ToString("G", CultureInfo.InvariantCulture));
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("Tf");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000326 RID: 806
		public const string Symbol = "Tf";
	}
}
