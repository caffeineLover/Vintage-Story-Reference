using System;

namespace YamlDotNet.Core
{
	// Token: 0x020000D5 RID: 213
	internal enum ParserState
	{
		// Token: 0x0400017D RID: 381
		StreamStart,
		// Token: 0x0400017E RID: 382
		StreamEnd,
		// Token: 0x0400017F RID: 383
		ImplicitDocumentStart,
		// Token: 0x04000180 RID: 384
		DocumentStart,
		// Token: 0x04000181 RID: 385
		DocumentContent,
		// Token: 0x04000182 RID: 386
		DocumentEnd,
		// Token: 0x04000183 RID: 387
		BlockNode,
		// Token: 0x04000184 RID: 388
		BlockNodeOrIndentlessSequence,
		// Token: 0x04000185 RID: 389
		FlowNode,
		// Token: 0x04000186 RID: 390
		BlockSequenceFirstEntry,
		// Token: 0x04000187 RID: 391
		BlockSequenceEntry,
		// Token: 0x04000188 RID: 392
		IndentlessSequenceEntry,
		// Token: 0x04000189 RID: 393
		BlockMappingFirstKey,
		// Token: 0x0400018A RID: 394
		BlockMappingKey,
		// Token: 0x0400018B RID: 395
		BlockMappingValue,
		// Token: 0x0400018C RID: 396
		FlowSequenceFirstEntry,
		// Token: 0x0400018D RID: 397
		FlowSequenceEntry,
		// Token: 0x0400018E RID: 398
		FlowSequenceEntryMappingKey,
		// Token: 0x0400018F RID: 399
		FlowSequenceEntryMappingValue,
		// Token: 0x04000190 RID: 400
		FlowSequenceEntryMappingEnd,
		// Token: 0x04000191 RID: 401
		FlowMappingFirstKey,
		// Token: 0x04000192 RID: 402
		FlowMappingKey,
		// Token: 0x04000193 RID: 403
		FlowMappingValue,
		// Token: 0x04000194 RID: 404
		FlowMappingEmptyValue
	}
}
