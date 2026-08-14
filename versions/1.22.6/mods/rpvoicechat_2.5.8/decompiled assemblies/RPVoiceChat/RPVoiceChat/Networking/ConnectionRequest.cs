using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000053 RID: 83
	[ProtoContract(ImplicitFields = 1)]
	public class ConnectionRequest : NetworkPacket
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000D422 File Offset: 0x0000B622
		public ConnectionInfo[] SupportedTransports { get; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000D42A File Offset: 0x0000B62A
		protected override PacketType Code
		{
			get
			{
				return PacketType.ConnectionRequest;
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000D42E File Offset: 0x0000B62E
		public ConnectionRequest()
		{
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000D436 File Offset: 0x0000B636
		public ConnectionRequest(List<ConnectionInfo> serverConnectionInfos)
		{
			this.SupportedTransports = serverConnectionInfos.ToArray();
		}
	}
}
