using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace System
{
	// Token: 0x02000008 RID: 8
	[SupportedOSPlatform("windows10.0.10240.0")]
	internal sealed class AsyncInfoToTaskBridge<TResult, TProgress> : TaskCompletionSource<TResult>
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002405 File Offset: 0x00000605
		internal AsyncInfoToTaskBridge(CancellationToken cancellationToken)
		{
			this._ct = cancellationToken;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002414 File Offset: 0x00000614
		internal void RegisterForCancellation(IAsyncInfo asyncInfo)
		{
			try
			{
				if (this._ct.CanBeCanceled && !this._completing)
				{
					Action<object> action;
					if ((action = AsyncInfoToTaskBridge<TResult, TProgress>.<>c.<>9__4_0) == null)
					{
						action = (AsyncInfoToTaskBridge<TResult, TProgress>.<>c.<>9__4_0 = new Action<object>(AsyncInfoToTaskBridge<TResult, TProgress>.<>c.<>9.<RegisterForCancellation>b__4_0));
					}
					CancellationTokenRegistration ctr = this._ct.Register(action, asyncInfo);
					bool flag = false;
					lock (this)
					{
						if (this._completing)
						{
							flag = true;
						}
						else
						{
							this._ctr = ctr;
						}
					}
					if (flag)
					{
						ctr.Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				if (!base.Task.IsFaulted)
				{
					base.TrySetException(ex);
				}
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000024D4 File Offset: 0x000006D4
		internal void CompleteFromAsyncAction(IAsyncAction asyncInfo, AsyncStatus asyncStatus)
		{
			this.Complete(asyncInfo, null, asyncStatus);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000024DF File Offset: 0x000006DF
		internal void CompleteFromAsyncActionWithProgress(IAsyncActionWithProgress<TProgress> asyncInfo, AsyncStatus asyncStatus)
		{
			this.Complete(asyncInfo, null, asyncStatus);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024EA File Offset: 0x000006EA
		internal void CompleteFromAsyncOperation(IAsyncOperation<TResult> asyncInfo, AsyncStatus asyncStatus)
		{
			Func<IAsyncInfo, TResult> func;
			if ((func = AsyncInfoToTaskBridge<TResult, TProgress>.<>c.<>9__7_0) == null)
			{
				func = (AsyncInfoToTaskBridge<TResult, TProgress>.<>c.<>9__7_0 = new Func<IAsyncInfo, TResult>(AsyncInfoToTaskBridge<TResult, TProgress>.<>c.<>9.<CompleteFromAsyncOperation>b__7_0));
			}
			this.Complete(asyncInfo, func, asyncStatus);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002513 File Offset: 0x00000713
		internal void CompleteFromAsyncOperationWithProgress(IAsyncOperationWithProgress<TResult, TProgress> asyncInfo, AsyncStatus asyncStatus)
		{
			Func<IAsyncInfo, TResult> func;
			if ((func = AsyncInfoToTaskBridge<TResult, TProgress>.<>c.<>9__8_0) == null)
			{
				func = (AsyncInfoToTaskBridge<TResult, TProgress>.<>c.<>9__8_0 = new Func<IAsyncInfo, TResult>(AsyncInfoToTaskBridge<TResult, TProgress>.<>c.<>9.<CompleteFromAsyncOperationWithProgress>b__8_0));
			}
			this.Complete(asyncInfo, func, asyncStatus);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000253C File Offset: 0x0000073C
		private void Complete(IAsyncInfo asyncInfo, Func<IAsyncInfo, TResult> getResultsFunction, AsyncStatus asyncStatus)
		{
			if (asyncInfo == null)
			{
				throw new ArgumentNullException("asyncInfo");
			}
			try
			{
				if (base.Task.IsCompleted)
				{
					throw new InvalidOperationException("The asynchronous operation could not be completed.");
				}
				CancellationTokenRegistration ctr;
				lock (this)
				{
					this._completing = true;
					ctr = this._ctr;
					this._ctr = default(CancellationTokenRegistration);
				}
				ctr.Dispose();
				if (asyncStatus != AsyncStatus.Completed && asyncStatus != AsyncStatus.Canceled && asyncStatus != AsyncStatus.Error)
				{
					throw new InvalidOperationException("The asynchronous operation could not be completed.");
				}
				TResult tresult = default(TResult);
				Exception ex = null;
				if (asyncStatus == AsyncStatus.Error)
				{
					ex = asyncInfo.ErrorCode;
					if (ex == null)
					{
						ex = new InvalidOperationException("The asynchronous operation could not be completed.");
					}
				}
				else if (asyncStatus == AsyncStatus.Completed && getResultsFunction != null)
				{
					try
					{
						tresult = getResultsFunction.Invoke(asyncInfo);
					}
					catch (Exception ex)
					{
						asyncStatus = AsyncStatus.Error;
					}
				}
				switch (asyncStatus)
				{
				case AsyncStatus.Completed:
					base.TrySetResult(tresult);
					break;
				case AsyncStatus.Canceled:
					base.TrySetCanceled(this._ct.IsCancellationRequested ? this._ct : new CancellationToken(true));
					break;
				case AsyncStatus.Error:
					base.TrySetException(ex);
					break;
				}
			}
			catch (Exception ex2)
			{
				if (!base.TrySetException(ex2))
				{
					throw;
				}
			}
		}

		// Token: 0x04000003 RID: 3
		private readonly CancellationToken _ct;

		// Token: 0x04000004 RID: 4
		private CancellationTokenRegistration _ctr;

		// Token: 0x04000005 RID: 5
		private bool _completing;
	}
}
