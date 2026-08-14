using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000295 RID: 661
	public static class OpeningAnimationHelper
	{
		// Token: 0x06001398 RID: 5016 RVA: 0x00039BA1 File Offset: 0x00037DA1
		public static Storyboard GetStoryboard(FrameworkElement element)
		{
			return (Storyboard)element.GetValue(OpeningAnimationHelper.StoryboardProperty);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00039BB3 File Offset: 0x00037DB3
		public static void SetStoryboard(FrameworkElement element, Storyboard value)
		{
			element.SetValue(OpeningAnimationHelper.StoryboardProperty, value);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00039BC4 File Offset: 0x00037DC4
		private static void OnStoryboardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if (e.OldValue != null)
			{
				frameworkElement.Loaded -= new RoutedEventHandler(OpeningAnimationHelper.OnElementLoaded);
			}
			if (e.NewValue != null)
			{
				frameworkElement.Loaded += new RoutedEventHandler(OpeningAnimationHelper.OnElementLoaded);
			}
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00039C10 File Offset: 0x00037E10
		private static void OnElementLoaded(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)sender;
			if (frameworkElement.IsVisible && Helper.IsAnimationsEnabled && !DesignMode.DesignModeEnabled)
			{
				Storyboard storyboard = OpeningAnimationHelper.GetStoryboard(frameworkElement);
				if (storyboard != null)
				{
					storyboard.Begin();
				}
			}
		}

		// Token: 0x04000691 RID: 1681
		public static readonly DependencyProperty StoryboardProperty = DependencyProperty.RegisterAttached("Storyboard", typeof(Storyboard), typeof(OpeningAnimationHelper), new PropertyMetadata(new PropertyChangedCallback(OpeningAnimationHelper.OnStoryboardChanged)));
	}
}
