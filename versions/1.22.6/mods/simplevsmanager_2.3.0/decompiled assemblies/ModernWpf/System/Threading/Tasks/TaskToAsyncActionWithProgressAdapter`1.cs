using System;
using System.Runtime.Versioning;
using Windows.Foundation;

namespace System.Threading.Tasks
{
	// Token: 0x0200000C RID: 12
	[SupportedOSPlatform("windows10.0.10240.0")]
	internal sealed class TaskToAsyncActionWithProgressAdapter<TProgress> : TaskToAsyncInfoAdapter<AsyncActionWithProgressCompletedHandler<TProgress>, AsyncActionProgressHandler<TProgress>, VoidValueTypeParameter, TProgress>, IAsyncActionWithProgress<TProgress>, IAsyncInfo
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002811 File Offset: 0x00000A11
		internal TaskToAsyncActionWithProgressAdapter(Delegate taskGenerator) : base(taskGenerator)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000281C File Offset: 0x00000A1C
		internal TaskToAsyncActionWithProgressAdapter(bool isCanceled) : base(default(VoidValueTypeParameter))
		{
			if (isCanceled)
			{
				base.DangerousSetCanceled();
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002842 File Offset: 0x00000A42
		public void GetResults()
		{
			base.GetResultsInternal();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000284B File Offset: 0x00000A4B
		internal override void OnCompleted(AsyncActionWithProgressCompletedHandler<TProgress> userCompletionHandler, AsyncStatus asyncStatus)
		{
			userCompletionHandler.Invoke(this, asyncStatus);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002855 File Offset: 0x00000A55
		internal override void OnProgress(AsyncActionProgressHandler<TProgress> userProgressHandler, TProgress progressInfo)
		{
			userProgressHandler.Invoke(this, progressInfo);
		}
	}
}
