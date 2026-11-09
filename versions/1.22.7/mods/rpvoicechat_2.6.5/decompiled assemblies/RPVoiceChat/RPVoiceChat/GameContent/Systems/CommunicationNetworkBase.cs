using System;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000B4 RID: 180
	public abstract class CommunicationNetworkBase
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0001EFE6 File Offset: 0x0001D1E6
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x0001EFEE File Offset: 0x0001D1EE
		public string CustomName { get; private set; } = "";

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060005B1 RID: 1457
		public abstract NetworkTransportType TransportType { get; }

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001EFF7 File Offset: 0x0001D1F7
		public void SetCustomName(string name)
		{
			this.CustomName = (name ?? "").Trim();
		}

		// Token: 0x040002E5 RID: 741
		public long networkID;
	}
}
