using System;
using ProtoBuf;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000100 RID: 256
	[ProtoContract(ImplicitFields = 1)]
	public sealed class DamageBlockCallbackPacket
	{
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x000404DF File Offset: 0x0003E6DF
		// (set) Token: 0x06000CF9 RID: 3321 RVA: 0x000404E7 File Offset: 0x0003E6E7
		public ulong Id { get; set; }
	}
}
