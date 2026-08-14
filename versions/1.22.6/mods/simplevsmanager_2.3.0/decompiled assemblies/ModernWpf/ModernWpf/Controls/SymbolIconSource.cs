using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200027A RID: 634
	public class SymbolIconSource : IconSource
	{
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x00034FF4 File Offset: 0x000331F4
		// (set) Token: 0x06001232 RID: 4658 RVA: 0x00035006 File Offset: 0x00033206
		public Symbol Symbol
		{
			get
			{
				return (Symbol)base.GetValue(SymbolIconSource.SymbolProperty);
			}
			set
			{
				base.SetValue(SymbolIconSource.SymbolProperty, value);
			}
		}

		// Token: 0x040005D6 RID: 1494
		public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register("Symbol", typeof(Symbol), typeof(SymbolIconSource), new PropertyMetadata(Symbol.Emoji));
	}
}
