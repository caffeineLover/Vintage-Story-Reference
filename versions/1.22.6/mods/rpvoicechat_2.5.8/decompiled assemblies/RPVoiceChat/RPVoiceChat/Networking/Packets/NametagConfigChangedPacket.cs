using System;
using ProtoBuf;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000061 RID: 97
	[ProtoContract(ImplicitFields = 1)]
	public class NametagConfigChangedPacket : NetworkPacket
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000F641 File Offset: 0x0000D841
		// (set) Token: 0x060002BE RID: 702 RVA: 0x0000F649 File Offset: 0x0000D849
		public bool ForceSpeakerNametag { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000F652 File Offset: 0x0000D852
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x0000F65A File Offset: 0x0000D85A
		public bool PlayerNametagTargetedOnly { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000F663 File Offset: 0x0000D863
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000F66B File Offset: 0x0000D86B
		public bool UseNametagDynamicRange { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000F674 File Offset: 0x0000D874
		protected override PacketType Code
		{
			get
			{
				return PacketType.NametagConfigChanged;
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000F678 File Offset: 0x0000D878
		public NametagConfigChangedPacket()
		{
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000F680 File Offset: 0x0000D880
		public NametagConfigChangedPacket(bool forceSpeakerNametag, bool playerNametagTargetedOnly, bool useNametagDynamicRange)
		{
			this.ForceSpeakerNametag = forceSpeakerNametag;
			this.PlayerNametagTargetedOnly = playerNametagTargetedOnly;
			this.UseNametagDynamicRange = useNametagDynamicRange;
		}
	}
}
