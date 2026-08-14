using System;

namespace YamlDotNet.Core
{
	// Token: 0x020001C8 RID: 456
	internal enum ParserState
	{
		// Token: 0x04000434 RID: 1076
		StreamStart,
		// Token: 0x04000435 RID: 1077
		StreamEnd,
		// Token: 0x04000436 RID: 1078
		ImplicitDocumentStart,
		// Token: 0x04000437 RID: 1079
		DocumentStart,
		// Token: 0x04000438 RID: 1080
		DocumentContent,
		// Token: 0x04000439 RID: 1081
		DocumentEnd,
		// Token: 0x0400043A RID: 1082
		BlockNode,
		// Token: 0x0400043B RID: 1083
		BlockNodeOrIndentlessSequence,
		// Token: 0x0400043C RID: 1084
		FlowNode,
		// Token: 0x0400043D RID: 1085
		BlockSequenceFirstEntry,
		// Token: 0x0400043E RID: 1086
		BlockSequenceEntry,
		// Token: 0x0400043F RID: 1087
		IndentlessSequenceEntry,
		// Token: 0x04000440 RID: 1088
		BlockMappingFirstKey,
		// Token: 0x04000441 RID: 1089
		BlockMappingKey,
		// Token: 0x04000442 RID: 1090
		BlockMappingValue,
		// Token: 0x04000443 RID: 1091
		FlowSequenceFirstEntry,
		// Token: 0x04000444 RID: 1092
		FlowSequenceEntry,
		// Token: 0x04000445 RID: 1093
		FlowSequenceEntryMappingKey,
		// Token: 0x04000446 RID: 1094
		FlowSequenceEntryMappingValue,
		// Token: 0x04000447 RID: 1095
		FlowSequenceEntryMappingEnd,
		// Token: 0x04000448 RID: 1096
		FlowMappingFirstKey,
		// Token: 0x04000449 RID: 1097
		FlowMappingKey,
		// Token: 0x0400044A RID: 1098
		FlowMappingValue,
		// Token: 0x0400044B RID: 1099
		FlowMappingEmptyValue
	}
}
