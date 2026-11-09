using System;
using System.Runtime.CompilerServices;

namespace Firearms
{
	// Token: 0x02000010 RID: 16
	[NullableContext(1)]
	[Nullable(0)]
	public class PowderFlaskStats
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000092 RID: 146 RVA: 0x000043C3 File Offset: 0x000025C3
		// (set) Token: 0x06000093 RID: 147 RVA: 0x000043CB File Offset: 0x000025CB
		public float DurabilityPerPowderItem { get; set; } = 8f;

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000043D4 File Offset: 0x000025D4
		// (set) Token: 0x06000095 RID: 149 RVA: 0x000043DC File Offset: 0x000025DC
		public string PowderWildcard { get; set; } = "*gunpowder-fine";

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000043E5 File Offset: 0x000025E5
		// (set) Token: 0x06000097 RID: 151 RVA: 0x000043ED File Offset: 0x000025ED
		public string RefillSound { get; set; } = "maltiezfirearms:sounds/powder/powder-prime";
	}
}
