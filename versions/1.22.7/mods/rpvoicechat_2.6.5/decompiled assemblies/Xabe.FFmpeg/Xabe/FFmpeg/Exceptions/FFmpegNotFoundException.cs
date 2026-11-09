using System;
using System.IO;

namespace Xabe.FFmpeg.Exceptions
{
	// Token: 0x02000046 RID: 70
	public class FFmpegNotFoundException : FileNotFoundException
	{
		// Token: 0x06000217 RID: 535 RVA: 0x0000720E File Offset: 0x0000540E
		internal FFmpegNotFoundException(string errorMessage) : base(errorMessage)
		{
		}
	}
}
