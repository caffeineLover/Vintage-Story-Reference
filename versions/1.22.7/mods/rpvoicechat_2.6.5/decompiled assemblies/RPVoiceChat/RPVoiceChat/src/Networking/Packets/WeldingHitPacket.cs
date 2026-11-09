using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.src.Networking.Packets
{
	// Token: 0x02000063 RID: 99
	[ProtoContract(ImplicitFields = 1)]
	public class WeldingHitPacket
	{
		// Token: 0x0400010D RID: 269
		public BlockPos Pos;

		// Token: 0x0400010E RID: 270
		public Vec3d HitPosition;
	}
}
