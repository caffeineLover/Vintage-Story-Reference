using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000C2 RID: 194
	public sealed class SerializableConfigList
	{
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x00054C53 File Offset: 0x00052E53
		// (set) Token: 0x06000FF2 RID: 4082 RVA: 0x00054C5B File Offset: 0x00052E5B
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public List<SerializableModConfiguration> Configurations { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }
	}
}
