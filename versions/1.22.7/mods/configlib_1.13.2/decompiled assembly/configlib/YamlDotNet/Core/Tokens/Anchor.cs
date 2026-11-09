using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D0 RID: 464
	public class Anchor : Token
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00024D31 File Offset: 0x00022F31
		public AnchorName Value { get; }

		// Token: 0x06000A6E RID: 2670 RVA: 0x00024D39 File Offset: 0x00022F39
		public Anchor(AnchorName value) : this(value, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00024D4C File Offset: 0x00022F4C
		public Anchor(AnchorName value, Mark start, Mark end) : base(start, end)
		{
			if (value.IsEmpty)
			{
				throw new ArgumentNullException("value");
			}
			this.Value = value;
		}
	}
}
