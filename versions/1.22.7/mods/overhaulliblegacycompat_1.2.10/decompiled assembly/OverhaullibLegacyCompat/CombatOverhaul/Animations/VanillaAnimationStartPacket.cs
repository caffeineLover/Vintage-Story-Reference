using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001AF RID: 431
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class VanillaAnimationStartPacket
	{
		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x00066C92 File Offset: 0x00064E92
		// (set) Token: 0x060014B4 RID: 5300 RVA: 0x00066C9A File Offset: 0x00064E9A
		public string Code { get; set; } = "";
	}
}
