using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace BetterErProspecting.Tracking
{
	// Token: 0x02000004 RID: 4
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract]
	public class PptDataPacket
	{
		// Token: 0x06000012 RID: 18 RVA: 0x0000243A File Offset: 0x0000063A
		public PptDataPacket(Dictionary<string, PptData> data)
		{
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002449 File Offset: 0x00000649
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002451 File Offset: 0x00000651
		[ProtoMember(1)]
		public Dictionary<string, PptData> codeToData { get; set; } = data;

		// Token: 0x06000015 RID: 21 RVA: 0x0000245A File Offset: 0x0000065A
		public PptDataPacket() : this(new Dictionary<string, PptData>())
		{
		}
	}
}
