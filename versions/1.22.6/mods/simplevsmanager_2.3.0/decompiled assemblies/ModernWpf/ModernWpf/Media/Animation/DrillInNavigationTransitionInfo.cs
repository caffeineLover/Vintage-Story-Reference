using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ModernWpf.Media.Animation
{
	// Token: 0x02000243 RID: 579
	public sealed class DrillInNavigationTransitionInfo : NavigationTransitionInfo
	{
		// Token: 0x060010F0 RID: 4336 RVA: 0x0003192C File Offset: 0x0002FB2C
		internal override NavigationAnimation GetEnterAnimation(FrameworkElement element, bool movingBackwards)
		{
			Storyboard storyboard = new Storyboard();
			if (movingBackwards)
			{
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.15, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames.KeyFrames.Add(new SplineDoubleKeyFrame(1.0, NavigationTransitionInfo.EnterDuration, NavigationTransitionInfo.DecelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames2 = doubleAnimationUsingKeyFrames;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2, NavigationTransitionInfo.ScaleXPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames2);
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames3.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.15, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames3.KeyFrames.Add(new SplineDoubleKeyFrame(1.0, NavigationTransitionInfo.EnterDuration, NavigationTransitionInfo.DecelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames4 = doubleAnimationUsingKeyFrames3;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames4, NavigationTransitionInfo.ScaleYPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames4);
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames5 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames5.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames5.KeyFrames.Add(new SplineDoubleKeyFrame(1.0, NavigationTransitionInfo.EnterDuration, NavigationTransitionInfo.DecelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames6 = doubleAnimationUsingKeyFrames5;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames6, NavigationTransitionInfo.OpacityPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames6);
			}
			else
			{
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames7 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames7.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.9, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames7.KeyFrames.Add(new SplineDoubleKeyFrame(1.0, NavigationTransitionInfo.MaxMoveDuration, NavigationTransitionInfo.DecelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames8 = doubleAnimationUsingKeyFrames7;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames8, NavigationTransitionInfo.ScaleXPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames8);
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames9 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames9.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.9, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames9.KeyFrames.Add(new SplineDoubleKeyFrame(1.0, NavigationTransitionInfo.MaxMoveDuration, NavigationTransitionInfo.DecelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames10 = doubleAnimationUsingKeyFrames9;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames10, NavigationTransitionInfo.ScaleYPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames10);
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames11 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames11.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames11.KeyFrames.Add(new SplineDoubleKeyFrame(1.0, NavigationTransitionInfo.MaxMoveDuration, NavigationTransitionInfo.DecelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames12 = doubleAnimationUsingKeyFrames11;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames12, NavigationTransitionInfo.OpacityPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames12);
			}
			element.SetCurrentValue(UIElement.RenderTransformProperty, new ScaleTransform());
			element.SetCurrentValue(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5));
			return new NavigationAnimation(element, storyboard);
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00031C10 File Offset: 0x0002FE10
		internal override NavigationAnimation GetExitAnimation(FrameworkElement element, bool movingBackwards)
		{
			Storyboard storyboard = new Storyboard();
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
			doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.0, TimeSpan.Zero));
			doubleAnimationUsingKeyFrames.KeyFrames.Add(new SplineDoubleKeyFrame(0.0, NavigationTransitionInfo.ExitDuration, NavigationTransitionInfo.AccelerateKeySpline));
			DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames2 = doubleAnimationUsingKeyFrames;
			Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2, NavigationTransitionInfo.OpacityPath);
			storyboard.Children.Add(doubleAnimationUsingKeyFrames2);
			return new NavigationAnimation(element, storyboard);
		}
	}
}
