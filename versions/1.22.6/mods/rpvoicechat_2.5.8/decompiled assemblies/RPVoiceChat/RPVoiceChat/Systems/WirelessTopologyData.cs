using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000036 RID: 54
	[ProtoContract(ImplicitFields = 1)]
	public class WirelessTopologyData
	{
		// Token: 0x0400007E RID: 126
		public CommunicationTopologyData Links = new CommunicationTopologyData();

		// Token: 0x0400007F RID: 127
		public List<WirelessNetworkMembership> Networks = new List<WirelessNetworkMembership>();
	}
}
