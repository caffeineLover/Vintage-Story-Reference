using System;
using ProtoBuf;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000052 RID: 82
	[ProtoContract(ImplicitFields = 1)]
	public class ConnectionInfo : NetworkPacket
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000D3AE File Offset: 0x0000B5AE
		// (set) Token: 0x06000235 RID: 565 RVA: 0x0000D3B6 File Offset: 0x0000B5B6
		public string Address { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000D3BF File Offset: 0x0000B5BF
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0000D3C7 File Offset: 0x0000B5C7
		public int Port { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000D3D0 File Offset: 0x0000B5D0
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		public string Transport { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000D3E1 File Offset: 0x0000B5E1
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000D3E9 File Offset: 0x0000B5E9
		public bool DevicesVoiceFeedback { get; set; } = true;

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000D3F2 File Offset: 0x0000B5F2
		protected override PacketType Code
		{
			get
			{
				return PacketType.ConnectionInfo;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000D3F6 File Offset: 0x0000B5F6
		public ConnectionInfo()
		{
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000D405 File Offset: 0x0000B605
		public ConnectionInfo(int port, string address = null)
		{
			this.Port = port;
			this.Address = address;
		}
	}
}
