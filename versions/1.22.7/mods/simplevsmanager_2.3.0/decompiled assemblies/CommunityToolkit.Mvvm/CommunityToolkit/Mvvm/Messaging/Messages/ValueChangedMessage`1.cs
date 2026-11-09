using System;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging.Messages
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	[Nullable(0)]
	public class ValueChangedMessage<[Nullable(2)] T>
	{
		// Token: 0x0600009A RID: 154 RVA: 0x000044E2 File Offset: 0x000026E2
		public ValueChangedMessage(T value)
		{
			this.<Value>k__BackingField = value;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000044F1 File Offset: 0x000026F1
		public T Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
		}
	}
}
