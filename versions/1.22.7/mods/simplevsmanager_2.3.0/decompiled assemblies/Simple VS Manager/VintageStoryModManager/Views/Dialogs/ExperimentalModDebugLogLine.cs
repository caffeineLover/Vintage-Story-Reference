using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000026 RID: 38
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ExperimentalModDebugLogLine
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x0002162E File Offset: 0x0001F82E
		[NullableContext(2)]
		private ExperimentalModDebugLogLine([Nullable(1)] string text, bool isHighlighted, string modName = null, string filePath = null, int lineNumber = 0)
		{
			this.Text = text;
			this.IsHighlighted = isHighlighted;
			this.ModName = modName;
			this.FilePath = filePath;
			this.LineNumber = lineNumber;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0002165B File Offset: 0x0001F85B
		public string Text { get; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00021663 File Offset: 0x0001F863
		public bool IsHighlighted { get; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0002166B File Offset: 0x0001F86B
		[Nullable(2)]
		public string ModName { [NullableContext(2)] get; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00021673 File Offset: 0x0001F873
		[Nullable(2)]
		public string FilePath { [NullableContext(2)] get; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0002167B File Offset: 0x0001F87B
		public int LineNumber { get; }

		// Token: 0x06000469 RID: 1129 RVA: 0x00021684 File Offset: 0x0001F884
		public static ExperimentalModDebugLogLine FromLogEntry(string rawText, [Nullable(2)] string modName = null, [Nullable(2)] string filePath = null, int lineNumber = 0)
		{
			ArgumentNullException.ThrowIfNull(rawText, "rawText");
			string text = (rawText.Length > 300) ? (MemoryExtensions.AsSpan(rawText, 0, 300) + "... (log line too long to show)") : rawText;
			bool isHighlighted = ExperimentalModDebugLogLine.ContainsHighlightKeyword(rawText);
			return new ExperimentalModDebugLogLine(text, isHighlighted, modName, filePath, lineNumber);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000216D7 File Offset: 0x0001F8D7
		public static ExperimentalModDebugLogLine FromPlainText(string text, [Nullable(2)] string modName = null, [Nullable(2)] string filePath = null, int lineNumber = 0)
		{
			ArgumentNullException.ThrowIfNull(text, "text");
			return new ExperimentalModDebugLogLine(text, false, modName, filePath, lineNumber);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000216F0 File Offset: 0x0001F8F0
		private static bool ContainsHighlightKeyword(string text)
		{
			foreach (string text2 in ExperimentalModDebugLogLine.HighlightKeywords)
			{
				if (text.IndexOf(text2, 5) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000257 RID: 599
		private const int MaxLogLineLength = 300;

		// Token: 0x04000258 RID: 600
		private static readonly string[] HighlightKeywords = new string[]
		{
			"error",
			"warning",
			"exception",
			"failed ",
			"aborting",
			"missing"
		};
	}
}
