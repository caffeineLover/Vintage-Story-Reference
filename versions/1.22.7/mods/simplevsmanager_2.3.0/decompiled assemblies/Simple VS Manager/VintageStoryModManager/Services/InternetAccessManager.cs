using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000064 RID: 100
	[NullableContext(2)]
	[Nullable(0)]
	public static class InternetAccessManager
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0003A2E9 File Offset: 0x000384E9
		public static bool IsInternetAccessDisabled
		{
			get
			{
				return Volatile.Read(ref InternetAccessManager._isInternetAccessDisabled) != 0;
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000998 RID: 2456 RVA: 0x0003A2F8 File Offset: 0x000384F8
		// (remove) Token: 0x06000999 RID: 2457 RVA: 0x0003A32C File Offset: 0x0003852C
		public static event EventHandler InternetAccessChanged;

		// Token: 0x0600099A RID: 2458 RVA: 0x0003A360 File Offset: 0x00038560
		public static void SetInternetAccessDisabled(bool disabled)
		{
			int num = (disabled > false) ? 1 : 0;
			if (Interlocked.Exchange(ref InternetAccessManager._isInternetAccessDisabled, num) == num)
			{
				return;
			}
			EventHandler internetAccessChanged = InternetAccessManager.InternetAccessChanged;
			if (internetAccessChanged == null)
			{
				return;
			}
			internetAccessChanged.Invoke(null, EventArgs.Empty);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0003A396 File Offset: 0x00038596
		public static void ThrowIfInternetAccessDisabled()
		{
			if (InternetAccessManager.IsInternetAccessDisabled)
			{
				throw new InternetAccessDisabledException();
			}
		}

		// Token: 0x04000494 RID: 1172
		private static int _isInternetAccessDisabled;
	}
}
