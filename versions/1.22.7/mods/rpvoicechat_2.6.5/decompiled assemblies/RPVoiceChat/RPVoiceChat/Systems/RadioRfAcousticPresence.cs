using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003C RID: 60
	[ProtoContract(ImplicitFields = 1)]
	public class RadioRfAcousticPresence
	{
		// Token: 0x04000098 RID: 152
		public BlockPos Pos;

		// Token: 0x04000099 RID: 153
		public int Dimension;

		// Token: 0x0400009A RID: 154
		public int RangeBlocks;
	}
}
