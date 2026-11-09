using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000BF RID: 191
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModVersionVoteComments
	{
		// Token: 0x06000FCA RID: 4042 RVA: 0x00054ADE File Offset: 0x00052CDE
		public ModVersionVoteComments(IReadOnlyList<string> notFunctional, IReadOnlyList<string> crashesOrFreezesGame)
		{
			this.NotFunctional = (notFunctional ?? Array.Empty<string>());
			this.CrashesOrFreezesGame = (crashesOrFreezesGame ?? Array.Empty<string>());
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000FCB RID: 4043 RVA: 0x00054B06 File Offset: 0x00052D06
		public IReadOnlyList<string> NotFunctional { get; }

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x00054B0E File Offset: 0x00052D0E
		public IReadOnlyList<string> CrashesOrFreezesGame { get; }

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00054B16 File Offset: 0x00052D16
		public static ModVersionVoteComments Empty { get; } = new ModVersionVoteComments(Array.Empty<string>(), Array.Empty<string>());
	}
}
