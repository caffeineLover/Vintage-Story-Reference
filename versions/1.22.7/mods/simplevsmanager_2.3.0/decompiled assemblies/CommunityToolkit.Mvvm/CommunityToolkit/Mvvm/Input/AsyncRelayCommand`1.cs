using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input.Internals;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x02000020 RID: 32
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class AsyncRelayCommand<T> : IAsyncRelayCommand<T>, IAsyncRelayCommand, IRelayCommand, ICommand, INotifyPropertyChanged, IRelayCommand<T>, ICancellationAwareCommand
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000C5 RID: 197 RVA: 0x00004C5C File Offset: 0x00002E5C
		// (remove) Token: 0x060000C6 RID: 198 RVA: 0x00004C94 File Offset: 0x00002E94
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000C7 RID: 199 RVA: 0x00004CCC File Offset: 0x00002ECC
		// (remove) Token: 0x060000C8 RID: 200 RVA: 0x00004D04 File Offset: 0x00002F04
		public event EventHandler CanExecuteChanged;

		// Token: 0x060000C9 RID: 201 RVA: 0x00004D39 File Offset: 0x00002F39
		public AsyncRelayCommand([Nullable(new byte[]
		{
			1,
			2,
			1
		})] Func<T, Task> execute)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			this.execute = execute;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004D53 File Offset: 0x00002F53
		public AsyncRelayCommand([Nullable(new byte[]
		{
			1,
			2,
			1
		})] Func<T, Task> execute, AsyncRelayCommandOptions options)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			this.execute = execute;
			this.options = options;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004D74 File Offset: 0x00002F74
		public AsyncRelayCommand([Nullable(new byte[]
		{
			1,
			2,
			1
		})] Func<T, CancellationToken, Task> cancelableExecute)
		{
			ArgumentNullException.ThrowIfNull(cancelableExecute, "cancelableExecute");
			this.cancelableExecute = cancelableExecute;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004D8E File Offset: 0x00002F8E
		public AsyncRelayCommand([Nullable(new byte[]
		{
			1,
			2,
			1
		})] Func<T, CancellationToken, Task> cancelableExecute, AsyncRelayCommandOptions options)
		{
			ArgumentNullException.ThrowIfNull(cancelableExecute, "cancelableExecute");
			this.cancelableExecute = cancelableExecute;
			this.options = options;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004DAF File Offset: 0x00002FAF
		public AsyncRelayCommand([Nullable(new byte[]
		{
			1,
			2,
			1
		})] Func<T, Task> execute, [Nullable(new byte[]
		{
			1,
			2
		})] Predicate<T> canExecute)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			ArgumentNullException.ThrowIfNull(canExecute, "canExecute");
			this.execute = execute;
			this.canExecute = canExecute;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004DDB File Offset: 0x00002FDB
		public AsyncRelayCommand([Nullable(new byte[]
		{
			1,
			2,
			1
		})] Func<T, Task> execute, [Nullable(new byte[]
		{
			1,
			2
		})] Predicate<T> canExecute, AsyncRelayCommandOptions options)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			ArgumentNullException.ThrowIfNull(canExecute, "canExecute");
			this.execute = execute;
			this.canExecute = canExecute;
			this.options = options;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004E0E File Offset: 0x0000300E
		public AsyncRelayCommand([Nullable(new byte[]
		{
			1,
			2,
			1
		})] Func<T, CancellationToken, Task> cancelableExecute, [Nullable(new byte[]
		{
			1,
			2
		})] Predicate<T> canExecute)
		{
			ArgumentNullException.ThrowIfNull(cancelableExecute, "cancelableExecute");
			ArgumentNullException.ThrowIfNull(canExecute, "canExecute");
			this.cancelableExecute = cancelableExecute;
			this.canExecute = canExecute;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004E3A File Offset: 0x0000303A
		public AsyncRelayCommand([Nullable(new byte[]
		{
			1,
			2,
			1
		})] Func<T, CancellationToken, Task> cancelableExecute, [Nullable(new byte[]
		{
			1,
			2
		})] Predicate<T> canExecute, AsyncRelayCommandOptions options)
		{
			ArgumentNullException.ThrowIfNull(cancelableExecute, "cancelableExecute");
			ArgumentNullException.ThrowIfNull(canExecute, "canExecute");
			this.cancelableExecute = cancelableExecute;
			this.canExecute = canExecute;
			this.options = options;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00004E6D File Offset: 0x0000306D
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00004E78 File Offset: 0x00003078
		public Task ExecutionTask
		{
			get
			{
				return this.executionTask;
			}
			private set
			{
				if (this.executionTask == value)
				{
					return;
				}
				this.executionTask = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged != null)
				{
					propertyChanged.Invoke(this, AsyncRelayCommand.ExecutionTaskChangedEventArgs);
				}
				PropertyChangedEventHandler propertyChanged2 = this.PropertyChanged;
				if (propertyChanged2 != null)
				{
					propertyChanged2.Invoke(this, AsyncRelayCommand.IsRunningChangedEventArgs);
				}
				bool flag = value == null || value.IsCompleted;
				if (this.cancellationTokenSource != null)
				{
					PropertyChangedEventHandler propertyChanged3 = this.PropertyChanged;
					if (propertyChanged3 != null)
					{
						propertyChanged3.Invoke(this, AsyncRelayCommand.CanBeCanceledChangedEventArgs);
					}
					PropertyChangedEventHandler propertyChanged4 = this.PropertyChanged;
					if (propertyChanged4 != null)
					{
						propertyChanged4.Invoke(this, AsyncRelayCommand.IsCancellationRequestedChangedEventArgs);
					}
				}
				if (flag)
				{
					return;
				}
				AsyncRelayCommand<T>.<set_ExecutionTask>g__MonitorTask|22_0(this, value);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004F10 File Offset: 0x00003110
		public bool CanBeCanceled
		{
			get
			{
				if (this.IsRunning)
				{
					CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource;
					return cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested;
				}
				return false;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004F3C File Offset: 0x0000313C
		public bool IsCancellationRequested
		{
			get
			{
				CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource;
				return cancellationTokenSource != null && cancellationTokenSource.IsCancellationRequested;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00004F5C File Offset: 0x0000315C
		public bool IsRunning
		{
			get
			{
				Task task = this.ExecutionTask;
				return task != null && !task.IsCompleted;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004F7E File Offset: 0x0000317E
		bool ICancellationAwareCommand.IsCancellationSupported
		{
			get
			{
				return this.execute == null;
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004F89 File Offset: 0x00003189
		public void NotifyCanExecuteChanged()
		{
			EventHandler canExecuteChanged = this.CanExecuteChanged;
			if (canExecuteChanged == null)
			{
				return;
			}
			canExecuteChanged.Invoke(this, EventArgs.Empty);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004FA4 File Offset: 0x000031A4
		[MethodImpl(256)]
		public bool CanExecute(T parameter)
		{
			Predicate<T> predicate = this.canExecute;
			if (predicate != null && !predicate.Invoke(parameter))
			{
				return false;
			}
			if ((this.options & AsyncRelayCommandOptions.AllowConcurrentExecutions) == AsyncRelayCommandOptions.None)
			{
				Task task = this.ExecutionTask;
				return task == null || task.IsCompleted;
			}
			return true;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004FF0 File Offset: 0x000031F0
		[MethodImpl(256)]
		public bool CanExecute(object parameter)
		{
			if (parameter == null && default(T) != null)
			{
				return false;
			}
			T t;
			if (!RelayCommand<T>.TryGetCommandArgument(parameter, ref t))
			{
				RelayCommand<T>.ThrowArgumentExceptionForInvalidCommandArgument(parameter);
			}
			return this.CanExecute(t);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000502C File Offset: 0x0000322C
		[MethodImpl(256)]
		public void Execute(T parameter)
		{
			Task task = this.ExecuteAsync(parameter);
			if ((this.options & AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler) == AsyncRelayCommandOptions.None)
			{
				AsyncRelayCommand.AwaitAndThrowIfFailed(task);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005054 File Offset: 0x00003254
		public void Execute(object parameter)
		{
			T t;
			if (!RelayCommand<T>.TryGetCommandArgument(parameter, ref t))
			{
				RelayCommand<T>.ThrowArgumentExceptionForInvalidCommandArgument(parameter);
			}
			this.Execute(t);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005078 File Offset: 0x00003278
		[NullableContext(1)]
		public Task ExecuteAsync([Nullable(2)] T parameter)
		{
			Task result;
			if (this.execute != null)
			{
				result = (this.ExecutionTask = this.execute.Invoke(parameter));
			}
			else
			{
				CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource;
				if (cancellationTokenSource != null)
				{
					cancellationTokenSource.Cancel();
				}
				CancellationTokenSource cancellationTokenSource2 = this.cancellationTokenSource = new CancellationTokenSource();
				result = (this.ExecutionTask = this.cancelableExecute.Invoke(parameter, cancellationTokenSource2.Token));
			}
			if ((this.options & AsyncRelayCommandOptions.AllowConcurrentExecutions) == AsyncRelayCommandOptions.None)
			{
				EventHandler canExecuteChanged = this.CanExecuteChanged;
				if (canExecuteChanged != null)
				{
					canExecuteChanged.Invoke(this, EventArgs.Empty);
				}
			}
			return result;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005104 File Offset: 0x00003304
		[NullableContext(1)]
		public Task ExecuteAsync([Nullable(2)] object parameter)
		{
			T t;
			if (!RelayCommand<T>.TryGetCommandArgument(parameter, ref t))
			{
				RelayCommand<T>.ThrowArgumentExceptionForInvalidCommandArgument(parameter);
			}
			return this.ExecuteAsync(t);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005128 File Offset: 0x00003328
		public void Cancel()
		{
			CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource;
			if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
			{
				cancellationTokenSource.Cancel();
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged != null)
				{
					propertyChanged.Invoke(this, AsyncRelayCommand.CanBeCanceledChangedEventArgs);
				}
				PropertyChangedEventHandler propertyChanged2 = this.PropertyChanged;
				if (propertyChanged2 == null)
				{
					return;
				}
				propertyChanged2.Invoke(this, AsyncRelayCommand.IsCancellationRequestedChangedEventArgs);
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000517C File Offset: 0x0000337C
		[NullableContext(1)]
		[CompilerGenerated]
		internal static void <set_ExecutionTask>g__MonitorTask|22_0(AsyncRelayCommand<T> @this, Task task)
		{
			AsyncRelayCommand<T>.<<set_ExecutionTask>g__MonitorTask|22_0>d <<set_ExecutionTask>g__MonitorTask|22_0>d;
			<<set_ExecutionTask>g__MonitorTask|22_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
			<<set_ExecutionTask>g__MonitorTask|22_0>d.@this = @this;
			<<set_ExecutionTask>g__MonitorTask|22_0>d.task = task;
			<<set_ExecutionTask>g__MonitorTask|22_0>d.<>1__state = -1;
			<<set_ExecutionTask>g__MonitorTask|22_0>d.<>t__builder.Start<AsyncRelayCommand<T>.<<set_ExecutionTask>g__MonitorTask|22_0>d>(ref <<set_ExecutionTask>g__MonitorTask|22_0>d);
		}

		// Token: 0x0400004A RID: 74
		[Nullable(new byte[]
		{
			2,
			2,
			1
		})]
		private readonly Func<T, Task> execute;

		// Token: 0x0400004B RID: 75
		[Nullable(new byte[]
		{
			2,
			2,
			1
		})]
		private readonly Func<T, CancellationToken, Task> cancelableExecute;

		// Token: 0x0400004C RID: 76
		private readonly Predicate<T> canExecute;

		// Token: 0x0400004D RID: 77
		private readonly AsyncRelayCommandOptions options;

		// Token: 0x0400004E RID: 78
		private CancellationTokenSource cancellationTokenSource;

		// Token: 0x04000051 RID: 81
		private Task executionTask;
	}
}
