using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002A1 RID: 673
	public static class ScrollBarThumbHelper
	{
		// Token: 0x06001434 RID: 5172 RVA: 0x0003C07B File Offset: 0x0003A27B
		public static bool GetIsExpanded(Thumb thumb)
		{
			return (bool)thumb.GetValue(ScrollBarThumbHelper.IsExpandedProperty);
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0003C08D File Offset: 0x0003A28D
		public static void SetIsExpanded(Thumb thumb, bool value)
		{
			thumb.SetValue(ScrollBarThumbHelper.IsExpandedProperty, value);
		}

		// Token: 0x040006C3 RID: 1731
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.RegisterAttached("IsExpanded", typeof(bool), typeof(ScrollBarThumbHelper), new PropertyMetadata(false));
	}
}
