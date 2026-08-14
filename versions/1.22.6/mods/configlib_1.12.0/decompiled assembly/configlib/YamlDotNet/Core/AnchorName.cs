using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace YamlDotNet.Core
{
	// Token: 0x020001A8 RID: 424
	[NullableContext(1)]
	[Nullable(0)]
	public struct AnchorName : IEquatable<AnchorName>
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x0001D138 File Offset: 0x0001B338
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

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0001D14F File Offset: 0x0001B34F
		public bool IsEmpty
		{
			get
			{
				return this.value == null;
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001D15A File Offset: 0x0001B35A
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

		// Token: 0x06000917 RID: 2327 RVA: 0x0001D19A File Offset: 0x0001B39A
		public override string ToString()
		{
			return this.value ?? "[empty]";
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001D1AB File Offset: 0x0001B3AB
		public bool Equals(AnchorName other)
		{
			return object.Equals(this.value, other.value);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001D1C0 File Offset: 0x0001B3C0
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

		// Token: 0x0600091A RID: 2330 RVA: 0x0001D1E5 File Offset: 0x0001B3E5
		public override int GetHashCode()
		{
			string text = this.value;
			if (text == null)
			{
				return 0;
			}
			return text.GetHashCode();
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001D1F8 File Offset: 0x0001B3F8
		public static bool operator ==(AnchorName left, AnchorName right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001D202 File Offset: 0x0001B402
		public static bool operator !=(AnchorName left, AnchorName right)
		{
			return !(left == right);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001D20E File Offset: 0x0001B40E
		[NullableContext(2)]
		public static implicit operator AnchorName(string value)
		{
			if (value != null)
			{
				return new AnchorName(value);
			}
			return AnchorName.Empty;
		}

		// Token: 0x040003B2 RID: 946
		public static readonly AnchorName Empty = default(AnchorName);

		// Token: 0x040003B3 RID: 947
		private static readonly Regex AnchorPattern = new Regex("^[^\\[\\]\\{\\},]+$", 8);

		// Token: 0x040003B4 RID: 948
		[Nullable(2)]
		private readonly string value;
	}
}
