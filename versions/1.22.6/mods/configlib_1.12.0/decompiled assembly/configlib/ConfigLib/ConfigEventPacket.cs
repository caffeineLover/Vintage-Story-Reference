using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace ConfigLib
{
	// Token: 0x02000017 RID: 23
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	internal class ConfigEventPacket
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003FAB File Offset: 0x000021AB
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00003FB3 File Offset: 0x000021B3
		public string EventName { get; set; } = "";

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003FBC File Offset: 0x000021BC
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00003FC4 File Offset: 0x000021C4
		public byte[] Data { get; set; } = Array.Empty<byte>();
	}
}
