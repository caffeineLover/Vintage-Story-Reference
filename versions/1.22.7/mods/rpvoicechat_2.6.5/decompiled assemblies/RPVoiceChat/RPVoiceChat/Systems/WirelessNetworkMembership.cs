using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200004F RID: 79
	[ProtoContract(ImplicitFields = 1)]
	public class WirelessNetworkMembership
	{
		// Token: 0x040000C7 RID: 199
		public long NetworkId;

		// Token: 0x040000C8 RID: 200
		public List<TopologyNodeRef> Members = new List<TopologyNodeRef>();
	}
}
