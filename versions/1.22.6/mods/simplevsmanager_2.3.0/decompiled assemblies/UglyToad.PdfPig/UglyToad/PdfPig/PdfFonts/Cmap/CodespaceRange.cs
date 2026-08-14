using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.PdfFonts.Cmap
{
	// Token: 0x020000B6 RID: 182
	internal class CodespaceRange
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600034D RID: 845 RVA: 0x000152B4 File Offset: 0x000134B4
		public IReadOnlyList<byte> Start { get; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600034E RID: 846 RVA: 0x000152BC File Offset: 0x000134BC
		public IReadOnlyList<byte> End { get; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600034F RID: 847 RVA: 0x000152C4 File Offset: 0x000134C4
		public int StartInt { get; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000350 RID: 848 RVA: 0x000152CC File Offset: 0x000134CC
		public int EndInt { get; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000351 RID: 849 RVA: 0x000152D4 File Offset: 0x000134D4
		public int CodeLength { get; }

		// Token: 0x06000352 RID: 850 RVA: 0x000152DC File Offset: 0x000134DC
		public CodespaceRange(IReadOnlyList<byte> start, IReadOnlyList<byte> end)
		{
			this.Start = start;
			this.End = end;
			this.StartInt = start.ToInt(start.Count);
			this.EndInt = end.ToInt(end.Count);
			this.CodeLength = start.Count;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0001532D File Offset: 0x0001352D
		public bool Matches(byte[] code)
		{
			if (code == null)
			{
				throw new ArgumentNullException("code");
			}
			return this.IsFullMatch(code, code.Length);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00015348 File Offset: 0x00013548
		public bool IsFullMatch(byte[] code, int codeLength)
		{
			if (code == null)
			{
				throw new ArgumentNullException("code");
			}
			if (codeLength != this.CodeLength)
			{
				return false;
			}
			int num = code.ToInt(codeLength);
			return num >= this.StartInt && num <= this.EndInt;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0001538C File Offset: 0x0001358C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(13, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Length ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.CodeLength);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.StartInt);
			defaultInterpolatedStringHandler.AppendLiteral(" -> ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.EndInt);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
