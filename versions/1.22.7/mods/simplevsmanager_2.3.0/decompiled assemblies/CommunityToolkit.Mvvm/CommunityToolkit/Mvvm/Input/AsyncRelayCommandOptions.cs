using System;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x0200001F RID: 31
	[Flags]
	public enum AsyncRelayCommandOptions
	{
		// Token: 0x04000047 RID: 71
		None = 0,
		// Token: 0x04000048 RID: 72
		AllowConcurrentExecutions = 1,
		// Token: 0x04000049 RID: 73
		FlowExceptionsToTaskScheduler = 2
	}
}
