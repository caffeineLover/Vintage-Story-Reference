using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200002F RID: 47
	[ProtoContract(ImplicitFields = 1)]
	public class CommunicationTopologyData
	{
		// Token: 0x04000078 RID: 120
		public List<TopologyLink> Links = new List<TopologyLink>();
	}
}
