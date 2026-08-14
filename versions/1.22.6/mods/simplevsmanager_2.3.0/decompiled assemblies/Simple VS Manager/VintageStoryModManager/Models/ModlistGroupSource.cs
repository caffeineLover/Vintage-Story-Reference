using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000AC RID: 172
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModlistGroupSource : IEquatable<ModlistGroupSource>
	{
		// Token: 0x06000EAF RID: 3759 RVA: 0x00052152 File Offset: 0x00050352
		public ModlistGroupSource(string Reference, string FilePath, string DisplayName, SerializablePreset Preset)
		{
			this.Reference = Reference;
			this.FilePath = FilePath;
			this.DisplayName = DisplayName;
			this.Preset = Preset;
			base..ctor();
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x00052177 File Offset: 0x00050377
		[CompilerGenerated]
		private Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(ModlistGroupSource);
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x00052183 File Offset: 0x00050383
		// (set) Token: 0x06000EB2 RID: 3762 RVA: 0x0005218B File Offset: 0x0005038B
		public string Reference { get; set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x00052194 File Offset: 0x00050394
		// (set) Token: 0x06000EB4 RID: 3764 RVA: 0x0005219C File Offset: 0x0005039C
		public string FilePath { get; set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x000521A5 File Offset: 0x000503A5
		// (set) Token: 0x06000EB6 RID: 3766 RVA: 0x000521AD File Offset: 0x000503AD
		public string DisplayName { get; set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x000521B6 File Offset: 0x000503B6
		// (set) Token: 0x06000EB8 RID: 3768 RVA: 0x000521BE File Offset: 0x000503BE
		public SerializablePreset Preset { get; set; }

		// Token: 0x06000EB9 RID: 3769 RVA: 0x000521C8 File Offset: 0x000503C8
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModlistGroupSource");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00052214 File Offset: 0x00050414
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Reference = ");
			builder.Append(this.Reference);
			builder.Append(", FilePath = ");
			builder.Append(this.FilePath);
			builder.Append(", DisplayName = ");
			builder.Append(this.DisplayName);
			builder.Append(", Preset = ");
			builder.Append(this.Preset);
			return true;
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0005228B File Offset: 0x0005048B
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(ModlistGroupSource left, ModlistGroupSource right)
		{
			return !(left == right);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00052297 File Offset: 0x00050497
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(ModlistGroupSource left, ModlistGroupSource right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x000522AC File Offset: 0x000504AC
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Reference>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<FilePath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<DisplayName>k__BackingField)) * -1521134295 + EqualityComparer<SerializablePreset>.Default.GetHashCode(this.<Preset>k__BackingField);
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00052325 File Offset: 0x00050525
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModlistGroupSource);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00052334 File Offset: 0x00050534
		[NullableContext(2)]
		[CompilerGenerated]
		public bool Equals(ModlistGroupSource other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Reference>k__BackingField, other.<Reference>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<FilePath>k__BackingField, other.<FilePath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<DisplayName>k__BackingField, other.<DisplayName>k__BackingField) && EqualityComparer<SerializablePreset>.Default.Equals(this.<Preset>k__BackingField, other.<Preset>k__BackingField));
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x000523C5 File Offset: 0x000505C5
		[CompilerGenerated]
		private ModlistGroupSource(ModlistGroupSource original)
		{
			this.Reference = original.<Reference>k__BackingField;
			this.FilePath = original.<FilePath>k__BackingField;
			this.DisplayName = original.<DisplayName>k__BackingField;
			this.Preset = original.<Preset>k__BackingField;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x000523FD File Offset: 0x000505FD
		[CompilerGenerated]
		public void Deconstruct(out string Reference, out string FilePath, out string DisplayName, out SerializablePreset Preset)
		{
			Reference = this.Reference;
			FilePath = this.FilePath;
			DisplayName = this.DisplayName;
			Preset = this.Preset;
		}
	}
}
