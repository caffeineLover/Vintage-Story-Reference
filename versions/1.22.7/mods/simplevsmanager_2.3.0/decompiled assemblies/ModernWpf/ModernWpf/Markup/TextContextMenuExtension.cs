using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Markup;
using ModernWpf.Controls;

namespace ModernWpf.Markup
{
	// Token: 0x02000255 RID: 597
	[EditorBrowsable(1)]
	[MarkupExtensionReturnType(typeof(ContextMenu))]
	public class TextContextMenuExtension : MarkupExtension
	{
		// Token: 0x0600112A RID: 4394 RVA: 0x00032AFB File Offset: 0x00030CFB
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return TextContextMenuExtension.DefaultContextMenu.Value;
		}

		// Token: 0x040004AF RID: 1199
		private static readonly ThreadLocal<TextContextMenu> DefaultContextMenu = new ThreadLocal<TextContextMenu>(() => new TextContextMenu());
	}
}
