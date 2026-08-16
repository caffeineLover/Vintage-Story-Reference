using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Vanity
{
	// Token: 0x020000B5 RID: 181
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class VanityPacket
	{
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x00033003 File Offset: 0x00031203
		// (set) Token: 0x06000975 RID: 2421 RVA: 0x0003300B File Offset: 0x0003120B
		public long PlayerEntityId { get; set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x00033014 File Offset: 0x00031214
		// (set) Token: 0x06000977 RID: 2423 RVA: 0x0003301C File Offset: 0x0003121C
		public byte[] HiddenSlots { get; set; } = Array.Empty<byte>();
	}
}
