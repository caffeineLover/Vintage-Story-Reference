using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000AE RID: 174
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModlistGroupConflictChoice : IEquatable<ModlistGroupConflictChoice>
	{
		// Token: 0x06000EDD RID: 3805 RVA: 0x0005287C File Offset: 0x00050A7C
		public ModlistGroupConflictChoice(string MemberReference, string FilePath, string ModlistName, [Nullable(2)] string Version, bool HasCustomConfiguration)
		{
			this.MemberReference = MemberReference;
			this.FilePath = FilePath;
			this.ModlistName = ModlistName;
			this.Version = Version;
			this.HasCustomConfiguration = HasCustomConfiguration;
			base..ctor();
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000EDE RID: 3806 RVA: 0x000528A9 File Offset: 0x00050AA9
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ModlistGroupConflictChoice);
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x000528B5 File Offset: 0x00050AB5
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x000528BD File Offset: 0x00050ABD
		public string MemberReference { get; set; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x000528C6 File Offset: 0x00050AC6
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x000528CE File Offset: 0x00050ACE
		public string FilePath { get; set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x000528D7 File Offset: 0x00050AD7
		// (set) Token: 0x06000EE4 RID: 3812 RVA: 0x000528DF File Offset: 0x00050ADF
		public string ModlistName { get; set; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x000528E8 File Offset: 0x00050AE8
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x000528F0 File Offset: 0x00050AF0
		[Nullable(2)]
		public string Version { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x000528F9 File Offset: 0x00050AF9
		// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x00052901 File Offset: 0x00050B01
		public bool HasCustomConfiguration { get; set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x0005290A File Offset: 0x00050B0A
		public string VersionDisplay
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.Version))
				{
					return "Version " + this.Version;
				}
				return "Version not specified";
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000EEA RID: 3818 RVA: 0x0005292F File Offset: 0x00050B2F
		public string ConfigurationDisplay
		{
			get
			{
				if (!this.HasCustomConfiguration)
				{
					return "No custom config";
				}
				return "Custom config included";
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x00052944 File Offset: 0x00050B44
		public string DisplayText
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(6, 3);
				defaultInterpolatedStringHandler.AppendFormatted(this.ModlistName);
				defaultInterpolatedStringHandler.AppendLiteral(" — ");
				defaultInterpolatedStringHandler.AppendFormatted(this.VersionDisplay);
				defaultInterpolatedStringHandler.AppendLiteral(" — ");
				defaultInterpolatedStringHandler.AppendFormatted(this.ConfigurationDisplay);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x000529A0 File Offset: 0x00050BA0
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModlistGroupConflictChoice");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000529EC File Offset: 0x00050BEC
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("MemberReference = ");
			builder.Append(this.MemberReference);
			builder.Append(", FilePath = ");
			builder.Append(this.FilePath);
			builder.Append(", ModlistName = ");
			builder.Append(this.ModlistName);
			builder.Append(", Version = ");
			builder.Append(this.Version);
			builder.Append(", HasCustomConfiguration = ");
			builder.Append(this.HasCustomConfiguration.ToString());
			builder.Append(", VersionDisplay = ");
			builder.Append(this.VersionDisplay);
			builder.Append(", ConfigurationDisplay = ");
			builder.Append(this.ConfigurationDisplay);
			builder.Append(", DisplayText = ");
			builder.Append(this.DisplayText);
			return true;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00052AD5 File Offset: 0x00050CD5
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ModlistGroupConflictChoice left, ModlistGroupConflictChoice right)
		{
			return !(left == right);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00052AE1 File Offset: 0x00050CE1
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ModlistGroupConflictChoice left, ModlistGroupConflictChoice right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00052AF8 File Offset: 0x00050CF8
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<MemberReference>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<FilePath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ModlistName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Version>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<HasCustomConfiguration>k__BackingField);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00052B88 File Offset: 0x00050D88
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModlistGroupConflictChoice);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00052B98 File Offset: 0x00050D98
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ModlistGroupConflictChoice other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<MemberReference>k__BackingField, other.<MemberReference>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<FilePath>k__BackingField, other.<FilePath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ModlistName>k__BackingField, other.<ModlistName>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Version>k__BackingField, other.<Version>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<HasCustomConfiguration>k__BackingField, other.<HasCustomConfiguration>k__BackingField));
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00052C48 File Offset: 0x00050E48
		[CompilerGenerated]
		private ModlistGroupConflictChoice(ModlistGroupConflictChoice original)
		{
			this.MemberReference = original.<MemberReference>k__BackingField;
			this.FilePath = original.<FilePath>k__BackingField;
			this.ModlistName = original.<ModlistName>k__BackingField;
			this.Version = original.<Version>k__BackingField;
			this.HasCustomConfiguration = original.<HasCustomConfiguration>k__BackingField;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00052C97 File Offset: 0x00050E97
		[CompilerGenerated]
		public void Deconstruct(out string MemberReference, out string FilePath, out string ModlistName, [Nullable(2)] out string Version, out bool HasCustomConfiguration)
		{
			MemberReference = this.MemberReference;
			FilePath = this.FilePath;
			ModlistName = this.ModlistName;
			Version = this.Version;
			HasCustomConfiguration = this.HasCustomConfiguration;
		}
	}
}
