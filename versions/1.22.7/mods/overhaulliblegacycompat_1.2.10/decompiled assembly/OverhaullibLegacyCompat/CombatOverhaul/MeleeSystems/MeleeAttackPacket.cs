using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000FC RID: 252
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public struct MeleeAttackPacket
	{
		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0003FCD3 File Offset: 0x0003DED3
		// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x0003FCDB File Offset: 0x0003DEDB
		public MeleeDamagePacket[] MeleeAttackDamagePackets { readonly get; set; }
	}
}
