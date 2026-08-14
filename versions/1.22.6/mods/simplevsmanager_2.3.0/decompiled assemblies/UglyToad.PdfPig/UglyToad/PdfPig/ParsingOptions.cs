using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Logging;

namespace UglyToad.PdfPig
{
	// Token: 0x02000006 RID: 6
	public class ParsingOptions
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000231B File Offset: 0x0000051B
		public static ParsingOptions LenientParsingOff { get; } = new ParsingOptions
		{
			UseLenientParsing = false
		};

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002322 File Offset: 0x00000522
		// (set) Token: 0x06000013 RID: 19 RVA: 0x0000232A File Offset: 0x0000052A
		public bool ClipPaths { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002333 File Offset: 0x00000533
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000233B File Offset: 0x0000053B
		public bool UseLenientParsing { get; set; } = true;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002344 File Offset: 0x00000544
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002355 File Offset: 0x00000555
		public ILog Logger
		{
			get
			{
				return this.logger ?? new NoOpLog();
			}
			set
			{
				this.logger = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000235E File Offset: 0x0000055E
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002366 File Offset: 0x00000566
		public string Password { get; set; } = string.Empty;

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000236F File Offset: 0x0000056F
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002377 File Offset: 0x00000577
		public List<string> Passwords { get; set; } = new List<string>();

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002380 File Offset: 0x00000580
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002388 File Offset: 0x00000588
		public bool SkipMissingFonts { get; set; }

		// Token: 0x0400000F RID: 15
		private ILog logger = new NoOpLog();
	}
}
