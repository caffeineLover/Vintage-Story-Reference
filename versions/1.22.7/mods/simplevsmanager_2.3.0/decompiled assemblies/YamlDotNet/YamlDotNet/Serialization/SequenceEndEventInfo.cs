using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000015 RID: 21
	public sealed class SequenceEndEventInfo : EventInfo
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x000039D6 File Offset: 0x00001BD6
		[NullableContext(1)]
		public SequenceEndEventInfo(IObjectDescriptor source) : base(source)
		{
		}
	}
}
