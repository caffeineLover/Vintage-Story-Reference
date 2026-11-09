using System;
using ProtoBuf;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x0200007E RID: 126
	[ProtoContract(ImplicitFields = 1)]
	public class NametagConfigChangedPacket : NetworkPacket
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0001310D File Offset: 0x0001130D
		// (set) Token: 0x06000359 RID: 857 RVA: 0x00013115 File Offset: 0x00011315
		public bool ForceSpeakerNametag { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0001311E File Offset: 0x0001131E
		// (set) Token: 0x0600035B RID: 859 RVA: 0x00013126 File Offset: 0x00011326
		public bool PlayerNametagTargetedOnly { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0001312F File Offset: 0x0001132F
		// (set) Token: 0x0600035D RID: 861 RVA: 0x00013137 File Offset: 0x00011337
		public bool UseNametagDynamicRange { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600035E RID: 862 RVA: 0x00013140 File Offset: 0x00011340
		protected override PacketType Code
		{
			get
			{
				return PacketType.NametagConfigChanged;
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00013144 File Offset: 0x00011344
		public NametagConfigChangedPacket()
		{
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0001314C File Offset: 0x0001134C
		public NametagConfigChangedPacket(bool forceSpeakerNametag, bool playerNametagTargetedOnly, bool useNametagDynamicRange)
		{
			this.ForceSpeakerNametag = forceSpeakerNametag;
			this.PlayerNametagTargetedOnly = playerNametagTargetedOnly;
			this.UseNametagDynamicRange = useNametagDynamicRange;
		}
	}
}
