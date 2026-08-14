using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E4 RID: 484
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Scalar : Token
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00025847 File Offset: 0x00023A47
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x0002584F File Offset: 0x00023A4F
		public bool IsKey { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x00025858 File Offset: 0x00023A58
		public string Value { get; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x00025860 File Offset: 0x00023A60
		public ScalarStyle Style { get; }

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00025868 File Offset: 0x00023A68
		public Scalar(string value) : this(value, ScalarStyle.Any)
		{
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00025872 File Offset: 0x00023A72
		public Scalar(string value, ScalarStyle style) : this(value, style, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00025886 File Offset: 0x00023A86
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
