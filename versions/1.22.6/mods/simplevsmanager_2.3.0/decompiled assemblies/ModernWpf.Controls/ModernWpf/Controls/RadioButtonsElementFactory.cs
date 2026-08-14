using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x02000082 RID: 130
	internal class RadioButtonsElementFactory : ElementFactory
	{
		// Token: 0x06000689 RID: 1673 RVA: 0x00017E40 File Offset: 0x00016040
		internal void UserElementFactory(object newValue)
		{
			this.m_itemTemplateWrapper = (newValue as IElementFactoryShim);
			if (this.m_itemTemplateWrapper == null)
			{
				DataTemplate dataTemplate = newValue as DataTemplate;
				if (dataTemplate != null)
				{
					this.m_itemTemplateWrapper = new ItemTemplateWrapper(dataTemplate);
				}
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00017E78 File Offset: 0x00016078
		protected override UIElement GetElementCore(ElementFactoryGetArgs args)
		{
			object obj;
			if (this.m_itemTemplateWrapper != null)
			{
				obj = this.m_itemTemplateWrapper.GetElement(args);
			}
			else
			{
				obj = args.Data;
			}
			RadioButton radioButton = obj as RadioButton;
			if (radioButton != null)
			{
				return radioButton;
			}
			RadioButton radioButton2 = new RadioButton();
			radioButton2.Content = args.Data;
			ItemTemplateWrapper itemTemplateWrapper = this.m_itemTemplateWrapper as ItemTemplateWrapper;
			if (itemTemplateWrapper != null)
			{
				radioButton2.ContentTemplate = itemTemplateWrapper.Template;
			}
			return radioButton2;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00017EDD File Offset: 0x000160DD
		protected override void RecycleElementCore(ElementFactoryRecycleArgs args)
		{
		}

		// Token: 0x04000300 RID: 768
		private IElementFactoryShim m_itemTemplateWrapper;
	}
}
