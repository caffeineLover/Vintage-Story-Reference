using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001D2 RID: 466
	public sealed class Version
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x000254A5 File Offset: 0x000236A5
		public int Major { get; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x000254AD File Offset: 0x000236AD
		public int Minor { get; }

		// Token: 0x06000A9F RID: 2719 RVA: 0x000254B8 File Offset: 0x000236B8
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

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00025544 File Offset: 0x00023744
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			Version version = obj as Version;
			return version != null && this.Major == version.Major && this.Minor == version.Minor;
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002557C File Offset: 0x0002377C
		public override int GetHashCode()
		{
			return HashCode.CombineHashCodes(this.Major.GetHashCode(), this.Minor.GetHashCode());
		}
	}
}
