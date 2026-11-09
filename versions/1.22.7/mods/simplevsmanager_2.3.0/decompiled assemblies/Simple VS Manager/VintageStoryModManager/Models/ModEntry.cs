using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000A9 RID: 169
	[NullableContext(2)]
	[Nullable(0)]
	[RequiredMember]
	public sealed class ModEntry
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x00051E9C File Offset: 0x0005009C
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x00051EA4 File Offset: 0x000500A4
		[Nullable(1)]
		[RequiredMember]
		public string ModId { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x00051EAD File Offset: 0x000500AD
		// (set) Token: 0x06000E71 RID: 3697 RVA: 0x00051EB5 File Offset: 0x000500B5
		[Nullable(1)]
		[RequiredMember]
		public string Name { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x00051EBE File Offset: 0x000500BE
		// (set) Token: 0x06000E73 RID: 3699 RVA: 0x00051EC6 File Offset: 0x000500C6
		public string ManifestName { get; set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x00051ECF File Offset: 0x000500CF
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x00051ED7 File Offset: 0x000500D7
		public string Version { get; set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x00051EE0 File Offset: 0x000500E0
		// (set) Token: 0x06000E77 RID: 3703 RVA: 0x00051EE8 File Offset: 0x000500E8
		public string NetworkVersion { get; set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x00051EF1 File Offset: 0x000500F1
		// (set) Token: 0x06000E79 RID: 3705 RVA: 0x00051EF9 File Offset: 0x000500F9
		public string Description { get; set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x00051F02 File Offset: 0x00050102
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x00051F0A File Offset: 0x0005010A
		[Nullable(1)]
		public IReadOnlyList<string> Authors { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<string>();

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x00051F13 File Offset: 0x00050113
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x00051F1B File Offset: 0x0005011B
		[Nullable(1)]
		public IReadOnlyList<string> Contributors { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<string>();

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x00051F24 File Offset: 0x00050124
		// (set) Token: 0x06000E7F RID: 3711 RVA: 0x00051F2C File Offset: 0x0005012C
		[Nullable(1)]
		public IReadOnlyList<ModDependencyInfo> Dependencies { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<ModDependencyInfo>();

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x00051F35 File Offset: 0x00050135
		// (set) Token: 0x06000E81 RID: 3713 RVA: 0x00051F3D File Offset: 0x0005013D
		public string Website { get; set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x00051F46 File Offset: 0x00050146
		// (set) Token: 0x06000E83 RID: 3715 RVA: 0x00051F4E File Offset: 0x0005014E
		[Nullable(1)]
		public string SourcePath { [NullableContext(1)] get; [NullableContext(1)] set; } = string.Empty;

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x00051F57 File Offset: 0x00050157
		// (set) Token: 0x06000E85 RID: 3717 RVA: 0x00051F5F File Offset: 0x0005015F
		public ModSourceKind SourceKind { get; set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00051F68 File Offset: 0x00050168
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x00051F70 File Offset: 0x00050170
		public byte[] IconBytes { get; set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x00051F79 File Offset: 0x00050179
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x00051F81 File Offset: 0x00050181
		public string Error { get; set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x00051F8A File Offset: 0x0005018A
		public bool HasErrors
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.Error);
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x00051F9A File Offset: 0x0005019A
		// (set) Token: 0x06000E8C RID: 3724 RVA: 0x00051FA2 File Offset: 0x000501A2
		public string LoadError { get; set; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00051FAB File Offset: 0x000501AB
		public bool HasLoadError
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.LoadError);
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x00051FBB File Offset: 0x000501BB
		// (set) Token: 0x06000E8F RID: 3727 RVA: 0x00051FC3 File Offset: 0x000501C3
		[Nullable(1)]
		public IReadOnlyList<ModDependencyInfo> MissingDependencies { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<ModDependencyInfo>();

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x00051FCC File Offset: 0x000501CC
		// (set) Token: 0x06000E91 RID: 3729 RVA: 0x00051FD4 File Offset: 0x000501D4
		public bool DependencyHasErrors { get; set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x00051FDD File Offset: 0x000501DD
		public bool HasDependencyIssues
		{
			get
			{
				return this.DependencyHasErrors || this.MissingDependencies.Count > 0;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x00051FF7 File Offset: 0x000501F7
		// (set) Token: 0x06000E94 RID: 3732 RVA: 0x00051FFF File Offset: 0x000501FF
		public string Side { get; set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x00052008 File Offset: 0x00050208
		// (set) Token: 0x06000E96 RID: 3734 RVA: 0x00052010 File Offset: 0x00050210
		public bool? RequiredOnClient { get; set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00052019 File Offset: 0x00050219
		// (set) Token: 0x06000E98 RID: 3736 RVA: 0x00052021 File Offset: 0x00050221
		public bool? RequiredOnServer { get; set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x0005202A File Offset: 0x0005022A
		// (set) Token: 0x06000E9A RID: 3738 RVA: 0x00052032 File Offset: 0x00050232
		public ModDatabaseInfo DatabaseInfo { get; set; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x0005203B File Offset: 0x0005023B
		// (set) Token: 0x06000E9C RID: 3740 RVA: 0x00052043 File Offset: 0x00050243
		public double? ModDatabaseSearchScore { get; set; }

		// Token: 0x06000E9D RID: 3741 RVA: 0x0005204C File Offset: 0x0005024C
		public void UpdateDatabaseInfo(ModDatabaseInfo info)
		{
			this.DatabaseInfo = info;
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00052055 File Offset: 0x00050255
		[NullableContext(1)]
		public override string ToString()
		{
			return this.Name + " (" + this.ModId + ")";
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00052072 File Offset: 0x00050272
		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public ModEntry()
		{
		}
	}
}
