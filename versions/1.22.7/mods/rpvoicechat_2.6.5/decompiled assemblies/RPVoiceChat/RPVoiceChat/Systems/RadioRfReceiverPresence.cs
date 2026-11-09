using System;
using System.Collections.Generic;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003D RID: 61
	[ProtoContract(ImplicitFields = 1)]
	public class RadioRfReceiverPresence
	{
		// Token: 0x0400009B RID: 155
		public BlockPos Pos;

		// Token: 0x0400009C RID: 156
		public int Dimension;

		// Token: 0x0400009D RID: 157
		public string TunedFrequency = "";

		// Token: 0x0400009E RID: 158
		public int PlaybackRangeBlocks;

		// Token: 0x0400009F RID: 159
		public bool IsEnabled;

		// Token: 0x040000A0 RID: 160
		public List<RadioRfAcousticPresence> AcousticPoints = new List<RadioRfAcousticPresence>();
	}
}
