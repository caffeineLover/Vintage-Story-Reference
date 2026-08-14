using System;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000055 RID: 85
	public enum PacketType
	{
		// Token: 0x040000F2 RID: 242
		SelfPing,
		// Token: 0x040000F3 RID: 243
		Ping,
		// Token: 0x040000F4 RID: 244
		Pong,
		// Token: 0x040000F5 RID: 245
		ConnectionRequest = 10,
		// Token: 0x040000F6 RID: 246
		ConnectionInfo,
		// Token: 0x040000F7 RID: 247
		Audio = 20,
		// Token: 0x040000F8 RID: 248
		VoiceBanStatus,
		// Token: 0x040000F9 RID: 249
		Announce,
		// Token: 0x040000FA RID: 250
		NametagConfigChanged
	}
}
