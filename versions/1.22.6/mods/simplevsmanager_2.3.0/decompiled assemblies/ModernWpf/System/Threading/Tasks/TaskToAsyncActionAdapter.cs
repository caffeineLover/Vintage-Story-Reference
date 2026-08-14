using System;
using System.Runtime.Versioning;
using Windows.Foundation;

namespace System.Threading.Tasks
{
	// Token: 0x0200000B RID: 11
	[SupportedOSPlatform("windows10.0.10240.0")]
	internal sealed class TaskToAsyncActionAdapter : TaskToAsyncInfoAdapter<AsyncActionCompletedHandler, VoidReferenceTypeParameter, VoidValueTypeParameter, VoidValueTypeParameter>, IAsyncAction, IAsyncInfo
	{
		// Token: 0x06000026 RID: 38 RVA: 0x000027C4 File Offset: 0x000009C4
		internal TaskToAsyncActionAdapter(Delegate taskGenerator) : base(taskGenerator)
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000027CD File Offset: 0x000009CD
		internal TaskToAsyncActionAdapter(Task underlyingTask, CancellationTokenSource underlyingCancelTokenSource) : base(underlyingTask, underlyingCancelTokenSource, null)
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000027D8 File Offset: 0x000009D8
		internal TaskToAsyncActionAdapter(bool isCanceled) : base(default(VoidValueTypeParameter))
		{
			if (isCanceled)
			{
				base.DangerousSetCanceled();
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000027FE File Offset: 0x000009FE
		public void GetResults()
		{
			base.GetResultsInternal();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002807 File Offset: 0x00000A07
		internal override void OnCompleted(AsyncActionCompletedHandler userCompletionHandler, AsyncStatus asyncStatus)
		{
			userCompletionHandler(this, asyncStatus);
		}
	}
}
