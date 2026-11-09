using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000046 RID: 70
	[NullableContext(2)]
	public interface IContainer
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001AC RID: 428
		// (set) Token: 0x060001AD RID: 429
		IElement Child { get; set; }
	}
}
