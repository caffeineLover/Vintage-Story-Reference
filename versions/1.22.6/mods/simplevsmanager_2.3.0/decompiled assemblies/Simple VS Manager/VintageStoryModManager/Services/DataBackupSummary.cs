using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200005B RID: 91
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DataBackupSummary : IEquatable<DataBackupSummary>
	{
		// Token: 0x06000911 RID: 2321 RVA: 0x00037F21 File Offset: 0x00036121
		public DataBackupSummary(string Id, DateTime CreatedOnUtc, string DirectoryPath, [Nullable(2)] string SourceDataDirectory, [Nullable(2)] string VintageStoryVersion)
		{
			this.Id = Id;
			this.CreatedOnUtc = CreatedOnUtc;
			this.DirectoryPath = DirectoryPath;
			this.SourceDataDirectory = SourceDataDirectory;
			this.VintageStoryVersion = VintageStoryVersion;
			base..ctor();
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00037F4E File Offset: 0x0003614E
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(DataBackupSummary);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x00037F5A File Offset: 0x0003615A
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x00037F62 File Offset: 0x00036162
		public string Id { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x00037F6B File Offset: 0x0003616B
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x00037F73 File Offset: 0x00036173
		public DateTime CreatedOnUtc { get; set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00037F7C File Offset: 0x0003617C
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x00037F84 File Offset: 0x00036184
		public string DirectoryPath { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x00037F8D File Offset: 0x0003618D
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x00037F95 File Offset: 0x00036195
		[Nullable(2)]
		public string SourceDataDirectory { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x00037F9E File Offset: 0x0003619E
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x00037FA6 File Offset: 0x000361A6
		[Nullable(2)]
		public string VintageStoryVersion { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x0600091D RID: 2333 RVA: 0x00037FB0 File Offset: 0x000361B0
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DataBackupSummary");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00037FFC File Offset: 0x000361FC
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Id = ");
			builder.Append(this.Id);
			builder.Append(", CreatedOnUtc = ");
			builder.Append(this.CreatedOnUtc.ToString());
			builder.Append(", DirectoryPath = ");
			builder.Append(this.DirectoryPath);
			builder.Append(", SourceDataDirectory = ");
			builder.Append(this.SourceDataDirectory);
			builder.Append(", VintageStoryVersion = ");
			builder.Append(this.VintageStoryVersion);
			return true;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0003809A File Offset: 0x0003629A
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(DataBackupSummary left, DataBackupSummary right)
		{
			return !(left == right);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x000380A6 File Offset: 0x000362A6
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(DataBackupSummary left, DataBackupSummary right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x000380BC File Offset: 0x000362BC
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Id>k__BackingField)) * -1521134295 + EqualityComparer<DateTime>.Default.GetHashCode(this.<CreatedOnUtc>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<DirectoryPath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SourceDataDirectory>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<VintageStoryVersion>k__BackingField);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0003814C File Offset: 0x0003634C
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as DataBackupSummary);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0003815C File Offset: 0x0003635C
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(DataBackupSummary other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Id>k__BackingField, other.<Id>k__BackingField) && EqualityComparer<DateTime>.Default.Equals(this.<CreatedOnUtc>k__BackingField, other.<CreatedOnUtc>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<DirectoryPath>k__BackingField, other.<DirectoryPath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<SourceDataDirectory>k__BackingField, other.<SourceDataDirectory>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<VintageStoryVersion>k__BackingField, other.<VintageStoryVersion>k__BackingField));
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0003820C File Offset: 0x0003640C
		[CompilerGenerated]
		private DataBackupSummary(DataBackupSummary original)
		{
			this.Id = original.<Id>k__BackingField;
			this.CreatedOnUtc = original.<CreatedOnUtc>k__BackingField;
			this.DirectoryPath = original.<DirectoryPath>k__BackingField;
			this.SourceDataDirectory = original.<SourceDataDirectory>k__BackingField;
			this.VintageStoryVersion = original.<VintageStoryVersion>k__BackingField;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0003825B File Offset: 0x0003645B
		[CompilerGenerated]
		public void Deconstruct(out string Id, out DateTime CreatedOnUtc, out string DirectoryPath, [Nullable(2)] out string SourceDataDirectory, [Nullable(2)] out string VintageStoryVersion)
		{
			Id = this.Id;
			CreatedOnUtc = this.CreatedOnUtc;
			DirectoryPath = this.DirectoryPath;
			SourceDataDirectory = this.SourceDataDirectory;
			VintageStoryVersion = this.VintageStoryVersion;
		}
	}
}
