using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.Messaging.Messages
{
	// Token: 0x02000014 RID: 20
	[NullableContext(1)]
	[Nullable(0)]
	public class AsyncCollectionRequestMessage<[Nullable(2)] T> : IAsyncEnumerable<T>
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00004275 File Offset: 0x00002475
		public CancellationToken CancellationToken
		{
			get
			{
				return this.cancellationTokenSource.Token;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004282 File Offset: 0x00002482
		public void Reply(T response)
		{
			this.Reply(Task.FromResult<T>(response));
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004290 File Offset: 0x00002490
		public void Reply(Task<T> response)
		{
			ArgumentNullException.ThrowIfNull(response, "response");
			this.responses.Add(new ValueTuple<Task<T>, Func<CancellationToken, Task<T>>>(response, null));
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000042AF File Offset: 0x000024AF
		public void Reply(Func<CancellationToken, Task<T>> response)
		{
			ArgumentNullException.ThrowIfNull(response, "response");
			this.responses.Add(new ValueTuple<Task<T>, Func<CancellationToken, Task<T>>>(null, response));
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000042D0 File Offset: 0x000024D0
		public Task<IReadOnlyCollection<T>> GetResponsesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			AsyncCollectionRequestMessage<T>.<GetResponsesAsync>d__7 <GetResponsesAsync>d__;
			<GetResponsesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyCollection<T>>.Create();
			<GetResponsesAsync>d__.<>4__this = this;
			<GetResponsesAsync>d__.cancellationToken = cancellationToken;
			<GetResponsesAsync>d__.<>1__state = -1;
			<GetResponsesAsync>d__.<>t__builder.Start<AsyncCollectionRequestMessage<T>.<GetResponsesAsync>d__7>(ref <GetResponsesAsync>d__);
			return <GetResponsesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000431B File Offset: 0x0000251B
		[AsyncIteratorStateMachine(typeof(AsyncCollectionRequestMessage<>.<GetAsyncEnumerator>d__8))]
		[EditorBrowsable(1)]
		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			AsyncCollectionRequestMessage<T>.<GetAsyncEnumerator>d__8 <GetAsyncEnumerator>d__ = new AsyncCollectionRequestMessage<T>.<GetAsyncEnumerator>d__8(-3);
			<GetAsyncEnumerator>d__.<>4__this = this;
			<GetAsyncEnumerator>d__.cancellationToken = cancellationToken;
			return <GetAsyncEnumerator>d__;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004332 File Offset: 0x00002532
		public AsyncCollectionRequestMessage()
		{
			this.responses = new List<ValueTuple<Task<T>, Func<CancellationToken, Task<T>>>>();
			this.cancellationTokenSource = new CancellationTokenSource();
			base..ctor();
		}

		// Token: 0x04000028 RID: 40
		[Nullable(new byte[]
		{
			1,
			0,
			2,
			1,
			2,
			1,
			1
		})]
		private readonly List<ValueTuple<Task<T>, Func<CancellationToken, Task<T>>>> responses;

		// Token: 0x04000029 RID: 41
		private readonly CancellationTokenSource cancellationTokenSource;
	}
}
