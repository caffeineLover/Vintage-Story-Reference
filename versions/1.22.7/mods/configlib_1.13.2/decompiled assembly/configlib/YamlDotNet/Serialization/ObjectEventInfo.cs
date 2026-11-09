using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200008A RID: 138
	public class ObjectEventInfo : EventInfo
	{
		// Token: 0x0600035F RID: 863 RVA: 0x00010348 File Offset: 0x0000E548
		[NullableContext(1)]
		protected ObjectEventInfo(IObjectDescriptor source) : base(source)
		{
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00010351 File Offset: 0x0000E551
		// (set) Token: 0x06000361 RID: 865 RVA: 0x00010359 File Offset: 0x0000E559
		public AnchorName Anchor { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00010362 File Offset: 0x0000E562
		// (set) Token: 0x06000363 RID: 867 RVA: 0x0001036A File Offset: 0x0000E56A
		public TagName Tag { get; set; }
	}
}
