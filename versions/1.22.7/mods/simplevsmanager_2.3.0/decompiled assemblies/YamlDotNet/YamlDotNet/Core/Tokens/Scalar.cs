using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000F1 RID: 241
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Scalar : Token
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00016F99 File Offset: 0x00015199
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x00016FA1 File Offset: 0x000151A1
		public bool IsKey { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x00016FAA File Offset: 0x000151AA
		public string Value { get; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x00016FB2 File Offset: 0x000151B2
		public ScalarStyle Style { get; }

		// Token: 0x06000610 RID: 1552 RVA: 0x00016FBA File Offset: 0x000151BA
		public Scalar(string value) : this(value, ScalarStyle.Any)
		{
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00016FC4 File Offset: 0x000151C4
		public Scalar(string value, ScalarStyle style) : this(value, style, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00016FD8 File Offset: 0x000151D8
		public Scalar(string value, ScalarStyle style, Mark start, Mark end) : base(start, end)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.Value = value;
			this.Style = style;
		}
	}
}
