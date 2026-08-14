using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000BC RID: 188
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModVersionVoteCounts
	{
		// Token: 0x06000FB3 RID: 4019 RVA: 0x000546F8 File Offset: 0x000528F8
		public ModVersionVoteCounts(int fullyFunctional, int noIssuesSoFar, int someIssuesButWorks, int notFunctional, int crashesOrFreezesGame)
		{
			this.FullyFunctional = Math.Max(0, fullyFunctional);
			this.NoIssuesSoFar = Math.Max(0, noIssuesSoFar);
			this.SomeIssuesButWorks = Math.Max(0, someIssuesButWorks);
			this.NotFunctional = Math.Max(0, notFunctional);
			this.CrashesOrFreezesGame = Math.Max(0, crashesOrFreezesGame);
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0005474E File Offset: 0x0005294E
		public int FullyFunctional { get; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00054756 File Offset: 0x00052956
		public int NoIssuesSoFar { get; }

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0005475E File Offset: 0x0005295E
		public int SomeIssuesButWorks { get; }

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x00054766 File Offset: 0x00052966
		public int NotFunctional { get; }

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x0005476E File Offset: 0x0005296E
		public int CrashesOrFreezesGame { get; }

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x00054776 File Offset: 0x00052976
		public int Total
		{
			get
			{
				return this.FullyFunctional + this.NoIssuesSoFar + this.SomeIssuesButWorks + this.NotFunctional + this.CrashesOrFreezesGame;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0005479A File Offset: 0x0005299A
		public static ModVersionVoteCounts Empty { get; } = new ModVersionVoteCounts(0, 0, 0, 0, 0);

		// Token: 0x06000FBB RID: 4027 RVA: 0x000547A4 File Offset: 0x000529A4
		public int GetCount(ModVersionVoteOption option)
		{
			int result;
			switch (option)
			{
			case ModVersionVoteOption.FullyFunctional:
				result = this.FullyFunctional;
				break;
			case ModVersionVoteOption.NoIssuesSoFar:
				result = this.NoIssuesSoFar;
				break;
			case ModVersionVoteOption.SomeIssuesButWorks:
				result = this.SomeIssuesButWorks;
				break;
			case ModVersionVoteOption.NotFunctional:
				result = this.NotFunctional;
				break;
			case ModVersionVoteOption.CrashesOrFreezesGame:
				result = this.CrashesOrFreezesGame;
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}
	}
}
