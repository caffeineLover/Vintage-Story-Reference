using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000155 RID: 341
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class ToolBagPacket
	{
		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x00051F32 File Offset: 0x00050132
		// (set) Token: 0x060010C7 RID: 4295 RVA: 0x00051F3A File Offset: 0x0005013A
		public string ToolBagId { get; set; } = "";

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x00051F43 File Offset: 0x00050143
		// (set) Token: 0x060010C9 RID: 4297 RVA: 0x00051F4B File Offset: 0x0005014B
		public int ToolBagIndex { get; set; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x00051F54 File Offset: 0x00050154
		// (set) Token: 0x060010CB RID: 4299 RVA: 0x00051F5C File Offset: 0x0005015C
		public bool MainHand { get; set; } = true;

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x00051F65 File Offset: 0x00050165
		// (set) Token: 0x060010CD RID: 4301 RVA: 0x00051F6D File Offset: 0x0005016D
		public int SlotIndex { get; set; }
	}
}
