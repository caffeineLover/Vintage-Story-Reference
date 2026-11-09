using System;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging.Internals
{
	// Token: 0x0200001B RID: 27
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class MessageHandlerDispatcher
	{
		// Token: 0x060000A2 RID: 162
		public abstract void Invoke(object recipient, object message);

		// Token: 0x0200005D RID: 93
		[Nullable(0)]
		public sealed class For<TRecipient, TMessage> : MessageHandlerDispatcher where TRecipient : class where TMessage : class
		{
			// Token: 0x060001FA RID: 506 RVA: 0x00008A06 File Offset: 0x00006C06
			public For(MessageHandler<TRecipient, TMessage> handler)
			{
				this.handler = handler;
			}

			// Token: 0x060001FB RID: 507 RVA: 0x00008A15 File Offset: 0x00006C15
			public override void Invoke(object recipient, object message)
			{
				this.handler.Invoke(Unsafe.As<TRecipient>(recipient), Unsafe.As<TMessage>(message));
			}

			// Token: 0x040000B6 RID: 182
			private readonly MessageHandler<TRecipient, TMessage> handler;
		}
	}
}
