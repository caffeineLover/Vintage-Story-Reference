using System;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000072 RID: 114
	public enum PacketType
	{
		// Token: 0x04000144 RID: 324
		SelfPing,
		// Token: 0x04000145 RID: 325
		Ping,
		// Token: 0x04000146 RID: 326
		Pong,
		// Token: 0x04000147 RID: 327
		ConnectionRequest = 10,
		// Token: 0x04000148 RID: 328
		ConnectionInfo,
		// Token: 0x04000149 RID: 329
		Audio = 20,
		// Token: 0x0400014A RID: 330
		VoiceBanStatus,
		// Token: 0x0400014B RID: 331
		Announce,
		// Token: 0x0400014C RID: 332
		NametagConfigChanged
	}
}
