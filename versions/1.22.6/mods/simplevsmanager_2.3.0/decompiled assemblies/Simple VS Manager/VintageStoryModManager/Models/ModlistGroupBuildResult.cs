using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000B0 RID: 176
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModlistGroupBuildResult : IEquatable<ModlistGroupBuildResult>
	{
		// Token: 0x06000F0A RID: 3850 RVA: 0x00052F7F File Offset: 0x0005117F
		public ModlistGroupBuildResult(SerializablePreset MergedPreset, IReadOnlyList<ModlistGroupConflict> Conflicts, IReadOnlyList<string> MissingMembers)
		{
			this.MergedPreset = MergedPreset;
			this.Conflicts = Conflicts;
			this.MissingMembers = MissingMembers;
			base..ctor();
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x00052F9C File Offset: 0x0005119C
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ModlistGroupBuildResult);
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x00052FA8 File Offset: 0x000511A8
		// (set) Token: 0x06000F0D RID: 3853 RVA: 0x00052FB0 File Offset: 0x000511B0
		public SerializablePreset MergedPreset { get; set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x00052FB9 File Offset: 0x000511B9
		// (set) Token: 0x06000F0F RID: 3855 RVA: 0x00052FC1 File Offset: 0x000511C1
		public IReadOnlyList<ModlistGroupConflict> Conflicts { get; set; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x00052FCA File Offset: 0x000511CA
		// (set) Token: 0x06000F11 RID: 3857 RVA: 0x00052FD2 File Offset: 0x000511D2
		public IReadOnlyList<string> MissingMembers { get; set; }

		// Token: 0x06000F12 RID: 3858 RVA: 0x00052FDC File Offset: 0x000511DC
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModlistGroupBuildResult");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00053028 File Offset: 0x00051228
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("MergedPreset = ");
			builder.Append(this.MergedPreset);
			builder.Append(", Conflicts = ");
			builder.Append(this.Conflicts);
			builder.Append(", MissingMembers = ");
			builder.Append(this.MissingMembers);
			return true;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00053086 File Offset: 0x00051286
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ModlistGroupBuildResult left, ModlistGroupBuildResult right)
		{
			return !(left == right);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00053092 File Offset: 0x00051292
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ModlistGroupBuildResult left, ModlistGroupBuildResult right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x000530A8 File Offset: 0x000512A8
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<SerializablePreset>.Default.GetHashCode(this.<MergedPreset>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<ModlistGroupConflict>>.Default.GetHashCode(this.<Conflicts>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<string>>.Default.GetHashCode(this.<MissingMembers>k__BackingField);
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0005310A File Offset: 0x0005130A
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModlistGroupBuildResult);
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00053118 File Offset: 0x00051318
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ModlistGroupBuildResult other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<SerializablePreset>.Default.Equals(this.<MergedPreset>k__BackingField, other.<MergedPreset>k__BackingField) && EqualityComparer<IReadOnlyList<ModlistGroupConflict>>.Default.Equals(this.<Conflicts>k__BackingField, other.<Conflicts>k__BackingField) && EqualityComparer<IReadOnlyList<string>>.Default.Equals(this.<MissingMembers>k__BackingField, other.<MissingMembers>k__BackingField));
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x00053191 File Offset: 0x00051391
		[CompilerGenerated]
		private ModlistGroupBuildResult(ModlistGroupBuildResult original)
		{
			this.MergedPreset = original.<MergedPreset>k__BackingField;
			this.Conflicts = original.<Conflicts>k__BackingField;
			this.MissingMembers = original.<MissingMembers>k__BackingField;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x000531BD File Offset: 0x000513BD
		[CompilerGenerated]
		public void Deconstruct(out SerializablePreset MergedPreset, out IReadOnlyList<ModlistGroupConflict> Conflicts, out IReadOnlyList<string> MissingMembers)
		{
			MergedPreset = this.MergedPreset;
			Conflicts = this.Conflicts;
			MissingMembers = this.MissingMembers;
		}
	}
}
