using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200005E RID: 94
	[NullableContext(1)]
	internal interface IStateful
	{
		// Token: 0x0600023D RID: 573
		void ResetState(bool hardReset = true);

		// Token: 0x0600023E RID: 574
		object GetState();

		// Token: 0x0600023F RID: 575
		void SetState(object state);
	}
}
