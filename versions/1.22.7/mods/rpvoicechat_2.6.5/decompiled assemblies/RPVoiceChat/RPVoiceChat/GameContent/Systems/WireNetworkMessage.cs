using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000BB RID: 187
	[ProtoContract(ImplicitFields = 1)]
	public class WireNetworkMessage
	{
		// Token: 0x040002F7 RID: 759
		public long NetworkUID;

		// Token: 0x040002F8 RID: 760
		public string Message;

		// Token: 0x040002F9 RID: 761
		public BlockPos SenderPos;

		// Token: 0x040002FA RID: 762
		public string SenderPlayerUID;

		// Token: 0x040002FB RID: 763
		public WireRouteMode RouteMode;

		// Token: 0x040002FC RID: 764
		public string TargetEndpointName;

		// Token: 0x040002FD RID: 765
		public BlockPos TargetPos;
	}
}
