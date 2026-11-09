using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000058 RID: 88
	[ProtoContract(ImplicitFields = 1)]
	public class WireTopologyData
	{
		// Token: 0x040000DF RID: 223
		public List<WireEdge> Edges = new List<WireEdge>();
	}
}
