using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000086 RID: 134
	[ProtoContract(ImplicitFields = 1)]
	public class SwitchboardRenameNetworkPacket
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0001326E File Offset: 0x0001146E
		// (set) Token: 0x06000381 RID: 897 RVA: 0x00013276 File Offset: 0x00011476
		public BlockPos SwitchboardPos { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0001327F File Offset: 0x0001147F
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00013287 File Offset: 0x00011487
		public string NetworkName { get; set; }
	}
}
