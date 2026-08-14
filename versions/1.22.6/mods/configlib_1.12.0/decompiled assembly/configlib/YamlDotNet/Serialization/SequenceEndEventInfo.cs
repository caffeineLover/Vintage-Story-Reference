using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000093 RID: 147
	public sealed class SequenceEndEventInfo : EventInfo
	{
		// Token: 0x060003B4 RID: 948 RVA: 0x00010C90 File Offset: 0x0000EE90
		[NullableContext(1)]
		public SequenceEndEventInfo(IObjectDescriptor source) : base(source)
		{
		}
	}
}
