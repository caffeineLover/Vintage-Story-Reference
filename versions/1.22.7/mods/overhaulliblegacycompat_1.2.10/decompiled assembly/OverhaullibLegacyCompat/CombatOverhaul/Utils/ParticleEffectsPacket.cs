using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000029 RID: 41
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public struct ParticleEffectsPacket
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000C3D9 File Offset: 0x0000A5D9
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000C3E1 File Offset: 0x0000A5E1
		public string Code { readonly get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000C3EA File Offset: 0x0000A5EA
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000C3F2 File Offset: 0x0000A5F2
		public double[] Position { readonly get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000C3FB File Offset: 0x0000A5FB
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000C403 File Offset: 0x0000A603
		public float[] Velocity { readonly get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000C40C File Offset: 0x0000A60C
		// (set) Token: 0x060002DB RID: 731 RVA: 0x0000C414 File Offset: 0x0000A614
		public float Intensity { readonly get; set; }
	}
}
