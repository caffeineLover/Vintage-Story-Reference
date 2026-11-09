using System;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001EC RID: 492
	internal enum EventType
	{
		// Token: 0x04000464 RID: 1124
		None,
		// Token: 0x04000465 RID: 1125
		StreamStart,
		// Token: 0x04000466 RID: 1126
		StreamEnd,
		// Token: 0x04000467 RID: 1127
		DocumentStart,
		// Token: 0x04000468 RID: 1128
		DocumentEnd,
		// Token: 0x04000469 RID: 1129
		Alias,
		// Token: 0x0400046A RID: 1130
		Scalar,
		// Token: 0x0400046B RID: 1131
		SequenceStart,
		// Token: 0x0400046C RID: 1132
		SequenceEnd,
		// Token: 0x0400046D RID: 1133
		MappingStart,
		// Token: 0x0400046E RID: 1134
		MappingEnd,
		// Token: 0x0400046F RID: 1135
		Comment
	}
}
