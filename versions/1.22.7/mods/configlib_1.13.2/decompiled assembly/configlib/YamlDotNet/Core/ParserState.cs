using System;

namespace YamlDotNet.Core
{
	// Token: 0x020001C4 RID: 452
	internal enum ParserState
	{
		// Token: 0x04000406 RID: 1030
		StreamStart,
		// Token: 0x04000407 RID: 1031
		StreamEnd,
		// Token: 0x04000408 RID: 1032
		ImplicitDocumentStart,
		// Token: 0x04000409 RID: 1033
		DocumentStart,
		// Token: 0x0400040A RID: 1034
		DocumentContent,
		// Token: 0x0400040B RID: 1035
		DocumentEnd,
		// Token: 0x0400040C RID: 1036
		BlockNode,
		// Token: 0x0400040D RID: 1037
		BlockNodeOrIndentlessSequence,
		// Token: 0x0400040E RID: 1038
		FlowNode,
		// Token: 0x0400040F RID: 1039
		BlockSequenceFirstEntry,
		// Token: 0x04000410 RID: 1040
		BlockSequenceEntry,
		// Token: 0x04000411 RID: 1041
		IndentlessSequenceEntry,
		// Token: 0x04000412 RID: 1042
		BlockMappingFirstKey,
		// Token: 0x04000413 RID: 1043
		BlockMappingKey,
		// Token: 0x04000414 RID: 1044
		BlockMappingValue,
		// Token: 0x04000415 RID: 1045
		FlowSequenceFirstEntry,
		// Token: 0x04000416 RID: 1046
		FlowSequenceEntry,
		// Token: 0x04000417 RID: 1047
		FlowSequenceEntryMappingKey,
		// Token: 0x04000418 RID: 1048
		FlowSequenceEntryMappingValue,
		// Token: 0x04000419 RID: 1049
		FlowSequenceEntryMappingEnd,
		// Token: 0x0400041A RID: 1050
		FlowMappingFirstKey,
		// Token: 0x0400041B RID: 1051
		FlowMappingKey,
		// Token: 0x0400041C RID: 1052
		FlowMappingValue,
		// Token: 0x0400041D RID: 1053
		FlowMappingEmptyValue
	}
}
