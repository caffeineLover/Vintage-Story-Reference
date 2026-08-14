using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000044 RID: 68
	[NullableContext(1)]
	[Nullable(0)]
	public class OrderByOption : IEquatable<OrderByOption>
	{
		// Token: 0x0600072B RID: 1835 RVA: 0x0002E3DB File Offset: 0x0002C5DB
		public OrderByOption(string Key, string Display, string Icon)
		{
			this.Key = Key;
			this.Display = Display;
			this.Icon = Icon;
			base..ctor();
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0002E3F8 File Offset: 0x0002C5F8
		[CompilerGenerated]
		protected virtual Type EqualityContract
		{
			[CompilerGenerated]
			get
			{
				return typeof(OrderByOption);
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0002E404 File Offset: 0x0002C604
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x0002E40C File Offset: 0x0002C60C
		public string Key { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0002E415 File Offset: 0x0002C615
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x0002E41D File Offset: 0x0002C61D
		public string Display { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0002E426 File Offset: 0x0002C626
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x0002E42E File Offset: 0x0002C62E
		public string Icon { get; set; }

		// Token: 0x06000733 RID: 1843 RVA: 0x0002E438 File Offset: 0x0002C638
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("OrderByOption");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0002E484 File Offset: 0x0002C684
		[CompilerGenerated]
		protected virtual bool PrintMembers(StringBuilder builder)
		{
			RuntimeHelpers.EnsureSufficientExecutionStack();
			builder.Append("Key = ");
			builder.Append(this.Key);
			builder.Append(", Display = ");
			builder.Append(this.Display);
			builder.Append(", Icon = ");
			builder.Append(this.Icon);
			return true;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0002E4E2 File Offset: 0x0002C6E2
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator !=(OrderByOption left, OrderByOption right)
		{
			return !(left == right);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0002E4EE File Offset: 0x0002C6EE
		[NullableContext(2)]
		[CompilerGenerated]
		public static bool operator ==(OrderByOption left, OrderByOption right)
		{
			return left == right || (left != null && left.Equals(right));
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0002E504 File Offset: 0x0002C704
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Key>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Display>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Icon>k__BackingField);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0002E566 File Offset: 0x0002C766
		[NullableContext(2)]
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return this.Equals(obj as OrderByOption);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0002E574 File Offset: 0x0002C774
		[NullableContext(2)]
		[CompilerGenerated]
		public virtual bool Equals(OrderByOption other)
		{
			return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Key>k__BackingField, other.<Key>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Display>k__BackingField, other.<Display>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Icon>k__BackingField, other.<Icon>k__BackingField));
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0002E5ED File Offset: 0x0002C7ED
		[CompilerGenerated]
		protected OrderByOption(OrderByOption original)
		{
			this.Key = original.<Key>k__BackingField;
			this.Display = original.<Display>k__BackingField;
			this.Icon = original.<Icon>k__BackingField;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0002E619 File Offset: 0x0002C819
		[CompilerGenerated]
		public void Deconstruct(out string Key, out string Display, out string Icon)
		{
			Key = this.Key;
			Display = this.Display;
			Icon = this.Icon;
		}
	}
}
