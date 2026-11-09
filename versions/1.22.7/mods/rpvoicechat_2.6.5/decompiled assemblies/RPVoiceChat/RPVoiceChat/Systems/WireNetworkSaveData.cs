using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000054 RID: 84
	[ProtoContract(ImplicitFields = 1)]
	public class WireNetworkSaveData
	{
		// Token: 0x040000D7 RID: 215
		public long NetworkId;

		// Token: 0x040000D8 RID: 216
		public string CustomName = "";

		// Token: 0x040000D9 RID: 217
		public List<WireNodeRef> Nodes = new List<WireNodeRef>();
	}
}
