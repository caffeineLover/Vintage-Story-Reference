using System;
using System.Collections.Generic;

namespace Xabe.FFmpeg
{
	// Token: 0x02000033 RID: 51
	public interface IStream
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600017D RID: 381
		string Path { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600017E RID: 382
		int Index { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600017F RID: 383
		string Codec { get; }

		// Token: 0x06000180 RID: 384
		string BuildParameters(ParameterPosition forPosition);

		// Token: 0x06000181 RID: 385
		IEnumerable<string> GetSource();

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000182 RID: 386
		StreamType StreamType { get; }
	}
}
