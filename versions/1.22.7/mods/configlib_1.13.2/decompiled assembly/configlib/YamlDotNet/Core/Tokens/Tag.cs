using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E3 RID: 483
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Tag : Token
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x00024F8B File Offset: 0x0002318B
		public string Handle { get; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x00024F93 File Offset: 0x00023193
		public string Suffix { get; }

		// Token: 0x06000A9E RID: 2718 RVA: 0x00024F9B File Offset: 0x0002319B
		public Tag(string handle, string suffix) : this(handle, suffix, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00024FAF File Offset: 0x000231AF
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
