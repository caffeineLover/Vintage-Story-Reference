using System;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x0200008D RID: 141
	public class RadioNetwork : CommunicationNetworkBase
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x00018B45 File Offset: 0x00016D45
		public override NetworkTransportType TransportType
		{
			get
			{
				return NetworkTransportType.Radio;
			}
		}
	}
}
