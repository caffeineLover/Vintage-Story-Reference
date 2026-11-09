using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003B RID: 59
	[ProtoContract(ImplicitFields = 1)]
	public class RadioRfEmitterPresence
	{
		// Token: 0x04000091 RID: 145
		public BlockPos Pos;

		// Token: 0x04000092 RID: 146
		public int Dimension;

		// Token: 0x04000093 RID: 147
		public long NetworkId;

		// Token: 0x04000094 RID: 148
		public bool IsRepeater;

		// Token: 0x04000095 RID: 149
		public string Frequency = "";

		// Token: 0x04000096 RID: 150
		public int RangeBlocks;

		// Token: 0x04000097 RID: 151
		public bool IsActive;
	}
}
