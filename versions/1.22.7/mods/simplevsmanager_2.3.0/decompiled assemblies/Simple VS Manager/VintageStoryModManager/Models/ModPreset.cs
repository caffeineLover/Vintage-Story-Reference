using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000B9 RID: 185
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModPreset : IEquatable<ModPreset>
	{
		// Token: 0x06000F92 RID: 3986 RVA: 0x00054268 File Offset: 0x00052468
		public ModPreset(string Name, IReadOnlyList<string> DisabledEntries, IReadOnlyList<ModPresetModState> ModStates, bool IncludesModStatus, bool IncludesModVersions, bool IsExclusive)
		{
			this.Name = Name;
			this.DisabledEntries = DisabledEntries;
			this.ModStates = ModStates;
			this.IncludesModStatus = IncludesModStatus;
			this.IncludesModVersions = IncludesModVersions;
			this.IsExclusive = IsExclusive;
			base..ctor();
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0005429D File Offset: 0x0005249D
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ModPreset);
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x000542A9 File Offset: 0x000524A9
		// (set) Token: 0x06000F95 RID: 3989 RVA: 0x000542B1 File Offset: 0x000524B1
		public string Name { get; set; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x000542BA File Offset: 0x000524BA
		// (set) Token: 0x06000F97 RID: 3991 RVA: 0x000542C2 File Offset: 0x000524C2
		public IReadOnlyList<string> DisabledEntries { get; set; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x000542CB File Offset: 0x000524CB
		// (set) Token: 0x06000F99 RID: 3993 RVA: 0x000542D3 File Offset: 0x000524D3
		public IReadOnlyList<ModPresetModState> ModStates { get; set; }

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x000542DC File Offset: 0x000524DC
		// (set) Token: 0x06000F9B RID: 3995 RVA: 0x000542E4 File Offset: 0x000524E4
		public bool IncludesModStatus { get; set; }

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x000542ED File Offset: 0x000524ED
		// (set) Token: 0x06000F9D RID: 3997 RVA: 0x000542F5 File Offset: 0x000524F5
		public bool IncludesModVersions { get; set; }

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x000542FE File Offset: 0x000524FE
		// (set) Token: 0x06000F9F RID: 3999 RVA: 0x00054306 File Offset: 0x00052506
		public bool IsExclusive { get; set; }

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0005430F File Offset: 0x0005250F
		public bool HasModStates
		{
			get
			{
				return this.ModStates.Count > 0;
			}
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00054320 File Offset: 0x00052520
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModPreset");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0005436C File Offset: 0x0005256C
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Name = ");
			builder.Append(this.Name);
			builder.Append(", DisabledEntries = ");
			builder.Append(this.DisabledEntries);
			builder.Append(", ModStates = ");
			builder.Append(this.ModStates);
			builder.Append(", IncludesModStatus = ");
			builder.Append(this.IncludesModStatus.ToString());
			builder.Append(", IncludesModVersions = ");
			builder.Append(this.IncludesModVersions.ToString());
			builder.Append(", IsExclusive = ");
			builder.Append(this.IsExclusive.ToString());
			builder.Append(", HasModStates = ");
			builder.Append(this.HasModStates.ToString());
			return true;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00054466 File Offset: 0x00052666
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ModPreset left, ModPreset right)
		{
			return !(left == right);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00054472 File Offset: 0x00052672
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ModPreset left, ModPreset right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00054488 File Offset: 0x00052688
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Name>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<string>>.Default.GetHashCode(this.<DisabledEntries>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<ModPresetModState>>.Default.GetHashCode(this.<ModStates>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IncludesModStatus>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IncludesModVersions>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsExclusive>k__BackingField);
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0005452F File Offset: 0x0005272F
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModPreset);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00054540 File Offset: 0x00052740
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ModPreset other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Name>k__BackingField, other.<Name>k__BackingField) && EqualityComparer<IReadOnlyList<string>>.Default.Equals(this.<DisabledEntries>k__BackingField, other.<DisabledEntries>k__BackingField) && EqualityComparer<IReadOnlyList<ModPresetModState>>.Default.Equals(this.<ModStates>k__BackingField, other.<ModStates>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IncludesModStatus>k__BackingField, other.<IncludesModStatus>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IncludesModVersions>k__BackingField, other.<IncludesModVersions>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsExclusive>k__BackingField, other.<IsExclusive>k__BackingField));
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0005460C File Offset: 0x0005280C
		[CompilerGenerated]
		private ModPreset(ModPreset original)
		{
			this.Name = original.<Name>k__BackingField;
			this.DisabledEntries = original.<DisabledEntries>k__BackingField;
			this.ModStates = original.<ModStates>k__BackingField;
			this.IncludesModStatus = original.<IncludesModStatus>k__BackingField;
			this.IncludesModVersions = original.<IncludesModVersions>k__BackingField;
			this.IsExclusive = original.<IsExclusive>k__BackingField;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00054667 File Offset: 0x00052867
		[CompilerGenerated]
		public void Deconstruct(out string Name, out IReadOnlyList<string> DisabledEntries, out IReadOnlyList<ModPresetModState> ModStates, out bool IncludesModStatus, out bool IncludesModVersions, out bool IsExclusive)
		{
			Name = this.Name;
			DisabledEntries = this.DisabledEntries;
			ModStates = this.ModStates;
			IncludesModStatus = this.IncludesModStatus;
			IncludesModVersions = this.IncludesModVersions;
			IsExclusive = this.IsExclusive;
		}
	}
}
