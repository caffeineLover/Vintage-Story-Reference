using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ModernWpf.Media.Animation
{
	// Token: 0x02000244 RID: 580
	public sealed class EntranceNavigationTransitionInfo : NavigationTransitionInfo
	{
		// Token: 0x060010F3 RID: 4339 RVA: 0x00031C9C File Offset: 0x0002FE9C
		internal override NavigationAnimation GetEnterAnimation(FrameworkElement element, bool movingBackwards)
		{
			Storyboard storyboard = new Storyboard();
			if (movingBackwards)
			{
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames.KeyFrames.Add(new SplineDoubleKeyFrame(1.0, NavigationTransitionInfo.EnterDuration, NavigationTransitionInfo.DecelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames2 = doubleAnimationUsingKeyFrames;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2, NavigationTransitionInfo.OpacityPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames2);
			}
			else
			{
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames3.KeyFrames.Add(new DiscreteDoubleKeyFrame(200.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames3.KeyFrames.Add(new SplineDoubleKeyFrame(0.0, NavigationTransitionInfo.EnterDuration, NavigationTransitionInfo.DecelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames4 = doubleAnimationUsingKeyFrames3;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames4, NavigationTransitionInfo.TranslateYPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames4);
				DoubleAnimation doubleAnimation = new DoubleAnimation(1.0, TimeSpan.Zero);
				Storyboard.SetTargetProperty(doubleAnimation, NavigationTransitionInfo.OpacityPath);
				storyboard.Children.Add(doubleAnimation);
				element.SetCurrentValue(UIElement.RenderTransformProperty, new TranslateTransform());
			}
			return new NavigationAnimation(element, storyboard);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00031DD4 File Offset: 0x0002FFD4
		internal override NavigationAnimation GetExitAnimation(FrameworkElement element, bool movingBackwards)
		{
			Storyboard storyboard = new Storyboard();
			if (movingBackwards)
			{
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames.KeyFrames.Add(new SplineDoubleKeyFrame(200.0, NavigationTransitionInfo.ExitDuration, NavigationTransitionInfo.AccelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames2 = doubleAnimationUsingKeyFrames;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2, NavigationTransitionInfo.TranslateYPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames2);
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames3.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames3.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, NavigationTransitionInfo.ExitDuration));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames4 = doubleAnimationUsingKeyFrames3;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames4, NavigationTransitionInfo.OpacityPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames4);
				element.SetCurrentValue(UIElement.RenderTransformProperty, new TranslateTransform());
			}
			else
			{
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames5 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames5.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames5.KeyFrames.Add(new SplineDoubleKeyFrame(0.0, NavigationTransitionInfo.ExitDuration, NavigationTransitionInfo.AccelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames6 = doubleAnimationUsingKeyFrames5;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames6, NavigationTransitionInfo.OpacityPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames6);
			}
			return new NavigationAnimation(element, storyboard);
		}
	}
}
