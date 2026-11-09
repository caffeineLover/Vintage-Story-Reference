using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000008 RID: 8
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerDisplay("Name: {OriginalName}, Value: {Value}")]
	public class HtmlAttribute : IComparable
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000021CA File Offset: 0x000003CA
		internal HtmlAttribute(HtmlDocument ownerdocument)
		{
			this._ownerdocument = ownerdocument;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000021D9 File Offset: 0x000003D9
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000021E1 File Offset: 0x000003E1
		public int Line
		{
			get
			{
				return this._line;
			}
			internal set
			{
				this._line = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000021EA File Offset: 0x000003EA
		public int LinePosition
		{
			get
			{
				return this._lineposition;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000021F2 File Offset: 0x000003F2
		public int ValueStartIndex
		{
			get
			{
				return this._valuestartindex;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000021FA File Offset: 0x000003FA
		public int ValueLength
		{
			get
			{
				return this._valuelength;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002204 File Offset: 0x00000404
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002243 File Offset: 0x00000443
		public bool UseOriginalName
		{
			get
			{
				bool result = false;
				if (this._localUseOriginalName != null)
				{
					result = this._localUseOriginalName.Value;
				}
				else if (this.OwnerDocument != null)
				{
					result = this.OwnerDocument.OptionDefaultUseOriginalName;
				}
				return result;
			}
			set
			{
				this._localUseOriginalName = new bool?(value);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002254 File Offset: 0x00000454
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000022A5 File Offset: 0x000004A5
		public string Name
		{
			get
			{
				if (this._name == null)
				{
					this._name = this._ownerdocument.Text.Substring(this._namestartindex, this._namelength);
				}
				if (!this.UseOriginalName)
				{
					return this._name.ToLowerInvariant();
				}
				return this._name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._name = value;
				if (this._ownernode != null)
				{
					this._ownernode.SetChanged();
				}
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000022CF File Offset: 0x000004CF
		public string OriginalName
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000022D7 File Offset: 0x000004D7
		public HtmlDocument OwnerDocument
		{
			get
			{
				return this._ownerdocument;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000022DF File Offset: 0x000004DF
		public HtmlNode OwnerNode
		{
			get
			{
				return this._ownernode;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000022E8 File Offset: 0x000004E8
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002338 File Offset: 0x00000538
		public AttributeValueQuote QuoteType
		{
			get
			{
				AttributeValueQuote? quoteType = this._quoteType;
				if (quoteType != null)
				{
					return quoteType.GetValueOrDefault();
				}
				AttributeValueQuote? internalQuoteType = this.InternalQuoteType;
				if (internalQuoteType == null)
				{
					return this.OwnerDocument.GlobalAttributeValueQuote.GetValueOrDefault(AttributeValueQuote.DoubleQuote);
				}
				return internalQuoteType.GetValueOrDefault();
			}
			set
			{
				this._quoteType = ((value != AttributeValueQuote.Initial) ? new AttributeValueQuote?(value) : default(AttributeValueQuote?));
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002360 File Offset: 0x00000560
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002368 File Offset: 0x00000568
		internal AttributeValueQuote? InternalQuoteType { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002371 File Offset: 0x00000571
		public int StreamPosition
		{
			get
			{
				return this._streamposition;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000237C File Offset: 0x0000057C
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002400 File Offset: 0x00000600
		public string Value
		{
			get
			{
				if (this._value == null && this._ownerdocument.Text == null && this._valuestartindex == 0 && this._valuelength == 0)
				{
					return null;
				}
				if (this._value == null)
				{
					this._value = this._ownerdocument.Text.Substring(this._valuestartindex, this._valuelength);
					if (!this._ownerdocument.BackwardCompatibility)
					{
						this._value = HtmlEntity.DeEntitize(this._value);
					}
				}
				return this._value;
			}
			set
			{
				this._value = value;
				if (!string.IsNullOrEmpty(this._value) && (this.QuoteType == AttributeValueQuote.WithoutValue || this.QuoteType == AttributeValueQuote.None))
				{
					this.InternalQuoteType = new AttributeValueQuote?((this.OwnerDocument.GlobalAttributeValueQuote.GetValueOrDefault() != AttributeValueQuote.Initial) ? this.OwnerDocument.GlobalAttributeValueQuote.GetValueOrDefault(AttributeValueQuote.DoubleQuote) : AttributeValueQuote.DoubleQuote);
				}
				if (this._ownernode != null)
				{
					this._ownernode.SetChanged();
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002478 File Offset: 0x00000678
		public string DeEntitizeValue
		{
			get
			{
				return HtmlEntity.DeEntitize(this.Value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002485 File Offset: 0x00000685
		internal string XmlName
		{
			get
			{
				return HtmlDocument.GetXmlName(this.Name, true, this.OwnerDocument.OptionPreserveXmlNamespaces);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000249E File Offset: 0x0000069E
		internal string XmlValue
		{
			get
			{
				return this.Value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000024A6 File Offset: 0x000006A6
		public string XPath
		{
			get
			{
				return ((this.OwnerNode == null) ? "/" : (this.OwnerNode.XPath + "/")) + this.GetRelativeXpath();
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000024D8 File Offset: 0x000006D8
		public int CompareTo(object obj)
		{
			HtmlAttribute htmlAttribute = obj as HtmlAttribute;
			if (htmlAttribute == null)
			{
				throw new ArgumentException("obj");
			}
			return this.Name.CompareTo(htmlAttribute.Name);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000250B File Offset: 0x0000070B
		public HtmlAttribute Clone()
		{
			return new HtmlAttribute(this._ownerdocument)
			{
				Name = this.OriginalName,
				Value = this.Value,
				_quoteType = this._quoteType,
				InternalQuoteType = this.InternalQuoteType
			};
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002548 File Offset: 0x00000748
		public void Remove()
		{
			this._ownernode.Attributes.Remove(this);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000255C File Offset: 0x0000075C
		private string GetRelativeXpath()
		{
			if (this.OwnerNode == null)
			{
				return this.Name;
			}
			int num = 1;
			foreach (HtmlAttribute htmlAttribute in this.OwnerNode.Attributes)
			{
				if (!(htmlAttribute.Name != this.Name))
				{
					if (htmlAttribute == this)
					{
						break;
					}
					num++;
				}
			}
			return string.Concat(new string[]
			{
				"@",
				this.Name,
				"[",
				num.ToString(),
				"]"
			});
		}

		// Token: 0x04000007 RID: 7
		private int _line;

		// Token: 0x04000008 RID: 8
		internal int _lineposition;

		// Token: 0x04000009 RID: 9
		internal string _name;

		// Token: 0x0400000A RID: 10
		internal int _namelength;

		// Token: 0x0400000B RID: 11
		internal int _namestartindex;

		// Token: 0x0400000C RID: 12
		internal HtmlDocument _ownerdocument;

		// Token: 0x0400000D RID: 13
		internal HtmlNode _ownernode;

		// Token: 0x0400000E RID: 14
		private AttributeValueQuote? _quoteType;

		// Token: 0x0400000F RID: 15
		internal int _streamposition;

		// Token: 0x04000010 RID: 16
		internal string _value;

		// Token: 0x04000011 RID: 17
		internal int _valuelength;

		// Token: 0x04000012 RID: 18
		internal int _valuestartindex;

		// Token: 0x04000013 RID: 19
		private bool? _localUseOriginalName;
	}
}
