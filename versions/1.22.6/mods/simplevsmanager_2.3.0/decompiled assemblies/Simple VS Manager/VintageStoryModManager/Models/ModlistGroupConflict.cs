using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000AF RID: 175
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModlistGroupConflict : IEquatable<ModlistGroupConflict>
	{
		// Token: 0x06000EF6 RID: 3830 RVA: 0x00052CC3 File Offset: 0x00050EC3
		public ModlistGroupConflict(string ModId, IReadOnlyList<ModlistGroupConflictChoice> Choices, [Nullable(2)] string SelectedMemberReference)
		{
			this.ModId = ModId;
			this.Choices = Choices;
			this.SelectedMemberReference = SelectedMemberReference;
			base..ctor();
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x00052CE0 File Offset: 0x00050EE0
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ModlistGroupConflict);
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x00052CEC File Offset: 0x00050EEC
		// (set) Token: 0x06000EF9 RID: 3833 RVA: 0x00052CF4 File Offset: 0x00050EF4
		public string ModId { get; set; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000EFA RID: 3834 RVA: 0x00052CFD File Offset: 0x00050EFD
		// (set) Token: 0x06000EFB RID: 3835 RVA: 0x00052D05 File Offset: 0x00050F05
		public IReadOnlyList<ModlistGroupConflictChoice> Choices { get; set; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000EFC RID: 3836 RVA: 0x00052D0E File Offset: 0x00050F0E
		// (set) Token: 0x06000EFD RID: 3837 RVA: 0x00052D16 File Offset: 0x00050F16
		[Nullable(2)]
		public string SelectedMemberReference { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x00052D1F File Offset: 0x00050F1F
		public bool IsResolved
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.SelectedMemberReference) && Enumerable.Any<ModlistGroupConflictChoice>(this.Choices, (ModlistGroupConflictChoice choice) => string.Equals(choice.MemberReference, this.SelectedMemberReference, 5));
			}
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00052D48 File Offset: 0x00050F48
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModlistGroupConflict");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00052D94 File Offset: 0x00050F94
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("ModId = ");
			builder.Append(this.ModId);
			builder.Append(", Choices = ");
			builder.Append(this.Choices);
			builder.Append(", SelectedMemberReference = ");
			builder.Append(this.SelectedMemberReference);
			builder.Append(", IsResolved = ");
			builder.Append(this.IsResolved.ToString());
			return true;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00052E19 File Offset: 0x00051019
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ModlistGroupConflict left, ModlistGroupConflict right)
		{
			return !(left == right);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00052E25 File Offset: 0x00051025
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ModlistGroupConflict left, ModlistGroupConflict right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00052E3C File Offset: 0x0005103C
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ModId>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<ModlistGroupConflictChoice>>.Default.GetHashCode(this.<Choices>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SelectedMemberReference>k__BackingField);
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00052E9E File Offset: 0x0005109E
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModlistGroupConflict);
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00052EAC File Offset: 0x000510AC
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ModlistGroupConflict other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<ModId>k__BackingField, other.<ModId>k__BackingField) && EqualityComparer<IReadOnlyList<ModlistGroupConflictChoice>>.Default.Equals(this.<Choices>k__BackingField, other.<Choices>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<SelectedMemberReference>k__BackingField, other.<SelectedMemberReference>k__BackingField));
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00052F25 File Offset: 0x00051125
		[CompilerGenerated]
		private ModlistGroupConflict(ModlistGroupConflict original)
		{
			this.ModId = original.<ModId>k__BackingField;
			this.Choices = original.<Choices>k__BackingField;
			this.SelectedMemberReference = original.<SelectedMemberReference>k__BackingField;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00052F51 File Offset: 0x00051151
		[CompilerGenerated]
		public void Deconstruct(out string ModId, out IReadOnlyList<ModlistGroupConflictChoice> Choices, [Nullable(2)] out string SelectedMemberReference)
		{
			ModId = this.ModId;
			Choices = this.Choices;
			SelectedMemberReference = this.SelectedMemberReference;
		}
	}
}
