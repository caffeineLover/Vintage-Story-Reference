using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200005D RID: 93
	[NullableContext(2)]
	internal interface ISemanticAware
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600023B RID: 571
		// (set) Token: 0x0600023C RID: 572
		SemanticTreeManager SemanticTreeManager { get; set; }
	}
}
