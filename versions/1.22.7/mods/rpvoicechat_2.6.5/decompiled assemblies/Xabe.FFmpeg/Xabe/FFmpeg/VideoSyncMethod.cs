using System;

namespace Xabe.FFmpeg
{
	// Token: 0x0200000F RID: 15
	public enum VideoSyncMethod
	{
		// Token: 0x04000267 RID: 615
		passthrough,
		// Token: 0x04000268 RID: 616
		cfr,
		// Token: 0x04000269 RID: 617
		vfr,
		// Token: 0x0400026A RID: 618
		drop,
		// Token: 0x0400026B RID: 619
		auto
	}
}
