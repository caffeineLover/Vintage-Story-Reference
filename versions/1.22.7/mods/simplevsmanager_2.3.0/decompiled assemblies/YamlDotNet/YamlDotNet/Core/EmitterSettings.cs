using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000C6 RID: 198
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmitterSettings
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x000114E9 File Offset: 0x0000F6E9
		public int BestIndent { get; } = 2;

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x000114F1 File Offset: 0x0000F6F1
		public int BestWidth { get; } = int.MaxValue;

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x000114F9 File Offset: 0x0000F6F9
		public string NewLine { get; } = Environment.NewLine;

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00011501 File Offset: 0x0000F701
		public bool IsCanonical { get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00011509 File Offset: 0x0000F709
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x00011511 File Offset: 0x0000F711
		public bool SkipAnchorName { get; private set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0001151A File Offset: 0x0000F71A
		public int MaxSimpleKeyLength { get; } = 1024;

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x00011522 File Offset: 0x0000F722
		public bool IndentSequences { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x0001152A File Offset: 0x0000F72A
		public bool UseUtf16SurrogatePairs { get; }

		// Token: 0x060004FC RID: 1276 RVA: 0x00011532 File Offset: 0x0000F732
		public EmitterSettings()
		{
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00011564 File Offset: 0x0000F764
		[NullableContext(2)]
		public EmitterSettings(int bestIndent, int bestWidth, bool isCanonical, int maxSimpleKeyLength, bool skipAnchorName = false, bool indentSequences = false, string newLine = null, bool useUtf16SurrogatePairs = false)
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
			this.UseUtf16SurrogatePairs = useUtf16SurrogatePairs;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00011629 File Offset: 0x0000F829
		public EmitterSettings WithBestIndent(int bestIndent)
		{
			return new EmitterSettings(bestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, this.NewLine, this.UseUtf16SurrogatePairs);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001165B File Offset: 0x0000F85B
		public EmitterSettings WithBestWidth(int bestWidth)
		{
			return new EmitterSettings(this.BestIndent, bestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, this.NewLine, this.UseUtf16SurrogatePairs);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001168D File Offset: 0x0000F88D
		public EmitterSettings WithMaxSimpleKeyLength(int maxSimpleKeyLength)
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, maxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, this.NewLine, this.UseUtf16SurrogatePairs);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000116BF File Offset: 0x0000F8BF
		public EmitterSettings WithNewLine(string newLine)
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, newLine, this.UseUtf16SurrogatePairs);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x000116F1 File Offset: 0x0000F8F1
		public EmitterSettings Canonical()
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, true, this.MaxSimpleKeyLength, this.SkipAnchorName, false, null, false);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00011714 File Offset: 0x0000F914
		public EmitterSettings WithoutAnchorName()
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, true, this.IndentSequences, this.NewLine, this.UseUtf16SurrogatePairs);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00011746 File Offset: 0x0000F946
		public EmitterSettings WithIndentedSequences()
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, true, this.NewLine, this.UseUtf16SurrogatePairs);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00011778 File Offset: 0x0000F978
		public EmitterSettings WithUtf16SurrogatePairs()
		{
			return new EmitterSettings(this.BestIndent, this.BestWidth, this.IsCanonical, this.MaxSimpleKeyLength, this.SkipAnchorName, this.IndentSequences, this.NewLine, true);
		}

		// Token: 0x0400014A RID: 330
		public static readonly EmitterSettings Default = new EmitterSettings();
	}
}
