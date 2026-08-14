using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000BE RID: 190
	public static class ModVersionVoteOptionExtensions
	{
		// Token: 0x06000FC8 RID: 4040 RVA: 0x00054A50 File Offset: 0x00052C50
		[NullableContext(1)]
		public static string ToDisplayString(this ModVersionVoteOption option)
		{
			string result;
			switch (option)
			{
			case ModVersionVoteOption.FullyFunctional:
				result = "Fully functional";
				break;
			case ModVersionVoteOption.NoIssuesSoFar:
				result = "No issues noticed";
				break;
			case ModVersionVoteOption.SomeIssuesButWorks:
				result = "Some issues but works";
				break;
			case ModVersionVoteOption.NotFunctional:
				result = "Not functional";
				break;
			case ModVersionVoteOption.CrashesOrFreezesGame:
				result = "Crashes/Freezes game";
				break;
			default:
				result = (option.ToString() ?? string.Empty);
				break;
			}
			return result;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00054ABC File Offset: 0x00052CBC
		public static bool RequiresComment(this ModVersionVoteOption option)
		{
			return option == ModVersionVoteOption.NotFunctional || option == ModVersionVoteOption.CrashesOrFreezesGame;
		}
	}
}
