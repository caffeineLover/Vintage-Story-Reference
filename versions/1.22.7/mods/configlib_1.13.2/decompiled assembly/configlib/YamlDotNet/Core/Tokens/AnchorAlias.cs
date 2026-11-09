using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D1 RID: 465
	public sealed class AnchorAlias : Token
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x00024D73 File Offset: 0x00022F73
		public AnchorName Value { get; }

		// Token: 0x06000A71 RID: 2673 RVA: 0x00024D7B File Offset: 0x00022F7B
		public AnchorAlias(AnchorName value) : this(value, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00024D8E File Offset: 0x00022F8E
		public AnchorAlias(AnchorName value, Mark start, Mark end) : base(start, end)
		{
			if (value.IsEmpty)
			{
				throw new ArgumentNullException("value");
			}
			this.Value = value;
		}
	}
}
