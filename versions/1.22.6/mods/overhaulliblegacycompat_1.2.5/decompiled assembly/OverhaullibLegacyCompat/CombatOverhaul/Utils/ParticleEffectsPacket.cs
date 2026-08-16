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
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000C399 File Offset: 0x0000A599
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000C3A1 File Offset: 0x0000A5A1
		public string Code { readonly get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000C3AA File Offset: 0x0000A5AA
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000C3B2 File Offset: 0x0000A5B2
		public double[] Position { readonly get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000C3BB File Offset: 0x0000A5BB
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000C3C3 File Offset: 0x0000A5C3
		public float[] Velocity { readonly get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000C3CC File Offset: 0x0000A5CC
		// (set) Token: 0x060002DB RID: 731 RVA: 0x0000C3D4 File Offset: 0x0000A5D4
		public float Intensity { readonly get; set; }
	}
}
