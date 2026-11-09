using System;

namespace NVorbis.Contracts.Ogg
{
	// Token: 0x0200003B RID: 59
	[Flags]
	internal enum PageFlags
	{
		// Token: 0x040000D7 RID: 215
		None = 0,
		// Token: 0x040000D8 RID: 216
		ContinuesPacket = 1,
		// Token: 0x040000D9 RID: 217
		BeginningOfStream = 2,
		// Token: 0x040000DA RID: 218
		EndOfStream = 4
	}
}
