using System;
using System.IO;
using UglyToad.PdfPig.Graphics.Operations.TextPositioning;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics.Operations.TextShowing
{
	// Token: 0x0200012A RID: 298
	public class MoveToNextLineShowText : IGraphicsStateOperation
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x000235AF File Offset: 0x000217AF
		public string Operator
		{
			get
			{
				return "'";
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x000235B6 File Offset: 0x000217B6
		[CanBeNull]
		public string Text { get; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x000235BE File Offset: 0x000217BE
		[CanBeNull]
		public byte[] Bytes { get; }

		// Token: 0x060006A0 RID: 1696 RVA: 0x000235C6 File Offset: 0x000217C6
		public MoveToNextLineShowText(string text)
		{
			this.Text = text;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x000235D5 File Offset: 0x000217D5
		public MoveToNextLineShowText(byte[] hexBytes)
		{
			this.Bytes = hexBytes;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x000235E4 File Offset: 0x000217E4
		public void Run(IOperationContext operationContext)
		{
			MoveToNextLine value = MoveToNextLine.Value;
			ShowText showText = (this.Text != null) ? new ShowText(this.Text) : new ShowText(this.Bytes);
			value.Run(operationContext);
			showText.Run(operationContext);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00023624 File Offset: 0x00021824
		public void Write(Stream stream)
		{
			if (this.Bytes == null)
			{
				stream.WriteText("(" + this.Text + ") '", false);
				stream.WriteNewLine();
				return;
			}
			stream.WriteHex(this.Bytes);
			stream.WriteWhiteSpace();
			stream.WriteText("'", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00023680 File Offset: 0x00021880
		public override string ToString()
		{
			return this.Text + " '";
		}

		// Token: 0x0400033D RID: 829
		public const string Symbol = "'";
	}
}
