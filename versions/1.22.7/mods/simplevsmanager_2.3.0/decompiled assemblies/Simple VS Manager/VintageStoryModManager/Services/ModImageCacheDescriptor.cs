using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000073 RID: 115
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class ModImageCacheDescriptor : IEquatable<ModImageCacheDescriptor>
	{
		// Token: 0x06000A8D RID: 2701 RVA: 0x00042728 File Offset: 0x00040928
		public ModImageCacheDescriptor(string ModId, string ModName, string ApiSource)
		{
			this.ModId = ModId;
			this.ModName = ModName;
			this.ApiSource = ApiSource;
			base..ctor();
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x00042745 File Offset: 0x00040945
		[Nullable(1)]
		[CompilerGenerated]
		private Type EqualityContract
		{
			[NullableContext(1)]
			[CompilerGenerated]
			get
			{
				return typeof(ModImageCacheDescriptor);
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x00042751 File Offset: 0x00040951
		// (set) Token: 0x06000A90 RID: 2704 RVA: 0x00042759 File Offset: 0x00040959
		public string ModId { get; set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x00042762 File Offset: 0x00040962
		// (set) Token: 0x06000A92 RID: 2706 RVA: 0x0004276A File Offset: 0x0004096A
		public string ModName { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x00042773 File Offset: 0x00040973
		// (set) Token: 0x06000A94 RID: 2708 RVA: 0x0004277B File Offset: 0x0004097B
		public string ApiSource { get; set; }

		// Token: 0x06000A95 RID: 2709 RVA: 0x00042784 File Offset: 0x00040984
		[NullableContext(1)]
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ModImageCacheDescriptor");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x000427D0 File Offset: 0x000409D0
		[NullableContext(1)]
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("ModId = ");
			builder.Append(this.ModId);
			builder.Append(", ModName = ");
			builder.Append(this.ModName);
			builder.Append(", ApiSource = ");
			builder.Append(this.ApiSource);
			return true;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0004282E File Offset: 0x00040A2E
		[CompilerGenerated]
		public static bool operator !=(ModImageCacheDescriptor left, ModImageCacheDescriptor right)
		{
			return !(left == right);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0004283A File Offset: 0x00040A3A
		[CompilerGenerated]
		public static bool operator ==(ModImageCacheDescriptor left, ModImageCacheDescriptor right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00042850 File Offset: 0x00040A50
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ModId>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ModName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ApiSource>k__BackingField);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x000428B2 File Offset: 0x00040AB2
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ModImageCacheDescriptor);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x000428C0 File Offset: 0x00040AC0
		[CompilerGenerated]
		public bool Equals(ModImageCacheDescriptor other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<ModId>k__BackingField, other.<ModId>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ModName>k__BackingField, other.<ModName>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ApiSource>k__BackingField, other.<ApiSource>k__BackingField));
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00042939 File Offset: 0x00040B39
		[CompilerGenerated]
		private ModImageCacheDescriptor([Nullable(1)] ModImageCacheDescriptor original)
		{
			this.ModId = original.<ModId>k__BackingField;
			this.ModName = original.<ModName>k__BackingField;
			this.ApiSource = original.<ApiSource>k__BackingField;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00042965 File Offset: 0x00040B65
		[CompilerGenerated]
		public void Deconstruct(out string ModId, out string ModName, out string ApiSource)
		{
			ModId = this.ModId;
			ModName = this.ModName;
			ApiSource = this.ApiSource;
		}
	}
}
