using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000A8 RID: 168
	[NullableContext(2)]
	[Nullable(0)]
	[RequiredMember]
	public sealed class ModDatabaseSearchResult
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x00051D2A File Offset: 0x0004FF2A
		// (set) Token: 0x06000E46 RID: 3654 RVA: 0x00051D32 File Offset: 0x0004FF32
		[Nullable(1)]
		[RequiredMember]
		public string ModId { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x00051D3B File Offset: 0x0004FF3B
		// (set) Token: 0x06000E48 RID: 3656 RVA: 0x00051D43 File Offset: 0x0004FF43
		[Nullable(1)]
		[RequiredMember]
		public string Name { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x00051D4C File Offset: 0x0004FF4C
		// (set) Token: 0x06000E4A RID: 3658 RVA: 0x00051D54 File Offset: 0x0004FF54
		[Nullable(1)]
		public IReadOnlyList<string> AlternateIds { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<string>();

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x00051D5D File Offset: 0x0004FF5D
		// (set) Token: 0x06000E4C RID: 3660 RVA: 0x00051D65 File Offset: 0x0004FF65
		public string Summary { get; set; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x00051D6E File Offset: 0x0004FF6E
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x00051D76 File Offset: 0x0004FF76
		public string Author { get; set; }

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x00051D7F File Offset: 0x0004FF7F
		// (set) Token: 0x06000E50 RID: 3664 RVA: 0x00051D87 File Offset: 0x0004FF87
		[Nullable(1)]
		public IReadOnlyList<string> Tags { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<string>();

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x00051D90 File Offset: 0x0004FF90
		// (set) Token: 0x06000E52 RID: 3666 RVA: 0x00051D98 File Offset: 0x0004FF98
		public int Downloads { get; set; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x00051DA1 File Offset: 0x0004FFA1
		// (set) Token: 0x06000E54 RID: 3668 RVA: 0x00051DA9 File Offset: 0x0004FFA9
		public int Follows { get; set; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x00051DB2 File Offset: 0x0004FFB2
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x00051DBA File Offset: 0x0004FFBA
		public int TrendingPoints { get; set; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x00051DC3 File Offset: 0x0004FFC3
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x00051DCB File Offset: 0x0004FFCB
		public int Comments { get; set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x00051DD4 File Offset: 0x0004FFD4
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x00051DDC File Offset: 0x0004FFDC
		public string AssetId { get; set; }

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x00051DE5 File Offset: 0x0004FFE5
		// (set) Token: 0x06000E5C RID: 3676 RVA: 0x00051DED File Offset: 0x0004FFED
		public string UrlAlias { get; set; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x00051DF6 File Offset: 0x0004FFF6
		// (set) Token: 0x06000E5E RID: 3678 RVA: 0x00051DFE File Offset: 0x0004FFFE
		public string Side { get; set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00051E07 File Offset: 0x00050007
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x00051E0F File Offset: 0x0005000F
		public string LogoUrl { get; set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00051E18 File Offset: 0x00050018
		// (set) Token: 0x06000E62 RID: 3682 RVA: 0x00051E20 File Offset: 0x00050020
		public string LogoUrlSource { get; set; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x00051E29 File Offset: 0x00050029
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x00051E31 File Offset: 0x00050031
		public DateTime? LastReleasedUtc { get; set; }

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x00051E3A File Offset: 0x0005003A
		// (set) Token: 0x06000E66 RID: 3686 RVA: 0x00051E42 File Offset: 0x00050042
		public DateTime? CreatedUtc { get; set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x00051E4B File Offset: 0x0005004B
		// (set) Token: 0x06000E68 RID: 3688 RVA: 0x00051E53 File Offset: 0x00050053
		internal double Score { get; set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x00051E5C File Offset: 0x0005005C
		// (set) Token: 0x06000E6A RID: 3690 RVA: 0x00051E64 File Offset: 0x00050064
		public int? LatestReleaseDownloads { get; set; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x00051E6D File Offset: 0x0005006D
		// (set) Token: 0x06000E6C RID: 3692 RVA: 0x00051E75 File Offset: 0x00050075
		public ModDatabaseInfo DetailedInfo { get; set; }

		// Token: 0x06000E6D RID: 3693 RVA: 0x00051E7E File Offset: 0x0005007E
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public ModDatabaseSearchResult()
		{
		}
	}
}
