using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Graphics.Operations.TextPositioning;
using UglyToad.PdfPig.Graphics.Operations.TextState;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics.Operations.TextShowing
{
	// Token: 0x0200012B RID: 299
	public class MoveToNextLineShowTextWithSpacing : IGraphicsStateOperation
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x00023692 File Offset: 0x00021892
		public string Operator
		{
			get
			{
				return "\"";
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00023699 File Offset: 0x00021899
		public decimal WordSpacing { get; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x000236A1 File Offset: 0x000218A1
		public decimal CharacterSpacing { get; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x000236A9 File Offset: 0x000218A9
		[CanBeNull]
		public byte[] Bytes { get; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x000236B1 File Offset: 0x000218B1
		[CanBeNull]
		public string Text { get; }

		// Token: 0x060006AA RID: 1706 RVA: 0x000236B9 File Offset: 0x000218B9
		public MoveToNextLineShowTextWithSpacing(decimal wordSpacing, decimal characterSpacing, string text)
		{
			this.WordSpacing = wordSpacing;
			this.CharacterSpacing = characterSpacing;
			this.Text = text;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000236D6 File Offset: 0x000218D6
		public MoveToNextLineShowTextWithSpacing(decimal wordSpacing, decimal characterSpacing, byte[] hexBytes)
		{
			this.WordSpacing = wordSpacing;
			this.CharacterSpacing = characterSpacing;
			this.Bytes = hexBytes;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000236F4 File Offset: 0x000218F4
		public void Run(IOperationContext operationContext)
		{
			SetWordSpacing setWordSpacing = new SetWordSpacing(this.WordSpacing);
			SetCharacterSpacing setCharacterSpacing = new SetCharacterSpacing(this.CharacterSpacing);
			MoveToNextLine value = MoveToNextLine.Value;
			ShowText showText = (this.Text != null) ? new ShowText(this.Text) : new ShowText(this.Bytes);
			setWordSpacing.Run(operationContext);
			setCharacterSpacing.Run(operationContext);
			value.Run(operationContext);
			showText.Run(operationContext);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002375C File Offset: 0x0002195C
		public void Write(Stream stream)
		{
			stream.WriteDecimal(this.WordSpacing);
			stream.WriteWhiteSpace();
			stream.WriteDecimal(this.CharacterSpacing);
			stream.WriteWhiteSpace();
			if (this.Bytes != null)
			{
				stream.WriteHex(this.Bytes);
			}
			else
			{
				stream.WriteText("(" + this.Text + ")", false);
			}
			stream.WriteNewLine();
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000237C8 File Offset: 0x000219C8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 4);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.WordSpacing);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.CharacterSpacing);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Text);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("\"");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000340 RID: 832
		public const string Symbol = "\"";
	}
}
