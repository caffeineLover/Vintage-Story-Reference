using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x0200007D RID: 125
	[ProtoContract(ImplicitFields = 1)]
	public class CommDeliveryPacket
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0001308E File Offset: 0x0001128E
		// (set) Token: 0x0600034A RID: 842 RVA: 0x00013096 File Offset: 0x00011296
		public BlockPos DevicePos { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0001309F File Offset: 0x0001129F
		// (set) Token: 0x0600034C RID: 844 RVA: 0x000130A7 File Offset: 0x000112A7
		public string SourceEndpointName { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600034D RID: 845 RVA: 0x000130B0 File Offset: 0x000112B0
		// (set) Token: 0x0600034E RID: 846 RVA: 0x000130B8 File Offset: 0x000112B8
		public string TargetEndpointName { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600034F RID: 847 RVA: 0x000130C1 File Offset: 0x000112C1
		// (set) Token: 0x06000350 RID: 848 RVA: 0x000130C9 File Offset: 0x000112C9
		public string NetworkName { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000351 RID: 849 RVA: 0x000130D2 File Offset: 0x000112D2
		// (set) Token: 0x06000352 RID: 850 RVA: 0x000130DA File Offset: 0x000112DA
		public string TextMessage { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000353 RID: 851 RVA: 0x000130E3 File Offset: 0x000112E3
		// (set) Token: 0x06000354 RID: 852 RVA: 0x000130EB File Offset: 0x000112EB
		public byte[] VoicePayload { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000355 RID: 853 RVA: 0x000130F4 File Offset: 0x000112F4
		// (set) Token: 0x06000356 RID: 854 RVA: 0x000130FC File Offset: 0x000112FC
		public CommPayloadType PayloadType { get; set; }
	}
}
