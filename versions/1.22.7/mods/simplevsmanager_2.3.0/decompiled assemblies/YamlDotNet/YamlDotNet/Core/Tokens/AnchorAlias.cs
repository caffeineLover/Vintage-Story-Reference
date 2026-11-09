using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000E2 RID: 226
	public sealed class AnchorAlias : Token
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00016D9F File Offset: 0x00014F9F
		public AnchorName Value { get; }

		// Token: 0x060005EC RID: 1516 RVA: 0x00016DA7 File Offset: 0x00014FA7
		public AnchorAlias(AnchorName value) : this(value, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00016DBA File Offset: 0x00014FBA
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
