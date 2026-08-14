using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001D1 RID: 465
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct TagName : IEquatable<TagName>
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x00025327 File Offset: 0x00023527
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
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0002533E File Offset: 0x0002353E
		public bool IsEmpty
		{
			get
			{
				return this.value == null;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x00025349 File Offset: 0x00023549
		public bool IsNonSpecific
		{
			get
			{
				return !this.IsEmpty && (this.value == "!" || this.value == "?");
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00025379 File Offset: 0x00023579
		public bool IsLocal
		{
			get
			{
				return !this.IsEmpty && this.Value.get_Chars(0) == '!';
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x00025395 File Offset: 0x00023595
		public bool IsGlobal
		{
			get
			{
				return !this.IsEmpty && !this.IsLocal;
			}
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x000253AC File Offset: 0x000235AC
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

		// Token: 0x06000A93 RID: 2707 RVA: 0x00025408 File Offset: 0x00023608
		public override string ToString()
		{
			return this.value ?? "?";
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00025419 File Offset: 0x00023619
		public bool Equals(TagName other)
		{
			return object.Equals(this.value, other.value);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0002542C File Offset: 0x0002362C
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

		// Token: 0x06000A96 RID: 2710 RVA: 0x00025451 File Offset: 0x00023651
		public override int GetHashCode()
		{
			string text = this.value;
			if (text == null)
			{
				return 0;
			}
			return text.GetHashCode();
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00025464 File Offset: 0x00023664
		public static bool operator ==(TagName left, TagName right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0002546E File Offset: 0x0002366E
		public static bool operator !=(TagName left, TagName right)
		{
			return !(left == right);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0002547A File Offset: 0x0002367A
		public static bool operator ==(TagName left, string right)
		{
			return object.Equals(left.value, right);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00025488 File Offset: 0x00023688
		public static bool operator !=(TagName left, string right)
		{
			return !(left == right);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00025494 File Offset: 0x00023694
		[NullableContext(2)]
		public static implicit operator TagName(string value)
		{
			if (value != null)
			{
				return new TagName(value);
			}
			return TagName.Empty;
		}

		// Token: 0x04000474 RID: 1140
		public static readonly TagName Empty;

		// Token: 0x04000475 RID: 1141
		[Nullable(2)]
		private readonly string value;
	}
}
