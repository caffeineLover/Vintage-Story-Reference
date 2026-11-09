using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul
{
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class StatsPacket
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003939 File Offset: 0x00001B39
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00003941 File Offset: 0x00001B41
		public string Stat { get; set; } = "";

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008F RID: 143 RVA: 0x0000394A File Offset: 0x00001B4A
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003952 File Offset: 0x00001B52
		public string Category { get; set; } = "";

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000091 RID: 145 RVA: 0x0000395B File Offset: 0x00001B5B
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00003963 File Offset: 0x00001B63
		public float Value { get; set; }
	}
}
