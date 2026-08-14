using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ModernWpf.Media.Animation
{
	// Token: 0x02000249 RID: 585
	public class NavigationTransitionInfo : DependencyObject
	{
		// Token: 0x0600110C RID: 4364 RVA: 0x00032318 File Offset: 0x00030518
		static NavigationTransitionInfo()
		{
			NavigationTransitionInfo.AccelerateKeySpline = new KeySpline(0.7, 0.0, 1.0, 0.5);
			NavigationTransitionInfo.AccelerateKeySpline.Freeze();
			NavigationTransitionInfo.DecelerateKeySpline = new KeySpline(0.1, 0.9, 0.2, 1.0);
			NavigationTransitionInfo.DecelerateKeySpline.Freeze();
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0003242D File Offset: 0x0003062D
		protected NavigationTransitionInfo()
		{
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00032435 File Offset: 0x00030635
		internal virtual NavigationAnimation GetEnterAnimation(FrameworkElement element, bool movingBackwards)
		{
			return null;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00032438 File Offset: 0x00030638
		internal virtual NavigationAnimation GetExitAnimation(FrameworkElement element, bool movingBackwards)
		{
			return null;
		}

		// Token: 0x0400049F RID: 1183
		internal static readonly TimeSpan ExitDuration = TimeSpan.FromMilliseconds(150.0);

		// Token: 0x040004A0 RID: 1184
		internal static readonly TimeSpan EnterDuration = TimeSpan.FromMilliseconds(300.0);

		// Token: 0x040004A1 RID: 1185
		internal static readonly TimeSpan MaxMoveDuration = TimeSpan.FromMilliseconds(500.0);

		// Token: 0x040004A2 RID: 1186
		internal static readonly KeySpline AccelerateKeySpline;

		// Token: 0x040004A3 RID: 1187
		internal static readonly KeySpline DecelerateKeySpline;

		// Token: 0x040004A4 RID: 1188
		internal static readonly PropertyPath OpacityPath = new PropertyPath(UIElement.OpacityProperty);

		// Token: 0x040004A5 RID: 1189
		internal static readonly PropertyPath TranslateXPath = new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)", Array.Empty<object>());

		// Token: 0x040004A6 RID: 1190
		internal static readonly PropertyPath TranslateYPath = new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)", Array.Empty<object>());

		// Token: 0x040004A7 RID: 1191
		internal static readonly PropertyPath ScaleXPath = new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)", Array.Empty<object>());

		// Token: 0x040004A8 RID: 1192
		internal static readonly PropertyPath ScaleYPath = new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)", Array.Empty<object>());
	}
}
