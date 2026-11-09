using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace ConfigLib
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	internal class ConfigEventPacket
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000042CF File Offset: 0x000024CF
		// (set) Token: 0x06000099 RID: 153 RVA: 0x000042D7 File Offset: 0x000024D7
		public string EventName { get; set; } = "";

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000042E0 File Offset: 0x000024E0
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000042E8 File Offset: 0x000024E8
		public byte[] Data { get; set; } = Array.Empty<byte>();
	}
}
