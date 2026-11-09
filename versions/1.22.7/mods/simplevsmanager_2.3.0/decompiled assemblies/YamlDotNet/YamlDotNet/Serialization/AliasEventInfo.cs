using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200000F RID: 15
	public class AliasEventInfo : EventInfo
	{
		// Token: 0x06000083 RID: 131 RVA: 0x000038AB File Offset: 0x00001AAB
		[NullableContext(1)]
		public AliasEventInfo(IObjectDescriptor source, AnchorName alias) : base(source)
		{
			if (alias.IsEmpty)
			{
				throw new ArgumentNullException("alias");
			}
			this.Alias = alias;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000038CF File Offset: 0x00001ACF
		public AnchorName Alias { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000038D7 File Offset: 0x00001AD7
		// (set) Token: 0x06000086 RID: 134 RVA: 0x000038DF File Offset: 0x00001ADF
		public bool NeedsExpansion { get; set; }
	}
}
