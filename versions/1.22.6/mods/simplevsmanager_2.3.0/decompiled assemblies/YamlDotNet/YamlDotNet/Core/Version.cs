using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000DF RID: 223
	public sealed class Version
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x00016B89 File Offset: 0x00014D89
		public int Major { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x00016B91 File Offset: 0x00014D91
		public int Minor { get; }

		// Token: 0x060005DE RID: 1502 RVA: 0x00016B9C File Offset: 0x00014D9C
		public Version(int major, int minor)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (major < 0)
			{
				string text = "major";
				defaultInterpolatedStringHandler..ctor(15, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(major);
				defaultInterpolatedStringHandler.AppendLiteral(" should be >= 0");
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Major = major;
			if (minor < 0)
			{
				string text2 = "minor";
				defaultInterpolatedStringHandler..ctor(15, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(minor);
				defaultInterpolatedStringHandler.AppendLiteral(" should be >= 0");
				throw new ArgumentOutOfRangeException(text2, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Minor = minor;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00016C28 File Offset: 0x00014E28
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			Version version = obj as Version;
			return version != null && this.Major == version.Major && this.Minor == version.Minor;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00016C60 File Offset: 0x00014E60
		public override int GetHashCode()
		{
			return HashCode.CombineHashCodes(this.Major.GetHashCode(), this.Minor.GetHashCode());
		}
	}
}
