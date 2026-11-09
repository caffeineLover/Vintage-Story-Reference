using System;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000B6 RID: 182
	public class RadioNetwork : CommunicationNetworkBase
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x0001F021 File Offset: 0x0001D221
		public override NetworkTransportType TransportType
		{
			get
			{
				return NetworkTransportType.Radio;
			}
		}
	}
}
