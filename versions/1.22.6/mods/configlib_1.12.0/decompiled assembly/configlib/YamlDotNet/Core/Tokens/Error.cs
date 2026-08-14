using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001DD RID: 477
	[NullableContext(1)]
	[Nullable(0)]
	public class Error : Token
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x000257C0 File Offset: 0x000239C0
		public string Value { get; }

		// Token: 0x06000AC0 RID: 2752 RVA: 0x000257C8 File Offset: 0x000239C8
		public Error(string value, Mark start, Mark end) : base(start, end)
		{
			this.Value = value;
		}
	}
}
