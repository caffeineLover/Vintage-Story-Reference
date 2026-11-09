using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001CD RID: 461
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct TagName : IEquatable<TagName>
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x000249DF File Offset: 0x00022BDF
		public string Value
		{
			get
			{
				string text = this.value;
				if (text == null)
				{
					throw new InvalidOperationException("Cannot read the Value of a non-specific tag");
				}
				return text;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x000249F6 File Offset: 0x00022BF6
		public bool IsEmpty
		{
			get
			{
				return this.value == null;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x00024A01 File Offset: 0x00022C01
		public bool IsNonSpecific
		{
			get
			{
				return !this.IsEmpty && (this.value == "!" || this.value == "?");
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x00024A31 File Offset: 0x00022C31
		public bool IsLocal
		{
			get
			{
				return !this.IsEmpty && this.Value.get_Chars(0) == '!';
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x00024A4D File Offset: 0x00022C4D
		public bool IsGlobal
		{
			get
			{
				return !this.IsEmpty && !this.IsLocal;
			}
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00024A64 File Offset: 0x00022C64
		public TagName(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.value = value;
			if (value.Length == 0)
			{
				throw new ArgumentException("Tag value must not be empty.", "value");
			}
			if (this.IsGlobal && !Uri.IsWellFormedUriString(value, 0))
			{
				throw new ArgumentException("Global tags must be valid URIs.", "value");
			}
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00024AC0 File Offset: 0x00022CC0
		public override string ToString()
		{
			return this.value ?? "?";
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00024AD1 File Offset: 0x00022CD1
		public bool Equals(TagName other)
		{
			return object.Equals(this.value, other.value);
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00024AE4 File Offset: 0x00022CE4
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is TagName)
			{
				TagName other = (TagName)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00024B09 File Offset: 0x00022D09
		public override int GetHashCode()
		{
			string text = this.value;
			if (text == null)
			{
				return 0;
			}
			return text.GetHashCode();
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00024B1C File Offset: 0x00022D1C
		public static bool operator ==(TagName left, TagName right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00024B26 File Offset: 0x00022D26
		public static bool operator !=(TagName left, TagName right)
		{
			return !(left == right);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00024B32 File Offset: 0x00022D32
		public static bool operator ==(TagName left, string right)
		{
			return object.Equals(left.value, right);
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00024B40 File Offset: 0x00022D40
		public static bool operator !=(TagName left, string right)
		{
			return !(left == right);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00024B4C File Offset: 0x00022D4C
		[NullableContext(2)]
		public static implicit operator TagName(string value)
		{
			if (value != null)
			{
				return new TagName(value);
			}
			return TagName.Empty;
		}

		// Token: 0x04000446 RID: 1094
		public static readonly TagName Empty;

		// Token: 0x04000447 RID: 1095
		[Nullable(2)]
		private readonly string value;
	}
}
