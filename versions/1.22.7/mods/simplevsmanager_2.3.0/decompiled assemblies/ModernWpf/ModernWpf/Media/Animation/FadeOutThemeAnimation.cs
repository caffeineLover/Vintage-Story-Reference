using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ModernWpf.Media.Animation
{
	// Token: 0x02000246 RID: 582
	public sealed class FadeOutThemeAnimation : DoubleAnimation
	{
		// Token: 0x060010FA RID: 4346 RVA: 0x0003201C File Offset: 0x0003021C
		static FadeOutThemeAnimation()
		{
			DoubleAnimation.ToProperty.OverrideMetadata(typeof(FadeOutThemeAnimation), new FrameworkPropertyMetadata(0.0));
			Timeline.DurationProperty.OverrideMetadata(typeof(FadeOutThemeAnimation), new FrameworkPropertyMetadata(FadeOutThemeAnimation.DefaultDuration));
			Storyboard.TargetPropertyProperty.OverrideMetadata(typeof(FadeOutThemeAnimation), new FrameworkPropertyMetadata(new PropertyPath(UIElement.OpacityProperty)));
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x000320CF File Offset: 0x000302CF
		// (set) Token: 0x060010FD RID: 4349 RVA: 0x000320E1 File Offset: 0x000302E1
		public string TargetName
		{
			get
			{
				return (string)base.GetValue(FadeOutThemeAnimation.TargetNameProperty);
			}
			set
			{
				base.SetValue(FadeOutThemeAnimation.TargetNameProperty, value);
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x000320EF File Offset: 0x000302EF
		protected override Freezable CreateInstanceCore()
		{
			return new FadeOutThemeAnimation();
		}

		// Token: 0x04000497 RID: 1175
		private static readonly Duration DefaultDuration = TimeSpan.FromMilliseconds(167.0);

		// Token: 0x04000498 RID: 1176
		public static readonly DependencyProperty TargetNameProperty = Storyboard.TargetNameProperty.AddOwner(typeof(FadeOutThemeAnimation));
	}
}
