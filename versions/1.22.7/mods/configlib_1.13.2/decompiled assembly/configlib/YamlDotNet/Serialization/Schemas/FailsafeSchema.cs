using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Schemas
{
	// Token: 0x02000118 RID: 280
	public sealed class FailsafeSchema
	{
		// Token: 0x02000119 RID: 281
		public static class Tags
		{
			// Token: 0x040002A3 RID: 675
			public static readonly TagName Map = new TagName("tag:yaml.org,2002:map");

			// Token: 0x040002A4 RID: 676
			public static readonly TagName Seq = new TagName("tag:yaml.org,2002:seq");

			// Token: 0x040002A5 RID: 677
			public static readonly TagName Str = new TagName("tag:yaml.org,2002:str");
		}
	}
}
