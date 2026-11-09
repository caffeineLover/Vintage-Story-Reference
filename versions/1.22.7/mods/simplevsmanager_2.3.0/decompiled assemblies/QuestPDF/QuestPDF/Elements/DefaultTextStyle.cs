using System;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000C1 RID: 193
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DefaultTextStyle : ContainerElement
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0000E92C File Offset: 0x0000CB2C
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x0000E934 File Offset: 0x0000CB34
		public TextStyle TextStyle { get; set; } = TextStyle.Default;
	}
}
