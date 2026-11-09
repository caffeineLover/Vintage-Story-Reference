using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001B0 RID: 432
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class VanillaAnimationStopPacket
	{
		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x00066CB6 File Offset: 0x00064EB6
		// (set) Token: 0x060014B7 RID: 5303 RVA: 0x00066CBE File Offset: 0x00064EBE
		public string Code { get; set; } = "";
	}
}
