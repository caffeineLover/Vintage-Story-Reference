using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F2 RID: 242
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class ImpaleThrowRequestPacket
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0003D7ED File Offset: 0x0003B9ED
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x0003D7F5 File Offset: 0x0003B9F5
		public bool MainHand { get; set; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0003D7FE File Offset: 0x0003B9FE
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x0003D806 File Offset: 0x0003BA06
		public float[] Direction { get; set; } = Array.Empty<float>();
	}
}
