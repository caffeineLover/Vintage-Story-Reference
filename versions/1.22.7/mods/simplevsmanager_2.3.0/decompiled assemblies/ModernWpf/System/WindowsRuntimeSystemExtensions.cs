using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace System
{
	// Token: 0x02000005 RID: 5
	[SupportedOSPlatform("windows10.0.10240.0")]
	internal static class WindowsRuntimeSystemExtensions
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002098 File Offset: 0x00000298
		public static Task AsTask(this IAsyncAction source, CancellationToken cancellationToken)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			switch (source.Status)
			{
			case AsyncStatus.Completed:
				return Task.CompletedTask;
			case AsyncStatus.Canceled:
				return Task.FromCanceled(cancellationToken.IsCancellationRequested ? cancellationToken : new CancellationToken(true));
			case AsyncStatus.Error:
				return Task.FromException(source.ErrorCode);
			default:
			{
				AsyncInfoToTaskBridge<VoidValueTypeParameter, VoidValueTypeParameter> asyncInfoToTaskBridge = new AsyncInfoToTaskBridge<VoidValueTypeParameter, VoidValueTypeParameter>(cancellationToken);
				source.Completed = new AsyncActionCompletedHandler(asyncInfoToTaskBridge.CompleteFromAsyncAction);
				asyncInfoToTaskBridge.RegisterForCancellation(source);
				return asyncInfoToTaskBridge.Task;
			}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002120 File Offset: 0x00000320
		public static Task AsTask(this IAsyncAction source)
		{
			return source.AsTask(CancellationToken.None);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000212D File Offset: 0x0000032D
		public static TaskAwaiter GetAwaiter(this IAsyncAction source)
		{
			return source.AsTask().GetAwaiter();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000213C File Offset: 0x0000033C
		public static Task<TResult> AsTask<TResult>(this IAsyncOperation<TResult> source, CancellationToken cancellationToken)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			switch (source.Status)
			{
			case AsyncStatus.Completed:
				return Task.FromResult<TResult>(source.GetResults());
			case AsyncStatus.Canceled:
				return Task.FromCanceled<TResult>(cancellationToken.IsCancellationRequested ? cancellationToken : new CancellationToken(true));
			case AsyncStatus.Error:
				return Task.FromException<TResult>(source.ErrorCode);
			default:
			{
				AsyncInfoToTaskBridge<TResult, VoidValueTypeParameter> asyncInfoToTaskBridge = new AsyncInfoToTaskBridge<TResult, VoidValueTypeParameter>(cancellationToken);
				source.Completed = new AsyncOperationCompletedHandler<TResult>(asyncInfoToTaskBridge.CompleteFromAsyncOperation);
				asyncInfoToTaskBridge.RegisterForCancellation(source);
				return asyncInfoToTaskBridge.Task;
			}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021CA File Offset: 0x000003CA
		public static Task<TResult> AsTask<TResult>(this IAsyncOperation<TResult> source)
		{
			return source.AsTask(CancellationToken.None);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021D7 File Offset: 0x000003D7
		public static TaskAwaiter<TResult> GetAwaiter<TResult>(this IAsyncOperation<TResult> source)
		{
			return source.AsTask<TResult>().GetAwaiter();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021E4 File Offset: 0x000003E4
		public static Task AsTask<TProgress>(this IAsyncActionWithProgress<TProgress> source, CancellationToken cancellationToken, IProgress<TProgress> progress)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			switch (source.Status)
			{
			case AsyncStatus.Completed:
				return Task.CompletedTask;
			case AsyncStatus.Canceled:
				return Task.FromCanceled(cancellationToken.IsCancellationRequested ? cancellationToken : new CancellationToken(true));
			case AsyncStatus.Error:
				return Task.FromException(source.ErrorCode);
			default:
			{
				if (progress != null)
				{
					WindowsRuntimeSystemExtensions.SetProgress<TProgress>(source, progress);
				}
				AsyncInfoToTaskBridge<VoidValueTypeParameter, TProgress> asyncInfoToTaskBridge = new AsyncInfoToTaskBridge<VoidValueTypeParameter, TProgress>(cancellationToken);
				source.Completed = new AsyncActionWithProgressCompletedHandler<TProgress>(asyncInfoToTaskBridge.CompleteFromAsyncActionWithProgress);
				asyncInfoToTaskBridge.RegisterForCancellation(source);
				return asyncInfoToTaskBridge.Task;
			}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002278 File Offset: 0x00000478
		private static void SetProgress<TProgress>(IAsyncActionWithProgress<TProgress> source, IProgress<TProgress> sink)
		{
			WindowsRuntimeSystemExtensions.<>c__DisplayClass7_0<TProgress> CS$<>8__locals1 = new WindowsRuntimeSystemExtensions.<>c__DisplayClass7_0<TProgress>();
			CS$<>8__locals1.sink = sink;
			source.Progress = new AsyncActionProgressHandler<TProgress>(CS$<>8__locals1.<SetProgress>b__0);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022A4 File Offset: 0x000004A4
		public static Task AsTask<TProgress>(this IAsyncActionWithProgress<TProgress> source)
		{
			return source.AsTask(CancellationToken.None, null);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022B2 File Offset: 0x000004B2
		public static Task AsTask<TProgress>(this IAsyncActionWithProgress<TProgress> source, CancellationToken cancellationToken)
		{
			return source.AsTask(cancellationToken, null);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022BC File Offset: 0x000004BC
		public static Task AsTask<TProgress>(this IAsyncActionWithProgress<TProgress> source, IProgress<TProgress> progress)
		{
			return source.AsTask(CancellationToken.None, progress);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022CA File Offset: 0x000004CA
		public static TaskAwaiter GetAwaiter<TProgress>(this IAsyncActionWithProgress<TProgress> source)
		{
			return source.AsTask<TProgress>().GetAwaiter();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022D8 File Offset: 0x000004D8
		public static Task<TResult> AsTask<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source, CancellationToken cancellationToken, IProgress<TProgress> progress)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			switch (source.Status)
			{
			case AsyncStatus.Completed:
				return Task.FromResult<TResult>(source.GetResults());
			case AsyncStatus.Canceled:
				return Task.FromCanceled<TResult>(cancellationToken.IsCancellationRequested ? cancellationToken : new CancellationToken(true));
			case AsyncStatus.Error:
				return Task.FromException<TResult>(source.ErrorCode);
			default:
			{
				if (progress != null)
				{
					WindowsRuntimeSystemExtensions.SetProgress<TResult, TProgress>(source, progress);
				}
				AsyncInfoToTaskBridge<TResult, TProgress> asyncInfoToTaskBridge = new AsyncInfoToTaskBridge<TResult, TProgress>(cancellationToken);
				source.Completed = new AsyncOperationWithProgressCompletedHandler<TResult, TProgress>(asyncInfoToTaskBridge.CompleteFromAsyncOperationWithProgress);
				asyncInfoToTaskBridge.RegisterForCancellation(source);
				return asyncInfoToTaskBridge.Task;
			}
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002370 File Offset: 0x00000570
		private static void SetProgress<TResult, TProgress>(IAsyncOperationWithProgress<TResult, TProgress> source, IProgress<TProgress> sink)
		{
			WindowsRuntimeSystemExtensions.<>c__DisplayClass13_0<TResult, TProgress> CS$<>8__locals1 = new WindowsRuntimeSystemExtensions.<>c__DisplayClass13_0<TResult, TProgress>();
			CS$<>8__locals1.sink = sink;
			source.Progress = new AsyncOperationProgressHandler<TResult, TProgress>(CS$<>8__locals1.<SetProgress>b__0);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000239C File Offset: 0x0000059C
		public static Task<TResult> AsTask<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source)
		{
			return source.AsTask(CancellationToken.None, null);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023AA File Offset: 0x000005AA
		public static Task<TResult> AsTask<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source, CancellationToken cancellationToken)
		{
			return source.AsTask(cancellationToken, null);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023B4 File Offset: 0x000005B4
		public static Task<TResult> AsTask<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source, IProgress<TProgress> progress)
		{
			return source.AsTask(CancellationToken.None, progress);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023C2 File Offset: 0x000005C2
		public static TaskAwaiter<TResult> GetAwaiter<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source)
		{
			return source.AsTask<TResult, TProgress>().GetAwaiter();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000023CF File Offset: 0x000005CF
		public static IAsyncAction AsAsyncAction(this Task source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new TaskToAsyncActionAdapter(source, null);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023E6 File Offset: 0x000005E6
		public static IAsyncOperation<TResult> AsAsyncOperation<TResult>(this Task<TResult> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new TaskToAsyncOperationAdapter<TResult>(source, null);
		}
	}
}
