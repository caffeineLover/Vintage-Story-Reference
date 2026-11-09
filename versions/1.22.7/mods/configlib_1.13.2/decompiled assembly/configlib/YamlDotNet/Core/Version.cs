using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001CE RID: 462
	public sealed class Version
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00024B5D File Offset: 0x00022D5D
		public int Major { get; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00024B65 File Offset: 0x00022D65
		public int Minor { get; }

		// Token: 0x06000A63 RID: 2659 RVA: 0x00024B70 File Offset: 0x00022D70
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

		// Token: 0x06000A64 RID: 2660 RVA: 0x00024BFC File Offset: 0x00022DFC
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			Version version = obj as Version;
			return version != null && this.Major == version.Major && this.Minor == version.Minor;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00024C34 File Offset: 0x00022E34
		public override int GetHashCode()
		{
			return HashCode.CombineHashCodes(this.Major.GetHashCode(), this.Minor.GetHashCode());
		}
	}
}
