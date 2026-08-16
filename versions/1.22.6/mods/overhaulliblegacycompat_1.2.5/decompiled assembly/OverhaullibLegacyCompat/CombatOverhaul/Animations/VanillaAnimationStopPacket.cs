using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A2 RID: 418
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class VanillaAnimationStopPacket
	{
		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x0006520E File Offset: 0x0006340E
		// (set) Token: 0x0600143F RID: 5183 RVA: 0x00065216 File Offset: 0x00063416
		public string Code { get; set; } = "";
	}
}
