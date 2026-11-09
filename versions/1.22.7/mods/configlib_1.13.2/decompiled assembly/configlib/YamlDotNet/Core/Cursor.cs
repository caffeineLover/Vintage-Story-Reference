using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001A8 RID: 424
	[DebuggerStepThrough]
	public sealed class Cursor
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x0001CC0D File Offset: 0x0001AE0D
		// (set) Token: 0x060008FE RID: 2302 RVA: 0x0001CC15 File Offset: 0x0001AE15
		public int Index { get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x0001CC1E File Offset: 0x0001AE1E
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x0001CC26 File Offset: 0x0001AE26
		public int Line { get; private set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x0001CC2F File Offset: 0x0001AE2F
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x0001CC37 File Offset: 0x0001AE37
		public int LineOffset { get; private set; }

		// Token: 0x06000903 RID: 2307 RVA: 0x0001CC40 File Offset: 0x0001AE40
		public Cursor()
		{
			this.Line = 1;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001CC4F File Offset: 0x0001AE4F
		[NullableContext(1)]
		public Cursor(Cursor cursor)
		{
			this.Index = cursor.Index;
			this.Line = cursor.Line;
			this.LineOffset = cursor.LineOffset;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001CC7B File Offset: 0x0001AE7B
		public Mark Mark()
		{
			return new Mark(this.Index, this.Line, this.LineOffset + 1);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0001CC98 File Offset: 0x0001AE98
		public void Skip()
		{
			int num = this.Index;
			this.Index = num + 1;
			num = this.LineOffset;
			this.LineOffset = num + 1;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001CCC8 File Offset: 0x0001AEC8
		public void SkipLineByOffset(int offset)
		{
			this.Index += offset;
			int line = this.Line;
			this.Line = line + 1;
			this.LineOffset = 0;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001CCFC File Offset: 0x0001AEFC
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
