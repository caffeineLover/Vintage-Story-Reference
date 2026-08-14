using System;

namespace YamlDotNet.Core
{
	// Token: 0x020001B3 RID: 435
	internal enum EmitterState
	{
		// Token: 0x040003EE RID: 1006
		StreamStart,
		// Token: 0x040003EF RID: 1007
		StreamEnd,
		// Token: 0x040003F0 RID: 1008
		FirstDocumentStart,
		// Token: 0x040003F1 RID: 1009
		DocumentStart,
		// Token: 0x040003F2 RID: 1010
		DocumentContent,
		// Token: 0x040003F3 RID: 1011
		DocumentEnd,
		// Token: 0x040003F4 RID: 1012
		FlowSequenceFirstItem,
		// Token: 0x040003F5 RID: 1013
		FlowSequenceItem,
		// Token: 0x040003F6 RID: 1014
		FlowMappingFirstKey,
		// Token: 0x040003F7 RID: 1015
		FlowMappingKey,
		// Token: 0x040003F8 RID: 1016
		FlowMappingSimpleValue,
		// Token: 0x040003F9 RID: 1017
		FlowMappingValue,
		// Token: 0x040003FA RID: 1018
		BlockSequenceFirstItem,
		// Token: 0x040003FB RID: 1019
		BlockSequenceItem,
		// Token: 0x040003FC RID: 1020
		BlockMappingFirstKey,
		// Token: 0x040003FD RID: 1021
		BlockMappingKey,
		// Token: 0x040003FE RID: 1022
		BlockMappingSimpleValue,
		// Token: 0x040003FF RID: 1023
		BlockMappingValue
	}
}
