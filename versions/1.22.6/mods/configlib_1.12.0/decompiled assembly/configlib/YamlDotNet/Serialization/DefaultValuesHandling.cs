using System;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000082 RID: 130
	[Flags]
	public enum DefaultValuesHandling
	{
		// Token: 0x040001A1 RID: 417
		Preserve = 0,
		// Token: 0x040001A2 RID: 418
		OmitNull = 1,
		// Token: 0x040001A3 RID: 419
		OmitDefaults = 2,
		// Token: 0x040001A4 RID: 420
		OmitEmptyCollections = 4
	}
}
