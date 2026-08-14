using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.ComponentModel
{
	// Token: 0x02000035 RID: 53
	[NullableContext(2)]
	[Nullable(0)]
	public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600012D RID: 301 RVA: 0x00005870 File Offset: 0x00003A70
		// (remove) Token: 0x0600012E RID: 302 RVA: 0x000058A8 File Offset: 0x00003AA8
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600012F RID: 303 RVA: 0x000058E0 File Offset: 0x00003AE0
		// (remove) Token: 0x06000130 RID: 304 RVA: 0x00005918 File Offset: 0x00003B18
		public event PropertyChangingEventHandler PropertyChanging;

		// Token: 0x06000131 RID: 305 RVA: 0x0000594D File Offset: 0x00003B4D
		[NullableContext(1)]
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			ArgumentNullException.ThrowIfNull(e, "e");
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged.Invoke(this, e);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000596C File Offset: 0x00003B6C
		[NullableContext(1)]
		protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
		{
			ArgumentNullException.ThrowIfNull(e, "e");
			if (!FeatureSwitches.EnableINotifyPropertyChangingSupport)
			{
				return;
			}
			PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
			if (propertyChanging == null)
			{
				return;
			}
			propertyChanging.Invoke(this, e);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005993 File Offset: 0x00003B93
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000059A1 File Offset: 0x00003BA1
		protected void OnPropertyChanging([CallerMemberName] string propertyName = null)
		{
			if (!FeatureSwitches.EnableINotifyPropertyChangingSupport)
			{
				return;
			}
			this.OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000059B7 File Offset: 0x00003BB7
		[NullableContext(1)]
		protected bool SetProperty<[Nullable(2)] T>([NotNullIfNotNull("newValue")] ref T field, T newValue, [Nullable(2)] [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, newValue))
			{
				return false;
			}
			this.OnPropertyChanging(propertyName);
			field = newValue;
			this.OnPropertyChanged(propertyName);
			return true;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000059E4 File Offset: 0x00003BE4
		[NullableContext(1)]
		protected bool SetProperty<[Nullable(2)] T>([NotNullIfNotNull("newValue")] ref T field, T newValue, IEqualityComparer<T> comparer, [Nullable(2)] [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			if (comparer.Equals(field, newValue))
			{
				return false;
			}
			this.OnPropertyChanging(propertyName);
			field = newValue;
			this.OnPropertyChanged(propertyName);
			return true;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005A1A File Offset: 0x00003C1A
		[NullableContext(1)]
		protected bool SetProperty<[Nullable(2)] T>(T oldValue, T newValue, Action<T> callback, [Nullable(2)] [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(callback, "callback");
			if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
			{
				return false;
			}
			this.OnPropertyChanging(propertyName);
			callback.Invoke(newValue);
			this.OnPropertyChanged(propertyName);
			return true;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005A4F File Offset: 0x00003C4F
		[NullableContext(1)]
		protected bool SetProperty<[Nullable(2)] T>(T oldValue, T newValue, IEqualityComparer<T> comparer, Action<T> callback, [Nullable(2)] [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			if (comparer.Equals(oldValue, newValue))
			{
				return false;
			}
			this.OnPropertyChanging(propertyName);
			callback.Invoke(newValue);
			this.OnPropertyChanged(propertyName);
			return true;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005A90 File Offset: 0x00003C90
		[NullableContext(1)]
		protected bool SetProperty<TModel, [Nullable(2)] T>(T oldValue, T newValue, TModel model, Action<TModel, T> callback, [Nullable(2)] [CallerMemberName] string propertyName = null) where TModel : class
		{
			ArgumentNullException.ThrowIfNull(model, "model");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
			{
				return false;
			}
			this.OnPropertyChanging(propertyName);
			callback.Invoke(model, newValue);
			this.OnPropertyChanged(propertyName);
			return true;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005AE4 File Offset: 0x00003CE4
		[NullableContext(1)]
		protected bool SetProperty<TModel, [Nullable(2)] T>(T oldValue, T newValue, IEqualityComparer<T> comparer, TModel model, Action<TModel, T> callback, [Nullable(2)] [CallerMemberName] string propertyName = null) where TModel : class
		{
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(model, "model");
			ArgumentNullException.ThrowIfNull(callback, "callback");
			if (comparer.Equals(oldValue, newValue))
			{
				return false;
			}
			this.OnPropertyChanging(propertyName);
			callback.Invoke(model, newValue);
			this.OnPropertyChanged(propertyName);
			return true;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00005B40 File Offset: 0x00003D40
		protected bool SetPropertyAndNotifyOnCompletion([NotNull] ref ObservableObject.TaskNotifier taskNotifier, Task newValue, [CallerMemberName] string propertyName = null)
		{
			ObservableObject.TaskNotifier taskNotifier2;
			if ((taskNotifier2 = taskNotifier) == null)
			{
				ObservableObject.TaskNotifier taskNotifier3;
				taskNotifier = (taskNotifier3 = new ObservableObject.TaskNotifier());
				taskNotifier2 = taskNotifier3;
			}
			return this.SetPropertyAndNotifyOnCompletion<Task>(taskNotifier2, newValue, null, propertyName);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00005B68 File Offset: 0x00003D68
		protected bool SetPropertyAndNotifyOnCompletion([NotNull] ref ObservableObject.TaskNotifier taskNotifier, Task newValue, [Nullable(new byte[]
		{
			1,
			2
		})] Action<Task> callback, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(callback, "callback");
			ObservableObject.TaskNotifier taskNotifier2;
			if ((taskNotifier2 = taskNotifier) == null)
			{
				ObservableObject.TaskNotifier taskNotifier3;
				taskNotifier = (taskNotifier3 = new ObservableObject.TaskNotifier());
				taskNotifier2 = taskNotifier3;
			}
			return this.SetPropertyAndNotifyOnCompletion<Task>(taskNotifier2, newValue, callback, propertyName);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005B9C File Offset: 0x00003D9C
		protected bool SetPropertyAndNotifyOnCompletion<T>([Nullable(new byte[]
		{
			2,
			1
		})] [NotNull] ref ObservableObject.TaskNotifier<T> taskNotifier, [Nullable(new byte[]
		{
			2,
			1
		})] Task<T> newValue, [CallerMemberName] string propertyName = null)
		{
			ObservableObject.TaskNotifier<T> taskNotifier2;
			if ((taskNotifier2 = taskNotifier) == null)
			{
				ObservableObject.TaskNotifier<T> taskNotifier3;
				taskNotifier = (taskNotifier3 = new ObservableObject.TaskNotifier<T>());
				taskNotifier2 = taskNotifier3;
			}
			return this.SetPropertyAndNotifyOnCompletion<Task<T>>(taskNotifier2, newValue, null, propertyName);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00005BC4 File Offset: 0x00003DC4
		protected bool SetPropertyAndNotifyOnCompletion<T>([Nullable(new byte[]
		{
			2,
			1
		})] [NotNull] ref ObservableObject.TaskNotifier<T> taskNotifier, [Nullable(new byte[]
		{
			2,
			1
		})] Task<T> newValue, [Nullable(new byte[]
		{
			1,
			2,
			1
		})] Action<Task<T>> callback, [CallerMemberName] string propertyName = null)
		{
			ArgumentNullException.ThrowIfNull(callback, "callback");
			ObservableObject.TaskNotifier<T> taskNotifier2;
			if ((taskNotifier2 = taskNotifier) == null)
			{
				ObservableObject.TaskNotifier<T> taskNotifier3;
				taskNotifier = (taskNotifier3 = new ObservableObject.TaskNotifier<T>());
				taskNotifier2 = taskNotifier3;
			}
			return this.SetPropertyAndNotifyOnCompletion<Task<T>>(taskNotifier2, newValue, callback, propertyName);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005BF8 File Offset: 0x00003DF8
		private bool SetPropertyAndNotifyOnCompletion<[Nullable(0)] TTask>([Nullable(1)] ObservableObject.ITaskNotifier<TTask> taskNotifier, TTask newValue, Action<TTask> callback, [CallerMemberName] string propertyName = null) where TTask : Task
		{
			ObservableObject.<>c__DisplayClass20_0<TTask> CS$<>8__locals1 = new ObservableObject.<>c__DisplayClass20_0<TTask>();
			CS$<>8__locals1.newValue = newValue;
			CS$<>8__locals1.taskNotifier = taskNotifier;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.propertyName = propertyName;
			CS$<>8__locals1.callback = callback;
			if (CS$<>8__locals1.taskNotifier.Task == CS$<>8__locals1.newValue)
			{
				return false;
			}
			TTask ttask = CS$<>8__locals1.newValue;
			bool flag = ttask == null || ttask.IsCompleted;
			this.OnPropertyChanging(CS$<>8__locals1.propertyName);
			CS$<>8__locals1.taskNotifier.Task = CS$<>8__locals1.newValue;
			this.OnPropertyChanged(CS$<>8__locals1.propertyName);
			if (flag)
			{
				if (CS$<>8__locals1.callback != null)
				{
					CS$<>8__locals1.callback.Invoke(CS$<>8__locals1.newValue);
				}
				return true;
			}
			CS$<>8__locals1.<SetPropertyAndNotifyOnCompletion>g__MonitorTask|0();
			return true;
		}

		// Token: 0x02000061 RID: 97
		private interface ITaskNotifier<[Nullable(0)] TTask> where TTask : Task
		{
			// Token: 0x17000046 RID: 70
			// (get) Token: 0x06000202 RID: 514
			// (set) Token: 0x06000203 RID: 515
			TTask Task { get; set; }
		}

		// Token: 0x02000062 RID: 98
		[Nullable(0)]
		protected sealed class TaskNotifier : ObservableObject.ITaskNotifier<Task>
		{
			// Token: 0x06000204 RID: 516 RVA: 0x00008DFA File Offset: 0x00006FFA
			internal TaskNotifier()
			{
			}

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x06000205 RID: 517 RVA: 0x00008E02 File Offset: 0x00007002
			// (set) Token: 0x06000206 RID: 518 RVA: 0x00008E0A File Offset: 0x0000700A
			Task ObservableObject.ITaskNotifier<Task>.Task
			{
				get
				{
					return this.task;
				}
				set
				{
					this.task = value;
				}
			}

			// Token: 0x06000207 RID: 519 RVA: 0x00008E13 File Offset: 0x00007013
			public static implicit operator Task(ObservableObject.TaskNotifier notifier)
			{
				if (notifier == null)
				{
					return null;
				}
				return notifier.task;
			}

			// Token: 0x040000C5 RID: 197
			private Task task;
		}

		// Token: 0x02000063 RID: 99
		[NullableContext(0)]
		protected sealed class TaskNotifier<[Nullable(2)] T> : ObservableObject.ITaskNotifier<Task<T>>
		{
			// Token: 0x06000208 RID: 520 RVA: 0x00008E20 File Offset: 0x00007020
			internal TaskNotifier()
			{
			}

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x06000209 RID: 521 RVA: 0x00008E28 File Offset: 0x00007028
			// (set) Token: 0x0600020A RID: 522 RVA: 0x00008E30 File Offset: 0x00007030
			[Nullable(new byte[]
			{
				2,
				1
			})]
			Task<T> ObservableObject.ITaskNotifier<Task<!0>>.Task
			{
				[return: Nullable(new byte[]
				{
					2,
					1
				})]
				get
				{
					return this.task;
				}
				[param: Nullable(new byte[]
				{
					2,
					1
				})]
				set
				{
					this.task = value;
				}
			}

			// Token: 0x0600020B RID: 523 RVA: 0x00008E39 File Offset: 0x00007039
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			public static implicit operator Task<T>([Nullable(new byte[]
			{
				2,
				1
			})] ObservableObject.TaskNotifier<T> notifier)
			{
				if (notifier == null)
				{
					return null;
				}
				return notifier.task;
			}

			// Token: 0x040000C6 RID: 198
			[Nullable(new byte[]
			{
				2,
				1
			})]
			private Task<T> task;
		}
	}
}
