using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Schemas
{
	// Token: 0x0200011E RID: 286
	public sealed class JsonSchema
	{
		// Token: 0x0200011F RID: 287
		public static class Tags
		{
			// Token: 0x040002D4 RID: 724
			public static readonly TagName Null = new TagName("tag:yaml.org,2002:null");

			// Token: 0x040002D5 RID: 725
			public static readonly TagName Bool = new TagName("tag:yaml.org,2002:bool");

			// Token: 0x040002D6 RID: 726
			public static readonly TagName Int = new TagName("tag:yaml.org,2002:int");

			// Token: 0x040002D7 RID: 727
			public static readonly TagName Float = new TagName("tag:yaml.org,2002:float");
		}
	}
}
