using System;
using ProtoBuf;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010B RID: 267
	[ProtoContract(ImplicitFields = 1)]
	public sealed class DamageBlockCallbackPacket
	{
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x000414CF File Offset: 0x0003F6CF
		// (set) Token: 0x06000D55 RID: 3413 RVA: 0x000414D7 File Offset: 0x0003F6D7
		public ulong Id { get; set; }
	}
}
