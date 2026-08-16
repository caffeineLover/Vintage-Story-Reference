using System;
using ProtoBuf;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000036 RID: 54
	[ProtoContract(ImplicitFields = 1)]
	public sealed class CriticalHitFeedbackPacket
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00011336 File Offset: 0x0000F536
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0001133E File Offset: 0x0000F53E
		public float Intensity { get; set; } = 1f;
	}
}
