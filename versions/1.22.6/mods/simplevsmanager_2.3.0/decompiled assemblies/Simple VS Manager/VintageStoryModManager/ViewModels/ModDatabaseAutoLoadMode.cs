using System;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000054 RID: 84
	public enum ModDatabaseAutoLoadMode
	{
		// Token: 0x0400043B RID: 1083
		TotalDownloads,
		// Token: 0x0400043C RID: 1084
		DownloadsLastThirtyDays,
		// Token: 0x0400043D RID: 1085
		DownloadsLastTenDays,
		// Token: 0x0400043E RID: 1086
		DownloadsNewModsRecentMonths,
		// Token: 0x0400043F RID: 1087
		RecentlyUpdated,
		// Token: 0x04000440 RID: 1088
		RecentlyAdded,
		// Token: 0x04000441 RID: 1089
		MostTrending,
		// Token: 0x04000442 RID: 1090
		AddedLast30Days,
		// Token: 0x04000443 RID: 1091
		Random
	}
}
