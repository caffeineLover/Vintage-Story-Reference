using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020001AB RID: 427
	public static class Constants
	{
		// Token: 0x040003B6 RID: 950
		[Nullable(1)]
		public static readonly TagDirective[] DefaultTagDirectives = new TagDirective[]
		{
			new TagDirective("!", "!"),
			new TagDirective("!!", "tag:yaml.org,2002:")
		};

		// Token: 0x040003B7 RID: 951
		public const int MajorVersion = 1;

		// Token: 0x040003B8 RID: 952
		public const int MinorVersion = 3;
	}
}
