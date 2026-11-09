using System;

namespace WinRT
{
	// Token: 0x0200018E RID: 398
	internal struct EventRegistrationToken : IEquatable<EventRegistrationToken>
	{
		// Token: 0x06000C94 RID: 3220 RVA: 0x00023BB9 File Offset: 0x00021DB9
		public EventRegistrationToken(long _Value)
		{
			this.Value = _Value;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00023BC2 File Offset: 0x00021DC2
		public static bool operator ==(EventRegistrationToken x, EventRegistrationToken y)
		{
			return x.Value == y.Value;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00023BD2 File Offset: 0x00021DD2
		public static bool operator !=(EventRegistrationToken x, EventRegistrationToken y)
		{
			return !(x == y);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00023BDE File Offset: 0x00021DDE
		public bool Equals(EventRegistrationToken other)
		{
			return this == other;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00023BEC File Offset: 0x00021DEC
		public override bool Equals(object obj)
		{
			if (obj is EventRegistrationToken)
			{
				EventRegistrationToken y = (EventRegistrationToken)obj;
				return this == y;
			}
			return false;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00023C16 File Offset: 0x00021E16
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x04000221 RID: 545
		public long Value;
	}
}
