using System;
using ProtoBuf;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000083 RID: 131
	[ProtoContract(ImplicitFields = 1)]
	public class RadioTalkieSettingsPacket
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600036D RID: 877 RVA: 0x000131CE File Offset: 0x000113CE
		// (set) Token: 0x0600036E RID: 878 RVA: 0x000131D6 File Offset: 0x000113D6
		public string Frequency { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600036F RID: 879 RVA: 0x000131DF File Offset: 0x000113DF
		// (set) Token: 0x06000370 RID: 880 RVA: 0x000131E7 File Offset: 0x000113E7
		public bool InventoryListen { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000371 RID: 881 RVA: 0x000131F0 File Offset: 0x000113F0
		// (set) Token: 0x06000372 RID: 882 RVA: 0x000131F8 File Offset: 0x000113F8
		public int SlotNumber { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00013201 File Offset: 0x00011401
		// (set) Token: 0x06000374 RID: 884 RVA: 0x00013209 File Offset: 0x00011409
		public int ListenVolumePercent { get; set; }
	}
}
