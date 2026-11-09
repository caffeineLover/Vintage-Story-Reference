using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200008C RID: 140
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DebugLogScope : IDisposable
	{
		// Token: 0x06000BA3 RID: 2979 RVA: 0x00047D08 File Offset: 0x00045F08
		public void Dispose()
		{
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00047D0A File Offset: 0x00045F0A
		public void SetCacheStatus(bool cacheHit)
		{
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00047D0C File Offset: 0x00045F0C
		public void SetDetail(string key, [Nullable(2)] string value)
		{
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00047D0E File Offset: 0x00045F0E
		public void SetDetail(string key, int value)
		{
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00047D10 File Offset: 0x00045F10
		public void SetDetail(string key, long value)
		{
		}
	}
}
