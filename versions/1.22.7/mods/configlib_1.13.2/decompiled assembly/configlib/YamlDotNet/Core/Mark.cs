using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020001B9 RID: 441
	public readonly struct Mark : IEquatable<Mark>, IComparable<Mark>, IComparable
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x0001F872 File Offset: 0x0001DA72
		public int Index { get; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0001F87A File Offset: 0x0001DA7A
		public int Line { get; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0001F882 File Offset: 0x0001DA82
		public int Column { get; }

		// Token: 0x0600098F RID: 2447 RVA: 0x0001F88C File Offset: 0x0001DA8C
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

		// Token: 0x06000990 RID: 2448 RVA: 0x0001F8E8 File Offset: 0x0001DAE8
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

		// Token: 0x06000991 RID: 2449 RVA: 0x0001F951 File Offset: 0x0001DB51
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			return this.Equals((Mark)(obj ?? Mark.Empty));
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0001F96D File Offset: 0x0001DB6D
		public bool Equals(Mark other)
		{
			return this.Index == other.Index && this.Line == other.Line && this.Column == other.Column;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0001F9A0 File Offset: 0x0001DBA0
		public override int GetHashCode()
		{
			return HashCode.CombineHashCodes(this.Index.GetHashCode(), HashCode.CombineHashCodes(this.Line.GetHashCode(), this.Column.GetHashCode()));
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0001F9E1 File Offset: 0x0001DBE1
		[NullableContext(2)]
		public int CompareTo(object obj)
		{
			return this.CompareTo((Mark)(obj ?? Mark.Empty));
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0001FA00 File Offset: 0x0001DC00
		public int CompareTo(Mark other)
		{
			int num = this.Line.CompareTo(other.Line);
			if (num == 0)
			{
				num = this.Column.CompareTo(other.Column);
			}
			return num;
		}

		// Token: 0x040003E5 RID: 997
		public static readonly Mark Empty = new Mark(0, 1, 1);
	}
}
