using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DA RID: 474
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Comment : Token
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0002574F File Offset: 0x0002394F
		public string Value { get; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00025757 File Offset: 0x00023957
		public bool IsInline { get; }

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0002575F File Offset: 0x0002395F
		public Comment(string value, bool isInline) : this(value, isInline, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00025773 File Offset: 0x00023973
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
