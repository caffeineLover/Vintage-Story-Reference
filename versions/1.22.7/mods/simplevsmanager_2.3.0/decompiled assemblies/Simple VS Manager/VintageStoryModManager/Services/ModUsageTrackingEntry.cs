using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200007F RID: 127
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ModUsageTrackingEntry
	{
		// Token: 0x06000B4B RID: 2891 RVA: 0x000464B4 File Offset: 0x000446B4
		[NullableContext(2)]
		public ModUsageTrackingEntry(string modId, string modVersion, string gameVersion, bool canSubmitVote, bool hasUserVote)
		{
			this.ModId = (string.IsNullOrWhiteSpace(modId) ? string.Empty : modId.Trim());
			this.ModVersion = (string.IsNullOrWhiteSpace(modVersion) ? string.Empty : modVersion.Trim());
			this.GameVersion = (string.IsNullOrWhiteSpace(gameVersion) ? string.Empty : gameVersion.Trim());
			this.CanSubmitVote = canSubmitVote;
			this.HasUserVote = hasUserVote;
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x00046522 File Offset: 0x00044722
		public string ModId { get; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0004652A File Offset: 0x0004472A
		public string ModVersion { get; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00046532 File Offset: 0x00044732
		public string GameVersion { get; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0004653A File Offset: 0x0004473A
		public bool CanSubmitVote { get; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00046542 File Offset: 0x00044742
		public bool HasUserVote { get; }
	}
}
