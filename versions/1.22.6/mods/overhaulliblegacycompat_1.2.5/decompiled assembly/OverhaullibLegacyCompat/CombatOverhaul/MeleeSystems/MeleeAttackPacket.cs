using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F1 RID: 241
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public struct MeleeAttackPacket
	{
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0003ECE3 File Offset: 0x0003CEE3
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x0003ECEB File Offset: 0x0003CEEB
		public MeleeDamagePacket[] MeleeAttackDamagePackets { readonly get; set; }
	}
}
