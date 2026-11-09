using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000082 RID: 130
	[ProtoContract(ImplicitFields = 1)]
	public class RadioSettingsPacket
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00013182 File Offset: 0x00011382
		// (set) Token: 0x06000365 RID: 869 RVA: 0x0001318A File Offset: 0x0001138A
		public BlockPos BlockPos { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00013193 File Offset: 0x00011393
		// (set) Token: 0x06000367 RID: 871 RVA: 0x0001319B File Offset: 0x0001139B
		public RadioSettingsOperation Operation { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000368 RID: 872 RVA: 0x000131A4 File Offset: 0x000113A4
		// (set) Token: 0x06000369 RID: 873 RVA: 0x000131AC File Offset: 0x000113AC
		public string Value { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600036A RID: 874 RVA: 0x000131B5 File Offset: 0x000113B5
		// (set) Token: 0x0600036B RID: 875 RVA: 0x000131BD File Offset: 0x000113BD
		public int IntValue { get; set; }
	}
}
