using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.Messaging.Messages
{
	// Token: 0x02000015 RID: 21
	[NullableContext(1)]
	[Nullable(0)]
	public class AsyncRequestMessage<[Nullable(2)] T>
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00004350 File Offset: 0x00002550
		public Task<T> Response
		{
			get
			{
				if (!this.HasReceivedResponse)
				{
					AsyncRequestMessage<T>.ThrowInvalidOperationExceptionForNoResponseReceived();
				}
				return this.response;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00004365 File Offset: 0x00002565
		// (set) Token: 0x06000081 RID: 129 RVA: 0x0000436D File Offset: 0x0000256D
		public bool HasReceivedResponse
		{
			[CompilerGenerated]
			get
			{
				return this.<HasReceivedResponse>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<HasReceivedResponse>k__BackingField = value;
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004376 File Offset: 0x00002576
		public void Reply(T response)
		{
			this.Reply(Task.FromResult<T>(response));
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004384 File Offset: 0x00002584
		public void Reply(Task<T> response)
		{
			ArgumentNullException.ThrowIfNull(response, "response");
			if (this.HasReceivedResponse)
			{
				AsyncRequestMessage<T>.ThrowInvalidOperationExceptionForDuplicateReply();
			}
			this.HasReceivedResponse = true;
			this.response = response;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000043AC File Offset: 0x000025AC
		[EditorBrowsable(1)]
		[MethodImpl(256)]
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		public TaskAwaiter<T> GetAwaiter()
		{
			return this.Response.GetAwaiter();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000043B9 File Offset: 0x000025B9
		[DoesNotReturn]
		private static void ThrowInvalidOperationExceptionForNoResponseReceived()
		{
			throw new InvalidOperationException("No response was received for the given request message.");
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000043C5 File Offset: 0x000025C5
		[DoesNotReturn]
		private static void ThrowInvalidOperationExceptionForDuplicateReply()
		{
			throw new InvalidOperationException("A response has already been issued for the current message.");
		}

		// Token: 0x0400002A RID: 42
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private Task<T> response;
	}
}
