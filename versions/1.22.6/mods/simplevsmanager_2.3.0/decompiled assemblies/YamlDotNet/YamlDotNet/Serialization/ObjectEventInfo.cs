using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000010 RID: 16
	public class ObjectEventInfo : EventInfo
	{
		// Token: 0x06000087 RID: 135 RVA: 0x000038E8 File Offset: 0x00001AE8
		[NullableContext(1)]
		protected ObjectEventInfo(IObjectDescriptor source) : base(source)
		{
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000038F1 File Offset: 0x00001AF1
		// (set) Token: 0x06000089 RID: 137 RVA: 0x000038F9 File Offset: 0x00001AF9
		public AnchorName Anchor { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003902 File Offset: 0x00001B02
		// (set) Token: 0x0600008B RID: 139 RVA: 0x0000390A File Offset: 0x00001B0A
		public TagName Tag { get; set; }
	}
}
