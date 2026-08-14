using System;
using System.Collections.Generic;

namespace NVorbis.Contracts
{
	// Token: 0x02000028 RID: 40
	internal interface IHuffman
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001BB RID: 443
		int TableBits { get; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001BC RID: 444
		IReadOnlyList<HuffmanListNode> PrefixTree { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001BD RID: 445
		IReadOnlyList<HuffmanListNode> OverflowList { get; }

		// Token: 0x060001BE RID: 446
		void GenerateTable(IReadOnlyList<int> value, int[] lengthList, int[] codeList);
	}
}
