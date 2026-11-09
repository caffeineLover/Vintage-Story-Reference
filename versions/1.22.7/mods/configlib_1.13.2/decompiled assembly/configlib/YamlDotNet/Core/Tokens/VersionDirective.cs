using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001E7 RID: 487
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VersionDirective : Token
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x00025142 File Offset: 0x00023342
		public Version Version { get; }

		// Token: 0x06000AAE RID: 2734 RVA: 0x0002514A File Offset: 0x0002334A
		public VersionDirective(Version version) : this(version, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0002515D File Offset: 0x0002335D
		public VersionDirective(Version version, Mark start, Mark end) : base(start, end)
		{
			this.Version = version;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00025170 File Offset: 0x00023370
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			VersionDirective versionDirective = obj as VersionDirective;
			return versionDirective != null && this.Version.Equals(versionDirective.Version);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002519A File Offset: 0x0002339A
		public override int GetHashCode()
		{
			return this.Version.GetHashCode();
		}
	}
}
