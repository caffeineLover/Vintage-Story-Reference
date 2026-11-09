using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging.Messages
{
	// Token: 0x02000018 RID: 24
	[NullableContext(1)]
	[Nullable(0)]
	public class RequestMessage<[Nullable(2)] T>
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000446C File Offset: 0x0000266C
		public T Response
		{
			get
			{
				if (!this.HasReceivedResponse)
				{
					RequestMessage<T>.ThrowInvalidOperationExceptionForNoResponseReceived();
				}
				return this.response;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004481 File Offset: 0x00002681
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00004489 File Offset: 0x00002689
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

		// Token: 0x06000095 RID: 149 RVA: 0x00004492 File Offset: 0x00002692
		public void Reply(T response)
		{
			if (this.HasReceivedResponse)
			{
				RequestMessage<T>.ThrowInvalidOperationExceptionForDuplicateReply();
			}
			this.HasReceivedResponse = true;
			this.response = response;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000044AF File Offset: 0x000026AF
		public static implicit operator T(RequestMessage<T> message)
		{
			ArgumentNullException.ThrowIfNull(message, "message");
			return message.Response;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000044C2 File Offset: 0x000026C2
		[DoesNotReturn]
		private static void ThrowInvalidOperationExceptionForNoResponseReceived()
		{
			throw new InvalidOperationException("No response was received for the given request message.");
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000044CE File Offset: 0x000026CE
		[DoesNotReturn]
		private static void ThrowInvalidOperationExceptionForDuplicateReply()
		{
			throw new InvalidOperationException("A response has already been issued for the current message.");
		}

		// Token: 0x04000031 RID: 49
		private T response;
	}
}
