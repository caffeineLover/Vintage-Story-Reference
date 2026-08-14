using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x02000011 RID: 17
	[SupportedOSPlatform("windows10.0.10240.0")]
	internal static class AsyncInfo
	{
		// Token: 0x06000066 RID: 102 RVA: 0x000031FD File Offset: 0x000013FD
		public static IAsyncAction Run(Func<CancellationToken, Task> taskProvider)
		{
			if (taskProvider == null)
			{
				throw new ArgumentNullException("taskProvider");
			}
			return new TaskToAsyncActionAdapter(taskProvider);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003213 File Offset: 0x00001413
		public static IAsyncActionWithProgress<TProgress> Run<TProgress>(Func<CancellationToken, IProgress<TProgress>, Task> taskProvider)
		{
			if (taskProvider == null)
			{
				throw new ArgumentNullException("taskProvider");
			}
			return new TaskToAsyncActionWithProgressAdapter<TProgress>(taskProvider);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003229 File Offset: 0x00001429
		public static IAsyncOperation<TResult> Run<TResult>(Func<CancellationToken, Task<TResult>> taskProvider)
		{
			if (taskProvider == null)
			{
				throw new ArgumentNullException("taskProvider");
			}
			return new TaskToAsyncOperationAdapter<TResult>(taskProvider);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000323F File Offset: 0x0000143F
		public static IAsyncOperationWithProgress<TResult, TProgress> Run<TResult, TProgress>(Func<CancellationToken, IProgress<TProgress>, Task<TResult>> taskProvider)
		{
			if (taskProvider == null)
			{
				throw new ArgumentNullException("taskProvider");
			}
			return new TaskToAsyncOperationWithProgressAdapter<TResult, TProgress>(taskProvider);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003255 File Offset: 0x00001455
		internal static IAsyncAction CreateCompletedAction()
		{
			return new TaskToAsyncActionAdapter(false);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000325D File Offset: 0x0000145D
		internal static IAsyncActionWithProgress<TProgress> CreateCompletedAction<TProgress>()
		{
			return new TaskToAsyncActionWithProgressAdapter<TProgress>(false);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003265 File Offset: 0x00001465
		internal static IAsyncOperation<TResult> CreateCompletedOperation<TResult>(TResult synchronousResult)
		{
			return new TaskToAsyncOperationAdapter<TResult>(synchronousResult);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000326D File Offset: 0x0000146D
		internal static IAsyncOperationWithProgress<TResult, TProgress> CreateCompletedOperation<TResult, TProgress>(TResult synchronousResult)
		{
			return new TaskToAsyncOperationWithProgressAdapter<TResult, TProgress>(synchronousResult);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003275 File Offset: 0x00001475
		internal static IAsyncAction CreateFaultedAction(Exception error)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			TaskToAsyncActionAdapter taskToAsyncActionAdapter = new TaskToAsyncActionAdapter(false);
			taskToAsyncActionAdapter.DangerousSetError(error);
			return taskToAsyncActionAdapter;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003293 File Offset: 0x00001493
		internal static IAsyncActionWithProgress<TProgress> CreateFaultedAction<TProgress>(Exception error)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			TaskToAsyncActionWithProgressAdapter<TProgress> taskToAsyncActionWithProgressAdapter = new TaskToAsyncActionWithProgressAdapter<TProgress>(false);
			taskToAsyncActionWithProgressAdapter.DangerousSetError(error);
			return taskToAsyncActionWithProgressAdapter;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000032B4 File Offset: 0x000014B4
		internal static IAsyncOperation<TResult> CreateFaultedOperation<TResult>(Exception error)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			TaskToAsyncOperationAdapter<TResult> taskToAsyncOperationAdapter = new TaskToAsyncOperationAdapter<TResult>(default(TResult));
			taskToAsyncOperationAdapter.DangerousSetError(error);
			return taskToAsyncOperationAdapter;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000032E8 File Offset: 0x000014E8
		internal static IAsyncOperationWithProgress<TResult, TProgress> CreateFaultedOperation<TResult, TProgress>(Exception error)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			TaskToAsyncOperationWithProgressAdapter<TResult, TProgress> taskToAsyncOperationWithProgressAdapter = new TaskToAsyncOperationWithProgressAdapter<TResult, TProgress>(default(TResult));
			taskToAsyncOperationWithProgressAdapter.DangerousSetError(error);
			return taskToAsyncOperationWithProgressAdapter;
		}
	}
}
