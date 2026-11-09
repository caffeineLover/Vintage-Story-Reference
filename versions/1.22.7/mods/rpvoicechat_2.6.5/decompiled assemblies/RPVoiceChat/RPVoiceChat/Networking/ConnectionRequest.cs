using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000070 RID: 112
	[ProtoContract(ImplicitFields = 1)]
	public class ConnectionRequest : NetworkPacket
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00010EEE File Offset: 0x0000F0EE
		public ConnectionInfo[] SupportedTransports { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002DB RID: 731 RVA: 0x00010EF6 File Offset: 0x0000F0F6
		protected override PacketType Code
		{
			get
			{
				return PacketType.ConnectionRequest;
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00010EFA File Offset: 0x0000F0FA
		public ConnectionRequest()
		{
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00010F02 File Offset: 0x0000F102
		public ConnectionRequest(List<ConnectionInfo> serverConnectionInfos)
		{
			this.SupportedTransports = serverConnectionInfos.ToArray();
		}
	}
}
