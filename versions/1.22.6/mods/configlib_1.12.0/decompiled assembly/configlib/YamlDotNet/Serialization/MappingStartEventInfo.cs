using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000090 RID: 144
	public sealed class MappingStartEventInfo : ObjectEventInfo
	{
		// Token: 0x060003A9 RID: 937 RVA: 0x00010D1F File Offset: 0x0000EF1F
		[NullableContext(1)]
		public MappingStartEventInfo(IObjectDescriptor source) : base(source)
		{
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00010D28 File Offset: 0x0000EF28
		// (set) Token: 0x060003AB RID: 939 RVA: 0x00010D30 File Offset: 0x0000EF30
		public bool IsImplicit { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00010D39 File Offset: 0x0000EF39
		// (set) Token: 0x060003AD RID: 941 RVA: 0x00010D41 File Offset: 0x0000EF41
		public MappingStyle Style { get; set; }
	}
}
