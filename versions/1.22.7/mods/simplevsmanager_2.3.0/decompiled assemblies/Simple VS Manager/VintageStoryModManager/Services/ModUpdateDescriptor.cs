using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200007A RID: 122
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModUpdateDescriptor : IEquatable<ModUpdateDescriptor>
	{
		// Token: 0x06000B02 RID: 2818 RVA: 0x000459B0 File Offset: 0x00043BB0
		public ModUpdateDescriptor(string ModId, string DisplayName, Uri DownloadUri, string TargetPath, bool TargetIsDirectory, [Nullable(2)] string ReleaseFileName, [Nullable(2)] string ReleaseVersion, [Nullable(2)] string InstalledVersion)
		{
			this.ModId = ModId;
			this.DisplayName = DisplayName;
			this.DownloadUri = DownloadUri;
			this.TargetPath = TargetPath;
			this.TargetIsDirectory = TargetIsDirectory;
			this.ReleaseFileName = ReleaseFileName;
			this.ReleaseVersion = ReleaseVersion;
			this.InstalledVersion = InstalledVersion;
			base..ctor();
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00045A00 File Offset: 0x00043C00
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ModUpdateDescriptor);
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00045A0C File Offset: 0x00043C0C
		// (set) Token: 0x06000B05 RID: 2821 RVA: 0x00045A14 File Offset: 0x00043C14
		public string ModId { get; set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00045A1D File Offset: 0x00043C1D
		// (set) Token: 0x06000B07 RID: 2823 RVA: 0x00045A25 File Offset: 0x00043C25
		public string DisplayName { get; set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00045A2E File Offset: 0x00043C2E
		// (set) Token: 0x06000B09 RID: 2825 RVA: 0x00045A36 File Offset: 0x00043C36
		public Uri DownloadUri { get; set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00045A3F File Offset: 0x00043C3F
		// (set) Token: 0x06000B0B RID: 2827 RVA: 0x00045A47 File Offset: 0x00043C47
		public string TargetPath { get; set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00045A50 File Offset: 0x00043C50
		// (set) Token: 0x06000B0D RID: 2829 RVA: 0x00045A58 File Offset: 0x00043C58
		public bool TargetIsDirectory { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00045A61 File Offset: 0x00043C61
		// (set) Token: 0x06000B0F RID: 2831 RVA: 0x00045A69 File Offset: 0x00043C69
		[Nullable(2)]
		public string ReleaseFileName { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00045A72 File Offset: 0x00043C72
		// (set) Token: 0x06000B11 RID: 2833 RVA: 0x00045A7A File Offset: 0x00043C7A
		[Nullable(2)]
		public string ReleaseVersion { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x00045A83 File Offset: 0x00043C83
		// (set) Token: 0x06000B13 RID: 2835 RVA: 0x00045A8B File Offset: 0x00043C8B
		[Nullable(2)]
		public string InstalledVersion { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00045A94 File Offset: 0x00043C94
		// (set) Token: 0x06000B15 RID: 2837 RVA: 0x00045A9C File Offset: 0x00043C9C
		[Nullable(2)]
		public string ExistingPath { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x06000B16 RID: 2838 RVA: 0x00045AA8 File Offset: 0x00043CA8
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModUpdateDescriptor");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00045AF4 File Offset: 0x00043CF4
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("ModId = ");
			builder.Append(this.ModId);
			builder.Append(", DisplayName = ");
			builder.Append(this.DisplayName);
			builder.Append(", DownloadUri = ");
			builder.Append(this.DownloadUri);
			builder.Append(", TargetPath = ");
			builder.Append(this.TargetPath);
			builder.Append(", TargetIsDirectory = ");
			builder.Append(this.TargetIsDirectory.ToString());
			builder.Append(", ReleaseFileName = ");
			builder.Append(this.ReleaseFileName);
			builder.Append(", ReleaseVersion = ");
			builder.Append(this.ReleaseVersion);
			builder.Append(", InstalledVersion = ");
			builder.Append(this.InstalledVersion);
			builder.Append(", ExistingPath = ");
			builder.Append(this.ExistingPath);
			return true;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00045BF6 File Offset: 0x00043DF6
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ModUpdateDescriptor left, ModUpdateDescriptor right)
		{
			return !(left == right);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00045C02 File Offset: 0x00043E02
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ModUpdateDescriptor left, ModUpdateDescriptor right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00045C18 File Offset: 0x00043E18
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ModId>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<DisplayName>k__BackingField)) * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(this.<DownloadUri>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<TargetPath>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<TargetIsDirectory>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ReleaseFileName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ReleaseVersion>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<InstalledVersion>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ExistingPath>k__BackingField);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00045D04 File Offset: 0x00043F04
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModUpdateDescriptor);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00045D14 File Offset: 0x00043F14
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ModUpdateDescriptor other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<ModId>k__BackingField, other.<ModId>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<DisplayName>k__BackingField, other.<DisplayName>k__BackingField) && EqualityComparer<Uri>.Default.Equals(this.<DownloadUri>k__BackingField, other.<DownloadUri>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<TargetPath>k__BackingField, other.<TargetPath>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<TargetIsDirectory>k__BackingField, other.<TargetIsDirectory>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ReleaseFileName>k__BackingField, other.<ReleaseFileName>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ReleaseVersion>k__BackingField, other.<ReleaseVersion>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<InstalledVersion>k__BackingField, other.<InstalledVersion>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ExistingPath>k__BackingField, other.<ExistingPath>k__BackingField));
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00045E30 File Offset: 0x00044030
		[CompilerGenerated]
		private ModUpdateDescriptor(ModUpdateDescriptor original)
		{
			this.ModId = original.<ModId>k__BackingField;
			this.DisplayName = original.<DisplayName>k__BackingField;
			this.DownloadUri = original.<DownloadUri>k__BackingField;
			this.TargetPath = original.<TargetPath>k__BackingField;
			this.TargetIsDirectory = original.<TargetIsDirectory>k__BackingField;
			this.ReleaseFileName = original.<ReleaseFileName>k__BackingField;
			this.ReleaseVersion = original.<ReleaseVersion>k__BackingField;
			this.InstalledVersion = original.<InstalledVersion>k__BackingField;
			this.ExistingPath = original.<ExistingPath>k__BackingField;
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00045EB0 File Offset: 0x000440B0
		[CompilerGenerated]
		public void Deconstruct(out string ModId, out string DisplayName, out Uri DownloadUri, out string TargetPath, out bool TargetIsDirectory, [Nullable(2)] out string ReleaseFileName, [Nullable(2)] out string ReleaseVersion, [Nullable(2)] out string InstalledVersion)
		{
			ModId = this.ModId;
			DisplayName = this.DisplayName;
			DownloadUri = this.DownloadUri;
			TargetPath = this.TargetPath;
			TargetIsDirectory = this.TargetIsDirectory;
			ReleaseFileName = this.ReleaseFileName;
			ReleaseVersion = this.ReleaseVersion;
			InstalledVersion = this.InstalledVersion;
		}
	}
}
