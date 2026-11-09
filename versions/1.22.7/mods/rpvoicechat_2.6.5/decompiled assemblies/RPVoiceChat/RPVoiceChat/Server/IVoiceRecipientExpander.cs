using System;
using System.Collections.Generic;
using RPVoiceChat.Networking;

namespace RPVoiceChat.Server
{
	// Token: 0x02000061 RID: 97
	public interface IVoiceRecipientExpander
	{
		// Token: 0x06000271 RID: 625
		void ExpandRoutedRecipients(AudioPacket packet, IReadOnlyList<VoiceRoute> routes, Dictionary<string, RoutedVoiceRecipient> recipients);
	}
}
