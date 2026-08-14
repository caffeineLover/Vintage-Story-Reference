using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200008D RID: 141
	public class AliasEventInfo : EventInfo
	{
		// Token: 0x06000397 RID: 919 RVA: 0x00010C53 File Offset: 0x0000EE53
		[NullableContext(1)]
		public AliasEventInfo(IObjectDescriptor source, AnchorName alias) : base(source)
		{
			if (alias.IsEmpty)
			{
				throw new ArgumentNullException("alias");
			}
			this.Alias = alias;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000398 RID: 920 RVA: 0x00010C77 File Offset: 0x0000EE77
		public AnchorName Alias { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00010C7F File Offset: 0x0000EE7F
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00010C87 File Offset: 0x0000EE87
		public bool NeedsExpansion { get; set; }
	}
}
