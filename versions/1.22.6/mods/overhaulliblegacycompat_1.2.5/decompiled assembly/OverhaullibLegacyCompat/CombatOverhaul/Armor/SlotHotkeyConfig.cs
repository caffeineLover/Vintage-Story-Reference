using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000143 RID: 323
	[NullableContext(1)]
	[Nullable(0)]
	public class SlotHotkeyConfig
	{
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x0004EA1C File Offset: 0x0004CC1C
		// (set) Token: 0x06000FF1 RID: 4081 RVA: 0x0004EA24 File Offset: 0x0004CC24
		public string HotkeyCode { get; set; } = "";

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x0004EA2D File Offset: 0x0004CC2D
		// (set) Token: 0x06000FF3 RID: 4083 RVA: 0x0004EA35 File Offset: 0x0004CC35
		public string HotkeyName { get; set; } = "";

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x0004EA3E File Offset: 0x0004CC3E
		// (set) Token: 0x06000FF5 RID: 4085 RVA: 0x0004EA46 File Offset: 0x0004CC46
		public GlKeys HotkeyKey { get; set; } = 100;

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x0004EA4F File Offset: 0x0004CC4F
		// (set) Token: 0x06000FF7 RID: 4087 RVA: 0x0004EA57 File Offset: 0x0004CC57
		public string RequiredTypeCode { get; set; } = "";

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x0004EA60 File Offset: 0x0004CC60
		// (set) Token: 0x06000FF9 RID: 4089 RVA: 0x0004EA68 File Offset: 0x0004CC68
		public string RequiredTypeValue { get; set; } = "";
	}
}
