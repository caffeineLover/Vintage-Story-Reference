using System;
using ProtoBuf;

namespace CombatOverhaul.Vanity
{
	// Token: 0x020000B6 RID: 182
	[ProtoContract(ImplicitFields = 1)]
	public sealed class BackpackVanityPacket
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x00033070 File Offset: 0x00031270
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x00033078 File Offset: 0x00031278
		public bool Hide { get; set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x00033081 File Offset: 0x00031281
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x00033089 File Offset: 0x00031289
		public long PlayerEntityId { get; set; }
	}
}
