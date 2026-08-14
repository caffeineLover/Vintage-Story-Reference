using System;
using System.Windows.Threading;

namespace ModernWpf
{
	// Token: 0x02000215 RID: 533
	internal static class DispatcherHelper
	{
		// Token: 0x06000FA7 RID: 4007 RVA: 0x0002CB34 File Offset: 0x0002AD34
		public static void DoEvents(DispatcherPriority priority = 4)
		{
			DispatcherFrame dispatcherFrame = new DispatcherFrame();
			Dispatcher.CurrentDispatcher.BeginInvoke(priority, new DispatcherOperationCallback(DispatcherHelper.ExitFrame), dispatcherFrame);
			Dispatcher.PushFrame(dispatcherFrame);
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0002CB66 File Offset: 0x0002AD66
		private static object ExitFrame(object f)
		{
			((DispatcherFrame)f).Continue = false;
			return null;
		}
	}
}
