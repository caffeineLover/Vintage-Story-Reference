using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000B7 RID: 183
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModConfigurationSnapshot : IEquatable<ModConfigurationSnapshot>
	{
		// Token: 0x06000F68 RID: 3944 RVA: 0x00053C29 File Offset: 0x00051E29
		public ModConfigurationSnapshot(string FileName, string Content, [Nullable(2)] string RelativePath = null)
		{
			this.FileName = FileName;
			this.Content = Content;
			this.RelativePath = RelativePath;
			base..ctor();
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x00053C46 File Offset: 0x00051E46
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ModConfigurationSnapshot);
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x00053C52 File Offset: 0x00051E52
		// (set) Token: 0x06000F6B RID: 3947 RVA: 0x00053C5A File Offset: 0x00051E5A
		public string FileName { get; set; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x00053C63 File Offset: 0x00051E63
		// (set) Token: 0x06000F6D RID: 3949 RVA: 0x00053C6B File Offset: 0x00051E6B
		public string Content { get; set; }

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000F6E RID: 3950 RVA: 0x00053C74 File Offset: 0x00051E74
		// (set) Token: 0x06000F6F RID: 3951 RVA: 0x00053C7C File Offset: 0x00051E7C
		[Nullable(2)]
		public string RelativePath { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x06000F70 RID: 3952 RVA: 0x00053C88 File Offset: 0x00051E88
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModConfigurationSnapshot");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00053CD4 File Offset: 0x00051ED4
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("FileName = ");
			builder.Append(this.FileName);
			builder.Append(", Content = ");
			builder.Append(this.Content);
			builder.Append(", RelativePath = ");
			builder.Append(this.RelativePath);
			return true;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00053D32 File Offset: 0x00051F32
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ModConfigurationSnapshot left, ModConfigurationSnapshot right)
		{
			return !(left == right);
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00053D3E File Offset: 0x00051F3E
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ModConfigurationSnapshot left, ModConfigurationSnapshot right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00053D54 File Offset: 0x00051F54
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<FileName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Content>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<RelativePath>k__BackingField);
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00053DB6 File Offset: 0x00051FB6
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModConfigurationSnapshot);
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00053DC4 File Offset: 0x00051FC4
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ModConfigurationSnapshot other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<FileName>k__BackingField, other.<FileName>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Content>k__BackingField, other.<Content>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<RelativePath>k__BackingField, other.<RelativePath>k__BackingField));
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00053E3D File Offset: 0x0005203D
		[CompilerGenerated]
		private ModConfigurationSnapshot(ModConfigurationSnapshot original)
		{
			this.FileName = original.<FileName>k__BackingField;
			this.Content = original.<Content>k__BackingField;
			this.RelativePath = original.<RelativePath>k__BackingField;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00053E69 File Offset: 0x00052069
		[CompilerGenerated]
		public void Deconstruct(out string FileName, out string Content, [Nullable(2)] out string RelativePath)
		{
			FileName = this.FileName;
			Content = this.Content;
			RelativePath = this.RelativePath;
		}
	}
}
