using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200017F RID: 383
	[NullableContext(1)]
	[Nullable(0)]
	internal class EmitterState
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0001A50A File Offset: 0x0001870A
		public HashSet<AnchorName> EmittedAnchors { get; } = new HashSet<AnchorName>();
	}
}
