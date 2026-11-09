using System;

namespace YamlDotNet.Core
{
	// Token: 0x020001AF RID: 431
	internal enum EmitterState
	{
		// Token: 0x040003C0 RID: 960
		StreamStart,
		// Token: 0x040003C1 RID: 961
		StreamEnd,
		// Token: 0x040003C2 RID: 962
		FirstDocumentStart,
		// Token: 0x040003C3 RID: 963
		DocumentStart,
		// Token: 0x040003C4 RID: 964
		DocumentContent,
		// Token: 0x040003C5 RID: 965
		DocumentEnd,
		// Token: 0x040003C6 RID: 966
		FlowSequenceFirstItem,
		// Token: 0x040003C7 RID: 967
		FlowSequenceItem,
		// Token: 0x040003C8 RID: 968
		FlowMappingFirstKey,
		// Token: 0x040003C9 RID: 969
		FlowMappingKey,
		// Token: 0x040003CA RID: 970
		FlowMappingSimpleValue,
		// Token: 0x040003CB RID: 971
		FlowMappingValue,
		// Token: 0x040003CC RID: 972
		BlockSequenceFirstItem,
		// Token: 0x040003CD RID: 973
		BlockSequenceItem,
		// Token: 0x040003CE RID: 974
		BlockMappingFirstKey,
		// Token: 0x040003CF RID: 975
		BlockMappingKey,
		// Token: 0x040003D0 RID: 976
		BlockMappingSimpleValue,
		// Token: 0x040003D1 RID: 977
		BlockMappingValue
	}
}
