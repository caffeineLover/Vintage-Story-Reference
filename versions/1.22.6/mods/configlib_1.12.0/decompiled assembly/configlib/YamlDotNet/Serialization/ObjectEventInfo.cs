using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200008E RID: 142
	public class ObjectEventInfo : EventInfo
	{
		// Token: 0x0600039B RID: 923 RVA: 0x00010C90 File Offset: 0x0000EE90
		[NullableContext(1)]
		protected ObjectEventInfo(IObjectDescriptor source) : base(source)
		{
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00010C99 File Offset: 0x0000EE99
		// (set) Token: 0x0600039D RID: 925 RVA: 0x00010CA1 File Offset: 0x0000EEA1
		public AnchorName Anchor { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00010CAA File Offset: 0x0000EEAA
		// (set) Token: 0x0600039F RID: 927 RVA: 0x00010CB2 File Offset: 0x0000EEB2
		public TagName Tag { get; set; }
	}
}
