using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul
{
	// Token: 0x02000014 RID: 20
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class TogglePacket
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000470B File Offset: 0x0000290B
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00004713 File Offset: 0x00002913
		public string HotKeyCode { get; set; } = "";
	}
}
