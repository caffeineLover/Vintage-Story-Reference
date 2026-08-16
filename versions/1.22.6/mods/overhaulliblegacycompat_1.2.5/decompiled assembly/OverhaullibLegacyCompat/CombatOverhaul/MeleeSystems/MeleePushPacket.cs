using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F2 RID: 242
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public struct MeleePushPacket
	{
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0003ECF4 File Offset: 0x0003CEF4
		// (set) Token: 0x06000C98 RID: 3224 RVA: 0x0003ECFC File Offset: 0x0003CEFC
		public MeleeCollisionPacket[] MeleeAttackDamagePackets { readonly get; set; }
	}
}
