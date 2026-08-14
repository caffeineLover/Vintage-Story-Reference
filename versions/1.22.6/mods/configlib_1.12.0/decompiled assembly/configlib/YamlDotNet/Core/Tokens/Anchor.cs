using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D4 RID: 468
	public class Anchor : Token
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x00025679 File Offset: 0x00023879
		public AnchorName Value { get; }

		// Token: 0x06000AAA RID: 2730 RVA: 0x00025681 File Offset: 0x00023881
		public Anchor(AnchorName value) : this(value, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00025694 File Offset: 0x00023894
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
