using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x0200008B RID: 139
	public struct CompactFontFormatHeader
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600032D RID: 813 RVA: 0x000168FC File Offset: 0x00014AFC
		public readonly byte MajorVersion { get; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00016904 File Offset: 0x00014B04
		public readonly byte MinorVersion { get; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0001690C File Offset: 0x00014B0C
		public readonly byte SizeInBytes { get; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00016914 File Offset: 0x00014B14
		public readonly byte OffsetSize { get; }

		// Token: 0x06000331 RID: 817 RVA: 0x0001691C File Offset: 0x00014B1C
		public CompactFontFormatHeader(byte majorVersion, byte minorVersion, byte sizeInBytes, byte offsetSize)
		{
			this.MajorVersion = majorVersion;
			this.MinorVersion = minorVersion;
			this.SizeInBytes = sizeInBytes;
			this.OffsetSize = offsetSize;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0001693C File Offset: 0x00014B3C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(41, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Major: ");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.MajorVersion);
			defaultInterpolatedStringHandler.AppendLiteral(", Minor: ");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.MinorVersion);
			defaultInterpolatedStringHandler.AppendLiteral(", Header Size: ");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.SizeInBytes);
			defaultInterpolatedStringHandler.AppendLiteral(", Offset: ");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(this.OffsetSize);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
