using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200005A RID: 90
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct DataBackupProgress : IEquatable<DataBackupProgress>
	{
		// Token: 0x06000904 RID: 2308 RVA: 0x00037DBA File Offset: 0x00035FBA
		public DataBackupProgress(double Percent, string Status)
		{
			this.Percent = Percent;
			this.Status = Status;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x00037DCA File Offset: 0x00035FCA
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x00037DD2 File Offset: 0x00035FD2
		public double Percent { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x00037DDB File Offset: 0x00035FDB
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x00037DE3 File Offset: 0x00035FE3
		public string Status { get; set; }

		// Token: 0x06000909 RID: 2313 RVA: 0x00037DEC File Offset: 0x00035FEC
		[NullableContext(0)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DataBackupProgress");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00037E38 File Offset: 0x00036038
		[NullableContext(0)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Percent = ");
			builder.Append(this.Percent.ToString());
			builder.Append(", Status = ");
			builder.Append(this.Status);
			return true;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00037E86 File Offset: 0x00036086
		[CompilerGenerated]
		public static bool operator !=(DataBackupProgress left, DataBackupProgress right)
		{
			return !(left == right);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00037E92 File Offset: 0x00036092
		[CompilerGenerated]
		public static bool operator ==(DataBackupProgress left, DataBackupProgress right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00037E9C File Offset: 0x0003609C
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<double>.Default.GetHashCode(this.<Percent>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Status>k__BackingField);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00037EC5 File Offset: 0x000360C5
		[NullableContext(0)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return obj is DataBackupProgress && this.Equals((DataBackupProgress)obj);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00037EDD File Offset: 0x000360DD
		[CompilerGenerated]
		public bool Equals(DataBackupProgress other)
		{
			return EqualityComparer<double>.Default.Equals(this.<Percent>k__BackingField, other.<Percent>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Status>k__BackingField, other.<Status>k__BackingField);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00037F0F File Offset: 0x0003610F
		[CompilerGenerated]
		public void Deconstruct(out double Percent, out string Status)
		{
			Percent = this.Percent;
			Status = this.Status;
		}
	}
}
