using System;

namespace VintageStoryModManager.Views
{
	// Token: 0x02000015 RID: 21
	internal enum CompatibilityDecisionKind
	{
		// Token: 0x040000D0 RID: 208
		Latest,
		// Token: 0x040000D1 RID: 209
		LatestForAll,
		// Token: 0x040000D2 RID: 210
		LatestCompatible,
		// Token: 0x040000D3 RID: 211
		LatestCompatibleForAll,
		// Token: 0x040000D4 RID: 212
		Skip,
		// Token: 0x040000D5 RID: 213
		Abort
	}
}
