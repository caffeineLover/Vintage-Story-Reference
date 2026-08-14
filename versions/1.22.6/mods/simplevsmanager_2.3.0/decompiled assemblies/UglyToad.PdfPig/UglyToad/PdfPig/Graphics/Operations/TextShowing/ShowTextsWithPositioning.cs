using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Writer;

namespace UglyToad.PdfPig.Graphics.Operations.TextShowing
{
	// Token: 0x0200012D RID: 301
	public class ShowTextsWithPositioning : IGraphicsStateOperation
	{
		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00023959 File Offset: 0x00021B59
		public string Operator
		{
			get
			{
				return "TJ";
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00023960 File Offset: 0x00021B60
		public IReadOnlyList<IToken> Array { get; }

		// Token: 0x060006BA RID: 1722 RVA: 0x00023968 File Offset: 0x00021B68
		public ShowTextsWithPositioning(IReadOnlyList<IToken> array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			for (int i = 0; i < array.Count; i++)
			{
				IToken token = array[i];
				if (!(token is StringToken) && !(token is NumericToken) && !(token is HexToken))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(53, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Found invalid token for showing texts with position: ");
					defaultInterpolatedStringHandler.AppendFormatted<IToken>(token);
					throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			this.Array = array;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000239EC File Offset: 0x00021BEC
		public void Run(IOperationContext operationContext)
		{
			operationContext.ShowPositionedText(this.Array);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x000239FC File Offset: 0x00021BFC
		public void Write(Stream stream)
		{
			stream.WriteText("[", false);
			for (int i = 0; i < this.Array.Count; i++)
			{
				ShowTextsWithPositioning.TokenWriter.WriteToken(this.Array[i], stream);
				if (i < this.Array.Count - 1)
				{
					stream.WriteWhiteSpace();
				}
			}
			stream.WriteText("]", false);
			stream.WriteWhiteSpace();
			stream.WriteText("TJ", false);
			stream.WriteNewLine();
		}

		// Token: 0x04000348 RID: 840
		private static readonly TokenWriter TokenWriter = new TokenWriter();

		// Token: 0x04000349 RID: 841
		public const string Symbol = "TJ";
	}
}
