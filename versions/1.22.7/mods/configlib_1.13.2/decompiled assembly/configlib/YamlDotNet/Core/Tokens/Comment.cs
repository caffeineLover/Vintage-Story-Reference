using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D6 RID: 470
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Comment : Token
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x00024E07 File Offset: 0x00023007
		public string Value { get; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x00024E0F File Offset: 0x0002300F
		public bool IsInline { get; }

		// Token: 0x06000A7D RID: 2685 RVA: 0x00024E17 File Offset: 0x00023017
		public Comment(string value, bool isInline) : this(value, isInline, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00024E2B File Offset: 0x0002302B
		public Comment(string value, bool isInline, Mark start, Mark end) : base(start, end)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.Value = value;
			this.IsInline = isInline;
		}
	}
}
