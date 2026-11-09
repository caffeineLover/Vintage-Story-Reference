using System;
using System.Runtime.ExceptionServices;

namespace System.Threading.Tasks
{
	// Token: 0x0200000A RID: 10
	internal static class ExceptionDispatchHelper
	{
		// Token: 0x06000025 RID: 37 RVA: 0x0000271C File Offset: 0x0000091C
		internal static void ThrowAsync(Exception exception, SynchronizationContext targetContext)
		{
			if (exception == null)
			{
				return;
			}
			ExceptionDispatchInfo exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
			if (targetContext != null)
			{
				try
				{
					targetContext.Post(delegate(object edi)
					{
						((ExceptionDispatchInfo)edi).Throw();
					}, exceptionDispatchInfo);
				}
				catch
				{
					ExceptionDispatchHelper.ThrowAsync(exception, null);
				}
				return;
			}
			bool flag = true;
			try
			{
				new SynchronizationContext().Post(delegate(object edi)
				{
					((ExceptionDispatchInfo)edi).Throw();
				}, exceptionDispatchInfo);
			}
			catch
			{
				flag = false;
			}
			if (!flag)
			{
				exceptionDispatchInfo.Throw();
			}
		}
	}
}
