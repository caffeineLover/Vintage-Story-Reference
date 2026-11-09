using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000A7 RID: 167
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class ModDatabaseInfo
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x00051B8B File Offset: 0x0004FD8B
		// (set) Token: 0x06000E19 RID: 3609 RVA: 0x00051B93 File Offset: 0x0004FD93
		[Nullable(1)]
		public IReadOnlyList<string> Tags { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<string>();

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x00051B9C File Offset: 0x0004FD9C
		// (set) Token: 0x06000E1B RID: 3611 RVA: 0x00051BA4 File Offset: 0x0004FDA4
		public string CachedTagsVersion { get; set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x00051BAD File Offset: 0x0004FDAD
		// (set) Token: 0x06000E1D RID: 3613 RVA: 0x00051BB5 File Offset: 0x0004FDB5
		public string AssetId { get; set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x00051BBE File Offset: 0x0004FDBE
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x00051BC6 File Offset: 0x0004FDC6
		public string ModPageUrl { get; set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x00051BCF File Offset: 0x0004FDCF
		// (set) Token: 0x06000E21 RID: 3617 RVA: 0x00051BD7 File Offset: 0x0004FDD7
		public string LatestCompatibleVersion { get; set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x00051BE0 File Offset: 0x0004FDE0
		// (set) Token: 0x06000E23 RID: 3619 RVA: 0x00051BE8 File Offset: 0x0004FDE8
		public string LatestVersion { get; set; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x00051BF1 File Offset: 0x0004FDF1
		// (set) Token: 0x06000E25 RID: 3621 RVA: 0x00051BF9 File Offset: 0x0004FDF9
		[Nullable(1)]
		public IReadOnlyList<string> RequiredGameVersions { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<string>();

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x00051C02 File Offset: 0x0004FE02
		// (set) Token: 0x06000E27 RID: 3623 RVA: 0x00051C0A File Offset: 0x0004FE0A
		public int? Downloads { get; set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x00051C13 File Offset: 0x0004FE13
		// (set) Token: 0x06000E29 RID: 3625 RVA: 0x00051C1B File Offset: 0x0004FE1B
		public int? Comments { get; set; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x00051C24 File Offset: 0x0004FE24
		// (set) Token: 0x06000E2B RID: 3627 RVA: 0x00051C2C File Offset: 0x0004FE2C
		public int? Follows { get; set; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x00051C35 File Offset: 0x0004FE35
		// (set) Token: 0x06000E2D RID: 3629 RVA: 0x00051C3D File Offset: 0x0004FE3D
		public int? TrendingPoints { get; set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x00051C46 File Offset: 0x0004FE46
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x00051C4E File Offset: 0x0004FE4E
		public string LogoUrl { get; set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x00051C57 File Offset: 0x0004FE57
		// (set) Token: 0x06000E31 RID: 3633 RVA: 0x00051C5F File Offset: 0x0004FE5F
		public string LogoUrlSource { get; set; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x00051C68 File Offset: 0x0004FE68
		// (set) Token: 0x06000E33 RID: 3635 RVA: 0x00051C70 File Offset: 0x0004FE70
		public int? DownloadsLastThirtyDays { get; set; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x00051C79 File Offset: 0x0004FE79
		// (set) Token: 0x06000E35 RID: 3637 RVA: 0x00051C81 File Offset: 0x0004FE81
		public int? DownloadsLastTenDays { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00051C8A File Offset: 0x0004FE8A
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x00051C92 File Offset: 0x0004FE92
		public DateTime? LastReleasedUtc { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00051C9B File Offset: 0x0004FE9B
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x00051CA3 File Offset: 0x0004FEA3
		public DateTime? CreatedUtc { get; set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00051CAC File Offset: 0x0004FEAC
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x00051CB4 File Offset: 0x0004FEB4
		public ModReleaseInfo LatestRelease { get; set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x00051CBD File Offset: 0x0004FEBD
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x00051CC5 File Offset: 0x0004FEC5
		public ModReleaseInfo LatestCompatibleRelease { get; set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00051CCE File Offset: 0x0004FECE
		// (set) Token: 0x06000E3F RID: 3647 RVA: 0x00051CD6 File Offset: 0x0004FED6
		[Nullable(1)]
		public IReadOnlyList<ModReleaseInfo> Releases { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<ModReleaseInfo>();

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00051CDF File Offset: 0x0004FEDF
		// (set) Token: 0x06000E41 RID: 3649 RVA: 0x00051CE7 File Offset: 0x0004FEE7
		public bool IsOfflineOnly { get; set; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x00051CF0 File Offset: 0x0004FEF0
		// (set) Token: 0x06000E43 RID: 3651 RVA: 0x00051CF8 File Offset: 0x0004FEF8
		public string Side { get; set; }
	}
}
