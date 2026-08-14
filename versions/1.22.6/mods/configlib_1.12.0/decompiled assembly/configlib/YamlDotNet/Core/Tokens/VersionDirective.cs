using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020001EB RID: 491
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VersionDirective : Token
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00025A8A File Offset: 0x00023C8A
		public Version Version { get; }

		// Token: 0x06000AEA RID: 2794 RVA: 0x00025A92 File Offset: 0x00023C92
		public VersionDirective(Version version) : this(version, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00025AA5 File Offset: 0x00023CA5
		public VersionDirective(Version version, Mark start, Mark end) : base(start, end)
		{
			this.Version = version;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00025AB8 File Offset: 0x00023CB8
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			VersionDirective versionDirective = obj as VersionDirective;
			return versionDirective != null && this.Version.Equals(versionDirective.Version);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00025AE2 File Offset: 0x00023CE2
		public override int GetHashCode()
		{
			return this.Version.GetHashCode();
		}
	}
}
