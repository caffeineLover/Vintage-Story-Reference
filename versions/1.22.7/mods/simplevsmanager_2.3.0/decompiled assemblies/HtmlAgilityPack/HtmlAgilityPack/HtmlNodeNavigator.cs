using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace HtmlAgilityPack
{
	// Token: 0x0200001D RID: 29
	[NullableContext(1)]
	[Nullable(0)]
	public class HtmlNodeNavigator : XPathNavigator
	{
		// Token: 0x060001AA RID: 426 RVA: 0x0000CEAE File Offset: 0x0000B0AE
		internal HtmlNodeNavigator()
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this.Reset();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000CED4 File Offset: 0x0000B0D4
		internal HtmlNodeNavigator(HtmlDocument doc, HtmlNode currentNode)
		{
			if (currentNode == null)
			{
				throw new ArgumentNullException("currentNode");
			}
			if (currentNode.OwnerDocument != doc)
			{
				throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
			}
			if (doc == null)
			{
				throw new Exception("Oops! The HtmlDocument cannot be null.");
			}
			this._doc = doc;
			this._nametable = new HtmlNameTable();
			this.Reset();
			this._currentnode = currentNode;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000CF38 File Offset: 0x0000B138
		private HtmlNodeNavigator(HtmlNodeNavigator nav)
		{
			if (nav == null)
			{
				throw new ArgumentNullException("nav");
			}
			this._doc = nav._doc;
			this._currentnode = nav._currentnode;
			this._attindex = nav._attindex;
			this._nametable = nav._nametable;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000CF89 File Offset: 0x0000B189
		public HtmlNodeNavigator(Stream stream)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(stream);
			this.Reset();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000CFB9 File Offset: 0x0000B1B9
		public HtmlNodeNavigator(Stream stream, bool detectEncodingFromByteOrderMarks)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(stream, detectEncodingFromByteOrderMarks);
			this.Reset();
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000CFEA File Offset: 0x0000B1EA
		public HtmlNodeNavigator(Stream stream, Encoding encoding)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(stream, encoding);
			this.Reset();
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000D01B File Offset: 0x0000B21B
		public HtmlNodeNavigator(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(stream, encoding, detectEncodingFromByteOrderMarks);
			this.Reset();
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000D04D File Offset: 0x0000B24D
		public HtmlNodeNavigator(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(stream, encoding, detectEncodingFromByteOrderMarks, buffersize);
			this.Reset();
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000D081 File Offset: 0x0000B281
		public HtmlNodeNavigator(TextReader reader)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(reader);
			this.Reset();
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000D0B1 File Offset: 0x0000B2B1
		public HtmlNodeNavigator(string path)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(path);
			this.Reset();
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000D0E1 File Offset: 0x0000B2E1
		public HtmlNodeNavigator(string path, bool detectEncodingFromByteOrderMarks)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(path, detectEncodingFromByteOrderMarks);
			this.Reset();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000D112 File Offset: 0x0000B312
		public HtmlNodeNavigator(string path, Encoding encoding)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(path, encoding);
			this.Reset();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000D143 File Offset: 0x0000B343
		public HtmlNodeNavigator(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(path, encoding, detectEncodingFromByteOrderMarks);
			this.Reset();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000D175 File Offset: 0x0000B375
		public HtmlNodeNavigator(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			this._doc = new HtmlDocument();
			this._nametable = new HtmlNameTable();
			this._doc.Load(path, encoding, detectEncodingFromByteOrderMarks, buffersize);
			this.Reset();
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000D1A9 File Offset: 0x0000B3A9
		public override string BaseURI
		{
			get
			{
				return this._nametable.GetOrAdd(string.Empty);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000D1BB File Offset: 0x0000B3BB
		public HtmlDocument CurrentDocument
		{
			get
			{
				return this._doc;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000D1C3 File Offset: 0x0000B3C3
		public HtmlNode CurrentNode
		{
			get
			{
				return this._currentnode;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000D1CB File Offset: 0x0000B3CB
		public override bool HasAttributes
		{
			get
			{
				return this._currentnode.Attributes.Count > 0;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		public override bool HasChildren
		{
			get
			{
				return this._currentnode.ChildNodes.Count > 0;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000D1F5 File Offset: 0x0000B3F5
		public override bool IsEmptyElement
		{
			get
			{
				return !this.HasChildren;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000D200 File Offset: 0x0000B400
		public override string LocalName
		{
			get
			{
				if (this._attindex != -1)
				{
					return this._nametable.GetOrAdd(this._currentnode.Attributes[this._attindex].Name);
				}
				return this._nametable.GetOrAdd(this._currentnode.Name);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000D253 File Offset: 0x0000B453
		public override string Name
		{
			get
			{
				return this._nametable.GetOrAdd(this._currentnode.Name);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000D26B File Offset: 0x0000B46B
		public override string NamespaceURI
		{
			get
			{
				return this._nametable.GetOrAdd(string.Empty);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000D27D File Offset: 0x0000B47D
		public override XmlNameTable NameTable
		{
			get
			{
				return this._nametable;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000D288 File Offset: 0x0000B488
		public override XPathNodeType NodeType
		{
			get
			{
				switch (this._currentnode.NodeType)
				{
				case HtmlNodeType.Document:
					return 0;
				case HtmlNodeType.Element:
					if (this._attindex != -1)
					{
						return 2;
					}
					return 1;
				case HtmlNodeType.Comment:
					return 8;
				case HtmlNodeType.Text:
					return 4;
				default:
					throw new NotImplementedException("Internal error: Unhandled HtmlNodeType: " + this._currentnode.NodeType.ToString());
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000D2F4 File Offset: 0x0000B4F4
		public override string Prefix
		{
			get
			{
				return this._nametable.GetOrAdd(string.Empty);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000D308 File Offset: 0x0000B508
		public override string Value
		{
			get
			{
				switch (this._currentnode.NodeType)
				{
				case HtmlNodeType.Document:
					return "";
				case HtmlNodeType.Element:
					if (this._attindex != -1)
					{
						return this._currentnode.Attributes[this._attindex].Value;
					}
					return this._currentnode.InnerText;
				case HtmlNodeType.Comment:
					return ((HtmlCommentNode)this._currentnode).Comment;
				case HtmlNodeType.Text:
					return ((HtmlTextNode)this._currentnode).Text;
				default:
					throw new NotImplementedException("Internal error: Unhandled HtmlNodeType: " + this._currentnode.NodeType.ToString());
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000D3BA File Offset: 0x0000B5BA
		public override string XmlLang
		{
			get
			{
				return this._nametable.GetOrAdd(string.Empty);
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000D3CC File Offset: 0x0000B5CC
		public override XPathNavigator Clone()
		{
			return new HtmlNodeNavigator(this);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000D3D4 File Offset: 0x0000B5D4
		public override string GetAttribute(string localName, string namespaceURI)
		{
			HtmlAttribute htmlAttribute = this._currentnode.HasAttributes ? this._currentnode.Attributes[localName] : null;
			if (htmlAttribute == null)
			{
				return null;
			}
			return htmlAttribute.Value;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000D40E File Offset: 0x0000B60E
		public override string GetNamespace(string name)
		{
			return string.Empty;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000D418 File Offset: 0x0000B618
		public override bool IsSamePosition(XPathNavigator other)
		{
			HtmlNodeNavigator htmlNodeNavigator = other as HtmlNodeNavigator;
			return htmlNodeNavigator != null && htmlNodeNavigator._currentnode == this._currentnode;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000D440 File Offset: 0x0000B640
		public override bool MoveTo(XPathNavigator other)
		{
			HtmlNodeNavigator htmlNodeNavigator = other as HtmlNodeNavigator;
			if (htmlNodeNavigator == null)
			{
				return false;
			}
			if (htmlNodeNavigator._doc == this._doc)
			{
				this._currentnode = htmlNodeNavigator._currentnode;
				this._attindex = htmlNodeNavigator._attindex;
				return true;
			}
			return false;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000D484 File Offset: 0x0000B684
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			int attributeIndex = this._currentnode.Attributes.GetAttributeIndex(localName);
			if (attributeIndex == -1)
			{
				return false;
			}
			this._attindex = attributeIndex;
			return true;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000D4B1 File Offset: 0x0000B6B1
		public override bool MoveToFirst()
		{
			if (this._currentnode.ParentNode == null)
			{
				return false;
			}
			if (this._currentnode.ParentNode.FirstChild == null)
			{
				return false;
			}
			this._currentnode = this._currentnode.ParentNode.FirstChild;
			return true;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000D4ED File Offset: 0x0000B6ED
		public override bool MoveToFirstAttribute()
		{
			if (!this.HasAttributes)
			{
				return false;
			}
			this._attindex = 0;
			return true;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000D501 File Offset: 0x0000B701
		public override bool MoveToFirstChild()
		{
			if (!this._currentnode.HasChildNodes)
			{
				return false;
			}
			this._currentnode = this._currentnode.ChildNodes[0];
			return true;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000D52A File Offset: 0x0000B72A
		public override bool MoveToFirstNamespace(XPathNamespaceScope scope)
		{
			return false;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000D530 File Offset: 0x0000B730
		public override bool MoveToId(string id)
		{
			HtmlNode elementbyId = this._doc.GetElementbyId(id);
			if (elementbyId == null)
			{
				return false;
			}
			this._currentnode = elementbyId;
			return true;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000D557 File Offset: 0x0000B757
		public override bool MoveToNamespace(string name)
		{
			return false;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000D55A File Offset: 0x0000B75A
		public override bool MoveToNext()
		{
			if (this._currentnode.NextSibling == null)
			{
				return false;
			}
			this._currentnode = this._currentnode.NextSibling;
			return true;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000D57D File Offset: 0x0000B77D
		public override bool MoveToNextAttribute()
		{
			if (this._attindex >= this._currentnode.Attributes.Count - 1)
			{
				return false;
			}
			this._attindex++;
			return true;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000D5AA File Offset: 0x0000B7AA
		public override bool MoveToNextNamespace(XPathNamespaceScope scope)
		{
			return false;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000D5AD File Offset: 0x0000B7AD
		public override bool MoveToParent()
		{
			if (this._currentnode.ParentNode == null)
			{
				return false;
			}
			this._currentnode = this._currentnode.ParentNode;
			return true;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000D5D0 File Offset: 0x0000B7D0
		public override bool MoveToPrevious()
		{
			if (this._currentnode.PreviousSibling == null)
			{
				return false;
			}
			this._currentnode = this._currentnode.PreviousSibling;
			return true;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000D5F3 File Offset: 0x0000B7F3
		public override void MoveToRoot()
		{
			this._currentnode = this._doc.DocumentNode;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000D606 File Offset: 0x0000B806
		private void Reset()
		{
			this._currentnode = this._doc.DocumentNode;
			this._attindex = -1;
		}

		// Token: 0x04000095 RID: 149
		private int _attindex;

		// Token: 0x04000096 RID: 150
		private HtmlNode _currentnode;

		// Token: 0x04000097 RID: 151
		private readonly HtmlDocument _doc;

		// Token: 0x04000098 RID: 152
		private readonly HtmlNameTable _nametable;

		// Token: 0x04000099 RID: 153
		internal bool Trace;
	}
}
