using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000050 RID: 80
	[ProtoContract(ImplicitFields = 1)]
	public class WirelessTopologyData
	{
		// Token: 0x040000C9 RID: 201
		public CommunicationTopologyData Links = new CommunicationTopologyData();

		// Token: 0x040000CA RID: 202
		public List<WirelessNetworkMembership> Networks = new List<WirelessNetworkMembership>();
	}
}
