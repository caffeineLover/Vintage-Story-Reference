using System;

namespace CritchersCalculator
{
	// Token: 0x02000004 RID: 4
	public class FuelType
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021DC File Offset: 0x000003DC
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000021E4 File Offset: 0x000003E4
		public string Code { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000021ED File Offset: 0x000003ED
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000021F5 File Offset: 0x000003F5
		public string LangKey { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000021FE File Offset: 0x000003FE
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002206 File Offset: 0x00000406
		public int BurnTemperatureC { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000220F File Offset: 0x0000040F
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002217 File Offset: 0x00000417
		public float BurnDurationSec { get; set; }
	}
}
