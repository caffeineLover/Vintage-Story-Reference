using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Tokens
{
	// Token: 0x020000F8 RID: 248
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class VersionDirective : Token
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x000171FC File Offset: 0x000153FC
		public Version Version { get; }

		// Token: 0x06000629 RID: 1577 RVA: 0x00017204 File Offset: 0x00015404
		public VersionDirective(Version version) : this(version, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00017217 File Offset: 0x00015417
		public VersionDirective(Version version, Mark start, Mark end) : base(start, end)
		{
			this.Version = version;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001722C File Offset: 0x0001542C
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			VersionDirective versionDirective = obj as VersionDirective;
			return versionDirective != null && this.Version.Equals(versionDirective.Version);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00017256 File Offset: 0x00015456
		public override int GetHashCode()
		{
			return this.Version.GetHashCode();
		}
	}
}
