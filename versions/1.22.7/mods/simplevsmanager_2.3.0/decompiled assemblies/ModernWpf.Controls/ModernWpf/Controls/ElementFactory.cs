using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200009E RID: 158
	public class ElementFactory : DependencyObject, IElementFactoryShim
	{
		// Token: 0x060007EA RID: 2026 RVA: 0x0001C1EB File Offset: 0x0001A3EB
		public UIElement GetElement(ElementFactoryGetArgs args)
		{
			return this.GetElementCore(args);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001C1F4 File Offset: 0x0001A3F4
		public void RecycleElement(ElementFactoryRecycleArgs args)
		{
			this.RecycleElementCore(args);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001C1FD File Offset: 0x0001A3FD
		protected virtual UIElement GetElementCore(ElementFactoryGetArgs args)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0001C204 File Offset: 0x0001A404
		protected virtual void RecycleElementCore(ElementFactoryRecycleArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
