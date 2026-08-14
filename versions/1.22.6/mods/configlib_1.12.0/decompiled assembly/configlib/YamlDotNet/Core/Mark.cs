using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020001BD RID: 445
	public readonly struct Mark : IEquatable<Mark>, IComparable<Mark>, IComparable
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x000201BA File Offset: 0x0001E3BA
		public int Index { get; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x000201C2 File Offset: 0x0001E3C2
		public int Line { get; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x000201CA File Offset: 0x0001E3CA
		public int Column { get; }

		// Token: 0x060009CB RID: 2507 RVA: 0x000201D4 File Offset: 0x0001E3D4
		public Mark(int index, int line, int column)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException("index", "Index must be greater than or equal to zero.");
			}
			if (line < 1)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException("line", "Line must be greater than or equal to 1.");
			}
			if (column < 1)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException("column", "Column must be greater than or equal to 1.");
			}
			this.Index = index;
			this.Line = line;
			this.Column = column;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00020230 File Offset: 0x0001E430
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(20, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Line: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Line);
			defaultInterpolatedStringHandler.AppendLiteral(", Col: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Column);
			defaultInterpolatedStringHandler.AppendLiteral(", Idx: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Index);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00020299 File Offset: 0x0001E499
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			return this.Equals((Mark)(obj ?? Mark.Empty));
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x000202B5 File Offset: 0x0001E4B5
		public bool Equals(Mark other)
		{
			return this.Index == other.Index && this.Line == other.Line && this.Column == other.Column;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x000202E8 File Offset: 0x0001E4E8
		public override int GetHashCode()
		{
			return HashCode.CombineHashCodes(this.Index.GetHashCode(), HashCode.CombineHashCodes(this.Line.GetHashCode(), this.Column.GetHashCode()));
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00020329 File Offset: 0x0001E529
		[NullableContext(2)]
		public int CompareTo(object obj)
		{
			return this.CompareTo((Mark)(obj ?? Mark.Empty));
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00020348 File Offset: 0x0001E548
		public int CompareTo(Mark other)
		{
			int num = this.Line.CompareTo(other.Line);
			if (num == 0)
			{
				num = this.Column.CompareTo(other.Column);
			}
			return num;
		}

		// Token: 0x04000413 RID: 1043
		public static readonly Mark Empty = new Mark(0, 1, 1);
	}
}
