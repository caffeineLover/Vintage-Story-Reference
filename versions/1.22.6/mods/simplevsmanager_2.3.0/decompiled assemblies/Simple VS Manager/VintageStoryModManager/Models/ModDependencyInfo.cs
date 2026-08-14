using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000A5 RID: 165
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModDependencyInfo : IEquatable<ModDependencyInfo>
	{
		// Token: 0x06000DF3 RID: 3571 RVA: 0x0005185C File Offset: 0x0004FA5C
		public ModDependencyInfo(string ModId, string Version)
		{
			this.ModId = ModId;
			this.Version = Version;
			base..ctor();
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x00051872 File Offset: 0x0004FA72
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ModDependencyInfo);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000DF5 RID: 3573 RVA: 0x0005187E File Offset: 0x0004FA7E
		// (set) Token: 0x06000DF6 RID: 3574 RVA: 0x00051886 File Offset: 0x0004FA86
		public string ModId { get; set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x0005188F File Offset: 0x0004FA8F
		// (set) Token: 0x06000DF8 RID: 3576 RVA: 0x00051897 File Offset: 0x0004FA97
		public string Version { get; set; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x000518A0 File Offset: 0x0004FAA0
		public string Display
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.Version))
				{
					return this.ModId + " (≥ " + this.Version + ")";
				}
				return this.ModId;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x000518D1 File Offset: 0x0004FAD1
		public bool IsGameOrCoreDependency
		{
			get
			{
				return string.Equals(this.ModId, "game", 5) || string.Equals(this.ModId, "creative", 5) || string.Equals(this.ModId, "survival", 5);
			}
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0005190C File Offset: 0x0004FB0C
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModDependencyInfo");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00051958 File Offset: 0x0004FB58
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("ModId = ");
			builder.Append(this.ModId);
			builder.Append(", Version = ");
			builder.Append(this.Version);
			builder.Append(", Display = ");
			builder.Append(this.Display);
			builder.Append(", IsGameOrCoreDependency = ");
			builder.Append(this.IsGameOrCoreDependency.ToString());
			return true;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x000519DD File Offset: 0x0004FBDD
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ModDependencyInfo left, ModDependencyInfo right)
		{
			return !(left == right);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x000519E9 File Offset: 0x0004FBE9
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ModDependencyInfo left, ModDependencyInfo right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x000519FD File Offset: 0x0004FBFD
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ModId>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Version>k__BackingField);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00051A3D File Offset: 0x0004FC3D
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModDependencyInfo);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00051A4C File Offset: 0x0004FC4C
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ModDependencyInfo other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<ModId>k__BackingField, other.<ModId>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Version>k__BackingField, other.<Version>k__BackingField));
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00051AAD File Offset: 0x0004FCAD
		[CompilerGenerated]
		private ModDependencyInfo(ModDependencyInfo original)
		{
			this.ModId = original.<ModId>k__BackingField;
			this.Version = original.<Version>k__BackingField;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00051ACD File Offset: 0x0004FCCD
		[CompilerGenerated]
		public void Deconstruct(out string ModId, out string Version)
		{
			ModId = this.ModId;
			Version = this.Version;
		}
	}
}
