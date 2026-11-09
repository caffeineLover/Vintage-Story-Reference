using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000BD RID: 189
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModVersionVoteSummary
	{
		// Token: 0x06000FBD RID: 4029 RVA: 0x00054810 File Offset: 0x00052A10
		public ModVersionVoteSummary(string modId, string modVersion, [Nullable(2)] string vintageStoryVersion, ModVersionVoteCounts counts, ModVersionVoteComments comments, ModVersionVoteOption? userVote, [Nullable(2)] string userComment)
		{
			if (modId == null)
			{
				throw new ArgumentNullException("modId");
			}
			this.ModId = modId;
			if (modVersion == null)
			{
				throw new ArgumentNullException("modVersion");
			}
			this.ModVersion = modVersion;
			this.VintageStoryVersion = vintageStoryVersion;
			if (counts == null)
			{
				throw new ArgumentNullException("counts");
			}
			this.Counts = counts;
			if (comments == null)
			{
				throw new ArgumentNullException("comments");
			}
			this.Comments = comments;
			this.UserVote = userVote;
			this.UserComment = userComment;
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x00054894 File Offset: 0x00052A94
		public string ModId { get; }

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0005489C File Offset: 0x00052A9C
		public string ModVersion { get; }

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x000548A4 File Offset: 0x00052AA4
		[Nullable(2)]
		public string VintageStoryVersion { [NullableContext(2)] get; }

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x000548AC File Offset: 0x00052AAC
		public ModVersionVoteCounts Counts { get; }

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x000548B4 File Offset: 0x00052AB4
		public ModVersionVoteOption? UserVote { get; }

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x000548BC File Offset: 0x00052ABC
		[Nullable(2)]
		public string UserComment { [NullableContext(2)] get; }

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x000548C4 File Offset: 0x00052AC4
		public ModVersionVoteComments Comments { get; }

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x000548CC File Offset: 0x00052ACC
		public int TotalVotes
		{
			get
			{
				return this.Counts.Total;
			}
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x000548DC File Offset: 0x00052ADC
		public bool HasPositiveCompatibilityConsensus()
		{
			ModVersionVoteOption? majorityOption = this.GetMajorityOption();
			return ((majorityOption != null && majorityOption.GetValueOrDefault() == ModVersionVoteOption.FullyFunctional) || majorityOption == ModVersionVoteOption.NoIssuesSoFar) && this.Counts.GetCount(majorityOption.Value) >= 4;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00054930 File Offset: 0x00052B30
		public ModVersionVoteOption? GetMajorityOption()
		{
			int fullyFunctional = this.Counts.FullyFunctional;
			int noIssuesSoFar = this.Counts.NoIssuesSoFar;
			int someIssuesButWorks = this.Counts.SomeIssuesButWorks;
			int notFunctional = this.Counts.NotFunctional;
			int crashesOrFreezesGame = this.Counts.CrashesOrFreezesGame;
			int num = Math.Max(fullyFunctional, Math.Max(noIssuesSoFar, Math.Max(someIssuesButWorks, Math.Max(notFunctional, crashesOrFreezesGame))));
			if (num == 0)
			{
				return default(ModVersionVoteOption?);
			}
			bool flag = fullyFunctional == num;
			bool flag2 = noIssuesSoFar == num;
			bool flag3 = someIssuesButWorks == num;
			bool flag4 = notFunctional == num;
			bool flag5 = crashesOrFreezesGame == num;
			int num2 = 0;
			ModVersionVoteOption? result = default(ModVersionVoteOption?);
			if (flag)
			{
				num2++;
				result = new ModVersionVoteOption?(ModVersionVoteOption.FullyFunctional);
			}
			if (flag2)
			{
				num2++;
				result = new ModVersionVoteOption?(ModVersionVoteOption.NoIssuesSoFar);
			}
			if (flag3)
			{
				num2++;
				result = new ModVersionVoteOption?(ModVersionVoteOption.SomeIssuesButWorks);
			}
			if (flag4)
			{
				num2++;
				result = new ModVersionVoteOption?(ModVersionVoteOption.NotFunctional);
			}
			if (flag5)
			{
				num2++;
				result = new ModVersionVoteOption?(ModVersionVoteOption.CrashesOrFreezesGame);
			}
			if (num2 == 1)
			{
				return result;
			}
			if (num2 == 2 && flag && flag2)
			{
				return new ModVersionVoteOption?(ModVersionVoteOption.FullyFunctional);
			}
			return default(ModVersionVoteOption?);
		}

		// Token: 0x040006E1 RID: 1761
		private const int PositiveCompatibilityConsensusVoteCount = 4;
	}
}
