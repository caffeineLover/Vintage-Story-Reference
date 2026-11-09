using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000E7 RID: 231
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Comment : Token
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x00016E51 File Offset: 0x00015051
		public string Value { get; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00016E59 File Offset: 0x00015059
		public bool IsInline { get; }

		// Token: 0x060005F8 RID: 1528 RVA: 0x00016E61 File Offset: 0x00015061
		public Comment(string value, bool isInline) : this(value, isInline, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00016E75 File Offset: 0x00015075
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
