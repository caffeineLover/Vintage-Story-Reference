using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ModernWpf.Media.Animation
{
	// Token: 0x02000245 RID: 581
	public sealed class FadeInThemeAnimation : DoubleAnimation
	{
		// Token: 0x060010F5 RID: 4341 RVA: 0x00031F40 File Offset: 0x00030140
		static FadeInThemeAnimation()
		{
			DoubleAnimation.ToProperty.OverrideMetadata(typeof(FadeInThemeAnimation), new FrameworkPropertyMetadata(1.0));
			Timeline.DurationProperty.OverrideMetadata(typeof(FadeInThemeAnimation), new FrameworkPropertyMetadata(FadeInThemeAnimation.DefaultDuration));
			Storyboard.TargetPropertyProperty.OverrideMetadata(typeof(FadeInThemeAnimation), new FrameworkPropertyMetadata(new PropertyPath(UIElement.OpacityProperty)));
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x00031FF3 File Offset: 0x000301F3
		// (set) Token: 0x060010F8 RID: 4344 RVA: 0x00032005 File Offset: 0x00030205
		public string TargetName
		{
			get
			{
				return (string)base.GetValue(FadeInThemeAnimation.TargetNameProperty);
			}
			set
			{
				base.SetValue(FadeInThemeAnimation.TargetNameProperty, value);
			}
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00032013 File Offset: 0x00030213
		protected override Freezable CreateInstanceCore()
		{
			return new FadeInThemeAnimation();
		}

		// Token: 0x04000495 RID: 1173
		private static readonly Duration DefaultDuration = TimeSpan.FromMilliseconds(167.0);

		// Token: 0x04000496 RID: 1174
		public static readonly DependencyProperty TargetNameProperty = Storyboard.TargetNameProperty.AddOwner(typeof(FadeInThemeAnimation));
	}
}
