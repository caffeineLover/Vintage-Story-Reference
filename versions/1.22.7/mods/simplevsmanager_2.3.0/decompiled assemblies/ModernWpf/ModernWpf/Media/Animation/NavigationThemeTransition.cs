using System;
using System.Windows;
using System.Windows.Markup;

namespace ModernWpf.Media.Animation
{
	// Token: 0x02000248 RID: 584
	[ContentProperty("DefaultNavigationTransitionInfo")]
	public sealed class NavigationThemeTransition : Transition
	{
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x000322D1 File Offset: 0x000304D1
		// (set) Token: 0x0600110A RID: 4362 RVA: 0x000322E3 File Offset: 0x000304E3
		public NavigationTransitionInfo DefaultNavigationTransitionInfo
		{
			get
			{
				return (NavigationTransitionInfo)base.GetValue(NavigationThemeTransition.DefaultNavigationTransitionInfoProperty);
			}
			set
			{
				base.SetValue(NavigationThemeTransition.DefaultNavigationTransitionInfoProperty, value);
			}
		}

		// Token: 0x0400049E RID: 1182
		public static readonly DependencyProperty DefaultNavigationTransitionInfoProperty = DependencyProperty.Register("DefaultNavigationTransitionInfo", typeof(NavigationTransitionInfo), typeof(NavigationThemeTransition), null);
	}
}
