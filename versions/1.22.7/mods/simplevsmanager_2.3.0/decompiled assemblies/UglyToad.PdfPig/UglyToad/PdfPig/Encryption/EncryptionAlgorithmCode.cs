using System;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001CF RID: 463
	internal enum EncryptionAlgorithmCode
	{
		// Token: 0x04000599 RID: 1433
		Unrecognized,
		// Token: 0x0400059A RID: 1434
		Rc4OrAes40BitKey,
		// Token: 0x0400059B RID: 1435
		Rc4OrAesGreaterThan40BitKey,
		// Token: 0x0400059C RID: 1436
		UnpublishedAlgorithm40To128BitKey,
		// Token: 0x0400059D RID: 1437
		SecurityHandlerInDocument,
		// Token: 0x0400059E RID: 1438
		SecurityHandlerInDocument256,
		// Token: 0x0400059F RID: 1439
		UndocumentedDueToIso
	}
}
