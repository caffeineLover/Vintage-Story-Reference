using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D7 RID: 471
	public sealed class BlockEntry : Token
	{
		// Token: 0x06000AB1 RID: 2737 RVA: 0x00025719 File Offset: 0x00023919
		public BlockEntry() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0002570F File Offset: 0x0002390F
		public BlockEntry(in Mark start, in Mark end) : base(start, end)
		{
		}
	}
}
