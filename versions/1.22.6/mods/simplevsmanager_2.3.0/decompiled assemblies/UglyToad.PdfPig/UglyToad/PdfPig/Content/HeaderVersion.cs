using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001E2 RID: 482
	internal class HeaderVersion
	{
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x00039AF8 File Offset: 0x00037CF8
		public decimal Version { get; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x00039B00 File Offset: 0x00037D00
		public string VersionString { get; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x00039B08 File Offset: 0x00037D08
		public long OffsetInFile { get; }

		// Token: 0x06000B74 RID: 2932 RVA: 0x00039B10 File Offset: 0x00037D10
		public HeaderVersion(decimal version, string versionString, long offsetInFile)
		{
			this.Version = version;
			this.VersionString = versionString;
			if (offsetInFile < 0L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(59, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid offset for header version, must be positive. Got: ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(offsetInFile);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.OffsetInFile = offsetInFile;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00039B74 File Offset: 0x00037D74
		public override string ToString()
		{
			return "Version: " + this.VersionString;
		}
	}
}
