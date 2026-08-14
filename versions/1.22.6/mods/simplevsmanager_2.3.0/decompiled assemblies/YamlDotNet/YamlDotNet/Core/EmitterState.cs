using System;

namespace YamlDotNet.Core
{
	// Token: 0x020000C7 RID: 199
	internal enum EmitterState
	{
		// Token: 0x0400014C RID: 332
		StreamStart,
		// Token: 0x0400014D RID: 333
		StreamEnd,
		// Token: 0x0400014E RID: 334
		FirstDocumentStart,
		// Token: 0x0400014F RID: 335
		DocumentStart,
		// Token: 0x04000150 RID: 336
		DocumentContent,
		// Token: 0x04000151 RID: 337
		DocumentEnd,
		// Token: 0x04000152 RID: 338
		FlowSequenceFirstItem,
		// Token: 0x04000153 RID: 339
		FlowSequenceItem,
		// Token: 0x04000154 RID: 340
		FlowMappingFirstKey,
		// Token: 0x04000155 RID: 341
		FlowMappingKey,
		// Token: 0x04000156 RID: 342
		FlowMappingSimpleValue,
		// Token: 0x04000157 RID: 343
		FlowMappingValue,
		// Token: 0x04000158 RID: 344
		BlockSequenceFirstItem,
		// Token: 0x04000159 RID: 345
		BlockSequenceItem,
		// Token: 0x0400015A RID: 346
		BlockMappingFirstKey,
		// Token: 0x0400015B RID: 347
		BlockMappingKey,
		// Token: 0x0400015C RID: 348
		BlockMappingSimpleValue,
		// Token: 0x0400015D RID: 349
		BlockMappingValue
	}
}
