using System;

namespace RPVoiceChat.GameContent.Systems
{
	// Token: 0x020000C5 RID: 197
	public interface IRadioProgramSource
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060005F4 RID: 1524
		bool IsOnAir { get; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060005F5 RID: 1525
		string HlsStreamUrl { get; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060005F6 RID: 1526
		string ActiveOperatorPlayerUid { get; }
	}
}
