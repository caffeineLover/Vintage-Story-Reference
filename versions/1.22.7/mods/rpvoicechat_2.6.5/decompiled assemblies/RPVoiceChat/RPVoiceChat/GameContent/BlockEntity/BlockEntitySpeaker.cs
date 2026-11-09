using System;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.Systems;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000EE RID: 238
	public class BlockEntitySpeaker : BEWireNode, IWireTypedNode, ITelephoneVoiceEndpoint
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00029B16 File Offset: 0x00027D16
		protected override int MaxConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x00029B19 File Offset: 0x00027D19
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x00029B1C File Offset: 0x00027D1C
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.Infrastructure;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00029B1F File Offset: 0x00027D1F
		public int VoiceEmissionRangeBlocks
		{
			get
			{
				return ServerConfigManager.SpeakerAudibleDistance;
			}
		}
	}
}
