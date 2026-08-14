using System;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200004F RID: 79
	public interface IDynamicComponent<TState> : IDynamicComponent where TState : struct
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001E4 RID: 484
		// (set) Token: 0x060001E5 RID: 485
		TState State { get; set; }
	}
}
