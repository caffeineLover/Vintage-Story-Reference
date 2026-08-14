using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace YamlDotNet.Core
{
	// Token: 0x020000C0 RID: 192
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct AnchorName : IEquatable<AnchorName>
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0000EB6B File Offset: 0x0000CD6B
		public string Value
		{
			get
			{
				string text = this.value;
				if (text == null)
				{
					throw new InvalidOperationException("Cannot read the Value of an empty anchor");
				}
				return text;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0000EB82 File Offset: 0x0000CD82
		public bool IsEmpty
		{
			get
			{
				return this.value == null;
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000EB8D File Offset: 0x0000CD8D
		public AnchorName(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.value = value;
			if (!AnchorName.AnchorPattern.IsMatch(value))
			{
				throw new ArgumentException("Anchor cannot be empty or contain disallowed characters: []{},\nThe value was '" + value + "'.", "value");
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000EBCD File Offset: 0x0000CDCD
		public override string ToString()
		{
			return this.value ?? "[empty]";
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000EBDE File Offset: 0x0000CDDE
		public bool Equals(AnchorName other)
		{
			return object.Equals(this.value, other.value);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000EBF4 File Offset: 0x0000CDF4
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is AnchorName)
			{
				AnchorName other = (AnchorName)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000EC19 File Offset: 0x0000CE19
		public override int GetHashCode()
		{
			string text = this.value;
			if (text == null)
			{
				return 0;
			}
			return text.GetHashCode();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000EC2C File Offset: 0x0000CE2C
		public static bool operator ==(AnchorName left, AnchorName right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000EC36 File Offset: 0x0000CE36
		public static bool operator !=(AnchorName left, AnchorName right)
		{
			return !(left == right);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000EC42 File Offset: 0x0000CE42
		[NullableContext(2)]
		public static implicit operator AnchorName(string value)
		{
			if (value != null)
			{
				return new AnchorName(value);
			}
			return AnchorName.Empty;
		}

		// Token: 0x0400011D RID: 285
		public static readonly AnchorName Empty;

		// Token: 0x0400011E RID: 286
		private static readonly Regex AnchorPattern = new Regex("^[^\\[\\]\\{\\},]+$", 8);

		// Token: 0x0400011F RID: 287
		[Nullable(2)]
		private readonly string value;
	}
}
