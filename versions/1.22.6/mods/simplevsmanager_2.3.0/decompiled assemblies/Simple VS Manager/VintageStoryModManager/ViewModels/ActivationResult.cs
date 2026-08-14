using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000053 RID: 83
	[NullableContext(2)]
	[Nullable(0)]
	public readonly struct ActivationResult : IEquatable<ActivationResult>
	{
		// Token: 0x0600089C RID: 2204 RVA: 0x000346C7 File Offset: 0x000328C7
		public ActivationResult(bool Success, string ErrorMessage)
		{
			this.Success = Success;
			this.ErrorMessage = ErrorMessage;
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x000346D7 File Offset: 0x000328D7
		// (set) Token: 0x0600089E RID: 2206 RVA: 0x000346DF File Offset: 0x000328DF
		public bool Success { get; set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x000346E8 File Offset: 0x000328E8
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x000346F0 File Offset: 0x000328F0
		public string ErrorMessage { get; set; }

		// Token: 0x060008A1 RID: 2209 RVA: 0x000346FC File Offset: 0x000328FC
		[NullableContext(0)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ActivationResult");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00034748 File Offset: 0x00032948
		[NullableContext(0)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("Success = ");
			builder.Append(this.Success.ToString());
			builder.Append(", ErrorMessage = ");
			builder.Append(this.ErrorMessage);
			return true;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00034796 File Offset: 0x00032996
		[CompilerGenerated]
		public static bool operator !=(ActivationResult left, ActivationResult right)
		{
			return !(left == right);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x000347A2 File Offset: 0x000329A2
		[CompilerGenerated]
		public static bool operator ==(ActivationResult left, ActivationResult right)
		{
			return left.Equals(right);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x000347AC File Offset: 0x000329AC
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<bool>.Default.GetHashCode(this.<Success>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ErrorMessage>k__BackingField);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x000347D5 File Offset: 0x000329D5
		[NullableContext(0)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return obj is ActivationResult && this.Equals((ActivationResult)obj);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x000347ED File Offset: 0x000329ED
		[CompilerGenerated]
		public bool Equals(ActivationResult other)
		{
			return EqualityComparer<bool>.Default.Equals(this.<Success>k__BackingField, other.<Success>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ErrorMessage>k__BackingField, other.<ErrorMessage>k__BackingField);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0003481F File Offset: 0x00032A1F
		[CompilerGenerated]
		public void Deconstruct(out bool Success, out string ErrorMessage)
		{
			Success = this.Success;
			ErrorMessage = this.ErrorMessage;
		}
	}
}
