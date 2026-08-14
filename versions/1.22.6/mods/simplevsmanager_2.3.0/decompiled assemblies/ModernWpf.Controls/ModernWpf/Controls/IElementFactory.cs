using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200008D RID: 141
	public interface IElementFactory
	{
		// Token: 0x06000714 RID: 1812
		UIElement GetElement(ElementFactoryGetArgs args);

		// Token: 0x06000715 RID: 1813
		void RecycleElement(ElementFactoryRecycleArgs args);
	}
}
