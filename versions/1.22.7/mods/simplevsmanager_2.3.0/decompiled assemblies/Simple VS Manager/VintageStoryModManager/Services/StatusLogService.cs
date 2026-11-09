using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200008B RID: 139
	public static class StatusLogService
	{
		// Token: 0x06000BA1 RID: 2977 RVA: 0x00047D03 File Offset: 0x00045F03
		[NullableContext(1)]
		public static void AppendStatus(string message, bool isError)
		{
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00047D05 File Offset: 0x00045F05
		[NullableContext(2)]
		public static DebugLogScope BeginDebugScope(string modName, string modId, [Nullable(1)] string process)
		{
			return null;
		}
	}
}
