using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements.Text.Items;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000AA RID: 170
	[NullableContext(1)]
	[Nullable(0)]
	public class TextSpanDescriptor
	{
		// Token: 0x060004C9 RID: 1225 RVA: 0x0000C5E3 File Offset: 0x0000A7E3
		internal TextSpanDescriptor(TextBlockSpan textBlockSpan)
		{
			this.TextBlockSpan = textBlockSpan;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000C5F2 File Offset: 0x0000A7F2
		internal void MutateTextStyle<[Nullable(2)] T>(Func<TextStyle, T, TextStyle> handler, T argument)
		{
			this.TextBlockSpan.Style = handler.Invoke(this.TextBlockSpan.Style, argument);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0000C611 File Offset: 0x0000A811
		internal void MutateTextStyle(Func<TextStyle, TextStyle> handler)
		{
			this.TextBlockSpan.Style = handler.Invoke(this.TextBlockSpan.Style);
		}

		// Token: 0x0400028C RID: 652
		internal readonly TextBlockSpan TextBlockSpan;
	}
}
