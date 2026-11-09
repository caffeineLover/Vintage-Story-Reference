using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200017B RID: 379
	[NullableContext(1)]
	[Nullable(0)]
	internal class EmitterState
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x00019BC2 File Offset: 0x00017DC2
		public HashSet<AnchorName> EmittedAnchors { get; } = new HashSet<AnchorName>();
	}
}
