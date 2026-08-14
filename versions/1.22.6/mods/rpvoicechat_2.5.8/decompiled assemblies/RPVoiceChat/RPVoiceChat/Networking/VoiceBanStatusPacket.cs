using System;
using ProtoBuf;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000056 RID: 86
	[ProtoContract(ImplicitFields = 1)]
	public class VoiceBanStatusPacket : NetworkPacket
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000D4FD File Offset: 0x0000B6FD
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000D505 File Offset: 0x0000B705
		public string PlayerId { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000D50E File Offset: 0x0000B70E
		// (set) Token: 0x0600024A RID: 586 RVA: 0x0000D516 File Offset: 0x0000B716
		public bool IsBanned { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000D51F File Offset: 0x0000B71F
		protected override PacketType Code
		{
			get
			{
				return PacketType.VoiceBanStatus;
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000D523 File Offset: 0x0000B723
		public VoiceBanStatusPacket()
		{
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000D52B File Offset: 0x0000B72B
		public VoiceBanStatusPacket(string playerId, bool isBanned)
		{
			this.PlayerId = playerId;
			this.IsBanned = isBanned;
		}
	}
}
