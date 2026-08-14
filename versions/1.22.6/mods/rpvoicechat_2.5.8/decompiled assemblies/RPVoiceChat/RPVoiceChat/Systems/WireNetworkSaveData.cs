using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003A RID: 58
	[ProtoContract(ImplicitFields = 1)]
	public class WireNetworkSaveData
	{
		// Token: 0x0400008C RID: 140
		public long NetworkId;

		// Token: 0x0400008D RID: 141
		public string CustomName = "";

		// Token: 0x0400008E RID: 142
		public List<WireNodeRef> Nodes = new List<WireNodeRef>();
	}
}
