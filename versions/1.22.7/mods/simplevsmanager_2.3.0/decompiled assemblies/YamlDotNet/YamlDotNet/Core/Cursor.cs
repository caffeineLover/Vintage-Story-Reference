using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000C4 RID: 196
	[DebuggerStepThrough]
	public sealed class Cursor
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0000EFFB File Offset: 0x0000D1FB
		// (set) Token: 0x060004A5 RID: 1189 RVA: 0x0000F003 File Offset: 0x0000D203
		public long Index { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0000F00C File Offset: 0x0000D20C
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x0000F014 File Offset: 0x0000D214
		public long Line { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0000F01D File Offset: 0x0000D21D
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x0000F025 File Offset: 0x0000D225
		public long LineOffset { get; private set; }

		// Token: 0x060004AA RID: 1194 RVA: 0x0000F02E File Offset: 0x0000D22E
		public Cursor()
		{
			this.Line = 1L;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000F03E File Offset: 0x0000D23E
		[NullableContext(1)]
		public Cursor(Cursor cursor)
		{
			this.Index = cursor.Index;
			this.Line = cursor.Line;
			this.LineOffset = cursor.LineOffset;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000F06A File Offset: 0x0000D26A
		public Mark Mark()
		{
			return new Mark(this.Index, this.Line, this.LineOffset + 1L);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000F088 File Offset: 0x0000D288
		public void Skip()
		{
			long num = this.Index;
			this.Index = num + 1L;
			num = this.LineOffset;
			this.LineOffset = num + 1L;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000F0B8 File Offset: 0x0000D2B8
		public void SkipLineByOffset(int offset)
		{
			this.Index += (long)offset;
			long line = this.Line;
			this.Line = line + 1L;
			this.LineOffset = 0L;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		public void ForceSkipLineAfterNonBreak()
		{
			if (this.LineOffset != 0L)
			{
				long line = this.Line;
				this.Line = line + 1L;
				this.LineOffset = 0L;
			}
		}
	}
}
