using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Companion;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000063 RID: 99
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct SourceCodePath
	{
		// Token: 0x0600025F RID: 607 RVA: 0x00005A04 File Offset: 0x00003C04
		public SourceCodePath(StackFrame frame)
		{
			this.FilePath = (frame.GetFileName() ?? string.Empty);
			this.LineNumber = frame.GetFileLineNumber();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00005A28 File Offset: 0x00003C28
		internal static SourceCodePath? CreateFromCurrentStackTrace()
		{
			if (!CompanionService.IsCompanionAttached)
			{
				return default(SourceCodePath?);
			}
			if (CompanionService.IsDocumentHotReloaded)
			{
				return default(SourceCodePath?);
			}
			StackFrame stackFrame = Enumerable.FirstOrDefault<StackFrame>(new StackTrace(true).GetFrames(), (StackFrame x) => StackFrameExtensions.HasSource(x) && StackFrameExtensions.HasMethod(x));
			if (stackFrame == null)
			{
				return default(SourceCodePath?);
			}
			return new SourceCodePath?(new SourceCodePath(stackFrame));
		}

		// Token: 0x04000140 RID: 320
		public readonly string FilePath;

		// Token: 0x04000141 RID: 321
		public readonly int LineNumber;
	}
}
