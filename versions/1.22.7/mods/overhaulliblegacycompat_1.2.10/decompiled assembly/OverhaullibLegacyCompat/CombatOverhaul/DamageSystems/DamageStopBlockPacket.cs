using System;
using ProtoBuf;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010C RID: 268
	[ProtoContract(ImplicitFields = 1)]
	public sealed class DamageStopBlockPacket
	{
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x000414E8 File Offset: 0x0003F6E8
		// (set) Token: 0x06000D58 RID: 3416 RVA: 0x000414F0 File Offset: 0x0003F6F0
		public bool MainHand { get; set; }
	}
}
