using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200007B RID: 123
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class ModUpdateResult : IEquatable<ModUpdateResult>
	{
		// Token: 0x06000B20 RID: 2848 RVA: 0x00045F02 File Offset: 0x00044102
		public ModUpdateResult(bool Success, string ErrorMessage)
		{
			this.Success = Success;
			this.ErrorMessage = ErrorMessage;
			base..ctor();
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00045F18 File Offset: 0x00044118
		[Nullable(1)]
		[CompilerGenerated]
		private Type EqualityContract
		{
			[NullableContext(1)]
			[CompilerGenerated]
			get
			{
				return typeof(ModUpdateResult);
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00045F24 File Offset: 0x00044124
		// (set) Token: 0x06000B23 RID: 2851 RVA: 0x00045F2C File Offset: 0x0004412C
		public bool Success { get; set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x00045F35 File Offset: 0x00044135
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x00045F3D File Offset: 0x0004413D
		public string ErrorMessage { get; set; }

		// Token: 0x06000B26 RID: 2854 RVA: 0x00045F48 File Offset: 0x00044148
		[NullableContext(1)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModUpdateResult");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00045F94 File Offset: 0x00044194
		[NullableContext(1)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Success = ");
			builder.Append(this.Success.ToString());
			builder.Append(", ErrorMessage = ");
			builder.Append(this.ErrorMessage);
			return true;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00045FE7 File Offset: 0x000441E7
		[CompilerGenerated]
		public static bool operator !=(ModUpdateResult left, ModUpdateResult right)
		{
			return !(left == right);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00045FF3 File Offset: 0x000441F3
		[CompilerGenerated]
		public static bool operator ==(ModUpdateResult left, ModUpdateResult right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00046007 File Offset: 0x00044207
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Success>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ErrorMessage>k__BackingField);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00046047 File Offset: 0x00044247
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModUpdateResult);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00046058 File Offset: 0x00044258
		[CompilerGenerated]
		public bool Equals(ModUpdateResult other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<bool>.Default.Equals(this.<Success>k__BackingField, other.<Success>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ErrorMessage>k__BackingField, other.<ErrorMessage>k__BackingField));
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x000460B9 File Offset: 0x000442B9
		[CompilerGenerated]
		private ModUpdateResult([Nullable(1)] ModUpdateResult original)
		{
			this.Success = original.<Success>k__BackingField;
			this.ErrorMessage = original.<ErrorMessage>k__BackingField;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000460D9 File Offset: 0x000442D9
		[CompilerGenerated]
		public void Deconstruct(out bool Success, out string ErrorMessage)
		{
			Success = this.Success;
			ErrorMessage = this.ErrorMessage;
		}
	}
}
