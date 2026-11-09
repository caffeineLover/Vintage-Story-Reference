using System;

namespace Xabe.FFmpeg
{
	// Token: 0x02000017 RID: 23
	public interface IConversionResult
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000B4 RID: 180
		DateTime StartTime { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000B5 RID: 181
		DateTime EndTime { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000B6 RID: 182
		TimeSpan Duration { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000B7 RID: 183
		string Arguments { get; }
	}
}
