using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Core
{
	// Token: 0x020000D0 RID: 208
	public readonly struct Mark : IEquatable<Mark>, IComparable<Mark>, IComparable
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00011C96 File Offset: 0x0000FE96
		public long Index { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00011C9E File Offset: 0x0000FE9E
		public long Line { get; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00011CA6 File Offset: 0x0000FEA6
		public long Column { get; }

		// Token: 0x0600052C RID: 1324 RVA: 0x00011CB0 File Offset: 0x0000FEB0
		public Mark(long index, long line, long column)
		{
			if (index < 0L)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException("index", "Index must be greater than or equal to zero.");
			}
			if (line < 1L)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException("line", "Line must be greater than or equal to 1.");
			}
			if (column < 1L)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException("column", "Column must be greater than or equal to 1.");
			}
			this.Index = index;
			this.Line = line;
			this.Column = column;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00011D10 File Offset: 0x0000FF10
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(20, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Line: ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.Line);
			defaultInterpolatedStringHandler.AppendLiteral(", Col: ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.Column);
			defaultInterpolatedStringHandler.AppendLiteral(", Idx: ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.Index);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00011D79 File Offset: 0x0000FF79
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			return this.Equals((Mark)(obj ?? Mark.Empty));
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00011D95 File Offset: 0x0000FF95
		public bool Equals(Mark other)
		{
			return this.Index == other.Index && this.Line == other.Line && this.Column == other.Column;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00011DC8 File Offset: 0x0000FFC8
		public override int GetHashCode()
		{
			return HashCode.CombineHashCodes(this.Index.GetHashCode(), HashCode.CombineHashCodes(this.Line.GetHashCode(), this.Column.GetHashCode()));
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00011E09 File Offset: 0x00010009
		[NullableContext(2)]
		public int CompareTo(object obj)
		{
			return this.CompareTo((Mark)(obj ?? Mark.Empty));
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00011E28 File Offset: 0x00010028
		public int CompareTo(Mark other)
		{
			int num = this.Line.CompareTo(other.Line);
			if (num == 0)
			{
				num = this.Column.CompareTo(other.Column);
			}
			return num;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00011E65 File Offset: 0x00010065
		public static bool operator ==(Mark left, Mark right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00011E6F File Offset: 0x0001006F
		public static bool operator !=(Mark left, Mark right)
		{
			return !(left == right);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00011E7B File Offset: 0x0001007B
		public static bool operator <(Mark left, Mark right)
		{
			return left.CompareTo(right) < 0;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00011E88 File Offset: 0x00010088
		public static bool operator <=(Mark left, Mark right)
		{
			return left.CompareTo(right) <= 0;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00011E98 File Offset: 0x00010098
		public static bool operator >(Mark left, Mark right)
		{
			return left.CompareTo(right) > 0;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00011EA5 File Offset: 0x000100A5
		public static bool operator >=(Mark left, Mark right)
		{
			return left.CompareTo(right) >= 0;
		}

		// Token: 0x0400016C RID: 364
		public static readonly Mark Empty = new Mark(0L, 1L, 1L);
	}
}
