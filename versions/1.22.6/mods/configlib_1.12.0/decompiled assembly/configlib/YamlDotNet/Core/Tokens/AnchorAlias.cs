using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D5 RID: 469
	public sealed class AnchorAlias : Token
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x000256BB File Offset: 0x000238BB
		public AnchorName Value { get; }

		// Token: 0x06000AAD RID: 2733 RVA: 0x000256C3 File Offset: 0x000238C3
		public AnchorAlias(AnchorName value) : this(value, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x000256D6 File Offset: 0x000238D6
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
