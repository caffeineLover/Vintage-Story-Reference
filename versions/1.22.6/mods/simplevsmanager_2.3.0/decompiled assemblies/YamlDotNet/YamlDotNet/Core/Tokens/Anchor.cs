using System;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000E1 RID: 225
	public class Anchor : Token
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x00016D5D File Offset: 0x00014F5D
		public AnchorName Value { get; }

		// Token: 0x060005E9 RID: 1513 RVA: 0x00016D65 File Offset: 0x00014F65
		public Anchor(AnchorName value) : this(value, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00016D78 File Offset: 0x00014F78
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
