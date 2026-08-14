using System;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Schemas
{
	// Token: 0x0200011C RID: 284
	public sealed class FailsafeSchema
	{
		// Token: 0x0200011D RID: 285
		public static class Tags
		{
			// Token: 0x040002D1 RID: 721
			public static readonly TagName Map = new TagName("tag:yaml.org,2002:map");

			// Token: 0x040002D2 RID: 722
			public static readonly TagName Seq = new TagName("tag:yaml.org,2002:seq");

			// Token: 0x040002D3 RID: 723
			public static readonly TagName Str = new TagName("tag:yaml.org,2002:str");
		}
	}
}
