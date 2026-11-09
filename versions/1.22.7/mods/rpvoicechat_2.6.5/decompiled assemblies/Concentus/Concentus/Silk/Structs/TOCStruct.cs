using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000055 RID: 85
	internal class TOCStruct
	{
		// Token: 0x06000175 RID: 373 RVA: 0x00018C9D File Offset: 0x00016E9D
		internal void Reset()
		{
			this.VADFlag = 0;
			Arrays.MemSetInt(this.VADFlags, 0, 3);
			this.inbandFECFlag = 0;
		}

		// Token: 0x0400029D RID: 669
		internal int VADFlag;

		// Token: 0x0400029E RID: 670
		internal readonly int[] VADFlags = new int[3];

		// Token: 0x0400029F RID: 671
		internal int inbandFECFlag;
	}
}
