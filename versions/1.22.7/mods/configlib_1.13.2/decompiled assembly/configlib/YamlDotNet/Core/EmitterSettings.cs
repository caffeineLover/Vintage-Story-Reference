using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001AE RID: 430
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmitterSettings
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0001F098 File Offset: 0x0001D298
		public int BestIndent { get; } = 2;

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0001F0A0 File Offset: 0x0001D2A0
		public int BestWidth { get; } = int.MaxValue;

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x0001F0A8 File Offset: 0x0001D2A8
		public string NewLine { get; } = Environment.NewLine;

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x0001F0B0 File Offset: 0x0001D2B0
		public bool IsCanonical { get; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x0001F0B8 File Offset: 0x0001D2B8
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x0001F0C0 File Offset: 0x0001D2C0
		public bool SkipAnchorName { get; private set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x0001F0C9 File Offset: 0x0001D2C9
		public int MaxSimpleKeyLength { get; } = 1024;

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0001F0D1 File Offset: 0x0001D2D1
		public bool IndentSequences { get; }

		// Token: 0x0600095A RID: 2394 RVA: 0x0001F0D9 File Offset: 0x0001D2D9
		public EmitterSettings()
		{
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0001F10C File Offset: 0x0001D30C
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

		// Token: 0x0600095C RID: 2396 RVA: 0x0001F1C9 File Offset: 0x0001D3C9
		public EmitterSettings WithBestIndent(int bestIndent)
		{
			return new EmitterSettings(bestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, this.NewLine);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0001F1F5 File Offset: 0x0001D3F5
		public EmitterSettings WithBestWidth(int bestWidth)
		{
			return new EmitterSettings(this.BestIndent, bestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, this.NewLine);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0001F221 File Offset: 0x0001D421
		public EmitterSettings WithMaxSimpleKeyLength(int maxSimpleKeyLength)
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, maxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, this.NewLine);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0001F24D File Offset: 0x0001D44D
		public EmitterSettings WithNewLine(string newLine)
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, newLine);
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0001F279 File Offset: 0x0001D479
		public EmitterSettings Canonical()
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, true, this.MaxSimpleKeyLength, this.SkipAnchorName, false, null);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001F29B File Offset: 0x0001D49B
		public EmitterSettings WithoutAnchorName()
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, true, this.IndentSequences, this.NewLine);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0001F2C7 File Offset: 0x0001D4C7
		public EmitterSettings WithIndentedSequences()
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, true, this.NewLine);
		}

		// Token: 0x040003BE RID: 958
		public static readonly EmitterSettings Default = new EmitterSettings();
	}
}
