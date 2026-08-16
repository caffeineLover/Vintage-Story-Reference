using System;
using ProtoBuf;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000101 RID: 257
	[ProtoContract(ImplicitFields = 1)]
	public sealed class DamageStopBlockPacket
	{
		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x000404F8 File Offset: 0x0003E6F8
		// (set) Token: 0x06000CFC RID: 3324 RVA: 0x00040500 File Offset: 0x0003E700
		public bool MainHand { get; set; }
	}
}
