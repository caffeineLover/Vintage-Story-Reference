using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000067 RID: 103
	[ProtoContract(ImplicitFields = 1)]
	public class TelephoneSettingsPacket
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000F72C File Offset: 0x0000D92C
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x0000F734 File Offset: 0x0000D934
		public BlockPos TelephonePos { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000F73D File Offset: 0x0000D93D
		// (set) Token: 0x060002DA RID: 730 RVA: 0x0000F745 File Offset: 0x0000D945
		public TelephoneSettingsOperation Operation { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000F74E File Offset: 0x0000D94E
		// (set) Token: 0x060002DC RID: 732 RVA: 0x0000F756 File Offset: 0x0000D956
		public string Value { get; set; }
	}
}
