using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000092 RID: 146
	public sealed class SequenceStartEventInfo : ObjectEventInfo
	{
		// Token: 0x060003AF RID: 943 RVA: 0x00010D1F File Offset: 0x0000EF1F
		[NullableContext(1)]
		public SequenceStartEventInfo(IObjectDescriptor source) : base(source)
		{
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00010D4A File Offset: 0x0000EF4A
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x00010D52 File Offset: 0x0000EF52
		public bool IsImplicit { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00010D5B File Offset: 0x0000EF5B
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x00010D63 File Offset: 0x0000EF63
		public SequenceStyle Style { get; set; }
	}
}
