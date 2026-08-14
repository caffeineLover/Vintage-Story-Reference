using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200004E RID: 78
	[NullableContext(1)]
	public interface IDynamicComponent
	{
		// Token: 0x060001E3 RID: 483
		DynamicComponentComposeResult Compose(DynamicContext context);
	}
}
