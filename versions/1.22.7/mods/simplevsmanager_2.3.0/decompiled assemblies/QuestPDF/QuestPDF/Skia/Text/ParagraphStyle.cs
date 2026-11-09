using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuestPDF.Skia.Text
{
	// Token: 0x0200002A RID: 42
	[NullableContext(1)]
	[Nullable(0)]
	internal class ParagraphStyle : IEquatable<ParagraphStyle>
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00003CAA File Offset: 0x00001EAA
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ParagraphStyle);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00003CB6 File Offset: 0x00001EB6
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00003CBE File Offset: 0x00001EBE
		public ParagraphStyleConfiguration.TextAlign Alignment { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00003CC7 File Offset: 0x00001EC7
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00003CCF File Offset: 0x00001ECF
		public ParagraphStyleConfiguration.TextDirection Direction { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00003CD8 File Offset: 0x00001ED8
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00003CE0 File Offset: 0x00001EE0
		public int MaxLinesVisible { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00003CE9 File Offset: 0x00001EE9
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00003CF1 File Offset: 0x00001EF1
		public string LineClampEllipsis { get; set; }

		// Token: 0x060000EA RID: 234 RVA: 0x00003CFC File Offset: 0x00001EFC
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ParagraphStyle");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00003D48 File Offset: 0x00001F48
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Alignment = ");
			builder.Append(this.Alignment.ToString());
			builder.Append(", Direction = ");
			builder.Append(this.Direction.ToString());
			builder.Append(", MaxLinesVisible = ");
			builder.Append(this.MaxLinesVisible.ToString());
			builder.Append(", LineClampEllipsis = ");
			builder.Append(this.LineClampEllipsis);
			return true;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00003DE9 File Offset: 0x00001FE9
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ParagraphStyle left, ParagraphStyle right)
		{
			return !(left == right);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00003DF5 File Offset: 0x00001FF5
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ParagraphStyle left, ParagraphStyle right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00003E0C File Offset: 0x0000200C
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ParagraphStyleConfiguration.TextAlign>.Default.GetHashCode(this.<Alignment>k__BackingField)) * -1521134295 + EqualityComparer<ParagraphStyleConfiguration.TextDirection>.Default.GetHashCode(this.<Direction>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<MaxLinesVisible>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<LineClampEllipsis>k__BackingField);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00003E85 File Offset: 0x00002085
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ParagraphStyle);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00003E94 File Offset: 0x00002094
		[NullableContext(2)]
		[CompilerGenerated]
		public virtual bool Equals(ParagraphStyle other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ParagraphStyleConfiguration.TextAlign>.Default.Equals(this.<Alignment>k__BackingField, other.<Alignment>k__BackingField) && EqualityComparer<ParagraphStyleConfiguration.TextDirection>.Default.Equals(this.<Direction>k__BackingField, other.<Direction>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<MaxLinesVisible>k__BackingField, other.<MaxLinesVisible>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<LineClampEllipsis>k__BackingField, other.<LineClampEllipsis>k__BackingField));
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00003F25 File Offset: 0x00002125
		[CompilerGenerated]
		protected ParagraphStyle(ParagraphStyle original)
		{
			this.Alignment = original.<Alignment>k__BackingField;
			this.Direction = original.<Direction>k__BackingField;
			this.MaxLinesVisible = original.<MaxLinesVisible>k__BackingField;
			this.LineClampEllipsis = original.<LineClampEllipsis>k__BackingField;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00003F5D File Offset: 0x0000215D
		public ParagraphStyle()
		{
		}
	}
}
