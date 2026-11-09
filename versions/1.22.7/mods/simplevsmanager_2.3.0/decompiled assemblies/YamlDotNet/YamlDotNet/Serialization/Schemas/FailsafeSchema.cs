using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Schemas
{
	// Token: 0x0200005E RID: 94
	public sealed class FailsafeSchema
	{
		// Token: 0x02000172 RID: 370
		public static class Tags
		{
			// Token: 0x040002E9 RID: 745
			public static readonly TagName Map = new TagName("tag:yaml.org,2002:map");

			// Token: 0x040002EA RID: 746
			public static readonly TagName Seq = new TagName("tag:yaml.org,2002:seq");

			// Token: 0x040002EB RID: 747
			public static readonly TagName Str = new TagName("tag:yaml.org,2002:str");
		}
	}
}
