using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x0200006A RID: 106
	internal class NavigationViewItemsFactory : ElementFactory
	{
		// Token: 0x06000523 RID: 1315 RVA: 0x000132A8 File Offset: 0x000114A8
		public void UserElementFactory(object newValue)
		{
			this.m_itemTemplateWrapper = (newValue as IElementFactoryShim);
			if (this.m_itemTemplateWrapper == null)
			{
				DataTemplate dataTemplate = newValue as DataTemplate;
				if (dataTemplate != null)
				{
					this.m_itemTemplateWrapper = new ItemTemplateWrapper(dataTemplate);
				}
				else
				{
					DataTemplateSelector dataTemplateSelector = newValue as DataTemplateSelector;
					if (dataTemplateSelector != null)
					{
						this.m_itemTemplateWrapper = new ItemTemplateWrapper(dataTemplateSelector);
					}
				}
			}
			this.navigationViewItemPool = new List<NavigationViewItem>();
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00013302 File Offset: 0x00011502
		internal void SettingsItem(NavigationViewItemBase settingsItem)
		{
			this.m_settingsItem = settingsItem;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001330C File Offset: 0x0001150C
		protected override UIElement GetElementCore(ElementFactoryGetArgs args)
		{
			NavigationViewItemsFactory.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.args = args;
			object obj = this.<GetElementCore>g__init|2_0(ref CS$<>8__locals1);
			NavigationViewItemBase navigationViewItemBase = obj as NavigationViewItemBase;
			if (navigationViewItemBase != null)
			{
				return navigationViewItemBase;
			}
			NavigationViewItem navigationViewItem = this.<GetElementCore>g__init|2_1(ref CS$<>8__locals1);
			navigationViewItem.CreatedByNavigationViewItemsFactory = true;
			if (this.m_itemTemplateWrapper != null)
			{
				ItemTemplateWrapper itemTemplateWrapper = this.m_itemTemplateWrapper as ItemTemplateWrapper;
				if (itemTemplateWrapper != null)
				{
					ElementFactoryRecycleArgs elementFactoryRecycleArgs = new ElementFactoryRecycleArgs();
					elementFactoryRecycleArgs.Element = (obj as UIElement);
					this.m_itemTemplateWrapper.RecycleElement(elementFactoryRecycleArgs);
					navigationViewItem.Content = CS$<>8__locals1.args.Data;
					navigationViewItem.ContentTemplate = itemTemplateWrapper.Template;
					navigationViewItem.ContentTemplateSelector = itemTemplateWrapper.TemplateSelector;
					return navigationViewItem;
				}
			}
			navigationViewItem.Content = obj;
			return navigationViewItem;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000133C0 File Offset: 0x000115C0
		protected override void RecycleElementCore(ElementFactoryRecycleArgs args)
		{
			UIElement element = args.Element;
			if (element != null)
			{
				NavigationViewItem navigationViewItem = element as NavigationViewItem;
				if (navigationViewItem != null)
				{
					NavigationViewItem navigationViewItem2 = navigationViewItem;
					if (navigationViewItem2.CreatedByNavigationViewItemsFactory)
					{
						navigationViewItem2.CreatedByNavigationViewItemsFactory = false;
						this.UnlinkElementFromParent(args);
						args.Element = null;
						this.navigationViewItemPool.Add(navigationViewItem);
						IElementFactoryShim itemTemplateWrapper = this.m_itemTemplateWrapper;
					}
				}
				bool flag = this.m_settingsItem != null && this.m_settingsItem == args.Element;
				if (this.m_itemTemplateWrapper != null && !flag)
				{
					this.m_itemTemplateWrapper.RecycleElement(args);
					return;
				}
				this.UnlinkElementFromParent(args);
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001344C File Offset: 0x0001164C
		private void UnlinkElementFromParent(ElementFactoryRecycleArgs args)
		{
			Panel panel = args.Parent as Panel;
			if (panel != null)
			{
				UIElementCollection children = panel.Children;
				int num = 0;
				if (children.IndexOf(args.Element, out num))
				{
					children.RemoveAt(num);
				}
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001349C File Offset: 0x0001169C
		[CompilerGenerated]
		private object <GetElementCore>g__init|2_0(ref NavigationViewItemsFactory.<>c__DisplayClass2_0 A_1)
		{
			if (this.m_settingsItem != null && this.m_settingsItem == A_1.args.Data)
			{
				return A_1.args.Data;
			}
			if (this.m_itemTemplateWrapper != null)
			{
				return this.m_itemTemplateWrapper.GetElement(A_1.args);
			}
			return A_1.args.Data;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000134F5 File Offset: 0x000116F5
		[CompilerGenerated]
		private NavigationViewItem <GetElementCore>g__init|2_1(ref NavigationViewItemsFactory.<>c__DisplayClass2_0 A_1)
		{
			if (this.navigationViewItemPool.Count > 0)
			{
				NavigationViewItem result = this.navigationViewItemPool.Last<NavigationViewItem>();
				this.navigationViewItemPool.RemoveLast<NavigationViewItem>();
				return result;
			}
			return new NavigationViewItem();
		}

		// Token: 0x0400026A RID: 618
		private IElementFactoryShim m_itemTemplateWrapper;

		// Token: 0x0400026B RID: 619
		private NavigationViewItemBase m_settingsItem;

		// Token: 0x0400026C RID: 620
		private List<NavigationViewItem> navigationViewItemPool = new List<NavigationViewItem>();
	}
}
