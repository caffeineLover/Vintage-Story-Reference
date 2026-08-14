using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.src.Networking.Packets
{
	// Token: 0x02000046 RID: 70
	[ProtoContract(ImplicitFields = 1)]
	public class WeldingHitPacket
	{
		// Token: 0x040000BB RID: 187
		public BlockPos Pos;

		// Token: 0x040000BC RID: 188
		public Vec3d HitPosition;
	}
}
