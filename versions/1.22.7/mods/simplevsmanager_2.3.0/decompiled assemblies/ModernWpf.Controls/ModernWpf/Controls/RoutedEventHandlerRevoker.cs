using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000030 RID: 48
	internal class RoutedEventHandlerRevoker
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x000067CF File Offset: 0x000049CF
		public RoutedEventHandlerRevoker(UIElement source, RoutedEvent routedEvent, Delegate handler)
		{
			this.m_source = new WeakReference<UIElement>(source);
			this.m_event = routedEvent;
			this.m_handler = new WeakReference<Delegate>(handler);
			source.AddHandler(routedEvent, handler);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00006800 File Offset: 0x00004A00
		public void Revoke()
		{
			UIElement uielement;
			Delegate @delegate;
			if (this.m_source.TryGetTarget(ref uielement) && this.m_handler.TryGetTarget(ref @delegate))
			{
				uielement.RemoveHandler(this.m_event, @delegate);
			}
		}

		// Token: 0x0400008E RID: 142
		private readonly WeakReference<UIElement> m_source;

		// Token: 0x0400008F RID: 143
		private readonly RoutedEvent m_event;

		// Token: 0x04000090 RID: 144
		private readonly WeakReference<Delegate> m_handler;
	}
}
