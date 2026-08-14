using System;
using System.Runtime.Versioning;
using Windows.Foundation;

namespace System.Threading.Tasks
{
	// Token: 0x0200000F RID: 15
	[SupportedOSPlatform("windows10.0.10240.0")]
	internal sealed class TaskToAsyncOperationWithProgressAdapter<TResult, TProgress> : TaskToAsyncInfoAdapter<AsyncOperationWithProgressCompletedHandler<TResult, TProgress>, AsyncOperationProgressHandler<TResult, TProgress>, TResult, TProgress>, IAsyncOperationWithProgress<TResult, TProgress>, IAsyncInfo
	{
		// Token: 0x06000060 RID: 96 RVA: 0x000031C7 File Offset: 0x000013C7
		internal TaskToAsyncOperationWithProgressAdapter(Delegate taskGenerator) : base(taskGenerator)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000031D0 File Offset: 0x000013D0
		internal TaskToAsyncOperationWithProgressAdapter(TResult synchronousResult) : base(synchronousResult)
		{
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000031D9 File Offset: 0x000013D9
		public TResult GetResults()
		{
			return base.GetResultsInternal();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000031E1 File Offset: 0x000013E1
		internal override void OnCompleted(AsyncOperationWithProgressCompletedHandler<TResult, TProgress> userCompletionHandler, AsyncStatus asyncStatus)
		{
			userCompletionHandler.Invoke(this, asyncStatus);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000031EB File Offset: 0x000013EB
		internal override void OnProgress(AsyncOperationProgressHandler<TResult, TProgress> userProgressHandler, TProgress progressInfo)
		{
			userProgressHandler.Invoke(this, progressInfo);
		}
	}
}
