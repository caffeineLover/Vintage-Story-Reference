using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000065 RID: 101
	[NullableContext(1)]
	[Nullable(0)]
	[AttributeUsage(64)]
	public class HotkeyEventHandlerAttribute : Attribute
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x0001D0A0 File Offset: 0x0001B2A0
		public string HotkeyCode { get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0001D0A8 File Offset: 0x0001B2A8
		public string HotkeyName { get; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0001D0B0 File Offset: 0x0001B2B0
		public GlKeys Key { get; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001D0B8 File Offset: 0x0001B2B8
		public HotkeyType HotkeyType { get; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0001D0C0 File Offset: 0x0001B2C0
		public bool Alt { get; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001D0C8 File Offset: 0x0001B2C8
		public bool Shift { get; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0001D0D0 File Offset: 0x0001B2D0
		public bool Ctrl { get; }

		// Token: 0x06000503 RID: 1283 RVA: 0x0001D0D8 File Offset: 0x0001B2D8
		public HotkeyEventHandlerAttribute(string code, string name, GlKeys key, HotkeyType type = 4, bool alt = false, bool shift = false, bool ctrl = false)
		{
			this.HotkeyCode = code;
			this.HotkeyName = name;
			this.Alt = alt;
			this.Shift = shift;
			this.Ctrl = ctrl;
			this.Key = key;
			this.HotkeyType = type;
		}
	}
}
