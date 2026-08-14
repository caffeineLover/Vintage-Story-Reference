using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E7 RID: 487
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tag : Token
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x000258D3 File Offset: 0x00023AD3
		public string Handle { get; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x000258DB File Offset: 0x00023ADB
		public string Suffix { get; }

		// Token: 0x06000ADA RID: 2778 RVA: 0x000258E3 File Offset: 0x00023AE3
		public Tag(string handle, string suffix) : this(handle, suffix, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x000258F7 File Offset: 0x00023AF7
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
