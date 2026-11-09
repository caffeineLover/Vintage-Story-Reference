using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	public class HtmlAttributeCollection : IList<HtmlAttribute>, ICollection<HtmlAttribute>, IEnumerable<HtmlAttribute>, IEnumerable
	{
		// Token: 0x0600002D RID: 45 RVA: 0x0000260C File Offset: 0x0000080C
		internal HtmlAttributeCollection(HtmlNode ownernode)
		{
			this._ownernode = ownernode;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002636 File Offset: 0x00000836
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002643 File Offset: 0x00000843
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000016 RID: 22
		public HtmlAttribute this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				HtmlAttribute htmlAttribute = this.items[index];
				this.items[index] = value;
				if (htmlAttribute.Name != value.Name)
				{
					this.Hashitems.Remove(htmlAttribute.Name);
				}
				this.Hashitems[value.Name] = value;
				value._ownernode = this._ownernode;
				this._ownernode.SetChanged();
			}
		}

		// Token: 0x17000017 RID: 23
		public HtmlAttribute this[string name]
		{
			get
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				HtmlAttribute result;
				if (!this.Hashitems.TryGetValue(name, ref result))
				{
					return null;
				}
				return result;
			}
			set
			{
				HtmlAttribute htmlAttribute;
				if (!this.Hashitems.TryGetValue(name, ref htmlAttribute))
				{
					this.Append(value);
					return;
				}
				this[this.items.IndexOf(htmlAttribute)] = value;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002735 File Offset: 0x00000935
		public void Add(string name, string value)
		{
			this.Append(name, value);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002740 File Offset: 0x00000940
		public void Add(HtmlAttribute item)
		{
			this.Append(item);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000274C File Offset: 0x0000094C
		public void AddRange(IEnumerable<HtmlAttribute> items)
		{
			foreach (HtmlAttribute newAttribute in items)
			{
				this.Append(newAttribute);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002798 File Offset: 0x00000998
		public void AddRange(Dictionary<string, string> items)
		{
			foreach (KeyValuePair<string, string> keyValuePair in items)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000027F4 File Offset: 0x000009F4
		void ICollection<HtmlAttribute>.Clear()
		{
			this.Clear();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000027FC File Offset: 0x000009FC
		public bool Contains(HtmlAttribute item)
		{
			return this.items.Contains(item);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000280A File Offset: 0x00000A0A
		public void CopyTo(HtmlAttribute[] array, int arrayIndex)
		{
			this.items.CopyTo(array, arrayIndex);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002819 File Offset: 0x00000A19
		IEnumerator<HtmlAttribute> IEnumerable<HtmlAttribute>.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000282B File Offset: 0x00000A2B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000283D File Offset: 0x00000A3D
		public int IndexOf(HtmlAttribute item)
		{
			return this.items.IndexOf(item);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000284C File Offset: 0x00000A4C
		public void Insert(int index, HtmlAttribute item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.Hashitems[item.Name] = item;
			item._ownernode = this._ownernode;
			this.items.Insert(index, item);
			this._ownernode.SetChanged();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000028A0 File Offset: 0x00000AA0
		bool ICollection<HtmlAttribute>.Remove(HtmlAttribute item)
		{
			if (item == null)
			{
				return false;
			}
			int attributeIndex = this.GetAttributeIndex(item);
			if (attributeIndex == -1)
			{
				return false;
			}
			this.RemoveAt(attributeIndex);
			return true;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000028C8 File Offset: 0x00000AC8
		public void RemoveAt(int index)
		{
			HtmlAttribute htmlAttribute = this.items[index];
			this.Hashitems.Remove(htmlAttribute.Name);
			this.items.RemoveAt(index);
			this._ownernode.SetChanged();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000290C File Offset: 0x00000B0C
		public HtmlAttribute Append(HtmlAttribute newAttribute)
		{
			if (this._ownernode.NodeType == HtmlNodeType.Text || this._ownernode.NodeType == HtmlNodeType.Comment)
			{
				throw new Exception("A Text or Comment node cannot have attributes.");
			}
			if (newAttribute == null)
			{
				throw new ArgumentNullException("newAttribute");
			}
			this.Hashitems[newAttribute.Name] = newAttribute;
			newAttribute._ownernode = this._ownernode;
			this.items.Add(newAttribute);
			this._ownernode.SetChanged();
			return newAttribute;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002984 File Offset: 0x00000B84
		public HtmlAttribute Append(string name)
		{
			HtmlAttribute newAttribute = this._ownernode._ownerdocument.CreateAttribute(name);
			return this.Append(newAttribute);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000029AC File Offset: 0x00000BAC
		public HtmlAttribute Append(string name, string value)
		{
			HtmlAttribute newAttribute = this._ownernode._ownerdocument.CreateAttribute(name, value);
			return this.Append(newAttribute);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000029D4 File Offset: 0x00000BD4
		public bool Contains(string name)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (string.Equals(this.items[i].Name, name, 5))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002A14 File Offset: 0x00000C14
		public HtmlAttribute Prepend(HtmlAttribute newAttribute)
		{
			this.Insert(0, newAttribute);
			return newAttribute;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002A20 File Offset: 0x00000C20
		public void Remove(HtmlAttribute attribute)
		{
			if (attribute == null)
			{
				throw new ArgumentNullException("attribute");
			}
			int attributeIndex = this.GetAttributeIndex(attribute);
			if (attributeIndex == -1)
			{
				throw new IndexOutOfRangeException();
			}
			this.RemoveAt(attributeIndex);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002A54 File Offset: 0x00000C54
		public void Remove(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			for (int i = this.items.Count - 1; i >= 0; i--)
			{
				if (string.Equals(this.items[i].Name, name, 5))
				{
					this.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002AA8 File Offset: 0x00000CA8
		public void RemoveAll()
		{
			this.Hashitems.Clear();
			this.items.Clear();
			this._ownernode.SetChanged();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002ACB File Offset: 0x00000CCB
		public IEnumerable<HtmlAttribute> AttributesWithName(string attributeName)
		{
			HtmlAttributeCollection.<AttributesWithName>d__35 <AttributesWithName>d__ = new HtmlAttributeCollection.<AttributesWithName>d__35(-2);
			<AttributesWithName>d__.<>4__this = this;
			<AttributesWithName>d__.<>3__attributeName = attributeName;
			return <AttributesWithName>d__;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002AE2 File Offset: 0x00000CE2
		public void Remove()
		{
			this.items.Clear();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002AEF File Offset: 0x00000CEF
		internal void Clear()
		{
			this.Hashitems.Clear();
			this.items.Clear();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002B08 File Offset: 0x00000D08
		internal int GetAttributeIndex(HtmlAttribute attribute)
		{
			if (attribute == null)
			{
				throw new ArgumentNullException("attribute");
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] == attribute)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002B4C File Offset: 0x00000D4C
		internal int GetAttributeIndex(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (string.Equals(this.items[i].Name, name, 5))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0400001C RID: 28
		internal Dictionary<string, HtmlAttribute> Hashitems = new Dictionary<string, HtmlAttribute>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400001D RID: 29
		private HtmlNode _ownernode;

		// Token: 0x0400001E RID: 30
		internal List<HtmlAttribute> items = new List<HtmlAttribute>();
	}
}
