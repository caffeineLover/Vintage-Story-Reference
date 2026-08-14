using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200001C RID: 28
	[NullableContext(1)]
	[Nullable(0)]
	public class HtmlNodeCollection : IList<HtmlNode>, ICollection<HtmlNode>, IEnumerable<HtmlNode>, IEnumerable
	{
		// Token: 0x0600018A RID: 394 RVA: 0x0000C8C3 File Offset: 0x0000AAC3
		public HtmlNodeCollection(HtmlNode parentnode)
		{
			this._parentnode = parentnode;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000C8DD File Offset: 0x0000AADD
		internal HtmlNode ParentNode
		{
			get
			{
				return this._parentnode;
			}
		}

		// Token: 0x1700004F RID: 79
		public int this[HtmlNode node]
		{
			get
			{
				int nodeIndex = this.GetNodeIndex(node);
				if (nodeIndex == -1)
				{
					throw new ArgumentOutOfRangeException("node", "Node \"" + node.CloneNode(false).OuterHtml + "\" was not found in the collection");
				}
				return nodeIndex;
			}
		}

		// Token: 0x17000050 RID: 80
		public HtmlNode this[string nodeName]
		{
			get
			{
				for (int i = 0; i < this._items.Count; i++)
				{
					if (string.Equals(this._items[i].Name, nodeName, 5))
					{
						return this._items[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0000C963 File Offset: 0x0000AB63
		public int Count
		{
			get
			{
				return this._items.Count;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0000C970 File Offset: 0x0000AB70
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000053 RID: 83
		public HtmlNode this[int index]
		{
			get
			{
				return this._items[index];
			}
			set
			{
				this._items[index] = value;
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000C990 File Offset: 0x0000AB90
		public void Add(HtmlNode node)
		{
			this.Add(node, true);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000C99A File Offset: 0x0000AB9A
		public void Add(HtmlNode node, bool setParent)
		{
			this._items.Add(node);
			if (setParent)
			{
				node.ParentNode = this._parentnode;
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
		public void Clear()
		{
			foreach (HtmlNode htmlNode in this._items)
			{
				htmlNode.ParentNode = null;
				htmlNode.NextSibling = null;
				htmlNode.PreviousSibling = null;
			}
			this._items.Clear();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000CA24 File Offset: 0x0000AC24
		public bool Contains(HtmlNode item)
		{
			return this._items.Contains(item);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000CA32 File Offset: 0x0000AC32
		public void CopyTo(HtmlNode[] array, int arrayIndex)
		{
			this._items.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000CA41 File Offset: 0x0000AC41
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		public List<HtmlNode>.Enumerator GetEnumerator()
		{
			return this._items.GetEnumerator();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000CA4E File Offset: 0x0000AC4E
		IEnumerator<HtmlNode> IEnumerable<HtmlNode>.GetEnumerator()
		{
			return this._items.GetEnumerator();
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000CA60 File Offset: 0x0000AC60
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._items.GetEnumerator();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000CA72 File Offset: 0x0000AC72
		public int IndexOf(HtmlNode item)
		{
			return this._items.IndexOf(item);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000CA80 File Offset: 0x0000AC80
		public void Insert(int index, HtmlNode node)
		{
			HtmlNode htmlNode = null;
			HtmlNode htmlNode2 = null;
			if (index > 0)
			{
				htmlNode2 = this._items[index - 1];
			}
			if (index < this._items.Count)
			{
				htmlNode = this._items[index];
			}
			this._items.Insert(index, node);
			if (htmlNode2 != null)
			{
				if (node == htmlNode2)
				{
					throw new InvalidProgramException("Unexpected error.");
				}
				htmlNode2._nextnode = node;
			}
			if (htmlNode != null)
			{
				htmlNode._prevnode = node;
			}
			node._prevnode = htmlNode2;
			if (htmlNode == node)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			node._nextnode = htmlNode;
			node.SetParent(this._parentnode);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000CB18 File Offset: 0x0000AD18
		public bool Remove(HtmlNode item)
		{
			int index = this._items.IndexOf(item);
			this.RemoveAt(index);
			return true;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		public void RemoveAt(int index)
		{
			HtmlNode htmlNode = null;
			HtmlNode htmlNode2 = null;
			HtmlNode htmlNode3 = this._items[index];
			HtmlNode htmlNode4 = this._parentnode ?? htmlNode3._parentnode;
			if (index > 0)
			{
				htmlNode2 = this._items[index - 1];
			}
			if (index < this._items.Count - 1)
			{
				htmlNode = this._items[index + 1];
			}
			this._items.RemoveAt(index);
			if (htmlNode2 != null)
			{
				if (htmlNode == htmlNode2)
				{
					throw new InvalidProgramException("Unexpected error.");
				}
				htmlNode2._nextnode = htmlNode;
			}
			if (htmlNode != null)
			{
				htmlNode._prevnode = htmlNode2;
			}
			htmlNode3._prevnode = null;
			htmlNode3._nextnode = null;
			htmlNode3._parentnode = null;
			if (htmlNode4 != null)
			{
				htmlNode4.SetChanged();
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000CBEC File Offset: 0x0000ADEC
		public static HtmlNode FindFirst(HtmlNodeCollection items, string name)
		{
			foreach (HtmlNode htmlNode in items)
			{
				if (htmlNode.Name.Equals(name, 5))
				{
					return htmlNode;
				}
				if (htmlNode.HasChildNodes)
				{
					HtmlNode htmlNode2 = HtmlNodeCollection.FindFirst(htmlNode.ChildNodes, name);
					if (htmlNode2 != null)
					{
						return htmlNode2;
					}
				}
			}
			return null;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000CC68 File Offset: 0x0000AE68
		public void Append(HtmlNode node)
		{
			HtmlNode htmlNode = null;
			if (this._items.Count > 0)
			{
				htmlNode = this._items[this._items.Count - 1];
			}
			this._items.Add(node);
			node._prevnode = htmlNode;
			node._nextnode = null;
			node.SetParent(this._parentnode);
			if (htmlNode == null)
			{
				return;
			}
			if (htmlNode == node)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			htmlNode._nextnode = node;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000CCDE File Offset: 0x0000AEDE
		public HtmlNode FindFirst(string name)
		{
			return HtmlNodeCollection.FindFirst(this, name);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
		public int GetNodeIndex(HtmlNode node)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				if (node == this._items[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000CD20 File Offset: 0x0000AF20
		public void Prepend(HtmlNode node)
		{
			HtmlNode htmlNode = null;
			if (this._items.Count > 0)
			{
				htmlNode = this._items[0];
			}
			this._items.Insert(0, node);
			if (node == htmlNode)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			node._nextnode = htmlNode;
			node._prevnode = null;
			node.SetParent(this._parentnode);
			if (htmlNode != null)
			{
				htmlNode._prevnode = node;
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000CD8A File Offset: 0x0000AF8A
		public bool Remove(int index)
		{
			this.RemoveAt(index);
			return true;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000CD94 File Offset: 0x0000AF94
		public void Replace(int index, HtmlNode node)
		{
			HtmlNode htmlNode = null;
			HtmlNode htmlNode2 = null;
			HtmlNode htmlNode3 = this._items[index];
			if (index > 0)
			{
				htmlNode2 = this._items[index - 1];
			}
			if (index < this._items.Count - 1)
			{
				htmlNode = this._items[index + 1];
			}
			this._items[index] = node;
			if (htmlNode2 != null)
			{
				if (node == htmlNode2)
				{
					throw new InvalidProgramException("Unexpected error.");
				}
				htmlNode2._nextnode = node;
			}
			if (htmlNode != null)
			{
				htmlNode._prevnode = node;
			}
			node._prevnode = htmlNode2;
			if (htmlNode == node)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			node._nextnode = htmlNode;
			node.SetParent(this._parentnode);
			htmlNode3._prevnode = null;
			htmlNode3._nextnode = null;
			htmlNode3._parentnode = null;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000CE50 File Offset: 0x0000B050
		public IEnumerable<HtmlNode> Descendants()
		{
			HtmlNodeCollection.<Descendants>d__35 <Descendants>d__ = new HtmlNodeCollection.<Descendants>d__35(-2);
			<Descendants>d__.<>4__this = this;
			return <Descendants>d__;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000CE60 File Offset: 0x0000B060
		public IEnumerable<HtmlNode> Descendants(string name)
		{
			HtmlNodeCollection.<Descendants>d__36 <Descendants>d__ = new HtmlNodeCollection.<Descendants>d__36(-2);
			<Descendants>d__.<>4__this = this;
			<Descendants>d__.<>3__name = name;
			return <Descendants>d__;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000CE77 File Offset: 0x0000B077
		public IEnumerable<HtmlNode> Elements()
		{
			HtmlNodeCollection.<Elements>d__37 <Elements>d__ = new HtmlNodeCollection.<Elements>d__37(-2);
			<Elements>d__.<>4__this = this;
			return <Elements>d__;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000CE87 File Offset: 0x0000B087
		public IEnumerable<HtmlNode> Elements(string name)
		{
			HtmlNodeCollection.<Elements>d__38 <Elements>d__ = new HtmlNodeCollection.<Elements>d__38(-2);
			<Elements>d__.<>4__this = this;
			<Elements>d__.<>3__name = name;
			return <Elements>d__;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000CE9E File Offset: 0x0000B09E
		public IEnumerable<HtmlNode> Nodes()
		{
			HtmlNodeCollection.<Nodes>d__39 <Nodes>d__ = new HtmlNodeCollection.<Nodes>d__39(-2);
			<Nodes>d__.<>4__this = this;
			return <Nodes>d__;
		}

		// Token: 0x04000093 RID: 147
		private readonly HtmlNode _parentnode;

		// Token: 0x04000094 RID: 148
		private readonly List<HtmlNode> _items = new List<HtmlNode>();
	}
}
