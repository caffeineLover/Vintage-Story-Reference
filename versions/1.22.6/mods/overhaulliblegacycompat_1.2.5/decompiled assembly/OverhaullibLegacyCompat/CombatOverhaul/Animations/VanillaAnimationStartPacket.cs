using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A1 RID: 417
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class VanillaAnimationStartPacket
	{
		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x000651EA File Offset: 0x000633EA
		// (set) Token: 0x0600143C RID: 5180 RVA: 0x000651F2 File Offset: 0x000633F2
		public string Code { get; set; } = "";
	}
}
