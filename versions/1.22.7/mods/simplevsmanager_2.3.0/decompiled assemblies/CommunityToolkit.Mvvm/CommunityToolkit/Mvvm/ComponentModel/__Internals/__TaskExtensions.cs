using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.ComponentModel.__Internals
{
	// Token: 0x02000039 RID: 57
	[EditorBrowsable(1)]
	[Obsolete("This type is not intended to be used directly by user code")]
	public static class __TaskExtensions
	{
		// Token: 0x06000171 RID: 369 RVA: 0x000068C8 File Offset: 0x00004AC8
		[NullableContext(1)]
		[EditorBrowsable(1)]
		[Obsolete("This method is not intended to be called directly by user code")]
		public static __TaskExtensions.TaskAwaitableWithoutEndValidation GetAwaitableWithoutEndValidation(this Task task)
		{
			return new __TaskExtensions.TaskAwaitableWithoutEndValidation(task);
		}

		// Token: 0x02000067 RID: 103
		[EditorBrowsable(1)]
		[Obsolete("This type is not intended to be called directly by user code")]
		public readonly struct TaskAwaitableWithoutEndValidation
		{
			// Token: 0x0600021A RID: 538 RVA: 0x00008F5D File Offset: 0x0000715D
			[NullableContext(1)]
			[MethodImpl(256)]
			public TaskAwaitableWithoutEndValidation(Task task)
			{
				this.task = task;
			}

			// Token: 0x0600021B RID: 539 RVA: 0x00008F66 File Offset: 0x00007166
			[MethodImpl(256)]
			public __TaskExtensions.TaskAwaitableWithoutEndValidation.Awaiter GetAwaiter()
			{
				return new __TaskExtensions.TaskAwaitableWithoutEndValidation.Awaiter(this.task);
			}

			// Token: 0x040000D7 RID: 215
			[Nullable(1)]
			private readonly Task task;

			// Token: 0x02000070 RID: 112
			public readonly struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
			{
				// Token: 0x0600022E RID: 558 RVA: 0x00009186 File Offset: 0x00007386
				[NullableContext(1)]
				public Awaiter(Task task)
				{
					this.taskAwaiter = task.GetAwaiter();
				}

				// Token: 0x17000049 RID: 73
				// (get) Token: 0x0600022F RID: 559 RVA: 0x00009194 File Offset: 0x00007394
				public bool IsCompleted
				{
					[MethodImpl(256)]
					get
					{
						return this.taskAwaiter.IsCompleted;
					}
				}

				// Token: 0x06000230 RID: 560 RVA: 0x000091A1 File Offset: 0x000073A1
				[MethodImpl(256)]
				public void GetResult()
				{
				}

				// Token: 0x06000231 RID: 561 RVA: 0x000091A3 File Offset: 0x000073A3
				[NullableContext(1)]
				[MethodImpl(256)]
				public void OnCompleted(Action continuation)
				{
					this.taskAwaiter.OnCompleted(continuation);
				}

				// Token: 0x06000232 RID: 562 RVA: 0x000091B1 File Offset: 0x000073B1
				[NullableContext(1)]
				[MethodImpl(256)]
				public void UnsafeOnCompleted(Action continuation)
				{
					this.taskAwaiter.UnsafeOnCompleted(continuation);
				}

				// Token: 0x040000E7 RID: 231
				private readonly TaskAwaiter taskAwaiter;
			}
		}
	}
}
