using System;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging
{
	// Token: 0x02000010 RID: 16
	[NullableContext(1)]
	public interface IRecipient<in TMessage> where TMessage : class
	{
		// Token: 0x0600004F RID: 79
		void Receive(TMessage message);
	}
}
