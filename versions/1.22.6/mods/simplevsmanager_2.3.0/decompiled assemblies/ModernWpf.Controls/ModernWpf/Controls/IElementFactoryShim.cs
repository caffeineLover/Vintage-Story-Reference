using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200008E RID: 142
	public interface IElementFactoryShim
	{
		// Token: 0x06000716 RID: 1814
		UIElement GetElement(ElementFactoryGetArgs args);

		// Token: 0x06000717 RID: 1815
		void RecycleElement(ElementFactoryRecycleArgs context);
	}
}
