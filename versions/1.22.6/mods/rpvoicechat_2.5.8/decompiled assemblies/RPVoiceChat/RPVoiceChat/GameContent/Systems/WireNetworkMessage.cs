using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x02000092 RID: 146
	[ProtoContract(ImplicitFields = 1)]
	public class WireNetworkMessage
	{
		// Token: 0x04000255 RID: 597
		public long NetworkUID;

		// Token: 0x04000256 RID: 598
		public string Message;

		// Token: 0x04000257 RID: 599
		public BlockPos SenderPos;

		// Token: 0x04000258 RID: 600
		public string SenderPlayerUID;

		// Token: 0x04000259 RID: 601
		public WireRouteMode RouteMode;

		// Token: 0x0400025A RID: 602
		public string TargetEndpointName;

		// Token: 0x0400025B RID: 603
		public BlockPos TargetPos;
	}
}
