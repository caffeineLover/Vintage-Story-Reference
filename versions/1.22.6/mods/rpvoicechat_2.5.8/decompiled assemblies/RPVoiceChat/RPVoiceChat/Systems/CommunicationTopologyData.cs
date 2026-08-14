using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200002E RID: 46
	[ProtoContract(ImplicitFields = 1)]
	public class CommunicationTopologyData
	{
		// Token: 0x0400006A RID: 106
		public List<TopologyLink> Links = new List<TopologyLink>();
	}
}
