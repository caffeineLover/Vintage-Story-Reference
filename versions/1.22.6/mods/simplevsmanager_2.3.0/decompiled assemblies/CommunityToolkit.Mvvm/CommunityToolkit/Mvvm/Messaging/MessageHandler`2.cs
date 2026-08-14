using System;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging
{
	// Token: 0x02000011 RID: 17
	// (Invoke) Token: 0x06000051 RID: 81
	public delegate void MessageHandler<[Nullable(1)] in TRecipient, [Nullable(1)] in TMessage>(TRecipient recipient, TMessage message) where TRecipient : class where TMessage : class;
}
