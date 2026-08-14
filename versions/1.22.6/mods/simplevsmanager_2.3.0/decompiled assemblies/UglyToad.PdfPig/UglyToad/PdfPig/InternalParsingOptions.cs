using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Logging;

namespace UglyToad.PdfPig
{
	// Token: 0x02000005 RID: 5
	internal class InternalParsingOptions
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000022B6 File Offset: 0x000004B6
		public IReadOnlyList<string> Passwords { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000022BE File Offset: 0x000004BE
		public bool UseLenientParsing { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000022C6 File Offset: 0x000004C6
		public bool ClipPaths { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000022CE File Offset: 0x000004CE
		public bool SkipMissingFonts { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000022D6 File Offset: 0x000004D6
		public bool SkipMissingXObjects { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022DE File Offset: 0x000004DE
		public ILog Logger { get; }

		// Token: 0x06000010 RID: 16 RVA: 0x000022E6 File Offset: 0x000004E6
		public InternalParsingOptions(IReadOnlyList<string> passwords, bool useLenientParsing, bool clipPaths, bool skipMissingFonts, bool skipMissingXObjects, ILog logger)
		{
			this.Passwords = passwords;
			this.UseLenientParsing = useLenientParsing;
			this.ClipPaths = clipPaths;
			this.SkipMissingFonts = skipMissingFonts;
			this.SkipMissingXObjects = skipMissingXObjects;
			this.Logger = logger;
		}
	}
}
