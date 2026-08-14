using System;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x0200008B RID: 139
	public abstract class CommunicationNetworkBase
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00018B0A File Offset: 0x00016D0A
		// (set) Token: 0x06000495 RID: 1173 RVA: 0x00018B12 File Offset: 0x00016D12
		public string CustomName { get; private set; } = "";

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000496 RID: 1174
		public abstract NetworkTransportType TransportType { get; }

		// Token: 0x06000497 RID: 1175 RVA: 0x00018B1B File Offset: 0x00016D1B
		public void SetCustomName(string name)
		{
			this.CustomName = (name ?? "").Trim();
		}

		// Token: 0x04000243 RID: 579
		public long networkID;
	}
}
