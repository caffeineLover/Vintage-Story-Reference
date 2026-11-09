using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Schemas
{
	// Token: 0x0200005F RID: 95
	public sealed class JsonSchema
	{
		// Token: 0x02000173 RID: 371
		public static class Tags
		{
			// Token: 0x040002EC RID: 748
			public static readonly TagName Null = new TagName("tag:yaml.org,2002:null");

			// Token: 0x040002ED RID: 749
			public static readonly TagName Bool = new TagName("tag:yaml.org,2002:bool");

			// Token: 0x040002EE RID: 750
			public static readonly TagName Int = new TagName("tag:yaml.org,2002:int");

			// Token: 0x040002EF RID: 751
			public static readonly TagName Float = new TagName("tag:yaml.org,2002:float");
		}
	}
}
