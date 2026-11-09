using System;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200007E RID: 126
	[Flags]
	public enum DefaultValuesHandling
	{
		// Token: 0x04000173 RID: 371
		Preserve = 0,
		// Token: 0x04000174 RID: 372
		OmitNull = 1,
		// Token: 0x04000175 RID: 373
		OmitDefaults = 2,
		// Token: 0x04000176 RID: 374
		OmitEmptyCollections = 4
	}
}
