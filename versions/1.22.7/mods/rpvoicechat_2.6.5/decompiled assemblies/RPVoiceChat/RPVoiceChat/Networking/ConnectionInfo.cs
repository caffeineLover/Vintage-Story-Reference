using System;
using ProtoBuf;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200006F RID: 111
	[ProtoContract(ImplicitFields = 1)]
	public class ConnectionInfo : NetworkPacket
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00010E7A File Offset: 0x0000F07A
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x00010E82 File Offset: 0x0000F082
		public string Address { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x00010E8B File Offset: 0x0000F08B
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x00010E93 File Offset: 0x0000F093
		public int Port { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00010E9C File Offset: 0x0000F09C
		// (set) Token: 0x060002D4 RID: 724 RVA: 0x00010EA4 File Offset: 0x0000F0A4
		public string Transport { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00010EAD File Offset: 0x0000F0AD
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x00010EB5 File Offset: 0x0000F0B5
		public bool DevicesVoiceFeedback { get; set; } = true;

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00010EBE File Offset: 0x0000F0BE
		protected override PacketType Code
		{
			get
			{
				return PacketType.ConnectionInfo;
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00010EC2 File Offset: 0x0000F0C2
		public ConnectionInfo()
		{
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00010ED1 File Offset: 0x0000F0D1
		public ConnectionInfo(int port, string address = null)
		{
			this.Port = port;
			this.Address = address;
		}
	}
}
