using System;

namespace Concentus.Structs
{
	// Token: 0x02000060 RID: 96
	internal class StereoWidthState
	{
		// Token: 0x06000244 RID: 580 RVA: 0x0001F9FF File Offset: 0x0001DBFF
		internal void Reset()
		{
			this.XX = 0;
			this.XY = 0;
			this.YY = 0;
			this.smoothed_width = 0;
			this.max_follower = 0;
		}

		// Token: 0x04000309 RID: 777
		internal int XX;

		// Token: 0x0400030A RID: 778
		internal int XY;

		// Token: 0x0400030B RID: 779
		internal int YY;

		// Token: 0x0400030C RID: 780
		internal int smoothed_width;

		// Token: 0x0400030D RID: 781
		internal int max_follower;
	}
}
