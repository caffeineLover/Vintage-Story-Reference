using System;

namespace NVorbis.Contracts.Ogg
{
	// Token: 0x02000036 RID: 54
	internal interface IForwardOnlyPacketProvider : NVorbis.Contracts.IPacketProvider
	{
		// Token: 0x0600022E RID: 558
		bool AddPage(byte[] buf, bool isResync);

		// Token: 0x0600022F RID: 559
		void SetEndOfStream();
	}
}
