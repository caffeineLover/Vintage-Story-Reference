using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200005C RID: 92
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DataBackupResult : IEquatable<DataBackupResult>
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x0003828B File Offset: 0x0003648B
		public DataBackupResult(string Id, DateTime CreatedOnUtc, string DirectoryPath)
		{
			this.Id = Id;
			this.CreatedOnUtc = CreatedOnUtc;
			this.DirectoryPath = DirectoryPath;
			base..ctor();
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x000382A8 File Offset: 0x000364A8
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(DataBackupResult);
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x000382B4 File Offset: 0x000364B4
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x000382BC File Offset: 0x000364BC
		public string Id { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x000382C5 File Offset: 0x000364C5
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x000382CD File Offset: 0x000364CD
		public DateTime CreatedOnUtc { get; set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x000382D6 File Offset: 0x000364D6
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x000382DE File Offset: 0x000364DE
		public string DirectoryPath { get; set; }

		// Token: 0x0600092F RID: 2351 RVA: 0x000382E8 File Offset: 0x000364E8
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DataBackupResult");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00038334 File Offset: 0x00036534
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
			return true;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x000383A0 File Offset: 0x000365A0
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(DataBackupResult left, DataBackupResult right)
		{
			return !(left == right);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x000383AC File Offset: 0x000365AC
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(DataBackupResult left, DataBackupResult right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x000383C0 File Offset: 0x000365C0
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Id>k__BackingField)) * -1521134295 + EqualityComparer<DateTime>.Default.GetHashCode(this.<CreatedOnUtc>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<DirectoryPath>k__BackingField);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00038422 File Offset: 0x00036622
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as DataBackupResult);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00038430 File Offset: 0x00036630
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(DataBackupResult other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Id>k__BackingField, other.<Id>k__BackingField) && EqualityComparer<DateTime>.Default.Equals(this.<CreatedOnUtc>k__BackingField, other.<CreatedOnUtc>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<DirectoryPath>k__BackingField, other.<DirectoryPath>k__BackingField));
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x000384A9 File Offset: 0x000366A9
		[CompilerGenerated]
		private DataBackupResult(DataBackupResult original)
		{
			this.Id = original.<Id>k__BackingField;
			this.CreatedOnUtc = original.<CreatedOnUtc>k__BackingField;
			this.DirectoryPath = original.<DirectoryPath>k__BackingField;
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x000384D5 File Offset: 0x000366D5
		[CompilerGenerated]
		public void Deconstruct(out string Id, out DateTime CreatedOnUtc, out string DirectoryPath)
		{
			Id = this.Id;
			CreatedOnUtc = this.CreatedOnUtc;
			DirectoryPath = this.DirectoryPath;
		}
	}
}
