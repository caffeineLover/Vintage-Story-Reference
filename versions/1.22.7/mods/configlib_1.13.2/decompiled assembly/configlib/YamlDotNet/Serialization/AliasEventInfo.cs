using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000089 RID: 137
	public class AliasEventInfo : EventInfo
	{
		// Token: 0x0600035B RID: 859 RVA: 0x0001030B File Offset: 0x0000E50B
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
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0001032F File Offset: 0x0000E52F
		public AnchorName Alias { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00010337 File Offset: 0x0000E537
		// (set) Token: 0x0600035E RID: 862 RVA: 0x0001033F File Offset: 0x0000E53F
		public bool NeedsExpansion { get; set; }
	}
}
