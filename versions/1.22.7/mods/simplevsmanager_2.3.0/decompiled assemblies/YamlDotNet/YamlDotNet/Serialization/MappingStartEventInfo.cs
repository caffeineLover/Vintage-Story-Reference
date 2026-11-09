using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000012 RID: 18
	public sealed class MappingStartEventInfo : ObjectEventInfo
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00003977 File Offset: 0x00001B77
		[NullableContext(1)]
		public MappingStartEventInfo(IObjectDescriptor source) : base(source)
		{
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003980 File Offset: 0x00001B80
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003988 File Offset: 0x00001B88
		public bool IsImplicit { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003991 File Offset: 0x00001B91
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003999 File Offset: 0x00001B99
		public MappingStyle Style { get; set; }
	}
}
