using System;
using System.Runtime.CompilerServices;

namespace ConfigLib.Formatting
{
	// Token: 0x02000051 RID: 81
	[NullableContext(1)]
	internal interface IFormattingBlock : IConfigBlock
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600023A RID: 570
		float SortingWeight { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600023B RID: 571
		bool Collapsible { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600023C RID: 572
		bool StopCollapsible { get; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600023D RID: 573
		string Yaml { get; }

		// Token: 0x0600023E RID: 574
		bool Draw(string id);
	}
}
