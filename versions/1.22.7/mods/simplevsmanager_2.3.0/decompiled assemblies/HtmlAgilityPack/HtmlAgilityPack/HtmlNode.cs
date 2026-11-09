using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace HtmlAgilityPack
{
	// Token: 0x02000012 RID: 18
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerDisplay("Name: {OriginalName}")]
	public class HtmlNode : IXPathNavigable
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00008BC4 File Offset: 0x00006DC4
		static HtmlNode()
		{
			HtmlNode.ElementsFlags = new Dictionary<string, HtmlElementFlag>(StringComparer.OrdinalIgnoreCase);
			HtmlNode.ElementsFlags.Add("script", HtmlElementFlag.CData);
			HtmlNode.ElementsFlags.Add("style", HtmlElementFlag.CData);
			HtmlNode.ElementsFlags.Add("noxhtml", HtmlElementFlag.CData);
			HtmlNode.ElementsFlags.Add("textarea", HtmlElementFlag.CData);
			HtmlNode.ElementsFlags.Add("title", HtmlElementFlag.CData);
			HtmlNode.ElementsFlags.Add("base", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("link", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("meta", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("isindex", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("hr", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("col", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("img", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("param", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("embed", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("frame", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("wbr", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("bgsound", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("spacer", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("keygen", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("area", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("input", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("basefont", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("source", HtmlElementFlag.Empty);
			HtmlNode.ElementsFlags.Add("form", HtmlElementFlag.CanOverlap);
			HtmlNode.ElementsFlags.Add("br", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
			if (!HtmlDocument.DisableBehaviorTagP)
			{
				HtmlNode.ElementsFlags.Add("p", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00008DCC File Offset: 0x00006FCC
		public HtmlNode(HtmlNodeType type, HtmlDocument ownerdocument, int index)
		{
			this._nodetype = type;
			this._ownerdocument = ownerdocument;
			this._outerstartindex = index;
			switch (type)
			{
			case HtmlNodeType.Document:
				this.SetName(HtmlNode.HtmlNodeTypeNameDocument);
				this._endnode = this;
				break;
			case HtmlNodeType.Comment:
				this.SetName(HtmlNode.HtmlNodeTypeNameComment);
				this._endnode = this;
				break;
			case HtmlNodeType.Text:
				this.SetName(HtmlNode.HtmlNodeTypeNameText);
				this._endnode = this;
				break;
			}
			if (this._ownerdocument.Openednodes != null && !this.Closed && -1 != index)
			{
				this._ownerdocument.Openednodes.Add(index, this);
			}
			if (-1 != index || type == HtmlNodeType.Comment || type == HtmlNodeType.Text)
			{
				return;
			}
			this.SetChanged();
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00008E84 File Offset: 0x00007084
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00008EA0 File Offset: 0x000070A0
		public HtmlAttributeCollection Attributes
		{
			get
			{
				if (!this.HasAttributes)
				{
					this._attributes = new HtmlAttributeCollection(this);
				}
				return this._attributes;
			}
			internal set
			{
				this._attributes = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00008EAC File Offset: 0x000070AC
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00008ED2 File Offset: 0x000070D2
		public HtmlNodeCollection ChildNodes
		{
			get
			{
				HtmlNodeCollection result;
				if ((result = this._childnodes) == null)
				{
					result = (this._childnodes = new HtmlNodeCollection(this));
				}
				return result;
			}
			internal set
			{
				this._childnodes = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00008EDB File Offset: 0x000070DB
		public bool Closed
		{
			get
			{
				return this._endnode != null;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00008EE6 File Offset: 0x000070E6
		public HtmlAttributeCollection ClosingAttributes
		{
			get
			{
				if (this.HasClosingAttributes)
				{
					return this._endnode.Attributes;
				}
				return new HtmlAttributeCollection(this);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00008F02 File Offset: 0x00007102
		public HtmlNode EndNode
		{
			get
			{
				return this._endnode;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00008F0A File Offset: 0x0000710A
		public HtmlNode FirstChild
		{
			get
			{
				if (this.HasChildNodes)
				{
					return this._childnodes[0];
				}
				return null;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00008F22 File Offset: 0x00007122
		public bool HasAttributes
		{
			get
			{
				return this._attributes != null && this._attributes.Count > 0;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00008F3C File Offset: 0x0000713C
		public bool HasChildNodes
		{
			get
			{
				return this._childnodes != null && this._childnodes.Count > 0;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00008F59 File Offset: 0x00007159
		public bool HasClosingAttributes
		{
			get
			{
				return this._endnode != null && this._endnode != this && this._endnode._attributes != null && this._endnode._attributes.Count > 0;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00008F93 File Offset: 0x00007193
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00008FB3 File Offset: 0x000071B3
		public string Id
		{
			get
			{
				if (this._ownerdocument.Nodesid == null)
				{
					throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
				}
				return this.GetId();
			}
			set
			{
				if (this._ownerdocument.Nodesid == null)
				{
					throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.SetId(value);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00008FE4 File Offset: 0x000071E4
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000904C File Offset: 0x0000724C
		public virtual string InnerHtml
		{
			get
			{
				if (this._changed)
				{
					this.UpdateHtml();
					return this._innerhtml;
				}
				if (this._innerhtml != null)
				{
					return this._innerhtml;
				}
				if (this._innerstartindex < 0 || this._innerlength < 0)
				{
					return string.Empty;
				}
				return this._ownerdocument.Text.Substring(this._innerstartindex, this._innerlength);
			}
			set
			{
				HtmlDocument htmlDocument = new HtmlDocument();
				htmlDocument.LoadHtml(value);
				this.RemoveAllChildren();
				this.AppendChildren(htmlDocument.DocumentNode.ChildNodes);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00009080 File Offset: 0x00007280
		public virtual string InnerText
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int depthLevel = 0;
				string name = this.Name;
				if (name != null)
				{
					bool isDisplayScriptingText = string.Equals(name, "head", 5) || string.Equals(name, "script", 5) || string.Equals(name, "style", 5);
					this.InternalInnerText(stringBuilder, isDisplayScriptingText, depthLevel);
				}
				else
				{
					this.InternalInnerText(stringBuilder, false, depthLevel);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000090E8 File Offset: 0x000072E8
		internal virtual void InternalInnerText(StringBuilder sb, bool isDisplayScriptingText, int depthLevel)
		{
			depthLevel++;
			if (depthLevel > HtmlDocument.MaxDepthLevel)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Maximum deep level reached: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(HtmlDocument.MaxDepthLevel);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (!this._ownerdocument.BackwardCompatibility)
			{
				if (this.HasChildNodes)
				{
					this.AppendInnerText(sb, isDisplayScriptingText);
					return;
				}
				sb.Append(this.GetCurrentNodeText());
				return;
			}
			else
			{
				if (this._ownerdocument.OptionEnableBreakLineForInnerText && this.Name == "br")
				{
					sb.AppendLine();
					return;
				}
				if (this._nodetype == HtmlNodeType.Text)
				{
					sb.Append(((HtmlTextNode)this).Text);
					return;
				}
				if (this._nodetype == HtmlNodeType.Comment)
				{
					return;
				}
				if (!this.HasChildNodes || (this._isHideInnerText && !isDisplayScriptingText))
				{
					return;
				}
				foreach (HtmlNode htmlNode in this.ChildNodes)
				{
					htmlNode.InternalInnerText(sb, isDisplayScriptingText, depthLevel);
				}
				return;
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00009204 File Offset: 0x00007404
		public virtual string GetDirectInnerText()
		{
			if (!this._ownerdocument.BackwardCompatibility)
			{
				if (this.HasChildNodes)
				{
					StringBuilder stringBuilder = new StringBuilder();
					this.AppendDirectInnerText(stringBuilder);
					return stringBuilder.ToString();
				}
				return this.GetCurrentNodeText();
			}
			else
			{
				if (this._nodetype == HtmlNodeType.Text)
				{
					return ((HtmlTextNode)this).Text;
				}
				if (this._nodetype == HtmlNodeType.Comment)
				{
					return "";
				}
				if (!this.HasChildNodes)
				{
					return string.Empty;
				}
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (HtmlNode htmlNode in this.ChildNodes)
				{
					if (htmlNode._nodetype == HtmlNodeType.Text)
					{
						stringBuilder2.Append(((HtmlTextNode)htmlNode).Text);
					}
				}
				return stringBuilder2.ToString();
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000092DC File Offset: 0x000074DC
		internal string GetCurrentNodeText()
		{
			if (this._nodetype == HtmlNodeType.Text)
			{
				string text = ((HtmlTextNode)this).Text;
				if (this.ParentNode.Name != "pre")
				{
					text = text.Replace("\n", "").Replace("\r", "").Replace("\t", "");
				}
				return text;
			}
			return "";
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000934C File Offset: 0x0000754C
		internal void AppendDirectInnerText(StringBuilder sb)
		{
			if (this._nodetype == HtmlNodeType.Text)
			{
				sb.Append(this.GetCurrentNodeText());
			}
			if (!this.HasChildNodes)
			{
				return;
			}
			foreach (HtmlNode htmlNode in this.ChildNodes)
			{
				sb.Append(htmlNode.GetCurrentNodeText());
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000093C4 File Offset: 0x000075C4
		internal void AppendInnerText(StringBuilder sb, bool isShowHideInnerText)
		{
			if (this._nodetype == HtmlNodeType.Text)
			{
				sb.Append(this.GetCurrentNodeText());
			}
			if (!this.HasChildNodes || (this._isHideInnerText && !isShowHideInnerText))
			{
				return;
			}
			foreach (HtmlNode htmlNode in this.ChildNodes)
			{
				htmlNode.AppendInnerText(sb, isShowHideInnerText);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00009440 File Offset: 0x00007640
		public HtmlNode LastChild
		{
			get
			{
				if (this.HasChildNodes)
				{
					return this._childnodes[this._childnodes.Count - 1];
				}
				return null;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00009464 File Offset: 0x00007664
		// (set) Token: 0x060000EF RID: 239 RVA: 0x0000946C File Offset: 0x0000766C
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00009475 File Offset: 0x00007675
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x0000947D File Offset: 0x0000767D
		public int LinePosition
		{
			get
			{
				return this._lineposition;
			}
			internal set
			{
				this._lineposition = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00009486 File Offset: 0x00007686
		public int InnerStartIndex
		{
			get
			{
				return this._innerstartindex;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000948E File Offset: 0x0000768E
		public int OuterStartIndex
		{
			get
			{
				return this._outerstartindex;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00009496 File Offset: 0x00007696
		public int InnerLength
		{
			get
			{
				return this.InnerHtml.Length;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x000094A3 File Offset: 0x000076A3
		public int OuterLength
		{
			get
			{
				return this.OuterHtml.Length;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000094B0 File Offset: 0x000076B0
		public int OriginalOuterLength
		{
			get
			{
				return this._outerlength;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000094B8 File Offset: 0x000076B8
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00009556 File Offset: 0x00007756
		public string Name
		{
			get
			{
				if (this._optimizedName == null)
				{
					if (this._name == null)
					{
						this.SetName(this._ownerdocument.Text.Substring(this._namestartindex, this._namelength));
					}
					if (this._name == null)
					{
						this._optimizedName = string.Empty;
					}
					else if (this.OwnerDocument != null)
					{
						this._optimizedName = (this.OwnerDocument.OptionDefaultUseOriginalName ? this._name : this._name.ToLowerInvariant());
					}
					else
					{
						this._optimizedName = this._name.ToLowerInvariant();
					}
				}
				return this._optimizedName;
			}
			set
			{
				this.SetName(value);
				if (!(this is HtmlTextNode))
				{
					this.SetChanged();
				}
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000956D File Offset: 0x0000776D
		internal void SetName(string value)
		{
			this._name = value;
			this._optimizedName = null;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000957D File Offset: 0x0000777D
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00009585 File Offset: 0x00007785
		public HtmlNode NextSibling
		{
			get
			{
				return this._nextnode;
			}
			internal set
			{
				this._nextnode = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000958E File Offset: 0x0000778E
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00009596 File Offset: 0x00007796
		public HtmlNodeType NodeType
		{
			get
			{
				return this._nodetype;
			}
			internal set
			{
				this._nodetype = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000959F File Offset: 0x0000779F
		public string OriginalName
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000095A8 File Offset: 0x000077A8
		public virtual string OuterHtml
		{
			get
			{
				if (this._changed)
				{
					this.UpdateHtml();
					return this._outerhtml;
				}
				if (this._outerhtml != null)
				{
					return this._outerhtml;
				}
				if (this._outerstartindex < 0 || this._outerlength < 0)
				{
					return string.Empty;
				}
				return this._ownerdocument.Text.Substring(this._outerstartindex, this._outerlength);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000960D File Offset: 0x0000780D
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00009615 File Offset: 0x00007815
		public HtmlDocument OwnerDocument
		{
			get
			{
				return this._ownerdocument;
			}
			internal set
			{
				this._ownerdocument = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000961E File Offset: 0x0000781E
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00009626 File Offset: 0x00007826
		public HtmlNode ParentNode
		{
			get
			{
				return this._parentnode;
			}
			internal set
			{
				this._parentnode = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000962F File Offset: 0x0000782F
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00009637 File Offset: 0x00007837
		public HtmlNode PreviousSibling
		{
			get
			{
				return this._prevnode;
			}
			internal set
			{
				this._prevnode = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00009640 File Offset: 0x00007840
		public int StreamPosition
		{
			get
			{
				return this._streamposition;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00009648 File Offset: 0x00007848
		public string XPath
		{
			get
			{
				return ((this.ParentNode == null || this.ParentNode.NodeType == HtmlNodeType.Document) ? "/" : (this.ParentNode.XPath + "/")) + this.GetRelativeXpath();
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00009686 File Offset: 0x00007886
		// (set) Token: 0x06000109 RID: 265 RVA: 0x0000968E File Offset: 0x0000788E
		public int Depth { get; set; }

		// Token: 0x0600010A RID: 266 RVA: 0x00009698 File Offset: 0x00007898
		public static bool CanOverlapElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlElementFlag htmlElementFlag;
			return HtmlNode.ElementsFlags.TryGetValue(name, ref htmlElementFlag) && (htmlElementFlag & HtmlElementFlag.CanOverlap) > (HtmlElementFlag)0;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000096CA File Offset: 0x000078CA
		public static HtmlNode CreateNode(string html)
		{
			return HtmlNode.CreateNode(html, null);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000096D4 File Offset: 0x000078D4
		public static HtmlNode CreateNode(string html, Action<HtmlDocument> htmlDocumentBuilder)
		{
			HtmlDocument htmlDocument = new HtmlDocument();
			if (htmlDocumentBuilder != null)
			{
				htmlDocumentBuilder.Invoke(htmlDocument);
			}
			htmlDocument.LoadHtml(html);
			if (!htmlDocument.DocumentNode.IsSingleElementNode())
			{
				throw new Exception("Multiple node elements can't be created.");
			}
			for (HtmlNode htmlNode = htmlDocument.DocumentNode.FirstChild; htmlNode != null; htmlNode = htmlNode.NextSibling)
			{
				if (htmlNode.NodeType == HtmlNodeType.Element && htmlNode.OuterHtml != "\r\n")
				{
					return htmlNode;
				}
			}
			return htmlDocument.DocumentNode.FirstChild;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00009750 File Offset: 0x00007950
		public static bool IsCDataElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlElementFlag htmlElementFlag;
			return HtmlNode.ElementsFlags.TryGetValue(name, ref htmlElementFlag) && (htmlElementFlag & HtmlElementFlag.CData) > (HtmlElementFlag)0;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00009784 File Offset: 0x00007984
		public static bool IsClosedElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlElementFlag htmlElementFlag;
			return HtmlNode.ElementsFlags.TryGetValue(name, ref htmlElementFlag) && (htmlElementFlag & HtmlElementFlag.Closed) > (HtmlElementFlag)0;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000097B8 File Offset: 0x000079B8
		public static bool IsEmptyElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlElementFlag htmlElementFlag;
			return name.Length == 0 || '!' == name.get_Chars(0) || '?' == name.get_Chars(0) || (HtmlNode.ElementsFlags.TryGetValue(name, ref htmlElementFlag) && (htmlElementFlag & HtmlElementFlag.Empty) > (HtmlElementFlag)0);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00009810 File Offset: 0x00007A10
		public static bool IsOverlappedClosingElement(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			return text.Length >= 4 && text.get_Chars(0) == '<' && text.get_Chars(text.Length - 1) == '>' && text.get_Chars(1) == '/' && HtmlNode.CanOverlapElement(text.Substring(2, text.Length - 3));
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00009874 File Offset: 0x00007A74
		public IEnumerable<HtmlNode> Ancestors()
		{
			HtmlNode.<Ancestors>d__122 <Ancestors>d__ = new HtmlNode.<Ancestors>d__122(-2);
			<Ancestors>d__.<>4__this = this;
			return <Ancestors>d__;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00009884 File Offset: 0x00007A84
		public IEnumerable<HtmlNode> Ancestors(string name)
		{
			HtmlNode.<Ancestors>d__123 <Ancestors>d__ = new HtmlNode.<Ancestors>d__123(-2);
			<Ancestors>d__.<>4__this = this;
			<Ancestors>d__.<>3__name = name;
			return <Ancestors>d__;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000989B File Offset: 0x00007A9B
		public IEnumerable<HtmlNode> AncestorsAndSelf()
		{
			HtmlNode.<AncestorsAndSelf>d__124 <AncestorsAndSelf>d__ = new HtmlNode.<AncestorsAndSelf>d__124(-2);
			<AncestorsAndSelf>d__.<>4__this = this;
			return <AncestorsAndSelf>d__;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000098AB File Offset: 0x00007AAB
		public IEnumerable<HtmlNode> AncestorsAndSelf(string name)
		{
			HtmlNode.<AncestorsAndSelf>d__125 <AncestorsAndSelf>d__ = new HtmlNode.<AncestorsAndSelf>d__125(-2);
			<AncestorsAndSelf>d__.<>4__this = this;
			<AncestorsAndSelf>d__.<>3__name = name;
			return <AncestorsAndSelf>d__;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000098C4 File Offset: 0x00007AC4
		public HtmlNode AppendChild(HtmlNode newChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			this.ChildNodes.Append(newChild);
			this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
			this.SetChildNodesId(newChild);
			HtmlNode parentnode = this._parentnode;
			HtmlDocument htmlDocument = null;
			while (parentnode != null)
			{
				if (parentnode.OwnerDocument != htmlDocument)
				{
					parentnode.OwnerDocument.SetIdForNode(newChild, newChild.GetId());
					parentnode.SetChildNodesId(newChild);
					htmlDocument = parentnode.OwnerDocument;
				}
				parentnode = parentnode._parentnode;
			}
			this.SetChanged();
			return newChild;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000994C File Offset: 0x00007B4C
		public void SetChildNodesId(HtmlNode chilNode)
		{
			foreach (HtmlNode htmlNode in chilNode.ChildNodes)
			{
				this._ownerdocument.SetIdForNode(htmlNode, htmlNode.GetId());
				if (htmlNode.ChildNodes == chilNode.ChildNodes)
				{
					throw new Exception("Oops! a scenario that will cause a Stack Overflow has been found. See the following issue for an example: https://github.com/zzzprojects/html-agility-pack/issues/513");
				}
				this.SetChildNodesId(htmlNode);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000099CC File Offset: 0x00007BCC
		public void AppendChildren(HtmlNodeCollection newChildren)
		{
			if (newChildren == null)
			{
				throw new ArgumentNullException("newChildren");
			}
			foreach (HtmlNode newChild in newChildren)
			{
				this.AppendChild(newChild);
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00009A2C File Offset: 0x00007C2C
		public IEnumerable<HtmlAttribute> ChildAttributes(string name)
		{
			if (!this.HasAttributes)
			{
				return Enumerable.Empty<HtmlAttribute>();
			}
			return this.Attributes.AttributesWithName(name);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00009A48 File Offset: 0x00007C48
		public HtmlNode Clone()
		{
			return this.CloneNode(true);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00009A51 File Offset: 0x00007C51
		public HtmlNode CloneNode(string newName)
		{
			return this.CloneNode(newName, true);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00009A5B File Offset: 0x00007C5B
		public HtmlNode CloneNode(string newName, bool deep)
		{
			if (newName == null)
			{
				throw new ArgumentNullException("newName");
			}
			HtmlNode htmlNode = this.CloneNode(deep);
			htmlNode.SetName(newName);
			return htmlNode;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00009A7C File Offset: 0x00007C7C
		public HtmlNode CloneNode(bool deep)
		{
			HtmlNode htmlNode = this._ownerdocument.CreateNode(this._nodetype);
			htmlNode.SetName(this.OriginalName);
			HtmlNodeType nodetype = this._nodetype;
			if (nodetype == HtmlNodeType.Comment)
			{
				((HtmlCommentNode)htmlNode).Comment = ((HtmlCommentNode)this).Comment;
				return htmlNode;
			}
			if (nodetype == HtmlNodeType.Text)
			{
				((HtmlTextNode)htmlNode).Text = ((HtmlTextNode)this).Text;
				return htmlNode;
			}
			if (this.HasAttributes)
			{
				foreach (HtmlAttribute htmlAttribute in this._attributes)
				{
					HtmlAttribute newAttribute = htmlAttribute.Clone();
					htmlNode.Attributes.Append(newAttribute);
				}
			}
			if (this.HasClosingAttributes)
			{
				htmlNode._endnode = this._endnode.CloneNode(false);
				foreach (HtmlAttribute htmlAttribute2 in this._endnode._attributes)
				{
					HtmlAttribute newAttribute2 = htmlAttribute2.Clone();
					htmlNode._endnode._attributes.Append(newAttribute2);
				}
			}
			if (!deep)
			{
				return htmlNode;
			}
			if (!this.HasChildNodes)
			{
				return htmlNode;
			}
			foreach (HtmlNode htmlNode2 in this._childnodes)
			{
				HtmlNode newChild = htmlNode2.CloneNode(deep);
				htmlNode.AppendChild(newChild);
			}
			return htmlNode;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00009C04 File Offset: 0x00007E04
		public void CopyFrom(HtmlNode node)
		{
			this.CopyFrom(node, true);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00009C10 File Offset: 0x00007E10
		public void CopyFrom(HtmlNode node, bool deep)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this.Attributes.RemoveAll();
			if (node.HasAttributes)
			{
				foreach (HtmlAttribute htmlAttribute in node.Attributes)
				{
					HtmlAttribute newAttribute = htmlAttribute.Clone();
					this.Attributes.Append(newAttribute);
				}
			}
			if (deep)
			{
				this.RemoveAllChildren();
				if (node.HasChildNodes)
				{
					foreach (HtmlNode htmlNode in node.ChildNodes)
					{
						this.AppendChild(htmlNode.CloneNode(true));
					}
				}
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00009CE4 File Offset: 0x00007EE4
		[Obsolete("Use Descendants() instead, the results of this function will change in a future version")]
		public IEnumerable<HtmlNode> DescendantNodes(int level = 0)
		{
			HtmlNode.<DescendantNodes>d__136 <DescendantNodes>d__ = new HtmlNode.<DescendantNodes>d__136(-2);
			<DescendantNodes>d__.<>4__this = this;
			<DescendantNodes>d__.<>3__level = level;
			return <DescendantNodes>d__;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00009CFB File Offset: 0x00007EFB
		[Obsolete("Use DescendantsAndSelf() instead, the results of this function will change in a future version")]
		public IEnumerable<HtmlNode> DescendantNodesAndSelf()
		{
			return this.DescendantsAndSelf();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00009D03 File Offset: 0x00007F03
		public IEnumerable<HtmlNode> Descendants()
		{
			return this.Descendants(0);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00009D0C File Offset: 0x00007F0C
		public IEnumerable<HtmlNode> Descendants(int level)
		{
			HtmlNode.<Descendants>d__139 <Descendants>d__ = new HtmlNode.<Descendants>d__139(-2);
			<Descendants>d__.<>4__this = this;
			<Descendants>d__.<>3__level = level;
			return <Descendants>d__;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00009D23 File Offset: 0x00007F23
		public IEnumerable<HtmlNode> Descendants(string name)
		{
			HtmlNode.<Descendants>d__140 <Descendants>d__ = new HtmlNode.<Descendants>d__140(-2);
			<Descendants>d__.<>4__this = this;
			<Descendants>d__.<>3__name = name;
			return <Descendants>d__;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00009D3A File Offset: 0x00007F3A
		public IEnumerable<HtmlNode> DescendantsAndSelf()
		{
			HtmlNode.<DescendantsAndSelf>d__141 <DescendantsAndSelf>d__ = new HtmlNode.<DescendantsAndSelf>d__141(-2);
			<DescendantsAndSelf>d__.<>4__this = this;
			return <DescendantsAndSelf>d__;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00009D4A File Offset: 0x00007F4A
		public IEnumerable<HtmlNode> DescendantsAndSelf(string name)
		{
			HtmlNode.<DescendantsAndSelf>d__142 <DescendantsAndSelf>d__ = new HtmlNode.<DescendantsAndSelf>d__142(-2);
			<DescendantsAndSelf>d__.<>4__this = this;
			<DescendantsAndSelf>d__.<>3__name = name;
			return <DescendantsAndSelf>d__;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00009D64 File Offset: 0x00007F64
		public HtmlNode Element(string name)
		{
			foreach (HtmlNode htmlNode in this.ChildNodes)
			{
				if (htmlNode.Name == name)
				{
					return htmlNode;
				}
			}
			return null;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00009DC8 File Offset: 0x00007FC8
		public IEnumerable<HtmlNode> Elements(string name)
		{
			HtmlNode.<Elements>d__144 <Elements>d__ = new HtmlNode.<Elements>d__144(-2);
			<Elements>d__.<>4__this = this;
			<Elements>d__.<>3__name = name;
			return <Elements>d__;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00009DE0 File Offset: 0x00007FE0
		public HtmlAttribute GetDataAttribute(string key)
		{
			return Enumerable.SingleOrDefault<KeyValuePair<string, HtmlAttribute>>(this.Attributes.Hashitems, (KeyValuePair<string, HtmlAttribute> x) => x.Key.Equals("data-" + key, 5)).Value;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00009E20 File Offset: 0x00008020
		public IEnumerable<HtmlAttribute> GetDataAttributes()
		{
			return Enumerable.ToList<HtmlAttribute>(Enumerable.Select<KeyValuePair<string, HtmlAttribute>, HtmlAttribute>(Enumerable.Where<KeyValuePair<string, HtmlAttribute>>(this.Attributes.Hashitems, (KeyValuePair<string, HtmlAttribute> x) => x.Key.StartsWith("data-", 5)), (KeyValuePair<string, HtmlAttribute> x) => x.Value));
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00009E85 File Offset: 0x00008085
		public IEnumerable<HtmlAttribute> GetAttributes()
		{
			return this.Attributes.items;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00009E94 File Offset: 0x00008094
		public IEnumerable<HtmlAttribute> GetAttributes(params string[] attributeNames)
		{
			List<HtmlAttribute> list = new List<HtmlAttribute>();
			foreach (string name in attributeNames)
			{
				list.Add(this.Attributes[name]);
			}
			return list;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00009ECE File Offset: 0x000080CE
		public string GetAttributeValue(string name, string def)
		{
			return this.GetAttributeValue<string>(name, def, null);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00009ED9 File Offset: 0x000080D9
		public int GetAttributeValue(string name, int def)
		{
			HtmlNode.AttributeValueParser<int> parser;
			if ((parser = HtmlNode.<>O.<0>__TryParse) == null)
			{
				parser = (HtmlNode.<>O.<0>__TryParse = new HtmlNode.AttributeValueParser<int>(int.TryParse));
			}
			return this.GetAttributeValue<int>(name, def, parser);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00009EFE File Offset: 0x000080FE
		public bool GetAttributeValue(string name, bool def)
		{
			HtmlNode.AttributeValueParser<bool> parser;
			if ((parser = HtmlNode.<>O.<1>__TryParse) == null)
			{
				parser = (HtmlNode.<>O.<1>__TryParse = new HtmlNode.AttributeValueParser<bool>(bool.TryParse));
			}
			return this.GetAttributeValue<bool>(name, def, parser);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00009F24 File Offset: 0x00008124
		public T GetAttributeValue<[Nullable(2)] T>(string name, T def)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!this.HasAttributes)
			{
				return def;
			}
			HtmlAttribute htmlAttribute = this.Attributes[name];
			if (htmlAttribute == null)
			{
				return def;
			}
			T result;
			try
			{
				result = (T)((object)htmlAttribute.Value.To(typeof(T)));
			}
			catch
			{
				result = def;
			}
			return result;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00009F90 File Offset: 0x00008190
		public T GetAttributeValue<[Nullable(2)] T>(string name, T def, HtmlNode.AttributeValueParser<T> parser)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!this.HasAttributes)
			{
				return def;
			}
			HtmlAttribute htmlAttribute = this.Attributes[name];
			if (htmlAttribute == null || htmlAttribute.Value == null)
			{
				return def;
			}
			string value = htmlAttribute.Value;
			if (value is T)
			{
				return value as T;
			}
			T result;
			if (parser != null && parser.Invoke(htmlAttribute.Value, ref result))
			{
				return result;
			}
			return def;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000A004 File Offset: 0x00008204
		public HtmlNode InsertAfter(HtmlNode newChild, HtmlNode refChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			if (refChild == null)
			{
				return this.PrependChild(newChild);
			}
			if (newChild == refChild)
			{
				return newChild;
			}
			int num = -1;
			if (this._childnodes != null)
			{
				num = this._childnodes[refChild];
			}
			if (num == -1)
			{
				throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
			}
			if (this._childnodes != null)
			{
				this._childnodes.Insert(num + 1, newChild);
			}
			this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
			this.SetChildNodesId(newChild);
			this.SetChanged();
			return newChild;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000A090 File Offset: 0x00008290
		public HtmlNode InsertBefore(HtmlNode newChild, HtmlNode refChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			if (refChild == null)
			{
				return this.AppendChild(newChild);
			}
			if (newChild == refChild)
			{
				return newChild;
			}
			int num = -1;
			if (this._childnodes != null)
			{
				num = this._childnodes[refChild];
			}
			if (num == -1)
			{
				throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
			}
			if (this._childnodes != null)
			{
				this._childnodes.Insert(num, newChild);
			}
			this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
			this.SetChildNodesId(newChild);
			this.SetChanged();
			return newChild;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000A117 File Offset: 0x00008317
		public HtmlNode PrependChild(HtmlNode newChild)
		{
			if (newChild == null)
			{
				throw new ArgumentNullException("newChild");
			}
			this.ChildNodes.Prepend(newChild);
			this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
			this.SetChildNodesId(newChild);
			this.SetChanged();
			return newChild;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000A154 File Offset: 0x00008354
		public void PrependChildren(HtmlNodeCollection newChildren)
		{
			if (newChildren == null)
			{
				throw new ArgumentNullException("newChildren");
			}
			for (int i = newChildren.Count - 1; i >= 0; i--)
			{
				this.PrependChild(newChildren[i]);
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000A190 File Offset: 0x00008390
		public void Remove()
		{
			if (this.ParentNode != null)
			{
				this.ParentNode.ChildNodes.Remove(this);
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000A1AC File Offset: 0x000083AC
		public void RemoveAll()
		{
			this.RemoveAllChildren();
			if (this.HasAttributes)
			{
				this._attributes.Clear();
			}
			if (this._endnode != null && this._endnode != this && this._endnode._attributes != null)
			{
				this._endnode._attributes.Clear();
			}
			this.SetChanged();
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000A208 File Offset: 0x00008408
		public void RemoveAllChildren()
		{
			if (!this.HasChildNodes)
			{
				return;
			}
			if (this._ownerdocument.OptionUseIdAttribute)
			{
				foreach (HtmlNode htmlNode in this._childnodes)
				{
					this._ownerdocument.SetIdForNode(null, htmlNode.GetId());
					this.RemoveAllIDforNode(htmlNode);
				}
			}
			this._childnodes.Clear();
			this.SetChanged();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000A294 File Offset: 0x00008494
		public void RemoveAllIDforNode(HtmlNode node)
		{
			foreach (HtmlNode htmlNode in node.ChildNodes)
			{
				this._ownerdocument.SetIdForNode(null, htmlNode.GetId());
				this.RemoveAllIDforNode(htmlNode);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000A2FC File Offset: 0x000084FC
		public void MoveChild(HtmlNode child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("Oops! the 'child' parameter cannot be null.");
			}
			HtmlNode parentNode = child.ParentNode;
			this.AppendChild(child);
			if (parentNode != null)
			{
				parentNode.RemoveChild(child);
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000A334 File Offset: 0x00008534
		public void MoveChildren(HtmlNodeCollection children)
		{
			if (children == null)
			{
				throw new ArgumentNullException("Oops! the 'children' parameter cannot be null.");
			}
			HtmlNode parentNode = children.ParentNode;
			this.AppendChildren(children);
			if (parentNode != null)
			{
				parentNode.RemoveChildren(children);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000A368 File Offset: 0x00008568
		public void RemoveChildren(HtmlNodeCollection oldChildren)
		{
			if (oldChildren == null)
			{
				throw new ArgumentNullException("Oops! the 'oldChildren' parameter cannot be null.");
			}
			foreach (HtmlNode oldChild in Enumerable.ToList<HtmlNode>(oldChildren))
			{
				this.RemoveChild(oldChild);
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000A3CC File Offset: 0x000085CC
		public HtmlNode RemoveChild(HtmlNode oldChild)
		{
			if (oldChild == null)
			{
				throw new ArgumentNullException("oldChild");
			}
			int num = -1;
			if (this._childnodes != null)
			{
				num = this._childnodes[oldChild];
			}
			if (num == -1)
			{
				throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
			}
			if (this._childnodes != null)
			{
				this._childnodes.Remove(num);
			}
			this._ownerdocument.SetIdForNode(null, oldChild.GetId());
			this.RemoveAllIDforNode(oldChild);
			this.SetChanged();
			return oldChild;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000A444 File Offset: 0x00008644
		public HtmlNode RemoveChild(HtmlNode oldChild, bool keepGrandChildren)
		{
			if (oldChild == null)
			{
				throw new ArgumentNullException("oldChild");
			}
			if (oldChild._childnodes != null && keepGrandChildren)
			{
				HtmlNode refChild = oldChild.PreviousSibling;
				foreach (HtmlNode newChild in oldChild._childnodes)
				{
					refChild = this.InsertAfter(newChild, refChild);
				}
			}
			this.RemoveChild(oldChild);
			this.SetChanged();
			return oldChild;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000A4CC File Offset: 0x000086CC
		public HtmlNode ReplaceChild(HtmlNode newChild, HtmlNode oldChild)
		{
			if (newChild == null)
			{
				return this.RemoveChild(oldChild);
			}
			if (oldChild == null)
			{
				return this.AppendChild(newChild);
			}
			int num = -1;
			if (this._childnodes != null)
			{
				num = this._childnodes[oldChild];
			}
			if (num == -1)
			{
				throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
			}
			if (this._childnodes != null)
			{
				this._childnodes.Replace(num, newChild);
			}
			this._ownerdocument.SetIdForNode(null, oldChild.GetId());
			this.RemoveAllIDforNode(oldChild);
			this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
			this.SetChildNodesId(newChild);
			this.SetChanged();
			return newChild;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000A564 File Offset: 0x00008764
		public HtmlAttribute SetAttributeValue(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlAttribute htmlAttribute = this.Attributes[name];
			if (htmlAttribute == null)
			{
				return this.Attributes.Append(this._ownerdocument.CreateAttribute(name, value));
			}
			htmlAttribute.Value = value;
			return htmlAttribute;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000A5B0 File Offset: 0x000087B0
		public void WriteContentTo(TextWriter outText, int level = 0)
		{
			if (level > HtmlDocument.MaxDepthLevel)
			{
				throw new ArgumentException("The document is too complex to parse");
			}
			if (this._childnodes == null)
			{
				return;
			}
			foreach (HtmlNode htmlNode in this._childnodes)
			{
				htmlNode.WriteTo(outText, level + 1);
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000A620 File Offset: 0x00008820
		public string WriteContentTo()
		{
			StringWriter stringWriter = new StringWriter();
			this.WriteContentTo(stringWriter, 0);
			stringWriter.Flush();
			return stringWriter.ToString();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000A648 File Offset: 0x00008848
		public virtual void WriteTo(TextWriter outText, int level = 0)
		{
			switch (this._nodetype)
			{
			case HtmlNodeType.Document:
				if (this._ownerdocument.OptionOutputAsXml)
				{
					outText.Write("<?xml version=\"1.0\" encoding=\"" + this._ownerdocument.GetOutEncoding().BodyName + "\"?>");
					if (this._ownerdocument.DocumentNode.HasChildNodes)
					{
						int num = this._ownerdocument.DocumentNode._childnodes.Count;
						if (num > 0)
						{
							if (this._ownerdocument.GetXmlDeclaration() != null)
							{
								num--;
							}
							if (num > 1)
							{
								if (!this._ownerdocument.BackwardCompatibility)
								{
									this.WriteContentTo(outText, level);
									return;
								}
								if (this._ownerdocument.OptionOutputUpperCase)
								{
									outText.Write("<SPAN>");
									this.WriteContentTo(outText, level);
									outText.Write("</SPAN>");
									return;
								}
								outText.Write("<span>");
								this.WriteContentTo(outText, level);
								outText.Write("</span>");
								return;
							}
						}
					}
				}
				this.WriteContentTo(outText, level);
				return;
			case HtmlNodeType.Element:
			{
				string text = this._ownerdocument.OptionOutputUpperCase ? this.Name.ToUpperInvariant() : this.Name;
				if (this._ownerdocument.OptionOutputOriginalCase)
				{
					text = this.OriginalName;
				}
				if (this._ownerdocument.OptionOutputAsXml)
				{
					if (text.Length <= 0 || text.get_Chars(0) == '?' || text.Trim().Length == 0)
					{
						return;
					}
					text = HtmlDocument.GetXmlName(text, false, this._ownerdocument.OptionPreserveXmlNamespaces);
				}
				outText.Write("<");
				outText.Write(text);
				this.WriteAttributes(outText, false);
				if (this.HasChildNodes)
				{
					outText.Write(">");
					bool flag = false;
					if (this._ownerdocument.OptionOutputAsXml && HtmlNode.IsCDataElement(this.Name))
					{
						flag = true;
						outText.Write("\r\n//<![CDATA[\r\n");
					}
					if (flag)
					{
						if (this.HasChildNodes)
						{
							this.ChildNodes[0].WriteTo(outText, level);
						}
						outText.Write("\r\n//]]>//\r\n");
					}
					else
					{
						this.WriteContentTo(outText, level);
					}
					if (this._ownerdocument.OptionOutputAsXml || !this._isImplicitEnd)
					{
						outText.Write("</");
						outText.Write(text);
						if (!this._ownerdocument.OptionOutputAsXml)
						{
							this.WriteAttributes(outText, true);
						}
						outText.Write(">");
						return;
					}
				}
				else if (HtmlNode.IsEmptyElement(this.Name))
				{
					if (!this._ownerdocument.OptionWriteEmptyNodes && !this._ownerdocument.OptionOutputAsXml)
					{
						if (this.Name.Length > 0 && this.Name.get_Chars(0) == '?')
						{
							outText.Write("?");
						}
						outText.Write(">");
						return;
					}
					if (this._ownerdocument.OptionWriteEmptyNodesWithoutSpace)
					{
						outText.Write("/>");
						return;
					}
					outText.Write(" />");
					return;
				}
				else
				{
					if (!this._isImplicitEnd)
					{
						outText.Write("></");
						outText.Write(text);
						outText.Write(">");
						return;
					}
					outText.Write(">");
				}
				return;
			}
			case HtmlNodeType.Comment:
			{
				string text2 = ((HtmlCommentNode)this).Comment;
				if (!this._ownerdocument.OptionOutputAsXml)
				{
					outText.Write(text2);
					return;
				}
				HtmlCommentNode htmlCommentNode = (HtmlCommentNode)this;
				if (!this._ownerdocument.BackwardCompatibility && htmlCommentNode.Comment.StartsWith("<!doctype", 5))
				{
					outText.Write(htmlCommentNode.Comment);
					return;
				}
				if (this.OwnerDocument.OptionXmlForceOriginalComment)
				{
					outText.Write(htmlCommentNode.Comment);
					return;
				}
				outText.Write("<!--" + HtmlNode.GetXmlComment(htmlCommentNode) + "-->");
				return;
			}
			case HtmlNodeType.Text:
			{
				string text2 = ((HtmlTextNode)this).Text;
				outText.Write(this._ownerdocument.OptionOutputAsXml ? HtmlDocument.HtmlEncodeWithCompatibility(text2, this._ownerdocument.BackwardCompatibility) : text2);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000AA28 File Offset: 0x00008C28
		public void WriteTo(XmlWriter writer)
		{
			switch (this._nodetype)
			{
			case HtmlNodeType.Document:
				writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"" + this._ownerdocument.GetOutEncoding().BodyName + "\"");
				if (!this.HasChildNodes)
				{
					return;
				}
				using (List<HtmlNode>.Enumerator enumerator = this.ChildNodes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						HtmlNode htmlNode = enumerator.Current;
						htmlNode.WriteTo(writer);
					}
					return;
				}
				break;
			case HtmlNodeType.Element:
			{
				string text = this._ownerdocument.OptionOutputUpperCase ? this.Name.ToUpperInvariant() : this.Name;
				if (this._ownerdocument.OptionOutputOriginalCase)
				{
					text = this.OriginalName;
				}
				writer.WriteStartElement(text);
				HtmlNode.WriteAttributes(writer, this);
				if (this.HasChildNodes)
				{
					foreach (HtmlNode htmlNode2 in this.ChildNodes)
					{
						htmlNode2.WriteTo(writer);
					}
				}
				writer.WriteEndElement();
				return;
			}
			case HtmlNodeType.Comment:
				writer.WriteComment(HtmlNode.GetXmlComment((HtmlCommentNode)this));
				return;
			case HtmlNodeType.Text:
				break;
			default:
				return;
			}
			string text2 = ((HtmlTextNode)this).Text;
			writer.WriteString(text2);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000AB88 File Offset: 0x00008D88
		public string WriteTo()
		{
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				this.WriteTo(stringWriter, 0);
				stringWriter.Flush();
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000ABD0 File Offset: 0x00008DD0
		public void SetParent(HtmlNode parent)
		{
			if (parent == null)
			{
				return;
			}
			this.ParentNode = parent;
			if (this.OwnerDocument.OptionMaxNestedChildNodes > 0)
			{
				this.Depth = parent.Depth + 1;
				if (this.Depth > this.OwnerDocument.OptionMaxNestedChildNodes)
				{
					throw new Exception(string.Format("Document has more than {0} nested tags. This is likely due to the page not closing tags properly.", this.OwnerDocument.OptionMaxNestedChildNodes));
				}
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000AC37 File Offset: 0x00008E37
		internal void SetChanged()
		{
			this._changed = true;
			if (this.ParentNode != null)
			{
				this.ParentNode.SetChanged();
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000AC53 File Offset: 0x00008E53
		private void UpdateHtml()
		{
			this._innerhtml = this.WriteContentTo();
			this._outerhtml = this.WriteTo();
			this._changed = false;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000AC74 File Offset: 0x00008E74
		internal static string GetXmlComment(HtmlCommentNode comment)
		{
			string comment2 = comment.Comment;
			return comment2.Substring(4, comment2.Length - 7).Replace("--", " - -");
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000ACA8 File Offset: 0x00008EA8
		internal static void WriteAttributes(XmlWriter writer, HtmlNode node)
		{
			if (!node.HasAttributes)
			{
				return;
			}
			foreach (HtmlAttribute htmlAttribute in node.Attributes.Hashitems.Values)
			{
				writer.WriteAttributeString(htmlAttribute.XmlName, htmlAttribute.Value);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000AD1C File Offset: 0x00008F1C
		internal void UpdateLastNode()
		{
			HtmlNode htmlNode = null;
			if (this._prevwithsamename == null || !this._prevwithsamename._starttag)
			{
				if (this._ownerdocument.Openednodes == null)
				{
					goto IL_EC;
				}
				using (Dictionary<int, HtmlNode>.Enumerator enumerator = this._ownerdocument.Openednodes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, HtmlNode> keyValuePair = enumerator.Current;
						if ((keyValuePair.Key < this._outerstartindex || keyValuePair.Key > this._outerstartindex + this._outerlength) && keyValuePair.Value._name == this._name)
						{
							if (htmlNode == null && keyValuePair.Value._starttag)
							{
								htmlNode = keyValuePair.Value;
							}
							else if (htmlNode != null && htmlNode.InnerStartIndex < keyValuePair.Key && keyValuePair.Value._starttag)
							{
								htmlNode = keyValuePair.Value;
							}
						}
					}
					goto IL_EC;
				}
			}
			htmlNode = this._prevwithsamename;
			IL_EC:
			if (htmlNode != null)
			{
				this._ownerdocument.Lastnodes[htmlNode.Name] = htmlNode;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000AE40 File Offset: 0x00009040
		internal void CloseNode(HtmlNode endnode, int level = 0)
		{
			if (level > HtmlDocument.MaxDepthLevel)
			{
				throw new ArgumentException("The document is too complex to parse");
			}
			if (!this._ownerdocument.OptionAutoCloseOnEnd && this._childnodes != null)
			{
				foreach (HtmlNode htmlNode in this._childnodes)
				{
					if (!htmlNode.Closed)
					{
						HtmlNode htmlNode2 = new HtmlNode(this.NodeType, this._ownerdocument, -1);
						htmlNode2._endnode = htmlNode2;
						htmlNode.CloseNode(htmlNode2, level + 1);
					}
				}
			}
			if (!this.Closed)
			{
				this._endnode = endnode;
				if (this._ownerdocument.Openednodes != null)
				{
					this._ownerdocument.Openednodes.Remove(this._outerstartindex);
				}
				if (Utilities.GetDictionaryValueOrDefault<string, HtmlNode>(this._ownerdocument.Lastnodes, this.Name, null) == this)
				{
					this._ownerdocument.Lastnodes.Remove(this.Name);
					this._ownerdocument.UpdateLastParentNode();
					if (this._starttag && !string.IsNullOrEmpty(this.Name))
					{
						this.UpdateLastNode();
					}
				}
				if (endnode == this)
				{
					return;
				}
				this._innerstartindex = this._outerstartindex + this._outerlength;
				this._innerlength = endnode._outerstartindex - this._innerstartindex;
				this._outerlength = endnode._outerstartindex + endnode._outerlength - this._outerstartindex;
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000AFB4 File Offset: 0x000091B4
		internal string GetId()
		{
			HtmlAttribute htmlAttribute = this.HasAttributes ? this.Attributes["id"] : null;
			if (htmlAttribute != null)
			{
				return htmlAttribute.Value;
			}
			return string.Empty;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000AFEC File Offset: 0x000091EC
		internal void SetId(string id)
		{
			HtmlAttribute htmlAttribute = this.Attributes["id"] ?? this._ownerdocument.CreateAttribute("id");
			htmlAttribute.Value = id;
			this._ownerdocument.SetIdForNode(this, htmlAttribute.Value);
			this.Attributes["id"] = htmlAttribute;
			this.SetChanged();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000B050 File Offset: 0x00009250
		internal void WriteAttribute(TextWriter outText, HtmlAttribute att)
		{
			if (att.Value == null)
			{
				return;
			}
			AttributeValueQuote attributeValueQuote = this.OwnerDocument.GlobalAttributeValueQuote ?? att.QuoteType;
			if (attributeValueQuote == AttributeValueQuote.Initial)
			{
				attributeValueQuote = att.QuoteType;
			}
			else if (attributeValueQuote == AttributeValueQuote.InitialExceptWithoutValue)
			{
				attributeValueQuote = ((att.QuoteType == AttributeValueQuote.WithoutValue) ? AttributeValueQuote.DoubleQuote : att.QuoteType);
			}
			bool flag = attributeValueQuote == AttributeValueQuote.WithoutValue;
			string text = (attributeValueQuote == AttributeValueQuote.DoubleQuote) ? "\"" : ((attributeValueQuote == AttributeValueQuote.SingleQuote) ? "'" : "");
			string text2;
			if (this._ownerdocument.OptionOutputAsXml)
			{
				if (attributeValueQuote != AttributeValueQuote.DoubleQuote && attributeValueQuote != AttributeValueQuote.SingleQuote)
				{
					AttributeValueQuote? globalAttributeValueQuote = this.OwnerDocument.GlobalAttributeValueQuote;
					AttributeValueQuote attributeValueQuote2 = AttributeValueQuote.SingleQuote;
					text = ((globalAttributeValueQuote.GetValueOrDefault() == attributeValueQuote2 & globalAttributeValueQuote != null) ? "'" : "\"");
				}
				text2 = (this._ownerdocument.OptionOutputUpperCase ? att.XmlName.ToUpperInvariant() : att.XmlName);
				if (this._ownerdocument.OptionOutputOriginalCase)
				{
					text2 = att.OriginalName;
				}
				outText.Write(" ");
				outText.Write(text2);
				outText.Write("=");
				outText.Write(text);
				outText.Write(HtmlDocument.HtmlEncodeWithCompatibility(att.XmlValue, this._ownerdocument.BackwardCompatibility));
				outText.Write(text);
				return;
			}
			text2 = (this._ownerdocument.OptionOutputUpperCase ? att.Name.ToUpperInvariant() : att.Name);
			if (this._ownerdocument.OptionOutputOriginalCase)
			{
				text2 = att.OriginalName;
			}
			if (att.Name.Length >= 4 && att.Name.get_Chars(0) == '<' && att.Name.get_Chars(1) == '%' && att.Name.get_Chars(att.Name.Length - 1) == '>' && att.Name.get_Chars(att.Name.Length - 2) == '%')
			{
				outText.Write(" ");
				outText.Write(text2);
				return;
			}
			if (flag)
			{
				outText.Write(" ");
				outText.Write(text2);
				return;
			}
			string text3 = (attributeValueQuote == AttributeValueQuote.DoubleQuote) ? ((!att.Value.StartsWith("@")) ? att.Value.Replace("\"", "&quot;") : att.Value) : ((attributeValueQuote == AttributeValueQuote.SingleQuote) ? att.Value.Replace("'", "&#39;") : att.Value);
			if (!this._ownerdocument.OptionOutputOptimizeAttributeValues)
			{
				outText.Write(" ");
				outText.Write(text2);
				outText.Write("=");
				outText.Write(text);
				outText.Write(text3);
				outText.Write(text);
				return;
			}
			if (att.Value.IndexOfAny(HtmlNode.optimizeAttributesCheckedChars) < 0)
			{
				outText.Write(" ");
				outText.Write(text2);
				outText.Write("=");
				outText.Write(att.Value);
				return;
			}
			outText.Write(" ");
			outText.Write(text2);
			outText.Write("=");
			outText.Write(text);
			outText.Write(text3);
			outText.Write(text);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000B368 File Offset: 0x00009568
		internal void WriteAttributes(TextWriter outText, bool closing)
		{
			if (!this._ownerdocument.OptionOutputAsXml)
			{
				if (!closing)
				{
					if (this._attributes != null)
					{
						foreach (HtmlAttribute att in this._attributes)
						{
							this.WriteAttribute(outText, att);
						}
					}
					if (!this._ownerdocument.OptionAddDebuggingAttributes)
					{
						return;
					}
					this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_closed", this.Closed.ToString()));
					this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_children", this.ChildNodes.Count.ToString()));
					int num = 0;
					using (List<HtmlNode>.Enumerator enumerator2 = this.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							HtmlNode htmlNode = enumerator2.Current;
							this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_child_" + num.ToString(), htmlNode.Name));
							num++;
						}
						return;
					}
				}
				if (this._endnode == null || this._endnode._attributes == null || this._endnode == this)
				{
					return;
				}
				foreach (HtmlAttribute att2 in this._endnode._attributes)
				{
					this.WriteAttribute(outText, att2);
				}
				if (!this._ownerdocument.OptionAddDebuggingAttributes)
				{
					return;
				}
				this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_closed", this.Closed.ToString()));
				this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_children", this.ChildNodes.Count.ToString()));
				return;
			}
			if (this._attributes == null)
			{
				return;
			}
			foreach (HtmlAttribute att3 in this._attributes.Hashitems.Values)
			{
				this.WriteAttribute(outText, att3);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000B5BC File Offset: 0x000097BC
		private string GetRelativeXpath()
		{
			if (this.ParentNode == null)
			{
				return this.Name;
			}
			if (this.NodeType == HtmlNodeType.Document)
			{
				return string.Empty;
			}
			int num = 1;
			foreach (HtmlNode htmlNode in this.ParentNode.ChildNodes)
			{
				if (!(htmlNode.Name != this.Name))
				{
					if (htmlNode == this)
					{
						break;
					}
					num++;
				}
			}
			return this.Name + "[" + num.ToString() + "]";
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000B668 File Offset: 0x00009868
		private bool IsSingleElementNode()
		{
			int num = 0;
			for (HtmlNode htmlNode = this.FirstChild; htmlNode != null; htmlNode = htmlNode.NextSibling)
			{
				if (htmlNode.NodeType == HtmlNodeType.Element && htmlNode.OuterHtml != "\r\n")
				{
					num++;
				}
			}
			return num <= 1;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000B6B0 File Offset: 0x000098B0
		public void AddClass(string name)
		{
			this.AddClass(name, false);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000B6BC File Offset: 0x000098BC
		public void AddClass(string name, bool throwError)
		{
			IEnumerable<HtmlAttribute> enumerable = this.Attributes.AttributesWithName("class");
			bool flag = true;
			foreach (HtmlAttribute htmlAttribute in enumerable)
			{
				flag = false;
				if (htmlAttribute.Value != null && Array.IndexOf<string>(htmlAttribute.Value.Split(HtmlNode.spaceSeparator), name) != -1)
				{
					if (throwError)
					{
						throw new Exception(HtmlDocument.HtmlExceptionClassExists);
					}
				}
				else
				{
					this.SetAttributeValue(htmlAttribute.Name, htmlAttribute.Value + " " + name);
				}
			}
			if (flag)
			{
				HtmlAttribute newAttribute = this._ownerdocument.CreateAttribute("class", name);
				this.Attributes.Append(newAttribute);
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000B780 File Offset: 0x00009980
		public void RemoveClass()
		{
			this.RemoveClass(false);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000B78C File Offset: 0x0000998C
		public void RemoveClass(bool throwError)
		{
			IEnumerable<HtmlAttribute> enumerable = this.Attributes.AttributesWithName("class");
			if (this.IsEmpty(enumerable) && throwError)
			{
				throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
			}
			foreach (HtmlAttribute attribute in enumerable)
			{
				this.Attributes.Remove(attribute);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000B800 File Offset: 0x00009A00
		public void RemoveClass(string name)
		{
			this.RemoveClass(name, false);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000B80C File Offset: 0x00009A0C
		public void RemoveClass(string name, bool throwError)
		{
			IEnumerable<HtmlAttribute> enumerable = this.Attributes.AttributesWithName("class");
			if (this.IsEmpty(enumerable) && throwError)
			{
				throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
			}
			foreach (HtmlAttribute htmlAttribute in enumerable)
			{
				if (htmlAttribute.Value != null)
				{
					if (htmlAttribute.Value.Equals(name))
					{
						this.Attributes.Remove(htmlAttribute);
					}
					else if (htmlAttribute.Value != null && Enumerable.Contains<string>(htmlAttribute.Value.Split(' ', 0), name))
					{
						string[] array = htmlAttribute.Value.Split(' ', 0);
						string text = "";
						foreach (string text2 in array)
						{
							if (!text2.Equals(name))
							{
								text = text + text2 + " ";
							}
						}
						text = text.Trim();
						this.SetAttributeValue(htmlAttribute.Name, text);
					}
					else if (throwError)
					{
						throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
					}
					if (string.IsNullOrEmpty(htmlAttribute.Value))
					{
						this.Attributes.Remove(htmlAttribute);
					}
				}
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000B948 File Offset: 0x00009B48
		public void ReplaceClass(string newClass, string oldClass)
		{
			this.ReplaceClass(newClass, oldClass, false);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000B954 File Offset: 0x00009B54
		public void ReplaceClass(string newClass, string oldClass, bool throwError)
		{
			if (string.IsNullOrEmpty(newClass))
			{
				this.RemoveClass(oldClass);
			}
			if (string.IsNullOrEmpty(oldClass))
			{
				this.AddClass(newClass);
			}
			IEnumerable<HtmlAttribute> enumerable = this.Attributes.AttributesWithName("class");
			if (this.IsEmpty(enumerable) && throwError)
			{
				throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
			}
			foreach (HtmlAttribute htmlAttribute in enumerable)
			{
				if (htmlAttribute.Value != null)
				{
					if (htmlAttribute.Value.Equals(oldClass) || htmlAttribute.Value.Contains(oldClass))
					{
						string value = htmlAttribute.Value.Replace(oldClass, newClass);
						this.SetAttributeValue(htmlAttribute.Name, value);
					}
					else if (throwError)
					{
						throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
					}
				}
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000BA2C File Offset: 0x00009C2C
		public IEnumerable<string> GetClasses()
		{
			HtmlNode.<GetClasses>d__198 <GetClasses>d__ = new HtmlNode.<GetClasses>d__198(-2);
			<GetClasses>d__.<>4__this = this;
			return <GetClasses>d__;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000BA3C File Offset: 0x00009C3C
		public bool HasClass(string className)
		{
			foreach (string text in this.GetClasses())
			{
				string[] array = text.Split(null, 1);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == className)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000BAAC File Offset: 0x00009CAC
		private bool IsEmpty(IEnumerable en)
		{
			using (IEnumerator enumerator = en.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000BAFC File Offset: 0x00009CFC
		public T GetEncapsulatedData<[Nullable(2)] T>()
		{
			return (T)((object)this.GetEncapsulatedData(typeof(T), null));
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000BB14 File Offset: 0x00009D14
		public T GetEncapsulatedData<[Nullable(2)] T>(HtmlDocument htmlDocument)
		{
			return (T)((object)this.GetEncapsulatedData(typeof(T), htmlDocument));
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000BB2C File Offset: 0x00009D2C
		public object GetEncapsulatedData(Type targetType, HtmlDocument htmlDocument = null)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("Parameter targetType is null");
			}
			HtmlDocument htmlDocument2;
			if (htmlDocument == null)
			{
				htmlDocument2 = this.OwnerDocument;
			}
			else
			{
				htmlDocument2 = htmlDocument;
			}
			if (!targetType.IsInstantiable())
			{
				throw new MissingMethodException("Parameterless Constructor excpected for " + targetType.FullName);
			}
			object obj = Activator.CreateInstance(targetType);
			if (!targetType.IsDefinedAttribute(typeof(HasXPathAttribute)))
			{
				throw new MissingXPathException("Type T must define HasXPath attribute and include properties with XPath attribute.");
			}
			IEnumerable<PropertyInfo> propertiesDefinedXPath = targetType.GetPropertiesDefinedXPath();
			if (propertiesDefinedXPath.CountOfIEnumerable<PropertyInfo>() == 0)
			{
				throw new MissingXPathException("Type " + targetType.FullName + " defined HasXPath Attribute but it does not have any property with XPath Attribte.");
			}
			foreach (PropertyInfo propertyInfo in propertiesDefinedXPath)
			{
				XPathAttribute xpathAttribute = propertyInfo.GetCustomAttributes(typeof(XPathAttribute), false)[0] as XPathAttribute;
				if (!propertyInfo.IsIEnumerable())
				{
					HtmlNode htmlNode = null;
					try
					{
						htmlNode = htmlDocument2.DocumentNode.SelectSingleNode(xpathAttribute.XPath);
					}
					catch (XPathException ex)
					{
						throw new XPathException(string.Concat(new string[]
						{
							ex.Message,
							" That means you have a syntax error in XPath property of this Property : ",
							propertyInfo.PropertyType.FullName,
							" ",
							propertyInfo.Name
						}));
					}
					catch (Exception inner)
					{
						throw new NodeNotFoundException("Cannot find node with giving XPath to bind to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name, inner);
					}
					if (htmlNode == null)
					{
						if (!propertyInfo.IsDefined(typeof(SkipNodeNotFoundAttribute), false))
						{
							throw new NodeNotFoundException("Cannot find node with giving XPath to bind to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
						}
					}
					else if (propertyInfo.PropertyType.IsDefinedAttribute(typeof(HasXPathAttribute)))
					{
						HtmlDocument htmlDocument3 = new HtmlDocument();
						htmlDocument3.LoadHtml(Tools.GetHtmlForEncapsulation(htmlNode, xpathAttribute.IsNodeReturnTypeExplicitlySet ? xpathAttribute.NodeReturnType : ReturnType.InnerHtml));
						object encapsulatedData = this.GetEncapsulatedData(propertyInfo.PropertyType, htmlDocument3);
						propertyInfo.SetValue(obj, encapsulatedData, null);
					}
					else
					{
						string text = string.Empty;
						if (xpathAttribute.AttributeName == null)
						{
							text = Tools.GetNodeValueBasedOnXPathReturnType<string>(htmlNode, xpathAttribute);
						}
						else
						{
							HtmlNode.ThrowIfNodeReturnTypeIsExplicitlySetWhenAttributeNameIsGiven(xpathAttribute);
							text = htmlNode.GetAttributeValue(xpathAttribute.AttributeName, null);
						}
						if (text == null)
						{
							throw new NodeAttributeNotFoundException(string.Concat(new string[]
							{
								"Can not find ",
								xpathAttribute.AttributeName,
								" Attribute in ",
								htmlNode.Name,
								" related to ",
								propertyInfo.PropertyType.FullName,
								" ",
								propertyInfo.Name
							}));
						}
						object obj2;
						try
						{
							obj2 = Convert.ChangeType(text, propertyInfo.PropertyType);
						}
						catch (FormatException)
						{
							throw new FormatException("Can not convert Invalid string to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
						}
						catch (Exception ex2)
						{
							throw new Exception("Unhandled Exception : " + ex2.Message);
						}
						propertyInfo.SetValue(obj, obj2, null);
					}
				}
				else
				{
					IList<Type> list = propertyInfo.GetGenericTypes() as IList<Type>;
					if (list == null || list.Count == 0)
					{
						throw new ArgumentException(propertyInfo.Name + " should have one generic argument.");
					}
					if (list.Count > 1)
					{
						throw new ArgumentException(propertyInfo.Name + " should have one generic argument.");
					}
					if (list.Count == 1)
					{
						HtmlNodeCollection htmlNodeCollection;
						try
						{
							htmlNodeCollection = htmlDocument2.DocumentNode.SelectNodes(xpathAttribute.XPath);
						}
						catch (XPathException ex3)
						{
							throw new XPathException(string.Concat(new string[]
							{
								ex3.Message,
								" That means you have a syntax error in XPath property of this Property : ",
								propertyInfo.PropertyType.FullName,
								" ",
								propertyInfo.Name
							}));
						}
						catch (Exception inner2)
						{
							throw new NodeNotFoundException("Cannot find node with giving XPath to bind to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name, inner2);
						}
						if (htmlNodeCollection != null && htmlNodeCollection.Count != 0)
						{
							IList list2 = list[0].CreateIListOfType();
							if (list[0].IsDefinedAttribute(typeof(HasXPathAttribute)))
							{
								using (List<HtmlNode>.Enumerator enumerator2 = htmlNodeCollection.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										HtmlNode node = enumerator2.Current;
										HtmlDocument htmlDocument4 = new HtmlDocument();
										htmlDocument4.LoadHtml(Tools.GetHtmlForEncapsulation(node, xpathAttribute.IsNodeReturnTypeExplicitlySet ? xpathAttribute.NodeReturnType : ReturnType.InnerHtml));
										object encapsulatedData2 = this.GetEncapsulatedData(list[0], htmlDocument4);
										list2.Add(encapsulatedData2);
									}
									goto IL_632;
								}
								goto IL_4C0;
							}
							goto IL_4C0;
							IL_632:
							if (list2 == null || list2.Count == 0)
							{
								throw new Exception(string.Concat(new string[]
								{
									"Cannot fill ",
									propertyInfo.PropertyType.FullName,
									" ",
									propertyInfo.Name,
									" because it is null."
								}));
							}
							propertyInfo.SetValue(obj, list2, null);
							continue;
							IL_4C0:
							if (xpathAttribute.AttributeName == null)
							{
								try
								{
									list2 = Tools.GetNodesValuesBasedOnXPathReturnType(htmlNodeCollection, xpathAttribute, list[0]);
									goto IL_632;
								}
								catch (FormatException)
								{
									throw new FormatException("Can not convert Invalid string in node collection to " + list[0].FullName + " " + propertyInfo.Name);
								}
								catch (Exception ex4)
								{
									throw new Exception("Unhandled Exception : " + ex4.Message);
								}
							}
							HtmlNode.ThrowIfNodeReturnTypeIsExplicitlySetWhenAttributeNameIsGiven(xpathAttribute);
							foreach (HtmlNode htmlNode2 in htmlNodeCollection)
							{
								string attributeValue = htmlNode2.GetAttributeValue(xpathAttribute.AttributeName, null);
								if (attributeValue == null)
								{
									throw new NodeAttributeNotFoundException(string.Concat(new string[]
									{
										"Can not find ",
										xpathAttribute.AttributeName,
										" Attribute in ",
										htmlNode2.Name,
										" related to ",
										propertyInfo.PropertyType.FullName,
										" ",
										propertyInfo.Name
									}));
								}
								object obj3;
								try
								{
									obj3 = Convert.ChangeType(attributeValue, list[0]);
								}
								catch (FormatException)
								{
									throw new FormatException("Can not convert Invalid string to " + list[0].FullName + " " + propertyInfo.Name);
								}
								catch (Exception ex5)
								{
									throw new Exception("Unhandled Exception : " + ex5.Message);
								}
								list2.Add(obj3);
							}
							goto IL_632;
						}
						if (!propertyInfo.IsDefined(typeof(SkipNodeNotFoundAttribute), false))
						{
							throw new NodeNotFoundException("Cannot find node with giving XPath to bind to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000C324 File Offset: 0x0000A524
		private static void ThrowIfNodeReturnTypeIsExplicitlySetWhenAttributeNameIsGiven(XPathAttribute xPathAttr)
		{
			if (xPathAttr.IsNodeReturnTypeExplicitlySet && !string.IsNullOrEmpty(xPathAttr.AttributeName))
			{
				throw new InvalidNodeReturnTypeException("Specifying a ReturnType value not allowed for XPathAttribute annotations targeting element attributes");
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000C346 File Offset: 0x0000A546
		public XPathNavigator CreateNavigator()
		{
			return new HtmlNodeNavigator(this.OwnerDocument, this);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000C354 File Offset: 0x0000A554
		public XPathNavigator CreateRootNavigator()
		{
			return new HtmlNodeNavigator(this.OwnerDocument, this.OwnerDocument.DocumentNode);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000C36C File Offset: 0x0000A56C
		public HtmlNodeCollection SelectNodes(string xpath)
		{
			HtmlNodeCollection htmlNodeCollection = new HtmlNodeCollection(null);
			XPathNodeIterator xpathNodeIterator = new HtmlNodeNavigator(this.OwnerDocument, this).Select(xpath);
			while (xpathNodeIterator.MoveNext())
			{
				XPathNavigator xpathNavigator = xpathNodeIterator.Current;
				HtmlNodeNavigator htmlNodeNavigator = (HtmlNodeNavigator)xpathNavigator;
				htmlNodeCollection.Add(htmlNodeNavigator.CurrentNode, false);
			}
			if (htmlNodeCollection.Count == 0 && !this.OwnerDocument.OptionEmptyCollection)
			{
				return null;
			}
			return htmlNodeCollection;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000C3D0 File Offset: 0x0000A5D0
		public HtmlNodeCollection SelectNodes(XPathExpression xpath)
		{
			HtmlNodeCollection htmlNodeCollection = new HtmlNodeCollection(null);
			XPathNodeIterator xpathNodeIterator = new HtmlNodeNavigator(this.OwnerDocument, this).Select(xpath);
			while (xpathNodeIterator.MoveNext())
			{
				XPathNavigator xpathNavigator = xpathNodeIterator.Current;
				HtmlNodeNavigator htmlNodeNavigator = (HtmlNodeNavigator)xpathNavigator;
				htmlNodeCollection.Add(htmlNodeNavigator.CurrentNode, false);
			}
			if (htmlNodeCollection.Count == 0 && !this.OwnerDocument.OptionEmptyCollection)
			{
				return null;
			}
			return htmlNodeCollection;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000C434 File Offset: 0x0000A634
		public HtmlNode SelectSingleNode(string xpath)
		{
			if (xpath == null)
			{
				throw new ArgumentNullException("xpath");
			}
			XPathNodeIterator xpathNodeIterator = new HtmlNodeNavigator(this.OwnerDocument, this).Select(xpath);
			if (!xpathNodeIterator.MoveNext())
			{
				return null;
			}
			return ((HtmlNodeNavigator)xpathNodeIterator.Current).CurrentNode;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000C47C File Offset: 0x0000A67C
		public HtmlNode SelectSingleNode(XPathExpression xpath)
		{
			if (xpath == null)
			{
				throw new ArgumentNullException("xpath");
			}
			XPathNodeIterator xpathNodeIterator = new HtmlNodeNavigator(this.OwnerDocument, this).Select(xpath);
			if (!xpathNodeIterator.MoveNext())
			{
				return null;
			}
			return ((HtmlNodeNavigator)xpathNodeIterator.Current).CurrentNode;
		}

		// Token: 0x04000069 RID: 105
		internal const string DepthLevelExceptionMessage = "The document is too complex to parse";

		// Token: 0x0400006A RID: 106
		internal HtmlAttributeCollection _attributes;

		// Token: 0x0400006B RID: 107
		internal HtmlNodeCollection _childnodes;

		// Token: 0x0400006C RID: 108
		internal HtmlNode _endnode;

		// Token: 0x0400006D RID: 109
		private bool _changed;

		// Token: 0x0400006E RID: 110
		internal string _innerhtml;

		// Token: 0x0400006F RID: 111
		internal int _innerlength;

		// Token: 0x04000070 RID: 112
		internal int _innerstartindex;

		// Token: 0x04000071 RID: 113
		internal int _line;

		// Token: 0x04000072 RID: 114
		internal int _lineposition;

		// Token: 0x04000073 RID: 115
		private string _name;

		// Token: 0x04000074 RID: 116
		internal int _namelength;

		// Token: 0x04000075 RID: 117
		internal int _namestartindex;

		// Token: 0x04000076 RID: 118
		internal HtmlNode _nextnode;

		// Token: 0x04000077 RID: 119
		internal HtmlNodeType _nodetype;

		// Token: 0x04000078 RID: 120
		internal string _outerhtml;

		// Token: 0x04000079 RID: 121
		internal int _outerlength;

		// Token: 0x0400007A RID: 122
		internal int _outerstartindex;

		// Token: 0x0400007B RID: 123
		private string _optimizedName;

		// Token: 0x0400007C RID: 124
		internal HtmlDocument _ownerdocument;

		// Token: 0x0400007D RID: 125
		internal HtmlNode _parentnode;

		// Token: 0x0400007E RID: 126
		internal HtmlNode _prevnode;

		// Token: 0x0400007F RID: 127
		internal HtmlNode _prevwithsamename;

		// Token: 0x04000080 RID: 128
		internal bool _starttag;

		// Token: 0x04000081 RID: 129
		internal int _streamposition;

		// Token: 0x04000082 RID: 130
		internal bool _isImplicitEnd;

		// Token: 0x04000083 RID: 131
		internal bool _isHideInnerText;

		// Token: 0x04000084 RID: 132
		public static readonly string HtmlNodeTypeNameComment = "#comment";

		// Token: 0x04000085 RID: 133
		public static readonly string HtmlNodeTypeNameDocument = "#document";

		// Token: 0x04000086 RID: 134
		public static readonly string HtmlNodeTypeNameText = "#text";

		// Token: 0x04000087 RID: 135
		public static Dictionary<string, HtmlElementFlag> ElementsFlags;

		// Token: 0x04000089 RID: 137
		private static readonly char[] optimizeAttributesCheckedChars = new char[]
		{
			'\n',
			'\r',
			'\t',
			' '
		};

		// Token: 0x0400008A RID: 138
		private static readonly char[] spaceSeparator = new char[]
		{
			' '
		};

		// Token: 0x02000033 RID: 51
		// (Invoke) Token: 0x0600028B RID: 651
		[NullableContext(0)]
		public delegate bool AttributeValueParser<[Nullable(2)] T>(string value, out T result);

		// Token: 0x02000034 RID: 52
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040000FF RID: 255
			[Nullable(0)]
			public static HtmlNode.AttributeValueParser<int> <0>__TryParse;

			// Token: 0x04000100 RID: 256
			[Nullable(0)]
			public static HtmlNode.AttributeValueParser<bool> <1>__TryParse;
		}
	}
}
