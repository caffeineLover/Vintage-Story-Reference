using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ModernWpf.Media.Animation
{
	// Token: 0x0200024B RID: 587
	public sealed class SlideNavigationTransitionInfo : NavigationTransitionInfo, ISlideNavigationTransitionInfo2
	{
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x00032443 File Offset: 0x00030643
		// (set) Token: 0x06001112 RID: 4370 RVA: 0x00032455 File Offset: 0x00030655
		public SlideNavigationTransitionEffect Effect
		{
			get
			{
				return (SlideNavigationTransitionEffect)base.GetValue(SlideNavigationTransitionInfo.EffectProperty);
			}
			set
			{
				base.SetValue(SlideNavigationTransitionInfo.EffectProperty, value);
			}
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00032468 File Offset: 0x00030668
		internal override NavigationAnimation GetEnterAnimation(FrameworkElement element, bool movingBackwards)
		{
			Storyboard storyboard = new Storyboard();
			SlideNavigationTransitionEffect effect = this.Effect;
			if (effect == SlideNavigationTransitionEffect.FromBottom)
			{
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
			}
			else
			{
				bool flag;
				if (effect == SlideNavigationTransitionEffect.FromLeft)
				{
					flag = !movingBackwards;
				}
				else
				{
					flag = movingBackwards;
				}
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames5 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames5.KeyFrames.Add(new DiscreteDoubleKeyFrame((double)(flag ? -200 : 200), TimeSpan.Zero));
				doubleAnimationUsingKeyFrames5.KeyFrames.Add(new SplineDoubleKeyFrame(0.0, NavigationTransitionInfo.EnterDuration, NavigationTransitionInfo.DecelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames6 = doubleAnimationUsingKeyFrames5;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames6, NavigationTransitionInfo.TranslateXPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames6);
				DoubleAnimation doubleAnimation2 = new DoubleAnimation(1.0, TimeSpan.Zero);
				Storyboard.SetTargetProperty(doubleAnimation2, NavigationTransitionInfo.OpacityPath);
				storyboard.Children.Add(doubleAnimation2);
				element.SetCurrentValue(UIElement.RenderTransformProperty, new TranslateTransform());
			}
			return new NavigationAnimation(element, storyboard);
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0003267C File Offset: 0x0003087C
		internal override NavigationAnimation GetExitAnimation(FrameworkElement element, bool movingBackwards)
		{
			Storyboard storyboard = new Storyboard();
			SlideNavigationTransitionEffect effect = this.Effect;
			if (effect == SlideNavigationTransitionEffect.FromBottom)
			{
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
			}
			else
			{
				bool flag;
				if (effect == SlideNavigationTransitionEffect.FromLeft)
				{
					flag = movingBackwards;
				}
				else
				{
					flag = !movingBackwards;
				}
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames7 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames7.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames7.KeyFrames.Add(new SplineDoubleKeyFrame((double)(flag ? -200 : 200), NavigationTransitionInfo.ExitDuration, NavigationTransitionInfo.AccelerateKeySpline));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames8 = doubleAnimationUsingKeyFrames7;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames8, NavigationTransitionInfo.TranslateXPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames8);
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames9 = new DoubleAnimationUsingKeyFrames();
				doubleAnimationUsingKeyFrames9.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.0, TimeSpan.Zero));
				doubleAnimationUsingKeyFrames9.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, NavigationTransitionInfo.ExitDuration));
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames10 = doubleAnimationUsingKeyFrames9;
				Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames10, NavigationTransitionInfo.OpacityPath);
				storyboard.Children.Add(doubleAnimationUsingKeyFrames10);
				element.SetCurrentValue(UIElement.RenderTransformProperty, new TranslateTransform());
			}
			return new NavigationAnimation(element, storyboard);
		}

		// Token: 0x040004AD RID: 1197
		public static readonly DependencyProperty EffectProperty = DependencyProperty.Register("Effect", typeof(SlideNavigationTransitionEffect), typeof(SlideNavigationTransitionInfo), null);
	}
}
