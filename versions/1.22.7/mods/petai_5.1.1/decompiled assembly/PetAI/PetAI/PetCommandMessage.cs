using System;
using ProtoBuf;

namespace PetAI
{
	// Token: 0x02000023 RID: 35
	[ProtoContract(ImplicitFields = 1)]
	public class PetCommandMessage
	{
		// Token: 0x04000053 RID: 83
		public string playerUID;

		// Token: 0x04000054 RID: 84
		public string commandName;

		// Token: 0x04000055 RID: 85
		public string commandType;

		// Token: 0x04000056 RID: 86
		public long targetEntityUID;
	}
}
