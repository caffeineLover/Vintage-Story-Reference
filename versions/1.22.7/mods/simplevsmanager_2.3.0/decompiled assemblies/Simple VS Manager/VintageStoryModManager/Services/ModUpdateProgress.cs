using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200007C RID: 124
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ModUpdateProgress : IEquatable<ModUpdateProgress>
	{
		// Token: 0x06000B30 RID: 2864 RVA: 0x000460EB File Offset: 0x000442EB
		public ModUpdateProgress(ModUpdateStage Stage, string Message, double? Percent, double? BytesPerSecond)
		{
			this.Stage = Stage;
			this.Message = Message;
			this.Percent = Percent;
			this.BytesPerSecond = BytesPerSecond;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x0004610A File Offset: 0x0004430A
		// (set) Token: 0x06000B32 RID: 2866 RVA: 0x00046112 File Offset: 0x00044312
		public ModUpdateStage Stage { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0004611B File Offset: 0x0004431B
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x00046123 File Offset: 0x00044323
		public string Message { get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x0004612C File Offset: 0x0004432C
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x00046134 File Offset: 0x00044334
		public double? Percent { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x0004613D File Offset: 0x0004433D
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x00046145 File Offset: 0x00044345
		public double? BytesPerSecond { get; set; }

		// Token: 0x06000B39 RID: 2873 RVA: 0x00046150 File Offset: 0x00044350
		[NullableContext(0)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModUpdateProgress");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0004619C File Offset: 0x0004439C
		[NullableContext(0)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Stage = ");
			builder.Append(this.Stage.ToString());
			builder.Append(", Message = ");
			builder.Append(this.Message);
			builder.Append(", Percent = ");
			builder.Append(this.Percent.ToString());
			builder.Append(", BytesPerSecond = ");
			builder.Append(this.BytesPerSecond.ToString());
			return true;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00046238 File Offset: 0x00044438
		[CompilerGenerated]
		public static bool operator !=(ModUpdateProgress left, ModUpdateProgress right)
		{
			return !(left == right);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00046244 File Offset: 0x00044444
		[CompilerGenerated]
		public static bool operator ==(ModUpdateProgress left, ModUpdateProgress right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00046250 File Offset: 0x00044450
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<ModUpdateStage>.Default.GetHashCode(this.<Stage>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Message>k__BackingField)) * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(this.<Percent>k__BackingField)) * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(this.<BytesPerSecond>k__BackingField);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x000462B2 File Offset: 0x000444B2
		[NullableContext(0)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return obj is ModUpdateProgress && this.Equals((ModUpdateProgress)obj);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x000462CC File Offset: 0x000444CC
		[CompilerGenerated]
		public bool Equals(ModUpdateProgress other)
		{
			return EqualityComparer<ModUpdateStage>.Default.Equals(this.<Stage>k__BackingField, other.<Stage>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Message>k__BackingField, other.<Message>k__BackingField) && EqualityComparer<double?>.Default.Equals(this.<Percent>k__BackingField, other.<Percent>k__BackingField) && EqualityComparer<double?>.Default.Equals(this.<BytesPerSecond>k__BackingField, other.<BytesPerSecond>k__BackingField);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00046339 File Offset: 0x00044539
		[CompilerGenerated]
		public void Deconstruct(out ModUpdateStage Stage, out string Message, out double? Percent, out double? BytesPerSecond)
		{
			Stage = this.Stage;
			Message = this.Message;
			Percent = this.Percent;
			BytesPerSecond = this.BytesPerSecond;
		}
	}
}
