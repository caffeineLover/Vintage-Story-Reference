using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ModernWpf.Controls
{
	// Token: 0x020000A1 RID: 161
	internal class ItemTemplateWrapper : IElementFactoryShim
	{
		// Token: 0x060007FA RID: 2042 RVA: 0x0001C270 File Offset: 0x0001A470
		public ItemTemplateWrapper(DataTemplate dataTemplate)
		{
			this.Template = dataTemplate;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0001C27F File Offset: 0x0001A47F
		public ItemTemplateWrapper(DataTemplateSelector dataTemplateSelector)
		{
			this.TemplateSelector = dataTemplateSelector;
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x0001C28E File Offset: 0x0001A48E
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x0001C296 File Offset: 0x0001A496
		public DataTemplate Template { get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x0001C29F File Offset: 0x0001A49F
		// (set) Token: 0x060007FF RID: 2047 RVA: 0x0001C2A7 File Offset: 0x0001A4A7
		public DataTemplateSelector TemplateSelector { get; set; }

		// Token: 0x06000800 RID: 2048 RVA: 0x0001C2B0 File Offset: 0x0001A4B0
		public UIElement GetElement(ElementFactoryGetArgs args)
		{
			DataTemplate dataTemplate = this.Template ?? this.TemplateSelector.SelectTemplate(args.Data, null);
			if (dataTemplate == null)
			{
				dataTemplate = this.TemplateSelector.SelectTemplate(args.Data, null);
				if (dataTemplate == null)
				{
					throw new InvalidOperationException("Null encountered as data template. That is not a valid value for a data template, and can not be used.");
				}
			}
			RecyclePool poolInstance = RecyclePool.GetPoolInstance(dataTemplate);
			UIElement uielement = null;
			if (poolInstance != null)
			{
				uielement = poolInstance.TryGetElement(string.Empty, args.Parent as FrameworkElement);
			}
			if (uielement == null)
			{
				uielement = (dataTemplate.LoadContent() as FrameworkElement);
				if (uielement == null)
				{
					uielement = new Rectangle
					{
						Width = 0.0,
						Height = 0.0
					};
				}
				uielement.SetValue(RecyclePool.OriginTemplateProperty, dataTemplate);
			}
			return uielement;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001C364 File Offset: 0x0001A564
		public void RecycleElement(ElementFactoryRecycleArgs args)
		{
			UIElement element = args.Element;
			DataTemplate dataTemplate = this.Template ?? (element.GetValue(RecyclePool.OriginTemplateProperty) as DataTemplate);
			RecyclePool recyclePool = RecyclePool.GetPoolInstance(dataTemplate);
			if (recyclePool == null)
			{
				recyclePool = new RecyclePool();
				RecyclePool.SetPoolInstance(dataTemplate, recyclePool);
			}
			recyclePool.PutElement(args.Element, string.Empty, args.Parent);
		}
	}
}
