using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003E RID: 62
	[ProtoContract(ImplicitFields = 1)]
	public class RadioRfProgramPresence
	{
		// Token: 0x040000A1 RID: 161
		public BlockPos Pos;

		// Token: 0x040000A2 RID: 162
		public int Dimension;

		// Token: 0x040000A3 RID: 163
		public long NetworkId;

		// Token: 0x040000A4 RID: 164
		public bool IsOnAir;

		// Token: 0x040000A5 RID: 165
		public string HlsStreamUrl = "";

		// Token: 0x040000A6 RID: 166
		public string ActiveOperatorPlayerUid = "";
	}
}
