using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000070 RID: 112
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ModDirectoryChangeSet
	{
		// Token: 0x06000A4C RID: 2636 RVA: 0x0003FBCD File Offset: 0x0003DDCD
		public ModDirectoryChangeSet(bool requiresFullRescan, IReadOnlyCollection<string> paths)
		{
			this.RequiresFullRescan = requiresFullRescan;
			this.Paths = (paths ?? Array.Empty<string>());
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0003FBE6 File Offset: 0x0003DDE6
		public bool RequiresFullRescan { get; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0003FBEE File Offset: 0x0003DDEE
		public IReadOnlyCollection<string> Paths { get; }

		// Token: 0x040004C5 RID: 1221
		public static readonly ModDirectoryChangeSet Empty = new ModDirectoryChangeSet(false, Array.Empty<string>());
	}
}
