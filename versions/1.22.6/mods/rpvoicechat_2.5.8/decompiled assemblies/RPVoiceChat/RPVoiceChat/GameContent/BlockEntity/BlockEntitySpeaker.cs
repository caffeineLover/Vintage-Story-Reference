using System;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.Systems;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000B5 RID: 181
	public class BlockEntitySpeaker : BEWireNode, IWireTypedNode, ITelephoneVoiceEndpoint
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0001E8F6 File Offset: 0x0001CAF6
		protected override int MaxConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x0001E8F9 File Offset: 0x0001CAF9
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0001E8FC File Offset: 0x0001CAFC
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.Telephone;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0001E8FF File Offset: 0x0001CAFF
		public int VoiceEmissionRangeBlocks
		{
			get
			{
				return ServerConfigManager.SpeakerAudibleDistance;
			}
		}
	}
}
