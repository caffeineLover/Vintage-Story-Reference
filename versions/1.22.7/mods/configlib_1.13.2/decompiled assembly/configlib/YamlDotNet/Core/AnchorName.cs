using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace YamlDotNet.Core
{
	// Token: 0x020001A4 RID: 420
	[NullableContext(1)]
	[Nullable(0)]
	public struct AnchorName : IEquatable<AnchorName>
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0001C7F0 File Offset: 0x0001A9F0
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
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0001C807 File Offset: 0x0001AA07
		public bool IsEmpty
		{
			get
			{
				return this.value == null;
			}
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0001C812 File Offset: 0x0001AA12
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

		// Token: 0x060008DB RID: 2267 RVA: 0x0001C852 File Offset: 0x0001AA52
		public override string ToString()
		{
			return this.value ?? "[empty]";
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001C863 File Offset: 0x0001AA63
		public bool Equals(AnchorName other)
		{
			return object.Equals(this.value, other.value);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001C878 File Offset: 0x0001AA78
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

		// Token: 0x060008DE RID: 2270 RVA: 0x0001C89D File Offset: 0x0001AA9D
		public override int GetHashCode()
		{
			string text = this.value;
			if (text == null)
			{
				return 0;
			}
			return text.GetHashCode();
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0001C8B0 File Offset: 0x0001AAB0
		public static bool operator ==(AnchorName left, AnchorName right)
		{
			return left.Equals(right);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0001C8BA File Offset: 0x0001AABA
		public static bool operator !=(AnchorName left, AnchorName right)
		{
			return !(left == right);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0001C8C6 File Offset: 0x0001AAC6
		[NullableContext(2)]
		public static implicit operator AnchorName(string value)
		{
			if (value != null)
			{
				return new AnchorName(value);
			}
			return AnchorName.Empty;
		}

		// Token: 0x04000384 RID: 900
		public static readonly AnchorName Empty = default(AnchorName);

		// Token: 0x04000385 RID: 901
		private static readonly Regex AnchorPattern = new Regex("^[^\\[\\]\\{\\},]+$", 8);

		// Token: 0x04000386 RID: 902
		[Nullable(2)]
		private readonly string value;
	}
}
