using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000034 RID: 52
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NegativeVoteReasonGroup : IEquatable<NegativeVoteReasonGroup>
	{
		// Token: 0x060004DE RID: 1246 RVA: 0x0002332D File Offset: 0x0002152D
		public NegativeVoteReasonGroup(string Heading, IReadOnlyList<NegativeVoteReason> Reasons)
		{
			this.Heading = Heading;
			this.Reasons = Reasons;
			base..ctor();
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00023343 File Offset: 0x00021543
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(NegativeVoteReasonGroup);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0002334F File Offset: 0x0002154F
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x00023357 File Offset: 0x00021557
		public string Heading { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00023360 File Offset: 0x00021560
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00023368 File Offset: 0x00021568
		public IReadOnlyList<NegativeVoteReason> Reasons { get; set; }

		// Token: 0x060004E4 RID: 1252 RVA: 0x00023374 File Offset: 0x00021574
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("NegativeVoteReasonGroup");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x000233C0 File Offset: 0x000215C0
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Heading = ");
			builder.Append(this.Heading);
			builder.Append(", Reasons = ");
			builder.Append(this.Reasons);
			return true;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000233FA File Offset: 0x000215FA
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(NegativeVoteReasonGroup left, NegativeVoteReasonGroup right)
		{
			return !(left == right);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00023406 File Offset: 0x00021606
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(NegativeVoteReasonGroup left, NegativeVoteReasonGroup right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0002341A File Offset: 0x0002161A
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Heading>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<NegativeVoteReason>>.Default.GetHashCode(this.<Reasons>k__BackingField);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0002345A File Offset: 0x0002165A
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as NegativeVoteReasonGroup);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00023468 File Offset: 0x00021668
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(NegativeVoteReasonGroup other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Heading>k__BackingField, other.<Heading>k__BackingField) && EqualityComparer<IReadOnlyList<NegativeVoteReason>>.Default.Equals(this.<Reasons>k__BackingField, other.<Reasons>k__BackingField));
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000234C9 File Offset: 0x000216C9
		[CompilerGenerated]
		private NegativeVoteReasonGroup(NegativeVoteReasonGroup original)
		{
			this.Heading = original.<Heading>k__BackingField;
			this.Reasons = original.<Reasons>k__BackingField;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000234E9 File Offset: 0x000216E9
		[CompilerGenerated]
		public void Deconstruct(out string Heading, out IReadOnlyList<NegativeVoteReason> Reasons)
		{
			Heading = this.Heading;
			Reasons = this.Reasons;
		}
	}
}
