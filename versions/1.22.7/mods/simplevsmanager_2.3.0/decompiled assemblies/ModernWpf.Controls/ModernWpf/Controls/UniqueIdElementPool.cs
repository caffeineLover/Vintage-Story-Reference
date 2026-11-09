using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000AA RID: 170
	internal class UniqueIdElementPool : IEnumerable<KeyValuePair<string, UIElement>>, IEnumerable
	{
		// Token: 0x06000844 RID: 2116 RVA: 0x0001C97C File Offset: 0x0001AB7C
		public UniqueIdElementPool(ItemsRepeater owner)
		{
			this.m_owner = owner;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001C998 File Offset: 0x0001AB98
		public void Add(UIElement element)
		{
			VirtualizationInfo virtualizationInfo = ItemsRepeater.GetVirtualizationInfo(element);
			string uniqueId = virtualizationInfo.UniqueId;
			if (this.m_elementMap.ContainsKey(uniqueId))
			{
				throw new Exception("The unique id provided (" + virtualizationInfo.UniqueId + ") is not unique.");
			}
			this.m_elementMap.Add(uniqueId, element);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001C9EC File Offset: 0x0001ABEC
		public UIElement Remove(int index)
		{
			string text = this.m_owner.ItemsSourceView.KeyFromIndex(index);
			UIElement result;
			if (this.m_elementMap.TryGetValue(text, ref result))
			{
				this.m_elementMap.Remove(text);
			}
			return result;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001CA29 File Offset: 0x0001AC29
		public void Clear()
		{
			this.m_elementMap.Clear();
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001CA36 File Offset: 0x0001AC36
		public IEnumerator<KeyValuePair<string, UIElement>> GetEnumerator()
		{
			return this.m_elementMap.GetEnumerator();
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001CA48 File Offset: 0x0001AC48
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000396 RID: 918
		private readonly ItemsRepeater m_owner;

		// Token: 0x04000397 RID: 919
		private readonly Dictionary<string, UIElement> m_elementMap = new Dictionary<string, UIElement>();
	}
}
