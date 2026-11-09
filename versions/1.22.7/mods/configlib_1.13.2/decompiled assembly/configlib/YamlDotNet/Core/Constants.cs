using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020001A7 RID: 423
	public static class Constants
	{
		// Token: 0x04000388 RID: 904
		[Nullable(1)]
		public static readonly TagDirective[] DefaultTagDirectives = new TagDirective[]
		{
			new TagDirective("!", "!"),
			new TagDirective("!!", "tag:yaml.org,2002:")
		};

		// Token: 0x04000389 RID: 905
		public const int MajorVersion = 1;

		// Token: 0x0400038A RID: 906
		public const int MinorVersion = 3;
	}
}
