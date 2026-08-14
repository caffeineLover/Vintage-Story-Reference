using System;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200000A RID: 10
	[Flags]
	public enum DefaultValuesHandling
	{
		// Token: 0x04000010 RID: 16
		Preserve = 0,
		// Token: 0x04000011 RID: 17
		OmitNull = 1,
		// Token: 0x04000012 RID: 18
		OmitDefaults = 2,
		// Token: 0x04000013 RID: 19
		OmitEmptyCollections = 4
	}
}
