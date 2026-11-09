using System;
using System.IO;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics.Operations.TextShowing
{
	// Token: 0x0200012C RID: 300
	public class ShowText : IGraphicsStateOperation
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0002383C File Offset: 0x00021A3C
		public string Operator
		{
			get
			{
				return "Tj";
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00023843 File Offset: 0x00021A43
		[CanBeNull]
		public string Text { get; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0002384B File Offset: 0x00021A4B
		[CanBeNull]
		public byte[] Bytes { get; }

		// Token: 0x060006B2 RID: 1714 RVA: 0x00023853 File Offset: 0x00021A53
		public ShowText(string text)
		{
			this.Text = text;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00023862 File Offset: 0x00021A62
		public ShowText(byte[] hexBytes)
		{
			this.Bytes = hexBytes;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00023874 File Offset: 0x00021A74
		public void Run(IOperationContext operationContext)
		{
			ByteArrayInputBytes bytes = new ByteArrayInputBytes((this.Text != null) ? OtherEncodings.StringAsLatin1Bytes(this.Text) : this.Bytes);
			operationContext.ShowText(bytes);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x000238A9 File Offset: 0x00021AA9
		private string EscapeText(string text)
		{
			if (text == null)
			{
				return null;
			}
			text = text.Replace("\\", "\\\\)");
			text = text.Replace("(", "\\(");
			text = text.Replace(")", "\\)");
			return text;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x000238E8 File Offset: 0x00021AE8
		public void Write(Stream stream)
		{
			if (this.Bytes != null)
			{
				stream.WriteHex(this.Bytes);
			}
			else
			{
				string text = this.EscapeText(this.Text);
				stream.WriteText("(" + text + ")", false);
			}
			stream.WriteWhiteSpace();
			stream.WriteText("Tj", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00023947 File Offset: 0x00021B47
		public override string ToString()
		{
			return this.Text + " Tj";
		}

		// Token: 0x04000345 RID: 837
		public const string Symbol = "Tj";
	}
}
