using System;
using ProtoBuf;

namespace PetAI
{
	// Token: 0x02000024 RID: 36
	[ProtoContract(ImplicitFields = 1)]
	public class PetProfileMessage
	{
		// Token: 0x04000057 RID: 87
		public string petName;

		// Token: 0x04000058 RID: 88
		public bool multiplyAllowed;

		// Token: 0x04000059 RID: 89
		public bool abandon;

		// Token: 0x0400005A RID: 90
		public long targetEntityUID;

		// Token: 0x0400005B RID: 91
		public long oldEntityUID;
	}
}
