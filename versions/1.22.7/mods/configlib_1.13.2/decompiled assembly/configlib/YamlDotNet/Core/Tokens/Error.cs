using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001D9 RID: 473
	[NullableContext(1)]
	[Nullable(0)]
	public class Error : Token
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x00024E78 File Offset: 0x00023078
		public string Value { get; }

		// Token: 0x06000A84 RID: 2692 RVA: 0x00024E80 File Offset: 0x00023080
		public Error(string value, Mark start, Mark end) : base(start, end)
		{
			this.Value = value;
		}
	}
}
