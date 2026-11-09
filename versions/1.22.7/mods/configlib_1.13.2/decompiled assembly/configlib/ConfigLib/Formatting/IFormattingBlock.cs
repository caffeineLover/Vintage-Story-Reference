using System;
using System.Runtime.CompilerServices;

namespace ConfigLib.Formatting
{
	// Token: 0x02000053 RID: 83
	[NullableContext(1)]
	internal interface IFormattingBlock : IConfigBlock
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600025F RID: 607
		float SortingWeight { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000260 RID: 608
		bool Collapsible { get; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000261 RID: 609
		bool StopCollapsible { get; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000262 RID: 610
		string Yaml { get; }

		// Token: 0x06000263 RID: 611
		bool Draw(string id);
	}
}
