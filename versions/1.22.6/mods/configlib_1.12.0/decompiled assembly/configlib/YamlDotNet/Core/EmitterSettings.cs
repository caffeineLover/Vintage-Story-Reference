using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001B2 RID: 434
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmitterSettings
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0001F9E0 File Offset: 0x0001DBE0
		public int BestIndent { get; } = 2;

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0001F9E8 File Offset: 0x0001DBE8
		public int BestWidth { get; } = int.MaxValue;

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x0001F9F0 File Offset: 0x0001DBF0
		public string NewLine { get; } = Environment.NewLine;

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0001F9F8 File Offset: 0x0001DBF8
		public bool IsCanonical { get; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x0001FA00 File Offset: 0x0001DC00
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x0001FA08 File Offset: 0x0001DC08
		public bool SkipAnchorName { get; private set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0001FA11 File Offset: 0x0001DC11
		public int MaxSimpleKeyLength { get; } = 1024;

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0001FA19 File Offset: 0x0001DC19
		public bool IndentSequences { get; }

		// Token: 0x06000996 RID: 2454 RVA: 0x0001FA21 File Offset: 0x0001DC21
		public EmitterSettings()
		{
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0001FA54 File Offset: 0x0001DC54
		[NullableContext(2)]
		public EmitterSettings(int bestIndent, int bestWidth, bool isCanonical, int maxSimpleKeyLength, bool skipAnchorName = false, bool indentSequences = false, string newLine = null)
		{
			if (bestIndent < 2 || bestIndent > 9)
			{
				throw new ArgumentOutOfRangeException("bestIndent", "BestIndent must be between 2 and 9, inclusive");
			}
			if (bestWidth <= bestIndent * 2)
			{
				throw new ArgumentOutOfRangeException("bestWidth", "BestWidth must be greater than BestIndent x 2.");
			}
			if (maxSimpleKeyLength < 0)
			{
				throw new ArgumentOutOfRangeException("maxSimpleKeyLength", "MaxSimpleKeyLength must be >= 0");
			}
			this.BestIndent = bestIndent;
			this.BestWidth = bestWidth;
			this.IsCanonical = isCanonical;
			this.MaxSimpleKeyLength = maxSimpleKeyLength;
			this.SkipAnchorName = skipAnchorName;
			this.IndentSequences = indentSequences;
			this.NewLine = (newLine ?? Environment.NewLine);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0001FB11 File Offset: 0x0001DD11
		public EmitterSettings WithBestIndent(int bestIndent)
		{
			return new EmitterSettings(bestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, this.NewLine);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0001FB3D File Offset: 0x0001DD3D
		public EmitterSettings WithBestWidth(int bestWidth)
		{
			return new EmitterSettings(this.BestIndent, bestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, this.NewLine);
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0001FB69 File Offset: 0x0001DD69
		public EmitterSettings WithMaxSimpleKeyLength(int maxSimpleKeyLength)
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, maxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, this.NewLine);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0001FB95 File Offset: 0x0001DD95
		public EmitterSettings WithNewLine(string newLine)
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, newLine);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0001FBC1 File Offset: 0x0001DDC1
		public EmitterSettings Canonical()
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, true, this.MaxSimpleKeyLength, this.SkipAnchorName, false, null);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0001FBE3 File Offset: 0x0001DDE3
		public EmitterSettings WithoutAnchorName()
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, true, this.IndentSequences, this.NewLine);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0001FC0F File Offset: 0x0001DE0F
		public EmitterSettings WithIndentedSequences()
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, true, this.NewLine);
		}

		// Token: 0x040003EC RID: 1004
		public static readonly EmitterSettings Default = new EmitterSettings();
	}
}
