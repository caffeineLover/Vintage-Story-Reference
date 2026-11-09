using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000016 RID: 22
	[NullableContext(1)]
	[Nullable(0)]
	[AttributeUsage(128, Inherited = false, AllowMultiple = false)]
	public sealed class XPathAttribute : Attribute
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000C7CC File Offset: 0x0000A9CC
		// (set) Token: 0x06000175 RID: 373 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
		internal bool IsNodeReturnTypeExplicitlySet { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000C7DD File Offset: 0x0000A9DD
		public string XPath { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000C7E5 File Offset: 0x0000A9E5
		// (set) Token: 0x06000178 RID: 376 RVA: 0x0000C7ED File Offset: 0x0000A9ED
		public string AttributeName { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000C7F6 File Offset: 0x0000A9F6
		// (set) Token: 0x0600017A RID: 378 RVA: 0x0000C7FE File Offset: 0x0000A9FE
		public ReturnType NodeReturnType
		{
			get
			{
				return this._nodeReturnType;
			}
			set
			{
				this._nodeReturnType = value;
				this.IsNodeReturnTypeExplicitlySet = true;
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000C80E File Offset: 0x0000AA0E
		public XPathAttribute(string xpathString)
		{
			this.XPath = xpathString;
			this._nodeReturnType = ReturnType.InnerText;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000C824 File Offset: 0x0000AA24
		public XPathAttribute(string xpathString, ReturnType nodeReturnType)
		{
			this.XPath = xpathString;
			this.NodeReturnType = nodeReturnType;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000C83A File Offset: 0x0000AA3A
		public XPathAttribute(string xpathString, string attributeName)
		{
			this.XPath = xpathString;
			this.AttributeName = attributeName;
			this._nodeReturnType = ReturnType.InnerText;
		}

		// Token: 0x0400008F RID: 143
		private ReturnType _nodeReturnType;
	}
}
