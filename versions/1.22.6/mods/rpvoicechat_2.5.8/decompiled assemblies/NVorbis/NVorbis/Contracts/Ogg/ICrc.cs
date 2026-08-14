using System;

namespace NVorbis.Contracts.Ogg
{
	// Token: 0x02000035 RID: 53
	internal interface ICrc
	{
		// Token: 0x0600022B RID: 555
		void Reset();

		// Token: 0x0600022C RID: 556
		void Update(int nextVal);

		// Token: 0x0600022D RID: 557
		bool Test(uint checkCrc);
	}
}
