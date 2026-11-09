using System;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging.Messages
{
	// Token: 0x02000017 RID: 23
	[NullableContext(1)]
	[Nullable(0)]
	public class PropertyChangedMessage<[Nullable(2)] T>
	{
		// Token: 0x0600008D RID: 141 RVA: 0x0000441C File Offset: 0x0000261C
		public PropertyChangedMessage(object sender, [Nullable(2)] string propertyName, T oldValue, T newValue)
		{
			ArgumentNullException.ThrowIfNull(sender, "sender");
			this.<Sender>k__BackingField = sender;
			this.<PropertyName>k__BackingField = propertyName;
			this.<OldValue>k__BackingField = oldValue;
			this.<NewValue>k__BackingField = newValue;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000444C File Offset: 0x0000264C
		public object Sender
		{
			[CompilerGenerated]
			get
			{
				return this.<Sender>k__BackingField;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00004454 File Offset: 0x00002654
		[Nullable(2)]
		public string PropertyName
		{
			[NullableContext(2)]
			[CompilerGenerated]
			get
			{
				return this.<PropertyName>k__BackingField;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000445C File Offset: 0x0000265C
		public T OldValue
		{
			[CompilerGenerated]
			get
			{
				return this.<OldValue>k__BackingField;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004464 File Offset: 0x00002664
		public T NewValue
		{
			[CompilerGenerated]
			get
			{
				return this.<NewValue>k__BackingField;
			}
		}
	}
}
