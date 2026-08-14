using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000035 RID: 53
	[ProtoContract(ImplicitFields = 1)]
	public class WirelessNetworkMembership
	{
		// Token: 0x0400007C RID: 124
		public long NetworkId;

		// Token: 0x0400007D RID: 125
		public List<TopologyNodeRef> Members = new List<TopologyNodeRef>();
	}
}
