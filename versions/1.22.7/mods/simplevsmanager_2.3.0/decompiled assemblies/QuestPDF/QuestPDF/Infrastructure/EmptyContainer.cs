using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000041 RID: 65
	public static class EmptyContainer
	{
		// Token: 0x060001AA RID: 426 RVA: 0x00004EE2 File Offset: 0x000030E2
		[NullableContext(1)]
		public static IContainer Create()
		{
			return new Container();
		}
	}
}
