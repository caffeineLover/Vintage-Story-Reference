using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xaml;

namespace ModernWpf.Controls
{
	// Token: 0x020000A2 RID: 162
	public class RecyclePool
	{
		// Token: 0x06000802 RID: 2050 RVA: 0x0001C3C1 File Offset: 0x0001A5C1
		public void PutElement(UIElement element, string key)
		{
			this.PutElementCore(element, key, null);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001C3CC File Offset: 0x0001A5CC
		public void PutElement(UIElement element, string key, UIElement owner)
		{
			this.PutElementCore(element, key, owner);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001C3D7 File Offset: 0x0001A5D7
		public UIElement TryGetElement(string key)
		{
			return this.TryGetElementCore(key, null);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001C3E1 File Offset: 0x0001A5E1
		public UIElement TryGetElement(string key, UIElement owner)
		{
			return this.TryGetElementCore(key, owner);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001C3EC File Offset: 0x0001A5EC
		protected virtual void PutElementCore(UIElement element, string key, UIElement owner)
		{
			Panel owner2 = this.EnsureOwnerIsPanelOrNull(owner);
			RecyclePool.ElementInfo elementInfo = new RecyclePool.ElementInfo(element, owner2);
			List<RecyclePool.ElementInfo> list;
			if (this.m_elements.TryGetValue(key, ref list))
			{
				list.Add(elementInfo);
				return;
			}
			List<RecyclePool.ElementInfo> list2 = new List<RecyclePool.ElementInfo>();
			list2.Add(elementInfo);
			this.m_elements.Add(key, list2);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001C444 File Offset: 0x0001A644
		protected virtual UIElement TryGetElementCore(string key, UIElement owner)
		{
			List<RecyclePool.ElementInfo> list;
			if (this.m_elements.TryGetValue(key, ref list) && list.Count > 0)
			{
				RecyclePool.ElementInfo elementInfo = new RecyclePool.ElementInfo(null, null);
				UIElement winrtOwner = owner;
				int num = list.FindIndex((RecyclePool.ElementInfo elemInfo) => elemInfo.Owner == winrtOwner || elemInfo.Owner == null);
				if (num >= 0)
				{
					elementInfo = list[num];
					list.RemoveAt(num);
				}
				else
				{
					elementInfo = list.Last<RecyclePool.ElementInfo>();
					list.RemoveLast<RecyclePool.ElementInfo>();
				}
				Panel panel = this.EnsureOwnerIsPanelOrNull(winrtOwner);
				if (elementInfo.Owner != null && elementInfo.Owner != panel)
				{
					Panel owner2 = elementInfo.Owner;
					if (owner2 != null)
					{
						int num2 = owner2.Children.IndexOf(elementInfo.Element);
						if (num2 < 0)
						{
							throw new Exception("ItemsRepeater's child not found in its Children collection.");
						}
						owner2.Children.RemoveAt(num2);
					}
				}
				return elementInfo.Element;
			}
			return null;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001C525 File Offset: 0x0001A725
		internal static string GetReuseKey(UIElement element)
		{
			return (string)element.GetValue(RecyclePool.ReuseKeyProperty);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001C537 File Offset: 0x0001A737
		internal static void SetReuseKey(UIElement element, string value)
		{
			element.SetValue(RecyclePool.ReuseKeyProperty, value);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001C548 File Offset: 0x0001A748
		public static RecyclePool GetPoolInstance(DataTemplate dataTemplate)
		{
			RecyclePool result;
			AttachablePropertyServices.TryGetProperty<RecyclePool>(dataTemplate, RecyclePool.PoolInstanceProperty, ref result);
			return result;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001C564 File Offset: 0x0001A764
		public static void SetPoolInstance(DataTemplate dataTemplate, RecyclePool value)
		{
			AttachablePropertyServices.SetProperty(dataTemplate, RecyclePool.PoolInstanceProperty, value);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001C574 File Offset: 0x0001A774
		private Panel EnsureOwnerIsPanelOrNull(UIElement owner)
		{
			Panel panel = null;
			if (owner != null)
			{
				panel = (owner as Panel);
				if (panel == null)
				{
					throw new ArgumentException("owner must to be a Panel or null.");
				}
			}
			return panel;
		}

		// Token: 0x0400038A RID: 906
		internal static readonly DependencyProperty ReuseKeyProperty = DependencyProperty.RegisterAttached("ReuseKey", typeof(string), typeof(RecyclePool), new PropertyMetadata(string.Empty));

		// Token: 0x0400038B RID: 907
		private static readonly AttachableMemberIdentifier PoolInstanceProperty = new AttachableMemberIdentifier(typeof(RecyclePool), "PoolInstance");

		// Token: 0x0400038C RID: 908
		internal static readonly DependencyProperty OriginTemplateProperty = DependencyProperty.RegisterAttached("OriginTemplate", typeof(DataTemplate), typeof(RecyclePool), null);

		// Token: 0x0400038D RID: 909
		private readonly Dictionary<string, List<RecyclePool.ElementInfo>> m_elements = new Dictionary<string, List<RecyclePool.ElementInfo>>();

		// Token: 0x0200011A RID: 282
		private class ElementInfo
		{
			// Token: 0x06000CDC RID: 3292 RVA: 0x0002B62E File Offset: 0x0002982E
			public ElementInfo(UIElement element, Panel owner)
			{
				this.Element = element;
				this.Owner = owner;
			}

			// Token: 0x17000244 RID: 580
			// (get) Token: 0x06000CDD RID: 3293 RVA: 0x0002B644 File Offset: 0x00029844
			public UIElement Element { get; }

			// Token: 0x17000245 RID: 581
			// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0002B64C File Offset: 0x0002984C
			public Panel Owner { get; }
		}
	}
}
