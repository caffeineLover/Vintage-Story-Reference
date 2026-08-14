using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000EA RID: 234
	[NullableContext(1)]
	[Nullable(0)]
	public class Error : Token
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x00016ED6 File Offset: 0x000150D6
		public string Value { get; }

		// Token: 0x060005FF RID: 1535 RVA: 0x00016EDE File Offset: 0x000150DE
		public Error(string value, Mark start, Mark end) : base(start, end)
		{
			this.Value = value;
		}
	}
}
