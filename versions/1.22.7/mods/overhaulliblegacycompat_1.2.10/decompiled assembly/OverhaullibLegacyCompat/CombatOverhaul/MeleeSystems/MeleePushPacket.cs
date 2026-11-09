using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000FD RID: 253
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public struct MeleePushPacket
	{
		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0003FCE4 File Offset: 0x0003DEE4
		// (set) Token: 0x06000CF4 RID: 3316 RVA: 0x0003FCEC File Offset: 0x0003DEEC
		public MeleeCollisionPacket[] MeleeAttackDamagePackets { readonly get; set; }
	}
}
