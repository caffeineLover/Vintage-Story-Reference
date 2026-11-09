using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200008E RID: 142
	public sealed class SequenceStartEventInfo : ObjectEventInfo
	{
		// Token: 0x06000373 RID: 883 RVA: 0x000103D7 File Offset: 0x0000E5D7
		[NullableContext(1)]
		public SequenceStartEventInfo(IObjectDescriptor source) : base(source)
		{
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00010402 File Offset: 0x0000E602
		// (set) Token: 0x06000375 RID: 885 RVA: 0x0001040A File Offset: 0x0000E60A
		public bool IsImplicit { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00010413 File Offset: 0x0000E613
		// (set) Token: 0x06000377 RID: 887 RVA: 0x0001041B File Offset: 0x0000E61B
		public SequenceStyle Style { get; set; }
	}
}
