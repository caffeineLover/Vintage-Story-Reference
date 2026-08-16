using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000E7 RID: 231
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class ImpaleThrowRequestPacket
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0003C7FD File Offset: 0x0003A9FD
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0003C805 File Offset: 0x0003AA05
		public bool MainHand { get; set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0003C80E File Offset: 0x0003AA0E
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x0003C816 File Offset: 0x0003AA16
		public float[] Direction { get; set; } = Array.Empty<float>();
	}
}
