using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200008C RID: 140
	public sealed class MappingStartEventInfo : ObjectEventInfo
	{
		// Token: 0x0600036D RID: 877 RVA: 0x000103D7 File Offset: 0x0000E5D7
		[NullableContext(1)]
		public MappingStartEventInfo(IObjectDescriptor source) : base(source)
		{
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600036E RID: 878 RVA: 0x000103E0 File Offset: 0x0000E5E0
		// (set) Token: 0x0600036F RID: 879 RVA: 0x000103E8 File Offset: 0x0000E5E8
		public bool IsImplicit { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000370 RID: 880 RVA: 0x000103F1 File Offset: 0x0000E5F1
		// (set) Token: 0x06000371 RID: 881 RVA: 0x000103F9 File Offset: 0x0000E5F9
		public MappingStyle Style { get; set; }
	}
}
