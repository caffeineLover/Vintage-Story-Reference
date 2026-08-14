using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input.Internals;

namespace CommunityToolkit.Mvvm.Input
{
	// Token: 0x0200001E RID: 30
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AsyncRelayCommand : IAsyncRelayCommand, IRelayCommand, ICommand, INotifyPropertyChanged, ICancellationAwareCommand
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000AB RID: 171 RVA: 0x0000470C File Offset: 0x0000290C
		// (remove) Token: 0x060000AC RID: 172 RVA: 0x00004744 File Offset: 0x00002944
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000AD RID: 173 RVA: 0x0000477C File Offset: 0x0000297C
		// (remove) Token: 0x060000AE RID: 174 RVA: 0x000047B4 File Offset: 0x000029B4
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event EventHandler CanExecuteChanged;

		// Token: 0x060000AF RID: 175 RVA: 0x000047E9 File Offset: 0x000029E9
		public AsyncRelayCommand(Func<Task> execute)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			this.execute = execute;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004803 File Offset: 0x00002A03
		public AsyncRelayCommand(Func<Task> execute, AsyncRelayCommandOptions options)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			this.execute = execute;
			this.options = options;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004824 File Offset: 0x00002A24
		public AsyncRelayCommand(Func<CancellationToken, Task> cancelableExecute)
		{
			ArgumentNullException.ThrowIfNull(cancelableExecute, "cancelableExecute");
			this.cancelableExecute = cancelableExecute;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000483E File Offset: 0x00002A3E
		public AsyncRelayCommand(Func<CancellationToken, Task> cancelableExecute, AsyncRelayCommandOptions options)
		{
			ArgumentNullException.ThrowIfNull(cancelableExecute, "cancelableExecute");
			this.cancelableExecute = cancelableExecute;
			this.options = options;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000485F File Offset: 0x00002A5F
		public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			ArgumentNullException.ThrowIfNull(canExecute, "canExecute");
			this.execute = execute;
			this.canExecute = canExecute;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000488B File Offset: 0x00002A8B
		public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute, AsyncRelayCommandOptions options)
		{
			ArgumentNullException.ThrowIfNull(execute, "execute");
			ArgumentNullException.ThrowIfNull(canExecute, "canExecute");
			this.execute = execute;
			this.canExecute = canExecute;
			this.options = options;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000048BE File Offset: 0x00002ABE
		public AsyncRelayCommand(Func<CancellationToken, Task> cancelableExecute, Func<bool> canExecute)
		{
			ArgumentNullException.ThrowIfNull(cancelableExecute, "cancelableExecute");
			ArgumentNullException.ThrowIfNull(canExecute, "canExecute");
			this.cancelableExecute = cancelableExecute;
			this.canExecute = canExecute;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000048EA File Offset: 0x00002AEA
		public AsyncRelayCommand(Func<CancellationToken, Task> cancelableExecute, Func<bool> canExecute, AsyncRelayCommandOptions options)
		{
			ArgumentNullException.ThrowIfNull(cancelableExecute, "cancelableExecute");
			ArgumentNullException.ThrowIfNull(canExecute, "canExecute");
			this.cancelableExecute = cancelableExecute;
			this.canExecute = canExecute;
			this.options = options;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000491D File Offset: 0x00002B1D
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00004928 File Offset: 0x00002B28
		[Nullable(2)]
		public Task ExecutionTask
		{
			[NullableContext(2)]
			get
			{
				return this.executionTask;
			}
			[NullableContext(2)]
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
				AsyncRelayCommand.<set_ExecutionTask>g__MonitorTask|26_0(this, value);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000049C0 File Offset: 0x00002BC0
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

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000049EC File Offset: 0x00002BEC
		public bool IsCancellationRequested
		{
			get
			{
				CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource;
				return cancellationTokenSource != null && cancellationTokenSource.IsCancellationRequested;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004A0C File Offset: 0x00002C0C
		public bool IsRunning
		{
			get
			{
				Task task = this.ExecutionTask;
				return task != null && !task.IsCompleted;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004A2E File Offset: 0x00002C2E
		bool ICancellationAwareCommand.IsCancellationSupported
		{
			get
			{
				return this.execute == null;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004A39 File Offset: 0x00002C39
		public void NotifyCanExecuteChanged()
		{
			EventHandler canExecuteChanged = this.CanExecuteChanged;
			if (canExecuteChanged == null)
			{
				return;
			}
			canExecuteChanged.Invoke(this, EventArgs.Empty);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004A54 File Offset: 0x00002C54
		[NullableContext(2)]
		[MethodImpl(256)]
		public bool CanExecute(object parameter)
		{
			Func<bool> func = this.canExecute;
			if (func != null && !func.Invoke())
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

		// Token: 0x060000BF RID: 191 RVA: 0x00004A9C File Offset: 0x00002C9C
		[NullableContext(2)]
		public void Execute(object parameter)
		{
			Task task = this.ExecuteAsync(parameter);
			if ((this.options & AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler) == AsyncRelayCommandOptions.None)
			{
				AsyncRelayCommand.AwaitAndThrowIfFailed(task);
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004AC4 File Offset: 0x00002CC4
		public Task ExecuteAsync([Nullable(2)] object parameter)
		{
			Task result;
			if (this.execute != null)
			{
				result = (this.ExecutionTask = this.execute.Invoke());
			}
			else
			{
				CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource;
				if (cancellationTokenSource != null)
				{
					cancellationTokenSource.Cancel();
				}
				CancellationTokenSource cancellationTokenSource2 = this.cancellationTokenSource = new CancellationTokenSource();
				result = (this.ExecutionTask = this.cancelableExecute.Invoke(cancellationTokenSource2.Token));
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

		// Token: 0x060000C1 RID: 193 RVA: 0x00004B50 File Offset: 0x00002D50
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

		// Token: 0x060000C2 RID: 194 RVA: 0x00004BA4 File Offset: 0x00002DA4
		internal static void AwaitAndThrowIfFailed(Task executionTask)
		{
			AsyncRelayCommand.<AwaitAndThrowIfFailed>d__40 <AwaitAndThrowIfFailed>d__;
			<AwaitAndThrowIfFailed>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<AwaitAndThrowIfFailed>d__.executionTask = executionTask;
			<AwaitAndThrowIfFailed>d__.<>1__state = -1;
			<AwaitAndThrowIfFailed>d__.<>t__builder.Start<AsyncRelayCommand.<AwaitAndThrowIfFailed>d__40>(ref <AwaitAndThrowIfFailed>d__);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004C1C File Offset: 0x00002E1C
		[CompilerGenerated]
		internal static void <set_ExecutionTask>g__MonitorTask|26_0(AsyncRelayCommand @this, Task task)
		{
			AsyncRelayCommand.<<set_ExecutionTask>g__MonitorTask|26_0>d <<set_ExecutionTask>g__MonitorTask|26_0>d;
			<<set_ExecutionTask>g__MonitorTask|26_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
			<<set_ExecutionTask>g__MonitorTask|26_0>d.@this = @this;
			<<set_ExecutionTask>g__MonitorTask|26_0>d.task = task;
			<<set_ExecutionTask>g__MonitorTask|26_0>d.<>1__state = -1;
			<<set_ExecutionTask>g__MonitorTask|26_0>d.<>t__builder.Start<AsyncRelayCommand.<<set_ExecutionTask>g__MonitorTask|26_0>d>(ref <<set_ExecutionTask>g__MonitorTask|26_0>d);
		}

		// Token: 0x0400003A RID: 58
		internal static readonly PropertyChangedEventArgs ExecutionTaskChangedEventArgs = new PropertyChangedEventArgs("ExecutionTask");

		// Token: 0x0400003B RID: 59
		internal static readonly PropertyChangedEventArgs CanBeCanceledChangedEventArgs = new PropertyChangedEventArgs("CanBeCanceled");

		// Token: 0x0400003C RID: 60
		internal static readonly PropertyChangedEventArgs IsCancellationRequestedChangedEventArgs = new PropertyChangedEventArgs("IsCancellationRequested");

		// Token: 0x0400003D RID: 61
		internal static readonly PropertyChangedEventArgs IsRunningChangedEventArgs = new PropertyChangedEventArgs("IsRunning");

		// Token: 0x0400003E RID: 62
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private readonly Func<Task> execute;

		// Token: 0x0400003F RID: 63
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private readonly Func<CancellationToken, Task> cancelableExecute;

		// Token: 0x04000040 RID: 64
		[Nullable(2)]
		private readonly Func<bool> canExecute;

		// Token: 0x04000041 RID: 65
		private readonly AsyncRelayCommandOptions options;

		// Token: 0x04000042 RID: 66
		[Nullable(2)]
		private CancellationTokenSource cancellationTokenSource;

		// Token: 0x04000045 RID: 69
		[Nullable(2)]
		private Task executionTask;
	}
}
