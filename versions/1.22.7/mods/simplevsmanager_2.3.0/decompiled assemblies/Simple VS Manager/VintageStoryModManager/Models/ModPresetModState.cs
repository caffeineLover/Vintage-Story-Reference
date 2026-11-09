using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000B8 RID: 184
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class ModPresetModState : IEquatable<ModPresetModState>
	{
		// Token: 0x06000F7A RID: 3962 RVA: 0x00053E83 File Offset: 0x00052083
		public ModPresetModState([Nullable(1)] string ModId, string Version, bool? IsActive, string ConfigurationFileName, string ConfigurationContent, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<ModConfigurationSnapshot> Configurations = null)
		{
			this.ModId = ModId;
			this.Version = Version;
			this.IsActive = IsActive;
			this.ConfigurationFileName = ConfigurationFileName;
			this.ConfigurationContent = ConfigurationContent;
			this.Configurations = Configurations;
			base..ctor();
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x00053EB8 File Offset: 0x000520B8
		[Nullable(1)]
		[CompilerGenerated]
		private Type EqualityContract
		{
			[NullableContext(1)]
			[CompilerGenerated]
			get
			{
				return typeof(ModPresetModState);
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x00053EC4 File Offset: 0x000520C4
		// (set) Token: 0x06000F7D RID: 3965 RVA: 0x00053ECC File Offset: 0x000520CC
		[Nullable(1)]
		public string ModId { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x00053ED5 File Offset: 0x000520D5
		// (set) Token: 0x06000F7F RID: 3967 RVA: 0x00053EDD File Offset: 0x000520DD
		public string Version { get; set; }

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x00053EE6 File Offset: 0x000520E6
		// (set) Token: 0x06000F81 RID: 3969 RVA: 0x00053EEE File Offset: 0x000520EE
		public bool? IsActive { get; set; }

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x00053EF7 File Offset: 0x000520F7
		// (set) Token: 0x06000F83 RID: 3971 RVA: 0x00053EFF File Offset: 0x000520FF
		public string ConfigurationFileName { get; set; }

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x00053F08 File Offset: 0x00052108
		// (set) Token: 0x06000F85 RID: 3973 RVA: 0x00053F10 File Offset: 0x00052110
		public string ConfigurationContent { get; set; }

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x00053F19 File Offset: 0x00052119
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x00053F21 File Offset: 0x00052121
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public IReadOnlyList<ModConfigurationSnapshot> Configurations { [return: Nullable(new byte[]
		{
			2,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1
		})] set; }

		// Token: 0x06000F88 RID: 3976 RVA: 0x00053F2C File Offset: 0x0005212C
		[NullableContext(1)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModPresetModState");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00053F78 File Offset: 0x00052178
		[NullableContext(1)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("ModId = ");
			builder.Append(this.ModId);
			builder.Append(", Version = ");
			builder.Append(this.Version);
			builder.Append(", IsActive = ");
			builder.Append(this.IsActive.ToString());
			builder.Append(", ConfigurationFileName = ");
			builder.Append(this.ConfigurationFileName);
			builder.Append(", ConfigurationContent = ");
			builder.Append(this.ConfigurationContent);
			builder.Append(", Configurations = ");
			builder.Append(this.Configurations);
			return true;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0005402F File Offset: 0x0005222F
		[CompilerGenerated]
		public static bool operator !=(ModPresetModState left, ModPresetModState right)
		{
			return !(left == right);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0005403B File Offset: 0x0005223B
		[CompilerGenerated]
		public static bool operator ==(ModPresetModState left, ModPresetModState right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x00054050 File Offset: 0x00052250
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ModId>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Version>k__BackingField)) * -1521134295 + EqualityComparer<bool?>.Default.GetHashCode(this.<IsActive>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ConfigurationFileName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ConfigurationContent>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<ModConfigurationSnapshot>>.Default.GetHashCode(this.<Configurations>k__BackingField);
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x000540F7 File Offset: 0x000522F7
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModPresetModState);
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x00054108 File Offset: 0x00052308
		[CompilerGenerated]
		public bool Equals(ModPresetModState other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<ModId>k__BackingField, other.<ModId>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Version>k__BackingField, other.<Version>k__BackingField) && EqualityComparer<bool?>.Default.Equals(this.<IsActive>k__BackingField, other.<IsActive>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ConfigurationFileName>k__BackingField, other.<ConfigurationFileName>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ConfigurationContent>k__BackingField, other.<ConfigurationContent>k__BackingField) && EqualityComparer<IReadOnlyList<ModConfigurationSnapshot>>.Default.Equals(this.<Configurations>k__BackingField, other.<Configurations>k__BackingField));
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x000541D4 File Offset: 0x000523D4
		[CompilerGenerated]
		private ModPresetModState([Nullable(1)] ModPresetModState original)
		{
			this.ModId = original.<ModId>k__BackingField;
			this.Version = original.<Version>k__BackingField;
			this.IsActive = original.<IsActive>k__BackingField;
			this.ConfigurationFileName = original.<ConfigurationFileName>k__BackingField;
			this.ConfigurationContent = original.<ConfigurationContent>k__BackingField;
			this.Configurations = original.<Configurations>k__BackingField;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0005422F File Offset: 0x0005242F
		[CompilerGenerated]
		public void Deconstruct([Nullable(1)] out string ModId, out string Version, out bool? IsActive, out string ConfigurationFileName, out string ConfigurationContent, [Nullable(new byte[]
		{
			2,
			1
		})] out IReadOnlyList<ModConfigurationSnapshot> Configurations)
		{
			ModId = this.ModId;
			Version = this.Version;
			IsActive = this.IsActive;
			ConfigurationFileName = this.ConfigurationFileName;
			ConfigurationContent = this.ConfigurationContent;
			Configurations = this.Configurations;
		}
	}
}
