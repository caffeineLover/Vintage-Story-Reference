using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020000C3 RID: 195
	public static class Constants
	{
		// Token: 0x04000121 RID: 289
		[Nullable(1)]
		public static readonly TagDirective[] DefaultTagDirectives = new TagDirective[]
		{
			new TagDirective("!", "!"),
			new TagDirective("!!", "tag:yaml.org,2002:")
		};

		// Token: 0x04000122 RID: 290
		public const int MajorVersion = 1;

		// Token: 0x04000123 RID: 291
		public const int MinorVersion = 3;
	}
}
