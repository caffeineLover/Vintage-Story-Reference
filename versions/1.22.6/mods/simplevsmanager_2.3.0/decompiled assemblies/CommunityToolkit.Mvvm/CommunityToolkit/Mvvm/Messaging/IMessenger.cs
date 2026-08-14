using System;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging
{
	// Token: 0x0200000E RID: 14
	[NullableContext(1)]
	public interface IMessenger
	{
		// Token: 0x06000036 RID: 54
		bool IsRegistered<TMessage, [Nullable(0)] TToken>(object recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>;

		// Token: 0x06000037 RID: 55
		void Register<TRecipient, TMessage, [Nullable(0)] TToken>(TRecipient recipient, TToken token, MessageHandler<TRecipient, TMessage> handler) where TRecipient : class where TMessage : class where TToken : IEquatable<TToken>;

		// Token: 0x06000038 RID: 56
		void UnregisterAll(object recipient);

		// Token: 0x06000039 RID: 57
		void UnregisterAll<[Nullable(0)] TToken>(object recipient, TToken token) where TToken : IEquatable<TToken>;

		// Token: 0x0600003A RID: 58
		void Unregister<TMessage, [Nullable(0)] TToken>(object recipient, TToken token) where TMessage : class where TToken : IEquatable<TToken>;

		// Token: 0x0600003B RID: 59
		TMessage Send<TMessage, [Nullable(0)] TToken>(TMessage message, TToken token) where TMessage : class where TToken : IEquatable<TToken>;

		// Token: 0x0600003C RID: 60
		void Cleanup();

		// Token: 0x0600003D RID: 61
		void Reset();
	}
}
