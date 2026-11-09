using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000051 RID: 81
	[NullableContext(1)]
	[Nullable(0)]
	internal class GetImageVersionRequest : IEquatable<GetImageVersionRequest>
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00005029 File Offset: 0x00003229
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(GetImageVersionRequest);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00005035 File Offset: 0x00003235
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000503D File Offset: 0x0000323D
		internal ImageSize Resolution { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00005046 File Offset: 0x00003246
		// (set) Token: 0x060001EA RID: 490 RVA: 0x0000504E File Offset: 0x0000324E
		internal ImageCompressionQuality CompressionQuality { get; set; }

		// Token: 0x060001EB RID: 491 RVA: 0x00005058 File Offset: 0x00003258
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("GetImageVersionRequest");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000050A4 File Offset: 0x000032A4
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			return false;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000050A7 File Offset: 0x000032A7
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(GetImageVersionRequest left, GetImageVersionRequest right)
		{
			return !(left == right);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000050B3 File Offset: 0x000032B3
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(GetImageVersionRequest left, GetImageVersionRequest right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000050C7 File Offset: 0x000032C7
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ImageSize>.Default.GetHashCode(this.<Resolution>k__BackingField)) * -1521134295 + EqualityComparer<ImageCompressionQuality>.Default.GetHashCode(this.<CompressionQuality>k__BackingField);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00005107 File Offset: 0x00003307
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as GetImageVersionRequest);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00005118 File Offset: 0x00003318
		[NullableContext(2)]
		[CompilerGenerated]
		public virtual bool Equals(GetImageVersionRequest other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ImageSize>.Default.Equals(this.<Resolution>k__BackingField, other.<Resolution>k__BackingField) && EqualityComparer<ImageCompressionQuality>.Default.Equals(this.<CompressionQuality>k__BackingField, other.<CompressionQuality>k__BackingField));
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00005179 File Offset: 0x00003379
		[CompilerGenerated]
		protected GetImageVersionRequest(GetImageVersionRequest original)
		{
			this.Resolution = original.<Resolution>k__BackingField;
			this.CompressionQuality = original.<CompressionQuality>k__BackingField;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00005199 File Offset: 0x00003399
		public GetImageVersionRequest()
		{
		}
	}
}
