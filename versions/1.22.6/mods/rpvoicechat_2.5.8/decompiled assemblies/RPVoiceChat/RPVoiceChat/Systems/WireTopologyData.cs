using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003D RID: 61
	[ProtoContract(ImplicitFields = 1)]
	public class WireTopologyData
	{
		// Token: 0x04000094 RID: 148
		public List<WireEdge> Edges = new List<WireEdge>();
	}
}
