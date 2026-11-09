using System;
using ProtoBuf;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000073 RID: 115
	[ProtoContract(ImplicitFields = 1)]
	public class VoiceBanStatusPacket : NetworkPacket
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00010FC9 File Offset: 0x0000F1C9
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00010FD1 File Offset: 0x0000F1D1
		public string PlayerId { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00010FDA File Offset: 0x0000F1DA
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00010FE2 File Offset: 0x0000F1E2
		public bool IsBanned { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00010FEB File Offset: 0x0000F1EB
		protected override PacketType Code
		{
			get
			{
				return PacketType.VoiceBanStatus;
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00010FEF File Offset: 0x0000F1EF
		public VoiceBanStatusPacket()
		{
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00010FF7 File Offset: 0x0000F1F7
		public VoiceBanStatusPacket(string playerId, bool isBanned)
		{
			this.PlayerId = playerId;
			this.IsBanned = isBanned;
		}
	}
}
