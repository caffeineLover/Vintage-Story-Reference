using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003F RID: 63
	[ProtoContract(ImplicitFields = 1)]
	public class RadioRfPresenceData
	{
		// Token: 0x040000A7 RID: 167
		public List<RadioRfEmitterPresence> Emitters = new List<RadioRfEmitterPresence>();

		// Token: 0x040000A8 RID: 168
		public List<RadioRfReceiverPresence> Receivers = new List<RadioRfReceiverPresence>();

		// Token: 0x040000A9 RID: 169
		public List<RadioRfProgramPresence> Programs = new List<RadioRfProgramPresence>();
	}
}
