using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements.Text;
using QuestPDF.Elements.Text.Items;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000AD RID: 173
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TextBlockDescriptor : TextSpanDescriptor
	{
		// Token: 0x060004D3 RID: 1235 RVA: 0x0000C680 File Offset: 0x0000A880
		internal TextBlockDescriptor(TextBlock textBlock, TextBlockSpan textBlockSpan) : base(textBlockSpan)
		{
			this.TextBlock = textBlock;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0000C690 File Offset: 0x0000A890
		public TextBlockDescriptor AlignLeft()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.Left);
			return this;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0000C6A4 File Offset: 0x0000A8A4
		public TextBlockDescriptor AlignCenter()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.Center);
			return this;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0000C6B8 File Offset: 0x0000A8B8
		public TextBlockDescriptor AlignRight()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.Right);
			return this;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0000C6CC File Offset: 0x0000A8CC
		public TextBlockDescriptor Justify()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.Justify);
			return this;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0000C6E0 File Offset: 0x0000A8E0
		public TextBlockDescriptor AlignStart()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.Start);
			return this;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
		public TextBlockDescriptor AlignEnd()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.End);
			return this;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0000C708 File Offset: 0x0000A908
		public TextBlockDescriptor ClampLines(int maxLines, string ellipsis = "…")
		{
			if (maxLines < 0)
			{
				throw new ArgumentException("Line clamp must be greater or equal to zero", "maxLines");
			}
			this.TextBlock.LineClamp = new int?(maxLines);
			this.TextBlock.LineClampEllipsis = (ellipsis ?? "…");
			return this;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0000C745 File Offset: 0x0000A945
		public TextBlockDescriptor ParagraphSpacing(float value, Unit unit = Unit.Point)
		{
			if (value < 0f)
			{
				throw new ArgumentException("Paragraph spacing must be greater or equal to zero", "value");
			}
			this.TextBlock.ParagraphSpacing = value.ToPoints(unit);
			return this;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0000C772 File Offset: 0x0000A972
		public TextBlockDescriptor ParagraphFirstLineIndentation(float value, Unit unit = Unit.Point)
		{
			if (value < 0f)
			{
				throw new ArgumentException("Paragraph indentation must be greater or equal to zero", "value");
			}
			this.TextBlock.ParagraphFirstLineIndentation = value.ToPoints(unit);
			return this;
		}

		// Token: 0x0400028E RID: 654
		private TextBlock TextBlock;
	}
}
