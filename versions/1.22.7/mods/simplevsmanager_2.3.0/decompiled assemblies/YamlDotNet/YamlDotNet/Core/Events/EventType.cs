using System;

namespace YamlDotNet.Core.Events
{
	// Token: 0x02000106 RID: 262
	internal enum EventType
	{
		// Token: 0x040001E6 RID: 486
		None,
		// Token: 0x040001E7 RID: 487
		StreamStart,
		// Token: 0x040001E8 RID: 488
		StreamEnd,
		// Token: 0x040001E9 RID: 489
		DocumentStart,
		// Token: 0x040001EA RID: 490
		DocumentEnd,
		// Token: 0x040001EB RID: 491
		Alias,
		// Token: 0x040001EC RID: 492
		Scalar,
		// Token: 0x040001ED RID: 493
		SequenceStart,
		// Token: 0x040001EE RID: 494
		SequenceEnd,
		// Token: 0x040001EF RID: 495
		MappingStart,
		// Token: 0x040001F0 RID: 496
		MappingEnd,
		// Token: 0x040001F1 RID: 497
		Comment
	}
}
