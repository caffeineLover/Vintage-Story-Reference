using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuestPDF.Skia.Text
{
	// Token: 0x02000028 RID: 40
	internal struct ParagraphStyleConfiguration : IEquatable<ParagraphStyleConfiguration>
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00003A84 File Offset: 0x00001C84
		[CompilerGenerated]
		public override readonly string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ParagraphStyleConfiguration");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003AD0 File Offset: 0x00001CD0
		[CompilerGenerated]
		private readonly bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Alignment = ");
			ParagraphStyleConfiguration.TextAlign alignment = this.Alignment;
			builder.Append(alignment.ToString());
			builder.Append(", Direction = ");
			ParagraphStyleConfiguration.TextDirection direction = this.Direction;
			builder.Append(direction.ToString());
			builder.Append(", MaxLinesVisible = ");
			int maxLinesVisible = this.MaxLinesVisible;
			builder.Append(maxLinesVisible.ToString());
			builder.Append(", LineClampEllipsis = ");
			IntPtr lineClampEllipsis = this.LineClampEllipsis;
			builder.Append(lineClampEllipsis.ToString());
			return true;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003B7A File Offset: 0x00001D7A
		[CompilerGenerated]
		public static bool operator !=(ParagraphStyleConfiguration left, ParagraphStyleConfiguration right)
		{
			return !(left == right);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003B86 File Offset: 0x00001D86
		[CompilerGenerated]
		public static bool operator ==(ParagraphStyleConfiguration left, ParagraphStyleConfiguration right)
		{
			return left.Equals(right);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003B90 File Offset: 0x00001D90
		[CompilerGenerated]
		public override readonly int GetHashCode()
		{
			return ((EqualityComparer<ParagraphStyleConfiguration.TextAlign>.Default.GetHashCode(this.Alignment) * -1521134295 + EqualityComparer<ParagraphStyleConfiguration.TextDirection>.Default.GetHashCode(this.Direction)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.MaxLinesVisible)) * -1521134295 + EqualityComparer<IntPtr>.Default.GetHashCode(this.LineClampEllipsis);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00003BF2 File Offset: 0x00001DF2
		[CompilerGenerated]
		public override readonly bool Equals(object obj)
		{
			return obj is ParagraphStyleConfiguration && this.Equals((ParagraphStyleConfiguration)obj);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00003C0C File Offset: 0x00001E0C
		[CompilerGenerated]
		public readonly bool Equals(ParagraphStyleConfiguration other)
		{
			return EqualityComparer<ParagraphStyleConfiguration.TextAlign>.Default.Equals(this.Alignment, other.Alignment) && EqualityComparer<ParagraphStyleConfiguration.TextDirection>.Default.Equals(this.Direction, other.Direction) && EqualityComparer<int>.Default.Equals(this.MaxLinesVisible, other.MaxLinesVisible) && EqualityComparer<IntPtr>.Default.Equals(this.LineClampEllipsis, other.LineClampEllipsis);
		}

		// Token: 0x0400007E RID: 126
		public ParagraphStyleConfiguration.TextAlign Alignment;

		// Token: 0x0400007F RID: 127
		public ParagraphStyleConfiguration.TextDirection Direction;

		// Token: 0x04000080 RID: 128
		public int MaxLinesVisible;

		// Token: 0x04000081 RID: 129
		public IntPtr LineClampEllipsis;

		// Token: 0x02000149 RID: 329
		internal enum TextAlign
		{
			// Token: 0x0400043B RID: 1083
			Left,
			// Token: 0x0400043C RID: 1084
			Right,
			// Token: 0x0400043D RID: 1085
			Center,
			// Token: 0x0400043E RID: 1086
			Justify,
			// Token: 0x0400043F RID: 1087
			Start,
			// Token: 0x04000440 RID: 1088
			End
		}

		// Token: 0x0200014A RID: 330
		internal enum TextDirection
		{
			// Token: 0x04000442 RID: 1090
			Rtl,
			// Token: 0x04000443 RID: 1091
			Ltr
		}
	}
}
