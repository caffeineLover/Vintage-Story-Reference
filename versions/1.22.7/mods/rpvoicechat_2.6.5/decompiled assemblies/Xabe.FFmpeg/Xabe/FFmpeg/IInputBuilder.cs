using System;
using System.Collections.Generic;
using System.IO;

namespace Xabe.FFmpeg
{
	// Token: 0x02000013 RID: 19
	public interface IInputBuilder
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600009D RID: 157
		List<FileInfo> FileList { get; }

		// Token: 0x0600009E RID: 158
		Func<string, string> PrepareInputFiles(List<string> files, out string directory);
	}
}
