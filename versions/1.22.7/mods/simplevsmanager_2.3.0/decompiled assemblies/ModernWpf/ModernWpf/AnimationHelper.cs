using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Animation;

namespace ModernWpf
{
	// Token: 0x02000210 RID: 528
	internal static class AnimationHelper
	{
		// Token: 0x06000F82 RID: 3970 RVA: 0x0002C29A File Offset: 0x0002A49A
		public static void DeferBegin(Storyboard storyboard)
		{
			storyboard.CurrentStateInvalidated += new EventHandler(AnimationHelper.<DeferBegin>g__OnStoryboardCurrentStateInvalidated|0_0);
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0002C2B0 File Offset: 0x0002A4B0
		public static void DeferTransitions(VisualStateGroup group)
		{
			foreach (object obj in group.Transitions)
			{
				Storyboard storyboard = ((VisualTransition)obj).Storyboard;
				if (storyboard != null)
				{
					AnimationHelper.DeferBegin(storyboard);
				}
			}
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0002C310 File Offset: 0x0002A510
		[CompilerGenerated]
		internal static void <DeferBegin>g__OnStoryboardCurrentStateInvalidated|0_0(object sender, EventArgs e)
		{
			Clock clock = sender as Clock;
			if (clock != null && clock.HasControllableRoot && clock.CurrentState == null && !clock.IsPaused)
			{
				clock.Controller.Pause();
				clock.Dispatcher.BeginInvoke(delegate()
				{
					if (clock.IsPaused)
					{
						clock.Controller.Resume();
					}
				}, 6, Array.Empty<object>());
			}
		}
	}
}
