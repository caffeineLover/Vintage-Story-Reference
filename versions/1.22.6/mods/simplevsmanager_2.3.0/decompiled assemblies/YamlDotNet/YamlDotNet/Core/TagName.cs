using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000DE RID: 222
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct TagName : IEquatable<TagName>
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00016A0B File Offset: 0x00014C0B
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

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x00016A22 File Offset: 0x00014C22
		public bool IsEmpty
		{
			get
			{
				return this.value == null;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x00016A2D File Offset: 0x00014C2D
		public bool IsNonSpecific
		{
			get
			{
				return !this.IsEmpty && (this.value == "!" || this.value == "?");
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x00016A5D File Offset: 0x00014C5D
		public bool IsLocal
		{
			get
			{
				return !this.IsEmpty && this.Value.get_Chars(0) == '!';
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x00016A79 File Offset: 0x00014C79
		public bool IsGlobal
		{
			get
			{
				return !this.IsEmpty && !this.IsLocal;
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00016A90 File Offset: 0x00014C90
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

		// Token: 0x060005D3 RID: 1491 RVA: 0x00016AEC File Offset: 0x00014CEC
		public override string ToString()
		{
			return this.value ?? "?";
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00016AFD File Offset: 0x00014CFD
		public bool Equals(TagName other)
		{
			return object.Equals(this.value, other.value);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00016B10 File Offset: 0x00014D10
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

		// Token: 0x060005D6 RID: 1494 RVA: 0x00016B35 File Offset: 0x00014D35
		public override int GetHashCode()
		{
			string text = this.value;
			if (text == null)
			{
				return 0;
			}
			return text.GetHashCode();
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00016B48 File Offset: 0x00014D48
		public static bool operator ==(TagName left, TagName right)
		{
			return left.Equals(right);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00016B52 File Offset: 0x00014D52
		public static bool operator !=(TagName left, TagName right)
		{
			return !(left == right);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00016B5E File Offset: 0x00014D5E
		public static bool operator ==(TagName left, string right)
		{
			return object.Equals(left.value, right);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00016B6C File Offset: 0x00014D6C
		public static bool operator !=(TagName left, string right)
		{
			return !(left == right);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00016B78 File Offset: 0x00014D78
		[NullableContext(2)]
		public static implicit operator TagName(string value)
		{
			if (value != null)
			{
				return new TagName(value);
			}
			return TagName.Empty;
		}

		// Token: 0x040001BE RID: 446
		public static readonly TagName Empty;

		// Token: 0x040001BF RID: 447
		[Nullable(2)]
		private readonly string value;
	}
}
