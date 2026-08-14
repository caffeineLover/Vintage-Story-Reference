using System;
using ProtoBuf;

namespace BetterErProspecting.Tracking
{
	// Token: 0x02000005 RID: 5
	[ProtoContract]
	public sealed class CreateBoreholeWpPacket
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002467 File Offset: 0x00000667
		// (set) Token: 0x06000017 RID: 23 RVA: 0x0000246F File Offset: 0x0000066F
		[ProtoMember(1)]
		public int hash { get; set; }
	}
}
