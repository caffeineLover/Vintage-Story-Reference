using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Schemas
{
	// Token: 0x0200011A RID: 282
	public sealed class JsonSchema
	{
		// Token: 0x0200011B RID: 283
		public static class Tags
		{
			// Token: 0x040002A6 RID: 678
			public static readonly TagName Null = new TagName("tag:yaml.org,2002:null");

			// Token: 0x040002A7 RID: 679
			public static readonly TagName Bool = new TagName("tag:yaml.org,2002:bool");

			// Token: 0x040002A8 RID: 680
			public static readonly TagName Int = new TagName("tag:yaml.org,2002:int");

			// Token: 0x040002A9 RID: 681
			public static readonly TagName Float = new TagName("tag:yaml.org,2002:float");
		}
	}
}
