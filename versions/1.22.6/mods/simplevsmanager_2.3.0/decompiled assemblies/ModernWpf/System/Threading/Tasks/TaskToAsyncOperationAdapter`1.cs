using System;
using System.Runtime.Versioning;
using Windows.Foundation;

namespace System.Threading.Tasks
{
	// Token: 0x0200000E RID: 14
	[SupportedOSPlatform("windows10.0.10240.0")]
	internal sealed class TaskToAsyncOperationAdapter<TResult> : TaskToAsyncInfoAdapter<AsyncOperationCompletedHandler<TResult>, VoidReferenceTypeParameter, TResult, VoidValueTypeParameter>, IAsyncOperation<TResult>, IAsyncInfo
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00003198 File Offset: 0x00001398
		internal TaskToAsyncOperationAdapter(Delegate taskGenerator) : base(taskGenerator)
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000031A1 File Offset: 0x000013A1
		internal TaskToAsyncOperationAdapter(Task underlyingTask, CancellationTokenSource underlyingCancelTokenSource) : base(underlyingTask, underlyingCancelTokenSource, null)
		{
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000031AC File Offset: 0x000013AC
		internal TaskToAsyncOperationAdapter(TResult synchronousResult) : base(synchronousResult)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000031B5 File Offset: 0x000013B5
		public TResult GetResults()
		{
			return base.GetResultsInternal();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000031BD File Offset: 0x000013BD
		internal override void OnCompleted(AsyncOperationCompletedHandler<TResult> userCompletionHandler, AsyncStatus asyncStatus)
		{
			userCompletionHandler.Invoke(this, asyncStatus);
		}
	}
}
