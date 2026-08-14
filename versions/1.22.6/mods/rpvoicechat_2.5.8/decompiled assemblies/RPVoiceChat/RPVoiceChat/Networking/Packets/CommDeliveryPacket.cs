using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000060 RID: 96
	[ProtoContract(ImplicitFields = 1)]
	public class CommDeliveryPacket
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000F5C2 File Offset: 0x0000D7C2
		// (set) Token: 0x060002AF RID: 687 RVA: 0x0000F5CA File Offset: 0x0000D7CA
		public BlockPos DevicePos { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000F5D3 File Offset: 0x0000D7D3
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000F5DB File Offset: 0x0000D7DB
		public string SourceEndpointName { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000F5E4 File Offset: 0x0000D7E4
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x0000F5EC File Offset: 0x0000D7EC
		public string TargetEndpointName { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000F5F5 File Offset: 0x0000D7F5
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x0000F5FD File Offset: 0x0000D7FD
		public string NetworkName { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000F606 File Offset: 0x0000D806
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x0000F60E File Offset: 0x0000D80E
		public string TextMessage { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000F617 File Offset: 0x0000D817
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x0000F61F File Offset: 0x0000D81F
		public byte[] VoicePayload { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000F628 File Offset: 0x0000D828
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000F630 File Offset: 0x0000D830
		public CommPayloadType PayloadType { get; set; }
	}
}
