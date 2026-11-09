using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000AD RID: 173
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModlistGroupMemberListEntry : IEquatable<ModlistGroupMemberListEntry>
	{
		// Token: 0x06000EC3 RID: 3779 RVA: 0x00052420 File Offset: 0x00050620
		public ModlistGroupMemberListEntry(string GroupFilePath, string GroupName, string MemberReference, [Nullable(2)] string SourceFilePath, string DisplayName, bool IsMissing)
		{
			this.GroupFilePath = GroupFilePath;
			this.GroupName = GroupName;
			this.MemberReference = MemberReference;
			this.SourceFilePath = SourceFilePath;
			this.DisplayName = DisplayName;
			this.IsMissing = IsMissing;
			base..ctor();
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x00052455 File Offset: 0x00050655
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ModlistGroupMemberListEntry);
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x00052461 File Offset: 0x00050661
		// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x00052469 File Offset: 0x00050669
		public string GroupFilePath { get; set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x00052472 File Offset: 0x00050672
		// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x0005247A File Offset: 0x0005067A
		public string GroupName { get; set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x00052483 File Offset: 0x00050683
		// (set) Token: 0x06000ECA RID: 3786 RVA: 0x0005248B File Offset: 0x0005068B
		public string MemberReference { get; set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00052494 File Offset: 0x00050694
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x0005249C File Offset: 0x0005069C
		[Nullable(2)]
		public string SourceFilePath { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x000524A5 File Offset: 0x000506A5
		// (set) Token: 0x06000ECE RID: 3790 RVA: 0x000524AD File Offset: 0x000506AD
		public string DisplayName { get; set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x000524B6 File Offset: 0x000506B6
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x000524BE File Offset: 0x000506BE
		public bool IsMissing { get; set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x000524C7 File Offset: 0x000506C7
		public bool IsAvailable
		{
			get
			{
				return !this.IsMissing && !string.IsNullOrWhiteSpace(this.SourceFilePath);
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x000524E1 File Offset: 0x000506E1
		public string DisplayNameWithStatus
		{
			get
			{
				if (!this.IsMissing)
				{
					return this.DisplayName;
				}
				return this.DisplayName + " (Missing)";
			}
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00052504 File Offset: 0x00050704
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModlistGroupMemberListEntry");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00052550 File Offset: 0x00050750
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("GroupFilePath = ");
			builder.Append(this.GroupFilePath);
			builder.Append(", GroupName = ");
			builder.Append(this.GroupName);
			builder.Append(", MemberReference = ");
			builder.Append(this.MemberReference);
			builder.Append(", SourceFilePath = ");
			builder.Append(this.SourceFilePath);
			builder.Append(", DisplayName = ");
			builder.Append(this.DisplayName);
			builder.Append(", IsMissing = ");
			builder.Append(this.IsMissing.ToString());
			builder.Append(", IsAvailable = ");
			builder.Append(this.IsAvailable.ToString());
			builder.Append(", DisplayNameWithStatus = ");
			builder.Append(this.DisplayNameWithStatus);
			return true;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00052647 File Offset: 0x00050847
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ModlistGroupMemberListEntry left, ModlistGroupMemberListEntry right)
		{
			return !(left == right);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00052653 File Offset: 0x00050853
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ModlistGroupMemberListEntry left, ModlistGroupMemberListEntry right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00052668 File Offset: 0x00050868
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<GroupFilePath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<GroupName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<MemberReference>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SourceFilePath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<DisplayName>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsMissing>k__BackingField);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0005270F File Offset: 0x0005090F
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModlistGroupMemberListEntry);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00052720 File Offset: 0x00050920
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ModlistGroupMemberListEntry other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<GroupFilePath>k__BackingField, other.<GroupFilePath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<GroupName>k__BackingField, other.<GroupName>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<MemberReference>k__BackingField, other.<MemberReference>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<SourceFilePath>k__BackingField, other.<SourceFilePath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<DisplayName>k__BackingField, other.<DisplayName>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsMissing>k__BackingField, other.<IsMissing>k__BackingField));
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000527EC File Offset: 0x000509EC
		[CompilerGenerated]
		private ModlistGroupMemberListEntry(ModlistGroupMemberListEntry original)
		{
			this.GroupFilePath = original.<GroupFilePath>k__BackingField;
			this.GroupName = original.<GroupName>k__BackingField;
			this.MemberReference = original.<MemberReference>k__BackingField;
			this.SourceFilePath = original.<SourceFilePath>k__BackingField;
			this.DisplayName = original.<DisplayName>k__BackingField;
			this.IsMissing = original.<IsMissing>k__BackingField;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00052847 File Offset: 0x00050A47
		[CompilerGenerated]
		public void Deconstruct(out string GroupFilePath, out string GroupName, out string MemberReference, [Nullable(2)] out string SourceFilePath, out string DisplayName, out bool IsMissing)
		{
			GroupFilePath = this.GroupFilePath;
			GroupName = this.GroupName;
			MemberReference = this.MemberReference;
			SourceFilePath = this.SourceFilePath;
			DisplayName = this.DisplayName;
			IsMissing = this.IsMissing;
		}
	}
}
