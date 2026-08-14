using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Companion
{
	// Token: 0x02000131 RID: 305
	[NullableContext(2)]
	[Nullable(0)]
	internal static class HotReloadManager
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000ACC RID: 2764 RVA: 0x0001C2A4 File Offset: 0x0001A4A4
		// (remove) Token: 0x06000ACD RID: 2765 RVA: 0x0001C2D8 File Offset: 0x0001A4D8
		public static event EventHandler UpdateApplicationRequested;

		// Token: 0x06000ACE RID: 2766 RVA: 0x0001C30B File Offset: 0x0001A50B
		public static void UpdateApplication([Nullable(new byte[]
		{
			2,
			1
		})] Type[] _)
		{
			EventHandler updateApplicationRequested = HotReloadManager.UpdateApplicationRequested;
			if (updateApplicationRequested == null)
			{
				return;
			}
			updateApplicationRequested.Invoke(null, EventArgs.Empty);
		}
	}
}
