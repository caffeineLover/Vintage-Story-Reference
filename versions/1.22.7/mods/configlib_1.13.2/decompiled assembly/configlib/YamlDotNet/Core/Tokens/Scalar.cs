using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E0 RID: 480
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Scalar : Token
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x00024EFF File Offset: 0x000230FF
		// (set) Token: 0x06000A92 RID: 2706 RVA: 0x00024F07 File Offset: 0x00023107
		public bool IsKey { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x00024F10 File Offset: 0x00023110
		public string Value { get; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x00024F18 File Offset: 0x00023118
		public ScalarStyle Style { get; }

		// Token: 0x06000A95 RID: 2709 RVA: 0x00024F20 File Offset: 0x00023120
		public Scalar(string value) : this(value, ScalarStyle.Any)
		{
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00024F2A File Offset: 0x0002312A
		public Scalar(string value, ScalarStyle style) : this(value, style, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00024F3E File Offset: 0x0002313E
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
