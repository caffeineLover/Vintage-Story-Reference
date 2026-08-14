using System;
using System.Runtime.CompilerServices;

namespace VSImGui.API
{
	// Token: 0x02000028 RID: 40
	[NullableContext(1)]
	public interface IImGuiRenderer
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000283 RID: 643
		// (remove) Token: 0x06000284 RID: 644
		event DrawCallbackDelegate Draw;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000285 RID: 645
		// (remove) Token: 0x06000286 RID: 646
		event Action Closed;

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000287 RID: 647
		[Nullable(2)]
		Style DefaultStyle { [NullableContext(2)] get; }

		// Token: 0x06000288 RID: 648
		void Show();
	}
}
