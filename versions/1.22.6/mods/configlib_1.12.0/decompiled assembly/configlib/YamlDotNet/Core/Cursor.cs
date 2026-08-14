using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001AC RID: 428
	[DebuggerStepThrough]
	public sealed class Cursor
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0001D555 File Offset: 0x0001B755
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x0001D55D File Offset: 0x0001B75D
		public int Index { get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0001D566 File Offset: 0x0001B766
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x0001D56E File Offset: 0x0001B76E
		public int Line { get; private set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x0001D577 File Offset: 0x0001B777
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x0001D57F File Offset: 0x0001B77F
		public int LineOffset { get; private set; }

		// Token: 0x0600093F RID: 2367 RVA: 0x0001D588 File Offset: 0x0001B788
		public Cursor()
		{
			this.Line = 1;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0001D597 File Offset: 0x0001B797
		[NullableContext(1)]
		public Cursor(Cursor cursor)
		{
			this.Index = cursor.Index;
			this.Line = cursor.Line;
			this.LineOffset = cursor.LineOffset;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0001D5C3 File Offset: 0x0001B7C3
		public Mark Mark()
		{
			return new Mark(this.Index, this.Line, this.LineOffset + 1);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0001D5E0 File Offset: 0x0001B7E0
		public void Skip()
		{
			int num = this.Index;
			this.Index = num + 1;
			num = this.LineOffset;
			this.LineOffset = num + 1;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0001D610 File Offset: 0x0001B810
		public void SkipLineByOffset(int offset)
		{
			this.Index += offset;
			int line = this.Line;
			this.Line = line + 1;
			this.LineOffset = 0;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0001D644 File Offset: 0x0001B844
		public void ForceSkipLineAfterNonBreak()
		{
			if (this.LineOffset != 0)
			{
				int line = this.Line;
				this.Line = line + 1;
				this.LineOffset = 0;
			}
		}
	}
}
