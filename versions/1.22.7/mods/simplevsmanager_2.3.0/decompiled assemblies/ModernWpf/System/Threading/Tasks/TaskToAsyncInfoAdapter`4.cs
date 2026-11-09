using System;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;
using Windows.Foundation;
using WinRT;

namespace System.Threading.Tasks
{
	// Token: 0x0200000D RID: 13
	[SupportedOSPlatform("windows10.0.10240.0")]
	internal class TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo> : IAsyncInfo, IProgress<TProgressInfo> where TCompletedHandler : class where TProgressHandler : class
	{
		// Token: 0x06000030 RID: 48 RVA: 0x0000285F File Offset: 0x00000A5F
		private static InvalidOperationException CreateCannotGetResultsFromIncompleteOperationException(Exception cause)
		{
			InvalidOperationException ex = (cause == null) ? new InvalidOperationException(SR.InvalidOperation_CannotGetResultsFromIncompleteOperation) : new InvalidOperationException(SR.InvalidOperation_CannotGetResultsFromIncompleteOperation, cause);
			ex.SetHResult(-2147483634);
			return ex;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002888 File Offset: 0x00000A88
		internal TaskToAsyncInfoAdapter(Delegate taskProvider)
		{
			this._id = 2147483647U;
			base..ctor();
			this._startingContext = null;
			Task task = this.InvokeTaskProvider(taskProvider);
			if (task == null)
			{
				throw new NullReferenceException(SR.NullReference_TaskProviderReturnedNull);
			}
			if (task.Status == null)
			{
				throw new InvalidOperationException(SR.InvalidOperation_TaskProviderReturnedUnstartedTask);
			}
			this._dataContainer = task;
			this._state = 134217729;
			Task task2 = task;
			Action<Task, object> action;
			if ((action = TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9__25_0) == null)
			{
				action = (TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9__25_0 = new Action<Task, object>(TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9.<.ctor>b__25_0));
			}
			task2.ContinueWith(action, this, CancellationToken.None, 524288, TaskScheduler.Default);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002920 File Offset: 0x00000B20
		internal TaskToAsyncInfoAdapter(Task underlyingTask, CancellationTokenSource underlyingCancelTokenSource, Progress<TProgressInfo> underlyingProgressDispatcher)
		{
			this._id = 2147483647U;
			base..ctor();
			if (underlyingTask == null)
			{
				throw new ArgumentNullException("underlyingTask");
			}
			if (underlyingTask.Status == null)
			{
				throw new InvalidOperationException(SR.InvalidOperation_UnstartedTaskSpecified);
			}
			this._startingContext = null;
			this._dataContainer = underlyingTask;
			this._cancelTokenSource = underlyingCancelTokenSource;
			if (underlyingProgressDispatcher != null)
			{
				underlyingProgressDispatcher.ProgressChanged += new EventHandler<TProgressInfo>(this.OnReportChainedProgress);
			}
			this._state = 134217729;
			Action<Task, object> action;
			if ((action = TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9__26_0) == null)
			{
				action = (TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9__26_0 = new Action<Task, object>(TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9.<.ctor>b__26_0));
			}
			underlyingTask.ContinueWith(action, this, CancellationToken.None, 524288, TaskScheduler.Default);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000029CC File Offset: 0x00000BCC
		internal TaskToAsyncInfoAdapter(TResult synchronousResult)
		{
			this._id = 2147483647U;
			base..ctor();
			this._startingContext = null;
			this._dataContainer = synchronousResult;
			this._state = 939524098;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002A00 File Offset: 0x00000C00
		~TaskToAsyncInfoAdapter()
		{
			this.TransitionToClosed();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002A2C File Offset: 0x00000C2C
		internal bool DangerousSetCompleted(TResult synchronousResult)
		{
			if (!this.CompletedSynchronously)
			{
				return false;
			}
			this._dataContainer = synchronousResult;
			this._error = null;
			this._state = 939524098;
			return true;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A59 File Offset: 0x00000C59
		internal bool DangerousSetCanceled()
		{
			if (!this.CompletedSynchronously)
			{
				return false;
			}
			this._dataContainer = null;
			this._error = null;
			this._state = 939524104;
			return true;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A81 File Offset: 0x00000C81
		internal bool DangerousSetError(Exception error)
		{
			if (!this.CompletedSynchronously)
			{
				return false;
			}
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			this._dataContainer = null;
			this._error = error;
			this._state = 939524112;
			return true;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002AB7 File Offset: 0x00000CB7
		internal bool CompletedSynchronously
		{
			get
			{
				return (this._state & 536870912) != 0;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002ACA File Offset: 0x00000CCA
		private bool IsInStartedState
		{
			get
			{
				return (this._state & 1) != 0;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002AD9 File Offset: 0x00000CD9
		private bool IsInRunToCompletionState
		{
			get
			{
				return (this._state & 2) != 0;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002AE8 File Offset: 0x00000CE8
		private bool IsInErrorState
		{
			get
			{
				return (this._state & 16) != 0;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002AF8 File Offset: 0x00000CF8
		private bool IsInClosedState
		{
			get
			{
				return (this._state & 32) != 0;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002B08 File Offset: 0x00000D08
		private bool IsInRunningState
		{
			get
			{
				return (this._state & 5) != 0;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002B17 File Offset: 0x00000D17
		private bool IsInTerminalState
		{
			get
			{
				return (this._state & 26) != 0;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002B28 File Offset: 0x00000D28
		private bool CheckUniqueAsyncState(int state)
		{
			return (state & ~state + 1) == state;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002B40 File Offset: 0x00000D40
		private SynchronizationContext GetStartingContext()
		{
			return SynchronizationContext.Current;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002B47 File Offset: 0x00000D47
		internal Task Task
		{
			get
			{
				this.EnsureNotClosed();
				if (this.CompletedSynchronously)
				{
					return null;
				}
				return (Task)this._dataContainer;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002B64 File Offset: 0x00000D64
		internal CancellationTokenSource CancelTokenSource
		{
			get
			{
				return this._cancelTokenSource;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002B6C File Offset: 0x00000D6C
		internal void EnsureNotClosed()
		{
			if (!this.IsInClosedState)
			{
				return;
			}
			ObjectDisposedException ex = new ObjectDisposedException(SR.ObjectDisposed_AsyncInfoIsClosed);
			ex.SetHResult(-2147483634);
			throw ex;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002B8C File Offset: 0x00000D8C
		internal virtual void OnCompleted(TCompletedHandler userCompletionHandler, AsyncStatus asyncStatus)
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002B8E File Offset: 0x00000D8E
		internal virtual void OnProgress(TProgressHandler userProgressHandler, TProgressInfo progressInfo)
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B90 File Offset: 0x00000D90
		private void OnCompletedInvoker(AsyncStatus status)
		{
			TCompletedHandler tcompletedHandler = Volatile.Read<TCompletedHandler>(ref this._completedHandler);
			bool flag;
			if (tcompletedHandler == null)
			{
				this.SetState(268435456, -268435457, 0, false, ref flag);
				tcompletedHandler = Volatile.Read<TCompletedHandler>(ref this._completedHandler);
				if (tcompletedHandler == null)
				{
					return;
				}
			}
			this.SetState(0, -134217729, 134217728, true, ref flag);
			if (flag)
			{
				return;
			}
			this.OnCompleted(tcompletedHandler, status);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002BFC File Offset: 0x00000DFC
		private void OnProgressInvokerCrossContext(TProgressHandler handler, TProgressInfo progressInfo)
		{
			SynchronizationContext startingContext = this._startingContext;
			SendOrPostCallback sendOrPostCallback;
			if ((sendOrPostCallback = TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9__56_0) == null)
			{
				sendOrPostCallback = (TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9__56_0 = new SendOrPostCallback(TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9.<OnProgressInvokerCrossContext>b__56_0));
			}
			startingContext.Post(sendOrPostCallback, Tuple.Create<TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>, TProgressHandler, TProgressInfo>(this, handler, progressInfo));
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002C30 File Offset: 0x00000E30
		void IProgress<!3>.Report(TProgressInfo value)
		{
			TProgressHandler tprogressHandler = Volatile.Read<TProgressHandler>(ref this._progressHandler);
			if (tprogressHandler == null)
			{
				return;
			}
			if (this._startingContext == null)
			{
				this.OnProgress(tprogressHandler, value);
				return;
			}
			this.OnProgressInvokerCrossContext(tprogressHandler, value);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002C6B File Offset: 0x00000E6B
		private void OnReportChainedProgress(object sender, TProgressInfo progressInfo)
		{
			this.Report(progressInfo);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C74 File Offset: 0x00000E74
		private int SetAsyncState(int newAsyncState, int conditionBitMask, bool useCondition, out bool conditionFailed)
		{
			return this.SetState(newAsyncState, -64, conditionBitMask, useCondition, ref conditionFailed);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C84 File Offset: 0x00000E84
		private int SetState(int newStateSetMask, int newStateIgnoreMask, int conditionBitMask, bool useCondition, out bool conditionFailed)
		{
			int state = this._state;
			if (useCondition && (state & conditionBitMask) == 0)
			{
				conditionFailed = true;
				return state;
			}
			int num = (state & newStateIgnoreMask) | newStateSetMask;
			for (int num2 = Interlocked.CompareExchange(ref this._state, num, state); num2 != state; num2 = Interlocked.CompareExchange(ref this._state, num, state))
			{
				state = this._state;
				if (useCondition && (state & conditionBitMask) == 0)
				{
					conditionFailed = true;
					return state;
				}
				num = ((state & newStateIgnoreMask) | newStateSetMask);
			}
			conditionFailed = false;
			return num;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002CF4 File Offset: 0x00000EF4
		private int TransitionToTerminalState()
		{
			Task task = this._dataContainer as Task;
			int num = 16;
			switch (task.Status)
			{
			case 5:
				num = 2;
				break;
			case 6:
				num = 8;
				break;
			case 7:
				num = 16;
				break;
			}
			bool flag;
			return this.SetAsyncState(num, 63, true, ref flag);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002D44 File Offset: 0x00000F44
		private void TaskCompleted()
		{
			int num = this.TransitionToTerminalState();
			AsyncStatus status = this.GetStatus(num);
			try
			{
				if (this._startingContext == null)
				{
					this.OnCompletedInvoker(status);
				}
				else
				{
					SynchronizationContext startingContext = this._startingContext;
					SendOrPostCallback sendOrPostCallback;
					if ((sendOrPostCallback = TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9__62_0) == null)
					{
						sendOrPostCallback = (TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9__62_0 = new SendOrPostCallback(TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.<>c.<>9.<TaskCompleted>b__62_0));
					}
					startingContext.Post(sendOrPostCallback, Tuple.Create<TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>, AsyncStatus>(this, status));
				}
			}
			catch (Exception exception)
			{
				ExceptionDispatchHelper.ThrowAsync(exception, this._startingContext);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002DC4 File Offset: 0x00000FC4
		private AsyncStatus GetStatus(int state)
		{
			int num = state & 63;
			switch (num)
			{
			case 0:
				return AsyncStatus.Error;
			case 1:
				return AsyncStatus.Started;
			case 2:
				return AsyncStatus.Completed;
			case 3:
			case 5:
			case 6:
			case 7:
				break;
			case 4:
			case 8:
				return AsyncStatus.Canceled;
			default:
				if (num == 16)
				{
					return AsyncStatus.Error;
				}
				if (num == 32)
				{
					return AsyncStatus.Error;
				}
				break;
			}
			return AsyncStatus.Error;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002E1C File Offset: 0x0000101C
		internal TResult GetResultsInternal()
		{
			this.EnsureNotClosed();
			if (this.IsInErrorState)
			{
				ExceptionDispatchInfo.Capture(this.ErrorCode).Throw();
			}
			if (!this.IsInRunToCompletionState)
			{
				throw TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.CreateCannotGetResultsFromIncompleteOperationException(null);
			}
			if (this.CompletedSynchronously)
			{
				return (TResult)((object)this._dataContainer);
			}
			Task<TResult> task = this._dataContainer as Task<TResult>;
			TResult result;
			if (task == null)
			{
				result = default(TResult);
				return result;
			}
			try
			{
				result = task.GetAwaiter().GetResult();
			}
			catch (TaskCanceledException ex)
			{
				throw TaskToAsyncInfoAdapter<TCompletedHandler, TProgressHandler, TResult, TProgressInfo>.CreateCannotGetResultsFromIncompleteOperationException(ex);
			}
			return result;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002EAC File Offset: 0x000010AC
		private Task InvokeTaskProvider(Delegate taskProvider)
		{
			Func<Task> func = taskProvider as Func<Task>;
			if (func != null)
			{
				return func.Invoke();
			}
			Func<CancellationToken, Task> func2 = taskProvider as Func<CancellationToken, Task>;
			if (func2 != null)
			{
				this._cancelTokenSource = new CancellationTokenSource();
				return func2.Invoke(this._cancelTokenSource.Token);
			}
			Func<IProgress<TProgressInfo>, Task> func3 = taskProvider as Func<IProgress<TProgressInfo>, Task>;
			if (func3 != null)
			{
				return func3.Invoke(this);
			}
			Func<CancellationToken, IProgress<TProgressInfo>, Task> func4 = taskProvider as Func<CancellationToken, IProgress<TProgressInfo>, Task>;
			if (func4 != null)
			{
				this._cancelTokenSource = new CancellationTokenSource();
				return func4.Invoke(this._cancelTokenSource.Token, this);
			}
			return null;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002F2C File Offset: 0x0000112C
		private void TransitionToClosed()
		{
			bool flag;
			this.SetAsyncState(32, 0, false, ref flag);
			this._cancelTokenSource = null;
			this._dataContainer = null;
			this._error = null;
			this._completedHandler = default(TCompletedHandler);
			this._progressHandler = default(TProgressHandler);
			this._startingContext = null;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002F7A File Offset: 0x0000117A
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00002F90 File Offset: 0x00001190
		public virtual TCompletedHandler Completed
		{
			get
			{
				TCompletedHandler result = Volatile.Read<TCompletedHandler>(ref this._completedHandler);
				this.EnsureNotClosed();
				return result;
			}
			set
			{
				this.EnsureNotClosed();
				if (Interlocked.CompareExchange<TCompletedHandler>(ref this._completedHandler, value, default(TCompletedHandler)) != null)
				{
					InvalidOperationException ex = new InvalidOperationException(SR.InvalidOperation_CannotSetCompletionHanlderMoreThanOnce);
					ex.SetHResult(-2147483624);
					throw ex;
				}
				if (value == null)
				{
					return;
				}
				if ((this._state & 268435456) == 0)
				{
					return;
				}
				if (this.IsInClosedState)
				{
					Interlocked.Exchange<TCompletedHandler>(ref this._completedHandler, default(TCompletedHandler));
					return;
				}
				this.OnCompletedInvoker(this.Status);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003019 File Offset: 0x00001219
		// (set) Token: 0x06000055 RID: 85 RVA: 0x0000302C File Offset: 0x0000122C
		public virtual TProgressHandler Progress
		{
			get
			{
				TProgressHandler result = Volatile.Read<TProgressHandler>(ref this._progressHandler);
				this.EnsureNotClosed();
				return result;
			}
			set
			{
				this.EnsureNotClosed();
				Interlocked.Exchange<TProgressHandler>(ref this._progressHandler, value);
				if (this.IsInClosedState)
				{
					Interlocked.Exchange<TProgressHandler>(ref this._progressHandler, default(TProgressHandler));
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000306C File Offset: 0x0000126C
		public virtual void Cancel()
		{
			bool flag;
			this.SetAsyncState(4, 1, true, ref flag);
			if (!flag && this._cancelTokenSource != null)
			{
				this._cancelTokenSource.Cancel();
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000309B File Offset: 0x0000129B
		public virtual void Close()
		{
			if (this.IsInClosedState)
			{
				return;
			}
			if (!this.IsInTerminalState && (this._state & 63) != 0)
			{
				InvalidOperationException ex = new InvalidOperationException(SR.InvalidOperation_IllegalStateChange);
				ex.SetHResult(-2147483635);
				throw ex;
			}
			this.TransitionToClosed();
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000030D8 File Offset: 0x000012D8
		public virtual Exception ErrorCode
		{
			get
			{
				this.EnsureNotClosed();
				if (!this.IsInErrorState)
				{
					return null;
				}
				Exception ex = Volatile.Read<Exception>(ref this._error);
				if (ex != null || this.CompletedSynchronously)
				{
					return ex;
				}
				AggregateException exception = (this._dataContainer as Task).Exception;
				if (exception == null)
				{
					ex = new Exception(SR.WinRtCOM_Error);
					ex.SetHResult(-2147467259);
				}
				else
				{
					Exception innerException = exception.InnerException;
					ex = ((innerException == null) ? exception : innerException);
				}
				return Interlocked.CompareExchange<Exception>(ref this._error, ex, null) ?? ex;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000059 RID: 89 RVA: 0x0000315B File Offset: 0x0000135B
		public virtual uint Id
		{
			get
			{
				this.EnsureNotClosed();
				if (this._id != 2147483647U)
				{
					return this._id;
				}
				return AsyncInfoIdGenerator.EnsureInitializedThreadsafe(ref this._id);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003182 File Offset: 0x00001382
		public virtual AsyncStatus Status
		{
			get
			{
				this.EnsureNotClosed();
				return this.GetStatus(this._state);
			}
		}

		// Token: 0x04000008 RID: 8
		private const int E_ILLEGAL_STATE_CHANGE = -2147483635;

		// Token: 0x04000009 RID: 9
		private const int E_ILLEGAL_METHOD_CALL = -2147483634;

		// Token: 0x0400000A RID: 10
		private const int E_ILLEGAL_DELEGATE_ASSIGNMENT = -2147483624;

		// Token: 0x0400000B RID: 11
		private const int E_FAIL = -2147467259;

		// Token: 0x0400000C RID: 12
		private const int STATE_NOT_INITIALIZED = 0;

		// Token: 0x0400000D RID: 13
		private const int STATE_STARTED = 1;

		// Token: 0x0400000E RID: 14
		private const int STATE_RUN_TO_COMPLETION = 2;

		// Token: 0x0400000F RID: 15
		private const int STATE_CANCELLATION_REQUESTED = 4;

		// Token: 0x04000010 RID: 16
		private const int STATE_CANCELLATION_COMPLETED = 8;

		// Token: 0x04000011 RID: 17
		private const int STATE_ERROR = 16;

		// Token: 0x04000012 RID: 18
		private const int STATE_CLOSED = 32;

		// Token: 0x04000013 RID: 19
		private const int STATEFLAG_COMPLETED_SYNCHRONOUSLY = 536870912;

		// Token: 0x04000014 RID: 20
		private const int STATEFLAG_MUST_RUN_COMPLETION_HNDL_WHEN_SET = 268435456;

		// Token: 0x04000015 RID: 21
		private const int STATEFLAG_COMPLETION_HNDL_NOT_YET_INVOKED = 134217728;

		// Token: 0x04000016 RID: 22
		private const int STATEMASK_SELECT_ANY_ASYNC_STATE = 63;

		// Token: 0x04000017 RID: 23
		private const int STATEMASK_CLEAR_ALL_ASYNC_STATES = -64;

		// Token: 0x04000018 RID: 24
		private CancellationTokenSource _cancelTokenSource;

		// Token: 0x04000019 RID: 25
		private uint _id;

		// Token: 0x0400001A RID: 26
		private Exception _error;

		// Token: 0x0400001B RID: 27
		private volatile int _state;

		// Token: 0x0400001C RID: 28
		private object _dataContainer;

		// Token: 0x0400001D RID: 29
		private TCompletedHandler _completedHandler;

		// Token: 0x0400001E RID: 30
		private TProgressHandler _progressHandler;

		// Token: 0x0400001F RID: 31
		private SynchronizationContext _startingContext;
	}
}
