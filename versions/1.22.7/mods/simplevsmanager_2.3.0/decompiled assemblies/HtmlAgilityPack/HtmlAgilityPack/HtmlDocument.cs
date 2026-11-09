using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace HtmlAgilityPack
{
	// Token: 0x0200000E RID: 14
	[NullableContext(1)]
	[Nullable(0)]
	public class HtmlDocument : IXPathNavigable
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002F51 File Offset: 0x00001151
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00002F58 File Offset: 0x00001158
		public static bool DisableBehaviorTagP
		{
			get
			{
				return HtmlDocument._disableBehaviorTagP;
			}
			set
			{
				if (value)
				{
					if (HtmlNode.ElementsFlags.ContainsKey("p"))
					{
						HtmlNode.ElementsFlags.Remove("p");
					}
				}
				else if (!HtmlNode.ElementsFlags.ContainsKey("p"))
				{
					HtmlNode.ElementsFlags.Add("p", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
				}
				HtmlDocument._disableBehaviorTagP = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002FB2 File Offset: 0x000011B2
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002FB9 File Offset: 0x000011B9
		public static Action<HtmlDocument> DefaultBuilder { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002FC1 File Offset: 0x000011C1
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00002FC9 File Offset: 0x000011C9
		public Action<HtmlDocument> ParseExecuting { get; set; }

		// Token: 0x0600006A RID: 106 RVA: 0x00002FD4 File Offset: 0x000011D4
		public HtmlDocument()
		{
			if (HtmlDocument.DefaultBuilder != null)
			{
				HtmlDocument.DefaultBuilder.Invoke(this);
			}
			this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
			this.OptionDefaultStreamEncoding = Encoding.Default;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000304C File Offset: 0x0000124C
		public string ParsedText
		{
			get
			{
				return this.Text;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003054 File Offset: 0x00001254
		// (set) Token: 0x0600006D RID: 109 RVA: 0x0000305B File Offset: 0x0000125B
		public static int MaxDepthLevel
		{
			get
			{
				return HtmlDocument._maxDepthLevel;
			}
			set
			{
				HtmlDocument._maxDepthLevel = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003063 File Offset: 0x00001263
		public int CheckSum
		{
			get
			{
				if (this._crc32 != null)
				{
					return (int)this._crc32.CheckSum;
				}
				return 0;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006F RID: 111 RVA: 0x0000307A File Offset: 0x0000127A
		public Encoding DeclaredEncoding
		{
			get
			{
				return this._declaredencoding;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003082 File Offset: 0x00001282
		public HtmlNode DocumentNode
		{
			get
			{
				return this._documentnode;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000071 RID: 113 RVA: 0x0000308A File Offset: 0x0000128A
		public Encoding Encoding
		{
			get
			{
				return this.GetOutEncoding();
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003092 File Offset: 0x00001292
		public IEnumerable<HtmlParseError> ParseErrors
		{
			get
			{
				return this._parseerrors;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000073 RID: 115 RVA: 0x0000309A File Offset: 0x0000129A
		public string Remainder
		{
			get
			{
				return this._remainder;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000030A2 File Offset: 0x000012A2
		public int RemainderOffset
		{
			get
			{
				return this._remainderOffset;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000030AA File Offset: 0x000012AA
		public Encoding StreamEncoding
		{
			get
			{
				return this._streamencoding;
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000030B2 File Offset: 0x000012B2
		public static string GetXmlName(string name)
		{
			return HtmlDocument.GetXmlName(name, false, false);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000030BC File Offset: 0x000012BC
		public void UseAttributeOriginalName(string tagName)
		{
			foreach (HtmlNode htmlNode in this.DocumentNode.SelectNodes("//" + tagName))
			{
				foreach (HtmlAttribute htmlAttribute in htmlNode.Attributes)
				{
					htmlAttribute.UseOriginalName = true;
				}
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003150 File Offset: 0x00001350
		public static string GetXmlName(string name, bool isAttribute, bool preserveXmlNamespaces)
		{
			string text = string.Empty;
			bool flag = true;
			for (int i = 0; i < name.Length; i++)
			{
				if ((name.get_Chars(i) >= 'a' && name.get_Chars(i) <= 'z') || (name.get_Chars(i) >= 'A' && name.get_Chars(i) <= 'Z') || (name.get_Chars(i) >= '0' && name.get_Chars(i) <= '9') || ((isAttribute || preserveXmlNamespaces) && name.get_Chars(i) == ':') || name.get_Chars(i) == '_' || name.get_Chars(i) == '-' || name.get_Chars(i) == '.')
				{
					ReadOnlySpan<char> readOnlySpan = text;
					char c = name.get_Chars(i);
					text = readOnlySpan + new ReadOnlySpan<char>(ref c);
				}
				else
				{
					flag = false;
					byte[] bytes = Encoding.UTF8.GetBytes(new char[]
					{
						name.get_Chars(i)
					});
					for (int j = 0; j < bytes.Length; j++)
					{
						text += bytes[j].ToString("x2");
					}
					text += "_";
				}
			}
			if (flag)
			{
				return text;
			}
			return "_" + text;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003273 File Offset: 0x00001473
		public static string HtmlEncode(string html)
		{
			return HtmlDocument.HtmlEncodeWithCompatibility(html, true);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000327C File Offset: 0x0000147C
		internal static string HtmlEncodeWithCompatibility(string html, bool backwardCompatibility = true)
		{
			if (html == null)
			{
				throw new ArgumentNullException("html");
			}
			return (backwardCompatibility ? new Regex("&(?!(amp;)|(lt;)|(gt;)|(quot;))", 1) : new Regex("&(?!(amp;)|(lt;)|(gt;)|(quot;)|(nbsp;)|(reg;))", 1)).Replace(html, "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000032EA File Offset: 0x000014EA
		public static bool IsWhiteSpace(int c)
		{
			return c == 10 || c == 13 || c == 32 || c == 9;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003303 File Offset: 0x00001503
		public HtmlAttribute CreateAttribute(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlAttribute htmlAttribute = this.CreateAttribute();
			htmlAttribute.Name = name;
			return htmlAttribute;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003320 File Offset: 0x00001520
		public HtmlAttribute CreateAttribute(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlAttribute htmlAttribute = this.CreateAttribute(name);
			htmlAttribute.Value = value;
			return htmlAttribute;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000333E File Offset: 0x0000153E
		public HtmlCommentNode CreateComment()
		{
			return (HtmlCommentNode)this.CreateNode(HtmlNodeType.Comment);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000334C File Offset: 0x0000154C
		public HtmlCommentNode CreateComment(string comment)
		{
			if (comment == null)
			{
				throw new ArgumentNullException("comment");
			}
			if (!comment.StartsWith("<!--") && !comment.EndsWith("-->"))
			{
				comment = "<!--" + comment + "-->";
			}
			HtmlCommentNode htmlCommentNode = this.CreateComment();
			htmlCommentNode.Comment = comment;
			return htmlCommentNode;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000033A0 File Offset: 0x000015A0
		public HtmlNode CreateElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			HtmlNode htmlNode = this.CreateNode(HtmlNodeType.Element);
			htmlNode.SetName(name);
			return htmlNode;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000033BE File Offset: 0x000015BE
		public HtmlTextNode CreateTextNode()
		{
			return (HtmlTextNode)this.CreateNode(HtmlNodeType.Text);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000033CC File Offset: 0x000015CC
		public HtmlTextNode CreateTextNode(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			HtmlTextNode htmlTextNode = this.CreateTextNode();
			htmlTextNode.Text = text;
			return htmlTextNode;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000033E9 File Offset: 0x000015E9
		public Encoding DetectEncoding(Stream stream)
		{
			return this.DetectEncoding(stream, false);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000033F3 File Offset: 0x000015F3
		public Encoding DetectEncoding(Stream stream, bool checkHtml)
		{
			this._useHtmlEncodingForStream = checkHtml;
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			return this.DetectEncoding(new StreamReader(stream));
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003418 File Offset: 0x00001618
		public Encoding DetectEncoding(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this._onlyDetectEncoding = true;
			if (this.OptionCheckSyntax)
			{
				this.Openednodes = new Dictionary<int, HtmlNode>();
			}
			else
			{
				this.Openednodes = null;
			}
			if (this.OptionUseIdAttribute)
			{
				this.Nodesid = new Dictionary<string, HtmlNode>(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				this.Nodesid = null;
			}
			StreamReader streamReader = reader as StreamReader;
			if (streamReader != null && !this._useHtmlEncodingForStream)
			{
				this.Text = streamReader.ReadToEnd();
				this._streamencoding = streamReader.CurrentEncoding;
				return this._streamencoding;
			}
			this._streamencoding = null;
			this._declaredencoding = null;
			this.Text = reader.ReadToEnd();
			this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
			try
			{
				this.Parse();
			}
			catch (EncodingFoundException ex)
			{
				return ex.Encoding;
			}
			return this._streamencoding;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000034F8 File Offset: 0x000016F8
		public Encoding DetectEncodingHtml(string html)
		{
			if (html == null)
			{
				throw new ArgumentNullException("html");
			}
			Encoding result;
			using (StringReader stringReader = new StringReader(html))
			{
				result = this.DetectEncoding(stringReader);
			}
			return result;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003540 File Offset: 0x00001740
		public HtmlNode GetElementbyId(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (this.Nodesid == null)
			{
				throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
			}
			HtmlNode result;
			if (!this.Nodesid.TryGetValue(id, ref result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003581 File Offset: 0x00001781
		public void Load(Stream stream)
		{
			this.Load(new StreamReader(stream, this.OptionDefaultStreamEncoding));
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003595 File Offset: 0x00001795
		public void Load(Stream stream, bool detectEncodingFromByteOrderMarks)
		{
			this.Load(new StreamReader(stream, detectEncodingFromByteOrderMarks));
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000035A4 File Offset: 0x000017A4
		public void Load(Stream stream, Encoding encoding)
		{
			this.Load(new StreamReader(stream, encoding));
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000035B3 File Offset: 0x000017B3
		public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			this.Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000035C3 File Offset: 0x000017C3
		public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			this.Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000035D8 File Offset: 0x000017D8
		public void Load(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this._onlyDetectEncoding = false;
			if (this.OptionCheckSyntax)
			{
				this.Openednodes = new Dictionary<int, HtmlNode>();
			}
			else
			{
				this.Openednodes = null;
			}
			if (this.OptionUseIdAttribute)
			{
				this.Nodesid = new Dictionary<string, HtmlNode>(StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				this.Nodesid = null;
			}
			StreamReader streamReader = reader as StreamReader;
			if (streamReader != null)
			{
				try
				{
					streamReader.Peek();
				}
				catch (Exception)
				{
				}
				this._streamencoding = streamReader.CurrentEncoding;
			}
			else
			{
				this._streamencoding = null;
			}
			this._declaredencoding = null;
			this.Text = reader.ReadToEnd();
			this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
			this.Parse();
			if (!this.OptionCheckSyntax || this.Openednodes == null)
			{
				return;
			}
			foreach (HtmlNode htmlNode in this.Openednodes.Values)
			{
				if (htmlNode._starttag)
				{
					string text;
					if (this.OptionExtractErrorSourceText)
					{
						text = htmlNode.OuterHtml;
						if (text.Length > this.OptionExtractErrorSourceTextMaxLength)
						{
							text = text.Substring(0, this.OptionExtractErrorSourceTextMaxLength);
						}
					}
					else
					{
						text = string.Empty;
					}
					this.AddError(HtmlParseErrorCode.TagNotClosed, htmlNode._line, htmlNode._lineposition, htmlNode._streamposition, text, "End tag </" + htmlNode.Name + "> was not found");
				}
			}
			this.Openednodes.Clear();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003760 File Offset: 0x00001960
		public void LoadHtml(string html)
		{
			if (html == null)
			{
				throw new ArgumentNullException("html");
			}
			using (StringReader stringReader = new StringReader(html))
			{
				this.Load(stringReader);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000037A8 File Offset: 0x000019A8
		public void Save(Stream outStream)
		{
			StreamWriter writer = new StreamWriter(outStream, this.GetOutEncoding());
			this.Save(writer);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000037CC File Offset: 0x000019CC
		public void Save(Stream outStream, Encoding encoding)
		{
			if (outStream == null)
			{
				throw new ArgumentNullException("outStream");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			StreamWriter writer = new StreamWriter(outStream, encoding);
			this.Save(writer);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003804 File Offset: 0x00001A04
		public void Save(StreamWriter writer)
		{
			this.Save(writer);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000380D File Offset: 0x00001A0D
		public void Save(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.DocumentNode.WriteTo(writer, 0);
			writer.Flush();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003830 File Offset: 0x00001A30
		public void Save(XmlWriter writer)
		{
			this.DocumentNode.WriteTo(writer);
			writer.Flush();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003844 File Offset: 0x00001A44
		internal HtmlAttribute CreateAttribute()
		{
			return new HtmlAttribute(this);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000384C File Offset: 0x00001A4C
		internal HtmlNode CreateNode(HtmlNodeType type)
		{
			return this.CreateNode(type, -1);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003856 File Offset: 0x00001A56
		internal HtmlNode CreateNode(HtmlNodeType type, int index)
		{
			if (type == HtmlNodeType.Comment)
			{
				return new HtmlCommentNode(this, index);
			}
			if (type != HtmlNodeType.Text)
			{
				return new HtmlNode(type, this, index);
			}
			return new HtmlTextNode(this, index);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000387A File Offset: 0x00001A7A
		internal Encoding GetOutEncoding()
		{
			Encoding result;
			if ((result = this._declaredencoding) == null)
			{
				result = (this._streamencoding ?? this.OptionDefaultStreamEncoding);
			}
			return result;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003898 File Offset: 0x00001A98
		internal HtmlNode GetXmlDeclaration()
		{
			if (!this._documentnode.HasChildNodes)
			{
				return null;
			}
			foreach (HtmlNode htmlNode in this._documentnode._childnodes)
			{
				if (htmlNode.Name == "?xml")
				{
					return htmlNode;
				}
			}
			return null;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003914 File Offset: 0x00001B14
		internal void SetIdForNode(HtmlNode node, string id)
		{
			if (!this.OptionUseIdAttribute)
			{
				return;
			}
			if (this.Nodesid == null || id == null)
			{
				return;
			}
			if (node == null)
			{
				this.Nodesid.Remove(id);
				return;
			}
			this.Nodesid[id] = node;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000394C File Offset: 0x00001B4C
		internal void UpdateLastParentNode()
		{
			do
			{
				if (this._lastparentnode.Closed)
				{
					this._lastparentnode = this._lastparentnode.ParentNode;
				}
			}
			while (this._lastparentnode != null && this._lastparentnode.Closed);
			if (this._lastparentnode == null)
			{
				this._lastparentnode = this._documentnode;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000039A0 File Offset: 0x00001BA0
		private void AddError(HtmlParseErrorCode code, int line, int linePosition, int streamPosition, string sourceText, string reason)
		{
			HtmlParseError htmlParseError = new HtmlParseError(code, line, linePosition, streamPosition, sourceText, reason);
			this._parseerrors.Add(htmlParseError);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000039C8 File Offset: 0x00001BC8
		private void CloseCurrentNode()
		{
			if (this._currentnode.Closed)
			{
				return;
			}
			bool flag = false;
			HtmlNode dictionaryValueOrDefault = Utilities.GetDictionaryValueOrDefault<string, HtmlNode>(this.Lastnodes, this._currentnode.Name, null);
			if (dictionaryValueOrDefault == null)
			{
				if (HtmlNode.IsClosedElement(this._currentnode.Name))
				{
					this._currentnode.CloseNode(this._currentnode, 0);
					if (this._lastparentnode != null)
					{
						HtmlNode htmlNode = null;
						Stack<HtmlNode> stack = new Stack<HtmlNode>();
						if (!this._currentnode.Name.Equals("br"))
						{
							for (HtmlNode htmlNode2 = this._lastparentnode.LastChild; htmlNode2 != null; htmlNode2 = htmlNode2.PreviousSibling)
							{
								if (htmlNode2.Name == this._currentnode.Name && !htmlNode2.HasChildNodes)
								{
									htmlNode = htmlNode2;
									break;
								}
								stack.Push(htmlNode2);
							}
						}
						if (htmlNode != null)
						{
							while (stack.Count != 0)
							{
								HtmlNode htmlNode3 = stack.Pop();
								this._lastparentnode.RemoveChild(htmlNode3);
								htmlNode.AppendChild(htmlNode3);
							}
						}
						else
						{
							this._lastparentnode.AppendChild(this._currentnode);
						}
					}
				}
				else if (HtmlNode.CanOverlapElement(this._currentnode.Name))
				{
					HtmlNode htmlNode4 = this.CreateNode(HtmlNodeType.Text, this._currentnode._outerstartindex);
					htmlNode4._outerlength = this._currentnode._outerlength;
					((HtmlTextNode)htmlNode4).Text = ((HtmlTextNode)htmlNode4).Text.ToLowerInvariant();
					if (this._lastparentnode != null)
					{
						this._lastparentnode.AppendChild(htmlNode4);
					}
				}
				else if (HtmlNode.IsEmptyElement(this._currentnode.Name))
				{
					this.AddError(HtmlParseErrorCode.EndTagNotRequired, this._currentnode._line, this._currentnode._lineposition, this._currentnode._streamposition, this._currentnode.OuterHtml, "End tag </" + this._currentnode.Name + "> is not required");
				}
				else
				{
					this.AddError(HtmlParseErrorCode.TagNotOpened, this._currentnode._line, this._currentnode._lineposition, this._currentnode._streamposition, this._currentnode.OuterHtml, "Start tag <" + this._currentnode.Name + "> was not found");
					flag = true;
				}
			}
			else
			{
				if (this.OptionFixNestedTags && this.FindResetterNodes(dictionaryValueOrDefault, this.GetResetters(this._currentnode.Name)))
				{
					this.AddError(HtmlParseErrorCode.EndTagInvalidHere, this._currentnode._line, this._currentnode._lineposition, this._currentnode._streamposition, this._currentnode.OuterHtml, "End tag </" + this._currentnode.Name + "> invalid here");
					flag = true;
				}
				if (!flag)
				{
					this.Lastnodes[this._currentnode.Name] = dictionaryValueOrDefault._prevwithsamename;
					dictionaryValueOrDefault.CloseNode(this._currentnode, 0);
				}
			}
			if (!flag && this._lastparentnode != null && (!HtmlNode.IsClosedElement(this._currentnode.Name) || this._currentnode._starttag))
			{
				this.UpdateLastParentNode();
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003CDF File Offset: 0x00001EDF
		private string CurrentNodeName()
		{
			return this.Text.Substring(this._currentnode._namestartindex, this._currentnode._namelength);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003D04 File Offset: 0x00001F04
		private void DecrementPosition()
		{
			this._index--;
			if (this._lineposition == 0)
			{
				this._lineposition = this._maxlineposition;
				this._line--;
				return;
			}
			this._lineposition--;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003D50 File Offset: 0x00001F50
		private HtmlNode FindResetterNode(HtmlNode node, string name)
		{
			HtmlNode dictionaryValueOrDefault = Utilities.GetDictionaryValueOrDefault<string, HtmlNode>(this.Lastnodes, name, null);
			if (dictionaryValueOrDefault == null)
			{
				return null;
			}
			if (dictionaryValueOrDefault.Closed)
			{
				return null;
			}
			if (dictionaryValueOrDefault._streamposition < node._streamposition)
			{
				return null;
			}
			return dictionaryValueOrDefault;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003D8C File Offset: 0x00001F8C
		private bool FindResetterNodes(HtmlNode node, string[] names)
		{
			if (names == null)
			{
				return false;
			}
			for (int i = 0; i < names.Length; i++)
			{
				if (this.FindResetterNode(node, names[i]) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003DBC File Offset: 0x00001FBC
		private void FixNestedTag(string name, string[] resetters)
		{
			if (resetters == null)
			{
				return;
			}
			HtmlNode dictionaryValueOrDefault = Utilities.GetDictionaryValueOrDefault<string, HtmlNode>(this.Lastnodes, this._currentnode.Name, null);
			if (dictionaryValueOrDefault == null || this.Lastnodes[name].Closed)
			{
				return;
			}
			if (this.FindResetterNodes(dictionaryValueOrDefault, resetters))
			{
				return;
			}
			HtmlNode htmlNode = new HtmlNode(dictionaryValueOrDefault.NodeType, this, -1);
			htmlNode._endnode = htmlNode;
			dictionaryValueOrDefault.CloseNode(htmlNode, 0);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003E24 File Offset: 0x00002024
		private void FixNestedTags()
		{
			if (!this._currentnode._starttag)
			{
				return;
			}
			string name = this.CurrentNodeName();
			this.FixNestedTag(name, this.GetResetters(name));
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003E54 File Offset: 0x00002054
		private string[] GetResetters(string name)
		{
			string[] result;
			if (!HtmlDocument.HtmlResetters.TryGetValue(name, ref result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003E74 File Offset: 0x00002074
		private void IncrementPosition()
		{
			if (this._crc32 != null)
			{
				this._crc32.AddToCRC32(this._c);
			}
			this._index++;
			this._maxlineposition = this._lineposition;
			if (this._c == 10)
			{
				this._lineposition = 0;
				this._line++;
				return;
			}
			this._lineposition++;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003EE4 File Offset: 0x000020E4
		private bool IsValidTag()
		{
			return this._c == 60 && this._index < this.Text.Length && (char.IsLetter(this.Text.get_Chars(this._index)) || this.Text.get_Chars(this._index) == '/' || this.Text.get_Chars(this._index) == '?' || this.Text.get_Chars(this._index) == '!' || this.Text.get_Chars(this._index) == '%');
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003F84 File Offset: 0x00002184
		private bool NewCheck()
		{
			if (this._c != 60 || !this.IsValidTag())
			{
				return false;
			}
			if (this._index < this.Text.Length && this.Text.get_Chars(this._index) == '%')
			{
				if (this.DisableServerSideCode)
				{
					return false;
				}
				HtmlDocument.ParseState state = this._state;
				if (state != HtmlDocument.ParseState.WhichTag)
				{
					if (state != HtmlDocument.ParseState.BetweenAttributes)
					{
						if (state == HtmlDocument.ParseState.AttributeAfterEquals)
						{
							this.PushAttributeValueStart(this._index - 1);
						}
					}
					else
					{
						this.PushAttributeNameStart(this._index - 1, this._lineposition - 1);
					}
				}
				else
				{
					this.PushNodeNameStart(true, this._index - 1);
					this._state = HtmlDocument.ParseState.Tag;
				}
				this._oldstate = this._state;
				this._state = HtmlDocument.ParseState.ServerSideCode;
				return true;
			}
			else
			{
				if (!this.PushNodeEnd(this._index - 1, true))
				{
					this._index = this.Text.Length;
					return true;
				}
				this._state = HtmlDocument.ParseState.WhichTag;
				if (this._index - 1 > this.Text.Length - 2 || (this.Text.get_Chars(this._index) != '!' && this.Text.get_Chars(this._index) != '?'))
				{
					this.PushNodeStart(HtmlNodeType.Element, this._index - 1, this._lineposition - 1);
					return true;
				}
				if (this.OptionTreatCDataBlockAsComment && this.Text.Substring(this._index).StartsWith("![CDATA[", 5))
				{
					this.PushNodeStart(HtmlNodeType.Comment, this._index - 1, this._lineposition - 1);
					this.PushNodeNameStart(true, this._index);
					this.PushNodeNameEnd(this._index + 1);
					this._state = HtmlDocument.ParseState.PcDataComment;
					return true;
				}
				this.PushNodeStart(HtmlNodeType.Comment, this._index - 1, this._lineposition - 1);
				this.PushNodeNameStart(true, this._index);
				this.PushNodeNameEnd(this._index + 1);
				this._state = HtmlDocument.ParseState.Comment;
				if (this._index < this.Text.Length - 2)
				{
					if (this.Text.get_Chars(this._index + 1) == '-' && this.Text.get_Chars(this._index + 2) == '-')
					{
						this._fullcomment = true;
					}
					else
					{
						this._fullcomment = false;
					}
				}
				return true;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000041BC File Offset: 0x000023BC
		private void Parse()
		{
			if (this.ParseExecuting != null)
			{
				this.ParseExecuting.Invoke(this);
			}
			int num = 0;
			if (this.OptionComputeChecksum)
			{
				this._crc32 = new Crc32();
			}
			this.Lastnodes = new Dictionary<string, HtmlNode>();
			this._c = 0;
			this._fullcomment = false;
			this._parseerrors = new List<HtmlParseError>();
			this._line = 1;
			this._lineposition = 0;
			this._maxlineposition = 0;
			this._state = HtmlDocument.ParseState.Text;
			this._oldstate = this._state;
			this._documentnode._innerlength = this.Text.Length;
			this._documentnode._outerlength = this.Text.Length;
			this._remainderOffset = this.Text.Length;
			this._lastparentnode = this._documentnode;
			this._currentnode = this.CreateNode(HtmlNodeType.Text, 0);
			this._currentattribute = null;
			this._index = 0;
			this.PushNodeStart(HtmlNodeType.Text, 0, this._lineposition);
			while (this._index < this.Text.Length)
			{
				this._c = (int)this.Text.get_Chars(this._index);
				this.IncrementPosition();
				switch (this._state)
				{
				case HtmlDocument.ParseState.Text:
					if (this.NewCheck())
					{
					}
					break;
				case HtmlDocument.ParseState.WhichTag:
					if (!this.NewCheck())
					{
						if (this._c == 47)
						{
							this.PushNodeNameStart(false, this._index);
						}
						else
						{
							this.PushNodeNameStart(true, this._index - 1);
							this.DecrementPosition();
						}
						this._state = HtmlDocument.ParseState.Tag;
					}
					break;
				case HtmlDocument.ParseState.Tag:
					if (!this.NewCheck())
					{
						if (HtmlDocument.IsWhiteSpace(this._c))
						{
							this.CloseParentImplicitExplicitNode();
							this.PushNodeNameEnd(this._index - 1);
							if (this._state == HtmlDocument.ParseState.Tag)
							{
								this._state = HtmlDocument.ParseState.BetweenAttributes;
							}
						}
						else if (this._c == 47)
						{
							this.CloseParentImplicitExplicitNode();
							this.PushNodeNameEnd(this._index - 1);
							if (this._state == HtmlDocument.ParseState.Tag)
							{
								this._state = HtmlDocument.ParseState.EmptyTag;
							}
						}
						else if (this._c == 62)
						{
							this.CloseParentImplicitExplicitNode();
							this.PushNodeNameEnd(this._index - 1);
							if (this._state == HtmlDocument.ParseState.Tag)
							{
								if (!this.PushNodeEnd(this._index, false))
								{
									this._index = this.Text.Length;
								}
								else if (this._state == HtmlDocument.ParseState.Tag)
								{
									this._state = HtmlDocument.ParseState.Text;
									this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
								}
							}
						}
					}
					break;
				case HtmlDocument.ParseState.BetweenAttributes:
					if (!this.NewCheck() && !HtmlDocument.IsWhiteSpace(this._c))
					{
						if (this._c == 47 || this._c == 63)
						{
							this._state = HtmlDocument.ParseState.EmptyTag;
						}
						else if (this._c == 62)
						{
							if (!this.PushNodeEnd(this._index, false))
							{
								this._index = this.Text.Length;
							}
							else if (this._state == HtmlDocument.ParseState.BetweenAttributes)
							{
								this._state = HtmlDocument.ParseState.Text;
								this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
							}
						}
						else
						{
							this.PushAttributeNameStart(this._index - 1, this._lineposition - 1);
							this._state = HtmlDocument.ParseState.AttributeName;
						}
					}
					break;
				case HtmlDocument.ParseState.EmptyTag:
					if (!this.NewCheck())
					{
						if (this._c == 62)
						{
							if (!this.PushNodeEnd(this._index, true))
							{
								this._index = this.Text.Length;
							}
							else if (this._state == HtmlDocument.ParseState.EmptyTag)
							{
								this._state = HtmlDocument.ParseState.Text;
								this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
							}
						}
						else if (!HtmlDocument.IsWhiteSpace(this._c))
						{
							this.DecrementPosition();
							this._state = HtmlDocument.ParseState.BetweenAttributes;
						}
						else
						{
							this._state = HtmlDocument.ParseState.BetweenAttributes;
						}
					}
					break;
				case HtmlDocument.ParseState.AttributeName:
					if (!this.NewCheck())
					{
						if (this._c == 47)
						{
							this.PushAttributeNameEnd(this._index - 1);
							this._state = HtmlDocument.ParseState.AttributeBeforeEquals;
						}
						else if (HtmlDocument.IsWhiteSpace(this._c))
						{
							this.PushAttributeNameEnd(this._index - 1);
							this._state = HtmlDocument.ParseState.AttributeBeforeEquals;
						}
						else if (this._c == 61)
						{
							this.PushAttributeNameEnd(this._index - 1);
							this._state = HtmlDocument.ParseState.AttributeAfterEquals;
						}
						else if (this._c == 62)
						{
							this.PushAttributeNameEnd(this._index - 1);
							if (!this.PushNodeEnd(this._index, false))
							{
								this._index = this.Text.Length;
							}
							else if (this._state == HtmlDocument.ParseState.AttributeName)
							{
								this._state = HtmlDocument.ParseState.Text;
								this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
							}
						}
					}
					break;
				case HtmlDocument.ParseState.AttributeBeforeEquals:
					if (!this.NewCheck() && !HtmlDocument.IsWhiteSpace(this._c))
					{
						if (this._c == 62)
						{
							if (!this.PushNodeEnd(this._index, false))
							{
								this._index = this.Text.Length;
							}
							else if (this._state == HtmlDocument.ParseState.AttributeBeforeEquals)
							{
								this._state = HtmlDocument.ParseState.Text;
								this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
							}
						}
						else if (this._c == 61)
						{
							this._state = HtmlDocument.ParseState.AttributeAfterEquals;
						}
						else
						{
							this._state = HtmlDocument.ParseState.BetweenAttributes;
							this.DecrementPosition();
						}
					}
					break;
				case HtmlDocument.ParseState.AttributeAfterEquals:
					if (!this.NewCheck() && !HtmlDocument.IsWhiteSpace(this._c))
					{
						if (this._c == 39 || this._c == 34)
						{
							this._state = HtmlDocument.ParseState.QuotedAttributeValue;
							this.PushAttributeValueStart(this._index, this._c);
							num = this._c;
						}
						else if (this._c == 62)
						{
							if (!this.PushNodeEnd(this._index, false))
							{
								this._index = this.Text.Length;
							}
							else if (this._state == HtmlDocument.ParseState.AttributeAfterEquals)
							{
								this._state = HtmlDocument.ParseState.Text;
								this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
							}
						}
						else
						{
							this.PushAttributeValueStart(this._index - 1);
							this._state = HtmlDocument.ParseState.AttributeValue;
						}
					}
					break;
				case HtmlDocument.ParseState.AttributeValue:
					if (!this.NewCheck())
					{
						if (HtmlDocument.IsWhiteSpace(this._c))
						{
							this.PushAttributeValueEnd(this._index - 1);
							this._state = HtmlDocument.ParseState.BetweenAttributes;
						}
						else if (this._c == 62)
						{
							this.PushAttributeValueEnd(this._index - 1);
							if (!this.PushNodeEnd(this._index, false))
							{
								this._index = this.Text.Length;
							}
							else if (this._state == HtmlDocument.ParseState.AttributeValue)
							{
								this._state = HtmlDocument.ParseState.Text;
								this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
							}
						}
					}
					break;
				case HtmlDocument.ParseState.Comment:
					if (this._c == 62 && (!this._fullcomment || (this.Text.get_Chars(this._index - 2) == '-' && this.Text.get_Chars(this._index - 3) == '-') || (this.Text.get_Chars(this._index - 2) == '!' && this.Text.get_Chars(this._index - 3) == '-' && this.Text.get_Chars(this._index - 4) == '-')))
					{
						if (!this.PushNodeEnd(this._index, false))
						{
							this._index = this.Text.Length;
						}
						else
						{
							this._state = HtmlDocument.ParseState.Text;
							this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
						}
					}
					break;
				case HtmlDocument.ParseState.QuotedAttributeValue:
					if (this._c == num)
					{
						this.PushAttributeValueEnd(this._index - 1);
						this._state = HtmlDocument.ParseState.BetweenAttributes;
					}
					else if (this._c == 60 && this._index < this.Text.Length && this.Text.get_Chars(this._index) == '%')
					{
						this._oldstate = this._state;
						this._state = HtmlDocument.ParseState.ServerSideCode;
					}
					break;
				case HtmlDocument.ParseState.ServerSideCode:
					if (this._c == 37)
					{
						if (this._index < this.Text.Length && this.Text.get_Chars(this._index) == '>')
						{
							HtmlDocument.ParseState oldstate = this._oldstate;
							if (oldstate != HtmlDocument.ParseState.BetweenAttributes)
							{
								if (oldstate == HtmlDocument.ParseState.AttributeAfterEquals)
								{
									this._state = HtmlDocument.ParseState.AttributeValue;
								}
								else
								{
									this._state = this._oldstate;
								}
							}
							else
							{
								this.PushAttributeNameEnd(this._index + 1);
								this._state = HtmlDocument.ParseState.BetweenAttributes;
							}
							this.IncrementPosition();
						}
					}
					else if (this._oldstate == HtmlDocument.ParseState.QuotedAttributeValue && this._c == num)
					{
						this._state = this._oldstate;
						this.DecrementPosition();
					}
					break;
				case HtmlDocument.ParseState.PcData:
					if (this._currentnode._namelength + 3 <= this.Text.Length - (this._index - 1) && (this.Text.get_Chars(this._index - 1) == '<' && this.Text.get_Chars(this._index) == '/' && string.Compare(this.Text, this._index + 1, this._currentnode.Name, 0, this._currentnode._namelength, 5) == 0))
					{
						int num2 = (int)this.Text.get_Chars(this._index - 1 + 2 + this._currentnode.Name.Length);
						if (num2 == 62 || HtmlDocument.IsWhiteSpace(num2))
						{
							HtmlNode htmlNode = this.CreateNode(HtmlNodeType.Text, this._currentnode._outerstartindex + this._currentnode._outerlength);
							htmlNode._outerlength = this._index - 1 - htmlNode._outerstartindex;
							htmlNode._streamposition = htmlNode._outerstartindex;
							htmlNode._line = this._currentnode.Line;
							htmlNode._lineposition = this._currentnode.LinePosition + this._currentnode._namelength + 2;
							this._currentnode.AppendChild(htmlNode);
							if (this._currentnode.Name.Equals("script", 5) || this._currentnode.Name.Equals("style", 5))
							{
								this._currentnode._isHideInnerText = true;
							}
							this.PushNodeStart(HtmlNodeType.Element, this._index - 1, this._lineposition - 1);
							this.PushNodeNameStart(false, this._index - 1 + 2);
							this._state = HtmlDocument.ParseState.Tag;
							this.IncrementPosition();
						}
					}
					break;
				case HtmlDocument.ParseState.PcDataComment:
					if (this._c == 62 && this._index < this.Text.Length && this.Text.get_Chars(this._index - 3) == ']' && this.Text.get_Chars(this._index - 2) == ']')
					{
						if (!this.PushNodeEnd(this._index, false))
						{
							this._index = this.Text.Length;
						}
						else
						{
							this._state = HtmlDocument.ParseState.Text;
							this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
						}
					}
					break;
				}
			}
			if (this._currentnode._namestartindex > 0)
			{
				this.PushNodeNameEnd(this._index);
			}
			this.PushNodeEnd(this._index, false);
			this.Lastnodes.Clear();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004D30 File Offset: 0x00002F30
		private void PushAttributeNameEnd(int index)
		{
			this._currentattribute._namelength = index - this._currentattribute._namestartindex;
			if (this._currentattribute.Name != null && !HtmlDocument.BlockAttributes.Contains(this._currentattribute.Name))
			{
				this._currentnode.Attributes.Append(this._currentattribute);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004D90 File Offset: 0x00002F90
		private void PushAttributeNameStart(int index, int lineposition)
		{
			this._currentattribute = this.CreateAttribute();
			this._currentattribute._namestartindex = index;
			this._currentattribute.Line = this._line;
			this._currentattribute._lineposition = lineposition;
			this._currentattribute._streamposition = index;
			this._currentattribute.InternalQuoteType = new AttributeValueQuote?(AttributeValueQuote.WithoutValue);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004DEF File Offset: 0x00002FEF
		private void PushAttributeValueEnd(int index)
		{
			this._currentattribute._valuelength = index - this._currentattribute._valuestartindex;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004E09 File Offset: 0x00003009
		private void PushAttributeValueStart(int index)
		{
			this.PushAttributeValueStart(index, 0);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004E14 File Offset: 0x00003014
		private void CloseParentImplicitExplicitNode()
		{
			bool flag = true;
			while (flag && !this._lastparentnode.Closed)
			{
				flag = false;
				bool flag2 = false;
				if (this.IsParentImplicitEnd())
				{
					if (this.OptionOutputAsXml || this.DisableImplicitEnd)
					{
						flag2 = true;
					}
					else
					{
						this.CloseParentImplicitEnd();
						flag = true;
					}
				}
				if (flag2 || this.IsParentExplicitEnd())
				{
					this.CloseParentExplicitEnd();
					flag = true;
				}
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004E70 File Offset: 0x00003070
		private bool IsParentImplicitEnd()
		{
			if (!this._currentnode._starttag)
			{
				return false;
			}
			bool result = false;
			string name = this._lastparentnode.Name;
			string text = this.Text.Substring(this._currentnode._namestartindex, this._index - this._currentnode._namestartindex - 1).ToLowerInvariant();
			if (!(name == "a"))
			{
				if (!(name == "dd"))
				{
					if (!(name == "dt"))
					{
						if (!(name == "li"))
						{
							if (!(name == "p"))
							{
								if (name == "option")
								{
									result = (text == "option");
								}
							}
							else if (HtmlDocument.DisableBehaviorTagP)
							{
								result = (text == "address" || text == "article" || text == "aside" || text == "blockquote" || text == "dir" || text == "div" || text == "dl" || text == "fieldset" || text == "footer" || text == "form" || text == "h1" || text == "h2" || text == "h3" || text == "h4" || text == "h5" || text == "h6" || text == "header" || text == "hr" || text == "li" || text == "menu" || text == "nav" || text == "ol" || text == "p" || text == "pre" || text == "section" || text == "table" || text == "ul");
							}
							else
							{
								result = (text == "p");
							}
						}
						else
						{
							result = (text == "li");
						}
					}
					else
					{
						result = (text == "dt" || text == "dd");
					}
				}
				else
				{
					result = (text == "dt" || text == "dd");
				}
			}
			else
			{
				result = (text == "a");
			}
			return result;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005140 File Offset: 0x00003340
		private bool IsParentExplicitEnd()
		{
			if (!this._currentnode._starttag)
			{
				return false;
			}
			bool result = false;
			string name = this._lastparentnode.Name;
			string text = this.Text.Substring(this._currentnode._namestartindex, this._index - this._currentnode._namestartindex - 1).ToLowerInvariant();
			if (name != null)
			{
				switch (name.Length)
				{
				case 1:
					if (name == "p")
					{
						result = (text == "div");
					}
					break;
				case 2:
				{
					char c = name.get_Chars(1);
					if (c <= 'd')
					{
						switch (c)
						{
						case '1':
							if (name == "h1")
							{
								result = (text == "h2" || text == "h3" || text == "h4" || text == "h5");
							}
							break;
						case '2':
							if (name == "h2")
							{
								result = (text == "h1" || text == "h3" || text == "h4" || text == "h5");
							}
							break;
						case '3':
							if (name == "h3")
							{
								result = (text == "h1" || text == "h2" || text == "h4" || text == "h5");
							}
							break;
						case '4':
							if (name == "h4")
							{
								result = (text == "h1" || text == "h2" || text == "h3" || text == "h5");
							}
							break;
						case '5':
							if (name == "h5")
							{
								result = (text == "h1" || text == "h2" || text == "h3" || text == "h4");
							}
							break;
						default:
							if (c == 'd')
							{
								if (name == "td")
								{
									result = (text == "td" || text == "th" || text == "tr" || text == "tbody" || text == "tfoot");
								}
							}
							break;
						}
					}
					else if (c != 'h')
					{
						if (c == 'r')
						{
							if (name == "tr")
							{
								result = (text == "tr" || text == "thead" || text == "tbody" || text == "tfoot");
							}
						}
					}
					else if (name == "th")
					{
						result = (text == "td" || text == "th" || text == "tr" || text == "tbody" || text == "tfoot");
					}
					break;
				}
				case 5:
				{
					char c = name.get_Chars(1);
					if (c <= 'b')
					{
						if (c != 'a')
						{
							if (c == 'b')
							{
								if (name == "tbody")
								{
									result = (text == "tbody" || text == "tfoot");
								}
							}
						}
						else if (name == "table")
						{
							result = (text == "table");
						}
					}
					else if (c != 'h')
					{
						if (c == 'i')
						{
							if (name == "title")
							{
								result = (text == "title");
							}
						}
					}
					else if (name == "thead")
					{
						result = (text == "tbody" || text == "tfoot");
					}
					break;
				}
				case 7:
					if (name == "caption")
					{
						result = (text == "tr" || text == "thead" || text == "tbody" || text == "tfoot" || text == "colgroup");
					}
					break;
				case 8:
					if (name == "colgroup")
					{
						result = (text == "tr" || text == "thead" || text == "tbody" || text == "tfoot");
					}
					break;
				}
			}
			return result;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005678 File Offset: 0x00003878
		private void CloseParentImplicitEnd()
		{
			HtmlNode htmlNode = new HtmlNode(this._lastparentnode.NodeType, this, -1);
			htmlNode._endnode = htmlNode;
			htmlNode._isImplicitEnd = true;
			this._lastparentnode._isImplicitEnd = true;
			this._lastparentnode.CloseNode(htmlNode, 0);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000056C0 File Offset: 0x000038C0
		private void CloseParentExplicitEnd()
		{
			HtmlNode htmlNode = new HtmlNode(this._lastparentnode.NodeType, this, -1);
			htmlNode._endnode = htmlNode;
			this._lastparentnode.CloseNode(htmlNode, 0);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000056F4 File Offset: 0x000038F4
		private void PushAttributeValueStart(int index, int quote)
		{
			this._currentattribute._valuestartindex = index;
			if (quote == 39)
			{
				this._currentattribute.InternalQuoteType = new AttributeValueQuote?(AttributeValueQuote.SingleQuote);
			}
			if (quote == 34)
			{
				this._currentattribute.InternalQuoteType = new AttributeValueQuote?(AttributeValueQuote.DoubleQuote);
			}
			if (quote == 0)
			{
				this._currentattribute.InternalQuoteType = new AttributeValueQuote?(AttributeValueQuote.None);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005750 File Offset: 0x00003950
		private bool PushNodeEnd(int index, bool close)
		{
			this._currentnode._outerlength = index - this._currentnode._outerstartindex;
			if (this._currentnode._nodetype == HtmlNodeType.Text || this._currentnode._nodetype == HtmlNodeType.Comment)
			{
				if (this._currentnode._outerlength > 0)
				{
					this._currentnode._innerlength = this._currentnode._outerlength;
					this._currentnode._innerstartindex = this._currentnode._outerstartindex;
					if (this._lastparentnode != null)
					{
						this._lastparentnode.AppendChild(this._currentnode);
					}
				}
			}
			else if (this._currentnode._starttag && this._lastparentnode != this._currentnode)
			{
				if (this._lastparentnode != null)
				{
					this._lastparentnode.AppendChild(this._currentnode);
				}
				this.ReadDocumentEncoding(this._currentnode);
				HtmlNode dictionaryValueOrDefault = Utilities.GetDictionaryValueOrDefault<string, HtmlNode>(this.Lastnodes, this._currentnode.Name, null);
				this._currentnode._prevwithsamename = dictionaryValueOrDefault;
				this.Lastnodes[this._currentnode.Name] = this._currentnode;
				if (this._currentnode.NodeType == HtmlNodeType.Document || this._currentnode.NodeType == HtmlNodeType.Element)
				{
					this._lastparentnode = this._currentnode;
				}
				if (HtmlNode.IsCDataElement(this.CurrentNodeName()))
				{
					this._state = HtmlDocument.ParseState.PcData;
					return true;
				}
				if (HtmlNode.IsClosedElement(this._currentnode.Name) || HtmlNode.IsEmptyElement(this._currentnode.Name))
				{
					close = true;
				}
			}
			if (close || !this._currentnode._starttag)
			{
				if (this.OptionStopperNodeName != null && this._remainder == null && string.Equals(this._currentnode.Name, this.OptionStopperNodeName, 5))
				{
					this._remainderOffset = index;
					this._remainder = this.Text.Substring(this._remainderOffset);
					this.CloseCurrentNode();
					return false;
				}
				this.CloseCurrentNode();
			}
			return true;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005942 File Offset: 0x00003B42
		private void PushNodeNameEnd(int index)
		{
			this._currentnode._namelength = index - this._currentnode._namestartindex;
			if (this.OptionFixNestedTags)
			{
				this.FixNestedTags();
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000596A File Offset: 0x00003B6A
		private void PushNodeNameStart(bool starttag, int index)
		{
			this._currentnode._starttag = starttag;
			this._currentnode._namestartindex = index;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005984 File Offset: 0x00003B84
		private void PushNodeStart(HtmlNodeType type, int index, int lineposition)
		{
			this._currentnode = this.CreateNode(type, index);
			this._currentnode._line = this._line;
			this._currentnode._lineposition = lineposition;
			this._currentnode._streamposition = index;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000059C0 File Offset: 0x00003BC0
		private void ReadDocumentEncoding(HtmlNode node)
		{
			if (!this.OptionReadEncoding)
			{
				return;
			}
			if (node._namelength != 4)
			{
				return;
			}
			if (node.Name != "meta")
			{
				return;
			}
			string text = null;
			HtmlAttribute htmlAttribute = node.Attributes["http-equiv"];
			if (htmlAttribute != null)
			{
				if (!string.Equals(htmlAttribute.Value, "content-type", 5))
				{
					return;
				}
				HtmlAttribute htmlAttribute2 = node.Attributes["content"];
				if (htmlAttribute2 != null)
				{
					text = NameValuePairList.GetNameValuePairsValue(htmlAttribute2.Value, "charset");
				}
			}
			else
			{
				htmlAttribute = node.Attributes["charset"];
				if (htmlAttribute != null)
				{
					text = htmlAttribute.Value;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (string.Equals(text, "utf8", 5))
				{
					text = "utf-8";
				}
				try
				{
					this._declaredencoding = Encoding.GetEncoding(text);
				}
				catch (ArgumentException)
				{
					this._declaredencoding = null;
				}
				if (this._onlyDetectEncoding)
				{
					throw new EncodingFoundException(this._declaredencoding);
				}
				if (this._streamencoding != null && this._declaredencoding != null && this._declaredencoding.CodePage != this._streamencoding.CodePage)
				{
					this.AddError(HtmlParseErrorCode.CharsetMismatch, this._line, this._lineposition, this._index, node.OuterHtml, "Encoding mismatch between StreamEncoding: " + this._streamencoding.WebName + " and DeclaredEncoding: " + this._declaredencoding.WebName);
				}
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005B28 File Offset: 0x00003D28
		public void DetectEncodingAndLoad(string path)
		{
			this.DetectEncodingAndLoad(path, true);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005B34 File Offset: 0x00003D34
		public void DetectEncodingAndLoad(string path, bool detectEncoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			Encoding encoding;
			if (detectEncoding)
			{
				encoding = this.DetectEncoding(path);
			}
			else
			{
				encoding = null;
			}
			if (encoding == null)
			{
				this.Load(path);
				return;
			}
			this.Load(path, encoding);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005B74 File Offset: 0x00003D74
		public Encoding DetectEncoding(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			Encoding result;
			using (StreamReader streamReader = new StreamReader(path, this.OptionDefaultStreamEncoding))
			{
				result = this.DetectEncoding(streamReader);
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005BC4 File Offset: 0x00003DC4
		public void Load(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			using (StreamReader streamReader = new StreamReader(path, this.OptionDefaultStreamEncoding))
			{
				this.Load(streamReader);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005C10 File Offset: 0x00003E10
		public void Load(string path, bool detectEncodingFromByteOrderMarks)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			using (StreamReader streamReader = new StreamReader(path, detectEncodingFromByteOrderMarks))
			{
				this.Load(streamReader);
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005C58 File Offset: 0x00003E58
		public void Load(string path, Encoding encoding)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			using (StreamReader streamReader = new StreamReader(path, encoding))
			{
				this.Load(streamReader);
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005CAC File Offset: 0x00003EAC
		public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			using (StreamReader streamReader = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks))
			{
				this.Load(streamReader);
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005D04 File Offset: 0x00003F04
		public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			using (StreamReader streamReader = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks, buffersize))
			{
				this.Load(streamReader);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005D5C File Offset: 0x00003F5C
		public void Save(string filename)
		{
			using (StreamWriter streamWriter = new StreamWriter(filename, false, this.GetOutEncoding()))
			{
				this.Save(streamWriter);
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005D9C File Offset: 0x00003F9C
		public void Save(string filename, Encoding encoding)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			using (StreamWriter streamWriter = new StreamWriter(filename, false, encoding))
			{
				this.Save(streamWriter);
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005DF4 File Offset: 0x00003FF4
		public XPathNavigator CreateNavigator()
		{
			return new HtmlNodeNavigator(this, this._documentnode);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005E04 File Offset: 0x00004004
		// Note: this type is marked as 'beforefieldinit'.
		static HtmlDocument()
		{
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			dictionary.Add("li", new string[]
			{
				"ul",
				"ol"
			});
			dictionary.Add("tr", new string[]
			{
				"table"
			});
			dictionary.Add("th", new string[]
			{
				"tr",
				"table"
			});
			dictionary.Add("td", new string[]
			{
				"tr",
				"table"
			});
			HtmlDocument.HtmlResetters = dictionary;
			List<string> list = new List<string>();
			list.Add("\"");
			list.Add("'");
			HtmlDocument.BlockAttributes = list;
		}

		// Token: 0x04000021 RID: 33
		internal static bool _disableBehaviorTagP = true;

		// Token: 0x04000024 RID: 36
		private static int _maxDepthLevel = int.MaxValue;

		// Token: 0x04000025 RID: 37
		private int _c;

		// Token: 0x04000026 RID: 38
		private Crc32 _crc32;

		// Token: 0x04000027 RID: 39
		private HtmlAttribute _currentattribute;

		// Token: 0x04000028 RID: 40
		private HtmlNode _currentnode;

		// Token: 0x04000029 RID: 41
		private Encoding _declaredencoding;

		// Token: 0x0400002A RID: 42
		private HtmlNode _documentnode;

		// Token: 0x0400002B RID: 43
		private bool _fullcomment;

		// Token: 0x0400002C RID: 44
		private int _index;

		// Token: 0x0400002D RID: 45
		internal Dictionary<string, HtmlNode> Lastnodes = new Dictionary<string, HtmlNode>();

		// Token: 0x0400002E RID: 46
		private HtmlNode _lastparentnode;

		// Token: 0x0400002F RID: 47
		private int _line;

		// Token: 0x04000030 RID: 48
		private int _lineposition;

		// Token: 0x04000031 RID: 49
		private int _maxlineposition;

		// Token: 0x04000032 RID: 50
		internal Dictionary<string, HtmlNode> Nodesid;

		// Token: 0x04000033 RID: 51
		private HtmlDocument.ParseState _oldstate;

		// Token: 0x04000034 RID: 52
		private bool _onlyDetectEncoding;

		// Token: 0x04000035 RID: 53
		internal Dictionary<int, HtmlNode> Openednodes;

		// Token: 0x04000036 RID: 54
		private List<HtmlParseError> _parseerrors = new List<HtmlParseError>();

		// Token: 0x04000037 RID: 55
		private string _remainder;

		// Token: 0x04000038 RID: 56
		private int _remainderOffset;

		// Token: 0x04000039 RID: 57
		private HtmlDocument.ParseState _state;

		// Token: 0x0400003A RID: 58
		private Encoding _streamencoding;

		// Token: 0x0400003B RID: 59
		private bool _useHtmlEncodingForStream;

		// Token: 0x0400003C RID: 60
		public string Text;

		// Token: 0x0400003D RID: 61
		public bool BackwardCompatibility = true;

		// Token: 0x0400003E RID: 62
		public bool OptionAddDebuggingAttributes;

		// Token: 0x0400003F RID: 63
		public bool OptionAutoCloseOnEnd;

		// Token: 0x04000040 RID: 64
		public bool OptionCheckSyntax = true;

		// Token: 0x04000041 RID: 65
		public bool OptionComputeChecksum;

		// Token: 0x04000042 RID: 66
		public bool OptionEmptyCollection;

		// Token: 0x04000043 RID: 67
		public bool OptionTreatCDataBlockAsComment;

		// Token: 0x04000044 RID: 68
		public bool DisableServerSideCode;

		// Token: 0x04000045 RID: 69
		public Encoding OptionDefaultStreamEncoding;

		// Token: 0x04000046 RID: 70
		public bool OptionXmlForceOriginalComment;

		// Token: 0x04000047 RID: 71
		public bool OptionExtractErrorSourceText;

		// Token: 0x04000048 RID: 72
		public int OptionExtractErrorSourceTextMaxLength = 100;

		// Token: 0x04000049 RID: 73
		public bool OptionFixNestedTags;

		// Token: 0x0400004A RID: 74
		public bool OptionOutputAsXml;

		// Token: 0x0400004B RID: 75
		public bool DisableImplicitEnd;

		// Token: 0x0400004C RID: 76
		public bool OptionPreserveXmlNamespaces;

		// Token: 0x0400004D RID: 77
		public bool OptionOutputOptimizeAttributeValues;

		// Token: 0x0400004E RID: 78
		public AttributeValueQuote? GlobalAttributeValueQuote;

		// Token: 0x0400004F RID: 79
		public bool OptionOutputOriginalCase;

		// Token: 0x04000050 RID: 80
		public bool OptionOutputUpperCase;

		// Token: 0x04000051 RID: 81
		public bool OptionReadEncoding = true;

		// Token: 0x04000052 RID: 82
		public string OptionStopperNodeName;

		// Token: 0x04000053 RID: 83
		public bool OptionDefaultUseOriginalName;

		// Token: 0x04000054 RID: 84
		public bool OptionUseIdAttribute = true;

		// Token: 0x04000055 RID: 85
		public bool OptionWriteEmptyNodes;

		// Token: 0x04000056 RID: 86
		public bool OptionWriteEmptyNodesWithoutSpace;

		// Token: 0x04000057 RID: 87
		public int OptionMaxNestedChildNodes;

		// Token: 0x04000058 RID: 88
		public bool OptionEnableBreakLineForInnerText;

		// Token: 0x04000059 RID: 89
		internal static readonly string HtmlExceptionRefNotChild = "Reference node must be a child of this node";

		// Token: 0x0400005A RID: 90
		internal static readonly string HtmlExceptionUseIdAttributeFalse = "You need to set UseIdAttribute property to true to enable this feature";

		// Token: 0x0400005B RID: 91
		internal static readonly string HtmlExceptionClassDoesNotExist = "Class name doesn't exist";

		// Token: 0x0400005C RID: 92
		internal static readonly string HtmlExceptionClassExists = "Class name already exists";

		// Token: 0x0400005D RID: 93
		internal static readonly Dictionary<string, string[]> HtmlResetters;

		// Token: 0x0400005E RID: 94
		private static List<string> BlockAttributes;

		// Token: 0x02000031 RID: 49
		[NullableContext(0)]
		private enum ParseState
		{
			// Token: 0x040000EE RID: 238
			Text,
			// Token: 0x040000EF RID: 239
			WhichTag,
			// Token: 0x040000F0 RID: 240
			Tag,
			// Token: 0x040000F1 RID: 241
			BetweenAttributes,
			// Token: 0x040000F2 RID: 242
			EmptyTag,
			// Token: 0x040000F3 RID: 243
			AttributeName,
			// Token: 0x040000F4 RID: 244
			AttributeBeforeEquals,
			// Token: 0x040000F5 RID: 245
			AttributeAfterEquals,
			// Token: 0x040000F6 RID: 246
			AttributeValue,
			// Token: 0x040000F7 RID: 247
			Comment,
			// Token: 0x040000F8 RID: 248
			QuotedAttributeValue,
			// Token: 0x040000F9 RID: 249
			ServerSideCode,
			// Token: 0x040000FA RID: 250
			PcData,
			// Token: 0x040000FB RID: 251
			PcDataComment
		}
	}
}
