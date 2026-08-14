using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000A3 RID: 163
	public class RecyclingElementFactory : ElementFactory
	{
		// Token: 0x0600080F RID: 2063 RVA: 0x0001C627 File Offset: 0x0001A827
		public RecyclingElementFactory()
		{
			this.m_templates = new Dictionary<string, DataTemplate>();
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0001C63A File Offset: 0x0001A83A
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x0001C642 File Offset: 0x0001A842
		public RecyclePool RecyclePool { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0001C64B File Offset: 0x0001A84B
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x0001C653 File Offset: 0x0001A853
		public Dictionary<string, DataTemplate> Templates
		{
			get
			{
				return this.m_templates;
			}
			set
			{
				this.m_templates = value;
			}
		}

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06000814 RID: 2068 RVA: 0x0001C65C File Offset: 0x0001A85C
		// (remove) Token: 0x06000815 RID: 2069 RVA: 0x0001C694 File Offset: 0x0001A894
		public event TypedEventHandler<RecyclingElementFactory, SelectTemplateEventArgs> SelectTemplateKey;

		// Token: 0x06000816 RID: 2070 RVA: 0x0001C6CC File Offset: 0x0001A8CC
		protected virtual string OnSelectTemplateKeyCore(object dataContext, UIElement owner)
		{
			if (this.m_args == null)
			{
				this.m_args = new SelectTemplateEventArgs();
			}
			SelectTemplateEventArgs args = this.m_args;
			args.TemplateKey = string.Empty;
			args.DataContext = dataContext;
			args.Owner = owner;
			TypedEventHandler<RecyclingElementFactory, SelectTemplateEventArgs> selectTemplateKey = this.SelectTemplateKey;
			if (selectTemplateKey != null)
			{
				selectTemplateKey.Invoke(this, args);
			}
			string templateKey = args.TemplateKey;
			if (string.IsNullOrEmpty(templateKey))
			{
				throw new InvalidOperationException("Please provide a valid template identifier in the handler for the SelectTemplateKey event.");
			}
			return templateKey;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001C738 File Offset: 0x0001A938
		protected override UIElement GetElementCore(ElementFactoryGetArgs args)
		{
			if (this.m_templates == null || this.m_templates.Count == 0)
			{
				throw new InvalidOperationException("Templates property cannot be null or empty.");
			}
			UIElement parent = args.Parent;
			string text = (this.m_templates.Count == 1) ? Enumerable.First<KeyValuePair<string, DataTemplate>>(this.m_templates).Key : this.OnSelectTemplateKeyCore(args.Data, parent);
			if (string.IsNullOrEmpty(text))
			{
				throw new InvalidOperationException("Template key cannot be empty or null.");
			}
			FrameworkElement frameworkElement = this.RecyclePool.TryGetElement(text, parent) as FrameworkElement;
			if (frameworkElement == null)
			{
				if (this.m_templates.Count > 1 && !this.m_templates.ContainsKey(text))
				{
					throw new InvalidOperationException("No templates of key " + text + " were found in the templates collection.");
				}
				frameworkElement = (this.m_templates[text].LoadContent() as FrameworkElement);
				RecyclePool.SetReuseKey(frameworkElement, text);
			}
			return frameworkElement;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001C818 File Offset: 0x0001AA18
		protected override void RecycleElementCore(ElementFactoryRecycleArgs args)
		{
			UIElement element = args.Element;
			string reuseKey = RecyclePool.GetReuseKey(element);
			this.RecyclePool.PutElement(element, reuseKey, args.Parent);
		}

		// Token: 0x04000390 RID: 912
		private Dictionary<string, DataTemplate> m_templates;

		// Token: 0x04000391 RID: 913
		private SelectTemplateEventArgs m_args;
	}
}
