using System;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Messaging.Internals
{
	// Token: 0x0200001C RID: 28
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct Type2 : IEquatable<Type2>
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x0000466B File Offset: 0x0000286B
		[MethodImpl(256)]
		public Type2(Type tMessage, Type tToken)
		{
			this.TMessage = tMessage;
			this.TToken = tToken;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000467B File Offset: 0x0000287B
		[MethodImpl(256)]
		public bool Equals(Type2 other)
		{
			return this.TMessage == other.TMessage && this.TToken == other.TToken;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000046A4 File Offset: 0x000028A4
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is Type2)
			{
				Type2 other = (Type2)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000046CC File Offset: 0x000028CC
		[MethodImpl(256)]
		public override int GetHashCode()
		{
			int num = this.TMessage.GetHashCode();
			num = (num << 5) + num;
			return num + this.TToken.GetHashCode();
		}

		// Token: 0x04000038 RID: 56
		public readonly Type TMessage;

		// Token: 0x04000039 RID: 57
		public readonly Type TToken;
	}
}
