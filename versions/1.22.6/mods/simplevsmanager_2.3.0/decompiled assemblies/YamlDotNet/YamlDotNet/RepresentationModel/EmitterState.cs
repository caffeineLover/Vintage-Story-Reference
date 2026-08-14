using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000A5 RID: 165
	[NullableContext(1)]
	[Nullable(0)]
	internal class EmitterState
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000377 RID: 887 RVA: 0x0000C82A File Offset: 0x0000AA2A
		public HashSet<AnchorName> EmittedAnchors { get; } = new HashSet<AnchorName>();
	}
}
