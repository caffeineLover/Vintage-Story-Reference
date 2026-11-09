using System;

namespace ModernWpf.Controls
{
	// Token: 0x0200008B RID: 139
	[Flags]
	public enum AnimationContext
	{
		// Token: 0x04000336 RID: 822
		None = 0,
		// Token: 0x04000337 RID: 823
		CollectionChangeAdd = 1,
		// Token: 0x04000338 RID: 824
		CollectionChangeRemove = 2,
		// Token: 0x04000339 RID: 825
		CollectionChangeReset = 4,
		// Token: 0x0400033A RID: 826
		LayoutTransition = 8
	}
}
