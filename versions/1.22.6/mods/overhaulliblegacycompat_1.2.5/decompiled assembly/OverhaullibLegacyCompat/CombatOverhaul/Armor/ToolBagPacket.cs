using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000149 RID: 329
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class ToolBagPacket
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001052 RID: 4178 RVA: 0x0005062A File Offset: 0x0004E82A
		// (set) Token: 0x06001053 RID: 4179 RVA: 0x00050632 File Offset: 0x0004E832
		public string ToolBagId { get; set; } = "";

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001054 RID: 4180 RVA: 0x0005063B File Offset: 0x0004E83B
		// (set) Token: 0x06001055 RID: 4181 RVA: 0x00050643 File Offset: 0x0004E843
		public int ToolBagIndex { get; set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0005064C File Offset: 0x0004E84C
		// (set) Token: 0x06001057 RID: 4183 RVA: 0x00050654 File Offset: 0x0004E854
		public bool MainHand { get; set; } = true;

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0005065D File Offset: 0x0004E85D
		// (set) Token: 0x06001059 RID: 4185 RVA: 0x00050665 File Offset: 0x0004E865
		public int SlotIndex { get; set; }
	}
}
