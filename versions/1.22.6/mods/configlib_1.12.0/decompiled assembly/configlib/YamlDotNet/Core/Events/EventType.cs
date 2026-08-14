using System;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F0 RID: 496
	internal enum EventType
	{
		// Token: 0x04000492 RID: 1170
		None,
		// Token: 0x04000493 RID: 1171
		StreamStart,
		// Token: 0x04000494 RID: 1172
		StreamEnd,
		// Token: 0x04000495 RID: 1173
		DocumentStart,
		// Token: 0x04000496 RID: 1174
		DocumentEnd,
		// Token: 0x04000497 RID: 1175
		Alias,
		// Token: 0x04000498 RID: 1176
		Scalar,
		// Token: 0x04000499 RID: 1177
		SequenceStart,
		// Token: 0x0400049A RID: 1178
		SequenceEnd,
		// Token: 0x0400049B RID: 1179
		MappingStart,
		// Token: 0x0400049C RID: 1180
		MappingEnd,
		// Token: 0x0400049D RID: 1181
		Comment
	}
}
