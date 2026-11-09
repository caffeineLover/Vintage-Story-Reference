using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000F4 RID: 244
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tag : Token
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x00017039 File Offset: 0x00015239
		public string Handle { get; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x00017041 File Offset: 0x00015241
		public string Suffix { get; }

		// Token: 0x06000619 RID: 1561 RVA: 0x00017049 File Offset: 0x00015249
		public Tag(string handle, string suffix) : this(handle, suffix, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001705D File Offset: 0x0001525D
		public Tag(string handle, string suffix, Mark start, Mark end) : base(start, end)
		{
			if (handle == null)
			{
				throw new ArgumentNullException("handle");
			}
			this.Handle = handle;
			if (suffix == null)
			{
				throw new ArgumentNullException("suffix");
			}
			this.Suffix = suffix;
		}
	}
}
