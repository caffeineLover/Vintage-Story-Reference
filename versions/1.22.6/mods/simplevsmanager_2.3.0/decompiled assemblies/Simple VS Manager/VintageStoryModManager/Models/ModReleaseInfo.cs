using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000A6 RID: 166
	[NullableContext(1)]
	[Nullable(0)]
	[RequiredMember]
	public sealed class ModReleaseInfo
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000E05 RID: 3589 RVA: 0x00051ADF File Offset: 0x0004FCDF
		// (set) Token: 0x06000E06 RID: 3590 RVA: 0x00051AE7 File Offset: 0x0004FCE7
		[RequiredMember]
		public string Version { get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000E07 RID: 3591 RVA: 0x00051AF0 File Offset: 0x0004FCF0
		// (set) Token: 0x06000E08 RID: 3592 RVA: 0x00051AF8 File Offset: 0x0004FCF8
		[Nullable(2)]
		public string NormalizedVersion { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x00051B01 File Offset: 0x0004FD01
		// (set) Token: 0x06000E0A RID: 3594 RVA: 0x00051B09 File Offset: 0x0004FD09
		[RequiredMember]
		public Uri DownloadUri { get; set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x00051B12 File Offset: 0x0004FD12
		// (set) Token: 0x06000E0C RID: 3596 RVA: 0x00051B1A File Offset: 0x0004FD1A
		[Nullable(2)]
		public string FileName { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x00051B23 File Offset: 0x0004FD23
		// (set) Token: 0x06000E0E RID: 3598 RVA: 0x00051B2B File Offset: 0x0004FD2B
		public IReadOnlyList<string> GameVersionTags { get; set; } = Array.Empty<string>();

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x00051B34 File Offset: 0x0004FD34
		// (set) Token: 0x06000E10 RID: 3600 RVA: 0x00051B3C File Offset: 0x0004FD3C
		public bool IsCompatibleWithInstalledGame { get; set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E11 RID: 3601 RVA: 0x00051B45 File Offset: 0x0004FD45
		// (set) Token: 0x06000E12 RID: 3602 RVA: 0x00051B4D File Offset: 0x0004FD4D
		[Nullable(2)]
		public string Changelog { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x00051B56 File Offset: 0x0004FD56
		// (set) Token: 0x06000E14 RID: 3604 RVA: 0x00051B5E File Offset: 0x0004FD5E
		public int? Downloads { get; set; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x00051B67 File Offset: 0x0004FD67
		// (set) Token: 0x06000E16 RID: 3606 RVA: 0x00051B6F File Offset: 0x0004FD6F
		public DateTime? CreatedUtc { get; set; }

		// Token: 0x06000E17 RID: 3607 RVA: 0x00051B78 File Offset: 0x0004FD78
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public ModReleaseInfo()
		{
		}
	}
}
