using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x0200008A RID: 138
	[ProtoContract(ImplicitFields = 1)]
	public class TelephoneSettingsPacket
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000132D3 File Offset: 0x000114D3
		// (set) Token: 0x0600038D RID: 909 RVA: 0x000132DB File Offset: 0x000114DB
		public BlockPos TelephonePos { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000132E4 File Offset: 0x000114E4
		// (set) Token: 0x0600038F RID: 911 RVA: 0x000132EC File Offset: 0x000114EC
		public TelephoneSettingsOperation Operation { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000132F5 File Offset: 0x000114F5
		// (set) Token: 0x06000391 RID: 913 RVA: 0x000132FD File Offset: 0x000114FD
		public string Value { get; set; }
	}
}
